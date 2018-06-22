using ImageService.Communication;
using ImageService.Communication.Interfaces;
using ImageService.Communication.Model.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.ImageService.Server {
    public class ServerCommunication {
        private const string IP = "127.0.0.1";
        private const int PORT = 8003;
        private const int APP_PORT = 8002;
        private IClientCommunicationChannel<string> m_server;
        private IClientCommunicationChannel<byte[]> m_server_app;
        private static ServerCommunication m_serverCommunication;

        public event EventHandler<DataReceivedEventArgs<string>> OnDataReceived {
            add { m_server.OnDataRecieved += value; }
            remove { m_server.OnDataRecieved -= value; }
        }

        public event EventHandler<DataReceivedEventArgs<byte[]>> OnDataReceivedApp {
            add { m_server_app.OnDataRecieved += value; }
            remove { m_server_app.OnDataRecieved -= value; }
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static ServerCommunication Instance {
            get {
                if(m_serverCommunication == null) {
                    m_serverCommunication = new ServerCommunication();
                }
                return m_serverCommunication;
            }
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="ServerCommunication"/> class from being created.
        /// </summary>
        private ServerCommunication() {
            m_server = new TcpClientChannel(IP, PORT);
            m_server_app = new TcpServerChannelByte(IP, APP_PORT);
            m_server.Start();
            m_server_app.Start();
        }

        /// <summary>
        /// Sends the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        public void Send(byte[] data) {
            m_server_app.Send(data);
        }

        /// <summary>
        /// Sends the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        public void Send(string data) {
            m_server.Send(data);
        }

    }
}
