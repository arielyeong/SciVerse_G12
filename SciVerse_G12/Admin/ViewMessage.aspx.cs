using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SciVerse_G12
{
    public partial class ViewMessage : System.Web.UI.Page
    {
        private const string CHECKED_STATE_KEY = "CheckedStates";
        private Dictionary<string, bool> CheckedStates
        {
            get
            {
                if (ViewState[CHECKED_STATE_KEY] == null)
                    ViewState[CHECKED_STATE_KEY] = new Dictionary<string, bool>();
                return (Dictionary<string, bool>)ViewState[CHECKED_STATE_KEY];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMessages();
            }
        }

        private void BindMessages()
        {
            rptMessages.DataSource = SqlDataSource1;
            rptMessages.DataBind();
        }

        protected void rptMessages_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                CheckBox chkReviewed = (CheckBox)e.Item.FindControl("chkReviewed");
                HiddenField hdnCheckedState = (HiddenField)e.Item.FindControl("hdnCheckedState");
                if (chkReviewed != null && hdnCheckedState != null)
                {
                    string cid = DataBinder.Eval(e.Item.DataItem, "CID").ToString();
                    chkReviewed.Checked = IsChecked(cid);
                    hdnCheckedState.Value = cid + "|" + (chkReviewed.Checked ? "1" : "0");
                }
            }
        }

        protected void chkReviewed_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            RepeaterItem item = (RepeaterItem)chk.NamingContainer;
            HiddenField hdnCheckedState = (HiddenField)item.FindControl("hdnCheckedState");
            string cid = hdnCheckedState.Value.Split('|')[0];
            bool isChecked = chk.Checked;
            CheckedStates[cid] = isChecked;
            hdnCheckedState.Value = cid + "|" + (isChecked ? "1" : "0");
        }

        // Make IsChecked public to be accessible in data-binding expressions
        public bool IsChecked(string cid)
        {
            return CheckedStates.ContainsKey(cid) && CheckedStates[cid];
        }
    }

}