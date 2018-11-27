using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using PDT.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDT.Utils
{
	public static class GeoJsonParser
	{
		public static Feature ParseBarToFeature(BarDto bar)
		{
			var geometry = new Point(new Position(bar.Lat, bar.Lon));
			var properties = new Dictionary<string, dynamic>
				{
					{ "Id", bar.Id.ToString() },
					{ "Name", bar.Name == null? "": bar.Name.ToString() },
					{ "BarType" , bar.BarType }
				};
			return new Feature(geometry, properties);
		}

		public static Feature ParseStreetToFeature(StreetDto street)
		{
			var geometry = new LineString(street.Location);
			var properties = new Dictionary<string, dynamic>
				{
					{ "Id", street.Id.ToString() },
					{ "Name", street.Name == null? "": street.Name.ToString() },
				};
			return new Feature(geometry, properties);
		}
		public static Feature ParseParkToFeature(ParkDto park)
		{
			if (park.Location[0][0] != park.Location.Last()[0] && park.Location[0][1] != park.Location.Last()[1])
				park.Location.Add(park.Location[0]);

			var geometry = new Polygon(new List<IEnumerable<IEnumerable<double>>> { park.Location });
			var properties = new Dictionary<string, dynamic>
					{
						{ "Id", park.Id.ToString() },
						{ "Name", park.Name == null? "": park.Name.ToString() },
						{ "HasBenches" , park.HasBenches }
					};
			return new Feature(geometry, properties);
		}

		public static Feature ParseShopToFeature(ShopDto shop)
		{
			var geometry = new Point(new Position(shop.Lat, shop.Lon));
			var properties = new Dictionary<string, dynamic>
				{
					{ "Id", shop.Id.ToString() },
					{ "Name", shop.Name == null? "": shop.Name.ToString() },
					{ "ShopType" , shop.ShopType }
				};
			return new Feature(geometry, properties);
		}
	}
}
