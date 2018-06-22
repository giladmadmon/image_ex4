
using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging {
    public class LoggingService : ILoggingService {
        private LogMessageRecords m_LogMessages;

        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;

        /// <summary>
        /// Gets the log messages.
        /// </summary>
        /// <value>
        /// The log messages.
        /// </value>
        public LogMessageRecords LogMessages {
            get { return new LogMessageRecords(m_LogMessages); }
            private set { m_LogMessages = value; }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingService"/> class.
        /// </summary>
        public LoggingService() {
            LogMessages = new LogMessageRecords();
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="type">The type of the message.</param>
        public void Log(string message, MessageTypeEnum type) {
            m_LogMessages.Add(new LogMessageRecord(message, type));

            MessageRecievedEventArgs msgEventArgs = new MessageRecievedEventArgs();
            msgEventArgs.Message = message;
            msgEventArgs.Status = type;
            MessageRecieved?.Invoke(this, msgEventArgs);
        }
    }
}
