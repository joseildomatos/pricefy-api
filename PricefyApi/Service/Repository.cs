using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace PricefyApi.Service
{
    public static class Repository
    {
        public static DataTable GetPaginacao(int numeroPagina, int limitePagina)
        {
            //string strConexao = ConfigurationManager.ConnectionStrings["conPriceFy"].ConnectionString;
            string strConexao = @"Data Source=GTI-15\PRICEFY;Initial Catalog=pricefy;Integrated Security=True";
            //string strConexao = @"Data Source=JAIR\SQLEXPRESS; Initial Catalog=pricefy; Integrated Security=true;";
            using (SqlConnection conn = new SqlConnection(strConexao))
            {
                try
                {
                    conn.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("    select count(*) over() TotalGeral,* ");
                    sb.Append("      from dbo.TbCargaDetalheCSV ");
                    sb.Append("  order by 2,3");
                    sb.Append("    offset " + (numeroPagina - 1) * limitePagina + " ROWS ");
                    sb.Append("fetch next " + limitePagina + " ROWS ONLY ");
                    using (SqlDataAdapter da = new SqlDataAdapter(sb.ToString(), conn))
                    {                       
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        return dt;                        
                    };
                }
                catch (Exception ex)
                {
                    throw new Exception();                     
                };
            }
        }
    }
}