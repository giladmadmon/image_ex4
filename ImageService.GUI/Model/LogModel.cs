using ImageService.Communication.Model;
using ImageService.Communication.Singleton;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ImageService.GUI.Model {
    public class LogModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        public LogMessageRecords LogMessages { set; get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogModel"/> class.
        /// </summary>
        public LogModel() {
            LogMessages = new LogMessageRecords();
            BindingOperations.EnableCollectionSynchronization(LogMessages, new object());
            LogMessages.CollectionChanged += (sender, e) => NotifyProperyChanged("LogMessages");

            ClientCommunication.Instance.OnDataRecieved += AddLogs;
            ClientCommunication.Instance.Send(new Communication.Model.CommandMessage
                (Infrastructure.Enums.CommandEnum.LogCommand, new string[] { }));
        }

        /// <summary>
        /// Adds the logs to the logs list.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Communication.Model.Event.DataReceivedEventArgs"/> instance containing the event data.</param>
        private void AddLogs(object sender, Communication.Model.Event.DataReceivedEventArgs e) {
            try {
                CommandMessage cmdMsg = CommandMessage.FromJSON(e.Data);

                if(cmdMsg.CmdId == CommandEnum.LogCommand) {
                    foreach(string logMsg in cmdMsg.Args) {
                        this.LogMessages.Add(LogMessageRecord.FromJSON(logMsg));
                    }
                }

            } catch { }
        }

        /// <summary>
        /// Notifies the propery changed.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        private void NotifyProperyChanged(string name) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
