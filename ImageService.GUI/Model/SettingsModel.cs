using ImageService.Communication.Model;
using ImageService.Communication.Model.Event;
using ImageService.Communication.Singleton;
using ImageService.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ImageService.GUI.Model {
    class SettingsModel : ISettingsModel {
        private string m_outputDirPath;
        private string m_sourceName;
        private string m_logName;
        private string m_thumbnailSize;
        private Dictionary<CommandEnum, CommandAction> m_actions;
        private ObservableCollection<string> m_Folders;

        public event PropertyChangedEventHandler PropertyChanged;

        delegate void CommandAction(CommandMessage cmdMsg);

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsModel"/> class.
        /// </summary>
        public SettingsModel() {
            Folders = new ObservableCollection<string>();
            BindingOperations.EnableCollectionSynchronization(Folders, new object());
            Folders.CollectionChanged += (sender, e) => NotifyProperyChanged("Folders");

            OutputDirPath = "Not Connected";

            m_actions = new Dictionary<CommandEnum, CommandAction>();
            m_actions.Add(CommandEnum.GetConfigCommand, OnConfigRefresh);
            m_actions.Add(CommandEnum.CloseCommand, OnRemoveHandler);

            ClientCommunication.Instance.OnDataRecieved += (s, e) => {
                CommandMessage cmdMsg = CommandMessage.FromJSON(e.Data);

                if(m_actions.ContainsKey(cmdMsg.CmdId))
                    m_actions[cmdMsg.CmdId](cmdMsg);
            };

            ClientCommunication.Instance.Send(new CommandMessage(CommandEnum.GetConfigCommand, new string[] { }));
        }

        /// <summary>
        /// Called when a handler is removed.
        /// </summary>
        /// <param name="cmdMsg">The command MSG.</param>
        private void OnRemoveHandler(CommandMessage cmdMsg) {
            Folders.Remove(cmdMsg.Args[0]);
        }

        /// <summary>
        /// Called when the client gets the configurations of the service.
        /// </summary>
        /// <param name="cmdMsg">The command MSG.</param>
        private void OnConfigRefresh(CommandMessage cmdMsg) {
            this.SourceName = cmdMsg.Args[0];
            this.LogName = cmdMsg.Args[1];
            this.OutputDirPath = cmdMsg.Args[2];
            this.ThumbnailSize = cmdMsg.Args[3];
            foreach(string folder in cmdMsg.Args[4].Trim().Split(';')) {
                if(!folder.Equals(""))
                    Folders.Add(folder);
            }
        }

        /// <summary>
        /// Gets or sets the name of the log.
        /// </summary>
        /// <value>
        /// The name of the log.
        /// </value>
        public string LogName {
            get {
                return m_logName;
            }

            set {
                if(value != m_logName) {
                    m_logName = value;
                    NotifyProperyChanged("LogName");
                }
            }
        }
        /// <summary>
        /// Gets or sets the name of the source.
        /// </summary>
        /// <value>
        /// The name of the source.
        /// </value>
        public string SourceName {
            get {
                return m_sourceName;
            }

            set {
                if(value != m_sourceName) {
                    m_sourceName = value;
                    NotifyProperyChanged("SourceName");
                }
            }
        }
        /// <summary>
        /// Gets or sets the output dir path.
        /// </summary>
        /// <value>
        /// The output dir path.
        /// </value>
        public string OutputDirPath {
            get {
                return m_outputDirPath;
            }

            set {
                if(value != m_outputDirPath) {
                    m_outputDirPath = value;
                    NotifyProperyChanged("OutputDirPath");
                }
            }
        }
        /// <summary>
        /// Gets or sets the size of the thumbnail.
        /// </summary>
        /// <value>
        /// The size of the thumbnail.
        /// </value>
        public string ThumbnailSize {
            get {
                return m_thumbnailSize;
            }

            set {
                if(value != m_thumbnailSize) {
                    m_thumbnailSize = value;
                    NotifyProperyChanged("ThumbnailSize");
                }
            }
        }

        /// <summary>
        /// Notifies the propery changed.
        /// </summary>
        /// <param name="name">The name.</param>
        private void NotifyProperyChanged(string name) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// Gets or sets the folders.
        /// </summary>
        /// <value>
        /// The folders.
        /// </value>
        public ObservableCollection<string> Folders {
            get {
                return m_Folders;
            }
            set {
                if(value != m_Folders) {
                    m_Folders = value;
                    NotifyProperyChanged("Folders");
                }
            }
        }

    }
}
