using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace CRUDINCORE.Pages.Student
{
    public class StudentModel : PageModel
    {
        // connection string
        string connString = "Your Connection String Here";

        public Student student = new Student();

        // list for storing retrieved students to use later on html page for iteration while displaying
        public List<Student> studentList = new List<Student>();

        public string message = "";

        public void OnGet()
        {
            if (TempData.Count > 0)
                message = TempData["Message"] as string;

            loadStudentList();
        }

        // load student list
        public void loadStudentList()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {

                    string qry = "SELECT * FROM STUDENT";
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Student student = new Student();

                                student.regNo = reader.GetString(0);
                                student.fullname = reader.GetString(1);
                                student.email = reader.GetString(2);
                                student.gender = reader.GetString(3);
                                student.dob = reader.GetDateTime(4);
                                student.marks = reader.GetDouble(5);

                                studentList.Add(student);
                            }
                        }
                    }
                    con.Close(); ;
                }
            }
            catch (Exception ex)
            {
                message = "There's a problem: " + ex.Message;
            }
        }


        public void OnPost()
        {
            try
            {
                student.regNo = Request.Form["regNo"];
                student.fullname = Request.Form["fullname"];
                student.gender = Request.Form["gender"];
                student.email = Request.Form["email"];
                student.dob = DateTime.Parse(Request.Form["dob"]);

                student.marks = double.Parse(Request.Form["marks"]);

            }
            catch (Exception ex)
            {
                message = "There's a problem: " + ex.Message;
            }

            // saving to DB
            using (SqlConnection con = new SqlConnection(connString))
            {
                string qry = "INSERT INTO STUDENT VALUES (@regNo, @fullname, @gender, @email, @dob, @marks)";

                try
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        cmd.Parameters.AddWithValue("@regNo", student.regNo);
                        cmd.Parameters.AddWithValue("@fullname", student.fullname);
                        cmd.Parameters.AddWithValue("@gender", student.gender);
                        cmd.Parameters.AddWithValue("@email", student.email);
                        cmd.Parameters.AddWithValue("@dob", student.dob);
                        cmd.Parameters.AddWithValue("@marks", student.marks);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            message = "Student created";
                            student = new Student(); // empty the inputs
                            loadStudentList();
                        }
                        else
                        {
                            message = "Student not created";
                            loadStudentList();
                        }
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Violation of PRIMARY KEY "))
                    {
                        message = "There's a problem: Student already exists";
                        loadStudentList();
                    }
                    else
                    {
                        message = "There's a problem: " + ex.Message;
                        loadStudentList();
                    }

                }
            }
        }
    }
}
