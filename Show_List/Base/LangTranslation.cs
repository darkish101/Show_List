using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NHibernate.Criterion;
using System.IO;


namespace Show_List.Base
{

    public class LangTranslation
    {
        #region variable
        public readonly NHibernate.ISession session;

        ControlCollection _Control;
        string _ModuleId;
        string _ActivityId;
        string _Language;
        IList<LanguageTranslation> _List;
        #endregion

        #region Constructor
        public LangTranslation(Page page, string module, string language, string isTranlate, ControlCollection control, string strPageName)
        {
            _Language = language;
            _Control = control;

            _ActivityId = strPageName;
            try
            {
                _ActivityId = _ActivityId.Substring(0, _ActivityId.IndexOf(".aspx", System.StringComparison.Ordinal));
            }
            catch (Exception ex)
            {
            }
            _ModuleId = module;
            if (!string.IsNullOrEmpty(isTranlate) && isTranlate == "Y")
                ReadControl(_Control);
            else
            {
                LanguageTranslation objLT = new LanguageTranslation();

                _List = objLT.getTranslationByActivityID(_ModuleId, _ActivityId, _Language);
                TranslateControl(_Control);
            }
        }
        #endregion

        #region for Read Control
        public void ReadControl(ControlCollection control)
        {
            string vLabel = string.Empty;
            IRepository<LanguageTranslation> repoLang = new LanguageTranslationRepository(this.session);
            foreach (Control C in control)
            {
                string vObjectName = string.Empty;
                string vControlType = string.Empty;
                string vDefaultLabel = string.Empty;
                if (C.HasControls())
                    ReadControl(C.Controls);
                if (C is TextBox)
                {
                    var textBox = (TextBox)C;
                    vObjectName = C.ID;
                    vControlType = textBox.GetType().ToString();
                    if (textBox.Attributes["Trans"] != "false")
                        vDefaultLabel = textBox.Attributes["placeholder"];
                }
                else if (C is Label)
                {
                    var Label = (Label)C;
                    vObjectName = C.ID;
                    vControlType = Label.GetType().ToString();
                    if (Label.Attributes["Trans"] != "false")
                        vDefaultLabel = Label.Text;
                }
                else if (C is LinkButton)
                {
                    var linkButton = (LinkButton)C;
                    vObjectName = C.ID;
                    vControlType = linkButton.GetType().ToString();
                    vDefaultLabel = linkButton.Text;
                }
                else if (C is Literal)
                {
                    var literal = (Literal)C;
                    vObjectName = C.ID;
                    vControlType = literal.GetType().ToString();
                    vDefaultLabel = literal.Text;
                }
                //else if (C is LiteralControl)
                //{
                //    var literal = (LiteralControl)C;
                //    vObjectName = C.ID;
                //    vControlType = literal.GetType().ToString();
                //    vDefaultLabel = literal.Text;
                //} 
                else if (C is Button)
                {
                    var button = (Button)C;
                    vObjectName = C.ID;
                    vControlType = button.GetType().ToString();
                    if (button.Attributes["Trans"] != "false")
                        vDefaultLabel = button.Text;
                }
                else if (C is Repeater)
                {
                    var repeater = (Repeater)C;
                    C.Load += new EventHandler(C_RepeterViewLoad);
                    repeater.ItemDataBound += new RepeaterItemEventHandler(repeater_ItemDataBoundReadControl);
                    //foreach(RepeaterItem item in repeater.Items)
                    //{
                    //    if (item.Controls[0] is Literal)
                    //    {
                    //        var literal = (Literal)C;
                    //        vObjectName = C.ID;
                    //        vControlType = literal.GetType().ToString();
                    //        vDefaultLabel = literal.Text;
                    //    }
                    //    if (item.ItemType is Literal)
                    //    {
                    //        var literal = (Literal)C;
                    //        vObjectName = C.ID;
                    //        vControlType = literal.GetType().ToString();
                    //        vDefaultLabel = literal.Text;
                    //    }
                    //}
                }
                else if (C is GridView)
                {
                    var gridView = (GridView)C;
                    C.Load += new EventHandler(C_GridViewLoad);
                    foreach (DataControlField col in gridView.Columns)
                    {
                        if (col is BoundField)
                        {
                            var boundField = (BoundField)col;
                            vObjectName = C.ID + "_BF_" + boundField.DataField;
                            vControlType = boundField.GetType().ToString();
                            vDefaultLabel = boundField.HeaderText;
                        }
                        if (col is TemplateField)
                        {
                            var templateField = (TemplateField)col;
                            if (!string.IsNullOrEmpty(templateField.SortExpression))
                                vObjectName = C.ID + "_TF_" + templateField.SortExpression;
                            vControlType = templateField.GetType().ToString();
                            vDefaultLabel = templateField.HeaderText;
                        }
                        if (vDefaultLabel != "")
                        {
                            var id = new LanguageTranslationIdentifier { Language = _Language, ModuleActivityId = _ActivityId, ModuleId = _ModuleId, ObjectName = vObjectName };
                            LanguageTranslation langSet = null;
                            if (!string.IsNullOrEmpty(vObjectName) && !string.IsNullOrEmpty(vDefaultLabel))
                            {
                                langSet = new LanguageTranslation() { Id = id, DefaultLabel = vDefaultLabel, Label = vDefaultLabel, ControlType = vControlType };
                                langSet.SaveorUpdateLangTranslation(langSet);
                            }
                        }
                    }

                    vDefaultLabel = string.Empty;
                }
                else if (C is DetailsView)
                {
                    var detailsView = (DetailsView)C;
                    C.Load += new EventHandler(C_Load);
                }
                else if (C is RadioButton)
                {
                    var radioButton = (RadioButton)C;
                    vObjectName = C.ID;
                    vControlType = radioButton.GetType().ToString();
                    vDefaultLabel = radioButton.Text;
                }
                else if (C is CheckBox)
                {
                    var checkBox = (CheckBox)C;
                    vObjectName = C.ID;
                    vControlType = checkBox.GetType().ToString();
                    if (checkBox.Attributes["Trans"] != "false")
                        vDefaultLabel = checkBox.Text;
                }
                else if (C is Panel)
                {
                    var panel = (Panel)C;
                    vObjectName = C.ID;
                    vControlType = panel.GetType().ToString();
                    vDefaultLabel = panel.GroupingText;
                }
                else if (C is HiddenField)
                {
                    var hiddenField = (HiddenField)C;
                    vObjectName = C.ID;
                    vControlType = hiddenField.GetType().ToString();
                    vDefaultLabel = hiddenField.Value;
                }
                if (vDefaultLabel != "")
                {
                    var id = new LanguageTranslationIdentifier { Language = _Language, ModuleActivityId = _ActivityId, ModuleId = _ModuleId, ObjectName = vObjectName };
                    LanguageTranslation langSet = null;
                    if (!string.IsNullOrEmpty(vObjectName) && !string.IsNullOrEmpty(vDefaultLabel))
                    {
                        langSet = new LanguageTranslation() { Id = id, DefaultLabel = vDefaultLabel, Label = vDefaultLabel, ControlType = vControlType };
                        langSet.SaveorUpdateLangTranslation(langSet);
                    }
                }
            }
        }

