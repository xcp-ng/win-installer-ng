using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cleaner
{
    public partial class Report : Form
    {
        public Report(string text)
        {
            InitializeComponent();

            this.textBox1.Text = text;
        }
    }
}
