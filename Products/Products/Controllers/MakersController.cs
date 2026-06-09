using Microsoft.AspNetCore.Mvc;
using Products.DTOs;
using Products.Exceptions;
using Products.Services;

namespace Products.Controllers;
[Route("api/[controller]")]
[ApiController]
public class MakersController : ControllerBase
{
    private readonly IDbService _dbService;
    public MakersController(IDbService dbService)
    {
        _dbService = dbService;
    }

    // [HttpGet]
    // public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    // {
    //     try
    //     {
    //         var result = await _dbService.GetAllAsync(cancellationToken);
    //         return Ok(result);
    //     }
    //     catch (NotFoundException e)
    //     {
    //         return NotFound(e.Message);
    //     }
    // }
    
    [HttpGet]
    public async Task<IActionResult> GetByLastName([FromQuery] string? name, CancellationToken ct)
    {
        try
        {
            var result = await _dbService.GetNameAsync(name, ct);
            return Ok(result);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateMakerDto dto, CancellationToken ct)
    {
        if (!dto.Products.Any())
            return BadRequest("At least one product is required.");
        
        try
        {
            await _dbService.createMakersWithProductsAsync(dto, ct);
            return Created($"api/makers", dto);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        } 
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

}