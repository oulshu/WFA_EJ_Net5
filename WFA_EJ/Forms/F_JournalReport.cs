using System;
using System.Linq;
using System.Windows.Forms;
using WFA_EJ.Data;

namespace WFA_EJ.Forms
{
    public partial class F_JournalReport : Form
    {
        #region Конструкторы

        public F_JournalReport(string selectedGroup)
        {
            InitializeComponent();
            var students = Program.DataBase.DataBaseEntity.Students.Where(student => student.GroupGuid == selectedGroup).OrderBy(x => x.ToString()).ToArray();
            var evaluation_of_students = Program.DataBase.DataBaseEntity.EvaluationOfStudents.Where(x => x.GroupGuid == selectedGroup).ToArray();
            var SubjectsGuids = evaluation_of_students.Select(x => x.SubjectGuid).ToArray();
            var Subjects = Program.DataBase.DataBaseEntity.Subjects.Where(x => SubjectsGuids.Contains(x.Guid)).OrderBy(x => x.Name).ToList();
            DialogResult = DialogResult.Abort;
            if (Subjects.Count == 0)
            {
                MessageBox.Show("По предметам нет оценок");
                return;
            }

            dataGridView1.MyStyleDataGridView();
            Subjects.ToList().ForEach(subject => dataGridView1.Columns.Add(subject.Guid , subject.Name));
            foreach (var student in students)
            {
                var n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].HeaderCell.Value = student.ToString();
            }

            foreach (DataGridViewRow student in dataGridView1.Rows)
                foreach (DataGridViewColumn SubjectGuid in dataGridView1.Columns)
                {
                    var evaluations = evaluation_of_students
                       .Where(
                            x => x.StudentGuid == students[student.Index].Guid && x.SubjectGuid == SubjectGuid.Name
                                                                               && x.Evaluation != EvaluationEnum.Evaluation.Absented).OrderBy(x => x.date_time)
                       .ToList();
                    var evaluationsCount = evaluations.Count;
                    var evaluation = "";
                    if (evaluations.Count == 0)
                    {
                        evaluation = "Н/А";
                    }
                    else
                    {
                        double sum = 0;
                        foreach (var d in evaluations.Select(x => (int)x.Evaluation)) sum += d;
                        var ev = (int)Math.Round(sum / evaluationsCount);
                        evaluation += $"{ev} {sum / evaluationsCount:F} (п/о: ";
                        evaluation = evaluations.Aggregate(evaluation, (current, evaluation_of_student) => current + ((int) evaluation_of_student.Evaluation + ","));
                        evaluation = evaluation.Substring(0, evaluation.Length - 1);
                        evaluation += ")";
                    }

                    student.Cells[SubjectGuid.Name].Value = evaluation;
                }

            DialogResult = DialogResult.OK;
        }

        #endregion
    }
}