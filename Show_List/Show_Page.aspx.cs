using Show_List.BAL;
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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            DataTable DT = S.GetShowByID().Tables[0];
            h2ShowTitle.InnerText = DT.Rows[0]["Show_Name"].ToString();
        }
    }
}