using CSharpVitamins;

namespace WFA_EJ.Data
{
    public class Student : Person
    {
        #region Методы

        public override string ToString()
        {
            return $@"{FirstName} {Surname?[0]} {Patronymic?[0]}";
        }

        #endregion

        #region Свойства

        public string Guid { get; set; }
        public string GroupGuid { get; set; }

        #endregion

        #region Конструкторы

        public Student()
        {
        }

        public Student(string FirstName, string Surname, string Patronymic, string GroupGuid)
        {
            this.GroupGuid = GroupGuid;
            this.Surname = Surname;
            this.Patronymic = Patronymic;
            this.FirstName = FirstName;
            Guid = ShortGuid.NewGuid().Value;
        }

        #endregion
    }
}