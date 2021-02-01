using Show_List.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Show_List.BAL
{
    public class Languages
    {
        public DataSet GetAlllanguages()
        {
            SqlService sql = new SqlService();
            DataSet DS = sql.ExecuteSqlDataSet("SELECT Lang_Show, Lang_Code FROM Language_Table");
            return DS;
        }
    }
}
