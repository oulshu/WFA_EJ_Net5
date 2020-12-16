using System.ComponentModel;
using CSharpVitamins;

namespace WFA_EJ.Data
{
    public class Teacher : Person
    {
        #region Методы

        public override string ToString()
        {
            return $@"{FirstName} {Surname[0]}. {Patronymic[0]}.";
        }

        #endregion

        #region Свойства

        public string Guid { get; set; }
        public BindingList<string> SubjectsGuid { get; set; }

        #endregion

        #region Конструкторы

        public Teacher()
        {
        }

        public Teacher(string FirstName, string Surname, string Patronymic, BindingList<string> SubjectsGuid)
        {
            this.Surname = Surname;
            this.Patronymic = Patronymic;
            this.SubjectsGuid = SubjectsGuid;
            this.FirstName = FirstName;
            Guid = ShortGuid.NewGuid().Value;
        }

        #endregion
    }
}