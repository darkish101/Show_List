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
    public partial class Show_Page : System.Web.UI.Page
    {
        Shows S = new Shows();
        Categories C = new Categories();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            S.Lang = C.Lang = (CommonMethods.GetCookieValue("Site_Language") == null) ? "en-US" : CommonMethods.GetCookieValue("Site_Language");
            S.Show_ID = Request.QueryString["Show_Id"];
            DataTable DT = S.GetShowByID().Tables[0];
            C.Cat_ID = DT.Rows[0]["Show_Category"].ToString();


            rpCategories.DataSource = C.GetCategories();
            rpCategories.DataBind();
            h2ShowTitle.InnerText = DT.Rows[0]["Show_Name"].ToString();
            pDescription.InnerText = DT.Rows[0]["Show_Description"].ToString();

        }
    }
}