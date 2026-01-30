using System.Text.Json.Serialization;

namespace Core.Entities
{
	public class CartItem
	{
		public int ProductId { get; set; }
		public required string ProductName { get; set; }
		public decimal Price { get; set; }
		public int Quantity { get; set; }

		[JsonPropertyName("pictureUrl")]
		public required string PictureURL { get; set; }

		public required string Brand { get; set; }
		public required string Type { get; set; }
	}
}
