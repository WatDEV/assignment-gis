using NetTopologySuite.Geometries;
using System.Collections.Generic;

namespace PDT.DTOs
{
	public class ParkDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public List<double[]> Location { get; set; }
		public bool HasBenches { get; set; }
	}
}
