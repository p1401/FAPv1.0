using FapDesktopVersion.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.SqlServer;
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

            listBox1.DataSource = GetCampusByStudent();

            listBox2.DataSource = GetAllTerm();

            listBox3.DataSource = GetSubjectByTerm();

            label2.Text = "CAMPUS: " + listBox1.SelectedItem.ToString();
            label12.Text = $"View attendance for {GetLoginCode()} ({fullName})";
            label4.Text = GetLoginCode().ToLower();
        }

        private void LoadDataForDGV()
        {
            using (AP2Context context = new AP2Context())
            {
                var list = (from c in context.Courses
                            join s in context.Subjects on c.SubjectId equals s.SubjectId
                            join i in context.Instructors on c.InstructorId equals i.InstructorId
                            join cs in context.CourseSchedules on c.CourseId equals cs.CourseId
                            join rcb in context.RollCallBooks on cs.TeachingScheduleId equals rcb.TeachingScheduleId
                            join r in context.Rooms on cs.RoomId equals r.RoomId
                            where rcb.StudentId == student.StudentId
                            select new
                            {
                                DATE = cs.TeachingDate,
                                SLOT = cs.Slot,
                                ROOM = r.RoomCode,
                                LECTURER = RemoveUnicode(i.InstructorFirstName),
                                GROUP_NAME = s.SubjectCode,
                                ATTEDANCE = ChangeStatus(rcb.isAbsent)
                            }).OrderBy(x => x.DATE).ToList();

                /*List<string> lec = (from sc in context.StudentCourses
                                  join s in context.Students on sc.StudentId equals s.StudentId
                                  join c in context.Courses on sc.CourseId equals c.CourseId
                                  join i in context.Instructors on c.InstructorId equals i.InstructorId
                                  where s.StudentId == student.StudentId
                                  select new
                                  {
                                      i.InstructorFirstName,
                                      i.InstructorLastName,
                                      i.InstructorMidName
                                  }).Select(x => x.InstructorMidName != null ? 
                                  $"{RemoveUnicode(x.InstructorFirstName)} {RemoveUnicode(x.InstructorLastName).Substring(0, 1)} {RemoveUnicode(x.InstructorMidName).Substring(0, 1)}" :
                                  $"{RemoveUnicode(x.InstructorFirstName)} {RemoveUnicode(x.InstructorLastName).Substring(0, 1)}").ToList();*/

                dataGridView1.DataSource = list;
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
                return RemoveUnicode(student.FirstName) + RemoveUnicode(student.LastName).Substring(0, 1) + string.Concat(RemoveUnicode(mid).Where(c => char.IsUpper(c))) + student.Roll;
            }
            else
            {
                return RemoveUnicode(student.FirstName) + RemoveUnicode(student.LastName).Substring(0, 1) + student.Roll;
            }
        }

        /*public List<string> DataHeader()
        {
            DataRelation data;
            data = data.
        }*/

        public static string ChangeStatus(bool isAbsent)
        {
            TimetableGUI gui = new TimetableGUI();
            DataGridView dataGridView = gui.dataGridView1;
            string status = null;
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (isAbsent == true)
                {
                    row.Cells["ATTEDANCE"].Style.ForeColor = Color.Red;
                    status = "Absent";
                }
                else
                {
                    row.Cells["ATTEDANCE"].Style.ForeColor = Color.Green;
                    status = "Present";
                }
            }
            return status;
        }

        public List<string> GetCampusByStudent()
        {
            using (AP2Context context = new AP2Context())
            {
                var term = (from sc in context.StudentCourses
                            join s in context.Students on sc.StudentId equals s.StudentId
                            join co in context.Courses on sc.CourseId equals co.CourseId
                            join ca in context.Campuses on co.CampusId equals ca.CampusId
                            where s.StudentId == student.StudentId
                            select new
                            {
                                ca.CampusCode
                            }).Select(x => x.CampusCode).Distinct().ToList();
                return term;
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

        public List<string> GetAllTerm()
        {
            using (AP2Context context = new AP2Context())
            {
                var term = context.Terms.Select(x => x.TermName).ToList();
                return term;
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
                    }).Where(x => x.CampusName == campusName).Select(x => x.TermName).Distinct().ToList();
                return term;
            }
        }

        public List<string> GetSubjectByTerm()
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
                        course.SubjectName
                    }).Where(x => x.TermName == termName).Select(x => x.SubjectName).Distinct().ToList();
                return course;
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(GetSubjectByTerm().Count == 0)
            {
                dataGridView1.DataSource = null;
            }
            else
            {
                listBox3.DataSource = GetSubjectByTerm();
            }
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.Columns.Clear();
            LoadDataForDGV();
        }

    }
}
