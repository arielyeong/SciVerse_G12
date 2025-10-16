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
    public partial class EditUserInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["username"] != null)
                {
                    string selectedUser = Request.QueryString["username"].ToString();
                    LoadUserDetails(selectedUser);
                }
                else
                {
                    Response.Redirect("~/Admin/ViewUserList.aspx"); // no user selected
                }
            }
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
                        rbGender.SelectedValue = gender;

                    dlCountry.SelectedValue = dr["country"].ToString();

                    if (dr["picture"] != DBNull.Value)
                        imgPreview.ImageUrl = "~/Images/Profile/" + dr["picture"].ToString();
                }
                con.Close();
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string pictureFileName = null;

            // Handle file upload
            if (FileUploadPic.HasFile)
            {
                pictureFileName = Path.GetFileName(FileUploadPic.PostedFile.FileName);
                string savePath = Server.MapPath("~/Images/Profile/") + pictureFileName;
                FileUploadPic.SaveAs(savePath);
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"UPDATE tblRegisteredUsers 
                         SET fullName = @fullName, emailAddress = @email, age = @age, 
                             gender = @gender, country = @country" + (pictureFileName != null ? ", picture = @picture" : "") +
                                 " WHERE username = @username";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@fullName", txtFullname.Text);
                cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@age", txtAge.Text);
                cmd.Parameters.AddWithValue("@gender", rbGender.SelectedValue);
                cmd.Parameters.AddWithValue("@country", dlCountry.SelectedValue);
                if (pictureFileName != null)
                    cmd.Parameters.AddWithValue("@picture", pictureFileName);
                cmd.Parameters.AddWithValue("@username", txtUsername.Text);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                lblMessage.Text = "Profile updated successfully!";
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/ViewUserList.aspx");
        }
    }
}