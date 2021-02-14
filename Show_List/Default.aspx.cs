using Show_List.BAL;
using Show_List.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Show_List
{
    public partial class Default : PageBaseClass //System.Web.UI.Page
    {
        Languages L = new Languages();
        Shows S = new Shows();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            //if (Session["Lang"] == null)
            //{
            //    Session["Lang"] = "ar-SA";
            //    //btnLang.Text = "Change Language";
            //    Session["ddlSelectLang"] = "الرجاء إختيار";
            //}
            S.Lang = (CommonMethods.GetCookieValue("Site_Language") == null) ? "en-US": CommonMethods.GetCookieValue("Site_Language");
            DataSet DS = S.GetAllShows();
            if (DS.Tables[0].Rows.Count == 0)
            {
                S.Lang = "en-US";
                DS = S.GetAllShows();
            }
            rpShows.DataSource = DS.Tables[0];
            rpShows.DataBind();


        }
    }
}