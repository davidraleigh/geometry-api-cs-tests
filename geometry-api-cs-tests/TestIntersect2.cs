using NUnit.Framework;

namespace com.esri.core.geometry
{
	public class TestIntersect2 : NUnit.Framework.TestFixtureAttribute
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
		public virtual void TestIntersectBetweenPolylineAndPolygon()
		{
			com.esri.core.geometry.Polyline basePl = new com.esri.core.geometry.Polyline();
			basePl.StartPath(new com.esri.core.geometry.Point(-117, 20));
			basePl.LineTo(new com.esri.core.geometry.Point(-117, 10));
			basePl.LineTo(new com.esri.core.geometry.Point(-130, 10));
			basePl.LineTo(new com.esri.core.geometry.Point(-130, 20));
			basePl.LineTo(new com.esri.core.geometry.Point(-117, 20));
			com.esri.core.geometry.Polygon compPl = new com.esri.core.geometry.Polygon();
			compPl.StartPath(-116, 20);
			compPl.LineTo(-131, 10);
			compPl.LineTo(-121, 50);
			com.esri.core.geometry.Geometry intersectGeom = null;
			int noException = 1;
			// no exception
			try
			{
				intersectGeom = com.esri.core.geometry.GeometryEngine.Intersect(basePl, compPl, com.esri.core.geometry.SpatialReference.Create(4326));
			}
			catch (System.Exception)
			{
				noException = 0;
			}
			NUnit.Framework.Assert.IsNotNull(intersectGeom);
		}

		// Geometry[] geometries = new Geometry[1];
		// geometries[0] = basePl;
		// BorgGeometryUtils.getIntersectFromRestWS(geometries, compPl, 4326);
		[NUnit.Framework.Test]
		public virtual void TestIntersectBetweenPolylines()
		{
			com.esri.core.geometry.Polyline basePl = new com.esri.core.geometry.Polyline();
			basePl.StartPath(new com.esri.core.geometry.Point(-117, 20));
			basePl.LineTo(new com.esri.core.geometry.Point(-130, 10));
			basePl.LineTo(new com.esri.core.geometry.Point(-120, 50));
			com.esri.core.geometry.Polyline compPl = new com.esri.core.geometry.Polyline();
			compPl.StartPath(new com.esri.core.geometry.Point(-116, 20));
			compPl.LineTo(new com.esri.core.geometry.Point(-131, 10));
			compPl.LineTo(new com.esri.core.geometry.Point(-121, 50));
			int noException = 1;
			// no exception
			try
			{
				com.esri.core.geometry.Geometry intersectGeom = com.esri.core.geometry.GeometryEngine.Intersect(basePl, compPl, com.esri.core.geometry.SpatialReference.Create(4326));
			}
			catch (System.Exception)
			{
				noException = 0;
			}
			NUnit.Framework.Assert.AreEqual(noException, 1);
		}

		[NUnit.Framework.Test]
		public virtual void TestPointAndPolyline1()
		{
			com.esri.core.geometry.Point basePl = new com.esri.core.geometry.Point(-116, 20);
			com.esri.core.geometry.Polyline compPl = new com.esri.core.geometry.Polyline();
			compPl.StartPath(new com.esri.core.geometry.Point(-116, 20));
			compPl.LineTo(new com.esri.core.geometry.Point(-131, 10));
			compPl.LineTo(new com.esri.core.geometry.Point(-121, 50));
			int noException = 1;
			// no exception
			com.esri.core.geometry.Geometry intersectGeom = null;
			try
			{
				intersectGeom = com.esri.core.geometry.GeometryEngine.Intersect(basePl, compPl, com.esri.core.geometry.SpatialReference.Create(4326));
			}
			catch (System.Exception)
			{
				noException = 0;
			}
			NUnit.Framework.Assert.AreEqual(noException, 1);
			NUnit.Framework.Assert.IsNotNull(intersectGeom);
			NUnit.Framework.Assert.IsTrue(intersectGeom.GetType() == com.esri.core.geometry.Geometry.Type.Point);
			com.esri.core.geometry.Point ip = (com.esri.core.geometry.Point)intersectGeom;
			NUnit.Framework.Assert.AreEqual(ip.GetX(), -116, 0.1E7);
			NUnit.Framework.Assert.AreEqual(ip.GetY(), 20, 0.1E7);
		}

