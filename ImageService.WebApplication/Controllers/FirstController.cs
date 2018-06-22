using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImageService.WebApplication.Models;
using ImageService.Communication.Singleton;
using System.Threading;
using ImageService.Logging;

namespace ImageService.WebApplication.Controllers {
    public class FirstController : Controller {
        // GET: First
        /// <summary>
        /// Index get request.
        /// </summary>
        /// <returns>Index view</returns>
        public ActionResult Index() {
            return View(new ImageWeb());
        }

        /// <summary>
        /// ImageWeb get request.
        /// </summary>
        /// <returns>ImageWeb view</returns>
        [HttpGet]
        public ActionResult ImageWeb() {
            return View();
        }

        // GET: First/logType
        /// <summary>
        /// Logs get request.
        /// </summary>
        /// <returns>Logs view</returns>
        public ActionResult Logs() {
            return View(new Logs(""));
        }

        // POST: First/logType
        /// <summary>
        /// Logs post request.
        /// </summary>
        /// <returns>the logs in json format</returns>
        [HttpPost]
        public JObject Logs(string format) {
            return new Logs("").LogMessages.ToJSON();
        }

        // GET: First/Config
        /// <summary>
        /// Config get request.
        /// </summary>
        /// <returns>Config view</returns>
        public ActionResult Config() {
            return View(new Config());
        }

        // GET: First/DeleteFolder
        /// <summary>
        /// DeleteFolder get request.
        /// </summary>
        /// <returns>DeleteFolder view</returns>
        /// <param name="folder">The folder.</param>
        [HttpGet]
        public ActionResult DeleteFolder(string folder) {
            return View((object)folder);
        }

        /// <summary>
        /// Delete post request.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="confirm">if set to <c>true</c> [confirm].</param>
        [HttpPost]
        public void DeleteFolder(string folder, bool confirm = false) {
            if(confirm && new Config().Folders.Contains(folder)) {
                new DeleteFolder(folder).SendDelete();
            }
        }

        // GET: First/DeleteImage
        /// <summary>
        /// DeleteImage get request.
        /// </summary>
        /// <returns>DeleteImage view</returns>
        [HttpGet]
        public ActionResult DeleteImage(string path, string image) {
            Dictionary<String, String> imageInfo = new Dictionary<String, String>() { { "path", path }, { "image", image } };
            return View(imageInfo);
        }

        /// <summary>
        /// DeleteImage post request.
        /// </summary>
        [HttpPost]
        public void DeleteImage(string image, bool confirm = false) {
            if(confirm) {
                new DeleteImage(image).Delete();
            }
        }

        // GET: First/ViewImage
        /// <summary>
        /// ViewImage get request.
        /// </summary>
        /// <returns>ViewImage view</returns>
        public ActionResult ViewImage(string path, string image) {
            Dictionary<String, String> imageInfo = new Dictionary<String, String>() { { "path", path }, { "image", image } };
            return View(imageInfo);
        }


        // GET: First/Photos
        /// <summary>
        /// Photos get request.
        /// </summary>
        /// <returns>Photos view</returns>
        public ActionResult Photos() {
            return View(new Photos());
        }
    }
}
