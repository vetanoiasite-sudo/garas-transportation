using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NewGaras.Infrastructure.Entities;
using static Azure.Core.HttpHeader;
using System.Net;
using System.Web;
using NewGarasAPI.Models.Common;
using Microsoft.Data.SqlClient;
using NewGaras.Infrastructure.DBContext;
using NewGaras.Infrastructure.Helper;

namespace NewGarasAPI.Helper
{
    public class Helper
    {
        static readonly string key = "SalesGarasPass";
        public string GetConnectonString(string CompName)
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            //var IntExample = MyConfig.GetValue<int>("AppSettings:SampleIntValue");
            //var AppName = MyConfig.GetValue<string>("AppSettings:APP_Name");
            string ServerName = MyConfig.GetValue<string>("AppSettings:ServerName");
            string result = "";
            switch (CompName)
            {
                case "proauto":
                    result = "Data Source=" + ServerName + ";Initial Catalog=GARASAuto;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True;";
                    break;
                case "marinaplt":
                    result = "Data Source=" + ServerName + ";Initial Catalog=GARAS;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True;";
                    break;
                case "piaroma":
                    result = "Data Source=" + ServerName + ";Initial Catalog=GARASAroma;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True;";
                    break;
                case "elsalam":
                    result = "Data Source=" + ServerName + ";Initial Catalog=GARASElSalam;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True;";
                    break;
                case "garastest":
                    result = "Data Source=" + ServerName + ";Initial Catalog=GARASTest;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True;";
                    //result = "Data Source=" + ServerName + ";initial catalog=GarasTest;password=P@ssw0rd;persist security info=True;user id=GARASDBUser;TrustServerCertificate=True;";
                    break;
                case "royaltent":
                    result = "Data Source=" + ServerName + ";Initial Catalog=GARASRoyal;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True;";
                    break;
                case "elwaseem":
                    result = "Data Source=" + ServerName + ";Initial Catalog=GARASElWaseem;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True;";
                    break;
                case "marinapltq":
                    result = "Data Source=" + ServerName + ";Initial Catalog=MARINAQATAR;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True;";
                    break;
                case "vetanoia":
                    result = "Data Source=" + ServerName + ";Initial Catalog=VETANOIA;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True;";
                    break;
                case "ramsissteel":
                    result = "Data Source=" + ServerName + ";Initial Catalog=GARASRamsisSteel;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True;";
                    break;
                case "periti":
                    result = "Data Source=" + ServerName + ";Initial Catalog=PERITI;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True;";
                    break;
                case "ortho":
                    result = "Data Source=" + ServerName + ";Initial Catalog=ORTHOUS;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True;";
                    break;
                case "libmark":
                    result = "Data Source=" + ServerName + ";Initial Catalog=LIBMARK;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True;";
                    break;
                case "coctail":
                    result = "Data Source=" + ServerName + ";Initial Catalog=COCTAIL;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True;";
                    break;
                case "eldib":
                    result = "Data Source=" + ServerName + ";Initial Catalog=ELDIB;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True;";
                    break;
                case "stmark":
                    result = "Data Source=" + ServerName + ";Initial Catalog=STMark;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True;";
                    break;
                case "elite":
                    result = "Data Source=" + ServerName + ";Initial Catalog=ELITE;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True;";
                    break;
                case "stmg":
                    result = "Data Source=" + ServerName + ";Initial Catalog=STMG;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True;";
                    break;
                case "shi":
                    result = "Data Source=" + ServerName + ";Initial Catalog=GARAS_SHI;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True;";
                    break;
                case "shc":
                    result = "Data Source=" + ServerName + ";Initial Catalog=GARAS_SHC;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True;";
                    break;
                case "graffiti":
                    result = "Data Source=" + ServerName + ";Initial Catalog=GRAFFITI;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True;";
                    break;
                case "sanpeter":
                    result = "Data Source=" + ServerName + ";Initial Catalog=SanPeterReservation;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True;";
                    break;
                case "libroyes":
                    result = "Data Source=" + ServerName + ";Initial Catalog=LIBRoyes;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True;";
                    break;
                case "stmary":
                    result = "Data Source=" + ServerName + ";Initial Catalog=STMary;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True;";
                    break;
                case "pharma":
                    result = "Data Source=" + ServerName + ";Initial Catalog=GARASTransportation;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True;";
                    break;
            }
            return result;





        }
        public t ExecuteProcedure<t>(string procName, object[] param, GarasTestContext _Context)
        {
            var LoadObjDB = _Context.Database.SqlQueryRaw<t>(procName, param).AsEnumerable().FirstOrDefault();

            return LoadObjDB;
        }




