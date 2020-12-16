using CSharpVitamins;

namespace WFA_EJ.Data
{
    public class Subject
    {
        #region Методы

        public override string ToString()
        {
            return Name;
        }

        #endregion

        #region Свойства

        public string Guid { get; set; }
        public string Name { get; set; }
        public string TeacherGuid { get; set; }

        #endregion

        #region Конструкторы

        public Subject()
        {
        }

        public Subject(string Name, string TeacherGuid)
        {
            Guid = ShortGuid.NewGuid().Value;
            this.Name = Name;
            this.TeacherGuid = TeacherGuid;
        }

        public Subject(string Name)
        {
            Guid = ShortGuid.NewGuid().Value;
            this.Name = Name;
        }

        #endregion
    }
}