using AutoMapper;
using Layer_Entities.ModelsDB;
using Layer_Entities.ModelsDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Layer_Service.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            #region User
                CreateMap<User, RegisterRequest>()
                .ReverseMap();

                CreateMap<User, RegisterResponse>()
                .ReverseMap();

                CreateMap<User, AuthenticationResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
                .ReverseMap()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));

            CreateMap<RegisterResponse, AuthenticationResponse>()
                .ReverseMap();
            #endregion

            #region Phones
                CreateMap<RegisterRequest, User>()
                    .ForMember(dest => dest.Phones, opt => opt.MapFrom(src => src.Phones));

                CreateMap<PhonesDTO, Phone>();
            #endregion
        }
    }
}
