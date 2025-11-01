using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace SciVerse_G12
{
    public partial class EditUserInfo : System.Web.UI.Page
    {
        private string originalEmail;
        private string originalUsername;
        private int currentRid;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["rid"] != null && int.TryParse(Request.QueryString["rid"], out int rid))
                {
                    currentRid = rid; // Save RID of the user being edited
                    LoadUserDetails(currentRid);
                }
                else
                {
                    Response.Redirect("~/Admin/ViewUserList.aspx");
                }
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            // Track if fields have been changed
            bool isEmailChanged = originalEmail != txtEmail.Text.Trim();
            bool isUsernameChanged = originalUsername != txtUsername.Text.Trim();

            // *** Validation: Check Empty/Required Fields ***
            if (string.IsNullOrWhiteSpace(txtFullname.Text))
            {
                lblMessage.Text = "Full name is required.";
                lblMessage.CssClass = "update-message text-center fw-bold mt-3 text-danger";
                return;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                lblMessage.Text = "Email is required.";
                lblMessage.CssClass = "update-message text-center fw-bold mt-3 text-danger";
                return;
            }

            // Email Format Validation
            if (!Regex.IsMatch(txtEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                lblMessage.Text = "Please enter a valid email address.";
                lblMessage.CssClass = "update-message text-center fw-bold mt-3 text-danger";
                return;
            }

            // Check for duplicate email if changed
            if (isEmailChanged && IsDuplicate("emailAddress", txtEmail.Text.Trim()))
            {
                lblMessage.Text = "Email already registered. Please use another.";
                lblMessage.CssClass = "update-message text-center fw-bold mt-3 text-danger";
                return;
            }

            // Check for duplicate username if changed
            if (isUsernameChanged && IsDuplicate("username", txtUsername.Text.Trim()))
            {
                lblMessage.Text = "Username already taken. Please use another.";
                lblMessage.CssClass = "update-message text-center fw-bold mt-3 text-danger";
                return;
            }

            // Age Validation
            if (string.IsNullOrWhiteSpace(txtAge.Text) || !int.TryParse(txtAge.Text, out int age) || age < 1 || age > 90)
            {
                lblMessage.Text = "Please enter a valid age (1-90).";
                lblMessage.CssClass = "update-message text-center fw-bold mt-3 text-danger";
                return;
            }

            // Gender Validation
            if (string.IsNullOrWhiteSpace(rbGender.SelectedValue))
            {
                lblMessage.Text = "Gender is required.";
                lblMessage.CssClass = "update-message text-center fw-bold mt-3 text-danger";
                return;
            }

            // Country Validation
            if (string.IsNullOrWhiteSpace(dlCountry.SelectedValue))
            {
                lblMessage.Text = "Country is required.";
                lblMessage.CssClass = "update-message text-center fw-bold mt-3 text-danger";
                return;
            }

            // Handle image upload (if any)
            string imagePath = null;
            if (FileUploadPic.HasFile)
            {
                if (FileUploadPic.PostedFile.ContentLength > 5 * 1024 * 1024 || !FileUploadPic.PostedFile.ContentType.StartsWith("image/"))
                {
                    lblMessage.Text = "Image must be <5MB and a valid image type.";
                    lblMessage.CssClass = "update-message text-center fw-bold mt-3 text-danger";
                    return;
                }

                try
                {
                    string folderPath = Server.MapPath("~/Images/Profile/");
                    if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                    string uniqueFileName = Guid.NewGuid() + Path.GetExtension(FileUploadPic.FileName);
                    string fullPath = Path.Combine(folderPath, uniqueFileName);
                    FileUploadPic.SaveAs(fullPath);
                    imagePath = "~/Images/Profile/" + uniqueFileName;
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Image upload failed: " + ex.Message;
                    lblMessage.CssClass = "update-message text-center fw-bold mt-3 text-danger";
                    return;
                }
            }

            // Proceed to update user information
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = imagePath != null
                    ? "UPDATE tblRegisteredUsers SET fullName=@fullName, emailAddress=@email, age=@age, gender=@gender, country=@country, picture=@picture WHERE RID=@rid"
                    : "UPDATE tblRegisteredUsers SET fullName=@fullName, emailAddress=@email, age=@age, gender=@gender, country=@country WHERE RID=@rid";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@fullName", txtFullname.Text.Trim());
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@age", age);
                    cmd.Parameters.AddWithValue("@gender", rbGender.SelectedValue);
                    cmd.Parameters.AddWithValue("@country", dlCountry.SelectedValue);
                    cmd.Parameters.AddWithValue("@rid", currentRid);

                    if (imagePath != null)
                        cmd.Parameters.AddWithValue("@picture", imagePath);

                    con.Open();
                    int rows = cmd.ExecuteNonQuery();
                    con.Close();

                    if (rows > 0)
                    {
                        lblMessage.Text = "Profile updated successfully!";
                        lblMessage.CssClass = "update-message text-center fw-bold mt-3 text-success";

                        if (imagePath != null) imgPreview.ImageUrl = imagePath;

                        string script = "setTimeout(function(){ window.location.href='" + ResolveUrl("~/Admin/ViewUserList.aspx") + "'; }, 2000);";
                        ScriptManager.RegisterStartupScript(this, GetType(), "Redirect", script, true);
                    }
                    else
                    {
                        lblMessage.Text = "No changes were made or update failed.";
                        lblMessage.CssClass = "update-message text-center fw-bold mt-3 text-danger";
                    }
                }
            }
        }


        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/ViewUserList.aspx");
        }

        private void LoadUserDetails(int rid)
        {
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT fullName, emailAddress, username, age, gender, country, picture FROM tblRegisteredUsers WHERE RID=@rid";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@rid", rid);
                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    txtFullname.Text = dr["fullName"].ToString().Trim();
                    originalEmail = dr["emailAddress"].ToString().Trim();
                    txtEmail.Text = originalEmail;
                    originalUsername = dr["username"].ToString().Trim();
                    txtUsername.Text = originalUsername;
                    txtAge.Text = dr["age"].ToString();

                    string gender = dr["gender"].ToString().Trim();
                    if (rbGender.Items.FindByValue(gender) != null)
                        rbGender.SelectedValue = gender;

                    string country = dr["country"].ToString().Trim();
                    if (dlCountry.Items.FindByValue(country) != null)
                        dlCountry.SelectedValue = country;

                    string picture = dr["picture"] != DBNull.Value ? dr["picture"].ToString() : null;
                    imgPreview.ImageUrl = !string.IsNullOrEmpty(picture) ? picture : "~/Images/Profile/default.png";
                }
                else
                {
                    lblMessage.Text = "User not found.";
                    lblMessage.CssClass = "update-message text-center fw-bold mt-3 text-danger";
                    Response.Redirect("~/Admin/ViewUserList.aspx");
                }
                con.Close();
            }
        }

        private bool IsDuplicate(string columnName, string value)
        {
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string query = $"SELECT COUNT(*) FROM tblRegisteredUsers WHERE [{columnName}] = @value AND RID != @currentRid";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@value", value.Trim()); // Trim input
                    cmd.Parameters.AddWithValue("@currentRid", currentRid);

                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    conn.Close();

                    return count > 0;
                }
            }
        }
    }
}
