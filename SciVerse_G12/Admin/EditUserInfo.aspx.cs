using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;

namespace SciVerse_G12
{
    public partial class EditUserInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Validation: Admin access
                if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
                {
                    Response.Redirect("~/Account/Login.aspx");
                    return;
                }

                // Get RID from query (safer than username)
                if (Request.QueryString["rid"] != null)
                {
                    int rid;
                    if (int.TryParse(Request.QueryString["rid"], out rid))
                    {
                        LoadUserDetails(rid);
                        return;
                    }
                }

                // Invalid/no param
                Response.Redirect("~/Admin/ViewUserList.aspx");
            }
        }

        private void LoadUserDetails(int rid)
        {
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT fullName, emailAddress, username, age, gender, country, picture FROM tblRegisteredUsers WHERE RID = @rid";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@rid", rid);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    txtFullname.Text = dr["fullName"]?.ToString() ?? "";
                    txtEmail.Text = dr["emailAddress"]?.ToString() ?? "";
                    txtUsername.Text = dr["username"]?.ToString() ?? ""; // Read-only
                    txtAge.Text = dr["age"]?.ToString() ?? "";

                    string gender = dr["gender"]?.ToString()?.Trim() ?? "";
                    if (rbGender.Items.FindByValue(gender) != null)
                        rbGender.SelectedValue = gender;

                    string country = dr["country"]?.ToString() ?? "";
                    if (dlCountry.Items.FindByValue(country) != null)
                        dlCountry.SelectedValue = country;

                    string picture = dr["picture"]?.ToString()?.Trim();
                    if (!string.IsNullOrEmpty(picture))
                    {
                        // Safe path: If not full, prepend
                        if (!picture.StartsWith("~") && !picture.StartsWith("/"))
                            picture = "~/Images/Profile/" + picture;
                        imgPreview.ImageUrl = picture;
                    }
                    else
                    {
                        imgPreview.ImageUrl = "~/Images/Profile/default.png";
                    }
                }
                else
                {
                    // No data - redirect
                    Response.Redirect("~/Admin/ViewUserList.aspx?error=no-user");
                }
                dr.Close();
                con.Close();
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            // Validation: Required fields
            if (string.IsNullOrWhiteSpace(txtFullname.Text) || string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtAge.Text) || string.IsNullOrWhiteSpace(rbGender.SelectedValue) ||
                string.IsNullOrWhiteSpace(dlCountry.SelectedValue))
            {
                lblMessage.Text = "⚠️ Please fill all required fields.";
                lblMessage.CssClass = "alert alert-warning";
                return;
            }

            // Age validation
            if (!int.TryParse(txtAge.Text, out int age) || age < 1 || age > 120)
            {
                lblMessage.Text = "⚠️ Invalid age (must be 1-120).";
                lblMessage.CssClass = "alert alert-warning";
                return;
            }

            // Email format
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                lblMessage.Text = "⚠️ Invalid email format.";
                lblMessage.CssClass = "alert alert-warning";
                return;
            }

            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string pictureFileName = null;

            // Handle file upload (optional, validate if present)
            if (FileUploadPic.HasFile)
            {
                if (FileUploadPic.PostedFile.ContentLength > 5 * 1024 * 1024 || !FileUploadPic.PostedFile.ContentType.StartsWith("image/"))
                {
                    lblMessage.Text = "⚠️ Invalid image: <5MB and image type only.";
                    lblMessage.CssClass = "alert alert-warning";
                    return;
                }

                pictureFileName = Guid.NewGuid().ToString() + Path.GetExtension(FileUploadPic.FileName);
                string savePath = Server.MapPath("~/Images/Profile/") + pictureFileName;
                FileUploadPic.SaveAs(savePath);
            }

            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string query = @"UPDATE tblRegisteredUsers 
                                     SET fullName = @fullName, emailAddress = @email, age = @age, 
                                         gender = @gender, country = @country" + (pictureFileName != null ? ", picture = @picture" : "") +
                                             " WHERE RID = @rid";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@fullName", txtFullname.Text.Trim());
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@age", age);
                    cmd.Parameters.AddWithValue("@gender", rbGender.SelectedValue);
                    cmd.Parameters.AddWithValue("@country", dlCountry.SelectedValue);
                    if (pictureFileName != null)
                        cmd.Parameters.AddWithValue("@picture", pictureFileName);
                    cmd.Parameters.AddWithValue("@rid", Request.QueryString["rid"]);

                    con.Open();
                    int rows = cmd.ExecuteNonQuery();
                    con.Close();

                    if (rows > 0)
                    {
                        lblMessage.Text = "✅ User updated successfully!";
                        lblMessage.CssClass = "alert alert-success";
                        // Refresh preview if new image
                        if (pictureFileName != null)
                            imgPreview.ImageUrl = "~/Images/Profile/" + pictureFileName;
                    }
                    else
                    {
                        lblMessage.Text = "❌ No changes made or update failed.";
                        lblMessage.CssClass = "alert alert-warning";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Update failed: " + ex.Message;
                lblMessage.CssClass = "alert alert-danger";
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/ViewUserList.aspx");
        }
    }
}