using NUnit.Framework;

namespace com.esri.core.geometry
{
	public class TestGeneralize : NUnit.Framework.TestFixtureAttribute
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
		public static void Test1()
		{
			com.esri.core.geometry.OperatorFactoryLocal engine = com.esri.core.geometry.OperatorFactoryLocal.GetInstance();
			com.esri.core.geometry.OperatorGeneralize op = (com.esri.core.geometry.OperatorGeneralize)engine.GetOperator(com.esri.core.geometry.Operator.Type.Generalize);
			com.esri.core.geometry.Polygon poly = new com.esri.core.geometry.Polygon();
			poly.StartPath(0, 0);
			poly.LineTo(1, 1);
			poly.LineTo(2, 0);
			poly.LineTo(3, 2);
			poly.LineTo(4, 1);
			poly.LineTo(5, 0);
			poly.LineTo(5, 10);
			poly.LineTo(0, 10);
			com.esri.core.geometry.Geometry geom = op.Execute(poly, 2, true, null);
			com.esri.core.geometry.Polygon p = (com.esri.core.geometry.Polygon)geom;
			com.esri.core.geometry.Point2D[] points = p.GetCoordinates2D();
			NUnit.Framework.Assert.IsTrue(points.Length == 4);
			NUnit.Framework.Assert.IsTrue(points[0].x == 0 && points[0].y == 0);
			NUnit.Framework.Assert.IsTrue(points[1].x == 5 && points[1].y == 0);
			NUnit.Framework.Assert.IsTrue(points[2].x == 5 && points[2].y == 10);
			NUnit.Framework.Assert.IsTrue(points[3].x == 0 && points[3].y == 10);
			com.esri.core.geometry.Geometry geom1 = op.Execute(geom, 5, false, null);
			p = (com.esri.core.geometry.Polygon)geom1;
			points = p.GetCoordinates2D();
			NUnit.Framework.Assert.IsTrue(points.Length == 3);
			NUnit.Framework.Assert.IsTrue(points[0].x == 0 && points[0].y == 0);
			NUnit.Framework.Assert.IsTrue(points[1].x == 5 && points[1].y == 10);
			NUnit.Framework.Assert.IsTrue(points[2].x == 5 && points[2].y == 10);
			geom1 = op.Execute(geom, 5, true, null);
			p = (com.esri.core.geometry.Polygon)geom1;
			points = p.GetCoordinates2D();
			NUnit.Framework.Assert.IsTrue(points.Length == 0);
		}

		[NUnit.Framework.Test]
		public static void Test2()
		{
			com.esri.core.geometry.OperatorFactoryLocal engine = com.esri.core.geometry.OperatorFactoryLocal.GetInstance();
			com.esri.core.geometry.OperatorGeneralize op = (com.esri.core.geometry.OperatorGeneralize)engine.GetOperator(com.esri.core.geometry.Operator.Type.Generalize);
			com.esri.core.geometry.Polyline polyline = new com.esri.core.geometry.Polyline();
			polyline.StartPath(0, 0);
			polyline.LineTo(1, 1);
			polyline.LineTo(2, 0);
			polyline.LineTo(3, 2);
			polyline.LineTo(4, 1);
			polyline.LineTo(5, 0);
			polyline.LineTo(5, 10);
			polyline.LineTo(0, 10);
			com.esri.core.geometry.Geometry geom = op.Execute(polyline, 2, true, null);
			com.esri.core.geometry.Polyline p = (com.esri.core.geometry.Polyline)geom;
			com.esri.core.geometry.Point2D[] points = p.GetCoordinates2D();
			NUnit.Framework.Assert.IsTrue(points.Length == 4);
			NUnit.Framework.Assert.IsTrue(points[0].x == 0 && points[0].y == 0);
			NUnit.Framework.Assert.IsTrue(points[1].x == 5 && points[1].y == 0);
			NUnit.Framework.Assert.IsTrue(points[2].x == 5 && points[2].y == 10);
			NUnit.Framework.Assert.IsTrue(points[3].x == 0 && points[3].y == 10);
			com.esri.core.geometry.Geometry geom1 = op.Execute(geom, 5, false, null);
			p = (com.esri.core.geometry.Polyline)geom1;
			points = p.GetCoordinates2D();
			NUnit.Framework.Assert.IsTrue(points.Length == 2);
			NUnit.Framework.Assert.IsTrue(points[0].x == 0 && points[0].y == 0);
			NUnit.Framework.Assert.IsTrue(points[1].x == 0 && points[1].y == 10);
			geom1 = op.Execute(geom, 5, true, null);
			p = (com.esri.core.geometry.Polyline)geom1;
			points = p.GetCoordinates2D();
			NUnit.Framework.Assert.IsTrue(points.Length == 2);
			NUnit.Framework.Assert.IsTrue(points[0].x == 0 && points[0].y == 0);
			NUnit.Framework.Assert.IsTrue(points[1].x == 0 && points[1].y == 10);
		}
	}
}
