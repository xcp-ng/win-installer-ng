using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace cleaner
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);




            /*
             * Leftofer XEN Driver Files in C:\windows\system32
             */
            DirectoryInfo windowsSystem32Dir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.System));
            FileInfo[] xenDriverFiles = windowsSystem32Dir.GetFiles("xen*");

            if (xenDriverFiles.Count() > 0)
            {
                TreeNode xenDriverFilesNode = treeView1.Nodes.Add("Leftover XenDrivers in " + windowsSystem32Dir.FullName + " (" + xenDriverFiles.Count() + ")");

                foreach (var xenDriverFile in xenDriverFiles)
                {
                    xenDriverFilesNode.Nodes.Add(xenDriverFile.FullName);
                }
            }




        }
    }
}
