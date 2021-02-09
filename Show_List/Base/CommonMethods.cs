using Newtonsoft.Json;
using Show_List.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Show_List.Base
{
    public class CommonMethods
    {
        private static int _peredit;
        private static int _peradd;
        private static int _perdelete;
        private static int _perview;

        private static bool loginSession;
        public static string DeviceDetails { get; set; }
        public static string Site_Language
        {
            set { HttpContext.Current.Session["Site_Language"] = value; }
            get
            {
                return HttpContext.Current.Session["Site_Language"] == null
                    ? "en-US"
                    : (string)HttpContext.Current.Session["Site_Language"];
                  //  : (string)HttpContext.Current.Session["CurrentCulture"];
            }
        }

        public static bool GetLoginSession()
        {
            return loginSession;
        }





        public static CurrentUserContext UserContext
        {
            set { HttpContext.Current.Session["UserContext"] = value; }
            get
            {
                return HttpContext.Current.Session["UserContext"] == null
                    ? null
                    : (CurrentUserContext)HttpContext.Current.Session["UserContext"];
            }
        }

        public static ThirdPartyUserContext ThirdPartyUserContext
        {
            set { HttpContext.Current.Session["thirdPartyUserContext"] = value; }
            get
            {
                return HttpContext.Current.Session["thirdPartyUserContext"] == null
                    ? null
                    : (ThirdPartyUserContext)HttpContext.Current.Session["thirdPartyUserContext"];
            }
        }
        /// ////////////////////call this method when login or register
        public static void SaveUserContext()
        {
            UserContext = new CurrentUserContext
            {
                //UserLang = User_LANGUAGE,
            };
            //UserID = user.UserID,
            //// UserPWD = txtPassword.Text,
            //UserName = user.UserName,
            //CurrCountryID = user.Curr_Country_ID,
            //CurrCityID = user.Curr_City_ID,
            //NationalityCountryID = user.Nationality_Country_ID,
            //NationalityCountryName = user.NationalityName,
            //DOB = user.DOB,
            //Gender = user.Gender,
            //MobileNo = user.Mobile,
            //Email_ID = user.Email_ID,
            //UserLang = user.User_LANGUAGE,
            //RoleIDsCommaSep = user.RoleIDsCommaSep,
            //TeacherID = user.TeacherID,
            //StudentID = user.StudentID,
            //Email_ID_Verified = user.Email_ID_Verified,

            //FullName_AR = user.FullName_AR,
            //FullName_EN = user.FullName_EN,
            try
            {
                CommonMethods.UserContext.UserLang = GetCookieValue("Site_Language");
            }
            catch { AddCookie("Site_Language", "en-US", DateTime.Now.AddMonths(12));
                CommonMethods.UserContext.UserLang = GetCookieValue("Site_Language"); }
            try
            {
                string pagetitle = "Show List";// GeneralMethod.GetSystemDefaultValue("WP_TITLE_BAR_DESCRIPTION");
                //this.Page.Title = pagetitle == string.Empty ? "وقار" : pagetitle;
                CommonMethods.UserContext.PageTitle = pagetitle;
            }
            catch (Exception ex)
            {

            }

            //if (UserContext.UserID == null)
            //{
            //    UserContext.UserID = user.EMPLOYEE_ID; // CRM User
            //}

            if (string.IsNullOrEmpty(UserContext.UserLang))
            {
                UserContext.UserLang = CommonMethods.GetLanguage();
            }

            var culture = new CultureInfo(UserContext.UserLang);

            UserContext.LangDir = culture.TextInfo.IsRightToLeft ? LangDirection.RTL : LangDirection.LTR;

            Thread.CurrentThread.CurrentUICulture = culture;

            AddCookie("Site_Language", UserContext.UserLang, DateTime.Now.AddMonths(12));
            AddCookie("Site_LangDir", UserContext.LangDir.ToString(), DateTime.Now.AddMonths(1));
            ////var cookiesLangDir = new HttpCookie("ESSLangDir", UserContext.LangDir.ToString());
            ////var cookiesLang = new HttpCookie("ESSLang", UserContext.UserLang);
            ////HttpContext.Current.Response.Cookies.Set(cookiesLangDir);
            ////HttpContext.Current.Response.Cookies.Set(cookiesLang);
        }

        public static void AddCookie(string Key, string Value, DateTime Expires)
        {
            HttpCookie objCookie = new HttpCookie(Key, Value);
            objCookie.Expires = Expires;
            HttpContext.Current.Response.Cookies.Set(objCookie);
        }

        public static HttpCookie GetCookie(string Key)
        {
            return HttpContext.Current.Request.Cookies.Get(Key);
        }

        public static string DataTableToJSONWithJSONNet(DataTable table)
        {
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(table);
            return JSONString;
        }

        public static string GetLanguage()
        {
            if (!string.IsNullOrEmpty(GetCookieValue("Site_Language")))
                return GetCookieValue("Site_Language");
            return GetSiteLanguage();
        }
        public static string GetSiteLanguage()
        {////////////////////////////////////////////////////////////////////////////////////////////////////////////////////and set_system_defaults table
            //var _result = new object();
            //string Query = string.Format("select Default_code_value from set_system_defaults where Default_code='PORTAL_LANGUAGE'");

            //SqlService objSqlService = new SqlService();
            //_result = objSqlService.ExecuteSqlScalar(Query);

            //if (_result != null)
            //    return _result.ToString();
            //else
            //    return string.Empty;
            return "en-US";
        }
        public static string GetCookieValue(string Key)
        {
            HttpCookie objCookie = HttpContext.Current.Request.Cookies.Get(Key);
            if (objCookie == null)
                return null;

            return objCookie.Value;
        }


        public static void EmptyGridFix(GridView pGridView)
        {
            // normally executes after a grid load method

            if (pGridView.Rows.Count == 0 &&
                pGridView.DataSource != null)
            {
                DataTable dt = null;

                // need to clone sources otherwise it will be indirectly adding to 

                // the original source

                var set = pGridView.DataSource as DataSet;
                if (set != null)
                {
                    dt = set.Tables[0].Clone();
                }
                else
                {
                    var table = pGridView.DataSource as DataTable;
                    if (table != null)
                    {
                        dt = table.Clone();
                    }
                }

                if (dt == null)
                {
                    return;
                }

                dt.Rows.Add(dt.NewRow()); // add empty row

                pGridView.DataSource = dt;
                pGridView.DataBind();

                // hide row

                pGridView.Rows[0].Visible = false;
                pGridView.Rows[0].Controls.Clear();
            }

            // normally executes at all postbacks

            if (pGridView.Rows.Count == 1 &&
                pGridView.DataSource == null)
            {
                bool bIsGridEmpty = true;

                // check first row that all cells empty

                for (int i = 0; i < pGridView.Rows[0].Cells.Count; i++)
                {
                    if (pGridView.Rows[0].Cells[i].Text != string.Empty)
                    {
                        bIsGridEmpty = false;
                    }
                }
                // hide row

                if (bIsGridEmpty)
                {
                    pGridView.Rows[0].Visible = false;
                    pGridView.Rows[0].Controls.Clear();
                }
            }
        }

        public static void EmptyRepeaterFix(Repeater pRepeater)
        {
            // normally executes after a grid load method

            if (pRepeater.Items.Count == 0 &&
                pRepeater.DataSource != null)
            {
                DataTable dt = null;

                // need to clone sources otherwise it will be indirectly adding to 

                // the original source


                var set = pRepeater.DataSource as DataSet;
                if (set != null)
                {
                    dt = set.Tables[0].Clone();
                }
                else
                {
                    var table = pRepeater.DataSource as DataTable;
                    if (table != null)
                    {
                        dt = table.Clone();
                    }
                }

                if (dt == null)
                {
                    return;
                }

                dt.Rows.Add(dt.NewRow()); // add empty row

                pRepeater.DataSource = dt;
                pRepeater.DataBind();

                // hide row

                pRepeater.Items[0].Visible = false;
                pRepeater.Items[0].Controls.Clear();
            }

            // normally executes at all postbacks

            if (pRepeater.Items.Count == 1 &&
                pRepeater.DataSource == null)
            {
                bool bIsGridEmpty = true;

                // check first row that all cells empty

                for (int i = 0; i < pRepeater.Items.Count; i++)
                {
                    if (pRepeater.Items[0].ToString() != string.Empty)
                    {
                        bIsGridEmpty = false;
                    }
                }
                // hide row

                if (bIsGridEmpty)
                {
                    pRepeater.Items[0].Visible = false;
                    pRepeater.Items[0].Controls.Clear();
                }
            }
        }

        public static double RoundUp(double pValueToRound, double pAddedValue)
        {
            return (Math.Round(pValueToRound + pAddedValue));
        }

        public static int CountPages(int pNoOfRecords, int pNoOfPages)
        {
            double pages = (double)pNoOfRecords / pNoOfPages;
            if (pNoOfPages == 3)
                return (int)RoundUp(pages, 0.3);
            return (int)RoundUp(pages, 0.5);
        }



        public static void SendEmail(string pEmailAddress, string pSubject, string pBody)
        {
            string mailFrom = System.Configuration.ConfigurationManager.AppSettings["mailFrom"];
            string mailFromPwd = System.Configuration.ConfigurationManager.AppSettings["mailFromPwd"];
            string displayName = System.Configuration.ConfigurationManager.AppSettings["DisplayName"];
            string mailTo = mailFrom;
            string mailSubject = pSubject;
            string mailMessage = pBody;
            string smtpserver = System.Configuration.ConfigurationManager.AppSettings["smtpserver"];
            string port = System.Configuration.ConfigurationManager.AppSettings["port"];

            mailMessage = mailMessage.Replace("@BestRegards",
                System.Configuration.ConfigurationManager.AppSettings["BestRegards"]);

            var mail = new MailMessage();
            mail.To.Add(mailTo);
            mail.From = new MailAddress(mailFrom, displayName);
            mail.Subject = mailSubject;
            mail.Body = mailMessage;
            mail.IsBodyHtml = true;

            var basicAuthenticationInfo = new NetworkCredential(mailFrom, mailFromPwd);

            var smtp = new SmtpClient(smtpserver)
            {
                UseDefaultCredentials = false,
                Port = int.Parse(port),
                EnableSsl = true,
                Credentials = basicAuthenticationInfo
            };

            smtp.Send(mail);
        }




        public static string GetCulture(string pCultureCode)
        {
            var culture = new CultureInfo(pCultureCode);

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            string pLanguageDirection = culture.TextInfo.IsRightToLeft ? "RTL" : "LTR";

            HttpContext.Current.Session["LanguageDirection"] = pLanguageDirection;

            return pLanguageDirection;
        }

        public static void Backpage()
        {
            if (UserContext.PreviousPage != null)
            {
                HttpContext.Current.Response.Redirect(UserContext.PreviousPage.ToString());
            }
        }

        public static void ClearCookies()
        {
            if (HttpContext.Current.Request.Cookies["menuId"] != null)
            {
                var myCookie = new HttpCookie("menuId") { Expires = DateTime.Now.AddDays(-1d) };
                HttpContext.Current.Response.Cookies.Add(myCookie);
            }
        }


        public static string GetArabicNumbersInText(string pEnglishNo)
        {
            //١٢٣٤٥٦٧٨٩٠;
            if (UserContext.UserLang == "ar-SA")
            {
                pEnglishNo = pEnglishNo.Replace("1", "١");
                pEnglishNo = pEnglishNo.Replace("2", "٢");
                pEnglishNo = pEnglishNo.Replace("3", "٣");
                pEnglishNo = pEnglishNo.Replace("4", "٤");
                pEnglishNo = pEnglishNo.Replace("5", "٥");
                pEnglishNo = pEnglishNo.Replace("6", "٦");
                pEnglishNo = pEnglishNo.Replace("7", "٧");
                pEnglishNo = pEnglishNo.Replace("8", "٨");
                pEnglishNo = pEnglishNo.Replace("9", "٩");
                pEnglishNo = pEnglishNo.Replace("0", "٠");
            }
            return pEnglishNo;
        }
        public static string ImageToBase64(System.Drawing.Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        public static System.Drawing.Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0,
              imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
            return image;
        }


        public static byte[] GetImage(int UserID)
        {
            SqlService sql = new SqlService();
            SqlDataReader reader = sql.ExecuteSqlReader("select dbo.fn_Get_Image(" + UserID + ")");
            reader.Read();
            byte[] result = (byte[])reader[0];
            reader.Close();
            sql.Disconnect();
            return result;

        }

        /// -=======-=======-=======-=======-=======-=======-=======-=======-=======-=======-=======-=======-=======-
        /// 
        /// <summary>
        /// will utilize the divMessage on the asp page
        /// </summary>
        /// <param name="divMessage">here you should pass the div</param>
        /// <param name="message">here should be the message text</param>
        /// <param name="messageType">E: Error; S: Success, I: Info, EmptyOrNull: Hide the message</param>
        public static void ShowMessage(HtmlGenericControl divMessage, string message, string messageType, string ltrControlID = "ltrMsg")
        {
            Literal ltrMsg = divMessage.FindControl(ltrControlID) as Literal;
            if (string.IsNullOrEmpty(messageType))
            {
                divMessage.Visible = false;
            }
            else
            {
                divMessage.Visible = true;
                ltrMsg.Text = message;
                divMessage.Attributes.Add("class", divMessage.Attributes["class"].ToString().Replace("alert-danger", ""));
                divMessage.Attributes.Add("class", divMessage.Attributes["class"].ToString().Replace("alert-info", ""));
                divMessage.Attributes.Add("class", divMessage.Attributes["class"].ToString().Replace("alert-success", ""));

                if (messageType == "S")
                    divMessage.Attributes.Add("class", divMessage.Attributes["class"].ToString() + " alert-success");
                else if (messageType == "E")
                    divMessage.Attributes.Add("class", divMessage.Attributes["class"].ToString() + " alert-danger");
                else if (messageType == "W")
                    divMessage.Attributes.Add("class", divMessage.Attributes["class"].ToString() + " alert-warning");
                else
                    divMessage.Attributes.Add("class", divMessage.Attributes["class"].ToString() + " alert-info");
            }
        }

        public static string GetDeviceDetails()
        {
            string deviceDetails;
            try
            {
                deviceDetails = "OS = " + HttpContext.Current.Request.Browser.Platform;
                deviceDetails += ", Browser = " + HttpContext.Current.Request.Browser.Browser;
                deviceDetails += " (" + HttpContext.Current.Request.Browser.Version + ")";
                if (HttpContext.Current.Request.Browser.IsMobileDevice)
                {
                    deviceDetails += ", MobileManufacturer = " + HttpContext.Current.Request.Browser.MobileDeviceManufacturer;
                    deviceDetails += ", MobileModel = " + HttpContext.Current.Request.Browser.MobileDeviceModel;
                }
            }
            catch
            {
                deviceDetails = "";
            }
            return deviceDetails;
        }
        #region Login Methods




        #endregion


        #region General Common Methods

        /// <summary>
        /// Get the object of String and return with TRIM String 
        /// Added by    : Junaid Hassan
        /// Dated       : 2017-01-18
        /// </summary>
        /// <param name="str"></param>
        /// <param name="applyMaxString">YES | NO</param>
        /// <param name="strMaxLength">100</param>
        /// <returns></returns>
        public static string GetHtmlText(object str, string applyMaxString = "YES", string strMaxLength = "100")
        {
            int intMaxLength = 100;
            int.TryParse(strMaxLength, out intMaxLength);
            if (intMaxLength == 0)
            {
                intMaxLength = 100;
            }
            if (str == null)
                str = "";
            string result = str.ToString();
            result = System.Text.RegularExpressions.Regex.Replace(result, "<[^>]*>", "");
            if (applyMaxString == "YES")
            {
                if (result.Length > intMaxLength)
                    result = result.Substring(0, intMaxLength) + "...";
            }
            return result;
        }

        /// <summary>
        /// Call this Method at the start of every post back it will set the Culture to Orignal
        /// As HijriGregDatePicker must have changed the culture to selected Calendar
        /// </summary>
        public static void setPageCulture(string strCultureInfoValue = "en-US")
        {
            System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.CreateSpecificCulture(strCultureInfoValue);
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Remove HTML Tags
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string StripHtmlTags(string source)
        {
            return System.Text.RegularExpressions.Regex.Replace(source, "<.*?>|&.*?;", string.Empty);
        }



        /// <summary>
        ///  Generates the Progress Bar from the DataTable Provided with Proper DataColumn Name.
        /// </summary>
        /// <param name="DT"></param>
        /// <param name="navURL"></param>
        /// <param name="lnkBtnClass"></param>
        /// <returns></returns>
        public static string generateProgressBar(DataTable DT, string navURL = "", string lnkBtnClass = "")
        {
            string[] arrBarType = new string[] { "primary", "success", "info", "warning", "danger" };
            int i = 0;
            StringBuilder strProgBarHTML = new StringBuilder();
            StringBuilder strLinkBtnsHTML = new StringBuilder();
            strProgBarHTML.AppendLine("<div class='progress-striped active progress ng-isolate-scope' ng-transclude='' aria-labelledby=''>");
            if (navURL != "")
            {
                strLinkBtnsHTML.AppendLine("<nav id='navLinkBtn' class='" + lnkBtnClass + "'>");
            }
            foreach (DataRow DR in DT.Rows)
            {
                strProgBarHTML.AppendLine("<div class=\"progress-bar ng-scope ng-isolate-scope progress-bar-" + arrBarType[i] + "\" ng-class=\"type &amp;&amp; 'progress-bar-' + type\" role=\"progressbar\" aria-valuenow=\"" + DR["Data_PERCENT"].ToString() + "\" aria-valuemin='0' aria-valuemax='100' ng-style=\"{width: (percent < 100 ? percent : 100) + '%'}\" aria-valuetext=\"" + DR["Data_PERCENT"].ToString() + "%\" aria-labelledby=\"progressbar\" ng-transclude=\" ng-repeat=\"bar in stacked track by $index\" value=\"bar.value\" type=\"success\" style=\"width: " + DR["Data_PERCENT"].ToString() + "%;\">");
                strProgBarHTML.AppendLine("     <span ng-hide=\"bar.value < 5\" class=\"ng-binding ng-scope\" style=''>" + "[" + DR["Data_PERCENT"].ToString() + "%]-" + DR["ProgressBarTitle"] + "</span>");
                strProgBarHTML.AppendLine("</div>");
                if (navURL != "")
                {
                    strLinkBtnsHTML.AppendLine("<a href='" + DR["LinkBtnNavURL"] + "' target='_Blank'><span class='label label-" + arrBarType[i] + "' data-hover='" + DR["LinkBtnHoverTitle"].ToString() + "'  title='" + DR["LinkBtnHoverTitle"].ToString() + "'>" + DR["LinkBtnTitle"] + "</span></a>");
                }
                i = i + 1;
                if (i >= 4) { i = 0; }
            }
            if (navURL != "")
            {
                strLinkBtnsHTML.AppendLine("</nav>");
            }
            strProgBarHTML.AppendLine("</div>");
            string ProgBarWithNavLinks = strProgBarHTML.ToString() + strLinkBtnsHTML.ToString();
            return ProgBarWithNavLinks;
        }


        #endregion General Common Methods




        #region Pie Chart Method
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="DIVid">Name of DIV to show Chart IN</param>
        /// <param name="ColNameString"></param>
        /// <param name="ColNameCount">Count column name in datatable </param>
        /// <param name="options">{width: 500, height: 375,title: '',is3D: true, }</param>
        /// <returns></returns>
        public static string PieChartScript(DataTable dt, string DIVid, string ColNameString, string ColNameCount, string options = "{width: 500, height: 375,title: '',is3D: true, }")
        {
            // here am taking datatable
            StringBuilder str = new StringBuilder();
            try
            {
                // here datatale dt is fill wit the adp
                //  adp.Fill(dt);

                // this string m catching in the stringbuilder class
                // in the str m writing same javascript code that is given by the google.
                str.AppendLine("");
                str.AppendLine("");
                str.AppendLine("");
                str.AppendLine("");
                str.AppendLine(@"<script type=text/javascript>  
                    
                     google.setOnLoadCallback(drawChart);
                    
                    function drawChart() {
                     var data = new google.visualization.DataTable();");
                // but m changing  only below line
                // (" data.addColumn('string'(datatype), 'student_name'(column name));");
                // str.Append(" data.addColumn('number'(datatype), 'average_marks'(column name));");
                // my data that will come from the sql server
                str.AppendLine(" data.addColumn('string', '" + ColNameString + "');");
                str.AppendLine(" data.addColumn('number', '" + ColNameCount + "');");
                str.AppendLine(" data.addRows([");
                // here i am declairing the variable i in int32 for the looping statement
                Int32 i;
                // loop start from 0 and its end depend on the value inside dt.Rows.Count - 1
                for (i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    // here i am fill the string builder with the value from the database
                    str.AppendLine("[\"" + (dt.Rows[i][ColNameString].ToString().Trim()) + "\"," + dt.Rows[i][ColNameCount].ToString() + "],");
                }


                // other all string is fill according to the javascript code
                str.AppendLine(" ]);");
                str.AppendLine(" var chart = new google.visualization.PieChart(document.getElementById('" + DIVid + "'));");

                //str.Append("var options = { 'title': 'How Much Pizza I Ate Last Night', 'width': 400, 'height': 300 };");

                str.AppendLine("chart.draw(data, " + options + ");     ");
                str.AppendLine("}</script>");
                // here am using literal conrol to display the complete graph

                //con.Close();
            }
            catch (Exception ex)
            {
                str.Clear();
            }
            return str.ToString().TrimEnd(',');
        }
        #endregion Pie Chart Method
    }
}