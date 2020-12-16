using CSharpVitamins;

namespace WFA_EJ.Data
{
    public class Student : Person
    {
        #region Свойства

        public string Guid { get; set; }
        public string GroupGuid { get; set; }

        #endregion

        #region Конструкторы

        public Student() { }

        public Student(string FirstName, string Surname, string Patronymic, string GroupGuid)
        {
            this.GroupGuid = GroupGuid;
            this.Surname = Surname;
            this.Patronymic = Patronymic;
            this.FirstName = FirstName;
            Guid = ShortGuid.NewGuid().Value;
        }

        #endregion

        #region Методы

        public override string ToString()
        {
            var res = FirstName ?? "NoName";
            if (string.IsNullOrEmpty(Surname)) return res;
            res += " " + Surname[0] + ".";
            if (string.IsNullOrEmpty(Patronymic)) return res;
            res += " " + Patronymic[0] + ".";
            return res;
        }

        #endregion
    }
}