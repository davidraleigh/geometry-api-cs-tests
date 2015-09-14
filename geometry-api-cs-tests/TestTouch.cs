using NUnit.Framework;

namespace com.esri.core.geometry
{
	public class TestTouch : NUnit.Framework.TestFixtureAttribute
	{
		/// <exception cref="System.Exception"/>
		protected void SetUp()
		{
			
		}

		/// <exception cref="System.Exception"/>
		protected void TearDown()
		{
			
		}

		[NUnit.Framework.Test]
		public virtual void TestTouchOnPointAndPolyline()
		{
			com.esri.core.geometry.Geometry baseGeom = new com.esri.core.geometry.Point(-130, 10);
			com.esri.core.geometry.Polyline pl = new com.esri.core.geometry.Polyline();
			pl.StartPath(new com.esri.core.geometry.Point(-130, 10));
			pl.LineTo(-131, 15);
			pl.LineTo(-140, 20);
			com.esri.core.geometry.SpatialReference sr = com.esri.core.geometry.SpatialReference.Create(4326);
			bool isTouched;
			bool isTouched2;
			try
			{
				isTouched = com.esri.core.geometry.GeometryEngine.Touches(baseGeom, pl, sr);
				isTouched2 = com.esri.core.geometry.GeometryEngine.Touches(pl, baseGeom, sr);
			}
			catch (System.ArgumentException)
			{
				isTouched = false;
				isTouched2 = false;
			}
			NUnit.Framework.Assert.AreEqual(isTouched && isTouched2, true);
		}

		[NUnit.Framework.Test]
		public virtual void TestTouchOnPointAndPolygon()
		{
			com.esri.core.geometry.Geometry baseGeom = new com.esri.core.geometry.Point(-130, 10);
			com.esri.core.geometry.Polygon pg = new com.esri.core.geometry.Polygon();
			pg.StartPath(new com.esri.core.geometry.Point(-130, 10));
			pg.LineTo(-131, 15);
			pg.LineTo(-140, 20);
			com.esri.core.geometry.SpatialReference sr = com.esri.core.geometry.SpatialReference.Create(4326);
			bool isTouched;
			bool isTouched2;
			try
			{
				isTouched = com.esri.core.geometry.GeometryEngine.Touches(baseGeom, pg, sr);
				isTouched2 = com.esri.core.geometry.GeometryEngine.Touches(pg, baseGeom, sr);
			}
			catch (System.ArgumentException)
			{
				isTouched = false;
				isTouched2 = false;
			}
			NUnit.Framework.Assert.AreEqual(isTouched && isTouched2, true);
		}

		[NUnit.Framework.Test]
		public virtual void TestTouchOnPolygons()
		{
			com.esri.core.geometry.Polygon pg = new com.esri.core.geometry.Polygon();
			pg.StartPath(new com.esri.core.geometry.Point(-130, 10));
			pg.LineTo(-131, 15);
			pg.LineTo(-140, 20);
			com.esri.core.geometry.Polygon pg2 = new com.esri.core.geometry.Polygon();
			pg2.StartPath(new com.esri.core.geometry.Point(-130, 10));
			pg2.LineTo(-131, 15);
			pg2.LineTo(-120, 20);
			com.esri.core.geometry.SpatialReference sr = com.esri.core.geometry.SpatialReference.Create(4326);
			bool isTouched;
			bool isTouched2;
			try
			{
				isTouched = com.esri.core.geometry.GeometryEngine.Touches(pg, pg2, sr);
				isTouched2 = com.esri.core.geometry.GeometryEngine.Touches(pg2, pg, sr);
			}
			catch (System.ArgumentException)
			{
				isTouched = false;
				isTouched2 = false;
			}
			NUnit.Framework.Assert.AreEqual(isTouched && isTouched2, true);
		}

