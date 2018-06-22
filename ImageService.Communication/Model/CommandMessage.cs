using ImageService.Infrastructure.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Communication.Model {
    public class CommandMessage {

        public CommandEnum CmdId { get; private set; }
        public string[] Args { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandMessage"/> class.
        /// </summary>
        /// <param name="cmdId">The command identifier.</param>
        /// <param name="args">The arguments.</param>
        public CommandMessage(CommandEnum cmdId, string[] args) {
            this.CmdId = cmdId;
            this.Args = args;

        }
        /// <summary>
        /// Turn CommandMessage to JSON format.
        /// </summary>
        /// <returns></returns>
        public string ToJSON() {
            JObject commandMessage = new JObject();
            commandMessage["CmdId"] = (int)this.CmdId;

            JObject args = new JObject();
            for(int i = 1; i <= this.Args.Length; ++i) {
                args[i.ToString()] = (string)this.Args[i - 1];
            }
            commandMessage["Args"] = args;
            commandMessage["ArgsNum"] = this.Args.Length;

            return commandMessage.ToString();
        }

        /// <summary>
        /// Turn JSON format to CommandMessage.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static CommandMessage FromJSON(string str) {

            JObject commandMessage = JObject.Parse(str);
            int size = (int)commandMessage["ArgsNum"];
            int cmdId = (int)commandMessage["CmdId"];
            string[] args = new string[size];
            for(int i = 1; i <= size; ++i) {
               args[i-1] = (string)commandMessage["Args"][i.ToString()];
            }

            return new CommandMessage((CommandEnum)cmdId,args);
        }
    }
}