        public HearderVaidatorOutput ValidateCompany(string CompanyName, ref GarasTestContext _Context)
        {
            HearderVaidatorOutput result = new HearderVaidatorOutput();
            result.errors = new List<Error>();
            result.result = true;
            try
            {
                if (string.IsNullOrEmpty(CompanyName))
                {
                    Error error = new NewGarasAPI.Models.Common.Error();
                    error.ErrorCode = "Err-P200";
                    error.ErrorMSG = "Invalid Company Name";
                    result.errors.Add(error);
                    result.result = false;
                    return result;
                }
                string CopName = CompanyName.ToLower();
                if (CopName != "marinaplt" &&
                    CopName != "proauto" &&
                    CopName != "piaroma" &&
                    CopName != "elsalam" &&
                    CopName != "elwaseem" &&
                    CopName != "garastest" &&
                    CopName != "marinapltq" &&
                    CopName != "vetanoia" &&
                    CopName != "ramsissteel" &&
                    CopName != "periti" &&
                    CopName != "stmark" &&
                    CopName != "elite" &&
                    CopName != "stmg" &&
                    CopName != "shi" &&
                    CopName != "shc" &&
                    CopName != "shc" &&
                    CopName != "graffiti" &&
                    CopName != "libmark" &&
                    CopName != "libroyes" &&
                    CopName != "sanpeter" &&
                    CopName != "pharma" &&
                    CopName != "royaltent")
                {
                    Error error = new NewGarasAPI.Models.Common.Error();
                    error.ErrorCode = "Err-P200";
                    error.ErrorMSG = "Invalid Company Name";
                    result.errors.Add(error);
                    result.result = false;
                    return result;
                }
                _Context.Database.SetConnectionString(GetConnectonString(CopName));

                result.CompanyName = CopName;
                return result;
            }
            catch (Exception ex)
            {
                Error error = new NewGarasAPI.Models.Common.Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                result.errors.Add(error);
                result.result = false;
                return result;

            }
        }






