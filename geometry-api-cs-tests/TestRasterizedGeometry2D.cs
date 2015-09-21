using NUnit.Framework;

namespace com.esri.core.geometry
{
	public class TestRasterizedGeometry2D : NUnit.Framework.TestFixtureAttribute
	{
		internal virtual bool RgHelper(com.esri.core.geometry.RasterizedGeometry2D rg, com.esri.core.geometry.MultiPath mp)
		{
			com.esri.core.geometry.SegmentIterator iter = mp.QuerySegmentIterator();
			while (iter.NextPath())
			{
				while (iter.HasNextSegment())
				{
					com.esri.core.geometry.Segment seg = iter.NextSegment();
					int count = 20;
					for (int i = 0; i < count; i++)
					{
						double t = (1.0 * i / count);
						com.esri.core.geometry.Point2D pt = seg.GetCoord2D(t);
						com.esri.core.geometry.RasterizedGeometry2D.HitType hit = rg.QueryPointInGeometry(pt.x, pt.y);
						if (hit != com.esri.core.geometry.RasterizedGeometry2D.HitType.Border)
						{
							return false;
						}
					}
				}
			}
			if (mp.GetType() != com.esri.core.geometry.Geometry.Type.Polygon)
			{
				return true;
			}
			com.esri.core.geometry.Polygon poly = (com.esri.core.geometry.Polygon)mp;
			com.esri.core.geometry.Envelope2D env = new com.esri.core.geometry.Envelope2D();
			poly.QueryEnvelope2D(env);
			int count_1 = 100;
			for (int iy = 0; iy < count_1; iy++)
			{
				double ty = 1.0 * iy / count_1;
				double y = env.ymin * (1.0 - ty) + ty * env.ymax;
				for (int ix = 0; ix < count_1; ix++)
				{
					double tx = 1.0 * ix / count_1;
					double x = env.xmin * (1.0 - tx) + tx * env.xmax;
					com.esri.core.geometry.RasterizedGeometry2D.HitType hit = rg.QueryPointInGeometry(x, y);
					com.esri.core.geometry.PolygonUtils.PiPResult res = com.esri.core.geometry.PolygonUtils.IsPointInPolygon2D(poly, new com.esri.core.geometry.Point2D(x, y), 0);
					if (res == com.esri.core.geometry.PolygonUtils.PiPResult.PiPInside)
					{
						bool bgood = (hit == com.esri.core.geometry.RasterizedGeometry2D.HitType.Border || hit == com.esri.core.geometry.RasterizedGeometry2D.HitType.Inside);
						if (!bgood)
						{
							return false;
						}
					}
					else
					{
						if (res == com.esri.core.geometry.PolygonUtils.PiPResult.PiPOutside)
						{
							bool bgood = (hit == com.esri.core.geometry.RasterizedGeometry2D.HitType.Border || hit == com.esri.core.geometry.RasterizedGeometry2D.HitType.Outside);
							if (!bgood)
							{
								return false;
							}
						}
						else
						{
							bool bgood = (hit == com.esri.core.geometry.RasterizedGeometry2D.HitType.Border);
							if (!bgood)
							{
								return false;
							}
						}
					}
				}
			}
			return true;
		}

