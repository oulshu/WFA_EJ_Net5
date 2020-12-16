using System;
using System.ComponentModel;
using CSharpVitamins;

namespace WFA_EJ.Data
{
    public class Group
    {
        #region Свойства

        public string Guid { get; set; }
        public string Name { get; set; }
        public BindingList<string> Students { get; set; }
        public DateTime DateCreate { get; set; }

        #endregion

        #region Конструкторы

        public Group() { }

        public Group(string Name, BindingList<string> Students, DateTime DateCreate)
        {
            this.Name = Name;
            this.Students = Students;
            this.DateCreate = DateCreate;
            Guid = ShortGuid.NewGuid().Value;
        }

        public Group(string Name, DateTime DateCreate)
        {
            this.Name = Name;
            this.DateCreate = DateCreate;
            Guid = ShortGuid.NewGuid().Value;
        }

        #endregion
    }
}