using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WFA_EJ.Data;

namespace WFA_EJ.Forms
{
    public partial class F_Journal : Form
    {
        #region Поля

        private readonly DateTime _Date;
        private readonly int _DaysInMonth;
        private readonly int CountStudents;
        private readonly string[] status = {"", "1", "2", "3", "4", "5", "НБ"};
        private IEnumerable<EvaluationOfStudent> EvalutionsInput;
        private List<Student> StudentsFromTheSelectedGroup;
        private string[,] tableIn;

        #endregion

        #region Свойства

        private (Group group, Teacher teacher, Subject subject) Selected { get; }

        #endregion

        #region Конструкторы

        public F_Journal(DateTime Date, (Group group, Teacher teacher, Subject subject) selected)
        {
            Selected = selected;
            _Date = Date;
            _DaysInMonth = DateTime.DaysInMonth(Date.Year, Date.Month);
            CountStudents = selected.group.Students.Count;
            InitializeComponent();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            for (var _day = 1; _day <= _DaysInMonth; _day++)
                dataGridView1.Columns.Add(
                    new DataGridViewComboBoxColumn
                    {
                        HeaderText = _day.ToString(),
                        Items =
                        {
                            "",
                            "1",
                            "2",
                            "3",
                            "4",
                            "5",
                            "НБ"
                        },
                        DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing,
                        MinimumWidth = 30
                    });
            var s = Program.DataBase.DataBaseEntity.Students;
            StudentsFromTheSelectedGroup = Program.DataBase.DataBaseEntity.Students.Where(x => x.GroupGuid == Selected.group.Guid).ToList();
            dataGridView1.Rows.Add(CountStudents);
            for (var row = 0; row < CountStudents; row++)
                dataGridView1.Rows[row].HeaderCell.Value = StudentsFromTheSelectedGroup[row].ToString();
            EvalutionsInput = Program.DataBase.DataBaseEntity.EvaluationOfStudents.Where(
                    x => x.TeacherGuid == selected.teacher.Guid && x.SubjectGuid == selected.subject.Guid && Selected.group.Students.Contains(x.StudentGuid))
               .ToList();
            foreach (var evaluation_of_student in EvalutionsInput)
            {
                var indexStudent =
                    StudentsFromTheSelectedGroup.IndexOf(Program.DataBase.DataBaseEntity.Students.First(x => x.Guid == evaluation_of_student.StudentGuid));
                dataGridView1[evaluation_of_student.date_time.Day, indexStudent].Value = status[(int) evaluation_of_student.Evaluation];
            }

            tableIn = GetValuesDataGridView(dataGridView1);
        }

        #endregion

        #region Методы

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            var validClick = e.RowIndex != -1 && e.ColumnIndex != -1;
            var datagridview = sender as DataGridView;
            if (datagridview.Columns[e.ColumnIndex] is DataGridViewComboBoxColumn && validClick)
            {
                datagridview.BeginEdit(true);
                ((ComboBox) datagridview.EditingControl).DroppedDown = true;
            }
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e) { dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit); }

        private string[,] GetValuesDataGridView(DataGridView DGV)
        {
            var table = new string[CountStudents, _DaysInMonth];
            for (var i = 0; i < CountStudents; i++)
                for (var j = 0; j < _DaysInMonth; j++)
                    table[i, j] = DGV[j, i].Value == null ? "" : DGV[j, i].Value.ToString();
            return table;
        }

        private EvaluationEnum.Evaluation GetEvaluationEnum(string status)
        {
            return status switch
            {
                "" => EvaluationEnum.Evaluation.Empty,
                "1" => EvaluationEnum.Evaluation.One,
                "2" => EvaluationEnum.Evaluation.Two,
                "3" => EvaluationEnum.Evaluation.Thee,
                "4" => EvaluationEnum.Evaluation.Four,
                "5" => EvaluationEnum.Evaluation.Five,
                "НБ" => EvaluationEnum.Evaluation.Absented,
                _ => throw new ArgumentException($"GetEvaluationEnum пришло другое значение [{status}]")
            };
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            var tableOut = GetValuesDataGridView(dataGridView1);
            for (var RowIndex = 0; RowIndex < CountStudents; RowIndex++)
                for (var ColumnIndex = 0; ColumnIndex < _DaysInMonth; ColumnIndex++)
                {
                    if (tableIn[RowIndex, ColumnIndex] == tableOut[RowIndex, ColumnIndex]) continue;
                    if (tableIn[RowIndex, ColumnIndex] == "")
                    {
                        var date = Convert.ToDateTime((ColumnIndex + 1).ToString() + '/' + _Date.Month + '/' + _Date.Year);
                        var Evalution = new EvaluationOfStudent
                        {
                            Guid = GuidExtension.GetNewGuid(),
                            date_time = date,
                            Evaluation = GetEvaluationEnum(tableOut[RowIndex, ColumnIndex]),
                            StudentGuid = StudentsFromTheSelectedGroup[RowIndex].Guid,
                            SubjectGuid = Selected.subject.Guid,
                            TeacherGuid = Selected.teacher.Guid,
                            GroupGuid = Selected.group.Guid
                        };
                        Program.DataBase.DataBaseEntity.EvaluationOfStudents.Add(Evalution);
                    }
                    else
                    {
                        Program.DataBase.DataBaseEntity.EvaluationOfStudents.First(
                                x => x.Guid == EvalutionsInput.First(
                                    EvaluationOfStudent => EvaluationOfStudent.StudentGuid == StudentsFromTheSelectedGroup[RowIndex].Guid
                                                           && EvaluationOfStudent.date_time.Day == ColumnIndex).Guid).Evaluation =
                            GetEvaluationEnum(tableOut[RowIndex, ColumnIndex]);
                    }
                }

            DialogResult = DialogResult.OK;
            Close();
        }

        #endregion
    }
}