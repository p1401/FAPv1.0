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
    public partial class StartGUI : Form
    {
        public StartGUI()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AP2Context context = new AP2Context();
            List<Student> listStudents = context.Students.ToList();
            string roll = txtRoll.Text.Trim();
            Boolean check = false;

            foreach (Student student in listStudents)
            {
                if (student.Roll.Equals(roll))
                {
                    MessageBox.Show("Open successfully!");
                    this.Hide();
                    TimetableGUI timetable = new TimetableGUI(student);
                    timetable.ShowDialog();
                    this.Close();
                    check = true;
                }
            }
            if (check == false)
            {
                MessageBox.Show("Roll number is incorrect!");
            }
        }
    }
}
