using System.Linq;
using System.Windows.Forms;

namespace WFA_EJ
{
    public static class TreeViewExtension
    {
        #region Методы

        public static TreeView UpdateTreeView(this TreeView tV)
        {
            tV.Nodes.Clear();
            var groupss = Program.DataBase.DataBaseEntity.Groups.ToList();
            if (groupss.Count == 0)
            {
                tV.Nodes.Clear();
                return tV;
            }

            var groups = groupss.GroupBy(x => x.DateCreate.Year).ToList();
            foreach (var dateGroup in groups)
            {
                var tn = new TreeNode[dateGroup.Count()];
                for (var node = 0; node < dateGroup.Count(); node++)
                    tn[node] = new TreeNode(dateGroup.ToList()[node].Name);

                //foreach (var Group in dateGroup)
                //    tn[0] = new TreeNode(Group.Name.ToString());
                tV.Nodes.Add(new TreeNode(dateGroup.Key.ToString(), tn));
            }

            return tV;
        }

        #endregion
    }
}