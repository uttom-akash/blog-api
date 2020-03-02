using AutoMapper;
using Blog_Rest_Api.DTOModels;
using Blog_Rest_Api.Persistent_Model;

namespace Blog_Rest_Api.Auto_Mapper{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Story,RequestStoryDTO>();
            CreateMap<Story,ResponseStoryDTO>();
            CreateMap<User,AuthorDTO>();
            CreateMap<User,UserInfoDTO>();
            CreateMap<User,LoggedInUserDTO>();
            CreateMap<UserRegistrationDTO,User>();
        }
    }
}