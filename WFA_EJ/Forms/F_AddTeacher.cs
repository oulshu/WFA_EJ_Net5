using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using WFA_EJ.Data;

namespace WFA_EJ.Forms
{
    public partial class F_AddTeacher : Form
    {
        #region Конструкторы

        public F_AddTeacher(bool IsEdit = false)
        {
            InitializeComponent();
            var bs = Program.DataBase.DataBaseEntity.Subjects;
            if (IsEdit)
            {
            }

            DialogResult = DialogResult.Abort;
        }

        #endregion

        #region Методы

        private void F_AddTeacher_Load(object sender, EventArgs e) { }

        private void button1_Click(object sender, EventArgs e)
        {
            var SubjectName = dataGridView1.Rows.Cast<DataGridViewRow>().Take(dataGridView1.Rows.Count - 1)
               .Select(row => row.Cells["ColSubject"].Value.ToString()).ToList();
            var Subjects = SubjectName.Select(S => new Subject(S)).ToList();
            var teacher = new Teacher(
                    textBoxFirstName.Text, textBoxSurname.Text, textBoxPatronymic.Text, new BindingList<string>(Subjects.Select(x => x.Guid).ToList()))
                {Guid = GuidExtension.GetNewGuid()};
            Subjects.ForEach(x => x.TeacherGuid = teacher.Guid);
            Subjects.ForEach(Program.DataBase.DataBaseEntity.Subjects.Add);
            Program.DataBase.DataBaseEntity.Teachers.Add(teacher);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonDelT_Click(object sender, EventArgs e) { }

        #endregion
    }
}