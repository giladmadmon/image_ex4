using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Configuration;
using ImageService.Server;
using ImageService.Controller;
using ImageService.Modal;
using ImageService.Logging;
using System.Security.Permissions;
using ImageService.ImageService.Modal;
using ImageService.Communication.Model;
using ImageService.Communication.Interfaces;
using ImageService.Infrastructure.Enums;
using ImageService.ImageService.Server;
using System.Drawing;
using System.IO;

public enum ServiceState {
    SERVICE_STOPPED = 0x00000001,
    SERVICE_START_PENDING = 0x00000002,
    SERVICE_STOP_PENDING = 0x00000003,
    SERVICE_RUNNING = 0x00000004,
    SERVICE_CONTINUE_PENDING = 0x00000005,
    SERVICE_PAUSE_PENDING = 0x00000006,
    SERVICE_PAUSED = 0x00000007,
}

[StructLayout(LayoutKind.Sequential)]
public struct ServiceStatus {
    public int dwServiceType;
    public ServiceState dwCurrentState;
    public int dwControlsAccepted;
    public int dwWin32ExitCode;
    public int dwServiceSpecificExitCode;
    public int dwCheckPoint;
    public int dwWaitHint;
};

namespace ImageService {
    public partial class x : ServiceBase {

        private ImageServer m_imageServer;
        private IImageController m_controller;

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);
        /// <summary>
        /// Initializes a new instance of the <see cref="x"/> class.
        /// </summary>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public x() {
            InitializeComponent();

            string sourceName = AppConfig.Instance.SourceName;
            string logName = AppConfig.Instance.LogName;

            if(!EventLog.SourceExists(sourceName)) {
                EventLog.CreateEventSource(sourceName, logName);
            }

            this.EventLogger.Source = sourceName;
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Start command is sent to the service 
        /// by the Service Control Manager (SCM) or when the operating system starts 
        /// (for a service that starts automatically). Specifies actions to take when the service starts.
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        protected override void OnStart(string[] args) {
            string outptDir = AppConfig.Instance.OutputDirPath;
            int thumbnailSize = Int32.Parse(AppConfig.Instance.ThumbnailSize);
            List<string> handlers = AppConfig.Instance.Folders;

            IImageServiceModal modal = new ImageServiceModal(outptDir, thumbnailSize);

            ILoggingService logging = new LoggingService();
            logging.MessageRecieved += UpdateLogInServer;

            m_controller = new ImageController(modal, logging);
            this.m_imageServer = new ImageServer(m_controller, logging);

            logging.MessageRecieved += (sender, msgReceived) => {
                this.EventLogger.WriteEntry(msgReceived.Message, (EventLogEntryType)msgReceived.Status);
            };

            foreach(string handler in handlers) {
                this.m_imageServer.createHandler(handler);
            }

            ServerCommunication.Instance.OnDataReceived += OnServerDataRecieved;
            ServerCommunication.Instance.OnDataReceivedApp += OnServerDataRecievedApp;
            ;

            this.EventLogger.WriteEntry("Service Started");
        }

        private void OnServerDataRecievedApp(object sender, Communication.Model.Event.DataReceivedEventArgs<byte[]> e) {
            Image image = null;
            using(var ms = new MemoryStream(e.Data)) {
                image = Image.FromStream(ms);
            }


            if(AppConfig.Instance.Folders.Count > 0 && image != null) {
                image.Save(Path.Combine(AppConfig.Instance.Folders[0], e.Name), image.RawFormat);
            }
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Stop command is sent to the service by 
        /// the Service Control Manager (SCM). Specifies actions to take when a service stops running.
        /// </summary>
        protected override void OnStop() {
            m_imageServer.CloseServer();
            this.EventLogger.WriteEntry("Service Stopped");
        }

        #region ServerCommunication events
        /// <summary>
        /// Called when [server data recieved].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Communication.Model.Event.DataReceivedEventArgs"/> instance containing the event data.</param>
        private void OnServerDataRecieved(object sender, Communication.Model.Event.DataReceivedEventArgs<string> e) {
            CommandMessage cmdMsg = CommandMessage.FromJSON(e.Data);
            IClientCommunicationChannel<string> receiver = (IClientCommunicationChannel<string>)sender;
            string msg = null;

            if(cmdMsg.CmdId == CommandEnum.CloseClientCommand) {
                receiver.Close();
            } else if(cmdMsg.CmdId == CommandEnum.CloseCommand) {
                m_imageServer.sendCommand(CommandEnum.CloseCommand, new string[] { }, cmdMsg.Args[0]);
                ServerCommunication.Instance.Send(new CommandMessage(CommandEnum.CloseCommand, cmdMsg.Args).ToJSON());
            } else {
                bool result;
                msg = m_controller.ExecuteCommand((int)cmdMsg.CmdId, cmdMsg.Args, out result);
            }

            if(msg != null) {
                receiver.Send(msg);
            }
        }

        /// <summary>
        /// Updates the log in server.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Logging.Modal.MessageRecievedEventArgs"/> instance containing the event data.</param>
        private void UpdateLogInServer(object sender, Logging.Modal.MessageRecievedEventArgs e) {
            LogMessageRecord record = new LogMessageRecord(e.Message, e.Status);
            CommandMessage cmd = new CommandMessage(CommandEnum.LogCommand, new string[] { record.ToJSON() });
            ServerCommunication.Instance.Send(cmd.ToJSON());
        }


        #endregion

    }
}
