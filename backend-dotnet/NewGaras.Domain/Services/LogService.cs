using AutoMapper;
using NewGaras.Domain.Consts;
using NewGaras.Domain.Models;
using NewGaras.Infrastructure;
using NewGaras.Infrastructure.DTO.Log;
using NewGaras.Infrastructure.Entities;
using NewGaras.Infrastructure.Interfaces.ServicesInterfaces;
using NewGaras.Infrastructure.Models;
using NewGaras.Infrastructure.Models.Log;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using System.Drawing;

namespace NewGaras.Domain.Services
{
    public class LogService : ILogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _host;
        public LogService(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment host)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _host = host;
        }

        public BaseResponseWithId AddLog(string ActionName,string tablename, string columnName, int entryId, long createdby)
        {
            BaseResponseWithId response = new BaseResponseWithId();
            response.Result = true;
            response.Errors = new List<Error>();

            var log = new SystemLog()
            {
                TableName = tablename,
                ColumnName = columnName,
                ActionName = ActionName,
                LogDate = DateTime.Now,
                OldValue = "",
                NewValue = "",
                CreatedBy=createdby
            };
            _unitOfWork.SystemLogs.Add(log);
            
            return response;
        }

        public BaseResponseWithDataAndHeader<List<GetSystemLogDto>> GetLogs([FromHeader] LogFilters filters)
        {
            BaseResponseWithDataAndHeader<List<GetSystemLogDto>> response = new BaseResponseWithDataAndHeader<List<GetSystemLogDto>>();
            response.Result = true;
            response.Errors = new List<Error>();
            response.Data = new List<GetSystemLogDto>();
            response.PaginationHeader = new PaginationHeader();
            // var logs = _unitOfWork.SystemLogs;

            //response.Data = _mapper.Map<List<GetSystemLogDto>>(logs);
            Expression<Func<SystemLog, bool>> Criteria = (x => true);
            Expression<Func<SystemLog, bool>> temp = (x => true);
            /*if (!string.IsNullOrEmpty(filters.TableName))
            {
                temp = (x => x.TableName.Trim().ToLower().Contains(filters.TableName.Trim().ToLower()));
                FilterLog = Expression.Lambda<Func<SystemLog, bool>>(FilterLog.Body, temp.Parameters);
            }
            if (!string.IsNullOrEmpty(filters.ColumnName))
            {
                temp = (x => x.ColumnName.Trim().ToLower().Contains(filters.ColumnName.Trim().ToLower()));
                FilterLog = Expression.Lambda<Func<SystemLog, bool>>(FilterLog.Body, temp.Parameters);
            }
            if (!string.IsNullOrEmpty(filters.ActionName))
            {
                temp = (x => x.ActionName.Trim().ToLower().Contains(filters.ActionName.Trim().ToLower()));
                FilterLog = Expression.Lambda<Func<SystemLog, bool>>(Expression.AndAlso(FilterLog.Body, temp.Body), FilterLog.Parameters[0]);
            }
            if (filters.CreatedBy != null && filters.CreatedBy != 0)
            {
                temp = (x => x.CreatedBy == filters.CreatedBy);
                FilterLog = Expression.Lambda<Func<SystemLog, bool>>(FilterLog.Body, temp.Parameters);
            }*/
            DateTime filterdate = DateTime.MinValue;
            if (!string.IsNullOrEmpty(filters.LogDate))
            {
                DateTime.TryParse(filters.LogDate, out filterdate);
            }

            Criteria = 
            a => (
            (!string.IsNullOrEmpty(filters.TableName) ?
            a.TableName.Trim().ToLower().Contains(filters.TableName.Trim().ToLower()) : true) &&
            (!string.IsNullOrEmpty(filters.OldValue) ?
            a.OldValue.Trim().ToLower().Contains(filters.OldValue.Trim().ToLower()) : true) &&
            (!string.IsNullOrEmpty(filters.NewValue) ?
            a.NewValue.Trim().ToLower().Contains(filters.NewValue.Trim().ToLower()) : true) &&
            (!string.IsNullOrEmpty(filters.ColumnName) ?
            a.ColumnName.Trim().ToLower().Contains(filters.ColumnName.Trim().ToLower()) : true) &&
            (!string.IsNullOrEmpty(filters.ActionName) ? 
            a.ActionName.Trim().ToLower().Contains(filters.ActionName.Trim().ToLower()) : true) && 
            (filters.CreatedBy != null && filters.CreatedBy != 0 ? 
            a.CreatedBy == filters.CreatedBy : true) &&
            (filterdate != DateTime.MinValue ? 
            ((DateTime)a.LogDate).Date == filterdate.Date : true));

            var logs = _unitOfWork.SystemLogs.FindAllPaging(criteria:Criteria, CurrentPage:filters.PageNumber, NumberOfItemsPerPage:filters.PageSize,orderBy:a=>a.Id,orderByDirection:ApplicationConsts.OrderByDescending);
            var users = _unitOfWork.Users.FindAll(a => logs.Select(x => x.CreatedBy).Contains(a.Id)).ToList();

            var data = _mapper.Map<List<GetSystemLogDto>>(logs);
            response.PaginationHeader.TotalItems = logs.TotalCount;
            response.PaginationHeader.CurrentPage = logs.CurrentPage;
            response.PaginationHeader.ItemsPerPage = logs.PageSize;
            response.PaginationHeader.TotalPages = logs.TotalPages;

            response.Data = data;
            return response;
        }

        public BaseResponseWithData<List<string>> GetLogsActionNames()
        {
            BaseResponseWithData<List<string>> response = new BaseResponseWithData<List<string>>();
            response.Result = true;
            response.Errors = new List<Error>();
            response.Data = new List<string>();

            var logs = _unitOfWork.SystemLogs.GetAll().Select(a=>a.ActionName).Distinct().ToList();
            response.Data = logs;
            return response;
        }

        public BaseResponseWithData<string> GetSystemLogReport([FromHeader] long? UserId, [FromHeader] string TableName, [FromHeader] long? RelatedToId ,string CompanyName)
        {
            BaseResponseWithData<string> Response = new BaseResponseWithData<string>();
            Response.Result = true;
            Response.Errors = new List<Error>();
            try
            {
                var logs = _unitOfWork.SystemLogs.FindAllQueryable(a => true, includes: new[] { "CreatedByNavigation" });

                if (UserId != 0 && UserId != null)
                {
                    logs = logs.Where(a => a.CreatedBy == UserId).AsQueryable();
                }
                if (TableName != null)
                {
                    logs = logs.Where(a => a.TableName.ToLower() == TableName.ToLower()).AsQueryable();
                }

                if (!string.IsNullOrEmpty(TableName)&&RelatedToId != null)
                {
                    logs = logs.Where(a => a.TableId == RelatedToId).AsQueryable();
                }
                var logsList = logs.ToList().OrderBy(a=>a.TableName);

                var TasksIds = logsList.Where(a => a.TableId!=null&& a.TableName == "Task").Select(a => (long)a.TableId).ToList();
                var ProjectsIds = logsList.Where(a => a.TableId != null && a.TableName == "Project").Select(a => (long)a.TableId).ToList();
                var HrUsersIds = logsList.Where(a => a.TableId != null && a.TableName == "HrUser").Select(a => (long)a.TableId).ToList();

                var tasks = _unitOfWork.Tasks.FindAll(x => TasksIds.Contains(x.Id)).ToList();

                var Projects = _unitOfWork.Projects.FindAll(x => ProjectsIds.Contains(x.Id), includes: new[] { "SalesOffer" }).ToList();

                var HrUsers = _unitOfWork.HrUsers.FindAll(x => HrUsersIds.Contains(x.Id)).ToList();


                var package = new ExcelPackage();
                var dt = new System.Data.DataTable("Grid");
                dt.Columns.AddRange(new DataColumn[9] {
                new DataColumn("Related To"),
                new DataColumn("Table Name"),
                new DataColumn("Action Name"),
                new DataColumn("Table ID"),
                new DataColumn("Change In"),
                new DataColumn("Old Value"),
                new DataColumn("New Value"),
                new DataColumn("Action Date"),
                new DataColumn("By User"),
                });
                foreach (var item in logsList)
                {
                    if (item.TableName == "Task")
                    {
                        dt.Rows.Add(
                        tasks.FirstOrDefault(a => a.Id == item.TableId)?.Name,
                        item.TableName,
                        item.ActionName,
                        item.TableId,
                        item.ColumnName,
                        item.OldValue,
                        item.NewValue,
                        item.LogDate.ToString(),
                        item.CreatedByNavigation.FirstName + " " + item.CreatedByNavigation.LastName
                        );
                    }
                    else if (item.TableName == "Project")
                    {
                        dt.Rows.Add(
                        Projects.FirstOrDefault(a => a.Id == item.TableId)?.SalesOffer?.ProjectName,
                        item.TableName,
                        item.ActionName,
                        item.TableId,
                        item.ColumnName,
                        item.OldValue,
                        item.NewValue,
                        item.LogDate.ToString(),
                        item.CreatedByNavigation.FirstName + " " + item.CreatedByNavigation.LastName
                        );
                    }
                    else if (item.TableName == "HrUser")
                    {
                        dt.Rows.Add(
                        HrUsers.Where(a => a.Id == item.TableId).Select(a => a.FirstName + " " + a.LastName).FirstOrDefault(),
                        item.TableName,
                        item.ActionName,
                        item.TableId,
                        item.ColumnName,
                        item.OldValue,
                        item.NewValue,
                        item.LogDate.ToString(),
                        item.CreatedByNavigation.FirstName + " " + item.CreatedByNavigation.LastName
                        );
                    }

                }


                var workSheet = package.Workbook.Worksheets.Add("SystemLogReport");
                workSheet.DefaultRowHeight = 12;
                workSheet.Row(1).Height = 20;
                workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Row(1).Style.Font.Bold = true;
                workSheet.Cells[1, 1, 1, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[1, 1, 1, 9].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 174, 81));
                ExcelRangeBase excelRangeBase = workSheet.Cells.LoadFromDataTable(dt, true);
                for (int i = 1; i <= excelRangeBase.Columns; i++)
                {
                    workSheet.Column(i).AutoFit();
                }
                for (int i = 1; i <= excelRangeBase.Rows; i++)
                {
                    workSheet.Row(i).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(i).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }

                workSheet.Column(6).Width = 20;
                workSheet.Column(7).Width = 20;
                
                workSheet.View.FreezePanes(2, 1);

                var newpath = $"Attachments\\{CompanyName}\\SystemLogReports";
                var savedPath = System.IO.Path.Combine(_host.WebRootPath, newpath);
                if (File.Exists(savedPath))
                {
                    File.Delete(savedPath);
                }
                Directory.CreateDirectory(savedPath);
                var date = DateTime.Now.ToString("yyyyMMddHHssFFF");
                var excelPath = savedPath + $"\\SystemLogReport_{date}.xlsx";
                package.SaveAs(excelPath);
                package.Dispose();
                Response.Data = Globals.baseURL + '\\' + newpath + $"\\SystemLogReport_{date}.xlsx";
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

        public void AddLogError(string ActionName, string ErrorMessage, long userId, string CompName, string description = null)
        {
            var dir = $"Attachments\\{CompName}\\ErrorLogs";
            var fileName = $"log_{DateTime.Now.Date.ToString("ddMMyyyy")}.txt";
            

            var savedPath = Path.Combine(_host.WebRootPath, dir);

            if (!Directory.Exists(savedPath))
            {
                Directory.CreateDirectory(savedPath);
            }

            var fullFilePath = Path.Combine(savedPath, fileName);

            if(!File.Exists(fullFilePath))
            {
                string firstLogEntry =  $" ActionName | " +
                                        $" ErrorMessage | " +
                                        $" description | " +
                                        $" userId | " +
                                        $" DateTime.Now:yyyy-MM-dd HH:mm:ss{Environment.NewLine}";

                // Write the log entry to the file
                File.AppendAllText(fullFilePath, firstLogEntry);
            }
            // Create a log entry with the required columns
            string logEntry = $"{ActionName} | " +
                                $"{ErrorMessage} | " +
                                $"{description} | " +
                                $"{userId} | " +
                                $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}{Environment.NewLine}";

            // Write the log entry to the file
            File.AppendAllText(fullFilePath, logEntry);
          
        }

        
    }
}
