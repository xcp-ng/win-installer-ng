using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Cleaner
{
    public partial class CleanerGui : Form
    {
        Cleaner cleaner;

        public CleanerGui()
        {
            InitializeComponent();

            this.Text += " " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version + " (alpha)";
            cleaner = new Cleaner();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.SuspendLayout();

            /*
             * PCI-Devices 
             */

            DeviceClean dc = cleaner.DeviceCleaner;
            dc.Investigate();

            TreeNode pciDevicesNode = treeView1.Nodes.Add("");
            if (dc.Result == InvestigationResult.Unclean)
            {
                pciDevicesNode.ForeColor = Color.Red;
            }
            else if (dc.Result == InvestigationResult.Clean)
            {
                pciDevicesNode.ForeColor = Color.Green;
            }
            else if(dc.Result == InvestigationResult.Unknown)
            {
                pciDevicesNode.ForeColor = Color.Blue;
            }
            pciDevicesNode.Text = dc.ResultText;

            foreach (var device in dc.Devices)
            {
                TreeNode deviceNode = pciDevicesNode.Nodes.Add(device.ID);
                deviceNode.ForeColor = device.Clean ? Color.Green : Color.Red;
                
                foreach (var d in device.Parameters)
                {
                    deviceNode.Nodes.Add(d.Key + ": " + d.Value);
                }
            }

            pciDevicesNode.Expand();


            /*
             * Leftofer XEN Driver Files in C:\windows\system32
             */
            
            FileClean fc = cleaner.FileCleaner;

            if (fc.Investigate())
            {
                TreeNode xenDriverFilesNode = treeView1.Nodes.Add("Leftover XenDrivers in " + fc.Path);
                xenDriverFilesNode.ForeColor = Color.Red;

                foreach (var xenDriverFile in fc.Files)
                {
                    xenDriverFilesNode.Nodes.Add(xenDriverFile.FullName);
                }

                xenDriverFilesNode.ExpandAll();
            }
            else
            {
                TreeNode xenDriverFilesNode = treeView1.Nodes.Add("No Leftover XenDrivers in " + fc.Path);
                xenDriverFilesNode.ForeColor = Color.Green;
            }


            this.ResumeLayout();

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox ab = new AboutBox();
            ab.ShowDialog();
        }

        private void createTextReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report r = new Report(cleaner.GenerateTextReport());
            r.ShowDialog();
        }
    }
}
