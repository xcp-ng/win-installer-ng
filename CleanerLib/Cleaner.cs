using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cleaner
{
    public class Cleaner
    {
        public Cleaner()
        {
            DeviceCleaner = new DeviceClean();
            FileCleaner = new FileClean();
        }

        public DeviceClean DeviceCleaner { get; }
        public FileClean FileCleaner { get; }

        public string GenerateTextReport()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("----------------------------------------------------------------------");
            sb.AppendLine(AssemblyTitle);
            sb.AppendLine("Version: " + Assembly.GetExecutingAssembly().GetName().Version.ToString());
            sb.AppendLine("---------------------");

            sb.Append(DeviceCleaner.CreateReport());
            sb.Append(FileCleaner.CreateReport());

            sb.AppendLine("----------------------------------------------------------------------");

            return sb.ToString();
        }

        private string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }
    }
}
