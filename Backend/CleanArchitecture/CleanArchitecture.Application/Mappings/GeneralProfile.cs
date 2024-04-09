using AutoMapper;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Features.Users.Commands.CreateProduct;
using CleanArchitecture.Core.Features.Users.Queries.GetAllProducts;

namespace CleanArchitecture.Core.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<User, GetAllProductsViewModel>().ReverseMap();
            CreateMap<CreateUserCommand, User>();
            CreateMap<GetAllProductsQuery, GetAllProductsParameter>();
        }
    }
}
