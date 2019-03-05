using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Reflection;

namespace Cleaner
{
    public class DeviceClean
    {
        private ManagementObjectSearcher searcher;
        private bool hasInvestigated = false;

        public DeviceClean()
        {
            searcher = new ManagementObjectSearcher("root\\CIMV2", "select * from Win32_PnPSignedDriver Where HardWareID Like '%VEN_5853%' OR HardWareID Like '%XEN%'");
            Devices = new List<PCIDevice>();
        }

        public List<PCIDevice> Devices { get; }

        public bool FoundAny { get { return Devices.Count() > 0; } }
        public bool FoundOther { get; private set; } = false;
        public bool Clean { get { return FoundCleanC000 && (FoundClean0001 || FoundClean0002); } }

        public bool FoundC000 { get; private set; } = false;
        public bool Found0001 { get; private set; } = false;
        public bool Found0002 { get; private set; } = false;

        public bool FoundClean0001 { get; private set; } = false;
        public bool FoundCleanC000 { get; private set; } = false;
        public bool FoundClean0002 { get; private set; } = false;

        public InvestigationResult Result = InvestigationResult.Unknown;
        public string ResultText { get; private set; } = "";

        public void Investigate()
        {
            foreach (ManagementObject obj in searcher.Get())
            {
                PCIDevice device = new PCIDevice(obj);
                Devices.Add(device);

                bool hasNoDriver = (device.InfName == string.Empty);

                if (device.ID.Contains("DEV_C000"))
                {
                    FoundC000 = true;
                    FoundCleanC000 = hasNoDriver;
                    device.Clean = hasNoDriver;
                }
                else if (device.ID.Contains("DEV_0001"))
                {
                    Found0001 = true;
                    FoundClean0001 = hasNoDriver;
                    device.Clean = hasNoDriver;
                }
                else if (device.ID.Contains("DEV_0002"))
                {
                    Found0002 = true;
                    FoundClean0002 = hasNoDriver;
                    device.Clean = hasNoDriver;
                }
                else if (device.ID.Contains(@"VEN_XEN&DEV_0000"))
                {
                    device.Clean = true;
                }
                else
                {
                    FoundOther = true;
                }
            }

            if (!FoundAny)
            {
                Result = InvestigationResult.Unclean;
                ResultText = "No XEN PCI devices found!";
            }
            else if (FoundOther)
            {
                Result = InvestigationResult.Unclean;
                ResultText = "Unclean XEN PCI devices found!";
            }
            else if (Clean)
            {
                Result = InvestigationResult.Clean;
                ResultText = "Clean XEN PCI devices found.";
            }
            else
            {
                Result = InvestigationResult.Unknown;
                ResultText = "Unexpected situation. Please create bug report!";
            }

            hasInvestigated = true;
        }

        public string CreateReport()
        {
            if (!hasInvestigated)
            {
                Investigate();
            }

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("DeviceCleaner result: " + Result);
            sb.AppendLine("DeviceCleaner result: " + ResultText);

            foreach (var device in Devices)
            {
                sb.AppendLine("- " + device.ID + " [" + (device.Clean ? "Clean" : "Unclean!") + "]");

                foreach (var d in device.Parameters)
                {
                    sb.AppendLine("  " + d.Key + ": " + d.Value);
                }
            }

            return sb.ToString();
        }
    }
}