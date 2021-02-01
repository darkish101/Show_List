using Show_List.BAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Show_List
{
    public partial class Default : System.Web.UI.Page
    {
        Languages L = new Languages();
        Shows S = new Shows();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            ddlLang.DataSource = L.GetAlllanguages();
            ddlLang.DataTextField = "Lang_Show";
            ddlLang.DataValueField = "Lang_Code";
            ddlLang.DataBind();
            ddlLang.Items.Insert(0, new ListItem("Select Lang please", "0"));

            rpAnimes.DataSource = S.GetAllAnimes();
            rpAnimes.DataBind();

            if (Session["Lang"] == null)
            {
                Session["Lang"] = "ar-SA";
                btnLang.Text = "Change Language";
            }
            else
            {
                if (Session["Lang"].ToString() == "ar-SA")
                {
                    //Session["Lang"] = "en-US";
                    btnLang.Text = "Change Language";
                }
                else
                {
                    //Session["Lang"] = "ar-SA";
                    btnLang.Text = "تغيير اللغة";
                }
            }
        }

        protected void ddlLang_SelectedIndexChanged(object sender, EventArgs e)
        {
            rpAnimes.DataSource = S.GetAllAnimes(ddlLang.SelectedValue);
            rpAnimes.DataBind();
        }

        protected void btnLang_Click(object sender, EventArgs e)
        {
            if (Session["Lang"] == null)
            {
                Session["Lang"] = "ar-SA";
                btnLang.Text = "Change Language";
            }
            else
            {
                if (Session["Lang"].ToString() == "ar-SA")
                {
                    Session["Lang"] = "en-US";
                    btnLang.Text = "تغيير اللغة";
                }
                else
                {
                    Session["Lang"] = "ar-SA";
                    btnLang.Text = "Change Language";
                }
            }
            rpAnimes.DataSource = S.GetAllAnimes(Session["Lang"].ToString());
            rpAnimes.DataBind();
        }
    }
}