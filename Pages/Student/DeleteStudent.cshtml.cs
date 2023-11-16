using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data.SqlClient;

namespace CRUDINCORE.Pages.Student
{
    public class DeleteStudentModel : PageModel
    {
        // connection string
        string connString = "Your Connection String Here";
        public int studRegNo;

        public IActionResult OnGet()
        {
            studRegNo = Int32.Parse(Request.Query["regNo"]);

            using (SqlConnection con = new SqlConnection(connString))
            {
                string qry = "DELETE FROM STUDENT WHERE RegNo = @regNo";

                try
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        cmd.Parameters.AddWithValue("@regNo", studRegNo);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            TempData["Message"] = "Student deleted";
                        }
                        else
                        {
                            TempData["Message"] = "Student not deleted";
                        }
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    TempData["Message"] = "There's a problem: " + ex.Message;
                }
            }

            // Redirect to CreateStudent page
            return RedirectToPage("/Student/CreateStudent");
        }
    }
}
