using NUnit.Framework;

namespace com.esri.core.geometry
{
	public class TestProximity2D : NUnit.Framework.TestFixtureAttribute
	{
		/// <exception cref="System.Exception"/>
		protected override void SetUp()
		{
			
		}

		/// <exception cref="System.Exception"/>
		protected override void TearDown()
		{
			
		}

		[NUnit.Framework.Test]
		public virtual void TestProximity_2D_1()
		{
			com.esri.core.geometry.OperatorFactoryLocal engine = com.esri.core.geometry.OperatorFactoryLocal.GetInstance();
			com.esri.core.geometry.OperatorProximity2D proximityOp = (com.esri.core.geometry.OperatorProximity2D)engine.GetOperator(com.esri.core.geometry.Operator.Type.Proximity2D);
			com.esri.core.geometry.Point inputPoint = new com.esri.core.geometry.Point(3, 2);
			com.esri.core.geometry.Point point0 = new com.esri.core.geometry.Point(2.75, 2);
			// Point point1 = new Point(3, 2.5);
			// Point point2 = new Point(3.75, 2);
			// Point point3 = new Point(2.25, 2.5);
			// Point point4 = new Point(4, 2.25);
			// GetNearestVertices for Polygon (Native and DotNet)
			com.esri.core.geometry.Polygon polygon = MakePolygon();
			com.esri.core.geometry.Proximity2DResult[] resultArray = com.esri.core.geometry.GeometryEngine.GetNearestVertices(polygon, inputPoint, 2.0, 8);
			NUnit.Framework.Assert.IsTrue(resultArray.Length == 8);
			double lastdistance;
			double distance;
			com.esri.core.geometry.Proximity2DResult result0 = resultArray[0];
			lastdistance = result0.GetDistance();
			NUnit.Framework.Assert.IsTrue(lastdistance <= 2.0);
			com.esri.core.geometry.Proximity2DResult result1 = resultArray[1];
			distance = result1.GetDistance();
			NUnit.Framework.Assert.IsTrue(distance <= 2.0 && distance >= lastdistance);
			lastdistance = distance;
			com.esri.core.geometry.Proximity2DResult result2 = resultArray[2];
			distance = result2.GetDistance();
			NUnit.Framework.Assert.IsTrue(distance <= 2.0 && distance >= lastdistance);
			lastdistance = distance;
			com.esri.core.geometry.Proximity2DResult result3 = resultArray[3];
			distance = result3.GetDistance();
			NUnit.Framework.Assert.IsTrue(distance <= 2.0 && distance >= lastdistance);
			lastdistance = distance;
			com.esri.core.geometry.Proximity2DResult result4 = resultArray[4];
			distance = result4.GetDistance();
			NUnit.Framework.Assert.IsTrue(distance <= 2.0 && distance >= lastdistance);
			lastdistance = distance;
			com.esri.core.geometry.Proximity2DResult result5 = resultArray[5];
			distance = result5.GetDistance();
			NUnit.Framework.Assert.IsTrue(distance <= 2.0 && distance >= lastdistance);
			lastdistance = distance;
			com.esri.core.geometry.Proximity2DResult result6 = resultArray[6];
			distance = result6.GetDistance();
			NUnit.Framework.Assert.IsTrue(distance <= 2.0 && distance >= lastdistance);
			lastdistance = distance;
			com.esri.core.geometry.Proximity2DResult result7 = resultArray[7];
			distance = result7.GetDistance();
			NUnit.Framework.Assert.IsTrue(distance <= 2.0 && distance >= lastdistance);
			// lastdistance = distance;
			// Point[] coordinates = polygon.get.getCoordinates2D();
			// int pointCount = polygon.getPointCount();
			//
			// int hits = 0;
			// for (int i = 0; i < pointCount; i++)
			// {
			// Point ipoint = coordinates[i];
			// distance = Point::Distance(ipoint, inputPoint);
			//
			// if (distance < lastdistance)
			// hits++;
			// }
			// assertTrue(hits < 8);
			// GetNearestVertices for Point
			com.esri.core.geometry.Point point = MakePoint();
			resultArray = com.esri.core.geometry.GeometryEngine.GetNearestVertices(point, inputPoint, 1.0, 1);
			NUnit.Framework.Assert.IsTrue(resultArray.Length == 1);
			result0 = resultArray[0];
			com.esri.core.geometry.Point resultPoint0 = result0.GetCoordinate();
			NUnit.Framework.Assert.IsTrue(resultPoint0.GetX() == point.GetX() && resultPoint0.GetY() == point.GetY());
			// GetNearestVertex for Polygon
			result0 = com.esri.core.geometry.GeometryEngine.GetNearestVertex(polygon, inputPoint);
			resultPoint0 = result0.GetCoordinate();
			NUnit.Framework.Assert.IsTrue(resultPoint0.GetX() == point0.GetX() && resultPoint0.GetY() == point0.GetY());
			// GetNearestVertex for Point
			result0 = com.esri.core.geometry.GeometryEngine.GetNearestVertex(point, inputPoint);
			resultPoint0 = result0.GetCoordinate();
			NUnit.Framework.Assert.IsTrue(resultPoint0.GetX() == point.GetX() && resultPoint0.GetY() == point.GetY());
			// GetNearestCoordinate for Polygon
			com.esri.core.geometry.Polygon polygon2 = MakePolygon2();
			result0 = com.esri.core.geometry.GeometryEngine.GetNearestCoordinate(polygon2, inputPoint, true);
			resultPoint0 = result0.GetCoordinate();
			NUnit.Framework.Assert.IsTrue(resultPoint0.GetX() == inputPoint.GetX() && resultPoint0.GetY() == inputPoint.GetY());
			// GetNearestCoordinate for Polyline
			com.esri.core.geometry.Polyline polyline = MakePolyline();
			result0 = com.esri.core.geometry.GeometryEngine.GetNearestCoordinate(polyline, inputPoint, true);
			resultPoint0 = result0.GetCoordinate();
			NUnit.Framework.Assert.IsTrue(resultPoint0.GetX() == 0.0 && resultPoint0.GetY() == 2.0);
			com.esri.core.geometry.Polygon pp = new com.esri.core.geometry.Polygon();
			pp.StartPath(0, 0);
			pp.LineTo(0, 10);
			pp.LineTo(10, 10);
			pp.LineTo(10, 0);
			inputPoint.SetXY(15, -5);
			result0 = proximityOp.GetNearestCoordinate(pp, inputPoint, true, true);
			bool is_right = result0.IsRightSide();
			NUnit.Framework.Assert.IsTrue(!is_right);
		}

