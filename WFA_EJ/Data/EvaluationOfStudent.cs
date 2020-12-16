using System;
using CSharpVitamins;

namespace WFA_EJ.Data
{
    public class EvaluationOfStudent
    {
        #region Свойства

        public string Guid { get; set; }
        public string TeacherGuid { get; set; }
        public string SubjectGuid { get; set; }
        public string StudentGuid { get; set; }
        public EvaluationEnum.Evaluation Evaluation { get; set; }
        public DateTime date_time { get; set; }
        public string GroupGuid { get; set; }

        #endregion

        #region Конструкторы

        public EvaluationOfStudent(string teacher, string subject, string student, EvaluationEnum.Evaluation evaluation)
        {
            TeacherGuid = teacher;
            SubjectGuid = subject;
            StudentGuid = student;
            Evaluation = evaluation;
            date_time = DateTime.Now;
            Guid = ShortGuid.NewGuid().Value;
        }

        public EvaluationOfStudent()
        {
        }

        #endregion
    }
}