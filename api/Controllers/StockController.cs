using api.Data;
using api.Dtos;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace api.Controllers;

[Route("api/stock")]
[ApiController]
public class StockController : ControllerBase
{
    private readonly ApplicationDBContext _context;

    public StockController(ApplicationDBContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var stocks = await _context.Stocks.ToListAsync();
        var stocksDto = stocks.Select(s => s.ToStockDto());
        return Ok(stocks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var stock = await _context.Stocks.FindAsync(id);

        if (stock is null)
            return NotFound();

        return Ok(stock.ToStockDto());
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] StockInput stockDto)
    {
        var stock = stockDto.ToStockDtoFromInput();

        await _context.Stocks.AddAsync(stock);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetById", new { id = stock.Id }, stock.ToStockDto());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockDto stockToUpdate)
    {
        var stock = await _context.Stocks.FindAsync(id);
        if (stock is null)
            return NotFound();

        stock.Symbol = stockToUpdate.Symbol ?? stock.Symbol;
        stock.CompanyName = stockToUpdate.CompanyName ?? stock.CompanyName;
        stock.Industry = stockToUpdate.Industry ?? stock.Industry;
        stock.LastDiv = stockToUpdate.LastDiv ?? stock.LastDiv;
        stock.MarketCap = stockToUpdate.MarketCap ?? stock.MarketCap;
        stock.Purchase = stockToUpdate.Purchase ?? stock.Purchase;

        await _context.SaveChangesAsync();

        return Ok(stock.ToStockDto());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var stock = await _context.Stocks.FindAsync(id);

        if (stock is null)
            return NotFound();

        _context.Stocks.Remove(stock);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}