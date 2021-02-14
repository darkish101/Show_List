using Show_List.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Show_List.BAL
{
   public class Categories
    {
        public string Lang { get; set; }
        public string Cat_ID { get; set; }

        public DataSet GetCategories()
        {
            SqlService sql = new SqlService();
            sql.AddParameter("@Lang", SqlDbType.NVarChar, Lang);
            sql.AddParameter("Cat_ID", SqlDbType.NVarChar, Cat_ID);
            return sql.ExecuteSqlDataSet(@"SELECT * FROM Category_Table C 
INNER JOIN Language_Table LT
ON LT.Lang_ID = C.Cat_Lang
AND LT.Lang_Code = @Lang
WHERE C.Cat_ID IN(SELECT Element FROM func_Split(@Cat_ID, ','))");
        }
    }
}
