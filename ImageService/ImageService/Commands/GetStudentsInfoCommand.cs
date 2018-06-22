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
    public class GetStudentsInfoCommand : ICommand {

        /// <summary>
        /// excute actions according to the command
        /// </summary>
        /// <param name="args"> the arguments of the command </param>
        /// <param name="result"> the result of the execution </param>
        /// <returns>The String Will Return the New Path if result = true, and will return the error message</returns>
        public string Execute(string[] args, out bool result) {
            List<string> studentsInfo = new List<string>();

            studentsInfo.Add("Gilad,Madmon,123456789");
            studentsInfo.Add("Dafna,Magid,987654321");


            result = true;
            return new CommandMessage(CommandEnum.GetStudentsInfoCommand, studentsInfo.ToArray()).ToJSON();
        }
    }

}
