using ImageService.Communication.Model.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Communication.Interfaces {
    public interface ICommunicationChannel<T> {
        bool Connected { get; }

        event EventHandler<DataReceivedEventArgs<T>> OnDataRecieved;
        /// <summary>
        /// Closes this communication channel.
        /// </summary>
        void Close();
        /// <summary>
        /// Starts listening to messages.
        /// </summary>
        /// <returns></returns>
        bool Start();
    }
}
