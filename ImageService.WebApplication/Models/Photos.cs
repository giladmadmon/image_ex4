using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace ImageService.WebApplication.Models {
    public class Photos {
        private static readonly string[] extensions = { ".jpg", ".png", ".gif", ".bmp" }; // the extensions to be monitored

        /// <summary>
        /// Initializes a new instance of the <see cref="Photos"/> class.
        /// </summary>
        public Photos() {
            this.AllPhotos = new List<string>();
            this.OutputDirPath = new Config().OutputDirPath;
            this.ThumbnailsDirPath = Path.Combine(OutputDirPath, "Thumbnails");

            string[] allPhotos = Directory.GetFiles(OutputDirPath, "*", SearchOption.AllDirectories);
            foreach(string photo in allPhotos) {
                if(!photo.StartsWith(ThumbnailsDirPath) && extensions.Contains(Path.GetExtension(photo))) {
                    AllPhotos.Add(photo);
                }
            }
        }

        [Required]
        [Display(Name = "Photos")]
        public List<string> AllPhotos { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Photos Directory")]
        public string OutputDirPath { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Thumbnails Directory")]
        public string ThumbnailsDirPath { get; set; }
    }
}