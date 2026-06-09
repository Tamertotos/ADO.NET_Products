namespace Products.DTOs;

public class CreateProductDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal StickerPrice { get; set; }
    public string Type { get; set; }
}