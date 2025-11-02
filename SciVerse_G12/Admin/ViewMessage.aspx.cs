using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SciVerse_G12
{
    public partial class ViewMessage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadMessages();
        }

        private void LoadMessages()
        {
            DataView dv = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
            rptMessages.DataSource = dv;
            rptMessages.DataBind();

            lblNoMessages.Visible = dv.Count == 0;
        }

        protected void chkReviewed_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            RepeaterItem item = (RepeaterItem)chk.NamingContainer;
            HiddenField hdnCID = (HiddenField)item.FindControl("hdnCID");

            int CID = Convert.ToInt32(hdnCID.Value);
            bool isReviewed = chk.Checked;

            SqlDataSource1.UpdateParameters["isReviewed"].DefaultValue = isReviewed ? "true" : "false";
            SqlDataSource1.UpdateParameters["CID"].DefaultValue = CID.ToString();
            SqlDataSource1.Update();

            LoadMessages();
        }
    }
}
