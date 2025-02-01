using System.ComponentModel.DataAnnotations;

namespace server_app.Presentation.ModelQueries;

public class GetDeliveryCompanyQuery
{
    [StringLength(20)] public string? CompanyName { get; set; }    
}