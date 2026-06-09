using Microsoft.Data.SqlClient;
using Products.DTOs;
using Products.Exceptions;

namespace Products.Services;

public class DbService : IDbService
{
    private readonly string _connectionString;
    
    public DbService(string connectionString)
    {
        _connectionString = connectionString;
    }

    // public async Task<List<MakersDto>> GetAllAsync(CancellationToken cancellationToken)
    // {
    //     var query = """
    //                 SELECT m.Id AS MakerId,
    //                 m.Name AS MakerName,
    //                 p.Id AS ProductId,
    //                 p.Name AS ProductName,
    //                 p.Description AS ProductDescription,
    //                 p.StickerPrice AS ProductPrice,
    //                 pt.Id AS ProductTypeId,
    //                 pt.Name AS ProductTypeName,
    //                 v.Code AS VendorCode,
    //                 v.Name AS VendorName,
    //                 vp.Amount AS Amount,
    //                 vp.PricePerUnit AS PricePerUnit
    //                 FROM Makers m
    //                 JOIN Products p ON m.Id = p.MakerId
    //                 JOIN ProductTypes pt ON pt.Id = p.ProductTypeId
    //                 JOIN VendorProducts vp ON vp.ProductId = p.Id
    //                 JOIN Vendors v ON v.Code = vp.VendorCode
    //                 """;
    //
    //     await using var connection = new SqlConnection(_connectionString);
    //     await connection.OpenAsync(cancellationToken);
    //     
    //     await using var command = new SqlCommand();
    //     command.Connection = connection;
    //     command.CommandText = query;
    //     await using var reader = await command.ExecuteReaderAsync(cancellationToken);
    //
    //     var result = new List<MakersDto>();
    //
    //     var ordMakerId = reader.GetOrdinal("MakerId");
    //     var ordMakerName = reader.GetOrdinal("MakerName");
    //     var ordProductId = reader.GetOrdinal("ProductId");
    //     var ordProductName = reader.GetOrdinal("ProductName");
    //     var ordDescription = reader.GetOrdinal("ProductDescription");
    //     var ordStickerPrice = reader.GetOrdinal("ProductPrice");
    //     var ordProductTypeId = reader.GetOrdinal("ProductTypeId");
    //     var ordProductTypeName = reader.GetOrdinal("ProductTypeName");
    //     var ordVendorCode = reader.GetOrdinal("VendorCode");
    //     var ordVendorName = reader.GetOrdinal("VendorName");
    //     var ordAmount = reader.GetOrdinal("Amount");
    //     var ordPricePerUnit = reader.GetOrdinal("PricePerUnit");
    //
    //     while (await reader.ReadAsync(cancellationToken))
    //     {
    //         
    //         var makerId = reader.GetInt32(ordMakerId);
    //         var maker = result.FirstOrDefault(m => m.Id == makerId);
    //         
    //         if (maker is null)
    //         {
    //             maker = new MakersDto
    //             {
    //                 Id = reader.GetInt32(ordMakerId),
    //                 Name = reader.GetString(ordMakerName),
    //                 Products = new List<ProductsDto>()
    //             };
    //             result.Add(maker);
    //         }
    //         
    //         var productId = reader.GetInt32(ordProductId);
    //         var product = maker.Products.FirstOrDefault(e => e.Id.Equals(productId));
    //
    //         if (product is null)
    //         {
    //             product = new ProductsDto
    //             {
    //                 Id = productId,
    //                 Name = reader.GetString(ordProductName),
    //                 Description = reader.GetString(ordDescription),
    //                 Price = reader.GetDecimal(ordStickerPrice),
    //                 ProductType = new ProductTypsDto
    //                 {
    //                     Id = reader.GetInt32(ordProductTypeId),
    //                     Name = reader.GetString(ordProductTypeName)
    //                 },
    //                 Vendors = new List<VendorsDto>()
    //             };
    //             maker.Products.Add(product);
    //         }
    //         product.Vendors.Add(new VendorsDto
    //         {
    //             Code = reader.GetString(ordVendorCode),
    //             Name = reader.GetString(ordVendorName),
    //             Amount = reader.GetInt32(ordAmount),
    //             PricePerUnit = reader.GetDecimal(ordPricePerUnit)
    //         });
    //     }
    //
    //     return result;
    // }