		// boolean isTouchedFromRest = GeometryUtils.isRelationTrue(pg2, pg, sr,
		// GeometryUtils.SpatialRelationType.esriGeometryRelationTouch, "");
		// assertTrue(isTouchedFromRest==isTouched);
		[NUnit.Framework.Test]
		public virtual void TestTouchesOnPolylines()
		{
			com.esri.core.geometry.SpatialReference sr = com.esri.core.geometry.SpatialReference.Create(4326);
			com.esri.core.geometry.Polyline basePl = new com.esri.core.geometry.Polyline();
			basePl.StartPath(new com.esri.core.geometry.Point(-117, 20));
			basePl.LineTo(new com.esri.core.geometry.Point(-100, 20));
			basePl.LineTo(new com.esri.core.geometry.Point(-100, 10));
			basePl.LineTo(new com.esri.core.geometry.Point(-117, 10));
			basePl.LineTo(new com.esri.core.geometry.Point(-117, 20));
			com.esri.core.geometry.Polyline compPl = new com.esri.core.geometry.Polyline();
			compPl.StartPath(new com.esri.core.geometry.Point(-104, 20));
			compPl.LineTo(new com.esri.core.geometry.Point(-108, 25));
			compPl.LineTo(new com.esri.core.geometry.Point(-100, 20));
			// compPl.lineTo(new Point(-100, 30));
			// compPl.lineTo(new Point(-117, 30));
			// compPl.lineTo(new Point(-117, 20));
			bool isTouched;
			try
			{
				isTouched = com.esri.core.geometry.GeometryEngine.Touches(basePl, compPl, sr);
			}
			catch (System.ArgumentException)
			{
				isTouched = false;
			}
			NUnit.Framework.Assert.AreEqual(isTouched, true);
		}

		[NUnit.Framework.Test]
		public virtual void TestTouchesOnPolylineAndPolygon()
		{
			com.esri.core.geometry.SpatialReference sr = com.esri.core.geometry.SpatialReference.Create(4326);
			com.esri.core.geometry.Polygon basePl = new com.esri.core.geometry.Polygon();
			basePl.StartPath(new com.esri.core.geometry.Point(-117, 20));
			basePl.LineTo(new com.esri.core.geometry.Point(-100, 20));
			basePl.LineTo(new com.esri.core.geometry.Point(-100, 10));
			basePl.LineTo(new com.esri.core.geometry.Point(-117, 10));
			com.esri.core.geometry.Polyline compPl = new com.esri.core.geometry.Polyline();
			compPl.StartPath(new com.esri.core.geometry.Point(-117, 20));
			compPl.LineTo(new com.esri.core.geometry.Point(-108, 25));
			compPl.LineTo(new com.esri.core.geometry.Point(-100, 20));
			compPl.LineTo(new com.esri.core.geometry.Point(-100, 30));
			bool isTouched;
			try
			{
				isTouched = com.esri.core.geometry.GeometryEngine.Touches(basePl, compPl, sr);
			}
			catch (System.ArgumentException)
			{
				isTouched = false;
			}
			NUnit.Framework.Assert.AreEqual(isTouched, true);
		}

		[NUnit.Framework.Test]
		public virtual void TestTouchOnEnvelopes()
		{
			// case1, not touched
			// Envelope env = new Envelope(new Point(-117,20), 12, 12);
			// Envelope env2 = new Envelope(-100,20,-80,30);
			// case2 touched
			com.esri.core.geometry.Envelope env = new com.esri.core.geometry.Envelope(new com.esri.core.geometry.Point(-117, 20), 12, 12);
			com.esri.core.geometry.Envelope env2 = new com.esri.core.geometry.Envelope(-117, 26, -80, 30);
			com.esri.core.geometry.SpatialReference sr = com.esri.core.geometry.SpatialReference.Create(4326);
			bool isTouched;
			try
			{
				isTouched = com.esri.core.geometry.GeometryEngine.Touches(env, env2, sr);
			}
			catch (System.ArgumentException)
			{
				isTouched = false;
			}
			NUnit.Framework.Assert.AreEqual(isTouched, true);
		}

		[NUnit.Framework.Test]
		public virtual void TestTouchesOnPolylineAndEnvelope()
		{
			com.esri.core.geometry.SpatialReference sr = com.esri.core.geometry.SpatialReference.Create(4326);
			com.esri.core.geometry.Polyline basePl = new com.esri.core.geometry.Polyline();
			basePl.StartPath(new com.esri.core.geometry.Point(-117, 20));
			basePl.LineTo(new com.esri.core.geometry.Point(-100, 20));
			basePl.LineTo(new com.esri.core.geometry.Point(-100, 10));
			basePl.LineTo(new com.esri.core.geometry.Point(-117, 10));
			basePl.LineTo(new com.esri.core.geometry.Point(-117, 20));
			// Envelope env = new Envelope(new Point(-117,20), 12, 12);//not touched
			com.esri.core.geometry.Envelope env = new com.esri.core.geometry.Envelope(-100, 20, -80, 30);
			// touched
			bool isTouched;
			try
			{
				isTouched = com.esri.core.geometry.GeometryEngine.Touches(basePl, env, sr);
			}
			catch (System.ArgumentException)
			{
				isTouched = false;
			}
			NUnit.Framework.Assert.AreEqual(isTouched, true);
		}

