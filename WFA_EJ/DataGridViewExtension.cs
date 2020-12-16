using System.Drawing;
using System.Windows.Forms;

namespace WFA_EJ
{
    public static class DataGridViewExtension
    {
        #region Методы

        public static DataGridView MyStyleDataGridView(this DataGridView dataGrid)
        {
            dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllHeaders;
            dataGrid.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            dataGrid.BackgroundColor = SystemColors.Control;
            return dataGrid;
        }

        #endregion
    }
}