    public async Task<List<MakersDto>> GetNameAsync(string? name, CancellationToken cancellationToken)
    {
        var query = """
                    SELECT m.Id AS MakerId,
                    m.Name AS MakerName,
                    p.Id AS ProductId,
                    p.Name AS ProductName,
                    p.Description AS ProductDescription,
                    p.StickerPrice AS ProductPrice,
                    pt.Id AS ProductTypeId,
                    pt.Name AS ProductTypeName,
                    v.Code AS VendorCode,
                    v.Name AS VendorName,
                    vp.Amount AS Amount,
                    vp.PricePerUnit AS PricePerUnit
                    FROM Makers m
                    JOIN Products p ON m.Id = p.MakerId
                    JOIN ProductTypes pt ON pt.Id = p.ProductTypeId
                    JOIN VendorProducts vp ON vp.ProductId = p.Id
                    JOIN Vendors v ON v.Code = vp.VendorCode
                    WHERE (@Name  IS NULL OR m.name LIKE @Name)
                    """;
        
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        
        
        await using var command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@Name",name == null ? DBNull.Value : $"%{name}%");
        
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        var results = new List<MakersDto>();
        
        
        var ordMakerId = reader.GetOrdinal("MakerId");
        var ordMakerName = reader.GetOrdinal("MakerName");
        var ordProductId = reader.GetOrdinal("ProductId");
        var ordProductName = reader.GetOrdinal("ProductName");
        var ordDescription = reader.GetOrdinal("ProductDescription");
        var ordStickerPrice = reader.GetOrdinal("ProductPrice");
        var ordProductTypeId = reader.GetOrdinal("ProductTypeId");
        var ordProductTypeName = reader.GetOrdinal("ProductTypeName");
        var ordVendorCode = reader.GetOrdinal("VendorCode");
        var ordVendorName = reader.GetOrdinal("VendorName");
        var ordAmount = reader.GetOrdinal("Amount");
        var ordPricePerUnit = reader.GetOrdinal("PricePerUnit");

        while (await reader.ReadAsync(cancellationToken))
        {
            var maker = results.FirstOrDefault(c => c.Name == reader.GetString(ordMakerName));

            if (maker is null)
            {
                maker = new MakersDto
                {
                    Id = reader.GetInt32(ordMakerId),
                    Name = reader.GetString(ordMakerName),

                    Products = new List<ProductsDto>()
                };
                results.Add(maker);
            }
            
            var productId = reader.GetInt32(ordProductId);
            var product = maker.Products.FirstOrDefault(p => p.Id == productId);
            if (product is null)
            {
                product = new ProductsDto
                {
                    Id = productId,
                    Name = reader.GetString(ordProductName),
                    Description = reader.GetString(ordDescription),
                    Price = reader.GetDecimal(ordStickerPrice),
                    ProductType = new ProductTypsDto
                    {
                        Id = reader.GetInt32(ordProductTypeId),
                        Name = reader.GetString(ordProductTypeName)
                    },
                    Vendors = new List<VendorsDto>()
                };
                maker.Products.Add(product);
            }
            product.Vendors.Add(new VendorsDto
            {
                Code = reader.GetString(ordVendorCode),
                Name = reader.GetString(ordVendorName),
                Amount = reader.GetInt32(ordAmount),
                PricePerUnit = reader.GetDecimal(ordPricePerUnit)
            });
        }
        
        return results.Any() ? results : throw new NotFoundException("No makers found.");
    
        }

    public async Task createMakersWithProductsAsync(CreateMakerDto dto, CancellationToken cancellationToken)
    {
        var createMakersQuery = """
                                INSERT INTO Makers
                                VALUES(@Name)
                                SELECT @@IDENTITY;
                                """;
        var createProductsQuery = """
                                  INSERT INTO Products
                                  VALUES(@Name, @Description, @StickerPrice, @ProductTypeId, @MakerId)
                                  SELECT @@IDENTITY;
                                  """;
        var getProductType = """
                             SELECT Id
                             FROM ProductTypes
                             WHERE Name = @TypeName;
                             """;

        var checkMakerQuery = """
                              SELECT ID
                              FROM Makers
                              WHERE Name = @Name;
                              """;
        
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        
         
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);
        
        await using var command = new SqlCommand();
        command.Connection = connection;
        command.Transaction = transaction as SqlTransaction;

        try
        {
            command.Parameters.Clear();
            command.CommandText = checkMakerQuery;
            command.Parameters.AddWithValue(@"Name", dto.Name);
            var existingMaker = await command.ExecuteScalarAsync(cancellationToken);

            if (existingMaker != null)
                throw new Exception($"Maker {dto.Name} already exists.");
            
            command.Parameters.Clear();
            command.CommandText = createMakersQuery;
            command.Parameters.AddWithValue("@Name", dto.Name);
            var makerObject = await command.ExecuteScalarAsync(cancellationToken);
            var makerId = Convert.ToInt32(makerObject);
        

            foreach (var product in dto.Products)
            {
                command.Parameters.Clear();
                command.CommandText = getProductType;
                command.Parameters.AddWithValue("@TypeName", product.Type);
                
                var typeObject = await command.ExecuteScalarAsync(cancellationToken);
                if (typeObject == null)
                {
                    throw new NotFoundException($"Product type - {product.Type} - not found.");
                }
                
                var productTypeId = Convert.ToInt32(typeObject);
                
                command.Parameters.Clear();
                command.CommandText = createProductsQuery;
                command.Parameters.AddWithValue("@Name",  product.Name);
                command.Parameters.AddWithValue("@Description", product.Description);
                command.Parameters.AddWithValue("@StickerPrice", product.StickerPrice);
                command.Parameters.AddWithValue("@ProductTypeId", productTypeId);
                command.Parameters.AddWithValue("@MakerId", makerId);
                
                
                await command.ExecuteNonQueryAsync(cancellationToken);
            }
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
   
}