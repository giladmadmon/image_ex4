using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.ImageService.Modal {
    public class AppConfig {
        private static AppConfig m_appConfig = null;
        public string OutputDirPath { get; private set; }
        public string SourceName { get; private set; }
        public string LogName { get; private set; }
        public string ThumbnailSize { get; private set; }

        private List<string> m_Folders;
        public List<string> Folders {
            get { return m_Folders; }
            private set { m_Folders = new List<string>(value); }
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="AppConfig"/> class from being created.
        /// </summary>
        private AppConfig() {
            this.SourceName = ConfigurationManager.AppSettings["SourceName"];
            this.LogName = ConfigurationManager.AppSettings["LogName"];
            this.OutputDirPath = ConfigurationManager.AppSettings["OutputDir"];
            this.ThumbnailSize = ConfigurationManager.AppSettings["ThunmbnailSize"];
            this.Folders = new List<string>(ConfigurationManager.AppSettings["Handler"].Split(';'));

        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static AppConfig Instance {
            get {
                if(m_appConfig == null) {
                    m_appConfig = new AppConfig();
                }

                return m_appConfig;
            }
        }
    }
}
