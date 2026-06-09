namespace Products.DTOs;

public class CreateMakerDto
{
    public string Name { get; set; }
    public List<CreateProductDto> Products { get; set; }
}