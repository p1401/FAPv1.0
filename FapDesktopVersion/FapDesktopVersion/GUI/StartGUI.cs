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
            int id = Convert.ToInt32(txtID.Text.Trim());
            string roll = txtRoll.Text.Trim();
            Boolean check = false;

            foreach (Student student in listStudents)
            {
                if (student.StudentId.Equals(id) && student.Roll.Equals(roll))
                {
                    MessageBox.Show("Open successfully!");
                    TimetableGUI timetable = new TimetableGUI(student);
                    timetable.Show();
                    this.Hide();
                    check = true;
                }
            }
            if (check == false)
            {
                MessageBox.Show("Student ID or Roll number is incorrect!");
            }
        }
    }
}
