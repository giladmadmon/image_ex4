using ImageService.Communication.Model;
using ImageService.Communication.Singleton;
using ImageService.GUI.Model;
using ImageService.Infrastructure.Enums;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageService.GUI.ViewModel {
    class SettingsViewModel : INotifyPropertyChanged {
        private ISettingsModel m_model;
        private string m_SelectedItem;

        public string VM_SelectedItem {
            get { return m_SelectedItem; }
            set { NotifyProperyChanged("SelectedItem"); m_SelectedItem = value; }
        }
        public string VM_OutputDirPath { get { return m_model.OutputDirPath; } }
        public string VM_SourceName { get { return m_model.SourceName; } }
        public string VM_LogName { get { return m_model.LogName; } }
        public string VM_ThumbnailSize { get { return m_model.ThumbnailSize; } }
        public bool VM_Connected { get { return ClientCommunication.Instance.Connected; } }
        public ObservableCollection<string> VM_Folders { get { return m_model.Folders; } }

        public ICommand RemoveCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsViewModel"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public SettingsViewModel(ISettingsModel model) {
            m_model = model;
            m_model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) {
                NotifyProperyChanged("VM_" + e.PropertyName);
            };

            this.RemoveCommand = (ICommand)new DelegateCommand<object>(new Action<object>(this.OnRemove), new Func<object, bool>(this.CanRemove));
            this.PropertyChanged += new PropertyChangedEventHandler(this.PropertyChangedCheck);
        }

        /// <summary>
        /// Called when a handler is requested to be removed.
        /// </summary>
        /// <param name="obj">The object.</param>
        private void OnRemove(object obj) {
            ClientCommunication.Instance.Send(new CommandMessage(CommandEnum.CloseCommand, new string[] { VM_SelectedItem }));
        }

        /// <summary>
        /// Determines whether this instance can remove the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        ///   <c>true</c> if this instance can remove the specified object; otherwise, <c>false</c>.
        /// </returns>
        private bool CanRemove(object obj) {
            return VM_SelectedItem != null;
        }

        /// <summary>
        /// Properties the changed check.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void PropertyChangedCheck(object sender, PropertyChangedEventArgs e) {
            (this.RemoveCommand as DelegateCommand<object>).RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Notifies the propery changed.
        /// </summary>
        /// <param name="name">The name.</param>
        private void NotifyProperyChanged(string name) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
