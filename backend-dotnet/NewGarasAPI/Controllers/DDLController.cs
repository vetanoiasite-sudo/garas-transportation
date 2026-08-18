using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewGaras.Infrastructure.DBContext;
using NewGaras.Infrastructure.Helper.TenantService;
using NewGaras.Infrastructure;
using NewGaras.Domain.Models;
using NewGaras.Infrastructure.Models.DDL;
using NewGaras.Infrastructure.Entities;
using NewGaras.Infrastructure.Models;

namespace NewGarasAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DDLController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _host;
        private Helper.Helper _helper;
        private GarasTestContext _Context;
        private readonly ITenantService _tenantService;
        //private readonly HttpClient _httpClient;

        public DDLController(IUnitOfWork unitOfWork, IWebHostEnvironment host, ITenantService tenantService)
        {
            _host = host;
            _unitOfWork = unitOfWork;
            _helper = new Helper.Helper();
            _tenantService = tenantService;
            _Context = new GarasTestContext(_tenantService);

            //---------------------new ----------------------------
            //_httpClient = new HttpClient();
        }

        


      
        [HttpGet("MaritalStatus")]
        public async Task<IActionResult> MaritalStatus()
        {
            BaseResponseWithData<List<MaritalStatus>> Response = new BaseResponseWithData<List<MaritalStatus>>();
            Response.Data=new List<MaritalStatus>();
            Response.Errors=new List<Error>();
            Response.Result=false;

            try
            {
                // without valdate
                Response.Data= _unitOfWork.MaritalStatus.FindAll(x=>x.Id > 0).ToList()  ;
                Response.Result=true;
                return Ok(Response);
            }
            catch(Exception ex)
            {
                Response.Result=false;
                // Response.Errors.Add(new Error { code="E-1" , message=ex.InnerException!=null ? ex.InnerException?.Message : ex.Message });
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException.Message;
                Response.Errors.Add(error);
                return BadRequest(Response);
            }
        }

        [HttpGet("InvoiceType")]
        public async Task<IActionResult> InvoiceType()
        {
            BaseResponseWithData<List<InvoiceType>> Response = new BaseResponseWithData<List<InvoiceType>>();
            Response.Data=new List<InvoiceType>();
            Response.Errors=new List<Error>();
            Response.Result=false;

            try
            {
                // without valdate
                Response.Data=_unitOfWork.InvoiceTypes.FindAll(x => x.Id>0).ToList();
                Response.Result=true;
                return Ok(Response);
            }
            catch(Exception ex)
            {
                Response.Result=false;
                // Response.Errors.Add(new Error { code="E-1" , message=ex.InnerException!=null ? ex.InnerException?.Message : ex.Message });
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException.Message;
                Response.Errors.Add(error);
                return BadRequest(Response);
            }
        }
    }
}
