using AutoMapper;
using Microsoft.Extensions.Configuration;
using NewGaras.Infrastructure.DTO;

using NewGaras.Infrastructure.DTO.HrUser;

using NewGaras.Infrastructure.DTO.VacationDay;
using NewGaras.Infrastructure.DTO.VacationOverTimeAndDeductionRates;
using NewGaras.Infrastructure.DTO.VacationType;
using NewGaras.Infrastructure.DTO.WorkFlow;
using NewGaras.Infrastructure.Entities;
using NewGaras.Infrastructure.Models;
using NewGaras.Infrastructure.Models.HrUser;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Domain.Mappers
{
    public class AutoMapperProfiles : Profile
    {
        //private string BaseURL = "https://garascore.garassolutions.com/";
        //private string BaseURL = "https://byapi.garassolutions.com/";
        //var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        //string BaseURL = MyConfig.GetValue<string>("AppSettings:baseURL");
        string BaseURL = Globals.baseURL;
        public AutoMapperProfiles() 
        {
            CreateMap<HrUserDto, HrUser>().ReverseMap();
       

            CreateMap<HrUser, GetHrUserDto>()
                .ForMember(HrUser => HrUser.HrNationalityId, GetDto => GetDto.MapFrom(a => a.NationalityId))
                .ForMember(HrUser => HrUser.DateOfBirth, GetDto => GetDto.MapFrom(a => a.DateOfBirth != null ? ((DateTime)a.DateOfBirth).ToShortDateString() : null))
                .ForMember(HrUser => HrUser.SystemEmail, GetDto => GetDto.MapFrom(a => a.User.Email))
                .ForMember(HrUser => HrUser.DepName, GetDto => GetDto.MapFrom(a => a.Department.Name))
                .ForMember(HrUser => HrUser.TeamName, GetDto => GetDto.MapFrom(a => a.Team.Name))
                .ForMember(HrUser => HrUser.BranchName, GetDto => GetDto.MapFrom(a => a.Branch.Name))
                .ForMember(HrUser => HrUser.MaritalStatusName, GetDto => GetDto.MapFrom(a => a.MaritalStatus.Name))
                .ForMember(HrUser => HrUser.JobTitle, GetDto => GetDto.MapFrom(a => a.JobTitle.Name))
                .ForMember(HrUser => HrUser.ImgPath, GetDto => GetDto.MapFrom(a => a.ImgPath != null ? BaseURL + a.ImgPath : null)) ;
            CreateMap<HrUser, GetHrTeamUsersDto>();
         














           













         



        // -------------------------------------------------------------------------Hany --------------------------------------------------
     

        }
    }
}
