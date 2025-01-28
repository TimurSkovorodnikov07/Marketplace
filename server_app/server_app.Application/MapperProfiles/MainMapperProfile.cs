using AutoMapper;
using server_app.Application.Extensions;
using server_app.Domain.Entities.ProductCategories;
using server_app.Domain.Entities.ProductCategories.DeliveryCompanies;
using server_app.Domain.Entities.ProductCategories.PurchasedProducts;
using server_app.Domain.Entities.ProductCategories.ValueObjects;
using server_app.Domain.Entities.Users.Seller;
using server_app.Domain.Model.Dtos;
using server_app.Domain.Model.Queries;

namespace server_app.Application.MapperProfiles;

public class MainMapperProfile : Profile
{
    public MainMapperProfile()
    {
        CreateMap<DeliveryCompanyUpdatedDto, DeliveryCompanyEntity>()
            .ForMember(des => des.Name, opt => opt.MapFrom(x => x.NewName))
            .ForMember(des => des.Description, opt => opt.MapFrom(x => x.NewDescription))
            .ForMember(des => des.WebSite, opt => opt.MapFrom(x => x.NewWebSite))
            .ForMember(des => des.PhoneNumber, opt => opt.MapFrom(x => x.NewPhoneNumber));
        CreateMap<DeliveryCompanyEntity, DeliveryCompanyForViewerDto>()
            .ForMember(des => des.WebSite, x => x.MapFrom(c => c.WebSite.ToString()))
            .ForMember(des => des.PhoneNumber, x => x.MapFrom(c => c.PhoneNumber.Number));

        CreateMap<UserEntity, UserDto>();
        CreateMap<SellerEntity, SellerDtoForOwner>();
        CreateMap<SellerEntity, SellerDtoForViewer>();

        CreateMap<ProductCategoryCreateQuery, ProductCategoryCreateDto>()
            .ForMember(x => x.Tags, opt => opt.MapFrom(t => new TagsValueObject { Tags = t.Tags }))
            .ForMember(x => x.Owner, opt => opt.Ignore())
            .ForMember(x => x.DeliveryCompany, opt => opt.Ignore());
        CreateMap<ProductCategoryUpdateQuery, ProductCategoryUpdateDto>()
            .ForMember(x => x.NewTags, opt => opt.MapFrom(t => new TagsValueObject { Tags = t.NewTags }));
        CreateMap<GetReviewsQuery, GetReviewsDto>();

        //ProductCategoryEntity => в обьект который не палит лишнию инфу
        CreateMap<ProductCategoryEntity, ProductCategoryDto>()
            .ForMember(des => des.Tags, opt => opt.MapFrom(x => x.Tags.Tags))
            .ForMember(des => des.DeliveryCompanyId, opt => opt.MapFrom(s => s.DeliveryCompany.Id))
            .ForMember(des => des.ImagesIdentifiers, opt => opt.MapFrom(x => x.Images.GetIdentifiers()))
            .Include<ProductCategoryEntity, ProductCategoryDtoForOwner>()
            .Include<ProductCategoryEntity, ProductCategoryDtoForViewer>();

        CreateMap<ProductCategoryEntity, ProductCategoryDtoForOwner>();
        CreateMap<ProductCategoryEntity, ProductCategoryDtoForViewer>();

        CreateMap<PurchasedProductEntity, PurchasedProductDto>()
            .ForMember(d => d.Name, opt => opt.MapFrom(x => x.Category.Name))
            .ForMember(d => d.Description, opt => opt.MapFrom(x => x.Category.Description))
            .ForMember(d => d.CategoryId, opt => opt.MapFrom(x => x.Category.Id))
            .ForMember(d => d.ImagesIdentifiers, opt => opt.MapFrom(x => x.Category.Images.GetIdentifiers()));
        //Вот так можно удобно говорить automapper-у что мапить(по дефолту он маппит по одному и тому же имени свйоства и типу)
    }
}