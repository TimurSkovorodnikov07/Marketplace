public abstract class ProductCategoryDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public TagsValueObject Tags { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }    
}