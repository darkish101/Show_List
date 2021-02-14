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
    public partial class Site1 : System.Web.UI.MasterPage
    {
        Languages L = new Languages();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            //if (Request.Cookies["Site_Language"] == null)
            //{
            //    string vLang = (Request.Cookies["Site_Language"] == null) ? "en-US" : Request.Cookies["UserLang"].ToString();
            //    CommonMethods.AddCookie("Site_Language", vLang, DateTime.Now.AddMonths(12));
            //}
            //if (Session["Site_Language"] == null)
            //{
            //    Session["Site_Language"] = "ar-SA";
            //    //btnLang.Text = "Change Language";
            //    Session["ddlSelectLang"] = "الرجاء إختيار";
            //}
            //else
            //{
            //    if (Session["Site_Language"].ToString() == "ar-SA")
            //    {
            //      //  Session["Site_Language"] = "ar-SA";
            //        //btnLang.Text = "Change Language";
            //        //btnLang.Text = "تغيير اللغة";
            //        Session["ddlSelectLang"] = "الرجاء إختيار";
            //    }
            //    else if (Session["Site_Language"].ToString() == "en-US")
            //    {
            //      //  Session["Site_Language"] = "en-US";
            //        Session["ddlSelectLang"] = "Select Lang please";
            //    }
            //}
            ddlLang.DataSource = L.GetAlllanguages();
            ddlLang.DataTextField = "Lang_Show";
            ddlLang.DataValueField = "Lang_Code";
            ddlLang.DataBind();
            //ddlLang.Items.Insert(0, new ListItem(Session["ddlSelectLang"].ToString(), "0"));

            ddlLang.SelectedValue = CommonMethods.GetCookieValue("Site_Language");
        }
        protected void ddlLang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlLang.SelectedValue == "ar-SA")
            {
                CommonMethods.AddCookie("Site_Language", ddlLang.SelectedValue, DateTime.Now.AddMonths(12));

                //Session["Site_Language"] = "ar-SA";
                //// btnLang.Text = "تغيير اللغة";
                //ddlLang.Items.Remove(new ListItem(Session["ddlSelectLang"].ToString(), "0"));

                //Session["ddlSelectLang"] = "الرجاء إختيار";
                //ddlLang.Items.Insert(0, new ListItem(Session["ddlSelectLang"].ToString(), "0"));
            }
            else if (ddlLang.SelectedValue == "en-US")
            {
                CommonMethods.AddCookie("Site_Language", ddlLang.SelectedValue, DateTime.Now.AddMonths(12));

                //Session["Site_Language"] = "en-US";
                //// btnLang.Text = "Change Language";
                ////ddlLang.Items.Remove("0");
                //ddlLang.Items.Remove(new ListItem(Session["ddlSelectLang"].ToString(), "0"));
                //Session["ddlSelectLang"] = "Select Lang please";
                //ddlLang.Items.Insert(0, new ListItem(Session["ddlSelectLang"].ToString(), "0"));

            } else if (ddlLang.SelectedValue == "ja-JP")
                CommonMethods.AddCookie("Site_Language", ddlLang.SelectedValue, DateTime.Now.AddMonths(12));

            else if (ddlLang.SelectedValue == "zh-TW")
                CommonMethods.AddCookie("Site_Language", ddlLang.SelectedValue, DateTime.Now.AddMonths(12));

             else if (ddlLang.SelectedValue == "fr-FR")
                CommonMethods.AddCookie("Site_Language", ddlLang.SelectedValue, DateTime.Now.AddMonths(12));
        }
        //protected void btnLang_Click(object sender, EventArgs e)
        //{
        //    if (Session["Site_Language"] == null)
        //    {
        //        Session["Site_Language"] = "ar-SA";
        //        btnLang.Text = "Change Language";
        //    }
        //    else
        //    {
        //        if (Session["Site_Language"].ToString() == "ar-SA")
        //        {
        //            Session["Site_Language"] = "en-US";
        //            btnLang.Text = "تغيير اللغة";
        //        }
        //        else
        //        {
        //            Session["Site_Language"] = "ar-SA";
        //            btnLang.Text = "Change Language";
        //        }
        //    }
        //}
    }
}