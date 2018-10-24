using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAPI.Dto;
using UserAPI.Models;

namespace UserAPI.Helpers
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<TableUser, TableUserDto>();
            CreateMap<TableUserDto, TableUser>();
        }
        
    }
}
