using NewGaras.Domain.Models;
using NewGaras.Infrastructure.DTO.Log;
using NewGaras.Infrastructure.Models;
using NewGaras.Infrastructure.Models.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.Interfaces.ServicesInterfaces
{
    public interface ILogService
    {
        public BaseResponseWithDataAndHeader<List<GetSystemLogDto>> GetLogs([FromHeader] LogFilters filters);

        public BaseResponseWithId AddLog(string ActionName,string tablename,string columnName,int entryId,long createdby);

        public BaseResponseWithData<List<string>> GetLogsActionNames();

        public BaseResponseWithData<string> GetSystemLogReport([FromHeader] long? UserId, [FromHeader] string TableName, [FromHeader] long? RelatedToId, string CompanyName);

        public void AddLogError(string ActionName, string ErrorMessage, long userId, string CompName, string description = null);
    }
}
