using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PDT.DTOs;
using PDT.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PDT.Controllers
{
	[Route("api/[controller]")]
	public class BarsController : Controller
	{
		private IGisProvider gis;

		public BarsController(IGisProvider gisProvider)
		{
			gis = gisProvider;
		}		

		[HttpGet("[action]")]
		public ActionResult<IEnumerable<FeatureCollection>> GetBars(double centerLat, double centerLon, double radius)
		{
			return Ok(new FeatureCollection(gis.GetBars(centerLat, centerLon, radius).Select(bar => GeoJsonParser.ParseBarToFeature(bar)).ToList()));;
		}

		[HttpPost("[action]")]
		public ActionResult<IEnumerable<FeatureCollection>> GetBars([FromBody] string[] cityParts)
		{
			return Ok(new FeatureCollection(gis.GetBars(cityParts).Select(bar => GeoJsonParser.ParseBarToFeature(bar)).ToList()));
		}

		[HttpPost("[action]")]
		public ActionResult<IEnumerable<FeatureCollection>> GetStreetsWithBars([FromBody] string[] streets)
		{
			var features = gis.GetBarsOnStreets(streets).Select(bar => GeoJsonParser.ParseBarToFeature(bar)).ToList();
			features.AddRange(gis.GetStreetMapProperties(streets).Select(street => GeoJsonParser.ParseStreetToFeature(street)));

			return Ok(new FeatureCollection(features));
		}

		[HttpGet("[action]")]
		public ActionResult<IEnumerable<FeatureCollection>> GetParks(double centerLat, double centerLon, double radius)
		{
			var features = new List<Feature>().AddAndParseRange(gis.GetParks(centerLat, centerLon, radius), GeoJsonParser.ParseParkToFeature);
			return Ok(new FeatureCollection(features));
		}

		[HttpPost("[action]")]
		public ActionResult<IEnumerable<FeatureCollection>> GetParks([FromBody] string[] cityParts)
		{
			var features = new List<Feature>().AddAndParseRange(gis.GetParks(cityParts), GeoJsonParser.ParseParkToFeature);
			return Ok(new FeatureCollection(features));			
		}

		[HttpPost("[action]")]
		public ActionResult<IEnumerable<FeatureCollection>> GetNearbyShops(int parkId, double radius, [FromBody] string[] cityParts)
		{
			var features = gis.GetNearbyShops(parkId, radius).Select(shop => GeoJsonParser.ParseShopToFeature(shop)).ToList();
			features.AddAndParseRange(gis.GetParks(cityParts), GeoJsonParser.ParseParkToFeature);

			return Ok(new FeatureCollection(features));;
		}

		[HttpGet("[action]")]
		public ActionResult<IEnumerable<string>> GetNearbyStreets(double centerLat, double centerLon, double radius)
		{
			return Ok(gis.GetNearbyStreets(centerLat, centerLon, radius));
		}

		[HttpPost("[action]")]
		public ActionResult<IEnumerable<string>> GetStreetsInCityParts([FromBody] string[] cityParts)
		{
			return Ok(gis.GetStreetsInCityParts(cityParts));
		}

		[HttpGet("[action]")]
		public ActionResult<IEnumerable<string>> GetCityParts()
		{
			return Ok(gis.GetCityParts());
		}
	}
}
 