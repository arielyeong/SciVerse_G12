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
        // Use ViewState to persist values between postbacks
        private string OriginalEmail
        {
            get { return ViewState["OriginalEmail"] as string ?? string.Empty; }
            set { ViewState["OriginalEmail"] = value; }
        }

        private string OriginalUsername
        {
            get { return ViewState["OriginalUsername"] as string ?? string.Empty; }
            set { ViewState["OriginalUsername"] = value; }
        }

        private int CurrentRid
        {
            get { return ViewState["CurrentRid"] as int? ?? 0; }
            set { ViewState["CurrentRid"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["rid"] != null && int.TryParse(Request.QueryString["rid"], out int rid))
                {
                    CurrentRid = rid; // Store in ViewState
                    LoadUserDetails(rid);
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

            // ** Track if fields have been changed ** - Use StringComparison for accuracy
            bool isEmailChanged = !string.Equals(OriginalEmail, txtEmail.Text.Trim(), StringComparison.OrdinalIgnoreCase);
            bool isUsernameChanged = !string.Equals(OriginalUsername, txtUsername.Text.Trim(), StringComparison.OrdinalIgnoreCase);

            // Required field validations
            if (string.IsNullOrWhiteSpace(txtFullname.Text))
            {
                ShowMessage("Full name is required.", false);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                ShowMessage("Email is required.", false);
                return;
            }

            string trimmedEmail = txtEmail.Text.Trim();
            if (!Regex.IsMatch(trimmedEmail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                ShowMessage("Please enter a valid email address.", false);
                return;
            }

            // ** Check for duplicate email if changed **
            if (isEmailChanged && IsDuplicate("emailAddress", trimmedEmail))
            {
                ShowMessage("Email already registered. Please use another.", false);
                return;
            }

            string trimmedUsername = txtUsername.Text.Trim();

            // ** Check for duplicate username if changed **
            if (isUsernameChanged && IsDuplicate("username", trimmedUsername))
            {
                ShowMessage("Username already taken. Please use another.", false);
                return;
            }

            // Age Validation
            if (string.IsNullOrWhiteSpace(txtAge.Text) || !int.TryParse(txtAge.Text, out int age) || age < 1 || age > 90)
            {
                ShowMessage("Please enter a valid age (1-90).", false);
                return;
            }

            // Gender Validation
            if (string.IsNullOrWhiteSpace(rbGender.SelectedValue))
            {
                ShowMessage("Gender is required.", false);
                return;
            }

            // Country Validation
            if (string.IsNullOrWhiteSpace(dlCountry.SelectedValue))
            {
                ShowMessage("Country is required.", false);
                return;
            }

            // Handle image upload
            string imagePath = null;
            if (FileUploadPic.HasFile)
            {
                if (FileUploadPic.PostedFile.ContentLength > 5 * 1024 * 1024 ||
                    !FileUploadPic.PostedFile.ContentType.StartsWith("image/"))
                {
                    ShowMessage("Image must be <5MB and a valid image type.", false);
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
                    ShowMessage("Image upload failed: " + ex.Message, false);
                    return;
                }
            }

            // Update database
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = imagePath != null
                    ? "UPDATE tblRegisteredUsers SET username=@username, fullName=@fullName, emailAddress=@email, age=@age, gender=@gender, country=@country, picture=@picture WHERE RID=@rid"
                    : "UPDATE tblRegisteredUsers SET username=@username, fullName=@fullName, emailAddress=@email, age=@age, gender=@gender, country=@country WHERE RID=@rid";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@username", trimmedUsername);
                    cmd.Parameters.AddWithValue("@fullName", txtFullname.Text.Trim());
                    cmd.Parameters.AddWithValue("@email", trimmedEmail);
                    cmd.Parameters.AddWithValue("@age", age);
                    cmd.Parameters.AddWithValue("@gender", rbGender.SelectedValue);
                    cmd.Parameters.AddWithValue("@country", dlCountry.SelectedValue);
                    cmd.Parameters.AddWithValue("@rid", CurrentRid);

                    if (imagePath != null)
                        cmd.Parameters.AddWithValue("@picture", imagePath);

                    con.Open();
                    int rows = cmd.ExecuteNonQuery();
                    con.Close();

                    if (rows > 0)
                    {
                        ShowMessage("Profile updated successfully!", true);
                        if (imagePath != null) imgPreview.ImageUrl = imagePath;

                        // Update the stored original values
                        OriginalEmail = trimmedEmail;
                        OriginalUsername = trimmedUsername;

                        string script = "setTimeout(function(){ window.location.href='" + ResolveUrl("~/Admin/ViewUserList.aspx") + "'; }, 2000);";
                        ScriptManager.RegisterStartupScript(this, GetType(), "Redirect", script, true);
                    }
                    else
                    {
                        ShowMessage("No changes were made or update failed.", false);
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

                    // Store original values in ViewState
                    OriginalEmail = dr["emailAddress"].ToString().Trim();
                    OriginalUsername = dr["username"].ToString().Trim();

                    txtEmail.Text = OriginalEmail;
                    txtUsername.Text = OriginalUsername;
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
                    ShowMessage("User not found.", false);
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
                    cmd.Parameters.AddWithValue("@value", value.Trim());
                    cmd.Parameters.AddWithValue("@currentRid", CurrentRid);

                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    conn.Close();

                    return count > 0;
                }
            }
        }

        private void ShowMessage(string message, bool success)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = success
                ? "update-message text-center fw-bold mt-3 text-success"
                : "update-message text-center fw-bold mt-3 text-danger";
            lblMessage.Visible = true;
        }
    }
}