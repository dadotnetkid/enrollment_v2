using System.Data;
using System.Data.OleDb;

namespace Helpers
{
  
    public class ExcelHelper
    {
        public ExcelHelper(string Filename)
        {
            connectionString = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={Filename};Extended Properties=Excel 8.0;HDR=Yes;";
        }
        public string ConnectionString { get { return connectionString; } set { connectionString = value; } }
        private string connectionString;
        public DataTable ExecuteReader()
        {
            DataTable dt = new DataTable();
            using (OleDbConnection cn = new OleDbConnection(ConnectionString))
            {
                cn.Open();
                string sheetname = cn.GetSchema("Tables").Rows[0]["TABLE_NAME"].ToString();
                var query = $"select * from [{sheetname}]";
                using (OleDbCommand cmd = new OleDbCommand(query, cn))
                {
                    OleDbDataAdapter da = new OleDbDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                }
            }
            return dt;
        }
        public DataTable ExecuteReader(string query)
        {
            DataTable dt = new DataTable();
            using (OleDbConnection cn = new OleDbConnection(ConnectionString))
            {
                cn.Open();
                using (OleDbCommand cmd = new OleDbCommand(query, cn))
                {
                    OleDbDataAdapter da = new OleDbDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                }
            }
            return dt;
        }
    }
}