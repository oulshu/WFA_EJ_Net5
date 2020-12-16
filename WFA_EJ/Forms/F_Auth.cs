using System;
using System.Windows.Forms;
using WFA_EJ.Forms;

namespace WFA_EJ
{
    public partial class F_Auth : Form
    {
        #region Поля

        private string pass;

        #endregion

        #region Конструкторы

        public F_Auth()
        {
            InitializeComponent();
        }

        #endregion

        #region Свойства

        private string Pass
        {
            get
            {
                if (pass != null) return pass;
                pass = getPass();
                return pass;
            }
        }

        #endregion

        #region Методы

        private string getPass()
        {
            return Program.cfg["password"];
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) Keys.Enter) button1_Click(null, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == Pass)
            {
                Program.Context.MainForm = new F_Main();
                Close();
                Program.Context.MainForm.Show();
            }
            else
            {
                MessageBox.Show("Введен неверный пароль");
            }
        }

        #endregion
    }
}