public abstract class ProductCategoryDto
{
    public Guid Id { get; set;}
    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> Tags { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public Guid DeliveryCompanyId { get; set; }
    public List<Guid> ImagesIdentifiers { get; set; }
    
    public int EstimationCount { get; set; }
    public int TotalEstimation { get; set; }
}