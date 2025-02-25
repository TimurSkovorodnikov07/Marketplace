using AutoMapper;
using Microsoft.EntityFrameworkCore;
using server_app.Application.Repositories;
using server_app.Domain.Entities.ProductCategories.DeliveryCompanies;
using server_app.Domain.Entities.ProductCategories.ValueObjects;
using server_app.Domain.Model.Dtos;

namespace server_app.Infrastructure.Repositories.ProductCategories;

public class DeliveryCompanyRepository(MainDbContext context, IMapper mapper)
    : IDeliveryCompanyRepository
{
    public async Task<DeliveryCompanyEntity?> Get(Guid guid)
    {
        return await context.Companies
            .FirstOrDefaultAsync(p => p.Id == guid);
    }
    public IEnumerable<DeliveryCompanyForViewerDto> GetAllCompanies()
    {
        return context.Companies
            .Select(c => mapper.Map<DeliveryCompanyForViewerDto>(c));
    }

    public IEnumerable<DeliveryCompanyForViewerDto> SearchCompaniesByName(string str)
    {
        var strToLower = str.ToLower();

        return context.Companies
            .Where(c => c.Name.ToLower().Contains(strToLower))
            .Select(c => mapper.Map<DeliveryCompanyForViewerDto>(c));
    }

    public async Task<DeliveryCompanyEntity?> GetByAnyParam(string name, string webSite, PhoneNumberValueObject phoneNum)
    {
        return await context.Companies
            .FirstOrDefaultAsync(p => p.Name == name
                                      || p.WebSite.WebSiteValue == webSite
                                      || p.PhoneNumber.Number == phoneNum.Number);
    }

    public async Task Add(DeliveryCompanyEntity newDeliveryCompany)
    {
        await context.Companies.AddAsync(newDeliveryCompany);
        await context.SaveChangesAsync();
    }

    public async Task<bool> Update(DeliveryCompanyUpdatedDto updatedCompany)
    {
        if (await context.Companies
                .AnyAsync(x => x.Id == updatedCompany.Id) == false)
            
            return false;

        var mappedCompany = mapper.Map<DeliveryCompanyEntity>(updatedCompany);

        context.Update(mappedCompany);
        await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> Remove(Guid guid)
    {
        var deletedCompany = await Get(guid);

        if (deletedCompany == null)
            return false;

        context.Companies.Remove(deletedCompany);
        await context.SaveChangesAsync();

        return true;
    }
}