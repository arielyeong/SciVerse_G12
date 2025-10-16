using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SciVerse_G12
{
    public partial class UpdateAdminProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Username"] != null)
                {
                    LoadUserDetails(Session["Username"].ToString());
                }
                else
                {
                    Response.Redirect("~/Account/Login.aspx");
                }
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            // Default image path
            string imagePath = null;

            // Handle file upload
            if (FileUploadPic.HasFile)
            {
                try
                {
                    string folderPath = Server.MapPath("~/Images/Profile/");
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string fileName = Path.GetFileName(FileUploadPic.FileName);
                    string fullPath = Path.Combine(folderPath, fileName);
                    FileUploadPic.SaveAs(fullPath);

                    // Save relative path for database
                    imagePath = "~/Images/Profile/" + fileName;// we store only the file name
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "⚠️ Image upload failed: " + ex.Message;
                    return;
                }
            }

            // Update user info
            using (SqlConnection con = new SqlConnection(cs))
            {
                // Include picture only if a new one was uploaded
                string query;
                if (imagePath != null)
                {
                    query = "UPDATE tblRegisteredUsers SET fullName = @fullName, emailAddress = @email, age = @age, gender = @gender, country = @country, picture = @picture WHERE username = @username";
                }
                else
                {
                    query = "UPDATE tblRegisteredUsers SET fullName = @fullName, emailAddress = @email, age = @age, gender = @gender, country = @country WHERE username = @username";
                }

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@fullName", txtFullname.Text);
                cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@age", txtAge.Text);
                cmd.Parameters.AddWithValue("@gender", rbGender.SelectedValue);
                cmd.Parameters.AddWithValue("@country", dlCountry.SelectedValue);
                cmd.Parameters.AddWithValue("@username", txtUsername.Text);

                if (imagePath != null)
                    cmd.Parameters.AddWithValue("@picture", imagePath);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                lblMessage.Text = "Profile updated successfully!";
                // Refresh image preview
                if (imagePath != null)
                    imgPreview.ImageUrl = imagePath;

                // ✅ Correct redirect path
                string script = "setTimeout(function() { window.location.href = '" + ResolveUrl("~/Admin/AdminProfile.aspx") + "'; }, 2000);";
                ScriptManager.RegisterStartupScript(this, GetType(), "redirectAfterUpdate", script, true);

            }
        }


        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/AdminProfile.aspx");
        }

        private void LoadUserDetails(string username)
        {
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT fullName, emailAddress, username, age, gender, country, picture FROM tblRegisteredUsers WHERE username = @username";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@username", username);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    txtFullname.Text = dr["fullName"].ToString();
                    txtEmail.Text = dr["emailAddress"].ToString();
                    txtUsername.Text = dr["username"].ToString();
                    txtAge.Text = dr["age"].ToString();

                    string gender = dr["gender"].ToString().Trim();
                    if (rbGender.Items.FindByValue(gender) != null)
                    {
                        rbGender.SelectedValue = gender;
                    }

                    dlCountry.SelectedValue = dr["country"].ToString();

                    // Show the current picture
                    if (dr["picture"] != DBNull.Value && !string.IsNullOrEmpty(dr["picture"].ToString()))
                        imgPreview.ImageUrl = dr["picture"].ToString();
                    else
                        imgPreview.ImageUrl = "~/Images/default.png";
                }
                con.Close();
            }
        }

    }

}