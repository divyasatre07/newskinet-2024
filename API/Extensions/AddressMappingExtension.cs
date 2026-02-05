using API.DTO;
using Core.Entities;

namespace API.Extensions
{
	public static class AddressExtensions
	{

		public static AddressDto? ToDto(this Address? address)
		{
			if (address == null) return null;
			return new AddressDto
			{
				Line1 = address.Line1,
				Line2 = address.Line2,
				City = address.City,
				State = address.State,
				PostalCode = address.PostalCode,
				Country = address.Country
			};
		}
		public static Address ToEntity(this AddressDto dto)
		{
			
			return new Address
			{
				Line1 = dto.Line1,
				Line2 = dto.Line2,
				City = dto.City,
				State = dto.State,
				PostalCode = dto.PostalCode,
				Country = dto.Country
			};
		}

	
		public static void UpdateFromDto(this Address address, AddressDto dto)
		{
			address.Line1 = dto.Line1;
			address.Line2 = dto.Line2;
			address.City = dto.City;
			address.State = dto.State;
			address.PostalCode = dto.PostalCode;
			address.Country = dto.Country;
		}
	}
}
