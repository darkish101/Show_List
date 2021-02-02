using Show_List.BAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Show_List
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        Languages L = new Languages();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            if (Session["Lang"] == null)
            {
                Session["Lang"] = "ar-SA";
                //btnLang.Text = "Change Language";
                Session["ddlSelectLang"] = "الرجاء إختيار";
            }
            else
            {
                if (Session["Lang"].ToString() == "ar-SA")
                {
                  //  Session["Lang"] = "ar-SA";
                    //btnLang.Text = "Change Language";
                    //btnLang.Text = "تغيير اللغة";
                    Session["ddlSelectLang"] = "الرجاء إختيار";
                }
                else if (Session["Lang"].ToString() == "en-US")
                {
                  //  Session["Lang"] = "en-US";
                    Session["ddlSelectLang"] = "Select Lang please";
                }
            }
            ddlLang.DataSource = L.GetAlllanguages();
            ddlLang.DataTextField = "Lang_Show";
            ddlLang.DataValueField = "Lang_Code";
            ddlLang.DataBind();
            ddlLang.Items.Insert(0, new ListItem(Session["ddlSelectLang"].ToString(), "0"));

            ddlLang.SelectedValue = Session["Lang"].ToString();
        }
        protected void ddlLang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlLang.SelectedValue == "ar-SA")
            {
                Session["Lang"] = "ar-SA";
                // btnLang.Text = "تغيير اللغة";
                ddlLang.Items.Remove(new ListItem(Session["ddlSelectLang"].ToString(), "0"));

                Session["ddlSelectLang"] = "الرجاء إختيار";
                ddlLang.Items.Insert(0, new ListItem(Session["ddlSelectLang"].ToString(), "0"));
            }
            else if (ddlLang.SelectedValue == "en-US")
            {
                Session["Lang"] = "en-US";
                // btnLang.Text = "Change Language";
                //ddlLang.Items.Remove("0");
                ddlLang.Items.Remove(new ListItem(Session["ddlSelectLang"].ToString(), "0"));
                Session["ddlSelectLang"] = "Select Lang please";
                ddlLang.Items.Insert(0, new ListItem(Session["ddlSelectLang"].ToString(), "0"));

            }
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
        }
    }
}