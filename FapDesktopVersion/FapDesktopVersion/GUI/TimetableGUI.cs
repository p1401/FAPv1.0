using FapDesktopVersion.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FapDesktopVersion.GUI
{
    public partial class TimetableGUI : Form
    {
        private Student student = null;

        public TimetableGUI()
        {
            InitializeComponent();
        }

        public TimetableGUI(Student st)
        {
            student = st;
        }
    }
}