        void C_GridViewLoad(object sender, EventArgs e)
        {
            GridView dv = (GridView)sender;
            if (dv.HasControls())
                ReadControl(dv.Controls);

        }
        void C_RepeterViewLoad(object sender, EventArgs e)
        {
            Repeater dv = (Repeater)sender;
            if (dv.HasControls())
                ReadControl(dv.Controls);

        }

        void C_Load(object sender, EventArgs e)
        {
            DetailsView dv = (DetailsView)sender;
            if (dv.HasControls())
                ReadControl(dv.Controls);

        }

        void repeater_ItemDataBoundReadControl(object sender, RepeaterItemEventArgs e)
        {
            ReadControl(e.Item.Controls);
        }

        #endregion

        #region for Translate Control
        public void TranslateControl(ControlCollection control)
        {
            string vLabel = string.Empty;


            foreach (Control C in control)
            {

                if (C.HasControls())
                    TranslateControl(C.Controls);
                if (C is TextBox)
                {
                    var textBox = (TextBox)C;
                    string vObjectName = C.ID;
                    LanguageTranslation langTrans = (LanguageTranslation)_List.FirstOrDefault(a => a.Id.ObjectName == vObjectName);
                    if (langTrans != null && langTrans.Label != "" && textBox.Attributes["Trans"] != "false")
                        textBox.Attributes["placeholder"] = langTrans.Label;

                }
                else if (C is Label)
                {
                    var Label = (Label)C;
                    string vObjectName = C.ID;
                    var langTrans = (LanguageTranslation)_List.FirstOrDefault(a => a.Id.ObjectName == vObjectName);
                    if (langTrans != null && langTrans.Label != "" && Label.Attributes["Trans"] != "false")
                        Label.Text = langTrans.Label;

                }
                else if (C is Literal)
                {
                    var literal = (Literal)C;
                    string vObjectName = C.ID;
                    var langTrans = (LanguageTranslation)_List.FirstOrDefault(a => a.Id.ObjectName == vObjectName);
                    if (langTrans != null && langTrans.Label != "")
                        literal.Text = langTrans.Label;

                }
                else if (C is LinkButton)
                {
                    var linkButton = (LinkButton)C;
                    string vObjectName = C.ID;
                    var langTrans = (LanguageTranslation)_List.FirstOrDefault(a => a.Id.ObjectName == vObjectName);
                    if (langTrans != null && langTrans.Label != "" && linkButton.Attributes["Trans"] != "false")
                        linkButton.Text = langTrans.Label;

                }
                else if (C is Button)
                {
                    var button = (Button)C;
                    string vObjectName = C.ID;
                    var langTrans = (LanguageTranslation)_List.FirstOrDefault(a => a.Id.ObjectName == vObjectName);
                    if (langTrans != null && langTrans.Label != "" && button.Attributes["Trans"] != "false")
                        button.Text = langTrans.Label;

                }
                else if (C is ImageButton)
                {
                    var button = (ImageButton)C;
                    button.ImageUrl = button.ImageUrl.Replace("{0}", "OK");

                }
                else if (C is System.Web.UI.WebControls.Image)
                {
                    var image = (System.Web.UI.WebControls.Image)C;
                    image.ImageUrl = image.ImageUrl.Replace("{0}", "OK");

                }
                else if (C is HiddenField)
                {
                    var hiddenField = (HiddenField)C;
                    string vObjectName = C.ID;
                    var langTrans = (LanguageTranslation)_List.FirstOrDefault(a => a.Id.ObjectName == vObjectName);
                    if (langTrans != null && langTrans.Label != "")
                        hiddenField.Value = langTrans.Label;

                }
                else if (C is Repeater)
                {
                    var repeater = (Repeater)C;
                    repeater.ItemDataBound += new RepeaterItemEventHandler(repeater_ItemDataBoundTranslateControl);
                }
                else if (C is GridView)
                {
                    var gridView = (GridView)C;
                    C.Load += new EventHandler(C_GridViewLoadTranslateControl);
                    foreach (DataControlField col in gridView.Columns)
                    {
                        if (col is BoundField)
                        {
                            var boundField = (BoundField)col;
                            string vObjectName = C.ID + "_BF_" + boundField.DataField;
                            var langTrans = (LanguageTranslation)_List.FirstOrDefault(a => a.Id.ObjectName == vObjectName);
                            if (langTrans != null && langTrans.Label != "")
                                boundField.HeaderText = langTrans.Label;
                        }
                        if (col is TemplateField)
                        {
                            var templateField = (TemplateField)col;
                            string vObjectName = C.ID + "_TF_" + templateField.SortExpression;
                            var langTrans = (LanguageTranslation)_List.FirstOrDefault(a => a.Id.ObjectName == vObjectName);
                            if (langTrans != null && langTrans.Label != "")
                                templateField.HeaderText = langTrans.Label;
                        }
                    }
                }
                else if (C is DetailsView)
                {
                    var detailsView = (DetailsView)C;
                    C.Load += new EventHandler(C_LoadTranslateControl);

                }
                else if (C is RadioButton)
                {
                    var radioButton = (RadioButton)C;
                    string vObjectName = C.ID;
                    var langTrans = (LanguageTranslation)_List.FirstOrDefault(a => a.Id.ObjectName == vObjectName);
                    if (langTrans != null && langTrans.Label != "")
                        radioButton.Text = langTrans.Label;
                }
                else if (C is CheckBox)
                {
                    var checkBox = (CheckBox)C;
                    string vObjectName = C.ID;
                    var langTrans = (LanguageTranslation)_List.FirstOrDefault(a => a.Id.ObjectName == vObjectName);
                    if (langTrans != null && langTrans.Label != "")
                        checkBox.Text = langTrans.Label;

                }
                else if (C is Panel)
                {
                    var panel = (Panel)C;
                    string vObjectName = C.ID;
                    var langTrans = (LanguageTranslation)_List.FirstOrDefault(a => a.Id.ObjectName == vObjectName);
                    if (langTrans != null && langTrans.Label != "")
                        panel.GroupingText = langTrans.Label;
                }
            }
        }

        void repeater_ItemDataBoundTranslateControl(object sender, RepeaterItemEventArgs e)
        {
            TranslateControl(e.Item.Controls);
        }


        void C_GridViewLoadTranslateControl(object sender, EventArgs e)
        {
            GridView dv = (GridView)sender;
            if (dv.HasControls())
                TranslateControl(dv.Controls);

        }
        void C_LoadTranslateControl(object sender, EventArgs e)
        {
            DetailsView dv = (DetailsView)sender;
            if (dv.HasControls())
                TranslateControl(dv.Controls);

        }

        #endregion
    }

}