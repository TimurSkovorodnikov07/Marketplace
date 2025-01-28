using System.ComponentModel.DataAnnotations;

namespace server_app.Domain.Model.Queries;

public class GetDeliveryCompanyQuery
{
    [StringLength(20)] public string? CompanyName { get; set; }    
}