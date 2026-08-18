using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NewGaras.Infrastructure.DBContext;
using NewGaras.Infrastructure.Entities;
using NewGaras.Infrastructure.Helper.TenantService;
using NewGarasAPI.Helper;
using System.Collections.Concurrent;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace NewGarasAPI.Hubs
{
    public class NotificationsHub : Hub
    {

        private static Dictionary<string, string> userConnectionMap = new Dictionary<string, string>();


        GarasTestContext _Context;
        Helper.Helper _helper ;
        private readonly ITenantService _tenantService;
        //private GarasTestContext _Context;
        //private string _conn;
        //private NotificationService _service;
        //static string logDirectory = "/logs";
        // static string logFileName = "applog.txt";
        // static string logFilePath = Path.Combine(logDirectory, logFileName);
        // var logger = new Logger(logFilePath);
        public NotificationsHub(ITenantService tenantService)
        {
            _tenantService = tenantService;
            _Context = new GarasTestContext(_tenantService);
            _helper = new Helper.Helper();
            //Helper.Helper _helper = new Helper.Helper(); ;
            //string CopName = "GARASTest";
            // _service = new NotificationService();
            // _Context = _context;
            //_Context.Database.SetConnectionString(_helper.GetConnectonString(CopName));
            //_Context = new GarasTestContext();
        }


        //public GarasTestContext CreateDbContext(ITenantService _tenantService)
        //{
        //    // Apply your multi-tenancy strategy here to determine the appropriate database connection details for the given tenant
        //    string connectionString = _dbContextTenant.GetConnectionString("garastest");

        //    // Create and configure the database options
        //    var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        //    optionsBuilder.UseSqlServer(connectionString);
        //    new ApplicationDbContext(optionsBuilder.Options, _tenantService);
        //    // Create the database context using the options
        //    return new ApplicationDbContext(optionsBuilder.Options, _tenantService);
        //}


        //public override System.Threading.Tasks.Task OnConnectedAsync()
        //{
        //    //IHeaderDictionary headers = new HeaderDictionary();
        //    //var httpCtx = Context.GetHttpContext();
        //    //headers = httpCtx.Request.Headers;
        //    //headers["UserToken"] = UserToken;
        //    //headers["CompanyName"] = CompanyName;
        //    //Helper.Helper _helper = new Helper.Helper();
        //    //var validation = _helper.ValidateHeader(headers, ref _Context);
        //    //var res = validation.result + " " + validation.errors.FirstOrDefault()?.ErrorMSG;

        //    //Clients.All.SendAsync("Connect",  res );

        //    return base.OnConnectedAsync();
        //}
        //public override System.Threading.Tasks.Task OnDisconnectedAsync(Exception exception)
        //{
        //    return base.OnDisconnectedAsync(exception);
        //}


        public override async System.Threading.Tasks.Task OnConnectedAsync()
        {
            //var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userId = Context.GetHttpContext()?.Request.Query["userId"].ToString();
            if (!string.IsNullOrEmpty(userId))
            {
                userConnectionMap[userId] = Context.ConnectionId;
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            }

            await base.OnConnectedAsync();
        }

        //public override System.Threading.Tasks.Task OnConnectedAsync()
        //{
        //    // Retrieve or generate a unique identifier for the user
        //    string userId = GenerateUserId();

        //    var httpCtx = Context.GetHttpContext();
            
        //    // Read headers from the request
        //    if (httpCtx.Request.Headers.TryGetValue("Authorization", out var headerValue))
        //    {
        //        // Use the header value as required
        //        string value = headerValue.ToString();
        //        // Perform necessary operations with the header value
        //        userConnectionMap["10004"] = Context.ConnectionId;

        //    }
        //    else
        //    {

        //    // Map the user identifier to the connection ID
        //    userConnectionMap[userId] = Context.ConnectionId;
        //    }

        //    return base.OnConnectedAsync();
        //}

        public override System.Threading.Tasks.Task OnDisconnectedAsync(Exception exception)
        {
            // Remove the user mapping when the connection is closed
            string userId = userConnectionMap.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
            if (userId != null)
            {
                userConnectionMap.Remove(userId);
            }

            return base.OnDisconnectedAsync(exception);
        }

        public async System.Threading.Tasks.Task SendMessageToUser(string userId, string message)
        {
            if (userConnectionMap.ContainsKey(userId))
            {
                string connectionId = userConnectionMap[userId];
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", message + " -"+ userConnectionMap.Count());
            }
            else
            {
                await Clients.All.SendAsync("ReceiveMessage", message + " -"+ userConnectionMap);
            }
        }

        private string GenerateUserId()
        {
            // Generate a unique identifier for the user using your own logic
            // For example, you can use Guid.NewGuid().ToString() to generate a random identifier
            return Guid.NewGuid().ToString();
        }

        public async void SendNotification(string CompanyName, List<long> UserIDSList, string Title, string Description, string URL, string SenderName, string SenderImg)
        {
            try
            {
                //using (var dbContext = new GarasTestContext())
                //{
                //var Senderdb = await _context.Users.Where(x => x.Id == ApplicationUserId).FirstOrDefaultAsync();
                //if (Senderdb != null)
                //{
                //    model.SenderName = Senderdb.FirstName + " " + Senderdb.LastName;
                //    string base64String = Convert.ToBase64String(Senderdb.Photo, 0, Senderdb.Photo.Length);
                //    model.SenderImg = "data:image/png;base64," + base64String;
                //    model.SenderImg2 = base64String;
                //}
                var validation = _helper.ValidateCompany(CompanyName, ref _Context);
                if (validation.result == false)
                {
                    await Clients.All.SendAsync("ReceiveNotification", validation.errors?.FirstOrDefault()?.errorMSG);
                }
                if (UserIDSList != null && UserIDSList.Count() > 0)
                {
                    try
                    {
                        var HubUserExist = _Context.Users.Where(x => UserIDSList.Contains(x.Id)).Select(x => x.Email).ToList();
                        foreach (var ToUserId in UserIDSList)
                        {
                            var Notificationdb = new Notification();
                            Notificationdb.UserId = 1;  // from user 
                            Notificationdb.Title = Title;
                            Notificationdb.Description = Description;
                            Notificationdb.Url = URL;
                            Notificationdb.Date = DateTime.Now;
                            Notificationdb.New = true;
                            //To USer ... if null send to all user but if specific user with send to this user only
                            Notificationdb.FromUserId = ToUserId; // model.UserIDSList.FirstOrDefault();
                            _Context.Notifications.Add(Notificationdb);
                        }
                        _Context.SaveChanges();
                        //var HubUserExist = _service.SendNotificationFromDB(model);
                        await Clients.Users(HubUserExist).SendAsync("ReceiveNotification", CompanyName, UserIDSList, Title, Description, URL, SenderName, SenderImg);
                    }
                    catch (Exception ex)
                    {
                        await Clients.All.SendAsync("ReceiveNotification", "ex-- :" + ex.InnerException?.Message ?? ex.Message);
                    }


                }
                else
                {
                    var Notificationdb = new Notification();
                    Notificationdb.UserId = 1;  // from user 
                    Notificationdb.Title = Title;
                    Notificationdb.Description = Description;
                    Notificationdb.Url = URL;
                    Notificationdb.Date = DateTime.Now;
                    Notificationdb.New = true;
                    _Context.Notifications.Add(Notificationdb);
                    _Context.SaveChanges();

                    //    // To USer ... if null send to all user but if specific user with send to this user only

                    await Clients.All.SendAsync("ReceiveNotification",  CompanyName, UserIDSList, Title, Description, URL, SenderName, SenderImg);
                }
                //}
            }
            catch (Exception ex)
            {
                await Clients.All.SendAsync("ReceiveNotification", "ex :" + ex.InnerException?.Message ?? ex.Message);
            }
        }


        //public async void SendNotification(NotificationModel model)
        //{
        //    try
        //    {
        //        //using (var dbContext = new GarasTestContext())
        //        //{
        //        //var Senderdb = await _context.Users.Where(x => x.Id == ApplicationUserId).FirstOrDefaultAsync();
        //        //if (Senderdb != null)
        //        //{
        //        //    model.SenderName = Senderdb.FirstName + " " + Senderdb.LastName;
        //        //    string base64String = Convert.ToBase64String(Senderdb.Photo, 0, Senderdb.Photo.Length);
        //        //    model.SenderImg = "data:image/png;base64," + base64String;
        //        //    model.SenderImg2 = base64String;
        //        //}
        //        var validation = _helper.ValidateCompany(model.CompanyName, ref _Context);
        //        if (validation.result == false) {
        //            await Clients.All.SendAsync("ReceiveNotification", validation.errors?.FirstOrDefault()?.errorMSG);
        //        }
        //        if (model.UserIDSList != null && model.UserIDSList.Count() > 0)
        //            {
        //                try
        //                {
        //                    var HubUserExist = _Context.Users.Where(x => model.UserIDSList.Contains(x.Id)).Select(x => x.Email).ToList();
        //                    foreach (var ToUserId in model.UserIDSList)
        //                    {
        //                        var Notificationdb = new Notification();
        //                        Notificationdb.UserId = 1;  // from user 
        //                        Notificationdb.Title = model.Title;
        //                        Notificationdb.Description = model.Description;
        //                        Notificationdb.Url = model.URL;
        //                        Notificationdb.Date = DateTime.Now;
        //                        Notificationdb.New = true;
        //                        //To USer ... if null send to all user but if specific user with send to this user only
        //                        Notificationdb.FromUserId = ToUserId; // model.UserIDSList.FirstOrDefault();
        //                    _Context.Notifications.Add(Notificationdb);
        //                    }
        //                _Context.SaveChanges();
        //                    //var HubUserExist = _service.SendNotificationFromDB(model);
        //                    await Clients.Users(HubUserExist).SendAsync("ReceiveNotification", model);
        //                }
        //                catch (Exception ex)
        //                {
        //                    await Clients.All.SendAsync("ReceiveNotification", "ex-- :" + ex.InnerException?.Message ?? ex.Message);
        //                }


        //            }
        //            else
        //            {
        //                var Notificationdb = new Notification();
        //                Notificationdb.UserId = 1;  // from user 
        //                Notificationdb.Title = model.Title;
        //                Notificationdb.Description = model.Description;
        //                Notificationdb.Url = model.URL;
        //                Notificationdb.Date = DateTime.Now;
        //                Notificationdb.New = true;
        //            _Context.Notifications.Add(Notificationdb);
        //            _Context.SaveChanges();

        //                //    // To USer ... if null send to all user but if specific user with send to this user only

        //                await Clients.All.SendAsync("ReceiveNotification", model);
        //            }
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        await Clients.All.SendAsync("ReceiveNotification", "ex :" + ex.InnerException?.Message ?? ex.Message);
        //    }
        //}


        public async void SendMessege(string CompanyName,string msg)
        {
            try
            {
                var validation = _helper.ValidateCompany(CompanyName, ref _Context);


                //headers["CompanyName"] = httpCtx.Request.Headers["CompanyName"];
                //headers["UserToken"] = httpCtx.Request.Headers["UserToken"];
                //using (GarasTestContext dbContext = new GarasTestContext())
                //{

                //var test = _service.Test();
                var User = _Context.Users.OrderByDescending(x=>x.Id).FirstOrDefault();
                    WriteLogFile.WriteLog("ConsoleLog.log", String.Format("{0} @ {1}", "success", DateTime.Now));
                    await Clients.All.SendAsync("ReceiveMessege", msg + " - " + User.FirstName);
                //}
            }
            catch(Exception ex) 
            {
                WriteLogFile.WriteLog("ConsoleLog.log", String.Format("{0} @ {1}", ex.Message + "  "+ex.InnerException.Message, DateTime.Now));
                await Clients.All.SendAsync("ReceiveMessege", "ex :" + ex.InnerException?.Message ?? ex.Message);
            }
        }

    }

    public class WriteLogFile 
    {
        public static bool WriteLog(string strFileName, string strMessage)
        {
            try
            {
                FileStream objFilestream = new FileStream(string.Format("{0}\\{1}", "C:\\inetpub\\wwwroot\\GarasCoreAPI\\logs", strFileName), FileMode.Append, FileAccess.Write);
                StreamWriter objStreamWriter = new StreamWriter((Stream)objFilestream);
                objStreamWriter.WriteLine(strMessage);
                objStreamWriter.Close();
                objFilestream.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
