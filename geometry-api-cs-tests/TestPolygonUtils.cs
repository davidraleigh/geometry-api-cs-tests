using NUnit.Framework;

namespace com.esri.core.geometry
{
	public class TestPolygonUtils : NUnit.Framework.TestFixtureAttribute
	{
		/// <exception cref="System.Exception"/>
		[SetUp]
        protected void SetUp()
		{
			
		}

		/// <exception cref="System.Exception"/>
		protected void TearDown()
		{
			
		}

		[NUnit.Framework.Test]
		public static void TestPointInAnyOuterRing()
		{
			com.esri.core.geometry.Polygon polygon = new com.esri.core.geometry.Polygon();
			// outer ring1
			polygon.StartPath(-200, -100);
			polygon.LineTo(200, -100);
			polygon.LineTo(200, 100);
			polygon.LineTo(-190, 100);
			polygon.LineTo(-190, 90);
			polygon.LineTo(-200, 90);
			// hole
			polygon.StartPath(-100, 50);
			polygon.LineTo(100, 50);
			polygon.LineTo(100, -40);
			polygon.LineTo(90, -40);
			polygon.LineTo(90, -50);
			polygon.LineTo(-100, -50);
			// island
			polygon.StartPath(-10, -10);
			polygon.LineTo(10, -10);
			polygon.LineTo(10, 10);
			polygon.LineTo(-10, 10);
			// outer ring2
			polygon.StartPath(300, 300);
			polygon.LineTo(310, 300);
			polygon.LineTo(310, 310);
			polygon.LineTo(300, 310);
			polygon.ReverseAllPaths();
			com.esri.core.geometry.Point2D testPointIn1 = new com.esri.core.geometry.Point2D(1, 2);
			// inside the island
			com.esri.core.geometry.Point2D testPointIn2 = new com.esri.core.geometry.Point2D(190, 90);
			// inside, betwen outer
			// ring1 and the hole
			com.esri.core.geometry.Point2D testPointIn3 = new com.esri.core.geometry.Point2D(305, 305);
			// inside the outer ring2
			com.esri.core.geometry.Point2D testPointOut1 = new com.esri.core.geometry.Point2D(300, 2);
			// outside any
			com.esri.core.geometry.Point2D testPointOut2 = new com.esri.core.geometry.Point2D(-195, 95);
			// outside any (in the
			// concave area of outer
			// ring 2)
			com.esri.core.geometry.Point2D testPointOut3 = new com.esri.core.geometry.Point2D(99, 49);
			// outside (in the hole)
			com.esri.core.geometry.PolygonUtils.PiPResult res;
			// is_point_in_polygon_2D
			res = com.esri.core.geometry.PolygonUtils.IsPointInPolygon2D(polygon, testPointIn1, 0);
			NUnit.Framework.Assert.IsTrue(res == com.esri.core.geometry.PolygonUtils.PiPResult.PiPInside);
			res = com.esri.core.geometry.PolygonUtils.IsPointInPolygon2D(polygon, testPointIn2, 0);
			NUnit.Framework.Assert.IsTrue(res == com.esri.core.geometry.PolygonUtils.PiPResult.PiPInside);
			res = com.esri.core.geometry.PolygonUtils.IsPointInPolygon2D(polygon, testPointIn3, 0);
			NUnit.Framework.Assert.IsTrue(res == com.esri.core.geometry.PolygonUtils.PiPResult.PiPInside);
			res = com.esri.core.geometry.PolygonUtils.IsPointInPolygon2D(polygon, testPointOut1, 0);
			NUnit.Framework.Assert.IsTrue(res == com.esri.core.geometry.PolygonUtils.PiPResult.PiPOutside);
			res = com.esri.core.geometry.PolygonUtils.IsPointInPolygon2D(polygon, testPointOut2, 0);
			NUnit.Framework.Assert.IsTrue(res == com.esri.core.geometry.PolygonUtils.PiPResult.PiPOutside);
			res = com.esri.core.geometry.PolygonUtils.IsPointInPolygon2D(polygon, testPointOut3, 0);
			NUnit.Framework.Assert.IsTrue(res == com.esri.core.geometry.PolygonUtils.PiPResult.PiPOutside);
			// Ispoint_in_any_outer_ring
			res = com.esri.core.geometry.PolygonUtils.IsPointInAnyOuterRing(polygon, testPointIn1, 0);
			NUnit.Framework.Assert.IsTrue(res == com.esri.core.geometry.PolygonUtils.PiPResult.PiPInside);
			res = com.esri.core.geometry.PolygonUtils.IsPointInAnyOuterRing(polygon, testPointIn2, 0);
			NUnit.Framework.Assert.IsTrue(res == com.esri.core.geometry.PolygonUtils.PiPResult.PiPInside);
			res = com.esri.core.geometry.PolygonUtils.IsPointInAnyOuterRing(polygon, testPointIn3, 0);
			NUnit.Framework.Assert.IsTrue(res == com.esri.core.geometry.PolygonUtils.PiPResult.PiPInside);
			res = com.esri.core.geometry.PolygonUtils.IsPointInAnyOuterRing(polygon, testPointOut1, 0);
			NUnit.Framework.Assert.IsTrue(res == com.esri.core.geometry.PolygonUtils.PiPResult.PiPOutside);
			res = com.esri.core.geometry.PolygonUtils.IsPointInAnyOuterRing(polygon, testPointOut2, 0);
			NUnit.Framework.Assert.IsTrue(res == com.esri.core.geometry.PolygonUtils.PiPResult.PiPOutside);
			res = com.esri.core.geometry.PolygonUtils.IsPointInAnyOuterRing(polygon, testPointOut3, 0);
			NUnit.Framework.Assert.IsTrue(res == com.esri.core.geometry.PolygonUtils.PiPResult.PiPInside);
		}