		[NUnit.Framework.Test]
		public virtual void Test()
		{
			{
				com.esri.core.geometry.Polygon poly = new com.esri.core.geometry.Polygon();
				poly.StartPath(10, 10);
				poly.LineTo(100, 10);
				poly.LineTo(100, 100);
				poly.LineTo(10, 100);
				// create using move semantics. Usually we do not use this
				// approach.
				com.esri.core.geometry.RasterizedGeometry2D rg = com.esri.core.geometry.RasterizedGeometry2D.Create(poly, 0, 1024);
				//rg.dbgSaveToBitmap("c:/temp/_dbg.bmp");
				com.esri.core.geometry.RasterizedGeometry2D.HitType res;
				res = rg.QueryPointInGeometry(7, 10);
				NUnit.Framework.Assert.IsTrue(res == com.esri.core.geometry.RasterizedGeometry2D.HitType.Outside);
				res = rg.QueryPointInGeometry(10, 10);
				NUnit.Framework.Assert.IsTrue(res == com.esri.core.geometry.RasterizedGeometry2D.HitType.Border);
				res = rg.QueryPointInGeometry(50, 50);
				NUnit.Framework.Assert.IsTrue(res == com.esri.core.geometry.RasterizedGeometry2D.HitType.Inside);
				NUnit.Framework.Assert.IsTrue(RgHelper(rg, poly));
			}
			{
				com.esri.core.geometry.Polygon poly = new com.esri.core.geometry.Polygon();
				// create a star (non-simple)
				poly.StartPath(1, 0);
				poly.LineTo(5, 10);
				poly.LineTo(9, 0);
				poly.LineTo(0, 6);
				poly.LineTo(10, 6);
				com.esri.core.geometry.RasterizedGeometry2D rg = com.esri.core.geometry.RasterizedGeometry2D.Create(poly, 0, 1024);
				//rg.dbgSaveToBitmap("c:/temp/_dbg.bmp");
				com.esri.core.geometry.RasterizedGeometry2D.HitType res;
				res = rg.QueryPointInGeometry(5, 5.5);
				NUnit.Framework.Assert.IsTrue(res == com.esri.core.geometry.RasterizedGeometry2D.HitType.Outside);
				res = rg.QueryPointInGeometry(5, 8);
				NUnit.Framework.Assert.IsTrue(res == com.esri.core.geometry.RasterizedGeometry2D.HitType.Inside);
				res = rg.QueryPointInGeometry(1.63, 0.77);
				NUnit.Framework.Assert.IsTrue(res == com.esri.core.geometry.RasterizedGeometry2D.HitType.Inside);
				res = rg.QueryPointInGeometry(1, 3);
				NUnit.Framework.Assert.IsTrue(res == com.esri.core.geometry.RasterizedGeometry2D.HitType.Outside);
				res = rg.QueryPointInGeometry(1.6, 0.1);
				NUnit.Framework.Assert.IsTrue(res == com.esri.core.geometry.RasterizedGeometry2D.HitType.Outside);
				NUnit.Framework.Assert.IsTrue(RgHelper(rg, poly));
			}
			{
				//com.esri.core.geometry.Polygon poly = new com.esri.core.geometry.Polygon();
				//// create a star (non-simple)
				//poly.StartPath(1, 0);
				//poly.LineTo(5, 10);
				//poly.LineTo(9, 0);
				//poly.LineTo(0, 6);
				//poly.LineTo(10, 6);
				//com.esri.core.geometry.SpatialReference sr = com.esri.core.geometry.SpatialReference.Create(4326);
				//poly = (com.esri.core.geometry.Polygon)com.esri.core.geometry.OperatorSimplify.Local().Execute(poly, sr, true, null);
				//com.esri.core.geometry.OperatorContains.Local().AccelerateGeometry(poly, sr, com.esri.core.geometry.Geometry.GeometryAccelerationDegree.enumMedium);
				//NUnit.Framework.Assert.IsFalse(com.esri.core.geometry.OperatorContains.Local().Execute(poly, new com.esri.core.geometry.Point(5, 5.5), sr, null));
				//NUnit.Framework.Assert.IsTrue(com.esri.core.geometry.OperatorContains.Local().Execute(poly, new com.esri.core.geometry.Point(5, 8), sr, null));
				//NUnit.Framework.Assert.IsTrue(com.esri.core.geometry.OperatorContains.Local().Execute(poly, new com.esri.core.geometry.Point(1.63, 0.77), sr, null));
				//NUnit.Framework.Assert.IsFalse(com.esri.core.geometry.OperatorContains.Local().Execute(poly, new com.esri.core.geometry.Point(1, 3), sr, null));
				//NUnit.Framework.Assert.IsFalse(com.esri.core.geometry.OperatorContains.Local().Execute(poly, new com.esri.core.geometry.Point(1.6, 0.1), sr, null));
			}
		}
		/*
		{
		Geometry g = OperatorFactoryLocal.loadGeometryFromEsriShapeDbg("c:/temp/_poly_final.bin");
		RasterizedGeometry2D rg1 = RasterizedGeometry2D
		.create(g, 0, 1024);//warmup
		rg1 = null;
		
		long t0 = System.nanoTime();
		RasterizedGeometry2D rg = RasterizedGeometry2D
		.create(g, 0, 1024 * 1024);
		long t1 = System.nanoTime();
		double d = (t1 - t0) / 1000000.0;
		System.out.printf("Time to rasterize the geometry: %f", d);
		
		rg.dbgSaveToBitmap("c:/temp/_dbg.bmp");
		for (;;){}
		}*/
	}
}
