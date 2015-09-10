using NUnit.Framework;

namespace com.esri.core.geometry
{
	public class TestWkid : NUnit.Framework.TestFixtureAttribute
	{
		[NUnit.Framework.Test]
		public virtual void Test()
		{
			com.esri.core.geometry.SpatialReference sr = com.esri.core.geometry.SpatialReference.Create(102100);
			NUnit.Framework.Assert.IsTrue(sr.GetID() == 102100);
			NUnit.Framework.Assert.IsTrue(sr.GetLatestID() == 3857);
			NUnit.Framework.Assert.IsTrue(sr.GetOldID() == 102100);
			NUnit.Framework.Assert.IsTrue(sr.GetTolerance() == 0.001);
			com.esri.core.geometry.SpatialReference sr84 = com.esri.core.geometry.SpatialReference.Create(4326);
			double tol84 = sr84.GetTolerance();
			NUnit.Framework.Assert.IsTrue(System.Math.Abs(tol84 - 1e-8) < 1e-8 * 1e-8);
		}

		[NUnit.Framework.Test]
		public virtual void Test_80()
		{
			com.esri.core.geometry.SpatialReference sr = com.esri.core.geometry.SpatialReference.Create(3857);
			NUnit.Framework.Assert.IsTrue(sr.GetID() == 3857);
			NUnit.Framework.Assert.IsTrue(sr.GetLatestID() == 3857);
			NUnit.Framework.Assert.IsTrue(sr.GetOldID() == 102100);
			NUnit.Framework.Assert.IsTrue(sr.GetTolerance() == 0.001);
		}
	}
}
