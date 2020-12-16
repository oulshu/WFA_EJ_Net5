using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using WFA_EJ.Data;

namespace WFA_EJ.Forms
{
    public partial class F_AddGroup : Form
    {
        #region Конструкторы

        public F_AddGroup(bool IsEdit = false, string GroupGuid = null)
        {
            _IsEdit = IsEdit;
            _GroupGuid = GroupGuid;
            InitializeComponent();
            if (IsEdit)
            {
                students = new BindingList<Student>(Program.DataBase.DataBaseEntity.Students
                    .Where(x => x.GroupGuid == GroupGuid).ToList());
                group = Program.DataBase.DataBaseEntity.Groups.First(x => x.Guid == GroupGuid);
                textBoxNameGroup.Text = group.Name;
                dateTimePickerDateCreate.Value = group.DateCreate;
                buttonDelGroup.Visible = true;
            }
            else
            {
                students = new BindingList<Student>();
                group = new Group(textBoxNameGroup.Text, dateTimePickerDateCreate.Value)
                    {Guid = GuidExtension.GetNewGuid()};
            }

            buttonEditStudent.Visible = false;
            textBoxFirstName.Enabled = false;
            textBoxSurname.Enabled = false;
            textBoxPatronymic.Enabled = false;
            listBox1.DataSource = students;
        }

        #endregion

        #region Поля

        private readonly string _GroupGuid;
        private readonly bool _IsEdit;
        private Group group;
        private BindingList<Student> students;

        #endregion

        #region Методы

        private void buttonAddGroup_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxNameGroup.Text))
            {
                MessageBox.Show("Заполните имя группы");
                return;
            }

            if (!_IsEdit && Program.DataBase.DataBaseEntity.Groups
                .Where(x => x.DateCreate.Year == dateTimePickerDateCreate.Value.Year).Select(x => x.Name)
                .Contains(textBoxNameGroup.Text))
            {
                MessageBox.Show("Такое название группы уже есть");
                return;
            }

            students = new BindingList<Student>(students.Where(x =>
                !string.IsNullOrWhiteSpace(x.FirstName) && !string.IsNullOrWhiteSpace(x.Surname) &&
                !string.IsNullOrWhiteSpace(x.Patronymic)).ToList());
            if (_IsEdit)
            {
                var groupDB = Program.DataBase.DataBaseEntity.Groups.First(x => x.Guid == _GroupGuid);
                var strudentsGuid = students.Select(x => x.Guid).ToList();
                var strudentsNewGuid = strudentsGuid.Except(groupDB.Students).ToList();
                var studentsNew = students.Where(x => strudentsNewGuid.Contains(x.Guid)).ToList();
                var studentsOldGuid = strudentsGuid.Intersect(groupDB.Students).ToList();
                var studentsOld = students.Where(x => studentsOldGuid.Contains(x.Guid)).ToList();
                var strudentsDelGuid = groupDB.Students.Except(strudentsGuid).ToList();
                foreach (var student in studentsNew)
                {
                    Program.DataBase.DataBaseEntity.Students.Add(student);
                    groupDB.Students.Add(student.Guid);
                }

                foreach (var student in studentsOld)
                foreach (var studentDB in Program.DataBase.DataBaseEntity.Students.Where(x => x.Guid == student.Guid))
                {
                    studentDB.FirstName = student.FirstName;
                    studentDB.Surname = student.Surname;
                    studentDB.Patronymic = student.Patronymic;
                }

                foreach (var student in strudentsDelGuid)
                    Program.DataBase.DataBaseEntity.Students.Remove(
                        Program.DataBase.DataBaseEntity.Students.First(x => x.Guid == student));
            }
            else
            {
                group.Students = new BindingList<string>(students.Select(x => x.Guid).ToList());
                Program.DataBase.DataBaseEntity.Groups.Add(group);
                foreach (var student in students) Program.DataBase.DataBaseEntity.Students.Add(student);
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonAddList_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count != 0)
                if (string.IsNullOrWhiteSpace(((Student) listBox1.Items[listBox1.Items.Count - 1]).ToString()))
                    return;
            students.Add(new Student {GroupGuid = group.Guid, Guid = GuidExtension.GetNewGuid()});
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }

        private void buttonDelList_Click(object sender, EventArgs e)
        {
            students.Remove((Student) listBox1.SelectedItem);
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }

        private void buttonEditStudent_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null) return;
            ((Student) listBox1.SelectedItem).FirstName = textBoxFirstName?.Text.Trim();
            ((Student) listBox1.SelectedItem).Surname = textBoxSurname?.Text.Trim();
            ((Student) listBox1.SelectedItem).Patronymic = textBoxPatronymic.Text?.Trim();
            listBox1.DataSource = null;
            listBox1.DataSource = students;
        }

        private void textBoxNameGroup_Leave(object sender, EventArgs e)
        {
            group.Name = textBoxNameGroup.Text?.Trim();
        }

        private void dateTimePickerDateCreate_Leave(object sender, EventArgs e)
        {
            group.DateCreate = dateTimePickerDateCreate.Value;
        }

        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                buttonEditStudent.Visible = true;
                textBoxFirstName.Enabled = true;
                textBoxSurname.Enabled = true;
                textBoxPatronymic.Enabled = true;
                textBoxFirstName.Text = ((Student) listBox1.SelectedValue).FirstName?.Trim();
                textBoxSurname.Text = ((Student) listBox1.SelectedValue).Surname?.Trim();
                textBoxPatronymic.Text = ((Student) listBox1.SelectedValue).Patronymic?.Trim();
            }
            else
            {
                buttonEditStudent.Visible = false;
                textBoxFirstName.Enabled = false;
                textBoxSurname.Enabled = false;
                textBoxPatronymic.Enabled = false;
            }
        }

        private void buttonDelGroup_Click(object sender, EventArgs e)
        {
            var groups = Program.DataBase.DataBaseEntity.Groups.Where(x => x.Guid == _GroupGuid).ToList();
            foreach (var Group in groups) Program.DataBase.DataBaseEntity.Groups.Remove(Group);
            var studentsDel = Program.DataBase.DataBaseEntity.Students.Where(x => x.GroupGuid == _GroupGuid).ToList();
            foreach (var student in students) Program.DataBase.DataBaseEntity.Students.Remove(student);
            var evaluation_of_students = Program.DataBase.DataBaseEntity.EvaluationOfStudents
                .Where(x => x.GroupGuid == _GroupGuid).ToList();
            foreach (var EvaluationOfStudent in evaluation_of_students)
                Program.DataBase.DataBaseEntity.EvaluationOfStudents.Remove(EvaluationOfStudent);
            Close();
            DialogResult = DialogResult.Yes;
        }

        #endregion
    }
}