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
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            this.SuspendLayout();

            /*
             * PCI-Devices 
             */
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_PNPEntity Where deviceid Like '%VEN_5853%'");
            TreeNode pciDevicesNode = treeView1.Nodes.Add("PCI devices");

            foreach (ManagementObject obj in searcher.Get())
            {
                pciDevicesNode.Nodes.Add(obj.ToString());
            }

            pciDevicesNode.ExpandAll();


            /*
             * Leftofer XEN Driver Files in C:\windows\system32
             */
            DirectoryInfo windowsSystem32Dir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.System));
            FileInfo[] xenDriverFiles = windowsSystem32Dir.GetFiles("xen*");

            if (xenDriverFiles.Count() > 0)
            {
                TreeNode xenDriverFilesNode = treeView1.Nodes.Add("Leftover XenDrivers in " + windowsSystem32Dir.FullName + " (" + xenDriverFiles.Count() + ")");
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
    }
}
