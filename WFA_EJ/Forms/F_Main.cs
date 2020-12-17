using System;
using System.Linq;
using System.Windows.Forms;
using WFA_EJ.Data;

namespace WFA_EJ.Forms
{
    public partial class F_Main : Form
    {
        #region Поля

        private (string Parent, string node) SelectedGroup;

        #endregion

        #region Конструкторы

        public F_Main()
        {
            InitializeComponent();
            listBoxSelectedTeacher.DataSource = Program.DataBase.DataBaseEntity.Teachers;
            treeView1.UpdateTreeView();
            switch (Program.cfg["SaveTypeFile"])
            {
                case "XML":
                    jSONToolStripMenuItem.Checked = false;
                    xMLToolStripMenuItem.Checked = true;
                    break;
                case "Json":
                    jSONToolStripMenuItem.Checked = true;
                    xMLToolStripMenuItem.Checked = false;
                    break;
                default: throw new ApplicationException("Ошибка в файле конфигурации такого формата нету XML или Json");
            }
        }

        #endregion

        #region Методы

        private async void F_Main_FormClosing(object sender, FormClosingEventArgs e) { await Program.DataBase.Save(); }

        private void группуToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenDialogAndSave(new F_AddGroup());
            treeView1.UpdateTreeView();
            treeView1.Refresh();
        }

        private void преподавателяToolStripMenuItem1_Click(object sender, EventArgs e) { OpenDialogAndSave(new F_AddTeacher()); }

        private bool OpenDialogAndSave(Form form)
        {
            form.ShowDialog();
            form.Dispose();
            return true;
        }

        private void listBoxSelectedTeacher_SelectedValueChanged(object sender, EventArgs e)
        {
            var itemmm = Program.DataBase.DataBaseEntity.Subjects;
            var item = itemmm.Where(x => ((Teacher) listBoxSelectedTeacher.SelectedItem).SubjectsGuid.Contains(x.Guid)).ToArray();
            listBoxSelectedSubject.Items.Clear();
            listBoxSelectedSubject.Items.AddRange(item);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Parent == null && e.Node.Nodes.Count > 0)
            {
                e.Node.ExpandAll();
                return;
            }

            SelectedGroup.Parent = e.Node.Parent.Text;
            SelectedGroup.node = e.Node.Text;
            label4.Text = $@"{SelectedGroup.Parent} {SelectedGroup.node}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (SelectedGroup.node == null || SelectedGroup.Parent == null)
            {
                MessageBox.Show("Выберете группу с которой будете работать");
                return;
            }

            if (listBoxSelectedSubject.SelectedItem == null)
            {
                MessageBox.Show("Выберете предмет с которой будете работать");
                return;
            }

            var selectedGroup =
                Program.DataBase.DataBaseEntity.Groups.First(x => x.Name == SelectedGroup.node && x.DateCreate.Year.ToString() == SelectedGroup.Parent);
            if (selectedGroup.Students == null || selectedGroup.Students.Count == 0)
            {
                MessageBox.Show($"В выбранной группе({SelectedGroup.Parent} - {SelectedGroup.node}) нет ни одного студента");
                return;
            }

            (Group group, Teacher teacher, Subject subject) Selected = (selectedGroup, (Teacher) listBoxSelectedTeacher.SelectedItem,
                (Subject) listBoxSelectedSubject.SelectedItem);
            OpenDialogAndSave(new F_Journal(dateTimePicker1.Value, Selected){Visible = false});
        }

        private void преподавателяToolStripMenuItem_Click(object sender, EventArgs e) { OpenDialogAndSave(new F_AddTeacher(true)); }

        private void EditGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new F_SelectedGroup();
            var result = form.ShowDialog();
            if (DialogResult.OK != result) return;
            OpenDialogAndSave(new F_AddGroup(true, form.GroupGuid));
            treeView1.UpdateTreeView();
        }

        private void xMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            jSONToolStripMenuItem.Checked = false;
            xMLToolStripMenuItem.Checked = true;
            Program.DataBase.SetXML();
        }

        private void jSONToolStripMenuItem_Click(object sender, EventArgs e)
        {
            jSONToolStripMenuItem.Checked = true;
            xMLToolStripMenuItem.Checked = false;
            Program.DataBase.SetJson();
        }

        private async void сохранитьToolStripMenuItem_Click(object sender, EventArgs e) { await Program.DataBase.Save(); }

        private void отчетToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new F_SelectedGroup();
            var result = form.ShowDialog();
            if (DialogResult.OK != result) return;
            OpenDialogAndSave(new F_JournalReport(form.GroupGuid));
        }
        #endregion
    }
}