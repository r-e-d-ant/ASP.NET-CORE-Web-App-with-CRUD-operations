namespace CRUDINCORE.Pages.Student
{
    public class Student
    {
        public string? regNo { get; set; }
        public string? fullname { get; set; }
        public string? gender { get; set; }
        public string? email { get; set; }
        public DateTime? dob { get; set; }
        public double? marks { get; set; }


        public Student()
        {
        }

        public Student(string? regNo, string? fullname, string? gender, string? email, DateTime? dob, double? marks)
        {
            this.regNo = regNo;
            this.fullname = fullname;
            this.gender = gender;
            this.email = email;
            this.dob = dob;
            this.marks = marks;
        }

    }
}
