using PDT.DTOs;
using System.Collections.Generic;

namespace PDT.Services
{
	public interface IGisProvider
	{
		IEnumerable<BarDto> GetBars(double centerLan, double centerLon, double radius);
		IEnumerable<BarDto> GetBars(string[] cityParts);
		IEnumerable<ParkDto> GetParks(double centerLat, double centerLon, double radius);
		IEnumerable<ParkDto> GetParks(string[] cityParts);
		IEnumerable<ShopDto> GetNearbyShops(int parkId, double radius);
		IEnumerable<string> GetCityParts();
		IEnumerable<string> GetNearbyStreets(double centerLat, double centerLon, double radius);
		IEnumerable<string> GetStreetsInCityParts(string[] cityParts);
		IEnumerable<BarDto> GetBarsOnStreets(string[] streets);
		IEnumerable<StreetDto> GetStreetMapProperties(string[] streets);
	}
}
