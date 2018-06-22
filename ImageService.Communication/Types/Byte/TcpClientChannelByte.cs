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
    public class TcpClientChannelByte : IClientCommunicationChannel<byte[]> {
        private TcpClient m_client;
        private NetworkStream m_stream;
        private BinaryReader m_reader;
        private BinaryWriter m_writer;

        public bool Connected { get; private set; }

        public event EventHandler<DataReceivedEventArgs<byte[]>> OnDataRecieved;
        /// <summary>
        /// Initializes a new instance of the <see cref="TcpClientChannelByte"/> class.
        /// </summary>
        /// <param name="ip">The ip.</param>
        /// <param name="port">The port.</param>
        public TcpClientChannelByte(string ip, int port) {
            try {
                m_client = new TcpClient();
                m_client.Connect(ip, port);
                Connected = true;
            } catch {
                Connected = false;
            }
            InitClient();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="TcpClientChannelByte"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        public TcpClientChannelByte(TcpClient client) {
            m_client = client;
            InitClient();
        }

        /// <summary>
        /// Initializes the client streams.
        /// </summary>
        private void InitClient() {
            try {
                m_stream = m_client.GetStream();
                m_reader = new BinaryReader(m_stream);
                m_writer = new BinaryWriter(m_stream);
            } catch {
                Connected = false;
            }
        }

        /// <summary>
        /// Closes this communication channel.
        /// </summary>
        public void Close() {
            if(!Connected)
                return;

            m_reader.Close();
            m_writer.Close();
            m_stream.Close();
            m_client.Close();
            Connected = false;
        }

        /// <summary>
        /// Starts listening to messages.
        /// </summary>
        /// <returns></returns>
        public bool Start() {
            new Task(() => {
                try {
                    while(true) {
                        OnDataRecieved?.Invoke(this, GetData());
                    }
                } catch {
                    Close();
                }
            }).Start();
            return true;
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <returns></returns>
        private DataReceivedEventArgs<byte[]> GetData() {
            // read image name
            string name = System.Text.Encoding.Default.GetString(GetArg());

            // read image bitmap
            byte[] image = GetArg();

            return new DataReceivedEventArgs<byte[]>(name, image);
        }

        /// <summary>
        /// Gets the argument.
        /// </summary>
        /// <returns></returns>
        private byte[] GetArg() {
            byte[] lengthBytes = m_reader.ReadBytes(4);

            if(BitConverter.IsLittleEndian)
                Array.Reverse(lengthBytes);

            int length = BitConverter.ToInt32(lengthBytes, 0);
            return m_reader.ReadBytes(length);
        }


        /// <summary>
        /// Sends the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public int Send(byte[] data) {
            try {
                m_writer.Write(data);
                m_writer.Flush();
                return 1;
            } catch {
                Close();
                return 0;
            }
        }
    }
}
