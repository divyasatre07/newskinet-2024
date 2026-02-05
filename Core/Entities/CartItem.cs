using System.Text.Json.Serialization;

namespace Core.Entities
{
	public class CartItem
	{
		public int ProductId { get; set; }
		public string? ProductName { get; set; }
		public decimal Price { get; set; }
		public int Quantity { get; set; }

		[JsonPropertyName("pictureUrl")]
		public string? PictureURL { get; set; }

		public string? Brand { get; set; }
		public string? Type { get; set; }
	}
}
