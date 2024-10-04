using AutoMapper;

public class MainMapperProfile : Profile
{
    public MainMapperProfile()
    {
        CreateMap<DeliveryCompanyUpdatedDto, DeliveryCompanyEntity>()
            .ForMember(des => des.Name, opt => opt.MapFrom(x => x.NewName))
            .ForMember(des => des.Description, opt => opt.MapFrom(x => x.NewDescription))
            .ForMember(des => des.WebSite, opt => opt.MapFrom(x => x.NewWebSite))
            .ForMember(des => des.PhoneNumber, opt => opt.MapFrom(x => x.NewPhoneNumber));
        
        
        CreateMap<UserEntity, UserDto>();

        CreateMap<ProductCategoryCreateQuery, ProductCategoryCreateDto>()
            .ForMember(x => x.Owner, opt => opt.Ignore())
            .ForMember(x => x.DeliveryCompany, opt => opt.Ignore());
        CreateMap<ProductCategoryUpdateQuery, ProductCategoryUpdateDto>();

        //ProductCategoryEntity => в дто который не палит лишнию инфу
        CreateMap<ProductCategoryEntity, ProductCategoryDtoForOwner>()
            .ForMember(des => des.DeliveryCompanyId, opt => opt.MapFrom(s => s.DeliveryCompany.Id))
            .ForMember(des => des.ImagesIdentifiers, opt => opt.MapFrom(x => x.Images.GetIdentifiers()));
        CreateMap<ProductCategoryEntity, ProductCategoryDtoForViewer>()
            .ForMember(des => des.SelleDtorForViewer, opt => opt.MapFrom(s => s.Owner))
            .ForMember(des => des.ImagesId, opt => opt.MapFrom(x => x.Images.GetIdentifiers()))
            .ForMember(des => des.DeliveryCompanyId, opt => opt.MapFrom(s => s.DeliveryCompany.Id));
        //Вот так можно удобно говорить automapper-у что мапить(по дефолту он маппит по одному и тому же имени свйоства и типу)
    }
}