		internal virtual com.esri.core.geometry.Polygon MakePolygon()
		{
			com.esri.core.geometry.Polygon poly = new com.esri.core.geometry.Polygon();
			poly.StartPath(3, -2);
			poly.LineTo(2, -1);
			poly.LineTo(3, 0);
			poly.LineTo(4, 0);
			poly.StartPath(1.75, 1);
			poly.LineTo(0.75, 2);
			poly.LineTo(1.75, 3);
			poly.LineTo(2.25, 2.5);
			poly.LineTo(2.75, 2);
			poly.StartPath(3, 2.5);
			poly.LineTo(2.5, 3);
			poly.LineTo(2, 3.5);
			poly.LineTo(3, 4.5);
			poly.LineTo(4, 3.5);
			poly.StartPath(4.75, 1);
			poly.LineTo(3.75, 2);
			poly.LineTo(4, 2.25);
			poly.LineTo(4.75, 3);
			poly.LineTo(5.75, 2);
			return poly;
		}

		internal virtual com.esri.core.geometry.Polygon MakePolygon2()
		{
			com.esri.core.geometry.Polygon poly = new com.esri.core.geometry.Polygon();
			poly.StartPath(0, 0);
			poly.LineTo(0, 10);
			poly.LineTo(10, 10);
			poly.LineTo(10, 0);
			return poly;
		}

		internal virtual com.esri.core.geometry.Polyline MakePolyline()
		{
			com.esri.core.geometry.Polyline poly = new com.esri.core.geometry.Polyline();
			poly.StartPath(0, 0);
			poly.LineTo(0, 10);
			poly.LineTo(10, 10);
			poly.LineTo(10, 0);
			return poly;
		}

