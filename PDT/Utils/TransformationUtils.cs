using System;

namespace PDT.Utils
{
	public static class TransformationUtils
	{
		private const double MultiplyConstant = 20037508.34;
		public static double ParseLattitudeToNormalFormat(double y)
		{
			return (Math.Atan(Math.Pow(Math.E, Math.PI / 180 * (y / (MultiplyConstant / 180)))) / (Math.PI / 360)) - 90;
		}

		public static double ParseLongtitudeToNormalFormat(double x)
		{
			return x * 180 / MultiplyConstant;
		}

		public static double ParseLattitudeToGisFormat(double lat)
		{
			return Math.Log(Math.Tan((90 + lat) * Math.PI / 360)) / (Math.PI / 180) * MultiplyConstant / 180;
		}

		public static double ParseLongtitudeToGisFormat(double lon)
		{
			return lon * MultiplyConstant / 180;
		}
	}
}
