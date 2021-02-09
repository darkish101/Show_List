
using System;
using System.Threading;
using System.Web.UI;
using System.Web.UI.HtmlControls;
//using WAQAR.Utility;

namespace Show_List.Base
{
    public class PageBaseClass : System.Web.UI.Page
    {

        private HtmlLink _cssLink;
        private HtmlLink _cssLink2;
        public bool isNewDesign = false;

        //protected MySession session;
        public PageBaseClass() { }
        protected void Page_PreInit(object sender, EventArgs e)
        {
            string vLang = string.Empty;
            var Ctl = this.Page.Controls;
            string pageName = Request.FilePath.Substring(1, Request.FilePath.Length - 1);
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////// ?Tran=Y&Lang=ar-SA ///////HOW TO ADD A PAGE TO BE TRANSLATED
            if (!string.IsNullOrEmpty(Request.QueryString["Tran"]) && Request.QueryString["Tran"] == "Y"
                && !string.IsNullOrEmpty(Request.QueryString["Lang"]))
            {
                if (this.Master != null)
                {

                    try
                    {
                        Ctl = this.Master.FindControl("MainContent").Controls; //Minton Controls
                    }
                    catch
                    {
                        Ctl = this.Master.FindControl("ContentPlaceHolder1").Controls; //MobileView Controls
                    }
                }

                var lanTrans = new LangTranslation(this.Page, "Show", Request.QueryString["Lang"], Request.QueryString["Tran"], Ctl, pageName);
            }
            else
            {
                if (this.Master != null)
                    Ctl = this.Master.Controls;
                //  vLang = (CommonMethods.UserContext != null && CommonMethods.UserContext.UserLang != null) ? CommonMethods.UserContext.UserLang : CommonMethods.GetLanguage();
                vLang = (Request.Cookies["Site_Language"] == null) ? "en-US" : CommonMethods.GetCookieValue("Site_Language"); //Request.Cookies["Site_Language"].ToString();

                var lanTrans = new LangTranslation(this.Page, "Show", vLang, "", Ctl, pageName);
            }

            var culture = new System.Globalization.CultureInfo(vLang);
            Thread.CurrentThread.CurrentUICulture = culture;
            string vLanDir = culture.TextInfo.IsRightToLeft ? LangDirection.RTL.ToString() : LangDirection.LTR.ToString();

            _cssLink = new HtmlLink();
            _cssLink.Attributes.Add("rel", "Stylesheet");
            _cssLink.Attributes.Add("type", "text/css");

            _cssLink2 = new HtmlLink();
            _cssLink2.Attributes.Add("rel", "Stylesheet");
            _cssLink2.Attributes.Add("type", "text/css");


            _cssLink.Href = ResolveUrl(vLanDir == LangDirection.RTL.ToString() ? "~/assets/css/bootstrap-rtl.min.css" : "");
            _cssLink2.Href = ResolveUrl(vLanDir == LangDirection.RTL.ToString() ? "~/assets/css/rtl.css" : "");
        }
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                //if (CommonMethods.UserContext.LangDir == LangDirection.RTL)
                //{

                    var myHeaderControl = Page.FindControl("HD1");

                    if (myHeaderControl != null)
                    {

                        var hnd = myHeaderControl.FindControl("hdnTrans");
                        var newhnd = (System.Web.UI.WebControls.HiddenField)hnd;

                        // ////////////////////////// START : New Design //////////////////////////////
                        //var lblWelcome = (System.Web.UI.WebControls.Label)myHeaderControl.FindControl("lblWelcome");
                        //var tempLblWelcome = (System.Web.UI.WebControls.Label)myHeaderControl.FindControl("templblWelcome");

                        //var lblNewMessages = (System.Web.UI.WebControls.Label)myHeaderControl.FindControl("lblNewMessages");
                        //var templblNewMessages = (System.Web.UI.WebControls.Label)myHeaderControl.FindControl("templblNewMessages");

                        //var lnkBtnSettings = (System.Web.UI.WebControls.LinkButton)myHeaderControl.FindControl("lnkBtnSettings");
                        //var templnkBtnSettings = (System.Web.UI.WebControls.LinkButton)myHeaderControl.FindControl("templnkBtnSettings");

                        //var imgBtnLogout = (System.Web.UI.WebControls.LinkButton)myHeaderControl.FindControl("imgBtnLogout");
                        //var tempImgBtnLogout = (System.Web.UI.WebControls.LinkButton)myHeaderControl.FindControl("tempImgBtnLogout");



                        // ////////////////////////// END : New Design //////////////////////////////

                        if (newhnd != null)
                        {
                            newhnd.Value = "1";
                            //if (lblWelcome != null)
                            //{
                            //    lblWelcome.Visible = true;
                            //    tempLblWelcome.Visible = false;
                            //}
                            //if (lblNewMessages != null)
                            //{
                            //    lblNewMessages.Visible = true;
                            //    templblNewMessages.Visible = false;
                            //}

                            //if (lnkBtnSettings != null)
                            //{
                            //    lnkBtnSettings.Visible = true;
                            //    templnkBtnSettings.Visible = false;
                            //}

                            //if (imgBtnLogout != null)
                            //{
                            //    imgBtnLogout.Visible = true;
                            //    tempImgBtnLogout.Visible = false;
                            //}



                        }

                    }
                    //WAQARWeb.Menus objMenu = (WAQARWeb.Menus)Page.FindControl("Menu1");
                    //if (objMenu != null)
                    //{
                    //    var linkBtnHome = (System.Web.UI.WebControls.LinkButton)objMenu.FindControl("SDBtnHome");
                    //    var tempLinkBtnHome = (System.Web.UI.WebControls.LinkButton)objMenu.FindControl("tempLinkBtnHome");

