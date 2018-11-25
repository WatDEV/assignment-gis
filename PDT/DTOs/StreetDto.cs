using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDT.DTOs
{
	public class StreetDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public List<double[]> Location { get; set; }
	}
}
