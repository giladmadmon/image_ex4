using ImageService.Communication.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Communication.Model.Event;
using System.Net.Sockets;
using System.IO;

namespace ImageService.Communication {
    public class ClientHandler : TcpClientChannel {
        public event EventHandler<EventArgs> OnClosed;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientHandler"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        public ClientHandler(TcpClient client) : base(client) { }

        /// <summary>
        /// Closes this communication channel.
        /// </summary>
        public new void Close() {
            base.Close();
            OnClosed?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Sends the specified sender.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DataReceivedEventArgs"/> instance containing the event data.</param>
        public void Send(object sender, DataReceivedEventArgs<string> e) {
            Send(e.Data);
        }
    }
}
