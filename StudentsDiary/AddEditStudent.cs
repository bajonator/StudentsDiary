using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace StudentsDiary
{
    public partial class AddEditStudent : Form
    {
        private FileHelper<List<Student>> _fileHelper = new FileHelper<List<Student>>(Program.FilePath);
        private int _studentId;
        private Student _student;

        public AddEditStudent(int id = 0)
        {
            InitializeComponent();
            _studentId = id;

            GetStudentData();

            tbFirstName.Select();
        }

        private void GetStudentData()
        {
            if (_studentId != 0)
            {
                Text = "Edytowanie danych ucznia";

                var students = _fileHelper.DeserializeFromFile();
                _student = students.FirstOrDefault(x => x.Id == _studentId);

                if (_student == null)
                    throw new Exception("Brak ucznia o podanym Id");

                FillTextBoxes();
            }
        }

        private void FillTextBoxes()
        {
            tbId.Text = _student.Id.ToString();
            tbFirstName.Text = _student.FirstName.ToString();
            tbLastName.Text = _student.LastName.ToString();
            tbMath.Text = _student.Math.ToString();
            tbPhysics.Text = _student.Physics.ToString();
            tbTechnology.Text = _student.Technology.ToString();
            tbPolishLang.Text = _student.PolishLang.ToString();
            tbForeignLang.Text = _student.ForeignLang.ToString();
            rtbComments.Text = _student.Comments.ToString();
            chbActivities.Checked = _student.Activities; //ZADANIE DOMOWE
            cbGroup.Text = _student.Group.ToString(); //ZADANIE DOMOWE
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            var students = _fileHelper.DeserializeFromFile();

            if (_studentId != 0)
                students.RemoveAll(x => x.Id == _studentId);
            else
                AssignIdToNewStudent(students);

            AddnewUserToList(students);

            _fileHelper.SerializeToFile(students);

            Close();
        }

        private void AddnewUserToList(List<Student> students)
        {
            var student = new Student
            {
                Id = _studentId,
                FirstName = tbFirstName.Text,
                LastName = tbLastName.Text,
                Comments = rtbComments.Text,
                ForeignLang = tbForeignLang.Text,
                Math = tbMath.Text,
                Physics = tbPhysics.Text,
                PolishLang = tbPolishLang.Text,
                Technology = tbTechnology.Text,
                Activities = chbActivities.Checked, //ZADANIE DOMOWE
                Group = cbGroup.Text //ZADANIE DOMOWE
            };

            students.Add(student);            
        }

        private void AssignIdToNewStudent(List<Student> students)
        {
            var studentWithHihgestId = students.OrderByDescending(x => x.Id).FirstOrDefault();
            _studentId = studentWithHihgestId == null ? 1 : studentWithHihgestId.Id + 1;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
