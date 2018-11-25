using System;

namespace PDT.Utils
{
	public static class TransformationUtils
	{
		public const double MagicNumber = 20037508.34;
		public static double GetLattitude(double y)
		{
			return (Math.Atan(Math.Pow(Math.E, Math.PI / 180 * (y / (MagicNumber / 180)))) / (Math.PI / 360)) - 90;
		}

		public static double GetLongtitude(double x)
		{
			return x * 180 / MagicNumber;
		}

		public static double ParseLattitudeToGisFormat(double lat)
		{
			return Math.Log(Math.Tan((90 + lat) * Math.PI / 360)) / (Math.PI / 180) * 20037508.34 / 180;
		}

		public static double ParseLongtitudeToGisFormat(double lon)
		{
			return lon * 20037508.34 / 180;
		}
	}
}
