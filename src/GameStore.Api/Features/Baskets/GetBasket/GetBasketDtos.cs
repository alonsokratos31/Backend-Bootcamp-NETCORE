namespace GameStore.Api.Features.Baskets.GetBasket;

public record class BasketItemDto(Guid id, string Name, decimal Price, int Quantity, string ImageUri);

public record class BasketDto(Guid CustomerId, IEnumerable<BasketItemDto> Items)
{
    public decimal TotalAmount => Items.Sum(item => item.Price * item.Quantity);
}


