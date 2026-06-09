namespace Products.DTOs;

public class MakersDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<ProductsDto> Products { get; set; }
}