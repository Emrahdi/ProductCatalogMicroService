using AutoMapper;
using UserApi.Models;
using UserApi.Services;

namespace UserApi.Helpers {
    public class AutoMapperProfile : Profile {
        public AutoMapperProfile() {
            CreateMap<UserModel, User>().ReverseMap();
        }
    }
}