		[NUnit.Framework.Test]
		public virtual void TestTouchesOnPolygonAndEnvelope()
		{
			com.esri.core.geometry.SpatialReference sr = com.esri.core.geometry.SpatialReference.Create(4326);
			com.esri.core.geometry.Polygon basePl = new com.esri.core.geometry.Polygon();
			basePl.StartPath(new com.esri.core.geometry.Point(-117, 20));
			basePl.LineTo(new com.esri.core.geometry.Point(-100, 20));
			basePl.LineTo(new com.esri.core.geometry.Point(-100, 10));
			basePl.LineTo(new com.esri.core.geometry.Point(-117, 10));
			// Envelope env = new Envelope(new Point(-117,20), 12, 12);//not touched
			com.esri.core.geometry.Envelope env = new com.esri.core.geometry.Envelope(-100, 20, -80, 30);
			// touched
			bool isTouched;
			try
			{
				isTouched = com.esri.core.geometry.GeometryEngine.Touches(basePl, env, sr);
			}
			catch (System.ArgumentException)
			{
				isTouched = false;
			}
			NUnit.Framework.Assert.AreEqual(isTouched, true);
		}

		[NUnit.Framework.Test]
		public virtual void TestTouchesOnPointAndEnvelope()
		{
			com.esri.core.geometry.SpatialReference sr = com.esri.core.geometry.SpatialReference.Create(4326);
			com.esri.core.geometry.Point p = new com.esri.core.geometry.Point(-130, 10);
			// Envelope env = new Envelope(p, 12, 12);//not touched
			com.esri.core.geometry.Envelope env = new com.esri.core.geometry.Envelope(-130, 10, -110, 20);
			// touched
			bool isTouched;
			try
			{
				isTouched = com.esri.core.geometry.GeometryEngine.Touches(p, env, sr);
			}
			catch (System.ArgumentException)
			{
				isTouched = false;
			}
			NUnit.Framework.Assert.AreEqual(isTouched, true);
		}

		[NUnit.Framework.Test]
		public virtual void TestRelationTouch()
		{
			com.esri.core.geometry.SpatialReference sr = com.esri.core.geometry.SpatialReference.Create(4326);
			com.esri.core.geometry.Polyline basePl = new com.esri.core.geometry.Polyline();
			basePl.StartPath(2, 2);
			basePl.LineTo(2, 10);
			com.esri.core.geometry.Polyline compPl = new com.esri.core.geometry.Polyline();
			compPl.StartPath(2, 4);
			compPl.LineTo(9, 4);
			compPl.LineTo(9, 9);
			compPl.LineTo(2, 9);
			compPl.LineTo(2, 4);
			bool isTouched = false;
			// GeometryEngine.relation(basePl, compPl, sr,
			// "G1 TOUCH G2");
			NUnit.Framework.Assert.AreEqual(isTouched, false);
		}

		[NUnit.Framework.Test]
		public virtual void TestTouchesBetweenPointAndLine()
		{
			com.esri.core.geometry.SpatialReference sr = com.esri.core.geometry.SpatialReference.Create(4326);
			com.esri.core.geometry.Point p = new com.esri.core.geometry.Point(2, 4);
			com.esri.core.geometry.Polyline compPl = new com.esri.core.geometry.Polyline();
			compPl.StartPath(2, 4);
			compPl.LineTo(9, 4);
			compPl.LineTo(9, 9);
			compPl.LineTo(2, 9);
			compPl.LineTo(2, 4);
			bool isTouched = com.esri.core.geometry.GeometryEngine.Touches(p, compPl, sr);
			NUnit.Framework.Assert.IsTrue(!isTouched);
		}

		[NUnit.Framework.Test]
		public virtual void TestTouchesBetweenPolylines()
		{
			com.esri.core.geometry.SpatialReference sr = com.esri.core.geometry.SpatialReference.Create(4326);
			com.esri.core.geometry.Polyline pl = new com.esri.core.geometry.Polyline();
			pl.StartPath(2, 4);
			pl.LineTo(9, 9);
			com.esri.core.geometry.Polyline compPl = new com.esri.core.geometry.Polyline();
			compPl.StartPath(2, 4);
			compPl.LineTo(9, 4);
			compPl.LineTo(9, 9);
			compPl.LineTo(2, 9);
			compPl.LineTo(2, 4);
			bool isTouched = com.esri.core.geometry.GeometryEngine.Touches(pl, compPl, sr);
			NUnit.Framework.Assert.AreEqual(isTouched, true);
		}