		[NUnit.Framework.Test]
		public virtual void TestPointAndPolyline2()
		{
			com.esri.core.geometry.Point basePl = new com.esri.core.geometry.Point(-115, 20);
			com.esri.core.geometry.Polyline compPl = new com.esri.core.geometry.Polyline();
			compPl.StartPath(new com.esri.core.geometry.Point(-116, 20));
			compPl.LineTo(new com.esri.core.geometry.Point(-131, 10));
			compPl.LineTo(new com.esri.core.geometry.Point(-121, 50));
			int noException = 1;
			// no exception
			com.esri.core.geometry.Geometry intersectGeom = null;
			try
			{
				intersectGeom = com.esri.core.geometry.GeometryEngine.Intersect(basePl, compPl, com.esri.core.geometry.SpatialReference.Create(4326));
			}
			catch (System.Exception)
			{
				noException = 0;
			}
			NUnit.Framework.Assert.AreEqual(noException, 1);
			NUnit.Framework.Assert.IsTrue(intersectGeom.IsEmpty());
		}

		[NUnit.Framework.Test]
		public virtual void TestPointAndPolygon1()
		{
			com.esri.core.geometry.Point basePl = new com.esri.core.geometry.Point(-116, 20);
			com.esri.core.geometry.Polygon compPl = new com.esri.core.geometry.Polygon();
			compPl.StartPath(new com.esri.core.geometry.Point(-116, 20));
			compPl.LineTo(new com.esri.core.geometry.Point(-131, 10));
			compPl.LineTo(new com.esri.core.geometry.Point(-121, 50));
			int noException = 1;
			// no exception
			com.esri.core.geometry.Geometry intersectGeom = null;
			try
			{
				intersectGeom = com.esri.core.geometry.GeometryEngine.Intersect(basePl, compPl, com.esri.core.geometry.SpatialReference.Create(4326));
			}
			catch (System.Exception)
			{
				noException = 0;
			}
			NUnit.Framework.Assert.AreEqual(noException, 1);
			NUnit.Framework.Assert.IsNotNull(intersectGeom);
			NUnit.Framework.Assert.IsTrue(intersectGeom.GetType() == com.esri.core.geometry.Geometry.Type.Point);
			com.esri.core.geometry.Point ip = (com.esri.core.geometry.Point)intersectGeom;
			NUnit.Framework.Assert.AreEqual(ip.GetX(), -116, 0.1E7);
			NUnit.Framework.Assert.AreEqual(ip.GetY(), 20, 0.1E7);
		}

		[NUnit.Framework.Test]
		public virtual void TestPointAndPolygon2()
		{
			com.esri.core.geometry.Point basePl = new com.esri.core.geometry.Point(-115, 20);
			com.esri.core.geometry.Polygon compPl = new com.esri.core.geometry.Polygon();
			compPl.StartPath(new com.esri.core.geometry.Point(-116, 20));
			compPl.LineTo(new com.esri.core.geometry.Point(-131, 10));
			compPl.LineTo(new com.esri.core.geometry.Point(-121, 50));
			int noException = 1;
			// no exception
			com.esri.core.geometry.Geometry intersectGeom = null;
			try
			{
				intersectGeom = com.esri.core.geometry.GeometryEngine.Intersect(basePl, compPl, com.esri.core.geometry.SpatialReference.Create(4326));
			}
			catch (System.Exception)
			{
				noException = 0;
			}
			NUnit.Framework.Assert.AreEqual(noException, 1);
			NUnit.Framework.Assert.IsTrue(intersectGeom.IsEmpty());
		}

		[NUnit.Framework.Test]
		public virtual void TestPointAndPolygon3()
		{
			com.esri.core.geometry.Point basePl = new com.esri.core.geometry.Point(-121, 20);
			com.esri.core.geometry.Polygon compPl = new com.esri.core.geometry.Polygon();
			compPl.StartPath(new com.esri.core.geometry.Point(-116, 20));
			compPl.LineTo(new com.esri.core.geometry.Point(-131, 10));
			compPl.LineTo(new com.esri.core.geometry.Point(-121, 50));
			int noException = 1;
			// no exception
			com.esri.core.geometry.Geometry intersectGeom = null;
			try
			{
				intersectGeom = com.esri.core.geometry.GeometryEngine.Intersect(basePl, compPl, com.esri.core.geometry.SpatialReference.Create(4326));
			}
			catch (System.Exception)
			{
				noException = 0;
			}
			NUnit.Framework.Assert.AreEqual(noException, 1);
			NUnit.Framework.Assert.IsNotNull(intersectGeom);
			NUnit.Framework.Assert.IsTrue(intersectGeom.GetType() == com.esri.core.geometry.Geometry.Type.Point);
			com.esri.core.geometry.Point ip = (com.esri.core.geometry.Point)intersectGeom;
			NUnit.Framework.Assert.AreEqual(ip.GetX(), -121, 0.1E7);
			NUnit.Framework.Assert.AreEqual(ip.GetY(), 20, 0.1E7);
		}

