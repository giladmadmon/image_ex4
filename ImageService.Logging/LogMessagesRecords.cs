using ImageService.Logging;
using ImageService.Logging.Modal;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging {
    public class LogMessageRecord {
        public string Message { get; set; }
        public MessageTypeEnum Type { get; set; }

        public LogMessageRecord(string message, MessageTypeEnum type) {
            this.Message = message;
            this.Type = type;
        }

        /// <summary>
        /// Turn LogMessageRecord to JSON format.
        /// </summary>
        /// <returns></returns>
        public string ToJSON() {
            JObject logMessage = new JObject();

            logMessage["Message"] = this.Message;
            logMessage["Type"] = (int)this.Type;

            return logMessage.ToString();
        }

        /// <summary>
        /// Turns JSON format to LogMessageRecord.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static LogMessageRecord FromJSON(string str) {
            JObject logMsgObj = JObject.Parse(str);

            LogMessageRecord logMsgRcrd = new LogMessageRecord(
            (string)logMsgObj["Message"],
            (MessageTypeEnum)(int)logMsgObj["Type"]);

            return logMsgRcrd;
        }
    }
}

public class LogMessageRecords : ObservableCollection<LogMessageRecord> {

    public LogMessageRecords() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="LogMessageRecords"/> class.
    /// </summary>
    /// <param name="logMsgRcrds">Log messages collection.</param>
    public LogMessageRecords(LogMessageRecords logMsgRcrds) : base(logMsgRcrds) { }

    /// <summary>
    /// Turn LogMessageRecords to JSON format.
    /// </summary>
    /// <returns></returns>
    public JObject ToJSON() {
        JObject logMessageRecords = new JObject();
        logMessageRecords["Size"] = this.Count;

        for(int i = 1; i <= this.Count; ++i) {
            JObject logMessage = new JObject();
            logMessage["Message"] = this[i - 1].Message;
            logMessage["Type"] = (int)this[i - 1].Type;
            logMessageRecords[i.ToString()] = logMessage;
        }

        return logMessageRecords;
    }

    /// <summary>
    /// Turns JSON format to LogMessageRecords.
    /// </summary>
    /// <param name="str">The string.</param>
    /// <returns></returns>
    public static LogMessageRecords FromJSON(string str) {
        LogMessageRecords logMsgs = new LogMessageRecords();
        JObject logMsgsObj = JObject.Parse(str);
        int size = (int)logMsgsObj["Size"];

        for(int i = 1; i <= size; ++i) {
            LogMessageRecord logMsgRcrd = new LogMessageRecord(
            (string)logMsgsObj[i]["Message"],
            (MessageTypeEnum)(int)logMsgsObj[i]["Type"]
                );
        }

        return logMsgs;
    }
}
