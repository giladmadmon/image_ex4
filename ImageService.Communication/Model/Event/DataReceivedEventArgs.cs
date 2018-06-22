using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Communication.Model.Event {
    public class DataReceivedEventArgs<T> : EventArgs {
        public string Name { get; private set; }
        public T Data { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataReceivedEventArgs"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public DataReceivedEventArgs(T data) {
            this.Name = null;
            this.Data = data;
        }

        public DataReceivedEventArgs(string name, T data) {
            this.Name = name;
            this.Data = data;
        }
    }
}
