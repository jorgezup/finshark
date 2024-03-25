using api.Dtos;
using api.Models;

namespace api.Mappers;

public static class StockMappers
{
    public static StockDto ToStockDto(this Stock stockModel)
    {
        return new StockDto
        {
            Id = stockModel.Id,
            Symbol = stockModel.Symbol,
            CompanyName = stockModel.CompanyName,
            Industry = stockModel.Industry,
            LastDiv = stockModel.LastDiv,
            MarketCap = stockModel.MarketCap,
            Purchase = stockModel.Purchase
        };
    }

    public static Stock ToStockDtoFromInput(this StockInput stockInput)
    {
        return new Stock
        {
            Symbol = stockInput.Symbol,
            CompanyName = stockInput.CompanyName,
            Industry = stockInput.Industry,
            LastDiv = stockInput.LastDiv,
            MarketCap = stockInput.MarketCap,
            Purchase = stockInput.Purchase
        };
    }
}