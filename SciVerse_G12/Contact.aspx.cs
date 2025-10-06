using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SciVerse_G12
{
    public partial class Contact : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

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
                    lblMessageStatus.Text = "Thank you! Your message has been saved.";
                    lblMessageStatus.CssClass = "text-success";
                    ClearForm();
                }
                catch (Exception ex)
                {
                    lblMessageStatus.Text = "Error saving message. Please try again later.";
                }
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