                    //    var lblenLoginWith = objMenu.FindControl("lblenLoginWith") as System.Web.UI.WebControls.Label;
                    //    var lblarrLoginWith = objMenu.FindControl("lblarrLoginWith") as System.Web.UI.WebControls.Label;

                    //    var txtSearchEN = (System.Web.UI.HtmlControls.HtmlInputText)objMenu.FindControl("txtSearchEN");
                    //    var txtSearchAR = (System.Web.UI.HtmlControls.HtmlInputText)objMenu.FindControl("txtSearchAR");

                    //    linkBtnHome.Visible = true;
                    //    tempLinkBtnHome.Visible = false;

                    //    if (lblenLoginWith != null)
                    //    {
                    //        lblenLoginWith.Visible = true;
                    //        lblarrLoginWith.Visible = false;
                    //    }
                    //    if (txtSearchEN != null)
                    //    {
                    //        txtSearchEN.Visible = true;
                    //        txtSearchAR.Visible = false;
                    //    }
                    //}


               // }
            }
            catch
            {/// NEW DESIGN TEST
            }
            //}
            base.OnLoad(e);
        }
        protected override void InitializeCulture()
        {
            UICulture = (CommonMethods.UserContext != null && CommonMethods.UserContext.UserLang != null) ? CommonMethods.UserContext.UserLang : CommonMethods.GetLanguage();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (Page.Master != null)
            {
                if (Master.Page.Controls[0].FindControl("Menu1") != null)
                {
                    Page.Header.Controls.Add(_cssLink);
                    Page.Header.Controls.Add(_cssLink2);
                }
                else if (Master.AppRelativeVirtualPath.ToLower().Contains("blank.master"))
                {
                    Page.Header.Controls.Add(_cssLink);
                    Page.Header.Controls.Add(_cssLink2);
                }
            }

            //if ((WAQARWeb.Menus)Page.FindControl("Menu1") != null)
            //{
            //    Page.Header.Controls.Add(_cssLink);
            //    Page.Header.Controls.Add(_cssLink2);
            //}
            //else if (Request.FilePath.ToString().ToLower().Contains("register") && _cssLink.Href.ToString().ToLower().Contains("rtl"))
            //{
            //    Page.Header.Controls.Add(_cssLink);
            //    Page.Header.Controls.Add(_cssLink2);
            //}
            //else if (Request.FilePath.ToString().ToLower().Contains("login") && _cssLink.Href.ToString().ToLower().Contains("rtl"))
            //{
            //    Page.Header.Controls.Add(_cssLink);
            //    Page.Header.Controls.Add(_cssLink2);
            //}
            //else if (Request.FilePath.ToString().ToLower().Contains("passwordchange") && _cssLink.Href.ToString().ToLower().Contains("rtl"))
            //{
            //    Page.Header.Controls.Add(_cssLink);
            //    Page.Header.Controls.Add(_cssLink2);
            //}
            //else if (Request.FilePath.ToString().ToLower().Contains("forgot") && _cssLink.Href.ToString().ToLower().Contains("rtl"))
            //{
            //    Page.Header.Controls.Add(_cssLink);
            //    Page.Header.Controls.Add(_cssLink2);
            //}
            //else if (Request.FilePath.ToString().ToLower().Contains("cpassword") && _cssLink.Href.ToString().ToLower().Contains("rtl"))
            //{
            //    Page.Header.Controls.Add(_cssLink);
            //    Page.Header.Controls.Add(_cssLink2);
            //}

            //else if (Request.RawUrl.ToString().ToLower().Contains("cpassword.aspx") && _cssLink.Href.ToString().ToLower().Contains("rtl"))
            //{
            //    Page.Header.Controls.Add(_cssLink);
            //    Page.Header.Controls.Add(_cssLink2);
            //}

            //else if (Request.RawUrl.ToString().ToLower().Contains("missingtimedetail.aspx") && _cssLink.Href.ToString().ToLower().Contains("rtl"))
            //{
            //    Page.Header.Controls.Add(_cssLink);
            //    Page.Header.Controls.Add(_cssLink2);
            //}
            //else if (Request.RawUrl.ToString().ToLower().Contains("scorecard.aspx") && _cssLink.Href.ToString().ToLower().Contains("rtl"))
            //{
            //    Page.Header.Controls.Add(_cssLink);
            //    Page.Header.Controls.Add(_cssLink2);
            //}
            //else if (Request.RawUrl.ToString().ToLower().Contains("cormessage.aspx") && _cssLink.Href.ToString().ToLower().Contains("rtl"))
            //{
            //    Page.Header.Controls.Add(_cssLink);
            //    Page.Header.Controls.Add(_cssLink2);
            //}

        }

    }
}
public enum LangDirection
{
    LTR,
    RTL
}