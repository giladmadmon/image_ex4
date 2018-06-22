using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ImageService.Communication.Singleton;
using System.IO;
using ImageService.Communication.Model;
using ImageService.Infrastructure.Enums;
using System.Threading;

namespace ImageService.WebApplication.Models {
    public class ImageWeb {
        private bool finishedPhotosAmount;
        private bool finishedStudentsInfo;
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageWeb"/> class.
        /// </summary>
        public ImageWeb() {
            Students = new List<Student>();
            if(ClientCommunication.Instance.Connected) {
                finishedPhotosAmount = false;
                finishedStudentsInfo = false;
                ClientCommunication.Instance.OnDataRecieved += GetPhotosAmount;
                ClientCommunication.Instance.OnDataRecieved += GetStudentsInfo;
                ClientCommunication.Instance.Send(new CommandMessage(CommandEnum.GetConfigCommand, new string[] { }));
                ClientCommunication.Instance.Send(new CommandMessage(CommandEnum.GetStudentsInfoCommand, new string[] { }));
                SpinWait.SpinUntil(() => finishedPhotosAmount && finishedStudentsInfo);
            }
        }

        /// <summary>
        /// Gets the photos amount.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Communication.Model.Event.DataReceivedEventArgs"/> instance containing the event data.</param>
        private void GetPhotosAmount(object sender, Communication.Model.Event.DataReceivedEventArgs e) {
            CommandMessage cmdMsg = CommandMessage.FromJSON(e.Data);

            if(cmdMsg.CmdId == CommandEnum.GetConfigCommand) {
                PhotosAmount = Directory.GetFiles(Path.Combine(cmdMsg.Args[2], "Thumbnails"), "*", SearchOption.AllDirectories).Length.ToString();

                ClientCommunication.Instance.OnDataRecieved -= GetPhotosAmount;
                finishedPhotosAmount = true;
            }
        }
        /// <summary>
        /// Gets the students information.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Communication.Model.Event.DataReceivedEventArgs"/> instance containing the event data.</param>
        private void GetStudentsInfo(object sender, Communication.Model.Event.DataReceivedEventArgs e) {
            CommandMessage cmdMsg = CommandMessage.FromJSON(e.Data);

            if(cmdMsg.CmdId == CommandEnum.GetStudentsInfoCommand) {
                foreach(string student in cmdMsg.Args) {
                    string[] studentInfo = student.Split(',');
                    Students.Add(new Student(studentInfo[0], studentInfo[1], int.Parse(studentInfo[2])));
                }

                ClientCommunication.Instance.OnDataRecieved -= GetStudentsInfo;
                finishedStudentsInfo = true;
            }
        }

        [Required]
        [Display(Name = "Status")]
        public bool Status {
            get {
                return ClientCommunication.Instance.Connected;
            }
        }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "PhotosAmount")]
        public string PhotosAmount { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Students")]
        public List<Student> Students { get; set; }

        public class Student {
            /// <summary>
            /// Initializes a new instance of the <see cref="Student"/> class.
            /// </summary>
            /// <param name="firstName">The first name.</param>
            /// <param name="lastName">The last name.</param>
            /// <param name="ID">The identifier.</param>
            public Student(string firstName, string lastName, int ID) {
                this.ID = ID;
                this.FirstName = firstName;
                this.LastName = lastName;
            }

            [Required]
            [Display(Name = "ID")]
            public int ID { get; set; }
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "FirstName")]
            public string FirstName { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "LastName")]
            public string LastName { get; set; }
        }
    }
}