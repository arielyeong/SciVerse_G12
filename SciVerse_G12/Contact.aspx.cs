using System;
using System.Web.UI;

namespace SciVerse_G12
{
    public partial class Contact : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblMessageStatus.Text = "";
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    // Insert data into tblContactUs using SqlDataSource
                    SqlDataSource1.Insert();

                    lblMessageStatus.Text = "Thank you! Your message has been sent successfully.";
                    lblMessageStatus.CssClass = "text-success";

                    ClearForm();
                }
                catch (Exception)
                {
                    lblMessageStatus.Text = "Error saving your message. Please try again later.";
                    lblMessageStatus.CssClass = "text-danger";
                }
            }
            else
            {
                lblMessageStatus.Text = "Please fill in all required fields.";
                lblMessageStatus.CssClass = "text-warning";
            }
        }

        private void ClearForm()
        {
            txtName.Text = "";
            txtEmail.Text = "";
            txtMessage.Text = "";
        }
    }
}
