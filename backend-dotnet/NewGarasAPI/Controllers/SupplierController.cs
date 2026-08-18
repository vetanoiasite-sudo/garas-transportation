using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewGaras.Domain.Models;
using NewGaras.Infrastructure.DBContext;
using NewGaras.Infrastructure.Helper.TenantService;
using NewGaras.Infrastructure.Models.Supplier;

namespace NewGarasAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private GarasTestContext _Context;
        private Helper.Helper _helper;
        static string key;
        private readonly IWebHostEnvironment _host;
        private readonly ITenantService _tenantService;
        private readonly ISupplierService _supplierService;

        public SupplierController(IWebHostEnvironment host, ITenantService tenantService, ISupplierService supplierService)
        {
            _tenantService = tenantService;
            _Context = new GarasTestContext(_tenantService);
            _helper = new Helper.Helper();
            key = "SalesGarasPass";
            _host = host;
            _supplierService = supplierService;
        }

        [HttpPost("AddNewSupplier")]
        public BaseResponseWithId<long> AddNewSupplier(SupplierData request)
        {
            var response = new BaseResponseWithId<long>();
            response.Result = true;
            response.Errors = new List<Error>();

            try
            {
                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors = validation.errors;
                response.Result = validation.result;
                if (response.Result)
                {
                    _supplierService.Validation = validation;
                    var data = _supplierService.AddNewSupplier(request);
                    if (!response.Result)
                    {
                        response.Result = false;
                        response.Errors.AddRange(data.Errors);
                        return response;
                    }
                    response = data;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Result = false;
                Error error = new Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.InnerException != null ? ex.InnerException.Message : ex.Message; ;
                response.Errors.Add(error);

                return response;
            }
        }

        [HttpGet("GetSupplierDataResponse")]
        public async Task<GetSupplierData> GetSupplierDataResponse([FromHeader] long SupplierId)
        {
            var response = new GetSupplierData();
            response.Result = true;
            response.Errors = new List<Error>();

            try
            {
                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors = validation.errors;
                response.Result = validation.result;
                if (response.Result)
                {
                    _supplierService.Validation = validation;
                    var data = await _supplierService.GetSupplierDataResponse(SupplierId);
                    if (!response.Result)
                    {
                        response.Result = false;
                        response.Errors.AddRange(data.Errors);
                        return response;
                    }
                    response = data;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Result = false;
                Error error = new Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.InnerException != null ? ex.InnerException.Message : ex.Message; ;
                response.Errors.Add(error);

                return response;
            }
        }

        [HttpGet("GetSupplierContactPersonsResponse")]
        public GetSupplierContactPersonsData GetSupplierContactPersonsResponse([FromHeader] long? SupplierId)
        {
            var response = new GetSupplierContactPersonsData();
            response.Result = true;
            response.Errors = new List<Error>();

            try
            {
                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors = validation.errors;
                response.Result = validation.result;
                if (response.Result)
                {
                    _supplierService.Validation = validation;
                    var data = _supplierService.GetSupplierContactPersonsResponse(SupplierId);
                    if (!response.Result)
                    {
                        response.Result = false;
                        response.Errors.AddRange(data.Errors);
                        return response;
                    }
                    response = data;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Result = false;
                Error error = new Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.InnerException != null ? ex.InnerException.Message : ex.Message; ;
                response.Errors.Add(error);

                return response;
            }
        }

        [HttpGet("GetSuppliersCards")]
        public SuppliersCardsResponse GetSuppliersCards(GetSuppliersCardsFilters filters)
        {
            var response = new SuppliersCardsResponse();
            response.Result = true;
            response.Errors = new List<Error>();

            try
            {
                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors = validation.errors;
                response.Result = validation.result;
                if (response.Result)
                {
                    _supplierService.Validation = validation;
                    var data = _supplierService.GetSuppliersCards(filters);
                    if (!response.Result)
                    {
                        response.Result = false;
                        response.Errors.AddRange(data.Errors);
                        return response;
                    }
                    response = data;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Result = false;
                Error error = new Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.InnerException != null ? ex.InnerException.Message : ex.Message; ;
                response.Errors.Add(error);

                return response;
            }
        }


        [HttpGet("CheckSupplierExistance")]
        public CheckSupplierExistanceResponse CheckSupplierExistance(CheckSupplierExistanceFilters filters)
        {
            var response = new CheckSupplierExistanceResponse();
            response.Result = true;
            response.Errors = new List<Error>();

            try
            {
                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors = validation.errors;
                response.Result = validation.result;
                if (response.Result)
                {
                    _supplierService.Validation = validation;
                    var data = _supplierService.CheckSupplierExistance(filters);
                    if (!response.Result)
                    {
                        response.Result = false;
                        response.Errors.AddRange(data.Errors);
                        return response;
                    }
                    response = data;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Result = false;
                Error error = new Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.InnerException != null ? ex.InnerException.Message : ex.Message; ;
                response.Errors.Add(error);

                return response;
            }
        }

        [HttpPost("AddSupplierAttachments")]
        public BaseResponseWithId<long> AddSupplierAttachments([FromForm] SupplierAttachmentsData request)
        {
            var response = new BaseResponseWithId<long>();
            response.Result = true;
            response.Errors = new List<Error>();

            try
            {
                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors = validation.errors;
                response.Result = validation.result;
                if (response.Result)
                {
                    _supplierService.Validation = validation;
                    var data = _supplierService.AddSupplierAttachments(request);
                    if (!response.Result)
                    {
                        response.Result = false;
                        response.Errors.AddRange(data.Errors);
                        return response;
                    }
                    response = data;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Result = false;
                Error error = new Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.InnerException != null ? ex.InnerException.Message : ex.Message; ;
                response.Errors.Add(error);

                return response;
            }
        }

        [HttpPost("AddSupplierTaxCard")]
        public async Task<BaseResponseWithId<long>> AddSupplierTaxCard(SupplierTaxCardData request)
        {
            var response = new BaseResponseWithId<long>();
            response.Result = true;
            response.Errors = new List<Error>();

            try
            {
                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors = validation.errors;
                response.Result = validation.result;
                if (response.Result)
                {
                    _supplierService.Validation = validation;
                    var data = await _supplierService.AddSupplierTaxCard(request);
                    if (!response.Result)
                    {
                        response.Result = false;
                        response.Errors.AddRange(data.Errors);
                        return response;
                    }
                    response = data;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Result = false;
                Error error = new Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.InnerException != null ? ex.InnerException.Message : ex.Message; ;
                response.Errors.Add(error);

                return response;
            }
        }

        [HttpPost("AddNewSupplierContacts")]
        public BaseResponseWithId<long> AddNewSupplierContacts(SupplierContactsData request)
        {
            var response = new BaseResponseWithId<long>();
            response.Result = true;
            response.Errors = new List<Error>();

            try
            {
                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors = validation.errors;
                response.Result = validation.result;
                if (response.Result)
                {
                    _supplierService.Validation = validation;
                    var data = _supplierService.AddNewSupplierContacts(request);
                    if (!response.Result)
                    {
                        response.Result = false;
                        response.Errors.AddRange(data.Errors);
                        return response;
                    }
                    response = data;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Result = false;
                Error error = new Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.InnerException != null ? ex.InnerException.Message : ex.Message; ;
                response.Errors.Add(error);

                return response;
            }
        }

        [HttpPost("AddNewSupplierContactPerson")]
        public BaseResponseWithId<long> AddNewSupplierContactPerson(SupplierContactPersonData request)
        {
            var response = new BaseResponseWithId<long>();
            response.Result = true;
            response.Errors = new List<Error>();

            try
            {
                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors = validation.errors;
                response.Result = validation.result;
                if (response.Result)
                {
                    _supplierService.Validation = validation;
                    var data = _supplierService.AddNewSupplierContactPerson(request);
                    if (!response.Result)
                    {
                        response.Result = false;
                        response.Errors.AddRange(data.Errors);
                        return response;
                    }
                    response = data;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Result = false;
                Error error = new Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.InnerException != null ? ex.InnerException.Message : ex.Message; ;
                response.Errors.Add(error);

                return response;
            }
        }

        [HttpPost("AddNewSupplierMobile")]
        public BaseResponseWithId<long> AddNewSupplierMobile(SupplierMobileData request)
        {
            var response = new BaseResponseWithId<long>();
            response.Result = true;
            response.Errors = new List<Error>();

            try
            {
                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors = validation.errors;
                response.Result = validation.result;
                if (response.Result)
                {
                    _supplierService.Validation = validation;
                    var data = _supplierService.AddNewSupplierMobile(request);
                    if (!response.Result)
                    {
                        response.Result = false;
                        response.Errors.AddRange(data.Errors);
                        return response;
                    }
                    response = data;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Result = false;
                Error error = new Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.InnerException != null ? ex.InnerException.Message : ex.Message; ;
                response.Errors.Add(error);

                return response;
            }
        }

        [HttpPost("AddNewSupplierSpeciality")]
        public BaseResponseWithId<long> AddNewSupplierSpeciality(SupplierSpecialityData request)
        {
            var response = new BaseResponseWithId<long>();
            response.Result = true;
            response.Errors = new List<Error>();

            try
            {
                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors = validation.errors;
                response.Result = validation.result;
                if (response.Result)
                {
                    _supplierService.Validation = validation;
                    var data = _supplierService.AddNewSupplierSpeciality(request);
                    if (!response.Result)
                    {
                        response.Result = false;
                        response.Errors.AddRange(data.Errors);
                        return response;
                    }
                    response = data;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Result = false;
                Error error = new Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.InnerException != null ? ex.InnerException.Message : ex.Message; ;
                response.Errors.Add(error);

                return response;
            }
        }

        [HttpPost("AddNewSupplierLandLine")]
        public BaseResponseWithId<long> AddNewSupplierLandLine(SupplierLandLineData request)
        {
            var response = new BaseResponseWithId<long>();
            response.Result = true;
            response.Errors = new List<Error>();

            try
            {
                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors = validation.errors;
                response.Result = validation.result;
                if (response.Result)
                {
                    _supplierService.Validation = validation;
                    var data = _supplierService.AddNewSupplierLandLine(request);
                    if (!response.Result)
                    {
                        response.Result = false;
                        response.Errors.AddRange(data.Errors);
                        return response;
                    }
                    response = data;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Result = false;
                Error error = new Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.InnerException != null ? ex.InnerException.Message : ex.Message; ;
                response.Errors.Add(error);

                return response;
            }
        }

        [HttpPost("AddNewSupplierFax")]
        public BaseResponseWithId<long> AddNewSupplierFax(SupplierFaxData request)
        {
            var response = new BaseResponseWithId<long>();
            response.Result = true;
            response.Errors = new List<Error>();

            try
            {
                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors = validation.errors;
                response.Result = validation.result;
                if (response.Result)
                {
                    _supplierService.Validation = validation;
                    var data = _supplierService.AddNewSupplierFax(request);
                    if (!response.Result)
                    {
                        response.Result = false;
                        response.Errors.AddRange(data.Errors);
                        return response;
                    }
                    response = data;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Result = false;
                Error error = new Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.InnerException != null ? ex.InnerException.Message : ex.Message; ;
                response.Errors.Add(error);

                return response;
            }
        }

        [HttpGet("GetSupplierList")]
        public SelectDDLResponse GetSupplierList([FromHeader] int GovernorateID, [FromHeader] string Import, [FromHeader] string SearchKey )
        {
            var response = new SelectDDLResponse();
            response.Result = true;
            response.Errors = new List<Error>();

            try
            {
                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors = validation.errors;
                response.Result = validation.result;
                if (response.Result)
                {
                    _supplierService.Validation = validation;
                    var data = _supplierService.GetSupplierList(GovernorateID, Import, SearchKey );
                    if (!response.Result)
                    {
                        response.Result = false;
                        response.Errors.AddRange(data.Errors);
                        return response;
                    }
                    response = data;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Result = false;
                Error error = new Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.InnerException != null ? ex.InnerException.Message : ex.Message; ;
                response.Errors.Add(error);

                return response;
            }
        }
    }
}
