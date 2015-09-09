using Sharpen;

namespace com.esri.core.geometry
{
	public class TestEquals : NUnit.Framework.TestCase
	{
		/// <exception cref="System.Exception"/>
		protected override void SetUp()
		{
			base.SetUp();
		}

		/// <exception cref="System.Exception"/>
		protected override void TearDown()
		{
			base.TearDown();
		}

		[NUnit.Framework.Test]
		public virtual void TestEqualsOnEnvelopes()
		{
			com.esri.core.geometry.SpatialReference sr = com.esri.core.geometry.SpatialReference.Create(4326);
			com.esri.core.geometry.Point p = new com.esri.core.geometry.Point(-130, 10);
			com.esri.core.geometry.Envelope env = new com.esri.core.geometry.Envelope(p, 12, 12);
			com.esri.core.geometry.Envelope env2 = new com.esri.core.geometry.Envelope(-136, 4, -124, 16);
			bool isEqual;
			try
			{
				isEqual = com.esri.core.geometry.GeometryEngine.Equals(env, env2, sr);
			}
			catch (System.ArgumentException)
			{
				isEqual = false;
			}
			NUnit.Framework.Assert.IsTrue(isEqual);
		}

		[NUnit.Framework.Test]
		public virtual void TestEqualsOnPoints()
		{
			com.esri.core.geometry.SpatialReference sr = com.esri.core.geometry.SpatialReference.Create(4326);
			com.esri.core.geometry.Point p1 = new com.esri.core.geometry.Point(-116, 40);
			com.esri.core.geometry.Point p2 = new com.esri.core.geometry.Point(-120, 39);
			com.esri.core.geometry.Point p3 = new com.esri.core.geometry.Point(-121, 10);
			com.esri.core.geometry.Point p4 = new com.esri.core.geometry.Point(-130, 12);
			com.esri.core.geometry.Point p5 = new com.esri.core.geometry.Point(-108, 25);
			com.esri.core.geometry.Point p12 = new com.esri.core.geometry.Point(-116, 40);
			com.esri.core.geometry.Point p22 = new com.esri.core.geometry.Point(-120, 39);
			com.esri.core.geometry.Point p32 = new com.esri.core.geometry.Point(-121, 10);
			com.esri.core.geometry.Point p42 = new com.esri.core.geometry.Point(-130, 12);
			com.esri.core.geometry.Point p52 = new com.esri.core.geometry.Point(-108, 25);
			bool isEqual1 = false;
			bool isEqual2 = false;
			bool isEqual3 = false;
			bool isEqual4 = false;
			bool isEqual5 = false;
			try
			{
				isEqual1 = com.esri.core.geometry.GeometryEngine.Equals(p1, p12, sr);
				isEqual2 = com.esri.core.geometry.GeometryEngine.Equals(p1, p12, sr);
				isEqual3 = com.esri.core.geometry.GeometryEngine.Equals(p1, p12, sr);
				isEqual4 = com.esri.core.geometry.GeometryEngine.Equals(p1, p12, sr);
				isEqual5 = com.esri.core.geometry.GeometryEngine.Equals(p1, p12, sr);
			}
			catch (System.ArgumentException)
			{
			}
			NUnit.Framework.Assert.IsTrue(isEqual1 && isEqual2 && isEqual3 && isEqual4 && isEqual5);
		}

		[NUnit.Framework.Test]
		public virtual void TestEqualsOnPolygons()
		{
			com.esri.core.geometry.SpatialReference sr = com.esri.core.geometry.SpatialReference.Create(4326);
			com.esri.core.geometry.Polygon baseMp = new com.esri.core.geometry.Polygon();
			com.esri.core.geometry.Polygon compMp = new com.esri.core.geometry.Polygon();
			baseMp.StartPath(-116, 40);
			baseMp.LineTo(-120, 39);
			baseMp.LineTo(-121, 10);
			baseMp.LineTo(-130, 12);
			baseMp.LineTo(-108, 25);
			compMp.StartPath(-116, 40);
			compMp.LineTo(-120, 39);
			compMp.LineTo(-121, 10);
			compMp.LineTo(-130, 12);
			compMp.LineTo(-108, 25);
			bool isEqual;
			try
			{
				isEqual = com.esri.core.geometry.GeometryEngine.Equals(baseMp, compMp, sr);
			}
			catch (System.ArgumentException)
			{
				isEqual = false;
			}
			NUnit.Framework.Assert.IsTrue(isEqual);
		}

		[NUnit.Framework.Test]
		public virtual void TestEqualsOnPolylines()
		{
			com.esri.core.geometry.SpatialReference sr = com.esri.core.geometry.SpatialReference.Create(4326);
			com.esri.core.geometry.Polyline baseMp = new com.esri.core.geometry.Polyline();
			com.esri.core.geometry.Polyline compMp = new com.esri.core.geometry.Polyline();
			baseMp.StartPath(-116, 40);
			baseMp.LineTo(-120, 39);
			baseMp.LineTo(-121, 10);
			baseMp.LineTo(-130, 12);
			baseMp.LineTo(-108, 25);
			compMp.StartPath(-116, 40);
			compMp.LineTo(-120, 39);
			compMp.LineTo(-121, 10);
			compMp.LineTo(-130, 12);
			compMp.LineTo(-108, 25);
			bool isEqual;
			try
			{
				isEqual = com.esri.core.geometry.GeometryEngine.Equals(baseMp, compMp, sr);
			}
			catch (System.ArgumentException)
			{
				isEqual = false;
			}
			NUnit.Framework.Assert.IsTrue(isEqual);
		}

		[NUnit.Framework.Test]
		public virtual void TestEqualsOnMultiPoints()
		{
			com.esri.core.geometry.SpatialReference sr = com.esri.core.geometry.SpatialReference.Create(4326);
			com.esri.core.geometry.MultiPoint baseMp = new com.esri.core.geometry.MultiPoint();
			com.esri.core.geometry.MultiPoint compMp = new com.esri.core.geometry.MultiPoint();
			baseMp.Add(new com.esri.core.geometry.Point(-116, 40));
			baseMp.Add(new com.esri.core.geometry.Point(-120, 39));
			baseMp.Add(new com.esri.core.geometry.Point(-121, 10));
			baseMp.Add(new com.esri.core.geometry.Point(-130, 12));
			baseMp.Add(new com.esri.core.geometry.Point(-108, 25));
			compMp.Add(new com.esri.core.geometry.Point(-116, 40));
			compMp.Add(new com.esri.core.geometry.Point(-120, 39));
			compMp.Add(new com.esri.core.geometry.Point(-121, 10));
			compMp.Add(new com.esri.core.geometry.Point(-130, 12));
			compMp.Add(new com.esri.core.geometry.Point(-108, 25));
			bool isEqual;
			try
			{
				isEqual = com.esri.core.geometry.GeometryEngine.Equals(baseMp, compMp, sr);
			}
			catch (System.ArgumentException)
			{
				isEqual = false;
			}
			NUnit.Framework.Assert.IsTrue(isEqual);
		}
	}
}
