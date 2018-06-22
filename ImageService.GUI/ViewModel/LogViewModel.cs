using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Logging.Modal;
using ImageService.Logging;
using System.ComponentModel;
using System.Collections.Specialized;
using ImageService.GUI.Model;

namespace ImageService.GUI.ViewModel {
    public class LogViewModel : INotifyPropertyChanged {
        private LogModel m_model;
        private double m_WindowWidth;
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<LogMessageRecord> VM_LogMessages { get { return m_model.LogMessages; } }

        /// <summary>
        /// Gets or sets the width of the vm window.
        /// </summary>
        /// <value>
        /// The width of the vm window.
        /// </value>
        public double VM_WindowWidth {
            get { return m_WindowWidth; }
            set {
                m_WindowWidth = value;
                NotifyProperyChanged("VM_WindowWidth");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogViewModel"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public LogViewModel(LogModel model) {

            m_model = model;
            m_model.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e) {
                NotifyProperyChanged("VM_" + e.PropertyName);
            };
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

