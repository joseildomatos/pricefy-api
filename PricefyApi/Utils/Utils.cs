using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PricefyApi.Utils
{
    public enum enStatusResult { BadRequest, Ok };
    public class ClsUrlPagininacaoModel
    {
        public double NumeroPagina { get; set; }
        public double LimitePagina { get; set; }        
    }
    public class ClsResponseModel
    {
        public int TotalRegistros { get; set; }
        public int QtdePaginas { get; set; }
        public int TotalRecuperados { get; set; }
        public string PaginaAnterior { get; set; }
        public string ProximaPagina { get; set; }         
        public DataTable Data { get; set; }
        public ClsResponseModel(int TotalRegistros, int QtdePaginas, int TotalRecuperados, string PaginaAnterior, string ProximaPagina, DataTable Data = null)
        {
            this.TotalRegistros = TotalRegistros;
            this.QtdePaginas = QtdePaginas;
            this.TotalRecuperados = TotalRecuperados;
            this.PaginaAnterior = PaginaAnterior;
            this.ProximaPagina = ProximaPagina;           
            this.Data = Data;
        }
    }
    public static class Util
    {
        public static object ResponseResultApi(enStatusResult enSatus, ClsResponseModel response)
        {
            var request = new
            {
                TotalRegistros = enSatus == enStatusResult.BadRequest ? 0 : response.TotalRegistros,
                QtdePaginas    = enSatus == enStatusResult.BadRequest ? 0 : response.QtdePaginas,
                PaginaAnterior = enSatus == enStatusResult.BadRequest ? "" : response.PaginaAnterior,
                ProximaPagina  = enSatus == enStatusResult.BadRequest ? "" : response.ProximaPagina,
                status = new
                {
                    Code                     = enSatus == enStatusResult.BadRequest ? 400 : 200,
                    Status                   = enSatus == enStatusResult.BadRequest ? "Cancel" : "Sucesso",
                    QtdeRegistrosRecuperados = enSatus == enStatusResult.BadRequest ? 0 : response.TotalRecuperados,
                    Menssagem                = enSatus == enStatusResult.BadRequest
                                                       ? "Resgistros não recuperados, não encontados na página especificada, inconsistentes ou URL inválida"
                                                       : response.TotalRegistros == 0 ? "Resgistros não recuperados ou não encontados na página especificada" : "Registros recuperados"
                },
                Data = response.Data == null ? null : ConverteDataSetParaJSON(response.Data)
            };
            return SerializacaoObjeto(request);
        }
        public static List<Dictionary<string, object>> ConverteDataSetParaJSON(DataTable dt)
        {
            List<Dictionary<string, object>> lst = new List<Dictionary<string, object>>();
            Dictionary<string, object> item;
            foreach (DataRow row in dt.Rows)
            {
                item = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                    item.Add(col.ColumnName, (Convert.IsDBNull(row[col]) ? null : row[col]));

                lst.Add(item);
            }
            return lst;
        }
        public static string SerializacaoObjeto(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }
    }     
}
