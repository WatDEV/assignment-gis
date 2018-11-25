using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDT.Services
{
	public static class ExtensionMethods
	{ 
		public static List<T> AddAndParseRange<T,T2>(this List<T> destination, IEnumerable<T2> valuesToAdd, Func<T2,T> parse)
		{
			foreach (var value in valuesToAdd)
			{
				try
				{
					destination.Add(parse(value));
				}
				catch (Exception)
				{
					continue;
				}
			}
			return destination;
		}
	}
}
