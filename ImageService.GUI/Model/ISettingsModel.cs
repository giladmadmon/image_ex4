using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.GUI.Model {
    interface ISettingsModel : INotifyPropertyChanged {
        string OutputDirPath { get; set; }
        string SourceName { get; set; }
        string LogName { get; set; }
        string ThumbnailSize { get; set; }
        ObservableCollection<string> Folders { get; set; }
    }
}
