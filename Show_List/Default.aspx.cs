using Show_List.BAL;
using Show_List.Base;
using System;
using System.Collections.Generic;
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
            if (Session["Lang"] == null)
            {
                Session["Lang"] = "ar-SA";
                //btnLang.Text = "Change Language";
                Session["ddlSelectLang"] = "الرجاء إختيار";
            }
            rpShows.DataSource = S.GetAllShows(Session["Lang"].ToString());
            rpShows.DataBind();


        }
    }
}