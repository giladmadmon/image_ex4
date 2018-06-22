using ImageService.Commands;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using ImageService.ImageService.Modal;
using ImageService.Communication.Model;
using ImageService.Infrastructure.Enums;

namespace ImageService.ImageService.Commands {
    public class GetConfigCommand : ICommand {

        /// <summary>
        /// excute actions according to the command
        /// </summary>
        /// <param name="args"> the arguments of the command </param>
        /// <param name="result"> the result of the execution </param>
        /// <returns>The String Will Return the New Path if result = true, and will return the error message</returns>
        public string Execute(string[] args, out bool result) {
            AppConfig appConfig = AppConfig.Instance;
            List<string> configs = new List<string>();

            configs.Add(appConfig.SourceName);
            configs.Add(appConfig.LogName);
            configs.Add(appConfig.OutputDirPath);
            configs.Add(appConfig.ThumbnailSize);
            configs.Add(String.Join(";", appConfig.Folders));

            result = true;
            return new CommandMessage(CommandEnum.GetConfigCommand, configs.ToArray()).ToJSON();
        }
    }

}
