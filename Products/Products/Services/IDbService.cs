using Products.DTOs;

namespace Products.Services;

public interface IDbService
{
    Task<List<MakersDto>> GetNameAsync(String? name, CancellationToken cancellationToken);
    Task createMakersWithProductsAsync(CreateMakerDto dto, CancellationToken cancellationToken);
}