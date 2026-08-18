using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using NewGaras.Domain.Models;
using NewGaras.Domain.Services.TransportationLineService;
using NewGaras.Infrastructure;
using NewGaras.Infrastructure.DBContext;
using NewGaras.Infrastructure.Entities;
using NewGaras.Infrastructure.Helper;
using NewGaras.Infrastructure.Interfaces.ServicesInterfaces.TransportationLine;

//using NewGaras.Infrastructure.Interfaces.ServicesInterfaces.TransportationLine;
using NewGaras.Infrastructure.Models.Supplier;
using NewGaras.Infrastructure.Models.TransportationLineModel;
using NewGarasAPI.Helper;
using NewGarasAPI.Models.User;
using Org.BouncyCastle.Utilities.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NewGaras.Domain.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _host;
        private GarasTestContext _Context;
        private readonly IUserService _userService;
        private readonly ITransportationLineService _transportationLineService;
        static readonly string key = "SalesGarasPass";
        private HearderVaidatorOutput validation;

        public HearderVaidatorOutput Validation
        {
            get
            {
                return validation;
            }
            set
            {
                validation = value;
            }
        }
        public SupplierService(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment host, GarasTestContext context, IUserService userService , ITransportationLineService transportationLineService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _host = host;
            _Context = context;
            _userService = userService;
            _transportationLineService = transportationLineService;
        }

        public BaseResponseWithId<long> AddNewSupplier(SupplierData Request)
        {
            BaseResponseWithId<long> Response = new BaseResponseWithId<long>();
            Response.Result = true;
            Response.Errors = new List<Error>();
            try
            {
                if (Response.Result)
                {

                    //check sent data
                    if (Request == null)
                    {
                        Response.Result = false;
                        Error error = new Error();
                        error.ErrorCode = "Err-P12";
                        error.ErrorMSG = "Please Insert a Valid Data.";
                        Response.Errors.Add(error);
                        return Response;
                    }

                    long? NewSupplierSerial = _unitOfWork.Suppliers.FindAllQueryable(a=>true).Max(p => p == null ? 0 : p.SupplierSerialCounter) + 1;

                    string Name = null;
                    long? SupplierId = null;
                    bool IsSalesPersonChanged = false;
                    if (Request.Id != null)
                    {
                        SupplierId = Request.Id;
                        var SupplierDb = _unitOfWork.Suppliers.GetById((long)SupplierId);
                        if (SupplierDb != null)
                        {
                            if (Request.Name != null)
                            {
                                if (SupplierDb.Name.ToLower() != Request.Name.ToLower())
                                {
                                    Name = Request.Name.ToLower();
                                    var ExistNameSupplierDb = _unitOfWork.Suppliers.FindAll(a => a.Name.ToLower() == Name).FirstOrDefault();
                                    if (ExistNameSupplierDb != null)
                                    {
                                        Response.Result = false;
                                        Error error = new Error();
                                        error.ErrorCode = "Err25";
                                        error.ErrorMSG = "This Supplier Name Already Exist, Please Enter Another One!!";
                                        Response.Errors.Add(error);
                                    }
                                }
                                Name = Request.Name;
                            }
                        }
                        else
                        {
                            Response.Result = false;
                            Error error = new Error();
                            error.ErrorCode = "Err25";
                            error.ErrorMSG = "This Supplier Doesn't Exist!!";
                            Response.Errors.Add(error);
                        }
                    }
                    else
                    {
                        if (Request.Name != null)
                        {
                            Name = Request.Name.ToLower();
                            var ExistNameSupplierDb = _unitOfWork.Suppliers.FindAll(a => a.Name.ToLower() == Name).FirstOrDefault();
                            if (ExistNameSupplierDb != null)
                            {
                                Response.Result = false;
                                Error error = new Error();
                                error.ErrorCode = "Err25";
                                error.ErrorMSG = "This Supplier Name Already Exist, Please Enter Another One!!";
                                Response.Errors.Add(error);
                            }
                            Name = Request.Name;
                        }
                        else
                        {
                            Response.Result = false;
                            Error error = new Error();
                            error.ErrorCode = "Err25";
                            error.ErrorMSG = "Supplier Name Is Mandatory";
                            Response.Errors.Add(error);
                        }
                    }

                    string Type = null;
                    if (Request.Type != null)
                    {
                        Type = Request.Type;
                    }
                    else
                    {
                        Response.Result = false;
                        Error error = new Error();
                        error.ErrorCode = "Err25";
                        error.ErrorMSG = "Type Is Mandatory";
                        Response.Errors.Add(error);
                    }

                    string CreatedByString = null;
                    if (Request.CreatedBy != null)
                    {
                        CreatedByString = Request.CreatedBy;
                    }
                    else
                    {
                        Response.Result = false;
                        Error error = new Error();
                        error.ErrorCode = "Err25";
                        error.ErrorMSG = "Created By Is Mandatory";
                        Response.Errors.Add(error);
                        return Response;
                    }
                    long CreatedBy = long.Parse(Encrypt_Decrypt.Decrypt(CreatedByString, key));


                    
                    DateTime FirstContractDateRequest = DateTime.Now;
                    DateTime? FirstContractDate = null;
                    if (!string.IsNullOrEmpty(Request.FirstContractDate))
                    {
                        if (!DateTime.TryParse(Request.FirstContractDate, out FirstContractDateRequest))
                        {
                            Response.Result = false;
                            Error error = new Error();
                            error.ErrorCode = "Err23";
                            error.ErrorMSG = "Invalid First Contract Date";
                            Response.Errors.Add(error);
                        }
                        else
                        {
                            FirstContractDate = FirstContractDateRequest;
                        }
                    }

                    if (Request.Website != null)
                    {
                        var ExistSupplierWebsite = _unitOfWork.Suppliers.FindAll(a => a.WebSite.ToLower() == Request.Website.ToLower()).FirstOrDefault();
                        if (ExistSupplierWebsite != null)
                        {
                            if (SupplierId != null)
                            {
                                if (ExistSupplierWebsite.Id != SupplierId)
                                {
                                    Response.Result = false;
                                    Error error = new Error();
                                    error.ErrorCode = "Err1";
                                    error.ErrorMSG = "This Website Is Exist Before";
                                    Response.Errors.Add(error);
                                }
                            }
                            else
                            {
                                Response.Result = false;
                                Error error = new Error();
                                error.ErrorCode = "Err1";
                                error.ErrorMSG = "This Website Is Exist Before";
                                Response.Errors.Add(error);
                            }

                        }
                    }

                    if (Request.SupplierAddresses != null)
                    {
                        if (Request.SupplierAddresses.Count > 0)
                        {
                            var counter = 0;
                            foreach (var Adrs in Request.SupplierAddresses)
                            {
                                counter++;

                                int CountryId = 0;
                                if (Adrs.CountryID != 0)
                                {
                                    CountryId = Adrs.CountryID;
                                    var country = _unitOfWork.Countries.GetById(CountryId);
                                    if (country == null)
                                    {
                                        Response.Result = false;
                                        Error error = new Error();
                                        error.ErrorCode = "ErrCRM1";
                                        error.ErrorMSG = "This Country Doesn't Exist!!";
                                        Response.Errors.Add(error);
                                    }
                                }
                                else
                                {
                                    Response.Result = false;
                                    Error error = new Error();
                                    error.ErrorCode = "Err25";
                                    error.ErrorMSG = "Line: " + counter + ", " + "Address Country ID Is Mandatory";
                                    Response.Errors.Add(error);
                                }

                                int GovernorateId = 0;
                                if (Adrs.GovernorateID != 0)
                                {
                                    GovernorateId = Adrs.GovernorateID;
                                    var governorate = _unitOfWork.Governorates.GetById(GovernorateId);
                                    if (governorate == null)
                                    {
                                        Response.Result = false;
                                        Error error = new Error();
                                        error.ErrorCode = "ErrCRM1";
                                        error.ErrorMSG = "This City Doesn't Exist!!";
                                        Response.Errors.Add(error);
                                    }
                                }
                                else
                                {
                                    Response.Result = false;
                                    Error error = new Error();
                                    error.ErrorCode = "Err25";
                                    error.ErrorMSG = "Line: " + counter + ", " + "Address Governorate ID Is Mandatory";
                                    Response.Errors.Add(error);
                                }

                                long? AreaId = 0;
                                if (Adrs.AreaID != 0)
                                {
                                    AreaId = Adrs.AreaID;
                                    var area = _unitOfWork.Areas.GetById((long)AreaId);
                                    if (area == null)
                                    {
                                        Response.Result = false;
                                        Error error = new Error();
                                        error.ErrorCode = "ErrCRM1";
                                        error.ErrorMSG = "This Area Doesn't Exist!!";
                                        Response.Errors.Add(error);
                                    }
                                }
                                else
                                {
                                    Response.Result = false;
                                    Error error = new Error();
                                    error.ErrorCode = "Err25";
                                    error.ErrorMSG = "Line: " + counter + ", " + "Address Area ID Is Mandatory";
                                    Response.Errors.Add(error);
                                }

                                string Address = null;
                                if (!string.IsNullOrEmpty(Adrs.Address))
                                {
                                    Address = Adrs.Address;
                                }
                                else
                                {
                                    Response.Result = false;
                                    Error error = new Error();
                                    error.ErrorCode = "Err25";
                                    error.ErrorMSG = "Line: " + counter + ", " + "Address Is Mandatory";
                                    Response.Errors.Add(error);
                                }
                            }
                        }
                    }

                    if (Request.SupplierLandLines != null)
                    {
                        if (Request.SupplierLandLines.Count > 0)
                        {
                            var counter = 0;
                            foreach (var LND in Request.SupplierLandLines)
                            {
                                counter++;

                                string LandLine = null;
                                if (!string.IsNullOrEmpty(LND.LandLine))
                                {
                                    var ExistPhone = _Context.SupplierPhones.Where(a => a.Phone == LND.LandLine).ToList();
                                    if (ExistPhone != null && ExistPhone.Any())
                                    {
                                        Response.Result = false;
                                        Error error = new Error();
                                        error.ErrorCode = "Err2";
                                        error.ErrorMSG = "Line: " + counter;
                                        Response.Errors.Add(error);
                                    }
                                    else
                                    {
                                        LandLine = LND.LandLine;
                                    }
                                }
                                else
                                {
                                    Response.Result = false;
                                    Error error = new Error();
                                    error.ErrorCode = "Err25";
                                    error.ErrorMSG = "Line: " + counter + ", " + "Land Line Is Mandatory";
                                    Response.Errors.Add(error);
                                }
                            }
                        }
                    }

                    if (Request.SupplierMobiles != null)
                    {
                        if (Request.SupplierMobiles.Count > 0)
                        {
                            var counter = 0;
                            foreach (var MOB in Request.SupplierMobiles)
                            {
                                counter++;

                                string Mobile = null;
                                if (!string.IsNullOrEmpty(MOB.Mobile))
                                {
                                    var ExistMobile = _unitOfWork.SupplierMobiles.FindAll(a => a.Mobile == MOB.Mobile).ToList();
                                    if (ExistMobile != null && ExistMobile.Any())
                                    {
                                        Response.Result = false;
                                        Error error = new Error();
                                        error.ErrorCode = "Err3";
                                        error.ErrorMSG = "Line: " + counter;
                                        Response.Errors.Add(error);
                                    }
                                    else
                                    {
                                        Mobile = MOB.Mobile;
                                    }
                                }
                                else
                                {
                                    Response.Result = false;
                                    Error error = new Error();
                                    error.ErrorCode = "Err25";
                                    error.ErrorMSG = "Line: " + counter + ", " + "Mobile Is Mandatory";
                                    Response.Errors.Add(error);
                                }
                            }
                        }
                    }

                    if (Request.SupplierFaxes != null)
                    {
                        if (Request.SupplierFaxes.Count > 0)
                        {
                            var counter = 0;
                            foreach (var FX in Request.SupplierFaxes)
                            {
                                counter++;

                                string Fax = null;
                                if (!string.IsNullOrEmpty(FX.Fax))
                                {
                                    var ExistFax = _unitOfWork.SupplierFaxes.FindAll(a => a.Fax == FX.Fax).ToList();
                                    if (ExistFax != null && ExistFax.Any())
                                    {
                                        Response.Result = false;
                                        Error error = new Error();
                                        error.ErrorCode = "Err4";
                                        error.ErrorMSG = "Line: " + counter;
                                        Response.Errors.Add(error);
                                    }
                                    else
                                    {
                                        Fax = FX.Fax;
                                    }
                                }
                                else
                                {
                                    Response.Result = false;
                                    Error error = new Error();
                                    error.ErrorCode = "Err25";
                                    error.ErrorMSG = "Line: " + counter + ", " + "Fax Is Mandatory";
                                    Response.Errors.Add(error);
                                }
                            }
                        }
                    }

                    if (Request.SupplierPaymentTerms != null)
                    {
                        if (Request.SupplierPaymentTerms.Count > 0)
                        {
                            var counter = 0;
                            foreach (var PT in Request.SupplierPaymentTerms)
                            {
                                counter++;

                                if (string.IsNullOrEmpty(PT.Name))
                                {

                                    Response.Result = false;
                                    Error error = new Error();
                                    error.ErrorCode = "Err25";
                                    error.ErrorMSG = "Line: " + counter + ", " + "PaymentTerm Name Is Mandatory";
                                    Response.Errors.Add(error);
                                }

                                if (PT.Percentage == null)
                                {
                                    Response.Result = false;
                                    Error error = new Error();
                                    error.ErrorCode = "Err25";
                                    error.ErrorMSG = "Line: " + counter + ", " + "PaymentTerm Percentage Is Mandatory";
                                    Response.Errors.Add(error);
                                }
                            }
                        }
                    }

                    byte[] LogoBytes = null;
                    if (!string.IsNullOrEmpty(Request.Logo))
                    {
                        LogoBytes = Convert.FromBase64String(Request.Logo);
                        Request.HasLogo = true;
                    }

                    if (Response.Result)
                    {
                        DateTime? openingBalanceDate = null;
                        if (!string.IsNullOrEmpty(Request.OpeningBalanceDate))
                        {
                            openingBalanceDate = DateTime.Parse(Request.OpeningBalanceDate);
                        }

                        if (SupplierId != null && SupplierId != 0)
                        {
                            var SupplierDb = _unitOfWork.Suppliers.GetById((long)SupplierId);
                            if (SupplierDb != null)
                            {
                                // Update
                                SupplierDb.Name = Name == null ? SupplierDb.Name : Name;
                                SupplierDb.Type = Request.Type;
                                SupplierDb.Email = Request.Email;
                                SupplierDb.WebSite = Request.Website;
                                SupplierDb.Note = Request.Note;
                                SupplierDb.Rate = Request.Rate;
                                SupplierDb.FirstContractDate = FirstContractDate!=null?DateOnly.FromDateTime((DateTime)FirstContractDate):null;
                                SupplierDb.Logo = LogoBytes != null ? LogoBytes : SupplierDb.Logo;
                                SupplierDb.HasLogo = SupplierDb.HasLogo != null ? (Request.HasLogo != null ? Request.HasLogo != null : SupplierDb.HasLogo) : Request.HasLogo;
                                SupplierDb.Active = true;
                                SupplierDb.ModifiedBy = validation.userID;
                                SupplierDb.ModifiedDate = DateTime.Now;
                                SupplierDb.OpeningBalance = Request.OpeningBalance;
                                SupplierDb.OpeningBalanceType = Request.OpeningBalanceType;
                                SupplierDb.OpeningBalanceDate = openingBalanceDate!=null?DateOnly.FromDateTime((DateTime)openingBalanceDate):null;
                                SupplierDb.OpeningBalanceCurrencyId = Request.OpeningBalanceCurrencyId;
                                SupplierDb.DefaultDelivaryAndShippingMethodId = Request.DefaultDelivaryAndShippingMethodId;
                                SupplierDb.OtherDelivaryAndShippingMethodName = Request.OtherDelivaryAndShippingMethodName;
                                SupplierDb.CommercialRecord = Request.CommercialRecord;
                                SupplierDb.TaxCard = Request.TaxCard;
                                SupplierDb.SupplierSerialCounter = Request.SupplierSerialCounter != null ? Request.SupplierSerialCounter : NewSupplierSerial;
                                SupplierDb.RegistrationNumber = Request.RegistrationNumber;
                                _unitOfWork.Suppliers.Update(SupplierDb);
                                var SupplierUpdate = _unitOfWork.Complete();

                                if (SupplierUpdate > 0)
                                {
                                    Response.Result = true;
                                    Response.ID = SupplierId ?? 0;

                                }
                                else
                                {
                                    Response.Result = false;
                                    Error error = new Error();
                                    error.ErrorCode = "Err25";
                                    error.ErrorMSG = "Faild To Update this Supplier!!";
                                    Response.Errors.Add(error);
                                }
                            }
                            else
                            {
                                Response.Result = false;
                                Error error = new Error();
                                error.ErrorCode = "Err25";
                                error.ErrorMSG = "This Supplier Doesn't Exist!!";
                                Response.Errors.Add(error);
                            }
                        }
                        else
                        {
                            // Insert
                            var newSupplier = new Supplier()
                            {
                                Name = Request.Name,
                                Type = Request.Type,
                                Email = Request.Email,
                                WebSite = Request.Website,
                                CreatedBy = validation.userID,
                                CreationDate = DateTime.Now,
                                Note = Request.Note,
                                Rate = Request.Rate,
                                FirstContractDate = FirstContractDate != null ? DateOnly.FromDateTime((DateTime)FirstContractDate) : null,
                                Logo = LogoBytes,
                                HasLogo = Request.HasLogo,
                                Active = true,
                                ModifiedBy = validation.userID,
                                ModifiedDate = DateTime.Now,
                                OpeningBalance = Request.OpeningBalance,
                                OpeningBalanceType = Request.OpeningBalanceType,
                                OpeningBalanceDate = DateOnly.FromDateTime(DateTime.Now),
                                OpeningBalanceCurrencyId = Request.OpeningBalanceCurrencyId,
                                DefaultDelivaryAndShippingMethodId = Request.DefaultDelivaryAndShippingMethodId,
                                OtherDelivaryAndShippingMethodName = Request.OtherDelivaryAndShippingMethodName,
                                CommercialRecord = Request.CommercialRecord,
                                TaxCard = Request.TaxCard,
                                SupplierSerialCounter = NewSupplierSerial,
                                RegistrationNumber = Request.RegistrationNumber,

                            };
                            _unitOfWork.Suppliers.Add(newSupplier);

                            var SupplierInsert = _unitOfWork.Complete();

                            if (SupplierInsert > 0)
                            {
                                SupplierId = newSupplier.Id;
                                Response.Result = true;
                                Response.ID = SupplierId ?? 0;
                            }
                            else
                            {
                                Response.Result = false;
                                Error error = new Error();
                                error.ErrorCode = "Err25";
                                error.ErrorMSG = "Faild To Insert this Supplier!!";
                                Response.Errors.Add(error);
                            }

                        }

                        if (SupplierId != 0 && SupplierId != null)
                        {
                            //Add-Edit supplier Speciality
                            if (Request.SupplierSpecialities != null)
                            {
                                if (Request.SupplierSpecialities.Count > 0)
                                {
                                    var counter = 0;
                                    foreach (var SPC in Request.SupplierSpecialities)
                                    {
                                        counter++;

                                        string ModifiedByString = null;
                                        long? ModifiedBy = 0;
                                        if (SPC.ID != 0 && SPC.ID != null)
                                        {
                                            if (SPC.ModifiedBy != null)
                                            {
                                                ModifiedByString = SPC.ModifiedBy;
                                            }
                                            else
                                            {
                                                Response.Result = false;
                                                Error error = new Error();
                                                error.ErrorCode = "Err25";
                                                error.ErrorMSG = "Line: " + counter + ", " + "Message: Modified By Id Is Mandatory";
                                                Response.Errors.Add(error);
                                            }
                                            ModifiedBy = long.Parse(Encrypt_Decrypt.Decrypt(ModifiedByString, key));
                                        }

                                        int SpecialityId = 0;
                                        if (SPC.SpecialityID != 0)
                                        {
                                            SpecialityId = SPC.SpecialityID;
                                            var speciality = _unitOfWork.Specialities.GetById(SpecialityId);
                                            if (speciality == null)
                                            {
                                                Response.Result = false;
                                                Error error = new Error();
                                                error.ErrorCode = "ErrCRM1";
                                                error.ErrorMSG = "This Speciality Doesn't Exist!!";
                                                Response.Errors.Add(error);
                                            }
                                        }
                                        else
                                        {
                                            Response.Result = false;
                                            Error error = new Error();
                                            error.ErrorCode = "Err25";
                                            error.ErrorMSG = "Line: " + counter + ", " + "Speciality Id Is Mandatory";
                                            Response.Errors.Add(error);
                                        }
                                    }
                                }
                                if (Response.Result)
                                {
                                    var SupplierSpecialitiesDB = _unitOfWork.SupplierSpecialities.FindAll(a => a.SupplierId == SupplierId).ToList();
                                    if (SupplierSpecialitiesDB != null && SupplierSpecialitiesDB.Count > 0)
                                    {
                                        foreach (var SPC in SupplierSpecialitiesDB)
                                        {
                                            _unitOfWork.SupplierSpecialities.Delete(SPC);
                                            var SupplierSpecialityDelete = _unitOfWork.Complete();
                                        }
                                    }

                                    var test = Request.SupplierSpecialities.Select(a => a.SpecialityID).ToList();
                                    var t1 = _unitOfWork.Specialities.FindAll(a => test.Contains(a.Id)).ToList();
                                    foreach (var SPC in Request.SupplierSpecialities)
                                    {
                                        //if (SPC.ID != null && SPC.ID != 0)
                                        //{
                                        //    var SupplierSpecialityDb = _Context.proc_SupplierSpecialityLoadByPrimaryKey(SPC.ID).FirstOrDefault();
                                        //    if (SupplierSpecialityDb != null)
                                        //    {
                                        //        // Update
                                        //        var SupplierSpecialityUpdate = _Context.proc_SupplierSpecialityUpdate(SPC.ID,
                                        //                                                                            SupplierId,
                                        //                                                                            SPC.SpecialityID,
                                        //                                                                            SupplierSpecialityDb.CreatedBy,
                                        //                                                                            SupplierSpecialityDb.CreationDate,
                                        //                                                                            long.Parse(Encrypt_Decrypt.Decrypt(SPC.ModifiedBy, key)),
                                        //                                                                            DateTime.Now,
                                        //                                                                            true
                                        //                                                                            );
                                        //    }
                                        //    else
                                        //    {
                                        //        Response.Result = false;
                                        //        Error error = new Error();
                                        //        error.ErrorCode = "Err25";
                                        //        error.ErrorMSG = "This Speciality Person Doesn't Exist!!";
                                        //        Response.Errors.Add(error);
                                        //    }
                                        //}
                                        //else
                                        //{
                                        // Insert
                                        var SupplierMobileInsert = _unitOfWork.SupplierSpecialities.Add(new SupplierSpeciality()
                                        {
                                            SupplierId = (long)SupplierId,
                                            SpecialityId = SPC.SpecialityID,
                                            CreatedBy = validation.userID,
                                            CreationDate = DateTime.Now,
                                            ModifiedBy = validation.userID,
                                            Modified = DateTime.Now,
                                            Active = true
                                        });

                                        _unitOfWork.Complete();

                                        //}
                                    }
                                }
                            }

                            if (Request.Type != null && Request.Type.ToLower() == "individual")
                            {
                                // Insert

                                var SupplierInsert = _unitOfWork.SupplierAddresses.Add(new SupplierAddress()
                                {
                                    SupplierId = (long)SupplierId,
                                    CountryId = 1,
                                    GovernorateId = 1,
                                    Address = "",
                                    CreatedBy = validation.userID,
                                    ModifiedBy = validation.userID,
                                    CreationDate = DateTime.Now,
                                    Modified = DateTime.Now,
                                    Active = true,
                                    BuildingNumber = "0",
                                    Floor = "0",
                                    Description = "",
                                    AreaId = null,

                                });
                                _unitOfWork.Complete();
                            }

                            //Add-Edit supplier Addresses
                            if (Request.SupplierAddresses != null)
                            {
                                if (Request.SupplierAddresses.Count > 0)
                                {
                                    foreach (var Adrs in Request.SupplierAddresses)
                                    {
                                        int BuildingNumber = 0;
                                        int Floor = 0;
                                        if (!string.IsNullOrEmpty(Adrs.BuildingNumber) && int.TryParse(Adrs.BuildingNumber, out BuildingNumber))
                                        {
                                            int.TryParse(Adrs.BuildingNumber, out BuildingNumber);
                                        }
                                        if (!string.IsNullOrEmpty(Adrs.Floor) && int.TryParse(Adrs.Floor, out Floor))
                                        {
                                            int.TryParse(Adrs.Floor, out Floor);
                                        }


                                        if (Adrs.ID != null && Adrs.ID != 0)
                                        {
                                            var AddressDb = _unitOfWork.SupplierAddresses.FindAll(a => a.Id == Adrs.ID).FirstOrDefault();
                                            if (AddressDb != null)
                                            {
                                                // Update
                                                AddressDb.SupplierId = (long)SupplierId;
                                                AddressDb.CountryId = Adrs.CountryID;
                                                AddressDb.GovernorateId = Adrs.GovernorateID;
                                                AddressDb.Address = Adrs.Address;
                                                AddressDb.ModifiedBy = validation.userID;
                                                AddressDb.Modified = DateTime.Now;
                                                AddressDb.Active = true;
                                                AddressDb.BuildingNumber = BuildingNumber.ToString();
                                                AddressDb.Floor = Floor.ToString();
                                                AddressDb.Description = Adrs.Description;
                                                AddressDb.AreaId = Adrs.AreaID;

                                                _unitOfWork.SupplierAddresses.Update(AddressDb);
                                                _unitOfWork.Complete();
                                            }
                                            else
                                            {
                                                Response.Result = false;
                                                Error error = new Error();
                                                error.ErrorCode = "Err25";
                                                error.ErrorMSG = "This Address Doesn't Exist!!";
                                                Response.Errors.Add(error);
                                            }
                                        }
                                        else
                                        {
                                            // Insert
                                            var SupplierInsert = _unitOfWork.SupplierAddresses.Add(new SupplierAddress()
                                            {
                                                SupplierId = (long)SupplierId,
                                                CountryId = Adrs.CountryID,
                                                GovernorateId = Adrs.GovernorateID,
                                                Address = Adrs.Address,
                                                CreatedBy = validation.userID,
                                                ModifiedBy = validation.userID,
                                                CreationDate = DateTime.Now,
                                                Modified = DateTime.Now,
                                                Active = true,
                                                BuildingNumber = BuildingNumber.ToString(),
                                                Floor = Floor.ToString(),
                                                Description = Adrs.Description,
                                                AreaId = Adrs.AreaID,

                                            });
                                            _unitOfWork.Complete();
                                        }
                                    }
                                }
                            }

                            //Add-Edit supplier Land Lines
                            if (Request.SupplierLandLines != null)
                            {
                                if (Request.SupplierSpecialities.Count > 0)
                                {
                                    foreach (var LND in Request.SupplierLandLines)
                                    {
                                        if (LND.ID != null && LND.ID != 0)
                                        {
                                            var SupplierLandLineDb = _unitOfWork.SupplierPhones.FindAll(a => a.Id == LND.ID).FirstOrDefault();
                                            if (SupplierLandLineDb != null)
                                            {
                                                // Update

                                                SupplierLandLineDb.SupplierId = (long)SupplierId;
                                                SupplierLandLineDb.Phone = LND.LandLine;
                                                SupplierLandLineDb.ModifiedBy = validation.userID;
                                                SupplierLandLineDb.Modified = DateTime.Now;
                                                SupplierLandLineDb.Active = true;
                                                _unitOfWork.SupplierPhones.Update(SupplierLandLineDb);
                                                _unitOfWork.Complete();
                                            }
                                            else
                                            {
                                                Response.Result = false;
                                                Error error = new Error();
                                                error.ErrorCode = "Err25";
                                                error.ErrorMSG = "This Land Line Doesn't Exist!!";
                                                Response.Errors.Add(error);
                                            }
                                        }
                                        else
                                        {
                                            // Insert
                                            _unitOfWork.SupplierPhones.Add(new SupplierPhone(){
                                                SupplierId = (long)SupplierId,
                                                Phone = LND.LandLine,
                                                CreatedBy = validation.userID,
                                                ModifiedBy = validation.userID,
                                                CreationDate = DateTime.Now,
                                                Modified = DateTime.Now,
                                                Active = true
                                            });


                                            var SupplierLandLineInsert = _unitOfWork.Complete();
                                        }
                                    }
                                }
                            }

                            //Add-Edit supplier Mobiles
                            if (Request.SupplierMobiles != null)
                            {
                                if (Request.SupplierMobiles.Count > 0)
                                {
                                    foreach (var MOB in Request.SupplierMobiles)
                                    {
                                        if (MOB.ID != null && MOB.ID != 0)
                                        {
                                            var SupplierMobileDb = _unitOfWork.SupplierMobiles.FindAll(a => a.Id == MOB.ID).FirstOrDefault();
                                            if (SupplierMobileDb != null)
                                            {
                                                // Update

                                                SupplierMobileDb.SupplierId = (long)SupplierId;
                                                SupplierMobileDb.Mobile = MOB.Mobile;
                                                SupplierMobileDb.ModifiedBy = validation.userID;
                                                SupplierMobileDb.Modified = DateTime.Now;
                                                SupplierMobileDb.Active = true;
                                                _unitOfWork.SupplierMobiles.Update(SupplierMobileDb);
                                                _unitOfWork.Complete();
                                            }
                                            else
                                            {
                                                Response.Result = false;
                                                Error error = new Error();
                                                error.ErrorCode = "Err25";
                                                error.ErrorMSG = "This Mobile Doesn't Exist!!";
                                                Response.Errors.Add(error);
                                            }
                                        }
                                        else
                                        {
                                            // Insert
                                            _unitOfWork.SupplierMobiles.Add(new SupplierMobile()
                                            {
                                                SupplierId = (long)SupplierId,
                                                Mobile = MOB.Mobile,
                                                ModifiedBy = validation.userID,
                                                CreatedBy = validation.userID,
                                                CreationDate = DateTime.Now,
                                                Modified = DateTime.Now,
                                                Active = true,
                                            });
                                            _unitOfWork.Complete();
                                        }
                                    }
                                }
                            }

                            //Add-Edit supplier Faxes
                            if (Request.SupplierFaxes != null)
                            {
                                if (Request.SupplierFaxes.Count > 0)
                                {
                                    foreach (var FX in Request.SupplierFaxes)
                                    {
                                        if (FX.ID != null && FX.ID != 0)
                                        {
                                            var SupplierFaxDb = _unitOfWork.SupplierFaxes.FindAll(a => a.Id == FX.ID).FirstOrDefault();
                                            if (SupplierFaxDb != null)
                                            {
                                                // Update

                                                SupplierFaxDb.SupplierId = (long)SupplierId;
                                                SupplierFaxDb.Fax = FX.Fax;
                                                SupplierFaxDb.ModifiedBy = validation.userID;
                                                SupplierFaxDb.Modified = DateTime.Now;
                                                SupplierFaxDb.Active = true;
                                                _unitOfWork.SupplierFaxes.Update(SupplierFaxDb);
                                                _unitOfWork.Complete();
                                            }
                                            else
                                            {
                                                Response.Result = false;
                                                Error error = new Error();
                                                error.ErrorCode = "Err25";
                                                error.ErrorMSG = "This Fax Doesn't Exist!!";
                                                Response.Errors.Add(error);
                                            }
                                        }
                                        else
                                        {
                                            // Insert
                                            _unitOfWork.SupplierFaxes.Add(new SupplierFax() { 
                                                SupplierId = (long)SupplierId,
                                                Fax = FX.Fax,
                                                CreatedBy = validation.userID,
                                                ModifiedBy = validation.userID,
                                                CreationDate = DateTime.Now,
                                                Modified = DateTime.Now,
                                                Active = true
                                            });
                                            _unitOfWork.Complete();

                                            var SupplierFaxInsert = _unitOfWork.Complete();
                                        }
                                    }
                                }
                            }

                            //Add-Edit supplier Payment Terms
                            if (Request.SupplierPaymentTerms != null)
                            {
                                if (Request.SupplierPaymentTerms.Count > 0)
                                {
                                    foreach (var PT in Request.SupplierPaymentTerms)
                                    {
                                        if (PT.ID != null && PT.ID != 0)
                                        {
                                            var SupplierPaymentTermDb = _unitOfWork.SupplierPaymentTerms.FindAll(a => a.Id == PT.ID).FirstOrDefault();
                                            if (SupplierPaymentTermDb != null)
                                            {
                                                // Update
                                                SupplierPaymentTermDb.SupplierId = (long)SupplierId;
                                                SupplierPaymentTermDb.Name = PT.Name;
                                                SupplierPaymentTermDb.Name = PT.Description;
                                                SupplierPaymentTermDb.Percentage = PT.Percentage??0;
                                                SupplierPaymentTermDb.Time = null;
                                                SupplierPaymentTermDb.Active = true;
                                                SupplierPaymentTermDb.ModifiedBy = validation.userID;
                                                SupplierPaymentTermDb.ModifiedDate = DateTime.Now;
                                                _unitOfWork.SupplierPaymentTerms.Update(SupplierPaymentTermDb);


                                                var SupplierPaymentTermUpdate = _unitOfWork.Complete();
                                            }
                                            else
                                            {
                                                Response.Result = false;
                                                Error error = new Error();
                                                error.ErrorCode = "Err25";
                                                error.ErrorMSG = "This Payment Term Doesn't Exist!!";
                                                Response.Errors.Add(error);
                                            }
                                        }
                                        else
                                        {
                                            // Insert

                                            _unitOfWork.SupplierPaymentTerms.Add(new SupplierPaymentTerm()
                                            {
                                                SupplierId = (long)SupplierId,
                                                Name = PT.Name,
                                                Description = PT.Description,
                                                Percentage = PT.Percentage??0,
                                                Time = null,
                                                Active = true,
                                                CreatedBy = validation.userID,
                                                CreationDate = DateTime.Now,
                                                ModifiedBy = validation.userID,
                                                ModifiedDate = DateTime.Now,
                                            });

                                            var SupplierPaymentTermInsert = _unitOfWork.Complete();
                                        }
                                    }
                                }
                            }

                            //Add-Edit supplier Bank Accounts
                            if (Request.SupplierBankAccounts != null)
                            {
                                if (Request.SupplierBankAccounts.Count > 0)
                                {
                                    foreach (var BA in Request.SupplierBankAccounts)
                                    {
                                        if (BA.ID != null && BA.ID != 0)
                                        {
                                            var SupplierBankAccountDb = _unitOfWork.SupplierBankAccounts.FindAll(a => a.Id == BA.ID).FirstOrDefault();
                                            if (SupplierBankAccountDb != null)
                                            {
                                                // Update
                                                SupplierBankAccountDb.SupplierId = (long)SupplierId;
                                                SupplierBankAccountDb.BankDetails = BA.BankDetails;
                                                SupplierBankAccountDb.BeneficiaryName = BA.BeneficiaryName;
                                                SupplierBankAccountDb.Iban = BA.IBAN;
                                                SupplierBankAccountDb.SwiftCode = BA.SwiftCode;
                                                SupplierBankAccountDb.Account = BA.Account;
                                                SupplierBankAccountDb.Active = true;
                                                SupplierBankAccountDb.ModifiedBy = validation.userID;
                                                SupplierBankAccountDb.ModifiedDate = DateTime.Now;
                                                _unitOfWork.SupplierBankAccounts.Update(SupplierBankAccountDb);
                                                _unitOfWork.Complete();
                                                
                                            }
                                            else
                                            {
                                                Response.Result = false;
                                                Error error = new Error();
                                                error.ErrorCode = "Err25";
                                                error.ErrorMSG = "This Bank Account Doesn't Exist!!";
                                                Response.Errors.Add(error);
                                            }
                                        }
                                        else
                                        {
                                            // Insert
                                            _unitOfWork.SupplierBankAccounts.Add(new SupplierBankAccount()
                                            {
                                                SupplierId = (long)SupplierId,
                                                BankDetails = BA.BankDetails,
                                                BeneficiaryName = BA.BeneficiaryName,
                                                Iban = BA.IBAN,
                                                SwiftCode = BA.SwiftCode,
                                                Account = BA.Account,
                                                Active = true,
                                                ModifiedBy = validation.userID,
                                                ModifiedDate = DateTime.Now,
                                                CreatedBy = validation.userID,
                                                CreationDate = DateTime.Now,
                                            });

                                            var SupplierBankAccountInsert = _unitOfWork.Complete();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                return Response;
            }
            catch (Exception ex)
            {
                Response.Result = false;
                Error error = new Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.InnerException != null ? ex.InnerException.Message : ex.Message; ;
                Response.Errors.Add(error);
                return Response;
            }
        }

        public async Task<GetSupplierData> GetSupplierDataResponse([FromHeader] long SupplierId)
        {
            GetSupplierData Response = new GetSupplierData();
            Response.Result = true;
            Response.Errors = new List<Error>();
            try
            {
                if (Response.Result)
                {
                    Supplier SupplierDataDB = new Supplier();

                    if (SupplierId!=0)
                    {
                        SupplierDataDB = (await _unitOfWork.Suppliers.FindAllAsync(a => a.Id == SupplierId, includes: new[] { "DefaultDelivaryAndShippingMethod" })).FirstOrDefault();
                        if (SupplierDataDB == null)
                        {
                            Response.Result = false;
                            Error error = new Error();
                            error.ErrorCode = "Err69";
                            error.ErrorMSG = "This Supplier Doesn't Exist";
                            Response.Errors.Add(error);
                            return Response;
                        }
                    }
                    else
                    {
                        Response.Result = false;
                        Error error = new Error();
                        error.ErrorCode = "Err69";
                        error.ErrorMSG = "Invalid Supplier Id";
                        Response.Errors.Add(error);
                        return Response;
                    }


                    if (SupplierDataDB.Id != 0)
                    {

                        var SupplierDataObj = new SupplierData()
                        {
                            Id = SupplierDataDB.Id,
                            Email = SupplierDataDB.Email,
                            FirstContractDate = SupplierDataDB.FirstContractDate.ToString(),
                            Name = SupplierDataDB.Name,
                            Note = SupplierDataDB.Note,
                            Rate = SupplierDataDB.Rate,
                            Type = SupplierDataDB.Type,
                            Website = SupplierDataDB.WebSite,
                            CommercialRecord = SupplierDataDB.CommercialRecord,
                            DefaultDelivaryAndShippingMethodId = SupplierDataDB.DefaultDelivaryAndShippingMethodId,
                            DefaultDelivaryAndShippingMethodName = SupplierDataDB.DefaultDelivaryAndShippingMethodId != null ? SupplierDataDB.DefaultDelivaryAndShippingMethod.Name : null,
                            SupplierSerialCounter = SupplierDataDB.SupplierSerialCounter,
                            OpeningBalance = SupplierDataDB.OpeningBalance,
                            OpeningBalanceCurrencyId = SupplierDataDB.OpeningBalanceCurrencyId,
                            OpeningBalanceCurrencyName = SupplierDataDB.OpeningBalanceCurrencyId != null ? Common.GetCurrencyName(SupplierDataDB.OpeningBalanceCurrencyId ?? 0,_Context) : null,
                            OpeningBalanceDate = SupplierDataDB.OpeningBalanceDate != null ? SupplierDataDB.OpeningBalanceDate.ToString().Split(' ')[0] : null,
                            OpeningBalanceType = SupplierDataDB.OpeningBalanceType,
                            OtherDelivaryAndShippingMethodName = SupplierDataDB.OtherDelivaryAndShippingMethodName,
                            TaxCard = SupplierDataDB.TaxCard,
                            RegistrationNumber = SupplierDataDB.RegistrationNumber
                        };
                        if (SupplierDataDB.Logo != null)
                        {
                            SupplierDataObj.Logo = Convert.ToBase64String(SupplierDataDB.Logo);
                        }

                        var SupplierSpecialitiesDB = (await _unitOfWork.VSupplierSpecialities.FindAllAsync(a => a.Id == SupplierDataDB.Id && a.Active == true)).ToList();
                        if (SupplierSpecialitiesDB != null && SupplierSpecialitiesDB.Count() > 0)
                        {
                            var SupplierSpecialitiesList = new List<AddSupplierSpeciality>();
                            foreach (var SPC in SupplierSpecialitiesDB)
                            {

                                var SupplierSpeciality = new AddSupplierSpeciality()
                                {
                                    ID = SPC.Id,
                                    SpecialityID = SPC.SpecialityId,
                                    SpecialityName = SPC.Speciality
                                };
                                SupplierSpecialitiesList.Add(SupplierSpeciality);
                            }
                            SupplierDataObj.SupplierSpecialities = SupplierSpecialitiesList;
                        }

                        Response.SuppliersData = SupplierDataObj;

                        var SupplierAddressesDB = (await _unitOfWork.VSupplierAddresses.FindAllAsync(a => a.SupplierId == SupplierDataDB.Id && a.Active == true)).ToList();
                        if (SupplierAddressesDB != null && SupplierAddressesDB.Count() > 0)
                        {
                            var SupplierAddressesList = new List<AddSupplierAddress>();
                            foreach (var Adrs in SupplierAddressesDB)
                            {

                                var SupplierAddress = new AddSupplierAddress()
                                {
                                    ID = Adrs.Id,
                                    Address = Adrs.Address,
                                    CountryID = Adrs.CountryId,
                                    CountryName = Adrs.Country,
                                    GovernorateID = Adrs.GovernorateId,
                                    GovernorateName = Adrs.Governorate,
                                    AreaID = Adrs.AreaId ?? 0,
                                    AreaName = Adrs.Area,
                                    BuildingNumber = Adrs.BuildingNumber,
                                    Description = Adrs.Description,
                                    Floor = Adrs.Floor
                                };
                                SupplierAddressesList.Add(SupplierAddress);
                            }
                            Response.SupplierAddressData = SupplierAddressesList;
                        }

                        var SupplierContactPersonsDB = _unitOfWork.SupplierContactPeople.FindAll(a => a.SupplierId == SupplierDataDB.Id && a.Active == true).ToList();
                        if (SupplierContactPersonsDB != null && SupplierContactPersonsDB.Count > 0)
                        {
                            var SupplierContactPersonsList = new List<GetSupplierContactPerson>();
                            foreach (var CP in SupplierContactPersonsDB)
                            {

                                var SupplierContactPerson = new GetSupplierContactPerson()
                                {
                                    ID = CP.Id,
                                    Name = CP.Name,
                                    Email = CP.Email,
                                    Location = CP.Location,
                                    Mobile = CP.Mobile,
                                    Title = CP.Title
                                };
                                SupplierContactPersonsList.Add(SupplierContactPerson);
                            }
                            Response.SupplierContactPersonData = SupplierContactPersonsList;
                        }

                        var SupplierMobileDB = (await _unitOfWork.SupplierMobiles.FindAllAsync(a => a.SupplierId == SupplierDataDB.Id && a.Active == true)).ToList();
                        if (SupplierMobileDB != null && SupplierMobileDB.Count() > 0)
                        {
                            var SupplierMobilesList = new List<AddSupplierMobile>();
                            foreach (var MOB in SupplierMobileDB)
                            {

                                var SupplierMobile = new AddSupplierMobile()
                                {
                                    ID = MOB.Id,
                                    Mobile = MOB.Mobile
                                };
                                SupplierMobilesList.Add(SupplierMobile);
                            }
                            Response.SupplierMobileData = SupplierMobilesList;
                        }

                        var SupplierLandLinesDB = (await _unitOfWork.SupplierPhones.FindAllAsync(a => a.SupplierId == SupplierDataDB.Id && a.Active == true)).ToList();
                        if (SupplierLandLinesDB != null && SupplierLandLinesDB.Count() > 0)
                        {
                            var SupplierLandLinesList = new List<AddSupplierLandLine>();
                            foreach (var LND in SupplierLandLinesDB)
                            {
                                var SupplierLandLine = new AddSupplierLandLine()
                                {
                                    ID = LND.Id,
                                    LandLine = LND.Phone
                                };
                                SupplierLandLinesList.Add(SupplierLandLine);
                            }
                            Response.SupplierLandLineData = SupplierLandLinesList;
                        }

                        var SupplierFaxesDB = (await _unitOfWork.SupplierFaxes.FindAllAsync(a => a.SupplierId == SupplierDataDB.Id && a.Active == true)).ToList();
                        if (SupplierFaxesDB != null && SupplierFaxesDB.Count() > 0)
                        {
                            var SupplierFaxesList = new List<AddSupplierFax>();
                            foreach (var FX in SupplierFaxesDB)
                            {
                                var SupplierFax = new AddSupplierFax()
                                {
                                    ID = FX.Id,
                                    Fax = FX.Fax
                                };
                                SupplierFaxesList.Add(SupplierFax);
                            }
                            Response.SupplierFaxData = SupplierFaxesList;
                        }

                        var SupplierPaymentTermsDB = (await _unitOfWork.SupplierPaymentTerms.FindAllAsync(a => a.SupplierId == SupplierDataDB.Id && a.Active == true)).ToList();
                        if (SupplierPaymentTermsDB != null && SupplierPaymentTermsDB.Count() > 0)
                        {
                            var SupplierPaymentTermsList = SupplierPaymentTermsDB.Select(PT => new AddSupplierPaymentTerm
                            {
                                ID = PT.Id,
                                Name = PT.Name,
                                Description = PT.Description,
                                Percentage = PT.Percentage
                            }).ToList();

                            Response.SupplierPaymentTermData = SupplierPaymentTermsList;
                        }

                        var SupplierBankAccountDB = (await _unitOfWork.SupplierBankAccounts.FindAllAsync(a => a.SupplierId == SupplierDataDB.Id && a.Active == true)).ToList();
                        if (SupplierBankAccountDB != null && SupplierBankAccountDB.Count() > 0)
                        {
                            var SupplierBankAccountList = SupplierBankAccountDB.Select(BA => new AddSupplierBankAccount
                            {
                                ID = BA.Id,
                                Account = BA.Account,
                                BankDetails = BA.BankDetails,
                                BeneficiaryName = BA.BeneficiaryName,
                                IBAN = BA.Iban,
                                SwiftCode = BA.SwiftCode
                            }).ToList();

                            Response.SupplierBankAccountData = SupplierBankAccountList;
                        }

                        var LicenseAttechementsPathsDB = (await _unitOfWork.SupplierAttachments.FindAllAsync(a => a.SupplierId == SupplierDataDB.Id && a.Type == "License")).ToList();
                        if (LicenseAttechementsPathsDB.Count() > 0)
                        {
                            List<Attachment> LicenseAttachmentsList = new List<Attachment>();
                            foreach (var attachment in LicenseAttechementsPathsDB)
                            {
                                //byte[] fileArray = System.IO.File.ReadAllBytes(baseURL + attachment.AttachmentPath);
                                //string base64FileRepresentation = Convert.ToBase64String(fileArray);
                                Attachment licenseAttach = new Attachment
                                {
                                    Id = attachment.Id,
                                    //FileContent = base64FileRepresentation,
                                    FileExtension = attachment.FileExtenssion,
                                    FileName = attachment.FileName,
                                    FilePath = Globals.baseURL + attachment.AttachmentPath.TrimStart('~')
                                };

                                LicenseAttachmentsList.Add(licenseAttach);
                            }

                            Response.LicenseAttachements = LicenseAttachmentsList;
                        }

                        var BrochureAttechementsPathsDB = (await _unitOfWork.SupplierAttachments.FindAllAsync(a => a.SupplierId == SupplierDataDB.Id && a.Type == "Brochure")).ToList();
                        if (BrochureAttechementsPathsDB.Count() > 0)
                        {
                            List<Attachment> BrochureAttachmentsList = new List<Attachment>();
                            foreach (var attachment in BrochureAttechementsPathsDB)
                            {
                                //byte[] fileArray = System.IO.File.ReadAllBytes(baseURL + attachment.AttachmentPath);
                                //string base64FileRepresentation = Convert.ToBase64String(fileArray);
                                Attachment brochureAttach = new Attachment
                                {
                                    Id = attachment.Id,
                                    //FileContent = base64FileRepresentation,
                                    FileExtension = attachment.FileExtenssion,
                                    FileName = attachment.FileName,
                                    FilePath = Globals.baseURL + attachment.AttachmentPath.TrimStart('~')
                                };

                                BrochureAttachmentsList.Add(brochureAttach);
                            }

                            Response.BrochuresAttachments = BrochureAttachmentsList;
                        }

                        var BusinessCardAttechementsPathsDB = (await _unitOfWork.SupplierAttachments.FindAllAsync(a => a.SupplierId == SupplierDataDB.Id && a.Type == "Business Cards")).ToList();
                        if (BusinessCardAttechementsPathsDB.Count() > 0)
                        {
                            List<Attachment> BusinessCardAttachmentsList = new List<Attachment>();
                            foreach (var attachment in BusinessCardAttechementsPathsDB)
                            {
                                //byte[] fileArray = System.IO.File.ReadAllBytes(baseURL + attachment.AttachmentPath);
                                //string base64FileRepresentation = Convert.ToBase64String(fileArray);
                                Attachment businessCardAttach = new Attachment
                                {
                                    Id = attachment.Id,
                                    //FileContent = base64FileRepresentation,
                                    FileExtension = attachment.FileExtenssion,
                                    FileName = attachment.FileName,
                                    FilePath = Globals.baseURL + attachment.AttachmentPath.TrimStart('~')
                                };

                                BusinessCardAttachmentsList.Add(businessCardAttach);
                            }

                            Response.BussinessCardsAttachments = BusinessCardAttachmentsList;
                        }

                        var TaxCardAttechementPathDB = (await _unitOfWork.SupplierAttachments.FindAllAsync(a => a.SupplierId == SupplierDataDB.Id && a.Type == "Tax Card")).FirstOrDefault();
                        if (TaxCardAttechementPathDB != null)
                        {
                            Attachment TaxCardAttachment = new Attachment()
                            {
                                Id = TaxCardAttechementPathDB.Id,
                                //FileContent = base64FileRepresentation,
                                FileExtension = TaxCardAttechementPathDB.FileExtenssion,
                                FileName = TaxCardAttechementPathDB.FileName,
                                FilePath = Globals.baseURL + TaxCardAttechementPathDB.AttachmentPath.TrimStart('~')
                            };

                            Response.TaxCardAttachment = TaxCardAttachment;
                        }

                        var CommercialRecordAttechementPathDB = (await _unitOfWork.SupplierAttachments.FindAllAsync(a => a.SupplierId == SupplierDataDB.Id && a.Type == "Commercial Record")).FirstOrDefault();
                        if (CommercialRecordAttechementPathDB != null)
                        {
                            Attachment CommercialRecordAttachment = new Attachment()
                            {
                                Id = CommercialRecordAttechementPathDB.Id,
                                FileExtension = CommercialRecordAttechementPathDB.FileExtenssion,
                                FileName = CommercialRecordAttechementPathDB.FileName,
                                FilePath = Globals.baseURL + CommercialRecordAttechementPathDB.AttachmentPath.TrimStart('~')
                            };

                            Response.CommercialRecordAttachment = CommercialRecordAttachment;
                        }

                        var OtherAttechementsPathsDB = (await _unitOfWork.SupplierAttachments.FindAllAsync(a => a.SupplierId == SupplierDataDB.Id && a.Type != "Business Cards" && a.Type != "Brochure" && a.Type != "License" && a.Type != "Tax Card" && a.Type != "Commercial Record")).ToList();
                        if (OtherAttechementsPathsDB.Count() > 0)
                        {
                            List<Attachment> OtherAttachmentsList = new List<Attachment>();
                            foreach (var attachment in OtherAttechementsPathsDB)
                            {
                                //byte[] fileArray = System.IO.File.ReadAllBytes(baseURL + attachment.AttachmentPath);
                                //string base64FileRepresentation = Convert.ToBase64String(fileArray);
                                Attachment otherAttach = new Attachment
                                {
                                    Id = attachment.Id,
                                    //FileContent = base64FileRepresentation,
                                    FileExtension = attachment.FileExtenssion,
                                    FileName = attachment.FileName,
                                    Category = attachment.Type,
                                    FilePath = Globals.baseURL + attachment.AttachmentPath.TrimStart('~')
                                };

                                OtherAttachmentsList.Add(otherAttach);
                            }

                            Response.OtherAttachments = OtherAttachmentsList;
                        }
                    }
                }

                return Response;
            }
            catch (Exception ex)
            {
                Response.Result = false;
                Error error = new Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.Message;
                Response.Errors.Add(error);
                return Response;
            }
        }

        public SuppliersCardsResponse GetSuppliersCards(GetSuppliersCardsFilters filters)
        {
            SuppliersCardsResponse Response = new SuppliersCardsResponse();
            Response.Result = true;
            Response.Errors = new List<Error>();
            try
            {
                if (Response.Result)
                {
                  
                    var RegistrationDateFrom = DateTime.Now;
                    var RegistrationDateTo = DateTime.Now;
                    var RegistrationDateFilter = false;
                    if (filters.RegistrationDateFrom!=null)
                    {
                        RegistrationDateFilter = true;

                        RegistrationDateFrom = (DateTime)filters.RegistrationDateFrom;

                        if (filters.RegistrationDateTo!=null)
                        {
                            RegistrationDateTo = (DateTime)filters.RegistrationDateTo;
                        }
                    }
                    else
                    {
                        if (filters.RegistrationDateTo!=null)
                        {
                            Error error = new Error();
                            error.ErrorCode = "Err-13";
                            error.ErrorMSG = "You have to Enter Registration Date From!";
                            Response.Errors.Add(error);
                            Response.Result = false;
                        }
                    }
                    if (filters.GovernorateId!=null && filters.GovernorateId!=0)
                    {
                        if (filters.CountryId != null && filters.CountryId != 0)
                        {
                            var CountryDB = _unitOfWork.Countries.GetById((int)filters.CountryId);
                            if (CountryDB != null)
                            {
                                var GovernorateDB = _unitOfWork.Governorates.GetById((int)filters.GovernorateId);
                                if (GovernorateDB != null)
                                {
                                    if (GovernorateDB.CountryId != filters.CountryId)
                                    {
                                        Response.Result = false;
                                        Error error = new Error();
                                        error.ErrorCode = "Err110";
                                        error.ErrorMSG = "Invalid Governorate For this Country!";
                                        Response.Errors.Add(error);
                                    }
                                }
                                else
                                {
                                    Response.Result = false;
                                    Error error = new Error();
                                    error.ErrorCode = "Err110";
                                    error.ErrorMSG = "This Governorate Doesn't Exist!";
                                    Response.Errors.Add(error);
                                }

                            }
                            else
                            {
                                Response.Result = false;
                                Error error = new Error();
                                error.ErrorCode = "Err110";
                                error.ErrorMSG = "This Country Doesn't Exist!";
                                Response.Errors.Add(error);
                            }

                        }
                        else
                        {
                            Response.Result = false;
                            Error error = new Error();
                            error.ErrorCode = "Err110";
                            error.ErrorMSG = "You must Enter CountryId of This City!";
                            Response.Errors.Add(error);
                        }
                    }
                    else if (filters.CountryId != null && filters.CountryId != 0)
                    {
                        var CountryDB = _unitOfWork.Countries.GetById((int)filters.CountryId);
                        if (CountryDB == null)
                        {
                            Response.Result = false;
                            Error error = new Error();
                            error.ErrorCode = "Err110";
                            error.ErrorMSG = "This Country Doesn't Exist!";
                            Response.Errors.Add(error);
                        }
                    }
                    if (Response.Result)
                    {
                        bool Filtered = false;

                        List<long> MobileSuppliersIds = new List<long>();
                        if (!string.IsNullOrEmpty(filters.Mobile))
                        {
                            MobileSuppliersIds.AddRange(_unitOfWork.SupplierMobiles.FindAll(a => a.Mobile.Contains(filters.Mobile)).Select(a => a.SupplierId).ToList());
                            MobileSuppliersIds.AddRange(_unitOfWork.SupplierContactPeople.FindAll(a => a.Mobile.Contains(filters.Mobile)).Select(a => a.SupplierId).ToList());
                            Filtered = true;
                        }
                        MobileSuppliersIds = MobileSuppliersIds.Distinct().ToList();

                        List<long> PhoneSuppliersIds = new List<long>();
                        if (!string.IsNullOrEmpty(filters.Phone))
                        {
                            PhoneSuppliersIds.AddRange(_unitOfWork.SupplierPhones.FindAll(a => a.Phone.Contains(filters.Phone)).Select(a => a.SupplierId).ToList());
                            Filtered = true;
                        }
                        PhoneSuppliersIds = PhoneSuppliersIds.Distinct().ToList();

                        List<long> FaxSuppliersIds = new List<long>();
                        if (!string.IsNullOrEmpty(filters.Fax))
                        {
                            FaxSuppliersIds.AddRange(_unitOfWork.SupplierFaxes.FindAll(a => a.Fax.Contains(filters.Fax)).Select(a => a.SupplierId).ToList());
                            Filtered = true;
                        }
                        FaxSuppliersIds = FaxSuppliersIds.Distinct().ToList();

                        List<long> SpecialitySuppliersIds = new List<long>();
                        if (filters.SpecialityId != 0)
                        {
                            SpecialitySuppliersIds.AddRange(_unitOfWork.SupplierSpecialities.FindAll(a => a.SpecialityId == filters.SpecialityId).Select(a => a.SupplierId).ToList());
                            Filtered = true;
                        }
                        SpecialitySuppliersIds = SpecialitySuppliersIds.Distinct().ToList();

                        List<long> AddressSuppliersIds = new List<long>();
                        if (filters.GovernorateId != 0 && filters.GovernorateId!=null)
                        {
                            AddressSuppliersIds.AddRange(_unitOfWork.SupplierAddresses.FindAll(a => a.GovernorateId == filters.GovernorateId).Select(a => a.SupplierId).ToList());
                            Filtered = true;
                        }
                        else if (filters.CountryId != 0 && filters.CountryId!=null)
                        {
                            AddressSuppliersIds.AddRange(_unitOfWork.SupplierAddresses.FindAll(a => a.CountryId == filters.CountryId).Select(a => a.SupplierId).ToList());
                            Filtered = true;
                        }
                        if (filters.AreaId != 0)
                        {
                            AddressSuppliersIds.AddRange(_unitOfWork.SupplierAddresses.FindAll(a => a.AreaId == filters.AreaId).Select(a => a.SupplierId).ToList());
                            Filtered = true;
                        }
                        AddressSuppliersIds = AddressSuppliersIds.Distinct().ToList();

                        var SuppliersIds = new List<long>();
                        if (MobileSuppliersIds.Count() > 0)
                        {
                            if (SuppliersIds.Count() == 0)
                                SuppliersIds = MobileSuppliersIds.ToList();
                            else
                                SuppliersIds = SuppliersIds.Intersect(MobileSuppliersIds).ToList();
                        }

                        if (PhoneSuppliersIds.Count() > 0)
                        {
                            if (SuppliersIds.Count() == 0)
                                SuppliersIds = PhoneSuppliersIds.ToList();
                            else
                                SuppliersIds = SuppliersIds.Intersect(PhoneSuppliersIds).ToList();
                        }

                        if (FaxSuppliersIds.Count() > 0)
                        {
                            if (SuppliersIds.Count() == 0)
                                SuppliersIds = FaxSuppliersIds.ToList();
                            else
                                SuppliersIds = SuppliersIds.Intersect(FaxSuppliersIds).ToList();
                        }

                        if (SpecialitySuppliersIds.Count() > 0)
                        {
                            if (SuppliersIds.Count() == 0)
                                SuppliersIds = SpecialitySuppliersIds.ToList();
                            else
                                SuppliersIds = SuppliersIds.Intersect(SpecialitySuppliersIds).ToList();
                        }

                        if (AddressSuppliersIds.Count() > 0)
                        {
                            if (SuppliersIds.Count() == 0)
                                SuppliersIds = AddressSuppliersIds.ToList();
                            else
                                SuppliersIds = SuppliersIds.Intersect(AddressSuppliersIds).ToList();
                        }

                        SuppliersIds = SuppliersIds.Distinct().ToList();

                        var SuppliersDBQuery = _Context.Suppliers.AsQueryable();
                        if (SuppliersIds.Count() > 0 || Filtered)
                        {
                            SuppliersDBQuery = SuppliersDBQuery.Where(a => SuppliersIds.Contains(a.Id));
                        }
                        if (!string.IsNullOrEmpty(filters.SupplierName))
                        {
                            var name = HttpUtility.UrlDecode(filters.SupplierName);
                            SuppliersDBQuery = SuppliersDBQuery.Where(a => a.Name.ToLower().Contains(name)).AsQueryable();
                        }

                        if (RegistrationDateFilter)
                        {
                            SuppliersDBQuery = SuppliersDBQuery.Where(a => a.CreationDate >= RegistrationDateFrom && a.CreationDate <= RegistrationDateTo).AsQueryable();
                        }
                        if (filters.DateSerach != null)
                        {

                            if (filters.RouteId > 0 ||filters.TransportionlineId > 0 || filters.SupplierId > 0 || filters.supplierContactPersonId > 0 || !string.IsNullOrEmpty(filters.serialBus))
                            {
                                 var vehicleRoutes = _transportationLineService.GetVehicleRoutes(filters.RouteId , filters.TransportionlineId, filters.SupplierId, filters.supplierContactPersonId, filters.serialBus);
                                if (!vehicleRoutes.Any())
                                {
                                    Response.Result = false;
                                    Error error = new Error();
                                    error.ErrorCode = "Err10";
                                    error.ErrorMSG = "لا يوجد مسارات نشطة لهذا الفلتر";
                                    Response.Errors.Add(error);
                                    return Response;
                                }
                                var SupplierIds = vehicleRoutes.Select(x => x.SupplierId).ToList();
                                SuppliersDBQuery = SuppliersDBQuery.Where(x => SupplierIds.Contains(x.Id));
                            }
                        }

                        SuppliersDBQuery = SuppliersDBQuery.OrderByDescending(a => a.CreationDate);

                        var SuppliersListDB = PagedList<Supplier>.Create(SuppliersDBQuery, filters.CurrentPage, filters.NumberOfItemsPerPage);

                        var SupplierResponse = new List<SupplierCardData>();
                        foreach (var supplier in SuppliersListDB)
                        {
                            var SupplierDataObj = new SupplierCardData()
                            {
                                Id = supplier.Id,
                                Name = supplier.Name,
                                CreationDate = supplier.CreationDate.ToShortDateString()
                            };
                            if (supplier.Logo != null)
                            {
                                SupplierDataObj.Logo = Convert.ToBase64String(supplier.Logo);
                            }

                            SupplierResponse.Add(SupplierDataObj);
                        }

                        Response.SuppliersList = SupplierResponse;

                        Response.PaginationHeader = new PaginationHeader
                        {
                            CurrentPage = filters.CurrentPage,
                            TotalPages = SuppliersListDB.TotalPages,
                            ItemsPerPage = filters.NumberOfItemsPerPage,
                            TotalItems = SuppliersListDB.TotalCount
                        };
                    }
                }

                return Response;
            }
            catch (Exception ex)
            {
                Response.Result = false;
                Error error = new Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.Message;
                Response.Errors.Add(error);
                return Response;
            }
        }

        public GetSupplierContactPersonsData GetSupplierContactPersonsResponse([FromHeader]long? SupplierId)
        {
            GetSupplierContactPersonsData Response = new GetSupplierContactPersonsData();
            Response.Result = true;
            Response.Errors = new List<Error>();
            try
            {
                if (Response.Result)
                {
                    
                        if (SupplierId==0)                    
                        {
                            Response.Result = false;
                            Error error = new Error();
                            error.ErrorCode = "Err110";
                            error.ErrorMSG = "Must Enter SupplierId ";
                            Response.Errors.Add(error);
                        }
                    

                    if (Response.Result)
                    {
                        var SupplierContactPersonsDB = _unitOfWork.SupplierContactPeople.FindAll(a => a.SupplierId == SupplierId && a.Active == true).ToList();
                        if (SupplierContactPersonsDB != null && SupplierContactPersonsDB.Count > 0)
                        {
                            var SupplierContactPersonsList = new List<GetSupplierContactPerson>();
                            foreach (var CP in SupplierContactPersonsDB)
                            {

                                var SupplierContactPerson = new GetSupplierContactPerson()
                                {
                                    ID = CP.Id,
                                    Name = CP.Name,
                                    Email = CP.Email,
                                    Location = CP.Location,
                                    Mobile = CP.Mobile,
                                    Title = CP.Title
                                };
                                SupplierContactPersonsList.Add(SupplierContactPerson);
                            }
                            Response.SupplierContactPersonData = SupplierContactPersonsList;
                        }
                    }
                }

                return Response;
            }
            catch (Exception ex)
            {
                Response.Result = false;
                Error error = new Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.Message;
                Response.Errors.Add(error);
                return Response;
            }
        }

        public CheckSupplierExistanceResponse CheckSupplierExistance(CheckSupplierExistanceFilters filters)
        {
            CheckSupplierExistanceResponse Response = new CheckSupplierExistanceResponse();
            Response.Result = true;
            Response.Errors = new List<Error>();
            try
            {
                if (Response.Result)
                {
                    if (Response.Result)
                    {
                        var SuppliersDBQuery = _unitOfWork.Suppliers.FindAllQueryable(a=>true);

                        List<Supplier> SuppliersDb = new List<Supplier>();

                        if (!string.IsNullOrEmpty(filters.SupplierName))
                        {
                            var name = HttpUtility.UrlDecode(filters.SupplierName);
                            SuppliersDb = SuppliersDBQuery.Where(a => a.Name.ToLower() == name).ToList();
                        }
                        if (!string.IsNullOrEmpty(filters.SupplierMobile))
                        {
                            var SuppliersIds = _unitOfWork.SupplierMobiles.FindAll(x => x.Mobile == filters.SupplierMobile).Select(x => x.SupplierId).ToList();
                            if (SuppliersIds != null && SuppliersIds.Count > 0)
                            {
                                SuppliersDb = SuppliersDBQuery.Where(a => SuppliersIds.Contains(a.Id)).ToList();
                            }
                        }
                        if (!string.IsNullOrEmpty(filters.SupplierPhone))
                        {
                            var SuppliersIds = _unitOfWork.SupplierPhones.FindAll(x => x.Phone == filters.SupplierPhone).Select(x => x.SupplierId).ToList();
                            if (SuppliersIds != null && SuppliersIds.Count > 0)
                            {
                                SuppliersDb = SuppliersDBQuery.Where(a => SuppliersIds.Contains(a.Id)).ToList();
                            }
                        }
                        if (!string.IsNullOrEmpty(filters.SupplierFax))
                        {
                            var SuppliersIds = _unitOfWork.SupplierFaxes.FindAll(x => x.Fax == filters.SupplierFax).Select(x => x.SupplierId).ToList();
                            if (SuppliersIds != null && SuppliersIds.Count > 0)
                            {
                                SuppliersDb = SuppliersDBQuery.Where(a => SuppliersIds.Contains(a.Id)).ToList();
                            }
                        }
                        if (!string.IsNullOrEmpty(filters.SupplierEmail))
                        {
                            SuppliersDb = SuppliersDBQuery.Where(a => a.Email.ToLower() == filters.SupplierEmail).ToList();

                        }
                        if (!string.IsNullOrEmpty(filters.SupplierWebsite))
                        {
                            SuppliersDb = SuppliersDBQuery.Where(a => a.WebSite.ToLower() == filters.SupplierWebsite).ToList();
                        }
                        if (!string.IsNullOrEmpty(filters.ContactPersonMobile))
                        {
                            var SuppliersIds = _unitOfWork.SupplierContactPeople.FindAll(x => x.Mobile == filters.ContactPersonMobile).Select(x => x.SupplierId).ToList();
                            if (SuppliersIds != null && SuppliersIds.Count > 0)
                            {
                                SuppliersDb = SuppliersDBQuery.Where(a => SuppliersIds.Contains(a.Id)).ToList();
                            }
                        }
                        if (SuppliersDb != null && SuppliersDb.Count > 0)
                        {
                            var SuppliersList = SuppliersDb.Select(a => new SelectDDL()
                            {
                                ID = a.Id,
                                Name = a.Name
                            }).ToList();

                            Response.SuppliersList = SuppliersList;
                            Response.IsExist = true;
                            Response.SuppliersCount = SuppliersList.Count();
                        }
                    }
                }

                return Response;
            }
            catch (Exception ex)
            {
                Response.Result = false;
                Error error = new Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.InnerException != null ? ex.InnerException.Message : ex.Message; ;
                Response.Errors.Add(error);
                return Response;
            }
        }

        public BaseResponseWithId<long> AddSupplierAttachments([FromForm]SupplierAttachmentsData Request)
        {
            BaseResponseWithId<long> Response = new BaseResponseWithId<long>();
            Response.Result = true;
            Response.Errors = new List<Error>();
            try
            {
                if (Response.Result)
                {

                    //check sent data
                    if (Request == null)
                    {
                        Response.Result = false;
                        Error error = new Error();
                        error.ErrorCode = "Err-P12";
                        error.ErrorMSG = "Please Insert a Valid Data.";
                        Response.Errors.Add(error);
                        return Response;
                    }

                    long SupplierId = 0;
                    if (Request.SupplierId != 0)
                    {
                        SupplierId = Request.SupplierId;
                    }
                    else
                    {
                        Response.Result = false;
                        Error error = new Error();
                        error.ErrorCode = "Err25";
                        error.ErrorMSG = "SupplierId Is Mandatory";
                        Response.Errors.Add(error);
                    }
                    if (Response.Result)
                    {
                        if (Request.LicenseAttachements != null && Request.LicenseAttachements.Count > 0)
                        {
                            var counter = 0;
                            foreach (var attachment in Request.LicenseAttachements)
                            {
                                counter++;
                                if (attachment.Id == null)
                                {
                                    if (attachment.File == null)
                                    {
                                        Response.Result = false;
                                        Error error = new Error();
                                        error.ErrorCode = "Err25";
                                        error.ErrorMSG = "Line " + counter + ": File Content Is Mandatory!";
                                        Response.Errors.Add(error);
                                    }

                                }
                            }

                            counter = 0;
                            if (Response.Result)
                            {
                                counter++;
                                foreach (var attachment in Request.LicenseAttachements)
                                {
                                    if (attachment.Id != null && attachment.Active == false)
                                    {
                                        // Delete
                                        var LicenseAttachmentDb = _unitOfWork.SupplierAttachments.GetById((long)attachment.Id);
                                        if (LicenseAttachmentDb != null)
                                        {
                                            var AttachmentPath = _host.WebRootPath + "/" + LicenseAttachmentDb.AttachmentPath;

                                            if (File.Exists(AttachmentPath))
                                            {
                                                File.Delete(AttachmentPath);
                                                _unitOfWork.SupplierAttachments.Delete(LicenseAttachmentDb);
                                                var LicenseAttachmentDelete = _unitOfWork.Complete;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var fileExtension = attachment.File.FileName.Split('.').Last();
                                        var virtualPath = $"Attachments\\{validation.CompanyName}\\Suppliers\\{SupplierId}\\";
                                        var FileName = System.IO.Path.GetFileNameWithoutExtension(attachment.File.FileName);
                                        var FilePath = Common.SaveFileIFF(virtualPath, attachment.File, FileName, fileExtension, _host);

                                        // Insert
                                        _unitOfWork.SupplierAttachments.Add(new SupplierAttachment()
                                        {
                                            SupplierId = SupplierId,
                                            AttachmentPath = FilePath,
                                            CreatedBy = validation.userID,
                                            CreationDate = DateTime.Now,
                                            ModifiedBy = validation.userID,
                                            Modified = DateTime.Now,
                                            FileName = FileName,
                                            FileExtenssion = fileExtension,
                                            Active = true,
                                            Type = "License"
                                        });
                                        _unitOfWork.Complete();
                                    }
                                }
                            }
                        }

                        if (Request.BrochuresAttachments != null && Request.BrochuresAttachments.Count > 0)
                        {
                            var counter = 0;
                            foreach (var attachment in Request.BrochuresAttachments)
                            {
                                counter++;
                                if (attachment.Id == null)
                                {
                                    if (attachment.File == null)
                                    {
                                        Response.Result = false;
                                        Error error = new Error();
                                        error.ErrorCode = "Err25";
                                        error.ErrorMSG = "Brochures Attachments Line " + counter + ": File Content Is Mandatory!";
                                        Response.Errors.Add(error);
                                    }
                                }
                            }

                            counter = 0;
                            if (Response.Result)
                            {
                                counter++;
                                foreach (var attachment in Request.BrochuresAttachments)
                                {
                                    if (attachment.Id != null && attachment.Active == false)
                                    {
                                        // Delete
                                        var BrochuresAttachmentDb = _unitOfWork.SupplierAttachments.GetById((long)attachment.Id);
                                        if (BrochuresAttachmentDb != null)
                                        {
                                            var AttachmentPath = _host.WebRootPath + "/" + BrochuresAttachmentDb.AttachmentPath;

                                            if (File.Exists(AttachmentPath))
                                            {
                                                File.Delete(AttachmentPath);
                                                _unitOfWork.SupplierAttachments.Delete(BrochuresAttachmentDb);
                                                var BrochuresAttachmentDelete = _unitOfWork.Complete();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var fileExtension = attachment.File.FileName.Split('.').Last();
                                        var virtualPath = $"Attachments\\{validation.CompanyName}\\Suppliers\\{SupplierId}\\";
                                        var FileName = System.IO.Path.GetFileNameWithoutExtension(attachment.File.FileName);
                                        var FilePath = Common.SaveFileIFF(virtualPath, attachment.File, FileName, fileExtension, _host);
                                        _unitOfWork.SupplierAttachments.Add(new SupplierAttachment()
                                        {
                                            SupplierId = SupplierId,
                                            AttachmentPath = FilePath,
                                            CreatedBy = validation.userID,
                                            CreationDate = DateTime.Now,
                                            ModifiedBy = validation.userID,
                                            Modified = DateTime.Now,
                                            FileName = FileName,
                                            FileExtenssion = fileExtension,
                                            Active = true,
                                            Type = "Brochure"
                                        });
                                        _unitOfWork.Complete();
                                    }
                                }
                            }
                        }

                        if (Request.BussinessCardsAttachments != null && Request.BussinessCardsAttachments.Count > 0)
                        {
                            var counter = 0;
                            foreach (var attachment in Request.BussinessCardsAttachments)
                            {
                                counter++;
                                if (attachment.Id == null)
                                {
                                    if (attachment.File == null)
                                    {
                                        Response.Result = false;
                                        Error error = new Error();
                                        error.ErrorCode = "Err25";
                                        error.ErrorMSG = "Bussiness Card Attachments Line " + counter + ": File Content Is Mandatory!";
                                        Response.Errors.Add(error);
                                    }
                                }
                            }

                            counter = 0;
                            if (Response.Result)
                            {
                                counter++;
                                foreach (var attachment in Request.BussinessCardsAttachments)
                                {
                                    if (attachment.Id != null && attachment.Active == false)
                                    {
                                        // Delete
                                        var BussinessCardAttachmentDb = _unitOfWork.SupplierAttachments.GetById((long)attachment.Id);
                                        if (BussinessCardAttachmentDb != null)
                                        {
                                            var AttachmentPath = _host.WebRootPath + "/" + BussinessCardAttachmentDb.AttachmentPath;

                                            if (File.Exists(AttachmentPath))
                                            {
                                                File.Delete(AttachmentPath);
                                                _unitOfWork.SupplierAttachments.Delete(BussinessCardAttachmentDb);
                                                var BussinessCardAttachmenDelete = _unitOfWork.Complete();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var fileExtension = attachment.File.FileName.Split('.').Last();
                                        var virtualPath = $"Attachments\\{validation.CompanyName}\\Suppliers\\{SupplierId}\\";
                                        var FileName = System.IO.Path.GetFileNameWithoutExtension(attachment.File.FileName);
                                        var FilePath = Common.SaveFileIFF(virtualPath, attachment.File, FileName, fileExtension, _host);
                                        // Insert

                                        _unitOfWork.SupplierAttachments.Add(new SupplierAttachment()
                                        {
                                            SupplierId = SupplierId,
                                            AttachmentPath = FilePath,
                                            CreatedBy = validation.userID,
                                            CreationDate = DateTime.Now,
                                            ModifiedBy = validation.userID,
                                            Modified = DateTime.Now,
                                            FileName = FileName,
                                            FileExtenssion = fileExtension,
                                            Active = true,
                                            Type = "Business Cards"
                                        });
                                        _unitOfWork.Complete();
                                    }
                                }
                            }
                        }

                        if (Request.OtherAttachments != null && Request.OtherAttachments.Count > 0)
                        {
                            var counter = 0;
                            foreach (var attachment in Request.OtherAttachments)
                            {
                                counter++;
                                if (attachment.Id == null)
                                {
                                    if (attachment.File == null)
                                    {
                                        Response.Result = false;
                                        Error error = new Error();
                                        error.ErrorCode = "Err25";
                                        error.ErrorMSG = "Other Attachments Line " + counter + ": File Content Is Mandatory!";
                                        Response.Errors.Add(error);
                                    }
                                }
                            }

                            counter = 0;
                            if (Response.Result)
                            {
                                counter++;
                                foreach (var attachment in Request.OtherAttachments)
                                {
                                    if (attachment.Id != null && attachment.Active == false)
                                    {
                                        // Delete
                                        var OtherAttachmentDb = _unitOfWork.SupplierAttachments.GetById((long)attachment.Id);
                                        if (OtherAttachmentDb != null)
                                        {
                                            var AttachmentPath = _host.WebRootPath + "/" + OtherAttachmentDb.AttachmentPath;

                                            if (File.Exists(AttachmentPath))
                                            {
                                                File.Delete(AttachmentPath);
                                                _unitOfWork.SupplierAttachments.Delete(OtherAttachmentDb);
                                                var OtherAttachmentDelete = _unitOfWork.Complete();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var fileExtension = attachment.File.FileName.Split('.').Last();
                                        var virtualPath = $"Attachments\\{validation.CompanyName}\\Suppliers\\{SupplierId}\\";
                                        var FileName = System.IO.Path.GetFileNameWithoutExtension(attachment.File.FileName);
                                        var FilePath = Common.SaveFileIFF(virtualPath, attachment.File, FileName, fileExtension, _host);

                                        _unitOfWork.SupplierAttachments.Add(new SupplierAttachment()
                                        {
                                            SupplierId = SupplierId,
                                            AttachmentPath = FilePath,
                                            CreatedBy = validation.userID,
                                            CreationDate = DateTime.Now,
                                            ModifiedBy = validation.userID,
                                            Modified = DateTime.Now,
                                            FileName = FileName,
                                            FileExtenssion = fileExtension,
                                            Active = true,
                                            Type = "Other"
                                        });
                                        _unitOfWork.Complete();
                                    }
                                }
                            }
                        }

                        if (Request.TaxCardAttachment != null)
                        {
                            if (Request.TaxCardAttachment.Id == null)
                            {
                                if (Request.TaxCardAttachment.File == null)
                                {
                                    Response.Result = false;
                                    Error error = new Error();
                                    error.ErrorCode = "Err25";
                                    error.ErrorMSG = "Tax Card Attachment File Content Is Mandatory!";
                                    Response.Errors.Add(error);
                                }
                            }

                            if (Response.Result)
                            {
                                if (Request.TaxCardAttachment.Id != null && Request.TaxCardAttachment.Active == false)
                                {
                                    // Delete
                                    var TaxCardAttachmentDb = _unitOfWork.SupplierAttachments.GetById((long)Request.TaxCardAttachment.Id);
                                    if (TaxCardAttachmentDb != null)
                                    {
                                        var AttachmentPath = _host.WebRootPath + "/" + TaxCardAttachmentDb.AttachmentPath;

                                        if (File.Exists(AttachmentPath))
                                        {
                                            File.Delete(AttachmentPath);
                                            _unitOfWork.SupplierAttachments.Delete(TaxCardAttachmentDb);
                                            var TaxCardAttachmentDelete = _unitOfWork.Complete();
                                        }
                                    }
                                }
                                else
                                {
                                    var fileExtension = Request.TaxCardAttachment.File.FileName.Split('.').Last();
                                    var virtualPath = $"Attachments\\{validation.CompanyName}\\Suppliers\\{SupplierId}\\";
                                    var FileName = System.IO.Path.GetFileNameWithoutExtension(Request.TaxCardAttachment.File.FileName);
                                    var FilePath = Common.SaveFileIFF(virtualPath, Request.TaxCardAttachment.File, FileName, fileExtension, _host);

                                    _unitOfWork.SupplierAttachments.Add(new SupplierAttachment()
                                    {
                                        SupplierId = SupplierId,
                                        AttachmentPath = FilePath,
                                        CreatedBy = validation.userID,
                                        CreationDate = DateTime.Now,
                                        ModifiedBy = validation.userID,
                                        Modified = DateTime.Now,
                                        FileName = FileName,
                                        FileExtenssion = fileExtension,
                                        Active = true,
                                        Type = "Tax Card"
                                    });
                                    _unitOfWork.Complete();
                                }
                            }
                        }
                        if (Request.CommercialRecordAttachment != null)
                        {
                            if (Request.CommercialRecordAttachment.Id == null)
                            {
                                if (Request.CommercialRecordAttachment.File == null)
                                {
                                    Response.Result = false;
                                    Error error = new Error();
                                    error.ErrorCode = "Err25";
                                    error.ErrorMSG = "Commercial Record Attachment File Content Is Mandatory!";
                                    Response.Errors.Add(error);
                                }
                            }

                            if (Response.Result)
                            {
                                if (Request.CommercialRecordAttachment.Id != null && Request.CommercialRecordAttachment.Active == false)
                                {
                                    // Delete
                                    var CommercialRecordAttachmentDb = _unitOfWork.SupplierAttachments.GetById((long)Request.CommercialRecordAttachment.Id);
                                    if (CommercialRecordAttachmentDb != null)
                                    {
                                        var AttachmentPath = _host.WebRootPath + "/" + CommercialRecordAttachmentDb.AttachmentPath;

                                        if (File.Exists(AttachmentPath))
                                        {
                                            File.Delete(AttachmentPath);
                                            _unitOfWork.SupplierAttachments.Delete(CommercialRecordAttachmentDb);
                                            var CommercialRecordAttachmentDelete = _unitOfWork.Complete();
                                        }
                                    }
                                }
                                else
                                {
                                    var fileExtension = Request.CommercialRecordAttachment.File.FileName.Split('.').Last();
                                    var virtualPath = $"Attachments\\{validation.CompanyName}\\Suppliers\\{SupplierId}\\";
                                    var FileName = System.IO.Path.GetFileNameWithoutExtension(Request.CommercialRecordAttachment.File.FileName);
                                    var FilePath = Common.SaveFileIFF(virtualPath, Request.CommercialRecordAttachment.File, FileName, fileExtension, _host);
                                    // Insert

                                    _unitOfWork.SupplierAttachments.Add(new SupplierAttachment()
                                    {
                                        SupplierId = SupplierId,
                                        AttachmentPath = FilePath,
                                        CreatedBy = validation.userID,
                                        CreationDate = DateTime.Now,
                                        ModifiedBy = validation.userID,
                                        Modified = DateTime.Now,
                                        FileName = FileName,
                                        FileExtenssion = fileExtension,
                                        Active = true,
                                        Type = "Commercial Record"
                                    });
                                    _unitOfWork.Complete();
                                }

                            }
                        }
                    }
                }
                
                return Response;
                        
            }
            catch (Exception ex)
            {
                Response.Result = false;
                Error error = new Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.InnerException != null ? ex.InnerException.Message : ex.Message; ;
                Response.Errors.Add(error);
                return Response;
            }
        }

        public async Task<BaseResponseWithId<long>> AddSupplierTaxCard(SupplierTaxCardData Request)
        {
            BaseResponseWithId<long> Response = new BaseResponseWithId<long>();
            Response.Result = true;
            Response.Errors = new List<Error>();
            try
            {
                if (Response.Result)
                {
                    if (Request == null)
                    {
                        Response.Result = false;
                        Error error = new Error();
                        error.ErrorCode = "Err-P12";
                        error.ErrorMSG = "Please Insert a Valid Data.";
                        Response.Errors.Add(error);
                        return Response;
                    }

                    long SupplierId = 0;
                    if (Request.SupplierId != 0)
                    {
                        var SupplierDb = await _unitOfWork.Suppliers.GetByIdAsync(Request.SupplierId);
                        if (SupplierDb != null)
                        {
                            SupplierId = Request.SupplierId;
                        }
                        else
                        {
                            Response.Result = false;
                            Error error = new Error();
                            error.ErrorCode = "Err25";
                            error.ErrorMSG = "This Supplier Doesn't Exist";
                            Response.Errors.Add(error);
                        }
                    }
                    else
                    {
                        Response.Result = false;
                        Error error = new Error();
                        error.ErrorCode = "Err25";
                        error.ErrorMSG = "SupplierId Is Mandatory";
                        Response.Errors.Add(error);
                    }

                    if (string.IsNullOrEmpty(Request.TaxCard))
                    {
                        Response.Result = false;
                        Error error = new Error();
                        error.ErrorCode = "Err25";
                        error.ErrorMSG = "TaxCard Is Mandatory";
                        Response.Errors.Add(error);
                    }

                    if (Response.Result)
                    {
                        var SupplierDb = await _unitOfWork.Suppliers.GetByIdAsync(SupplierId);
                        SupplierDb.TaxCard = Request.TaxCard;
                        var Result = _unitOfWork.Complete();
                        if (Result > 0)
                        {
                            Response.ID = SupplierId;
                        }
                        else
                        {
                            Response.Result = false;
                            Error error = new Error();
                            error.ErrorCode = "Err25";
                            error.ErrorMSG = "Faild To Update This Record, Or Updated With The Same Value";
                            Response.Errors.Add(error);
                        }
                    }
                }
                return Response;
            }
            catch (Exception ex)
            {
                Response.Result = false;
                Error error = new Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.InnerException != null ? ex.InnerException.Message : ex.Message; ;
                Response.Errors.Add(error);
                return Response;
            }
        }

        public BaseResponseWithId<long> AddNewSupplierContacts(SupplierContactsData Request)
        {
            BaseResponseWithId<long> Response = new BaseResponseWithId<long>();
            Response.Result = true;
            Response.Errors = new List<Error>();
            try
            {
                if (Response.Result)
                {
                    //check sent data
                    if (Request == null)
                    {
                        Response.Result = false;
                        Error error = new Error();
                        error.ErrorCode = "Err-P12";
                        error.ErrorMSG = "Please Insert a Valid Data.";
                        Response.Errors.Add(error);
                        return Response;
                    }

                    long SupplierId = 0;
                    if (Request.SupplierId != 0)
                    {
                        SupplierId = Request.SupplierId;
                        var supplier = _unitOfWork.Suppliers.GetById(SupplierId);
                        if (supplier == null)
                        {
                            Response.Result = false;
                            Error error = new Error();
                            error.ErrorCode = "ErrCRM1";
                            error.ErrorMSG = "This Supplier Doesn't Exist!!";
                            Response.Errors.Add(error);
                        }
                    }
                    else
                    {
                        Response.Result = false;
                        Error error = new Error();
                        error.ErrorCode = "Err25";
                        error.ErrorMSG = "Supplier Id Is Mandatory";
                        Response.Errors.Add(error);
                    }

                    if (Response.Result)
                    {
                        //Add-Edit supplier Addresses
                        if (Request.SupplierAddresses != null)
                        {
                            if (Request.SupplierAddresses.Count > 0)
                            {
                                var counter = 0;
                                foreach (var Adrs in Request.SupplierAddresses)
                                {
                                    counter++;

                                    string CreatedByString = null;
                                    string ModifiedByString = null;
                                    long? CreatedBy = 0;
                                    long? ModifiedBy = 0;
                                    if (Adrs.ID != 0 && Adrs.ID != null)
                                    {
                                        if (Adrs.ModifiedBy != null)
                                        {
                                            ModifiedByString = Adrs.ModifiedBy;
                                        }
                                        else
                                        {
                                            Response.Result = false;
                                            Error error = new Error();
                                            error.ErrorCode = "Err25";
                                            error.ErrorMSG = "Line: " + counter + ", " + "Message: Modified By Id Is Mandatory";
                                            Response.Errors.Add(error);
                                        }
                                    }
                                    else
                                    {
                                        if (Adrs.CreatedBy != null)
                                        {
                                            CreatedByString = Adrs.CreatedBy;
                                        }
                                        else
                                        {
                                            Response.Result = false;
                                            Error error = new Error();
                                            error.ErrorCode = "Err25";
                                            error.ErrorMSG = "Line: " + counter + ", " + "Created By Id Is Mandatory";
                                            Response.Errors.Add(error);
                                        }
                                    }

                                    int CountryId = 0;
                                    if (Adrs.CountryID != 0)
                                    {
                                        CountryId = Adrs.CountryID;
                                        var country = _unitOfWork.Countries.GetById(CountryId);
                                        if (country == null)
                                        {
                                            Response.Result = false;
                                            Error error = new Error();
                                            error.ErrorCode = "ErrCRM1";
                                            error.ErrorMSG = "This Country Doesn't Exist!!";
                                            Response.Errors.Add(error);
                                        }
                                    }
                                    else
                                    {
                                        Response.Result = false;
                                        Error error = new Error();
                                        error.ErrorCode = "Err25";
                                        error.ErrorMSG = "Line: " + counter + ", " + "Address Country ID Is Mandatory";
                                        Response.Errors.Add(error);
                                    }

                                    int GovernorateId = 0;
                                    if (Adrs.GovernorateID != 0)
                                    {
                                        GovernorateId = Adrs.GovernorateID;
                                        var governorate = _unitOfWork.Governorates.GetById(GovernorateId);
                                        if (governorate == null)
                                        {
                                            Response.Result = false;
                                            Error error = new Error();
                                            error.ErrorCode = "ErrCRM1";
                                            error.ErrorMSG = "This City Doesn't Exist!!";
                                            Response.Errors.Add(error);
                                        }
                                    }
                                    else
                                    {
                                        Response.Result = false;
                                        Error error = new Error();
                                        error.ErrorCode = "Err25";
                                        error.ErrorMSG = "Line: " + counter + ", " + "Address Governorate ID Is Mandatory";
                                        Response.Errors.Add(error);
                                    }

                                    long? AreaId = 0;
                                    if (Adrs.AreaID != 0 && Adrs.AreaID!=null)
                                    {
                                        AreaId = Adrs.AreaID;
                                        var area = _unitOfWork.Areas.GetById((long)AreaId);
                                        if (area == null)
                                        {
                                            Response.Result = false;
                                            Error error = new Error();
                                            error.ErrorCode = "ErrCRM1";
                                            error.ErrorMSG = "This Area Doesn't Exist!!";
                                            Response.Errors.Add(error);
                                        }
                                    }
                                    else
                                    {
                                        Response.Result = false;
                                        Error error = new Error();
                                        error.ErrorCode = "Err25";
                                        error.ErrorMSG = "Line: " + counter + ", " + "Address Area ID Is Mandatory";
                                        Response.Errors.Add(error);
                                    }

                                    string Address = null;
                                    if (!string.IsNullOrEmpty(Adrs.Address))
                                    {
                                        Address = Adrs.Address;
                                    }
                                    else
                                    {
                                        Response.Result = false;
                                        Error error = new Error();
                                        error.ErrorCode = "Err25";
                                        error.ErrorMSG = "Line: " + counter + ", " + "Address Is Mandatory";
                                        Response.Errors.Add(error);
                                    }
                                }
                            }
                            if (Response.Result)
                            {
                                foreach (var Adrs in Request.SupplierAddresses)
                                {
                                    int BuildingNumber = 0;
                                    int Floor = 0;
                                    if (!string.IsNullOrEmpty(Adrs.BuildingNumber) && int.TryParse(Adrs.BuildingNumber, out BuildingNumber))
                                    {
                                        int.TryParse(Adrs.BuildingNumber, out BuildingNumber);
                                    }
                                    if (!string.IsNullOrEmpty(Adrs.Floor) && int.TryParse(Adrs.Floor, out Floor))
                                    {
                                        int.TryParse(Adrs.Floor, out Floor);
                                    }
                                    if (Adrs.ID != null && Adrs.ID != 0)
                                    {
                                        var AddressDb = _unitOfWork.SupplierAddresses.GetById((long)Adrs.ID);
                                        if (AddressDb != null)
                                        {
                                            // Update
                                            AddressDb.SupplierId = Request.SupplierId;
                                            AddressDb.CountryId = Adrs.CountryID;
                                            AddressDb.GovernorateId = Adrs.GovernorateID;
                                            AddressDb.Address = Adrs.Address;
                                            AddressDb.ModifiedBy = validation.userID;
                                            AddressDb.Modified = DateTime.Now;
                                            AddressDb.Active = true;
                                            AddressDb.BuildingNumber = BuildingNumber.ToString();
                                            AddressDb.Floor = Floor.ToString();
                                            AddressDb.Description = Adrs.Description;
                                            AddressDb.AreaId = Adrs.AreaID;
                                            _unitOfWork.SupplierAddresses.Update(AddressDb);
                                            _unitOfWork.Complete();
                                        }
                                        else
                                        {
                                            Response.Result = false;
                                            Error error = new Error();
                                            error.ErrorCode = "Err25";
                                            error.ErrorMSG = "This Address Doesn't Exist!!";
                                            Response.Errors.Add(error);
                                        }
                                    }
                                    else
                                    {
                                        // Insert

                                        var AddressDb = new SupplierAddress();
                                        AddressDb.SupplierId = Request.SupplierId;
                                        AddressDb.CountryId = Adrs.CountryID;
                                        AddressDb.GovernorateId = Adrs.GovernorateID;
                                        AddressDb.Address = Adrs.Address;
                                        AddressDb.CreatedBy = validation.userID;
                                        AddressDb.ModifiedBy = validation.userID;
                                        AddressDb.CreationDate = DateTime.Now;
                                        AddressDb.Modified = DateTime.Now;
                                        AddressDb.Active = true;
                                        AddressDb.BuildingNumber = BuildingNumber.ToString();
                                        AddressDb.Floor = Floor.ToString();
                                        AddressDb.Description = Adrs.Description;
                                        AddressDb.AreaId = Adrs.AreaID;
                                        _unitOfWork.SupplierAddresses.Add(AddressDb);
                                        _unitOfWork.Complete();
                                    }
                                }
                            }
                        }
                        if (Request.SupplierLandLines != null)
                        {
                            if (Request.SupplierLandLines.Count > 0)
                            {
                                var counter = 0;
                                foreach (var LND in Request.SupplierLandLines)
                                {
                                    counter++;

                                    string CreatedByString = null;
                                    string ModifiedByString = null;
                                    long? CreatedBy = 0;
                                    long? ModifiedBy = 0;
                                    if (LND.ID != 0 && LND.ID != null)
                                    {
                                        if (LND.ModifiedBy != null)
                                        {
                                            ModifiedByString = LND.ModifiedBy;
                                        }
                                        else
                                        {
                                            Response.Result = false;
                                            Error error = new Error();
                                            error.ErrorCode = "Err25";
                                            error.ErrorMSG = "Line: " + counter + ", " + "Message: Modified By Id Is Mandatory";
                                            Response.Errors.Add(error);
                                        }
                                        ModifiedBy = long.Parse(Encrypt_Decrypt.Decrypt(ModifiedByString, key));
                                    }
                                    else
                                    {
                                        if (LND.CreatedBy != null)
                                        {
                                            CreatedByString = LND.CreatedBy;
                                        }
                                        else
                                        {
                                            Response.Result = false;
                                            Error error = new Error();
                                            error.ErrorCode = "Err25";
                                            error.ErrorMSG = "Line: " + counter + ", " + "Created By Id Is Mandatory";
                                            Response.Errors.Add(error);
                                        }
                                    }

                                    string LandLine = null;
                                    if (!string.IsNullOrEmpty(LND.LandLine))
                                    {
                                        var ExistPhone = _unitOfWork.SupplierPhones.FindAll(a => a.Phone == LND.LandLine).FirstOrDefault();
                                        if (ExistPhone != null)
                                        {
                                            if (LND.ID == null)
                                            {
                                                Response.Result = false;
                                                Error error = new Error();
                                                error.ErrorCode = "Err25";
                                                error.ErrorMSG = "Line: " + counter + ", " + "Land Line Is Exist Before";
                                                Response.Errors.Add(error);
                                            }
                                            else
                                            {
                                                LandLine = LND.LandLine;
                                            }
                                        }
                                        else
                                        {
                                            LandLine = LND.LandLine;
                                        }
                                    }
                                    else
                                    {
                                        Response.Result = false;
                                        Error error = new Error();
                                        error.ErrorCode = "Err25";
                                        error.ErrorMSG = "Line: " + counter + ", " + "Land Line Is Mandatory";
                                        Response.Errors.Add(error);
                                    }
                                }
                            }
                            if (Response.Result)
                            {
                                foreach (var LND in Request.SupplierLandLines)
                                {
                                    if (LND.ID != null && LND.ID != 0)
                                    {
                                        var SupplierLandLineDb = _unitOfWork.SupplierPhones.GetById((long)LND.ID);
                                        if (SupplierLandLineDb != null)
                                        {
                                            // Update
                                            SupplierLandLineDb.SupplierId = Request.SupplierId;
                                            SupplierLandLineDb.Phone = LND.LandLine;
                                            SupplierLandLineDb.ModifiedBy = validation.userID;
                                            SupplierLandLineDb.Modified = DateTime.Now;
                                            SupplierLandLineDb.Active =true;
                                            _unitOfWork.SupplierPhones.Update(SupplierLandLineDb);
                                            _unitOfWork.Complete();
                                        }
                                        else
                                        {
                                            Response.Result = false;
                                            Error error = new Error();
                                            error.ErrorCode = "Err25";
                                            error.ErrorMSG = "This Land Line Doesn't Exist!!";
                                            Response.Errors.Add(error);
                                        }
                                    }
                                    else
                                    {
                                        // Insert
                                        var SupplierLandLineDb = new SupplierPhone();
                                        SupplierLandLineDb.SupplierId = Request.SupplierId;
                                        SupplierLandLineDb.Phone = LND.LandLine;
                                        SupplierLandLineDb.CreatedBy = validation.userID;
                                        SupplierLandLineDb.ModifiedBy = validation.userID;
                                        SupplierLandLineDb.CreationDate = DateTime.Now;
                                        SupplierLandLineDb.Modified = DateTime.Now;
                                        SupplierLandLineDb.Active = true;
                                        _unitOfWork.SupplierPhones.Add(SupplierLandLineDb);
                                        _unitOfWork.Complete();
                                    }
                                }
                            }
                        }

                        //Add-Edit supplier Mobiles
                        if (Request.SupplierMobiles != null)
                        {
                            if (Request.SupplierMobiles.Count > 0)
                            {
                                var counter = 0;
                                foreach (var MOB in Request.SupplierMobiles)
                                {
                                    counter++;


                                    string CreatedByString = null;
                                    string ModifiedByString = null;
                                    long? CreatedBy = 0;
                                    long? ModifiedBy = 0;
                                    if (MOB.ID != 0 && MOB.ID != null)
                                    {
                                        if (MOB.ModifiedBy != null)
                                        {
                                            ModifiedByString = MOB.ModifiedBy;
                                        }
                                        else
                                        {
                                            Response.Result = false;
                                            Error error = new Error();
                                            error.ErrorCode = "Err25";
                                            error.ErrorMSG = "Line: " + counter + ", " + "Message: Modified By Id Is Mandatory";
                                            Response.Errors.Add(error);
                                        }
                                    }
                                    else
                                    {
                                        if (MOB.CreatedBy != null)
                                        {
                                            CreatedByString = MOB.CreatedBy;
                                        }
                                        else
                                        {
                                            Response.Result = false;
                                            Error error = new Error();
                                            error.ErrorCode = "Err25";
                                            error.ErrorMSG = "Line: " + counter + ", " + "Created By Id Is Mandatory";
                                            Response.Errors.Add(error);
                                        }
                                    }

                                    string Mobile = null;
                                    if (!string.IsNullOrEmpty(MOB.Mobile))
                                    {
                                        var ExistMobile = _unitOfWork.SupplierMobiles.FindAll(a => a.Mobile == MOB.Mobile).FirstOrDefault();
                                        if (ExistMobile != null)
                                        {
                                            if (MOB.ID == null)
                                            {
                                                Response.Result = false;
                                                Error error = new Error();
                                                error.ErrorCode = "Err25";
                                                error.ErrorMSG = "Line: " + counter + ", " + "Mobile Is Exist Before";
                                                Response.Errors.Add(error);
                                            }
                                            else
                                            {
                                                Mobile = MOB.Mobile;
                                            }
                                        }
                                        else
                                        {
                                            Mobile = MOB.Mobile;
                                        }
                                    }
                                    else
                                    {
                                        Response.Result = false;
                                        Error error = new Error();
                                        error.ErrorCode = "Err25";
                                        error.ErrorMSG = "Line: " + counter + ", " + "Mobile Is Mandatory";
                                        Response.Errors.Add(error);
                                    }
                                }
                            }
                            if (Response.Result)
                            {
                                foreach (var MOB in Request.SupplierMobiles)
                                {
                                    if (MOB.ID != null && MOB.ID != 0)
                                    {
                                        var SupplierMobileDb = _unitOfWork.SupplierMobiles.GetById((long)MOB.ID);
                                        if (SupplierMobileDb != null)
                                        {
                                            // Update
                                            SupplierMobileDb.SupplierId = Request.SupplierId;
                                            SupplierMobileDb.Mobile = MOB.Mobile;
                                            SupplierMobileDb.ModifiedBy = validation.userID;
                                            SupplierMobileDb.Modified = DateTime.Now;
                                            SupplierMobileDb.Active = true;
                                            _unitOfWork.SupplierMobiles.Update(SupplierMobileDb);
                                            _unitOfWork.Complete();
                                        }
                                        else
                                        {
                                            Response.Result = false;
                                            Error error = new Error();
                                            error.ErrorCode = "Err25";
                                            error.ErrorMSG = "This Mobile Doesn't Exist!!";
                                            Response.Errors.Add(error);
                                        }
                                    }
                                    else
                                    {
                                        // Insert
                                        var SupplierMobileDb = new SupplierMobile();
                                        SupplierMobileDb.SupplierId = Request.SupplierId;
                                        SupplierMobileDb.Mobile = MOB.Mobile;
                                        SupplierMobileDb.CreatedBy = validation.userID;
                                        SupplierMobileDb.ModifiedBy = validation.userID;
                                        SupplierMobileDb.CreationDate = DateTime.Now;
                                        SupplierMobileDb.Modified = DateTime.Now;
                                        SupplierMobileDb.Active = true;
                                        _unitOfWork.SupplierMobiles.Add(SupplierMobileDb);
                                        _unitOfWork.Complete();
                                    }
                                }
                            }
                        }

                        //Add-Edit supplier Faxes
                        if (Request.SupplierFaxes != null)
                        {
                            if (Request.SupplierFaxes.Count > 0)
                            {
                                var counter = 0;
                                foreach (var FX in Request.SupplierFaxes)
                                {
                                    counter++;

                                    string CreatedByString = null;
                                    string ModifiedByString = null;
                                    long? CreatedBy = 0;
                                    long? ModifiedBy = 0;
                                    if (FX.ID != 0 && FX.ID != null)
                                    {
                                        if (FX.ModifiedBy != null)
                                        {
                                            ModifiedByString = FX.ModifiedBy;
                                        }
                                        else
                                        {
                                            Response.Result = false;
                                            Error error = new Error();
                                            error.ErrorCode = "Err25";
                                            error.ErrorMSG = "Line: " + counter + ", " + "Message: Modified By Id Is Mandatory";
                                            Response.Errors.Add(error);
                                        }
                                    }
                                    else
                                    {
                                        if (FX.CreatedBy != null)
                                        {
                                            CreatedByString = FX.CreatedBy;
                                        }
                                        else
                                        {
                                            Response.Result = false;
                                            Error error = new Error();
                                            error.ErrorCode = "Err25";
                                            error.ErrorMSG = "Line: " + counter + ", " + "Created By Id Is Mandatory";
                                            Response.Errors.Add(error);
                                        }
                                    }

                                    string Fax = null;
                                    if (!string.IsNullOrEmpty(FX.Fax))
                                    {
                                        var ExistFax = _unitOfWork.SupplierFaxes.FindAll(a => a.Fax == FX.Fax).FirstOrDefault();
                                        if (ExistFax != null)
                                        {
                                            if (FX.ID != null)
                                            {
                                                Response.Result = false;
                                                Error error = new Error();
                                                error.ErrorCode = "Err25";
                                                error.ErrorMSG = "Line: " + counter + ", " + "Fax Is Exist Before";
                                                Response.Errors.Add(error);
                                            }
                                            else
                                            {
                                                Fax = FX.Fax;
                                            }
                                        }
                                        else
                                        {
                                            Fax = FX.Fax;
                                        }
                                    }
                                    else
                                    {
                                        Response.Result = false;
                                        Error error = new Error();
                                        error.ErrorCode = "Err25";
                                        error.ErrorMSG = "Line: " + counter + ", " + "Fax Is Mandatory";
                                        Response.Errors.Add(error);
                                    }
                                }
                            }
                            if (Response.Result)
                            {
                                foreach (var FX in Request.SupplierFaxes)
                                {
                                    if (FX.ID != null && FX.ID != 0)
                                    {
                                        var SupplierFaxDb = _unitOfWork.SupplierFaxes.GetById((long)FX.ID);
                                        if (SupplierFaxDb != null)
                                        {
                                            // Update
                                            SupplierFaxDb.SupplierId = Request.SupplierId;
                                            SupplierFaxDb.Fax = FX.Fax;
                                            SupplierFaxDb.ModifiedBy = validation.userID;
                                            SupplierFaxDb.Modified = DateTime.Now;
                                            SupplierFaxDb.Active = true;
                                            _unitOfWork.SupplierFaxes.Update(SupplierFaxDb);
                                            _unitOfWork.Complete();
                                        }
                                        else
                                        {
                                            Response.Result = false;
                                            Error error = new Error();
                                            error.ErrorCode = "Err25";
                                            error.ErrorMSG = "This Fax Doesn't Exist!!";
                                            Response.Errors.Add(error);
                                        }
                                    }
                                    else
                                    {
                                        // Insert
                                        var SupplierFaxDb = new SupplierFax();
                                        SupplierFaxDb.SupplierId = Request.SupplierId;
                                        SupplierFaxDb.Fax = FX.Fax;
                                        SupplierFaxDb.CreatedBy = validation.userID;
                                        SupplierFaxDb.ModifiedBy = validation.userID;
                                        SupplierFaxDb.CreationDate = DateTime.Now;
                                        SupplierFaxDb.Modified = DateTime.Now;
                                        SupplierFaxDb.Active = true;
                                        _unitOfWork.SupplierFaxes.Add(SupplierFaxDb);
                                        _unitOfWork.Complete();
                                    }
                                }
                            }
                        }
                    }
                }


                return Response;
            }
            catch (Exception ex)
            {
                Response.Result = false;
                Error error = new Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.InnerException != null ? ex.InnerException.Message : ex.Message; ;
                Response.Errors.Add(error);
                return Response;
            }
        }

        public BaseResponseWithId<long> AddNewSupplierContactPerson(SupplierContactPersonData Request)
        {
            BaseResponseWithId<long> Response = new BaseResponseWithId<long>();
            Response.Result = true;
            Response.Errors = new List<Error>();
            try
            {
                if (Response.Result)
                {

                    //check sent data
                    if (Request == null)
                    {
                        Response.Result = false;
                        Error error = new Error();
                        error.ErrorCode = "Err-P12";
                        error.ErrorMSG = "Please Insert a Valid Data.";
                        Response.Errors.Add(error);
                        return Response;
                    }

                    long SupplierId = 0;
                    if (Request.SupplierId != 0)
                    {
                        SupplierId = Request.SupplierId;
                        var supplier = _unitOfWork.Suppliers.GetById(SupplierId);
                        if (supplier == null)
                        {
                            Response.Result = false;
                            Error error = new Error();
                            error.ErrorCode = "ErrCRM1";
                            error.ErrorMSG = "This Supplier Doesn't Exist!!";
                            Response.Errors.Add(error);
                        }
                    }
                    else
                    {
                        Response.Result = false;
                        Error error = new Error();
                        error.ErrorCode = "Err25";
                        error.ErrorMSG = "Supplier Id Is Mandatory";
                        Response.Errors.Add(error);
                    }

                    if (Response.Result)
                    {
                        if (Request.SupplierContactPersons.Count > 0)
                        {
                            var counter = 0;
                            foreach (var CP in Request.SupplierContactPersons)
                            {
                                if (string.IsNullOrEmpty(CP.Name))                             
                                {
                                    Response.Result = false;
                                    Error error = new Error();
                                    error.ErrorCode = "Err25";
                                    error.ErrorMSG = "Line: " + counter + ", " + "Name Is Mandatory";
                                    Response.Errors.Add(error);
                                }

                                if (string.IsNullOrEmpty(CP.Title))                             
                                {
                                    Response.Result = false;
                                    Error error = new Error();
                                    error.ErrorCode = "Err25";
                                    error.ErrorMSG = "Line: " + counter + ", " + "Title Is Mandatory";
                                    Response.Errors.Add(error);
                                }

                                if (string.IsNullOrEmpty(CP.Mobile))                               
                                {
                                    Response.Result = false;
                                    Error error = new Error();
                                    error.ErrorCode = "Err25";
                                    error.ErrorMSG = "Line: " + counter + ", " + "Mobile Is Mandatory";
                                    Response.Errors.Add(error);
                                }
                            }
                        }

                        if (Response.Result)
                        {
                            foreach (var CP in Request.SupplierContactPersons)
                            {
                                if (CP.ID != null && CP.ID != 0)
                                {
                                    var SupplierContactPersonDb = _unitOfWork.SupplierContactPeople.GetById((long)CP.ID);
                                    if (SupplierContactPersonDb != null)
                                    {
                                        // Update
                                        SupplierContactPersonDb.SupplierId = Request.SupplierId;
                                        SupplierContactPersonDb.ModifiedBy = validation.userID;
                                        SupplierContactPersonDb.Modified = DateTime.Now;
                                        SupplierContactPersonDb.Active = true;
                                        SupplierContactPersonDb.Name = CP.Name;
                                        SupplierContactPersonDb.Title = CP.Title;
                                        SupplierContactPersonDb.Location = CP.Location;
                                        SupplierContactPersonDb.Email = CP.Email;
                                        SupplierContactPersonDb.Mobile = CP.Mobile;
                                        _unitOfWork.SupplierContactPeople.Update(SupplierContactPersonDb);
                                        _unitOfWork.Complete();                                       
                                    }
                                    else
                                    {
                                        Response.Result = false;
                                        Error error = new Error();
                                        error.ErrorCode = "Err25";
                                        error.ErrorMSG = "This Contact Person Doesn't Exist!!";
                                        Response.Errors.Add(error);
                                    }
                                }
                                else
                                {
                                    var SupplierContactPersonDb = new SupplierContactPerson();
                                    SupplierContactPersonDb.SupplierId = Request.SupplierId;
                                    SupplierContactPersonDb.CreatedBy = validation.userID;
                                    SupplierContactPersonDb.ModifiedBy = validation.userID;
                                    SupplierContactPersonDb.Modified = DateTime.Now;
                                    SupplierContactPersonDb.Modified = DateTime.Now;
                                    SupplierContactPersonDb.Active = true;
                                    SupplierContactPersonDb.Name = CP.Name;
                                    SupplierContactPersonDb.Title = CP.Title;
                                    SupplierContactPersonDb.Location = CP.Location;
                                    SupplierContactPersonDb.Email = CP.Email;
                                    SupplierContactPersonDb.Mobile = CP.Mobile;

                                    _unitOfWork.SupplierContactPeople.Add(SupplierContactPersonDb);
                                    _unitOfWork.Complete();
                                    var SupplierContactPersonInsert = _unitOfWork.Complete();
                                }
                            }
                        }
                    }
                }

                return Response;
            }
            catch (Exception ex)
            {
                Response.Result = false;
                Error error = new Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.InnerException != null ? ex.InnerException.Message : ex.Message; ;
                Response.Errors.Add(error);
                return Response;
            }
        }

        public BaseResponseWithId<long> AddNewSupplierMobile(SupplierMobileData Request)
        {
            BaseResponseWithId<long> Response = new BaseResponseWithId<long>();
            Response.Result = true;
            Response.Errors = new List<Error>();
            try
            {
                if (Response.Result)
                {

                    //check sent data
                    if (Request == null)
                    {
                        Response.Result = false;
                        Error error = new Error();
                        error.ErrorCode = "Err-P12";
                        error.ErrorMSG = "Please Insert a Valid Data.";
                        Response.Errors.Add(error);
                        return Response;
                    }

                    long SupplierId = 0;
                    if (Request.SupplierId != 0)
                    {
                        SupplierId = Request.SupplierId;
                    }
                    else
                    {
                        Response.Result = false;
                        Error error = new Error();
                        error.ErrorCode = "Err25";
                        error.ErrorMSG = "Supplier Id Is Mandatory";
                        Response.Errors.Add(error);
                    }

                    if (Response.Result)
                    {
                        if (Request.SupplierMobiles.Count > 0)
                        {
                            var counter = 0;
                            foreach (var MOB in Request.SupplierMobiles)
                            {
                                counter++;
                                string Mobile = null;
                                if (!string.IsNullOrEmpty(MOB.Mobile))
                                {
                                    Mobile = MOB.Mobile;
                                }
                                else
                                {
                                    Response.Result = false;
                                    Error error = new Error();
                                    error.ErrorCode = "Err25";
                                    error.ErrorMSG = "Line: " + counter + ", " + "Mobile Is Mandatory";
                                    Response.Errors.Add(error);
                                }
                            }
                        }

                        if (Response.Result)
                        {
                            foreach (var MOB in Request.SupplierMobiles)
                            {
                                if (MOB.ID != null && MOB.ID != 0)
                                {
                                    var SupplierMobileDb = _unitOfWork.SupplierMobiles.GetById((long)MOB.ID);
                                    if (SupplierMobileDb != null)
                                    {
                                        // Update
                                        SupplierMobileDb.SupplierId = Request.SupplierId;
                                        SupplierMobileDb.Mobile = MOB.Mobile;
                                        SupplierMobileDb.ModifiedBy = validation.userID;
                                        SupplierMobileDb.Modified = DateTime.Now;
                                        SupplierMobileDb.Active = true;

                                        _unitOfWork.SupplierMobiles.Update(SupplierMobileDb);

                                        var SupplierMobileUpdate = _unitOfWork.Complete();
                                    }
                                    else
                                    {
                                        Response.Result = false;
                                        Error error = new Error();
                                        error.ErrorCode = "Err25";
                                        error.ErrorMSG = "This Mobile Person Doesn't Exist!!";
                                        Response.Errors.Add(error);
                                    }
                                }
                                else
                                {
                                    // Insert
                                    var SupplierMobileDb = new SupplierMobile();
                                    SupplierMobileDb.SupplierId = Request.SupplierId;
                                    SupplierMobileDb.Mobile = MOB.Mobile;
                                    SupplierMobileDb.CreatedBy = validation.userID;
                                    SupplierMobileDb.ModifiedBy = validation.userID;
                                    SupplierMobileDb.CreationDate = DateTime.Now;
                                    SupplierMobileDb.Modified = DateTime.Now;
                                    SupplierMobileDb.Active = true;
                                    _unitOfWork.SupplierMobiles.Add(SupplierMobileDb);

                                    var SupplierMobileInsert = _unitOfWork.Complete();
                                }
                            }
                        }
                    }
                }

                return Response;
            }
            catch (Exception ex)
            {
                Response.Result = false;
                Error error = new Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.InnerException != null ? ex.InnerException.Message : ex.Message; ;
                Response.Errors.Add(error);
                return Response;
            }
        }

        public BaseResponseWithId<long> AddNewSupplierSpeciality(SupplierSpecialityData Request)
        {
            BaseResponseWithId<long> Response = new BaseResponseWithId<long>();
            Response.Result = true;
            Response.Errors = new List<Error>();
            try
            {
                if (Response.Result)
                {

                    //check sent data
                    if (Request == null)
                    {
                        Response.Result = false;
                        Error error = new Error();
                        error.ErrorCode = "Err-P12";
                        error.ErrorMSG = "Please Insert a Valid Data.";
                        Response.Errors.Add(error);
                        return Response;
                    }

                    long SupplierId = 0;
                    if (Request.SupplierId != 0)
                    {
                        SupplierId = Request.SupplierId;
                    }
                    else
                    {
                        Response.Result = false;
                        Error error = new Error();
                        error.ErrorCode = "Err25";
                        error.ErrorMSG = "Supplier Id Is Mandatory";
                        Response.Errors.Add(error);
                    }

                    if (Response.Result)
                    {
                        if (Request.SupplierSpecialities.Count > 0)
                        {
                            var counter = 0;
                            foreach (var SPC in Request.SupplierSpecialities)
                            {
                                counter++;
                                int SpecialityId = 0;
                                if (SPC.SpecialityID != 0)
                                {
                                    SpecialityId = SPC.SpecialityID;
                                }
                                else
                                {
                                    Response.Result = false;
                                    Error error = new Error();
                                    error.ErrorCode = "Err25";
                                    error.ErrorMSG = "Line: " + counter + ", " + "Speciality Id Is Mandatory";
                                    Response.Errors.Add(error);
                                }
                            }
                        }

                        if (Response.Result)
                        {
                            foreach (var SPC in Request.SupplierSpecialities)
                            {
                                if (SPC.ID != null && SPC.ID != 0)
                                {
                                    var SupplierSpecialityDb = _unitOfWork.SupplierSpecialities.FindAll(a => a.Id == SPC.ID).FirstOrDefault();
                                    if (SupplierSpecialityDb != null)
                                    {
                                        // Update
                                        SupplierSpecialityDb.SupplierId = Request.SupplierId;
                                        SupplierSpecialityDb.SpecialityId = SPC.SpecialityID;
                                        SupplierSpecialityDb.ModifiedBy = validation.userID;
                                        SupplierSpecialityDb.Modified = DateTime.Now;
                                        SupplierSpecialityDb.Modified = DateTime.Now;
                                        SupplierSpecialityDb.Active = true;

                                        _unitOfWork.SupplierSpecialities.Update(SupplierSpecialityDb);


                                        var SupplierSpecialityUpdate = _unitOfWork.Complete();
                                    }
                                    else
                                    {
                                        Response.Result = false;
                                        Error error = new Error();
                                        error.ErrorCode = "Err25";
                                        error.ErrorMSG = "This Speciality Person Doesn't Exist!!";
                                        Response.Errors.Add(error);
                                    }
                                }
                                else
                                {
                                    // Insert
                                    var SupplierSpecialityDb = new SupplierSpeciality();
                                    SupplierSpecialityDb.SupplierId = Request.SupplierId;
                                    SupplierSpecialityDb.SpecialityId = SPC.SpecialityID;
                                    SupplierSpecialityDb.CreatedBy = validation.userID;
                                    SupplierSpecialityDb.ModifiedBy = validation.userID;
                                    SupplierSpecialityDb.CreationDate = DateTime.Now;
                                    SupplierSpecialityDb.Modified = DateTime.Now;
                                    SupplierSpecialityDb.Active = true;

                                    _unitOfWork.SupplierSpecialities.Add(SupplierSpecialityDb);
                                    var SupplierMobileInsert = _unitOfWork.Complete();
                                }
                            }
                        }
                    }
                }

                return Response;
            }
            catch (Exception ex)
            {
                Response.Result = false;
                Error error = new Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.InnerException != null ? ex.InnerException.Message : ex.Message; ;
                Response.Errors.Add(error);
                return Response;
            }
        }

        public BaseResponseWithId<long> AddNewSupplierLandLine(SupplierLandLineData Request)
        {
            BaseResponseWithId<long> Response = new BaseResponseWithId<long>();
            Response.Result = true;
            Response.Errors = new List<Error>();
            try
            {
                if (Response.Result)
                {

                    //check sent data
                    if (Request == null)
                    {
                        Response.Result = false;
                        Error error = new Error();
                        error.ErrorCode = "Err-P12";
                        error.ErrorMSG = "Please Insert a Valid Data.";
                        Response.Errors.Add(error);
                        return Response;
                    }

                    long SupplierId = 0;
                    if (Request.SupplierId != 0)
                    {
                        SupplierId = Request.SupplierId;
                    }
                    else
                    {
                        Response.Result = false;
                        Error error = new Error();
                        error.ErrorCode = "Err25";
                        error.ErrorMSG = "Supplier Id Is Mandatory";
                        Response.Errors.Add(error);
                    }

                    if (Response.Result)
                    {
                        if (Request.SupplierLandLines.Count > 0)
                        {
                            var counter = 0;
                            foreach (var LND in Request.SupplierLandLines)
                            {
                                string LandLine = null;
                                if (!string.IsNullOrEmpty(LND.LandLine))
                                {
                                    LandLine = LND.LandLine;
                                }
                                else
                                {
                                    Response.Result = false;
                                    Error error = new Error();
                                    error.ErrorCode = "Err25";
                                    error.ErrorMSG = "Line: " + counter + ", " + "Land Line Is Mandatory";
                                    Response.Errors.Add(error);
                                }
                            }
                        }

                        if (Response.Result)
                        {
                            foreach (var LND in Request.SupplierLandLines)
                            {
                                if (LND.ID != null && LND.ID != 0)
                                {
                                    var SupplierLandLineDb = _unitOfWork.SupplierPhones.GetById((long)LND.ID);
                                    if (SupplierLandLineDb != null)
                                    {
                                        // Update
                                        SupplierLandLineDb.SupplierId = Request.SupplierId;
                                        SupplierLandLineDb.Phone = LND.LandLine;
                                        SupplierLandLineDb.Modified = DateTime.Now;
                                        SupplierLandLineDb.ModifiedBy = validation.userID;
                                        SupplierLandLineDb.Active = true;
                                        _unitOfWork.SupplierPhones.Update(SupplierLandLineDb);
                                        var SupplierLandLineUpdate = _unitOfWork.Complete();
                                    }
                                    else
                                    {
                                        Response.Result = false;
                                        Error error = new Error();
                                        error.ErrorCode = "Err25";
                                        error.ErrorMSG = "This Land Line Doesn't Exist!!";
                                        Response.Errors.Add(error);
                                    }
                                }
                                else
                                {
                                    // Insert
                                    var SupplierLandLineDb = new SupplierPhone();
                                    SupplierLandLineDb.SupplierId = Request.SupplierId;
                                    SupplierLandLineDb.Phone = LND.LandLine;
                                    SupplierLandLineDb.CreationDate = DateTime.Now;
                                    SupplierLandLineDb.Modified = DateTime.Now;
                                    SupplierLandLineDb.ModifiedBy = validation.userID;
                                    SupplierLandLineDb.CreatedBy = validation.userID;
                                    SupplierLandLineDb.Active = true;
                                    _unitOfWork.SupplierPhones.Add(SupplierLandLineDb);
                                    var SupplierLandLineInsert = _unitOfWork.Complete();
                                }
                            }
                        }
                    }
                }

                return Response;
            }
            catch (Exception ex)
            {
                Response.Result = false;
                Error error = new Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.InnerException != null ? ex.InnerException.Message : ex.Message; ;
                Response.Errors.Add(error);
                return Response;
            }
        }

        public BaseResponseWithId<long> AddNewSupplierFax(SupplierFaxData Request)
        {
            BaseResponseWithId<long> Response = new BaseResponseWithId<long>();
            Response.Result = true;
            Response.Errors = new List<Error>();
            try
            {
                if (Response.Result)
                {

                    //check sent data
                    if (Request == null)
                    {
                        Response.Result = false;
                        Error error = new Error();
                        error.ErrorCode = "Err-P12";
                        error.ErrorMSG = "Please Insert a Valid Data.";
                        Response.Errors.Add(error);
                        return Response;
                    }

                    long SupplierId = 0;
                    if (Request.SupplierId != 0)
                    {
                        SupplierId = Request.SupplierId;
                    }
                    else
                    {
                        Response.Result = false;
                        Error error = new Error();
                        error.ErrorCode = "Err25";
                        error.ErrorMSG = "Supplier Id Is Mandatory";
                        Response.Errors.Add(error);
                    }

                    if (Response.Result)
                    {
                        if (Request.SupplierFaxes.Count > 0)
                        {
                            var counter = 0;
                            foreach (var FX in Request.SupplierFaxes)
                            {
                                counter++;
                                string Fax = null;
                                if (!string.IsNullOrEmpty(FX.Fax))
                                {
                                    Fax = FX.Fax;
                                }
                                else
                                {
                                    Response.Result = false;
                                    Error error = new Error();
                                    error.ErrorCode = "Err25";
                                    error.ErrorMSG = "Line: " + counter + ", " + "Fax Is Mandatory";
                                    Response.Errors.Add(error);
                                }
                            }
                        }

                        if (Response.Result)
                        {
                            foreach (var FX in Request.SupplierFaxes)
                            {
                                if (FX.ID != null && FX.ID != 0)
                                {
                                    var SupplierFaxDb = _unitOfWork.SupplierFaxes.GetById((long)FX.ID);
                                    if (SupplierFaxDb != null)
                                    {
                                        // Update
                                        SupplierFaxDb.SupplierId = Request.SupplierId;
                                        SupplierFaxDb.Fax = FX.Fax;
                                        SupplierFaxDb.Modified = DateTime.Now;
                                        SupplierFaxDb.ModifiedBy =validation.userID;
                                        SupplierFaxDb.Active =true;
                                        _unitOfWork.SupplierFaxes.Update(SupplierFaxDb);
                                        var SupplierFaxUpdate = _unitOfWork.Complete();
                                    }
                                    else
                                    {
                                        Response.Result = false;
                                        Error error = new Error();
                                        error.ErrorCode = "Err25";
                                        error.ErrorMSG = "This Fax Doesn't Exist!!";
                                        Response.Errors.Add(error);
                                    }
                                }
                                else
                                {
                                    // Insert
                                    var SupplierFaxDb = new SupplierFax();
                                    SupplierFaxDb.SupplierId = Request.SupplierId;
                                    SupplierFaxDb.Fax = FX.Fax;
                                    SupplierFaxDb.CreationDate = DateTime.Now;
                                    SupplierFaxDb.Modified = DateTime.Now;
                                    SupplierFaxDb.CreatedBy = validation.userID;
                                    SupplierFaxDb.ModifiedBy = validation.userID;
                                    SupplierFaxDb.Active = true;
                                    _unitOfWork.SupplierFaxes.Add(SupplierFaxDb);
                                    var SupplierFaxInsert = _unitOfWork.Complete();
                                }
                            }
                        }
                    }
                }

                return Response;
            }
            catch (Exception ex)
            {
                Response.Result = false;
                Error error = new Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.InnerException != null ? ex.InnerException.Message : ex.Message; ;
                Response.Errors.Add(error);
                return Response;
            }
        }

        public SelectDDLResponse GetSupplierList([FromHeader] int GovernorateID, [FromHeader] string Import, [FromHeader] string SearchKey )
        {
            SelectDDLResponse Response = new SelectDDLResponse();
            Response.Result = true;
            Response.Errors = new List<Error>();
            try
            {
                var DDLList = new List<SelectDDL>();
                if (Response.Result)
                {
                    var ListDB = _unitOfWork.VSuppliers.FindAllQueryable(x => x.Active == true).AsQueryable();

                    if (GovernorateID > 0)
                    {
                        ListDB = ListDB.Where(x => x.GovernorateId == GovernorateID);
                    }
                    
                    if (!string.IsNullOrWhiteSpace(Import))
                    {
                        if (Import == "true")
                        {
                            ListDB = ListDB.Where(x => x.CountryId != 1);
                        }
                    }
                    var SupplierListDB = ListDB.ToList();
                    if (!string.IsNullOrWhiteSpace(SearchKey))
                    {
                        SearchKey = HttpUtility.UrlDecode(SearchKey);
                        var ListIDSMobileSupplier = _unitOfWork.SupplierMobiles.FindAll(x => x.Active == true && x.Mobile.Trim() == SearchKey.Trim()).Select(x => x.SupplierId).Distinct().ToList();
                        var ListIDSContactPersonSupplier = _unitOfWork.SupplierContactPeople.FindAll(x => x.Active == true && (x.Mobile.Trim() == SearchKey.Trim()
                                                                                                                          || x.Email != null ? x.Email.Contains(SearchKey) : false)
                                                                                                                          ).Select(x => x.SupplierId).Distinct().ToList();
                        SupplierListDB = SupplierListDB.Where(x => x.Name.ToLower().Contains(SearchKey.ToLower())
                                                || (x.Email != null ? x.Email.ToLower().Contains(SearchKey.ToLower()) : false)
                                                || ListIDSMobileSupplier.Contains(x.Id) || ListIDSContactPersonSupplier.Contains(x.Id)
                                                ).ToList();
                    }

                 
                        if (SupplierListDB.Count > 0)
                    {
                        foreach (var Supplier in SupplierListDB)
                        {
                            var DLLObj = new SelectDDL();
                            DLLObj.ID = Supplier.Id;
                            DLLObj.Name = Supplier.Name;

                            DDLList.Add(DLLObj);
                        }
                    }
                }
                Response.DDLList = DDLList.Distinct().ToList();
                return Response;

            }
            catch (Exception ex)
            {
                Response.Result = false;
                Error error = new Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.InnerException != null ? ex.InnerException.Message : ex.Message; ;
                Response.Errors.Add(error);
                return Response;
            }
        }
    }
}
