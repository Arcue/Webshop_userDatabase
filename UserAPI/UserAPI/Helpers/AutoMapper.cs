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
<<<<<<< HEAD
            
            CreateMap<TableUser, UserInfotDto>();
            CreateMap<TableUserDto, TableUser>();
            
=======
>>>>>>> c97239a217eaee684f79b15c3d37c53fcd0fbc44
        }
        
    }
}
