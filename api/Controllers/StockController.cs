using api.Data;
using api.Dtos;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
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
    public IActionResult GetAll()
    {
        var stocks = _context.Stocks.ToList()
            .Select(s => s.ToStockDto());
        return Ok(stocks);
    }

    [HttpGet("{id}")]
    public IActionResult GetById([FromRoute] int id)
    {
        var stock = _context.Stocks.Find(id);
        if (stock == null)
        {
            return NotFound();
        }

        return Ok(stock.ToStockDto());
    }

    [HttpPost]
    public IActionResult Create([FromBody] StockInput stockDto)
    {
        var stock = stockDto.ToStockDtoFromInput();
        _context.Stocks.Add(stock);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetById), new { id = stock.Id }, stock.ToStockDto());
    }

    [HttpPut("{id}")]
    public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockDto stockToUpdate)
    {
        var stock = _context.Stocks.Find(id);
        if (stock is null)
        {
            return NotFound();
        }

        stock.Symbol = stockToUpdate.Symbol ?? stock.Symbol;
        stock.CompanyName = stockToUpdate.CompanyName ?? stock.CompanyName;
        stock.Industry = stockToUpdate.Industry ?? stock.Industry;
        stock.LastDiv = stockToUpdate.LastDiv ?? stock.LastDiv;
        stock.MarketCap = stockToUpdate.MarketCap ?? stock.MarketCap;
        stock.Purchase = stockToUpdate.Purchase ?? stock.Purchase;

        _context.SaveChanges();

        return Ok(stock.ToStockDto());
    }

    [HttpDelete("{id}")]
    public IActionResult Delete([FromRoute] int id)
    {
        var stock = _context.Stocks.Find(id);
        if (stock is null)
        {
            return NotFound();
        }

        _context.Stocks.Remove(stock);
        _context.SaveChanges();

        return NoContent();
    }
}