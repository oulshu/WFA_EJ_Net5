using System.ComponentModel;
using CSharpVitamins;

namespace WFA_EJ.Data
{
    public class Teacher : Person
    {
        #region Свойства

        public string Guid { get; set; }
        public BindingList<string> SubjectsGuid { get; set; }

        #endregion

        #region Конструкторы

        public Teacher() { }

        public Teacher(string FirstName, string Surname, string Patronymic, BindingList<string> SubjectsGuid)
        {
            this.Surname = Surname;
            this.Patronymic = Patronymic;
            this.SubjectsGuid = SubjectsGuid;
            this.FirstName = FirstName;
            Guid = ShortGuid.NewGuid().Value;
        }

        #endregion

        #region Методы

        public override string ToString()
        {
            var res = FirstName ?? "NoName";
            if (string.IsNullOrEmpty(Surname)) return res;
            res += " " + Surname[0];
            if (string.IsNullOrEmpty(Patronymic)) return res;
            res += " " + Patronymic[0];
            return res;
        }

        #endregion
    }
}