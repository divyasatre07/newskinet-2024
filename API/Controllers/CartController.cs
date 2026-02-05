using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	public class CartController : BaseAPIController
	{
		private readonly ICartService _cartService;

		public CartController(ICartService cartService)
		{
			_cartService = cartService;
		}

		// GET: api/cart?id=cart1
		[HttpGet]
		public async Task<ActionResult<ShoppingCart>> GetCartById([FromQuery] string id)
		{
			var cart = await _cartService.GetCartAsync(id);
			return Ok(cart ?? new ShoppingCart { Id = id });

		}

		// POST: api/cart
		[HttpPost]
		public async Task<ActionResult<ShoppingCart>> UpdateCart(ShoppingCart cart)
		{
			var updatedCart = await _cartService.SetCartAsync(cart);

			if (updatedCart == null)
				return BadRequest("Problem updating cart");

			return Ok(updatedCart);
		}

		// DELETE: api/cart?id=cart1
		[HttpDelete]
		public async Task<ActionResult> DeleteCart([FromQuery] string id)
		{
			var result = await _cartService.DeleteCartAsync(id);

			if (!result)
				return BadRequest("Problem deleting cart");

			return Ok();
		}
	}
}
