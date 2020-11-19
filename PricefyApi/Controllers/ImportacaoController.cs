using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using PricefyApi.Service;
using System.Data;
using Microsoft.AspNetCore.Routing;

namespace PricefyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]/{:id?}")]
    public class ImportacaoController : ControllerBase
    {
        //https://localhost:5001/api/Importacao/paginacao?numeroPagina=1&LimitePagina=20
        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> Paginacao([FromQuery] Utils.ClsUrlPagininacaoModel pgModel)
        {
            dynamic responseModel, responseResult;
            try
            {
                if (pgModel.LimitePagina == 0 || pgModel.NumeroPagina == 0)
                {
                    responseModel  = new Utils.ClsResponseModel(0, 0, 0, "", "");
                    responseResult = Utils.Util.ResponseResultApi(Utils.enStatusResult.BadRequest, responseModel);
                    return Content(responseResult, "application/json");
                }

                DataTable dbResult = await Task.Run(() => Repository.GetPaginacao((int)pgModel.NumeroPagina, (int)pgModel.LimitePagina));

                var totalRegistros     = dbResult.Rows.Count > 0 ? (int)Convert.ToDouble(dbResult.Rows[0]["TotalGeral"]) : 0;
                var qtdPaginas         = (int)Math.Ceiling((double)totalRegistros / pgModel.LimitePagina);
                var anteriorPaginaLink = totalRegistros == 0 ? "" : pgModel.LimitePagina == 1 || pgModel.NumeroPagina == 1 ? "" : this.Url.Action("Paginacao", null, new { NumeroPagina = pgModel.NumeroPagina - 1, pgModel.LimitePagina });
                var proximaPaginaLink  = totalRegistros == 0 ? "" : pgModel.LimitePagina >= qtdPaginas ? "" : this.Url.Action("Paginacao", null, new { NumeroPagina = pgModel.NumeroPagina + 1, LimitePagina = pgModel.LimitePagina });

                dbResult.Columns.Remove("TotalGeral");
                responseModel = new Utils.ClsResponseModel(
                    totalRegistros,
                    qtdPaginas,
                    dbResult.Rows.Count,
                    anteriorPaginaLink,
                    proximaPaginaLink,
                    dbResult
                    );
                responseResult = Utils.Util.ResponseResultApi(Utils.enStatusResult.Ok, responseModel);
                return Content(responseResult, "application/json");
            }
            catch (Exception ex)
            {
                responseModel = new Utils.ClsResponseModel(0, 0, 0, "", "");
                responseResult = Utils.Util.ResponseResultApi(Utils.enStatusResult.BadRequest, responseModel);
                return Content(responseResult, "application/json");                
            }
        }
    }
}
