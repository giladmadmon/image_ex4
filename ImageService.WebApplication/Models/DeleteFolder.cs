using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using ImageService.Communication.Singleton;
using ImageService.Communication.Model;
using ImageService.Infrastructure.Enums;

namespace ImageService.WebApplication.Models {
    public class DeleteFolder {
        private bool finished;
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteFolder"/> class.
        /// </summary>
        /// <param name="folder">The folder.</param>
        public DeleteFolder(string folder) {
            this.Folder = folder;
        }

        /// <summary>
        /// Called when [client close].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Communication.Model.Event.DataReceivedEventArgs"/> instance containing the event data.</param>
        private void OnClientClose(object sender, Communication.Model.Event.DataReceivedEventArgs e) {
            CommandMessage cmdMsg = CommandMessage.FromJSON(e.Data);

            if(cmdMsg.CmdId == CommandEnum.CloseCommand && Folder.Equals(cmdMsg.Args[0])) {
                ClientCommunication.Instance.OnDataRecieved -= OnClientClose;
                finished = true;
            }
        }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Folder")]
        public string Folder { get; set; }

        /// <summary>
        /// Sends the delete command.
        /// </summary>
        public void SendDelete() {
            if(ClientCommunication.Instance.Connected) {
                finished = false;
                ClientCommunication.Instance.OnDataRecieved += OnClientClose;
                ClientCommunication.Instance.Send(new CommandMessage(CommandEnum.CloseCommand, new string[] { Folder }));
                SpinWait.SpinUntil(() => finished);
            }
        }
    }
}