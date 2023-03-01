using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HW.Logger
{
    internal class Logger
    {
        private readonly string filename;
        public Logger(string filename)
        {
            this.filename = Path.Combine(AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin")), filename);
        }
        public void Log(string message, string level = "INFO")
        {
            File.AppendAllText(filename, $"{level} - {message}\r\n");
        }
    }
}
