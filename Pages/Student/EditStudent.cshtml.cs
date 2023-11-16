using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace CRUDINCORE.Pages.Student
{
    public class EditStudentModel : PageModel
    {
        // connection string
        string connString = "Your Connection String Here";

        public Student student = new Student();

        public string message = "";

        public int studRegNo;

        public void OnGet()
        {
            studRegNo = Int32.Parse(Request.Query["regNo"]);
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {

                    string qry = "SELECT regNo, fullname, email, gender, dob, marks FROM STUDENT WHERE regNo=@v_regNo";
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        cmd.Parameters.AddWithValue("@v_regNo", studRegNo);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                student.regNo = reader.GetString(0);
                                student.fullname = reader.GetString(1);
                                student.email = reader.GetString(2);
                                student.gender = reader.GetString(3);
                                student.dob = reader.GetDateTime(4);
                                student.marks = reader.GetDouble(5);
                            } else
                            {
                                message = "No information available for this student:";
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

            // updating info
            using (SqlConnection con = new SqlConnection(connString))
            {
                string qry = "UPDATE STUDENT SET fullname=@fullname, gender=@gender, email=@email, dob=@dob, marks=@marks WHERE regNo=@regNo";

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
                            TempData["Message"] = "Student updated";
                            Response.Redirect("/Student/CreateStudent");
                        }
                        else
                        {
                            message = "Student not updated";
                        }
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    message = "Student not updated " + ex.Message;
                }
            }
        }
    }
}
