using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace SciVerse_G12
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            // Check if passwords match
            if (txtPassword.Text != txtConPass.Text)
            {
                lblMessage.Text = "❌ Passwords do not match!";
                return;
            }

            // Handle image upload
            string imagePath = "~/Images/default.png"; // default image if no upload
            if (FileUploadPic.HasFile)
            {
                try
                {
                    string folderPath = Server.MapPath("~/Images/");
                    string fileName = Path.GetFileName(FileUploadPic.FileName);
                    string fullPath = Path.Combine(folderPath, fileName);

                    // Save the uploaded file
                    FileUploadPic.SaveAs(fullPath);

                    // Store virtual path for database
                    imagePath = "~/Images/" + fileName;
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "⚠️ Image upload failed: " + ex.Message;
                    return;
                }
            }

            // Pass data to SqlDataSource
            SqlDataSource1.InsertParameters["dateRegister"].DefaultValue = DateTime.Now.ToString();
            SqlDataSource1.InsertParameters["picture"].DefaultValue = imagePath;

            try
            {
                SqlDataSource1.Insert();
                lblMessage.Text = "✅ Registration Successful!";
                Response.Redirect("~/Account/Login.aspx"); // redirect to login page
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Error inserting record: " + ex.Message + "<br/>" + ex.StackTrace;

            }
        }
    }
}