		[NUnit.Framework.Test]
		public virtual void TestPointAndPoint1()
		{
			com.esri.core.geometry.Point basePl = new com.esri.core.geometry.Point(-116, 20);
			com.esri.core.geometry.Point compPl = new com.esri.core.geometry.Point(-116, 20);
			int noException = 1;
			// no exception
			com.esri.core.geometry.Geometry intersectGeom = null;
			try
			{
				intersectGeom = com.esri.core.geometry.GeometryEngine.Intersect(basePl, compPl, com.esri.core.geometry.SpatialReference.Create(4326));
			}
			catch (System.Exception)
			{
				noException = 0;
			}
			NUnit.Framework.Assert.AreEqual(noException, 1);
			NUnit.Framework.Assert.IsNotNull(intersectGeom);
			NUnit.Framework.Assert.IsTrue(intersectGeom.GetType() == com.esri.core.geometry.Geometry.Type.Point);
			com.esri.core.geometry.Point ip = (com.esri.core.geometry.Point)intersectGeom;
			NUnit.Framework.Assert.AreEqual(ip.GetX(), -116, 0.1E7);
			NUnit.Framework.Assert.AreEqual(ip.GetY(), 20, 0.1E7);
		}

		[NUnit.Framework.Test]
		public virtual void TestPointAndPoint2()
		{
			com.esri.core.geometry.Point basePl = new com.esri.core.geometry.Point(-115, 20);
			com.esri.core.geometry.Point compPl = new com.esri.core.geometry.Point(-116, 20);
			int noException = 1;
			// no exception
			com.esri.core.geometry.Geometry intersectGeom = null;
			try
			{
				intersectGeom = com.esri.core.geometry.GeometryEngine.Intersect(basePl, compPl, com.esri.core.geometry.SpatialReference.Create(4326));
			}
			catch (System.Exception)
			{
				noException = 0;
			}
			NUnit.Framework.Assert.AreEqual(noException, 1);
			NUnit.Framework.Assert.IsTrue(intersectGeom.IsEmpty());
		}

		[NUnit.Framework.Test]
		public virtual void TestPointAndMultiPoint1()
		{
			com.esri.core.geometry.Point basePl = new com.esri.core.geometry.Point(-116, 20);
			com.esri.core.geometry.MultiPoint compPl = new com.esri.core.geometry.MultiPoint();
			compPl.Add(new com.esri.core.geometry.Point(-116, 20));
			compPl.Add(new com.esri.core.geometry.Point(-118, 21));
			int noException = 1;
			// no exception
			com.esri.core.geometry.Geometry intersectGeom = null;
			try
			{
				intersectGeom = com.esri.core.geometry.GeometryEngine.Intersect(basePl, compPl, com.esri.core.geometry.SpatialReference.Create(4326));
			}
			catch (System.Exception)
			{
				noException = 0;
			}
			NUnit.Framework.Assert.AreEqual(noException, 1);
			NUnit.Framework.Assert.IsNotNull(intersectGeom);
			NUnit.Framework.Assert.IsTrue(intersectGeom.GetType() == com.esri.core.geometry.Geometry.Type.Point);
			com.esri.core.geometry.Point ip = (com.esri.core.geometry.Point)intersectGeom;
			NUnit.Framework.Assert.AreEqual(ip.GetX(), -116, 0.1E7);
			NUnit.Framework.Assert.AreEqual(ip.GetY(), 20, 0.1E7);
		}

		[NUnit.Framework.Test]
		public virtual void TestPointAndMultiPoint2()
		{
			com.esri.core.geometry.Point basePl = new com.esri.core.geometry.Point(-115, 20);
			com.esri.core.geometry.MultiPoint compPl = new com.esri.core.geometry.MultiPoint();
			compPl.Add(new com.esri.core.geometry.Point(-116, 20));
			compPl.Add(new com.esri.core.geometry.Point(-117, 21));
			compPl.Add(new com.esri.core.geometry.Point(-118, 20));
			compPl.Add(new com.esri.core.geometry.Point(-119, 21));
			int noException = 1;
			// no exception
			com.esri.core.geometry.Geometry intersectGeom = null;
			try
			{
				intersectGeom = com.esri.core.geometry.GeometryEngine.Intersect(basePl, compPl, com.esri.core.geometry.SpatialReference.Create(4326));
			}
			catch (System.Exception)
			{
				noException = 0;
			}
			NUnit.Framework.Assert.AreEqual(noException, 1);
			NUnit.Framework.Assert.IsTrue(intersectGeom.IsEmpty());
		}

