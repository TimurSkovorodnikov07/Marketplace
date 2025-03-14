namespace server_app.Domain.Model.Dtos;

public class PurchasedProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal TotalSum { get; set; }
    public int PurchasedQuantity { get; set; }
    
    public Guid CategoryId { get; set; }
    public Guid MainImageId { get; set; }
    
    public DateTime PurchasedDate { get; set; }
    public DateTime MustDeliveredBefore { get; set; }
    public DateTime? DeliveredDate { get; set; }
}