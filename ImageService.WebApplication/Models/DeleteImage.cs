using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using ImageService.Communication.Singleton;
using ImageService.Communication.Model;
using ImageService.Infrastructure.Enums;
using System.IO;

namespace ImageService.WebApplication.Models {
    public class DeleteImage {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteImage"/> class.
        /// </summary>
        /// <param name="image">The image.</param>
        public DeleteImage(string image) {
            this.Image = image;
        }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Image")]
        public string Image { get; set; }

        /// <summary>
        /// Deletes the image in this instance.
        /// </summary>
        public void Delete() {
            try {
                Photos photos = new Photos();
                File.Delete(Image);
                File.Delete(Image.Replace(photos.OutputDirPath,photos.ThumbnailsDirPath));
                SpinWait.SpinUntil(() => !File.Exists(Image));
            } catch { }
        }
    }
}