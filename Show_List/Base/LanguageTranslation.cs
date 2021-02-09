using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations.Schema;
using Show_List.DAL;

namespace Show_List.Base
{
    public class LanguageTranslation 
    {
        public virtual LanguageTranslationIdentifier Id { get; set; }

        [Column("LABEL")]
        public virtual string Label { get; set; }

        [Column("DEFAULT_LABEL")]
        public virtual string DefaultLabel { get; set; }

        [Column("CONTROL_TYPE")]
        public virtual string ControlType { get; set; }

        public LanguageTranslation()
        {
            Id = new LanguageTranslationIdentifier();
        }



        public virtual string GetObjectName()
        {
            return "LANGUAGE_TRANSLATION";
        }

        public virtual string GetPrimaryKeyValue()
        {
            return this.Id.ToString();
        }

        public IList<LanguageTranslation> getTranslationByActivityID(string strModuleID, string strActivityID, string strLanguage)
        {
            string strQuery = "select * from LANGUAGE_TRANSLATION WHERE Module_ID = '" + strModuleID + "' AND Activity_ID = '" + strActivityID + "' AND [Language] = '" + strLanguage + "'";

            SqlService sql = new SqlService();
            DataSet DS = sql.ExecuteSqlDataSet(strQuery);

            var LangTransList = DS.Tables[0].AsEnumerable().Select(dataRow => new LanguageTranslation {
                  ControlType = dataRow.Field<string>("Control_Type")
                , DefaultLabel = dataRow.Field<string>("Default_Label")
                , Label = dataRow.Field<string>("Label")
                
                , Id = new LanguageTranslationIdentifier { Language = dataRow.Field<string>("Language"), ModuleActivityId = dataRow.Field<string>("Activity_ID"), ModuleId = dataRow.Field<string>("Module_ID"), ObjectName = dataRow.Field<string>("Object_Name") }


            }).ToList();

            return LangTransList;

        }


        public string SaveorUpdateLangTranslation(LanguageTranslation objT)
        {
            SqlService sql = new SqlService();

            SqlCommand sqlComm = new SqlCommand();
            sqlComm.CommandType = CommandType.StoredProcedure;

            sqlComm.Parameters.Add("@pModule_ID", SqlDbType.VarChar).Value = objT.Id.ModuleId;
            sqlComm.Parameters.Add("@pActivity_ID", SqlDbType.VarChar).Value = objT.Id.ModuleActivityId;
            sqlComm.Parameters.Add("@pOBJECT_NAME", SqlDbType.VarChar).Value = objT.Id.ObjectName;
            sqlComm.Parameters.Add("@pLanguage", SqlDbType.VarChar).Value = objT.Id.Language;
            sqlComm.Parameters.Add("@pCONTROL_TYPE", SqlDbType.VarChar).Value = objT.ControlType;
            sqlComm.Parameters.Add("@pDEFAULT_LABEL", SqlDbType.NVarChar).Value = objT.DefaultLabel;
            sqlComm.Parameters.Add("@pLABEL", SqlDbType.NVarChar).Value = objT.Label;

            sqlComm.CommandText = "[dbo].[sp_SET_Translation]";

            DataSet DS = sql.ExecuteSPDataSet(sqlComm);

            return "OK";

        }

    }

    [Serializable]
    public class LanguageTranslationIdentifier
    {
        public virtual string ModuleId { get; set; }
        public virtual string ModuleActivityId { get; set; }
        public virtual string Language { get; set; }
        public virtual string ObjectName { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var t = obj as LanguageTranslationIdentifier;
            if (t == null)
                return false;
            if (ModuleId.Equals(t.ModuleId) && ModuleActivityId.Equals(t.ModuleActivityId) && Language.Equals(t.Language) && ObjectName.Equals(t.ObjectName))
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            return (ModuleId + "|" + ModuleActivityId + "|" + Language).GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}_{1}", this.ModuleId, this.ModuleActivityId);
        }
    }
}
