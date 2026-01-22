namespace Beyti.Dtos
{
    public class PlaceOrderDto
    {
        public int ChefId { get; set; }
        public List<PlaceOrderItemDto> Items { get; set; } = new();
    }
}
