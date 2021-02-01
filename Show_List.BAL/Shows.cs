using Show_List.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Show_List.BAL
{
    public class Shows
    {
        public DataSet GetAllAnimes(string Lang = "0")
        {
            SqlService sql = new SqlService();
            sql.AddParameter("@Lang", SqlDbType.NVarChar, Lang);
            DataSet DS = sql.ExecuteSqlDataSet(@"SELECT  A.Anime_ID
  , A.Anime_Name
  , ST1.Status_Name AS 'Anime Status'
  , A.Episodes
  , ST2.Status_Name AS 'Watching Status'
  , LT2.Lang_Name AS 'Anime Language'
  , LT1.Lang_Show
  , R.R_Name AS 'Anime Rating' 
  , R.R_Emoji
  , I.Img_URL
  , A.Added_Date
  FROM Anime_Table A
  INNER JOIN Language_Table LT1 ON A.Lang = LT1.Lang_ID 
  INNER JOIN Language_Table LT2 ON A.Anime_Lang = LT2.Lang_ID
  INNER JOIN Status_Table ST1 ON ST1.Status_ID = A.Anime_Status AND ST1.Status_For = 'Show_Status' AND ST1.Status_Lang = A.Lang
  INNER JOIN Status_Table ST2 ON ST2.Status_ID = A.Anime_Watching_Status AND ST2.Status_For = 'Watching_Status' AND ST2.Status_Lang = A.Lang
  LEFT JOIN Rating_Table R ON A.Anime_Rating = R.R_ID AND R_lang = A.Lang
  LEFT JOIN Image_Table I ON I.Anime_ID = A.Anime_ID AND I.Anime_Poster = 1
  WHERE (@Lang = '0' OR LT1.Lang_Code = @Lang)");
            return DS;
        }
    }
}
