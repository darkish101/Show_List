using Show_List.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Show_List.BAL
{
    public class Shows
    {
        //      public DataSet GetAllShows(string Lang = "0")
        //      {
        //          SqlService sql = new SqlService();
        //          sql.AddParameter("@Lang", SqlDbType.NVarChar, Lang);
        //          DataSet DS = sql.ExecuteSqlDataSet(@"SELECT  A.Show_ID
        //, A.Show_Name
        //, ST1.Status_Name AS 'Show Status'
        //, A.Episodes
        //, ST2.Status_Name AS 'Watching Status'
        //, LT2.Lang_Name AS 'Show Language'
        //, LT1.Lang_Show
        //, R.R_Name AS 'Show Rating' 
        //, R.R_Emoji
        //, I.Img_URL
        //, A.Added_Date
        //FROM Show_Table A
        //INNER JOIN Language_Table LT1 ON A.Lang = LT1.Lang_ID 
        //INNER JOIN Language_Table LT2 ON A.Show_Lang = LT2.Lang_ID
        //INNER JOIN Status_Table ST1 ON ST1.Status_ID = A.Show_Status AND ST1.Status_For = 'Show_Status' AND ST1.Status_Lang = A.Lang
        //INNER JOIN Status_Table ST2 ON ST2.Status_ID = A.Show_Watching_Status AND ST2.Status_For = 'Watching_Status' AND ST2.Status_Lang = A.Lang
        //LEFT JOIN Rating_Table R ON A.Show_Rating = R.R_ID AND R_lang = A.Lang
        //LEFT JOIN Image_Table I ON I.Show_ID = A.Show_ID AND I.Show_Poster = 1
        //WHERE (@Lang = '0' OR LT1.Lang_Code = @Lang)");
        //          return DS;
        //      }
        public string Lang { get; set; }
        public string Show_ID { get; set; }
        public DataSet GetAllShows()
        {
            SqlService sql = new SqlService();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Parameters.Add("@Lang", SqlDbType.NVarChar).Value = Lang;
            sqlcomm.CommandText = "[dbo].[sp_Get_Show_By_Language]";
            DataSet DS = sql.ExecuteSPDataSet(sqlcomm);
            return DS;
        }
        public DataSet GetShowByID()
        {
            SqlService sql = new SqlService();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Parameters.Add("@Lang", SqlDbType.NVarChar).Value = Lang;
            sqlcomm.Parameters.Add("@Show_ID", SqlDbType.Int).Value = Show_ID;
            sqlcomm.CommandText = "[dbo].[sp_Get_Show_By_ID]";
            return sql.ExecuteSPDataSet(sqlcomm);
        }
    }
}
