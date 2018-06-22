using ImageService.Commands;
using ImageService.Communication.Model;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.ImageService.Commands {
    class LogCommand : ICommand {
        private ILoggingService m_logService;

        public LogCommand(ILoggingService logService) {
            m_logService = logService;            // Storing the logging service
        }

        /// <summary>
        /// excute actions according to the command
        /// </summary>
        /// <param name="args"> the arguments of the command </param>
        /// <param name="result"> the result of the execution </param>
        /// <returns>The String Will Return the New Path if result = true, and will return the error message</returns>
        public string Execute(string[] args, out bool result) {
            List<string> logMsgsJSON = new List<string>();
            LogMessageRecords logMsgs = m_logService.LogMessages;

            foreach(LogMessageRecord msgRcrd in logMsgs) {
                logMsgsJSON.Add(msgRcrd.ToJSON());
            }

            result = true;
            return new CommandMessage(CommandEnum.LogCommand, logMsgsJSON.ToArray()).ToJSON();
        }
    }
}
