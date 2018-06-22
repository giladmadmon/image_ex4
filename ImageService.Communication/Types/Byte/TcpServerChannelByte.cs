using ImageService.Communication.Interfaces;
using ImageService.Communication.Model.Event;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Communication {
    public class TcpServerChannelByte : IClientCommunicationChannel<byte[]> {
        private string m_ip;
        private int m_port;
        private TcpListener m_listener;

        public bool Connected { get; private set; }

        public event EventHandler<DataReceivedEventArgs<byte[]>> OnDataSending;
        public event EventHandler<DataReceivedEventArgs<byte[]>> OnDataRecieved;

        /// <summary>
        /// Initializes a new instance of the <see cref="TcpServerChannelByte"/> class.
        /// </summary>
        /// <param name="ip">The ip.</param>
        /// <param name="port">The port.</param>
        public TcpServerChannelByte(string ip, int port) {
            this.m_ip = ip;
            this.m_port = port;
        }
        /// <summary>
        /// Closes this communication channel.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Close() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Starts listening to messages.
        /// </summary>
        /// <returns></returns>
        public bool Start() {
            try {
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(m_ip), m_port);
                m_listener = new TcpListener(ep);
                m_listener.Start();
                Console.WriteLine("Waiting for client connections...");
                new Task(() => {
                    while(true) {
                        TcpClient client = m_listener.AcceptTcpClient();
                        ClientHandlerByte handler = new ClientHandlerByte(client);
                        OnDataSending += handler.Send;
                        handler.OnDataRecieved += this.OnDataRecieved;
                        handler.OnClosed += (sender, e) => OnDataSending -= handler.Send;
                        handler.Start();
                    }
                }).Start();
                return true;
            } catch {
                return false;
            }
        }

        /// <summary>
        /// Handles the OnDataSending event of the TcpServerChannelByte control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataReceivedEventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void TcpServerChannelByte_OnDataSending(object sender, DataReceivedEventArgs<byte[]> e) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sends the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public int Send(byte[] data) {
            try {
                OnDataSending?.Invoke(this, new DataReceivedEventArgs<byte[]>(data));
                return 1;
            } catch {
                return 0;
            }
        }
    }
}

