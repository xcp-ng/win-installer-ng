using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Cleaner
{
    public class FileClean
    {
        private bool hasInvestigated = false;
        private DirectoryInfo windowsSystem32Dir;

        public FileClean()
        {
            Path = Environment.GetFolderPath(Environment.SpecialFolder.System);
            SearchPattern = "xen*";

            windowsSystem32Dir = new DirectoryInfo(Path);
        }

        public string Path { get; private set; }
        public string SearchPattern { get; private set; }
        public FileInfo[] Files { get; private set; }

        public bool Investigate()
        {
            Files = windowsSystem32Dir.GetFiles(SearchPattern);
            return Files.Count() > 0;

            hasInvestigated = true;
        }

        public string CreateReport()
        {
            if (!hasInvestigated)
            {
                Investigate();
            }

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("FileCleaner ### Work in progress ### ");

            //sb.AppendLine("DeviceCleaner result: " + Result);
            //sb.AppendLine("DeviceCleaner result: " + ResultText);

            //foreach (var device in Devices)
            //{
            //    sb.AppendLine("- " + device.ID + " [" + (device.Clean ? "Clean" : "Unclean!") + "]");

            //    foreach (var d in device.Parameters)
            //    {
            //        sb.AppendLine("  " + d.Key + ": " + d.Value);
            //    }
            //}

            return sb.ToString();
        }
    }
}
