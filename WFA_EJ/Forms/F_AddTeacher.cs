using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using WFA_EJ.Data;

namespace WFA_EJ.Forms
{
    public partial class F_AddTeacher : Form
    {
        private readonly bool _IsEdit;
        private readonly string _GuidTeacher;
        private BindingList<Subject> _subjects;
        private Teacher _Teacher;

        #region Конструкторы

        public F_AddTeacher(bool IsEdit = false, string GuidTeacher = default)
        {
            _IsEdit = IsEdit;
            _GuidTeacher = GuidTeacher;
            InitializeComponent();

           //_subjects = new BindingList<Subject>();

            if (IsEdit)
            {
                _Teacher = Program.DataBase.DataBaseEntity.Teachers.First(x => x.Guid == GuidTeacher);
                _subjects = new BindingList<Subject>(Program.DataBase.DataBaseEntity.Subjects.Where(x => x.TeacherGuid == GuidTeacher).ToList());
                textBoxFirstName.Text = _Teacher.FirstName;
                textBoxSurname.Text = _Teacher.Surname;
                textBoxPatronymic.Text = _Teacher.Patronymic;
                buttonDelTeacher.Visible = true;
                buttonAddTeacher.Text = "Сохранить";
            }
            else
            {
                _Teacher = new Teacher() {Guid = GuidExtension.GetNewGuid()};
                _subjects = new BindingList<Subject>();
            }
            listBox1.DataSource = _subjects;
            DialogResult = DialogResult.Abort;
        }

        #endregion

        #region Методы

        #endregion

        private void buttonSaveNameSubject_Click(object sender, EventArgs e)
        {
            var nameSubject = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(nameSubject))
            {
                MessageBox.Show("Заполните название предмета");
                return;
            }

            listBox1.DataSource = null;
            listBox1.DataSource = _subjects;
            
            ((Subject) listBox1.SelectedItem).Name = nameSubject;
        }

        private void buttonDelTeacher_Click(object sender, EventArgs e)
        {
            var evaluation_of_students = Program.DataBase.DataBaseEntity.EvaluationOfStudents.Where(x => x.TeacherGuid == _GuidTeacher).ToList();
            var subjects = Program.DataBase.DataBaseEntity.Subjects.Where(x => x.TeacherGuid == _GuidTeacher).ToList();

            Program.DataBase.DataBaseEntity.Teachers.Remove(Program.DataBase.DataBaseEntity.Teachers.First(x => x.Guid == _GuidTeacher));
            foreach (var subject in subjects)
                Program.DataBase.DataBaseEntity.Subjects.Remove(subject);
            foreach (var evaluation_of_student in evaluation_of_students)
                Program.DataBase.DataBaseEntity.EvaluationOfStudents.Remove(evaluation_of_student);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonAddTeacher_Click(object sender, EventArgs e)
        {
            if (_IsEdit)
            {
                _Teacher.FirstName = textBoxFirstName.Text;
                _Teacher.Surname = textBoxSurname.Text;
                _Teacher.Patronymic = textBoxPatronymic.Text;

                _Teacher.SubjectsGuid = new BindingList<string>(_subjects.Select(x => x.Guid).ToList());

                var dbSubject = Program.DataBase.DataBaseEntity.Subjects.Where(x => x.TeacherGuid == _GuidTeacher).ToList();
                var DelSubject = dbSubject.Except(_subjects).ToList();
                var AddSubject = _subjects.Except(dbSubject).ToList();
                var EditSubject = _subjects.Intersect(dbSubject).ToList();

                foreach (var subject in DelSubject)
                    Program.DataBase.DataBaseEntity.Subjects.Remove(subject);

                foreach (var subject in AddSubject)
                    Program.DataBase.DataBaseEntity.Subjects.Add(subject);

                foreach (var subject in EditSubject)
                {
                    var s = Program.DataBase.DataBaseEntity.Subjects.First(x => x.Guid == subject.Guid);
                    s.Name = subject.Name;
                    s.TeacherGuid = subject.TeacherGuid;
                }
                
                
            }
            else
            {
                var SubjectsGuids = new BindingList<string>(_subjects.Select(x => x.Guid).ToList());
                
                _Teacher.FirstName = textBoxFirstName.Text;
                _Teacher.Surname = textBoxSurname.Text;
                _Teacher.Patronymic = textBoxPatronymic.Text;
                _Teacher.SubjectsGuid = SubjectsGuids;

                Program.DataBase.DataBaseEntity.Teachers.Add(_Teacher);
                foreach (var subject in _subjects)
                    Program.DataBase.DataBaseEntity.Subjects.Add(subject);
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonDelSubject_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
                _subjects.Remove((Subject) listBox1.SelectedItem);
        }

        private void buttonAddSubject_Click(object sender, EventArgs e)
        {
            _subjects.Add(new Subject() {Guid = GuidExtension.GetNewGuid(), TeacherGuid = _Teacher.Guid});
            listBox1.SelectedIndex = _subjects.Count-1;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = ((Subject) listBox1.SelectedItem).Name;
        }
    }
}