		internal virtual com.esri.core.geometry.Point MakePoint()
		{
			com.esri.core.geometry.Point point = new com.esri.core.geometry.Point(3, 2.5);
			return point;
		}

		[NUnit.Framework.Test]
		public virtual void TestProximity2D_2()
		{
			com.esri.core.geometry.Point point1 = new com.esri.core.geometry.Point(3, 2);
			com.esri.core.geometry.Point point2 = new com.esri.core.geometry.Point(2, 4);
			com.esri.core.geometry.Envelope envelope = new com.esri.core.geometry.Envelope();
			envelope.SetCoords(4, 3, 7, 6);
			com.esri.core.geometry.Polygon polygonToTest = new com.esri.core.geometry.Polygon();
			polygonToTest.AddEnvelope(envelope, false);
			com.esri.core.geometry.Proximity2DResult prxResult1 = com.esri.core.geometry.GeometryEngine.GetNearestVertex(envelope, point1);
			com.esri.core.geometry.Proximity2DResult prxResult2 = com.esri.core.geometry.GeometryEngine.GetNearestVertex(polygonToTest, point1);
			com.esri.core.geometry.Proximity2DResult prxResult3 = com.esri.core.geometry.GeometryEngine.GetNearestCoordinate(envelope, point2, false);
			com.esri.core.geometry.Proximity2DResult prxResult4 = com.esri.core.geometry.GeometryEngine.GetNearestCoordinate(polygonToTest, point2, false);
			com.esri.core.geometry.Point result1 = prxResult1.GetCoordinate();
			com.esri.core.geometry.Point result2 = prxResult2.GetCoordinate();
			NUnit.Framework.Assert.IsTrue(result1.GetX() == result2.GetX());
			com.esri.core.geometry.Point result3 = prxResult3.GetCoordinate();
			com.esri.core.geometry.Point result4 = prxResult4.GetCoordinate();
			NUnit.Framework.Assert.IsTrue(result3.GetX() == result4.GetX());
		}

		[NUnit.Framework.Test]
		public static void TestProximity2D_3()
		{
			com.esri.core.geometry.OperatorFactoryLocal factory = com.esri.core.geometry.OperatorFactoryLocal.GetInstance();
			com.esri.core.geometry.OperatorProximity2D proximity = (com.esri.core.geometry.OperatorProximity2D)factory.GetOperator(com.esri.core.geometry.Operator.Type.Proximity2D);
			com.esri.core.geometry.Polygon polygon = new com.esri.core.geometry.Polygon();
			polygon.StartPath(new com.esri.core.geometry.Point(-120, 22));
			polygon.LineTo(new com.esri.core.geometry.Point(-120, 10));
			polygon.LineTo(new com.esri.core.geometry.Point(-110, 10));
			polygon.LineTo(new com.esri.core.geometry.Point(-110, 22));
			com.esri.core.geometry.Point point = new com.esri.core.geometry.Point();
			point.SetXY(-110, 20);
			com.esri.core.geometry.Proximity2DResult result = proximity.GetNearestCoordinate(polygon, point, false);
			com.esri.core.geometry.Point point2 = new com.esri.core.geometry.Point();
			point2.SetXY(-120, 12);
			com.esri.core.geometry.Proximity2DResult[] results = proximity.GetNearestVertices(polygon, point2, 10, 12);
		}

		[NUnit.Framework.Test]
		public static void TestCR254240()
		{
			com.esri.core.geometry.OperatorProximity2D proximityOp = com.esri.core.geometry.OperatorProximity2D.Local();
			com.esri.core.geometry.Point inputPoint = new com.esri.core.geometry.Point(-12, 12);
			com.esri.core.geometry.Polyline line = new com.esri.core.geometry.Polyline();
			line.StartPath(-10, 0);
			line.LineTo(0, 0);
			com.esri.core.geometry.Proximity2DResult result = proximityOp.GetNearestCoordinate(line, inputPoint, false, true);
			NUnit.Framework.Assert.IsTrue(result.IsRightSide() == false);
		}
	}
}
