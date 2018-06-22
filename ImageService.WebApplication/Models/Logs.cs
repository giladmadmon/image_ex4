using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ImageService.Communication.Model;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Communication.Singleton;
using System.Threading;
using ImageService.Logging.Modal;

namespace ImageService.WebApplication.Models {
    public class Logs {
        private bool finishedGettingLogs;
        private string m_typeFilter;
        /// <summary>
        /// Initializes a new instance of the <see cref="Logs"/> class.
        /// </summary>
        /// <param name="typeFilter">The type filter.</param>
        public Logs(string typeFilter) {
            m_typeFilter = typeFilter != null && typeFilter != "" ? typeFilter.ToUpper() : null;
            LogMessages = new LogMessageRecords();

            if(ClientCommunication.Instance.Connected) {
                finishedGettingLogs = false;
                ClientCommunication.Instance.OnDataRecieved += AddLogs;
                ClientCommunication.Instance.Send(new CommandMessage(CommandEnum.LogCommand, new string[] { }));
                SpinWait.SpinUntil(() => finishedGettingLogs);
            }
        }

        [Required]
        [Display(Name = "Log Messages")]
        public LogMessageRecords LogMessages { get; set; }

        /// <summary>
        /// Adds the logs to the logs list.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Communication.Model.Event.DataReceivedEventArgs" /> instance containing the event data.</param>
        private void AddLogs(object sender, Communication.Model.Event.DataReceivedEventArgs e) {
            try {
                CommandMessage cmdMsg = CommandMessage.FromJSON(e.Data);

                if(cmdMsg.CmdId == CommandEnum.LogCommand) {
                    foreach(string logMsg in cmdMsg.Args) {
                        LogMessageRecord msgRcrd = LogMessageRecord.FromJSON(logMsg);
                        if(m_typeFilter == null || msgRcrd.Type.ToString().Equals(m_typeFilter))
                            this.LogMessages.Add(new LogMessageRecord(msgRcrd.Message, msgRcrd.Type));
                    }

                    ClientCommunication.Instance.OnDataRecieved -= AddLogs;
                    finishedGettingLogs = true;
                }
            } catch { }
        }
    }
    public class LogRecord {

        [Required]
        [Display(Name = "Message")]
        public string Message { get; set; }
        [Required]
        [Display(Name = "Type")]
        public MessageTypeEnum Type { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogRecord"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="type">The type.</param>
        public LogRecord(string message, MessageTypeEnum type) {
            this.Message = message;
            this.Type = type;
        }
    }
}