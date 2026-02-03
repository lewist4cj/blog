using AutoMapper;
using Blog.Domain;
using Blog.Services.UserApp;

namespace Blog.Services;

public class BlogProfile:Profile
{
    public BlogProfile()
    {
        // CreateMap<Product, ProductDto>().ReverseMap();
        // CreateMap<ProductSale, ProductDto>().ForMember(dest => dest.Id, opt => opt.Ignore())
        //     .ReverseMap();
        // CreateMap<ProductPhoto, ProductDto>().ForMember(dest => dest.Id, opt => opt.Ignore())
        //     .ReverseMap();
        // CreateMap<ProductSaleAreaDiff, ProductDto>().ForMember(dest => dest.Id, opt => opt.Ignore())
        //     .ReverseMap();

        CreateMap<UserModel, UserModelLoginDto>();

    }
}