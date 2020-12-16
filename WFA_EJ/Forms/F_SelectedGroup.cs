using System;
using System.Linq;
using System.Windows.Forms;

namespace WFA_EJ.Forms
{
    public partial class F_SelectedGroup : Form
    {
        #region Поля

        private (string Year, string NameGroup) SelectedGroup;

        #endregion

        #region Конструкторы

        public F_SelectedGroup()
        {
            InitializeComponent();
            treeView1.UpdateTreeView();
        }

        #endregion

        #region Свойства

        public string GroupGuid { get; set; }

        #endregion

        #region Методы

        private void button1_Click(object sender, EventArgs e)
        {
            if (SelectedGroup.Year == null || SelectedGroup.NameGroup == null)
            {
                MessageBox.Show("Выберете группу с которой будете работать");
                return;
            }

            var g = Program.DataBase.DataBaseEntity.Groups;
            GroupGuid = Program.DataBase.DataBaseEntity.Groups
                .First(x => x.DateCreate.Year.ToString() == SelectedGroup.Year && x.Name == SelectedGroup.NameGroup)
                .Guid;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Parent == null && e.Node.Nodes.Count > 0)
            {
                e.Node.ExpandAll();
                return;
            }

            SelectedGroup.Year = e.Node.Parent.Text;
            SelectedGroup.NameGroup = e.Node.Text;
        }

        #endregion
    }
}