        public HearderVaidatorOutput ValidateHeader(IHeaderDictionary headers, ref GarasTestContext _Context)
        {
            HearderVaidatorOutput result = new HearderVaidatorOutput();
            result.errors = new List<Error>();
            try
            {
                #region For Shared Server
                ValidateCompanyName validator = new ValidateCompanyName();
                if (string.IsNullOrEmpty(headers["CompanyName"]))
                {
                    Error error = new NewGarasAPI.Models.Common.Error();
                    error.ErrorCode = "Err-P200";
                    error.ErrorMSG = "Invalid Company Name";
                    result.errors.Add(error);
                    result.result = false;
                    return result;
                }

                if (!validator.ValidateInput(headers["CompanyName"].ToString().ToLower()))
                {
                    Error error = new NewGarasAPI.Models.Common.Error();
                    error.ErrorCode = "Err-P200";
                    error.ErrorMSG = "Invalid Company Name";
                    result.errors.Add(error);
                    result.result = false;
                    return result;
                }
                string CopName = headers["CompanyName"].ToString().ToLower();
                _Context.Database.SetConnectionString(GetConnectonString(CopName));
                #endregion
                if (!string.IsNullOrEmpty(headers["UserToken"]))
                {
                    string UserToken = headers["UserToken"];
                    bool FromClient = !string.IsNullOrEmpty(headers["FromClient"]) ? (headers["FromClient"].ToString().ToLower() == "true" ? true : false) : false;
                    UserToken = HttpUtility.UrlDecode(UserToken);
                    long userSessionID = 0;
                    if (!string.IsNullOrEmpty(UserToken) && long.TryParse(Encrypt_Decrypt.Decrypt(UserToken, key), out userSessionID))
                    {
                        // Just For Test --
                        // Check is from User Or supplier 
                        long ClientID = 0;
                        long UserID = 0;
                        if (FromClient)
                        {
                            var CheckClientSessionDB = _Context.ClientSessions.Where(x => x.Id == userSessionID
                                                                                             && x.Active == true
                                                                                             && x.EndDate > DateTime.Now).FirstOrDefault();
                            if (CheckClientSessionDB != null)
                            {
                                ClientID = CheckClientSessionDB.ClientId;
                            }

                        }
                        else
                        {
                            var CheckUserSessionDB = _Context.UserSessions.Where(x => x.Id == userSessionID
                                                                     && x.Active == true
                                                                     && x.EndDate > DateTime.Now).FirstOrDefault();
                            if (CheckUserSessionDB != null)
                            {
                                UserID = CheckUserSessionDB.UserId;
                            }
                        }

                        if (ClientID != 0 || UserID != 0)
                        {
                            // Call Constructor Common Class 
                            //Common._Context = _Context;
                            //AdminWS._Context = _Context;
                            //MaintenanceAndServiceWS._Context = _Context;
                            //AccountAndFinanceWS._Context = _Context;
                            //PurchasingWS._Context = _Context;
                            //CustomerBL._Context = _Context;
                            //InvoiceBL._Context = _Context;
                            //IssuerBL._Context = _Context;
                            //ItemBL._Context = _Context;
                            //APIEnvironment._Context = _Context;
                            result.result = true;
                            if (FromClient && ClientID != 0)
                            {
                                result.userID = 1; // ClientID;
                            }
                            else if (UserID != 0)
                            {
                                result.userID = UserID;
                            }
                            result.CompanyName = CopName;
                            return result;
                        }
                        else
                        {
                            Error error = new NewGarasAPI.Models.Common.Error();
                            error.ErrorCode = "Err-P2";
                            error.ErrorMSG = "Invalid Token, Please Login again";
                            result.errors.Add(error);
                            result.result = false;
                        }
                    }
                    else
                    {

                        Error error = new NewGarasAPI.Models.Common.Error();
                        error.ErrorCode = "Err-P2";
                        error.ErrorMSG = "Invalid Token, Please Login again";
                        result.errors.Add(error);
                        result.result = false;
                    }
                }
                else
                {

                    Error error = new NewGarasAPI.Models.Common.Error();
                    error.ErrorCode = "Err-P2";
                    error.ErrorMSG = "Invalid Token, Please Login again";
                    result.errors.Add(error);
                    result.result = false;
                }
                result.CompanyName = CopName;
                return result;
            }
            catch (Exception ex)
            {
                Error error = new NewGarasAPI.Models.Common.Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                result.errors.Add(error);
                result.result = false;
                return result;

            }
        }
    }
    public class PagedList<T> : List<T>
    {
        public PagedList(IEnumerable<T> ItemsList, int pageNumber, int count, int pageSize, int startFrom, int endTo)
        {
            CurrentPage = pageNumber;

            //calculate this depend on how many cout we have 
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            PageSize = pageSize;
            TotalCount = count;
            StartFrom = startFrom;
            EndTo = endTo;


            //we will add items inside here so that we have access to these items inside our page list
            AddRange(ItemsList);
        }

        // the number of the current page
        public int CurrentPage { get; set; }

        //the total number of pages
        public int TotalPages { get; set; }

        //the count of items per each page
        public int PageSize = 10;

        //total count of items in the query
        public int TotalCount { get; set; }
        public int StartFrom { get; set; }
        public int EndTo { get; set; }

        //creating static method that we can call from anywhere
        public static PagedList<T> Create(IQueryable<T> source, int pageNumber, int pageSize)
        {
            //calculate the count from query that we have
            var count = source.Count();

            var startFrom = ((pageNumber - 1) * pageSize) + 1;

            //calculate what we will skip and what will take
            //l7ad delwa2ty mro7nash eldatabase lma n3ml ToList byro7
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            var endTo = startFrom + items.Count - 1;

            //creating and returning object of PageList with constructor
            return new PagedList<T>(items, pageNumber, count, pageSize, startFrom, endTo);
        }

        public static PagedList<T> CreateOrdered(IOrderedQueryable<T> source, int pageNumber, int pageSize)
        {
            //calculate the count from query that we have
            var count = source.Count();

            var startFrom = ((pageNumber - 1) * pageSize) + 1;

            //calculate what we will skip and what will take
            //l7ad delwa2ty mro7nash eldatabase lma n3ml ToList byro7
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            var endTo = startFrom + items.Count - 1;

            //creating and returning object of PageList with constructor
            return new PagedList<T>(items, pageNumber, count, pageSize, startFrom, endTo);
        }
        public async static Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            //calculate the count from query that we have
            var count = source.Count();

            var startFrom = ((pageNumber - 1) * pageSize) + 1;

            //calculate what we will skip and what will take
            //l7ad delwa2ty mro7nash eldatabase lma n3ml ToList byro7
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var endTo = startFrom + items.Count - 1;

            //creating and returning object of PageList with constructor
            return new PagedList<T>(items, pageNumber, count, pageSize, startFrom, endTo);
        }
    }
}
