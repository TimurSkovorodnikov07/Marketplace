using AutoMapper;
using Microsoft.EntityFrameworkCore;

public class DeliveryCompanyService(MainDbContext context, IMapper mapper)
    : IEntityService<DeliveryCompanyEntity, DeliveryCompanyUpdatedDto>
{
    public async Task<DeliveryCompanyEntity?> Get(Guid guid)
    {
        return await context.Companies
            .FirstOrDefaultAsync(p => p.Id == guid);
    }

    public async Task<DeliveryCompanyEntity?> GetByAnyParam(string name, Uri webSite, PhoneNumberValueObject phoneNum)
    {
        return await context.Companies
            .FirstOrDefaultAsync(p => p.Name == name
                                      || p.WebSite == webSite
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