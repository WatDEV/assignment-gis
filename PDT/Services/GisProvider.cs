using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using PDT.DTOs;
using PDT.Entities;
using PDT.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PDT.Services
{
	public class GisProvider : IGisProvider
	{
		private GisDBContext db;

		public GisProvider(GisDBContext database)
		{
			db = database;
		}
		public IEnumerable<string> GetCityParts()
		{
			return db.PlanetOsmPolygon.Where(p => p.Boundary == "administrative")
				.Where(x => !string.IsNullOrEmpty(x.Name))
				.Select(x => x.Name)
				.Distinct()
				.ToList();
		}

		public IEnumerable<BarDto> GetBars(double centerLat, double centerLon, double radius)
		{
			var centerPoint = GetPoint(centerLon, centerLat);

			return (from bar in db.PlanetOsmPoint
					where (bar.Amenity == "bar" || bar.Amenity == "pub") && bar.Way.Distance(centerPoint) < radius
					select new BarDto
					{
						Name = bar.Name,
						Id = Convert.ToInt32(bar.Id),
						Lat = TransformationUtils.GetLattitude(bar.Way.Y),
						Lon = TransformationUtils.GetLongtitude(bar.Way.X),
						BarType = bar.Amenity
					}).ToList();
		}
		public IEnumerable<BarDto> GetBars(string[] cityParts)
		{
			if (cityParts == null || cityParts.Length == 0)
			{
				return (from bar in db.PlanetOsmPoint
						where (bar.Amenity == "bar" || bar.Amenity == "pub")
						select new BarDto
						{
							Name = bar.Name,
							Id = Convert.ToInt32(bar.Id),
							Lat = TransformationUtils.GetLattitude(bar.Way.Y),
							Lon = TransformationUtils.GetLongtitude(bar.Way.X),
							BarType = bar.Amenity
						}).ToList();
			}

			return (from bar in db.PlanetOsmPoint where bar.Amenity == "bar" || bar.Amenity == "pub"
					from cityPart in db.PlanetOsmPolygon where cityParts.Contains(cityPart.Name)
					where cityPart.Way.Contains(bar.Way)
					select new BarDto
					{
						Name = bar.Name,
						Id = Convert.ToInt32(bar.Id),
						Lat = TransformationUtils.GetLattitude(bar.Way.Y),
						Lon = TransformationUtils.GetLongtitude(bar.Way.X),
						BarType = bar.Amenity
					}).ToList();
		}

		public IEnumerable<ParkDto> GetParks(double centerLat, double centerLon, double radius)
		{
			var centerPoint = GetPoint(centerLon, centerLat);

			var benches = db.PlanetOsmPoint.Where(x => x.Amenity == "bench").Select(x => x.Way).ToList();

			return (from park in db.PlanetOsmPolygon where park.Leisure == "park" && park.Way.Distance(centerPoint) < radius
					select new ParkDto
					{
						Name = park.Name,
						Id = Convert.ToInt32(park.Id),
						Location = park.Way.Boundary.Coordinates.Select(y => new double[]
						{
							TransformationUtils.GetLongtitude(y.X),
							TransformationUtils.GetLattitude(y.Y)
						}).ToList(),
						HasBenches = benches.FirstOrDefault(b => park.Way.Contains(b)) != null
					}).ToList();
		}
		public IEnumerable<ParkDto> GetParks(string[] cityParts)
		{
			var benches = db.PlanetOsmPoint.Where(x => x.Amenity == "bench").Select(x => x.Way).ToList();

			if (cityParts == null || cityParts.Length == 0)
			{
				return (from park in db.PlanetOsmPolygon where park.Leisure == "park"
						select new ParkDto
						{
							Name = park.Name,
							Id = Convert.ToInt32(park.Id),
							Location = park.Way.Boundary.Coordinates.Select(y => new double[]
							{
							TransformationUtils.GetLongtitude(y.X),
							TransformationUtils.GetLattitude(y.Y)
							}).ToList(),
							HasBenches = benches.FirstOrDefault(b => park.Way.Contains(b)) != null
						}).ToList();
			}

			var toReturn = (from park in db.PlanetOsmPolygon where park.Leisure == "park"
							from cityPart in db.PlanetOsmPolygon where cityParts.Contains(cityPart.Name)
							where park.Way.Within(cityPart.Way)
							select new ParkDto
							{
								Name = park.Name,
								Id = Convert.ToInt32(park.Id),
								Location = park.Way.Boundary.Coordinates.Select(y => new double[]
								{
									TransformationUtils.GetLongtitude(y.X),
									TransformationUtils.GetLattitude(y.Y)
								}).ToList(),
								HasBenches = benches.FirstOrDefault(b => park.Way.Contains(b)) != null
							}).ToList();
			return toReturn;
		}

		public IEnumerable<ShopDto> GetNearbyShops(int parkId, double radius)
		{
			var toReturn = (from park in db.PlanetOsmPolygon where park.Id == parkId
							from shop in db.PlanetOsmPoint where shop.Shop == "alcohol" || shop.Shop == "wine" || shop.Shop == "supermarket" || shop.Shop == "tobacco"
							where shop.Way.Distance(park.Way) < radius
							select new ShopDto
							{
								Name = shop.Name,
								Id = Convert.ToInt32(shop.Id),
								Lat = TransformationUtils.GetLattitude(shop.Way.Y),
								Lon = TransformationUtils.GetLongtitude(shop.Way.X),
								ShopType = shop.Shop
							}).ToList();
			return toReturn;
		}
		public IEnumerable<string> GetNearbyStreets(double centerLat, double centerLon, double radius)
		{
			var centerPoint = GetPoint(centerLon, centerLat);
			return (from street in db.PlanetOsmLine
					where street.Name != null && street.Highway != null && street.Way.Distance(centerPoint) < radius
					select street.Name).OrderBy(x => x).Distinct().ToList();
		}
		public IEnumerable<string> GetStreetsInCityParts(string[] cityParts)
		{
			if (cityParts == null || cityParts.Length == 0)
			{
				return (from street in db.PlanetOsmLine where street.Name != null && street.Highway != null
						select street.Name).OrderBy(x => x).Distinct().ToList();
			}

			var toReturn = (from street in db.PlanetOsmLine
							where street.Name != null && street.Highway != null
							from cityPart in db.PlanetOsmPolygon where cityParts.Contains(cityPart.Name)
							where street.Way.Intersects(cityPart.Way)
							select street.Name).OrderBy(x => x).Distinct().ToList();
			return toReturn;
		}

		public IEnumerable<BarDto> GetBarsOnStreets(string[] streets)
		{
			if (streets == null || streets.Length == 0)
			{
				return new List<BarDto>();
			}

			var toReturn = (from bar in db.PlanetOsmPoint where bar.Amenity == "bar" || bar.Amenity == "pub"
							from street in db.PlanetOsmLine where streets.Contains(street.Name)
							where street.Way.Distance(bar.Way) <= 25
							select new BarDto
							{
								Name = bar.Name,
								Id = Convert.ToInt32(bar.Id),
								Lat = TransformationUtils.GetLattitude(bar.Way.Y),
								Lon = TransformationUtils.GetLongtitude(bar.Way.X),
								BarType = bar.Amenity
							}).ToList();
			return toReturn;
		}

		public IEnumerable<StreetDto> GetStreetMapProperties(string[] streets)
		{
			var toReturn = (from street in db.PlanetOsmLine
							where streets.Contains(street.Name)
							select new StreetDto
							{
								Name = street.Name,
								Id = Convert.ToInt32(street.Id),
								Location = street.Way.Coordinates.Select(y => new double[]
								{
									TransformationUtils.GetLongtitude(y.X),
									TransformationUtils.GetLattitude(y.Y)
								}).ToList()
							}).ToList();
			return toReturn;
		}


		private Point GetPoint(double lon, double lat)
		{
			var coordinates = new Coordinate
			{
				X = TransformationUtils.ParseLongtitudeToGisFormat(lon),
				Y = TransformationUtils.ParseLattitudeToGisFormat(lat)
			};
			return new Point(coordinates)
			{
				SRID = 3857
			};
		}
	}
}
