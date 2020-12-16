using System;
using System.ComponentModel;

namespace WFA_EJ.Data
{
    [Serializable]
    public class DataBaseEntity
    {
        #region Конструкторы

        public DataBaseEntity()
        {
            EvaluationOfStudents = new BindingList<EvaluationOfStudent>();
            Students = new BindingList<Student>();
            Teachers = new BindingList<Teacher>();
            Groups = new BindingList<Group>();
            EvaluationEnum = new EvaluationEnum();
            Subjects = new BindingList<Subject>();
        }

        #endregion

        #region Свойства

        public BindingList<EvaluationOfStudent> EvaluationOfStudents { get; set; }
        public BindingList<Student> Students { get; set; }
        public BindingList<Teacher> Teachers { get; set; }
        public BindingList<Group> Groups { get; set; }
        public EvaluationEnum EvaluationEnum { get; set; }
        public BindingList<Subject> Subjects { get; set; }

        #endregion
    }
}