		[NUnit.Framework.Test]
		public virtual void TestMultiPointAndMultiPoint1()
		{
			com.esri.core.geometry.MultiPoint basePl = new com.esri.core.geometry.MultiPoint();
			basePl.Add(new com.esri.core.geometry.Point(-116, 20));
			basePl.Add(new com.esri.core.geometry.Point(-117, 20));
			com.esri.core.geometry.MultiPoint compPl = new com.esri.core.geometry.MultiPoint();
			compPl.Add(new com.esri.core.geometry.Point(-116, 20));
			compPl.Add(new com.esri.core.geometry.Point(-118, 21));
			int noException = 1;
			// no exception
			com.esri.core.geometry.Geometry intersectGeom = null;
			try
			{
				intersectGeom = com.esri.core.geometry.GeometryEngine.Intersect(basePl, compPl, com.esri.core.geometry.SpatialReference.Create(4326));
			}
			catch (System.Exception)
			{
				noException = 0;
			}
			NUnit.Framework.Assert.AreEqual(noException, 1);
			NUnit.Framework.Assert.IsNotNull(intersectGeom);
			NUnit.Framework.Assert.IsTrue(intersectGeom.GetType() == com.esri.core.geometry.Geometry.Type.MultiPoint);
			com.esri.core.geometry.MultiPoint imp = (com.esri.core.geometry.MultiPoint)intersectGeom;
			NUnit.Framework.Assert.AreEqual(imp.GetCoordinates2D().Length, 1);
			NUnit.Framework.Assert.AreEqual(imp.GetCoordinates2D()[0].x, -116, 0.0);
			NUnit.Framework.Assert.AreEqual(imp.GetCoordinates2D()[0].y, 20, 0.0);
		}

		[NUnit.Framework.Test]
		public virtual void TestMultiPointAndMultiPoint2()
		{
			com.esri.core.geometry.MultiPoint basePl = new com.esri.core.geometry.MultiPoint();
			basePl.Add(new com.esri.core.geometry.Point(-116, 20));
			basePl.Add(new com.esri.core.geometry.Point(-118, 21));
			com.esri.core.geometry.MultiPoint compPl = new com.esri.core.geometry.MultiPoint();
			compPl.Add(new com.esri.core.geometry.Point(-116, 20));
			compPl.Add(new com.esri.core.geometry.Point(-118, 21));
			int noException = 1;
			// no exception
			com.esri.core.geometry.Geometry intersectGeom = null;
			try
			{
				intersectGeom = com.esri.core.geometry.GeometryEngine.Intersect(basePl, compPl, com.esri.core.geometry.SpatialReference.Create(4326));
			}
			catch (System.Exception)
			{
				noException = 0;
			}
			NUnit.Framework.Assert.AreEqual(noException, 1);
			NUnit.Framework.Assert.IsNotNull(intersectGeom);
			NUnit.Framework.Assert.IsTrue(intersectGeom.GetType() == com.esri.core.geometry.Geometry.Type.MultiPoint);
			com.esri.core.geometry.MultiPoint ip = (com.esri.core.geometry.MultiPoint)intersectGeom;
			NUnit.Framework.Assert.AreEqual(ip.GetPoint(0).GetX(), -116, 0.1E7);
			NUnit.Framework.Assert.AreEqual(ip.GetPoint(0).GetY(), 20, 0.1E7);
			NUnit.Framework.Assert.AreEqual(ip.GetPoint(0).GetX(), -118, 0.1E7);
			NUnit.Framework.Assert.AreEqual(ip.GetPoint(0).GetY(), 21, 0.1E7);
		}

		[NUnit.Framework.Test]
		public virtual void TestMultiPointAndMultiPoint3()
		{
			com.esri.core.geometry.MultiPoint basePl = new com.esri.core.geometry.MultiPoint();
			basePl.Add(new com.esri.core.geometry.Point(-116, 21));
			basePl.Add(new com.esri.core.geometry.Point(-117, 20));
			com.esri.core.geometry.MultiPoint compPl = new com.esri.core.geometry.MultiPoint();
			compPl.Add(new com.esri.core.geometry.Point(-116, 20));
			compPl.Add(new com.esri.core.geometry.Point(-117, 21));
			compPl.Add(new com.esri.core.geometry.Point(-118, 20));
			compPl.Add(new com.esri.core.geometry.Point(-119, 21));
			int noException = 1;
			// no exception
			com.esri.core.geometry.Geometry intersectGeom = null;
			try
			{
				intersectGeom = com.esri.core.geometry.GeometryEngine.Intersect(basePl, compPl, com.esri.core.geometry.SpatialReference.Create(4326));
			}
			catch (System.Exception)
			{
				noException = 0;
			}
			NUnit.Framework.Assert.AreEqual(noException, 1);
			NUnit.Framework.Assert.IsTrue(intersectGeom.IsEmpty());
		}
	}
}
