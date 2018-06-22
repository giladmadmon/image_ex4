using ImageService.Communication.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Communication.Model.Event {
    public class NewClientEventArgs<T> : EventArgs {
        public IClientCommunicationChannel<T> NewClient { get; set; }
      
        /// <summary>
        /// Initializes a new instance of the <see cref="NewClientEventArgs"/> class.
        /// </summary>
        /// <param name="newClient">The new client.</param>
        public NewClientEventArgs(IClientCommunicationChannel<T> newClient) {
            NewClient = newClient;
        }
    }
}