		// inside of outer
		// ring
		[NUnit.Framework.Test]
		public static void TestPointInPolygonBugCR181840()
		{
			com.esri.core.geometry.PolygonUtils.PiPResult res;
			{
				// pointInPolygonBugCR181840 - point in polygon bug
				com.esri.core.geometry.Polygon polygon = new com.esri.core.geometry.Polygon();
				// outer ring1
				polygon.StartPath(0, 0);
				polygon.LineTo(10, 10);
				polygon.LineTo(20, 0);
				res = com.esri.core.geometry.PolygonUtils.IsPointInPolygon2D(polygon, com.esri.core.geometry.Point2D.Construct(15, 10), 0);
				NUnit.Framework.Assert.IsTrue(res == com.esri.core.geometry.PolygonUtils.PiPResult.PiPOutside);
				res = com.esri.core.geometry.PolygonUtils.IsPointInPolygon2D(polygon, com.esri.core.geometry.Point2D.Construct(2, 10), 0);
				NUnit.Framework.Assert.IsTrue(res == com.esri.core.geometry.PolygonUtils.PiPResult.PiPOutside);
				res = com.esri.core.geometry.PolygonUtils.IsPointInPolygon2D(polygon, com.esri.core.geometry.Point2D.Construct(5, 5), 0);
				NUnit.Framework.Assert.IsTrue(res == com.esri.core.geometry.PolygonUtils.PiPResult.PiPInside);
			}
			{
				// CR181840 - point in polygon bug
				com.esri.core.geometry.Polygon polygon = new com.esri.core.geometry.Polygon();
				// outer ring1
				polygon.StartPath(10, 10);
				polygon.LineTo(20, 0);
				polygon.LineTo(0, 0);
				res = com.esri.core.geometry.PolygonUtils.IsPointInPolygon2D(polygon, com.esri.core.geometry.Point2D.Construct(15, 10), 0);
				NUnit.Framework.Assert.IsTrue(res == com.esri.core.geometry.PolygonUtils.PiPResult.PiPOutside);
				res = com.esri.core.geometry.PolygonUtils.IsPointInPolygon2D(polygon, com.esri.core.geometry.Point2D.Construct(2, 10), 0);
				NUnit.Framework.Assert.IsTrue(res == com.esri.core.geometry.PolygonUtils.PiPResult.PiPOutside);
				res = com.esri.core.geometry.PolygonUtils.IsPointInPolygon2D(polygon, com.esri.core.geometry.Point2D.Construct(5, 5), 0);
				NUnit.Framework.Assert.IsTrue(res == com.esri.core.geometry.PolygonUtils.PiPResult.PiPInside);
			}
		}
	}
}