		[NUnit.Framework.Test]
		public virtual void TestTouchesBetweenPolylineAndPolygon()
		{
			com.esri.core.geometry.SpatialReference sr = com.esri.core.geometry.SpatialReference.Create(4326);
			com.esri.core.geometry.Polyline pl = new com.esri.core.geometry.Polyline();
			pl.StartPath(2, 4);
			pl.LineTo(1, 10);
			pl.LineTo(6, 12);
			com.esri.core.geometry.Polygon compPg = new com.esri.core.geometry.Polygon();
			compPg.StartPath(2, 4);
			compPg.LineTo(2, 9);
			compPg.LineTo(9, 9);
			compPg.LineTo(9, 4);
			compPg.StartPath(2, 9);
			compPg.LineTo(6, 12);
			compPg.LineTo(9, 10);
			bool isTouched = com.esri.core.geometry.GeometryEngine.Touches(pl, compPg, sr);
			NUnit.Framework.Assert.AreEqual(isTouched, true);
		}

		[NUnit.Framework.Test]
		public virtual void TestTouchesBetweenMultipartPolylines()
		{
			com.esri.core.geometry.SpatialReference sr = com.esri.core.geometry.SpatialReference.Create(4326);
			com.esri.core.geometry.Polyline pl = new com.esri.core.geometry.Polyline();
			pl.StartPath(2, 4);
			pl.LineTo(1, 10);
			pl.LineTo(6, 12);
			pl.StartPath(6, 12);
			pl.LineTo(12, 12);
			pl.LineTo(9, 9);
			com.esri.core.geometry.Polyline compPl = new com.esri.core.geometry.Polyline();
			compPl.StartPath(2, 4);
			compPl.LineTo(2, 9);
			compPl.LineTo(9, 9);
			compPl.LineTo(9, 4);
			compPl.StartPath(2, 9);
			compPl.LineTo(6, 12);
			compPl.LineTo(9, 10);
			bool isTouched = com.esri.core.geometry.GeometryEngine.Touches(pl, compPl, sr);
			NUnit.Framework.Assert.IsTrue(!isTouched);
		}

		// boolean isTouchedFromRest = GeometryUtils.isRelationTrue(compPl, pl,
		// sr,
		// GeometryUtils.SpatialRelationType.esriGeometryRelationTouch, "");
		// assertTrue(isTouchedFromRest == isTouched);
		[NUnit.Framework.Test]
		public virtual void TestTouchesBetweenMultipartPolygons2()
		{
			com.esri.core.geometry.SpatialReference sr = com.esri.core.geometry.SpatialReference.Create(4326);
			com.esri.core.geometry.Polygon pl = new com.esri.core.geometry.Polygon();
			pl.StartPath(2, 4);
			pl.LineTo(1, 9);
			pl.LineTo(2, 6);
			pl.StartPath(2, 9);
			pl.LineTo(6, 14);
			pl.LineTo(6, 12);
			com.esri.core.geometry.Polygon compPl = new com.esri.core.geometry.Polygon();
			compPl.StartPath(2, 4);
			compPl.LineTo(2, 9);
			compPl.LineTo(9, 9);
			compPl.LineTo(9, 4);
			compPl.StartPath(2, 9);
			compPl.LineTo(6, 12);
			compPl.LineTo(9, 10);
			bool isTouched = com.esri.core.geometry.GeometryEngine.Touches(pl, compPl, sr);
			NUnit.Framework.Assert.AreEqual(isTouched, true);
		}

		[NUnit.Framework.Test]
		public virtual void TestTouchPointLineCR183227()
		{
			// Tests CR 183227
			com.esri.core.geometry.Geometry baseGeom = new com.esri.core.geometry.Point(-130, 10);
			com.esri.core.geometry.Polyline pl = new com.esri.core.geometry.Polyline();
			// pl.startPath(new Point(-130, 10));
			pl.StartPath(-130, 10);
			pl.LineTo(-131, 15);
			pl.LineTo(-140, 20);
			com.esri.core.geometry.SpatialReference sr = com.esri.core.geometry.SpatialReference.Create(4326);
			bool isTouched;
			bool isTouched2;
			isTouched = com.esri.core.geometry.GeometryEngine.Touches(baseGeom, pl, sr);
			isTouched2 = com.esri.core.geometry.GeometryEngine.Touches(pl, baseGeom, sr);
			NUnit.Framework.Assert.IsTrue(isTouched && isTouched2);
			{
				com.esri.core.geometry.Geometry baseGeom2 = (com.esri.core.geometry.Geometry)new com.esri.core.geometry.Point(-131, 15);
				bool bIsTouched;
				bool bIsTouched2;
				bIsTouched = com.esri.core.geometry.GeometryEngine.Touches(baseGeom2, pl, sr);
				bIsTouched2 = com.esri.core.geometry.GeometryEngine.Touches(pl, baseGeom2, sr);
				NUnit.Framework.Assert.IsTrue(!bIsTouched && !bIsTouched2);
			}
		}
	}
}
