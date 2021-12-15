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
            InitializeComponent();
            student = st;
        }

        private void TimetableGUI_Load(object sender, EventArgs e)
        {
            string fullName = $"{student.LastName} {student.MidName} {student.FirstName}";

            listBox1.DataSource = GetAllCampus();
            listBox1.SelectedIndex = 0;

            listBox2.DataSource = GetTermByCampus();
            listBox2.SelectedIndex = 0;

            listBox3.DataSource = GetCourseByTerm();

            label2.Text = "CAMPUS: " + listBox1.SelectedItem.ToString();
            label12.Text = $"View attendance for {GetLoginCode()} ({fullName})";
            label4.Text = GetLoginCode().ToLower();
            LoadDataForDGV();
        }

        private void LoadDataForDGV()
        {
            using (AP2Context context = new AP2Context())
            {
                dataGridView1.DataSource = (from c in context.Courses
                               join s in context.Subjects on c.SubjectId equals s.SubjectId
                               join cs in context.CourseSchedules on c.CourseId equals cs.CourseId
                               join rcb in context.RollCallBooks on cs.TeachingScheduleId equals rcb.TeachingScheduleId
                               join r in context.Rooms on cs.RoomId equals r.RoomId
                               where rcb.StudentId == student.StudentId
                               select new
                               {
                                   s.SubjectCode,
                                   r.RoomCode,
                                   rcb.IsAbsent
                               }).ToList();
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            StartGUI s = new StartGUI();
            s.ShowDialog();
            this.Close();
        }

        public static string RemoveUnicode(string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
                "đ","é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ", "í","ì","ỉ","ĩ","ị", "ó","ò","ỏ","õ","ọ","ô", "ố","ồ","ổ","ỗ",
                "ộ","ơ","ớ","ờ","ở","ỡ","ợ","ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự","ý","ỳ","ỷ","ỹ","ỵ",};
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
                "d","e","e","e","e","e","e","e","e","e","e","e", "i","i","i","i","i", "o","o","o","o","o","o","o","o","o","o",
                "o","o","o","o","o","o","o", "u","u","u","u","u","u","u","u","u","u","u", "y","y","y","y","y",};
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            return text;
        }

        public string GetLoginCode()
        {
            string mid = student.MidName;
            if (mid != null)
            {
                return RemoveUnicode(student.FirstName) + RemoveUnicode(student.LastName).Substring(0, 1) + RemoveUnicode(mid).Substring(0, 1) + student.Roll;
            }
            else
            {
                return RemoveUnicode(student.FirstName) + RemoveUnicode(student.LastName).Substring(0, 1) + student.Roll;
            }
        }

        public List<string> GetAllCampus()
        {
            using (AP2Context context = new AP2Context())
            {
                var campuses = context.Campuses.Select(x => x.CampusCode).ToList();
                return campuses;
            }
        }

        public List<string> GetTermByCampus()
        {
            string campusName = listBox1.SelectedItem.ToString();
            using (AP2Context context = new AP2Context())
            {
                var term = context.Courses.Join(context.Terms,
                    course => course.TermId,
                    term => term.TermId,
                    (course, term) => new { course, term })
                    .Join(context.Campuses,
                    cs => cs.course.CampusId,
                    campus => campus.CampusId,
                    (cs, campus) => new
                    {
                        campus.CampusName,
                        cs.term.TermName
                    }).Where(x => x.CampusName == campusName).Select(x => x.TermName).ToList();
                return term;
            }
        }

        public List<string> GetCourseByTerm()
        {
            string termName = listBox2.SelectedItem.ToString();
            using (AP2Context context = new AP2Context())
            {
                var course = context.Courses.Join(context.Terms,
                    course => course.TermId,
                    term => term.TermId,
                    (course, term) => new { course, term })
                    .Join(context.Subjects,
                    cs => cs.course.SubjectId,
                    course => course.SubjectId,
                    (cs, course) => new
                    {
                        cs.term.TermName,
                        course.SubjectCode
                    }).Where(x => x.TermName == termName).Select(x => x.SubjectCode).ToList();
                return course;
            }
        }
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox3.DataSource = GetCourseByTerm();
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox2.DataSource = GetTermByCampus();
        }
    }
}
