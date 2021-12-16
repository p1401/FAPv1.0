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
            label8.Text = "ABSENT: 0% ABSENT SO FAR (0 ABSENT ON 0 TOTAL).";
        }

        private void LoadDataForDGV()
        {
            using (AP2Context context = new AP2Context())
            {
                var list = (from c in context.Courses
                            join su in context.Subjects on c.SubjectId equals su.SubjectId
                            join i in context.Instructors on c.InstructorId equals i.InstructorId
                            join cs in context.CourseSchedules on c.CourseId equals cs.CourseId
                            join rcb in context.RollCallBooks on cs.TeachingScheduleId equals rcb.TeachingScheduleId
                            join st in context.Students on rcb.StudentId equals st.StudentId
                            join r in context.Rooms on cs.RoomId equals r.RoomId
                            where st.StudentId == student.StudentId
                            let lect = (i.InstructorMidName != null ?
                            RemoveUnicode(i.InstructorFirstName) + RemoveUnicode(i.InstructorLastName).Substring(0, 1) + RemoveUnicode(i.InstructorMidName).Substring(0, 1) :
                            RemoveUnicode(i.InstructorFirstName) + RemoveUnicode(i.InstructorLastName).Substring(0, 1))
                            select new
                            {
                                DATE = cs.TeachingDate,
                                SLOT = cs.Slot,
                                ROOM = r.RoomCode,
                                LECTURER = lect,
                                GROUP_NAME = su.SubjectCode,
                                ATTEDANCE = ChangeStatus(rcb.isAbsent)
                            }).OrderBy(x => x.DATE).ToList();

                dataGridView1.DataSource = list;

                /*DataTable dt = new DataTable();
                dt.Columns.Add("NO", typeof(int));
                int idx = 1;
                foreach (DataRow row in dataGridView1.Rows)
                    row["NO"] = idx++;
                dt.Columns.Add(new DataColumn[2] { new DataColumn("NO"), list });*/
                CountAbsent();
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

        public DataTable HeaderTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("DATE");
            table.Columns.Add("SLOT");
            table.Columns.Add("ROOM");
            table.Columns.Add("LECTURER");
            table.Columns.Add("GROUP_NAME");
            table.Columns.Add("ATTEDANCE");
            return table;
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

        public static string ChangeStatus(bool isAbsent)
        {
            string status;
            if (isAbsent == true)
            {
                status = "Absent";
            }
            else
            {
                status = "Present";
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
                if (listBox3.Items.Count > 0) listBox3.DataSource = null;
                dataGridView1.Columns.Clear();
                dataGridView1.DataSource = HeaderTable();
                label8.Text = "ABSENT: 0% ABSENT SO FAR (0 ABSENT ON 0 TOTAL).";
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

        public void CountAbsent()
        {
            int total = dataGridView1.Rows.Count;
            int absent = dataGridView1.Rows.Cast<DataGridViewRow>().Where(row => row.Cells["ATTEDANCE"].Value.ToString() == "Absent").Count();
            int percent = (int)Math.Round((double)(100 * absent) / total);

            label8.Text = $"ABSENT: {percent}% ABSENT SO FAR ({absent} ABSENT ON {total} TOTAL).";
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["ATTEDANCE"].Value != null)
                {
                    if (row.Cells["ATTEDANCE"].Value.ToString() == "Absent")
                    {
                        row.Cells["ATTEDANCE"].Style.ForeColor = Color.Red;
                    }
                    else if (row.Cells["ATTEDANCE"].Value.ToString() == "Present")
                    {
                        row.Cells["ATTEDANCE"].Style.ForeColor = Color.Green;
                    }
                }
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
        }
    }
}
