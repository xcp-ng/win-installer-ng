using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Windows.Forms;

namespace cleaner
{
    public partial class Cleaner : Form
    {
        public Cleaner()
        {
            InitializeComponent();

            this.Text += " " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            this.SuspendLayout();

            /*
             * PCI-Devices 
             */
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "select * from Win32_PnPSignedDriver Where HardWareID Like '%VEN_5853%' OR HardWareID Like '%XEN%'");
            TreeNode pciDevicesNode = treeView1.Nodes.Add("PCI devices");

            bool foundAny = false;
            bool foundEmptyC000 = false;
            bool foundEmpty0002 = false;
            bool foundEmpty0001 = false;
            bool foundACPI = false;
            bool foundOther = false;

            foreach (ManagementObject obj in searcher.Get())
            {
                foundAny = true;

                bool green = false;
                string deviceID = Convert.ToString(obj["DeviceID"]);
                string infName = Convert.ToString(obj["InfName"]);
                TreeNode pciDevice = pciDevicesNode.Nodes.Add(deviceID);

                if (deviceID.Contains("DEV_C000"))
                {
                    if (infName == "")
                    {
                        foundEmptyC000 = true;
                        green = true;
                    }
                }
                else if (deviceID.Contains("DEV_0002"))
                {
                    if (infName == "")
                    {
                        foundEmpty0002 = true;
                        green = true;
                    }
                }
                else if (deviceID.Contains("DEV_0001"))
                {
                    if (infName == "")
                    {
                        foundEmpty0001 = true;
                        green = true;
                    }
                }
                else if (deviceID.Contains(@"XEN0000"))
                {
                    // ignore
                    green = true;
                }
                else
                {
                    foundOther = true;
                }

                // Only Informational Section
                AddDeviceProperty(obj, pciDevice, "FriendlyName");
                AddDeviceProperty(obj, pciDevice, "HardwareID");
                AddDeviceProperty(obj, pciDevice, "DriverVersion");
                AddDeviceProperty(obj, pciDevice, "DriverDate");
                AddDeviceProperty(obj, pciDevice, "InfName");
                AddDeviceProperty(obj, pciDevice, "Manufacturer");
                AddDeviceProperty(obj, pciDevice, "DriverProviderName");
                AddDeviceProperty(obj, pciDevice, "Signer");
                AddDeviceProperty(obj, pciDevice, "StartMode");
                AddDeviceProperty(obj, pciDevice, "Status");

                // Set color
                if (green)
                {
                    pciDevice.ForeColor = Color.Green;
                }
                else
                {
                    pciDevice.ForeColor = Color.Red;
                }
            }

            if (!foundAny)
            {
                pciDevicesNode.ForeColor = Color.Red;
                pciDevicesNode.Text = "No XEN PCI devices found!";
            }
            else if (foundOther)
            {
                pciDevicesNode.ForeColor = Color.Red;
                pciDevicesNode.Text = "Used XEN PCI devices found!";
            }
            else if (foundEmptyC000 && (foundEmpty0002 || foundEmpty0001))
            {
                pciDevicesNode.ForeColor = Color.Green;
            }

            pciDevicesNode.Expand();


            /*
             * Leftofer XEN Driver Files in C:\windows\system32
             */
            DirectoryInfo windowsSystem32Dir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.System));
            FileInfo[] xenDriverFiles = windowsSystem32Dir.GetFiles("xen*");

            if (xenDriverFiles.Count() > 0)
            {
                TreeNode xenDriverFilesNode = treeView1.Nodes.Add("Leftover XenDrivers in " + windowsSystem32Dir.FullName);
                xenDriverFilesNode.ForeColor = Color.Red;

                foreach (var xenDriverFile in xenDriverFiles)
                {
                    xenDriverFilesNode.Nodes.Add(xenDriverFile.FullName);
                }

                xenDriverFilesNode.ExpandAll();
            }
            else
            {
                TreeNode xenDriverFilesNode = treeView1.Nodes.Add("No Leftover XenDrivers in " + windowsSystem32Dir.FullName);
                xenDriverFilesNode.ForeColor = Color.Green;
            }


            this.ResumeLayout();

        }

        private void AddDeviceProperty(ManagementObject obj, TreeNode pciDevice, string property)
        {
            pciDevice.Nodes.Add(property + ": " + Convert.ToString(obj[property]));
        }
    }
}
