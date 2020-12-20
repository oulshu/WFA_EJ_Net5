using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WFA_EJ.Data;

namespace WFA_EJ.Forms
{
    public partial class F_SelectedTeacher : Form
    {
        private string _guidTeacher = "None";
        public string GuidTeacher { get=>_guidTeacher; set=>_guidTeacher=value; }
        public F_SelectedTeacher()
        {
            InitializeComponent();
            listBox1.DataSource = Program.DataBase.DataBaseEntity.Teachers;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Не выбрана ни одна группа!");
                return;
            }

            var guidTeacher = ((Teacher)listBox1.SelectedItem).Guid;
            _guidTeacher = guidTeacher;
            DialogResult = DialogResult.OK;
        }
    }
}
