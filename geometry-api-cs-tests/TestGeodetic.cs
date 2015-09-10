

namespace com.esri.core.geometry
{
	public class TestGeodetic : NUnit.Framework.TestFixtureAttribute
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
		public virtual void TestTriangleLength()
		{
			com.esri.core.geometry.Point pt_0 = new com.esri.core.geometry.Point(10, 10);
			com.esri.core.geometry.Point pt_1 = new com.esri.core.geometry.Point(20, 20);
			com.esri.core.geometry.Point pt_2 = new com.esri.core.geometry.Point(20, 10);
			double length = 0.0;
			length += com.esri.core.geometry.GeometryEngine.GeodesicDistanceOnWGS84(pt_0, pt_1);
			length += com.esri.core.geometry.GeometryEngine.GeodesicDistanceOnWGS84(pt_1, pt_2);
			length += com.esri.core.geometry.GeometryEngine.GeodesicDistanceOnWGS84(pt_2, pt_0);
			NUnit.Framework.Assert.IsTrue(System.Math.Abs(length - 3744719.4094597572) < 1e-13 * 3744719.4094597572);
		}

		[NUnit.Framework.Test]
		public virtual void TestRotationInvariance()
		{
			com.esri.core.geometry.Point pt_0 = new com.esri.core.geometry.Point(10, 40);
			com.esri.core.geometry.Point pt_1 = new com.esri.core.geometry.Point(20, 60);
			com.esri.core.geometry.Point pt_2 = new com.esri.core.geometry.Point(20, 40);
			double length = 0.0;
			length += com.esri.core.geometry.GeometryEngine.GeodesicDistanceOnWGS84(pt_0, pt_1);
			length += com.esri.core.geometry.GeometryEngine.GeodesicDistanceOnWGS84(pt_1, pt_2);
			length += com.esri.core.geometry.GeometryEngine.GeodesicDistanceOnWGS84(pt_2, pt_0);
			NUnit.Framework.Assert.IsTrue(System.Math.Abs(length - 5409156.3896271614) < 1e-13 * 5409156.3896271614);
			for (int i = -540; i < 540; i += 5)
			{
				pt_0.SetXY(i + 10, 40);
				pt_1.SetXY(i + 20, 60);
				pt_2.SetXY(i + 20, 40);
				length = 0.0;
				length += com.esri.core.geometry.GeometryEngine.GeodesicDistanceOnWGS84(pt_0, pt_1);
				length += com.esri.core.geometry.GeometryEngine.GeodesicDistanceOnWGS84(pt_1, pt_2);
				length += com.esri.core.geometry.GeometryEngine.GeodesicDistanceOnWGS84(pt_2, pt_0);
				NUnit.Framework.Assert.IsTrue(System.Math.Abs(length - 5409156.3896271614) < 1e-13 * 5409156.3896271614);
			}
		}

		[NUnit.Framework.Test]
		public virtual void TestLengthAccurateCR191313()
		{
		}
		/*
		* // random_test(); OperatorFactoryLocal engine =
		* OperatorFactoryLocal.getInstance(); //TODO: Make this:
		* OperatorShapePreservingLength geoLengthOp =
		* (OperatorShapePreservingLength)
		* factory.getOperator(Operator.Type.ShapePreservingLength);
		* SpatialReference spatialRef = SpatialReference.create(102631);
		* //[6097817.59407673
		* ,17463475.2931517],[-1168053.34617516,11199801.3734424
		* ]]],"spatialReference":{"wkid":102631}
		*
		* Polyline polyline = new Polyline();
		* polyline.startPath(6097817.59407673, 17463475.2931517);
		* polyline.lineTo(-1168053.34617516, 11199801.3734424); double length =
		* geoLengthOp.execute(polyline, spatialRef, null);
		* assertTrue(Math.abs(length - 2738362.3249366437) < 2e-9 * length);
		*/
	}
}
