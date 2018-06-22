﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    public interface IImageController
    {
        /// <summary>
        /// Executing the Command Requested
        /// </summary>
        /// <param name="commandID"> the command id of the command to be executed </param>
        /// <param name="args"> the arguments of the command </param>
        /// <param name="result"> the result of the command </param>
        /// <returns>The String Will Return the New Path if result = true, and will return the error message</returns>
        string ExecuteCommand(int commandID, string[] args, out bool result);
    }
}
