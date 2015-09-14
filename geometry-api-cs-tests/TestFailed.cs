using NUnit.Framework;

namespace com.esri.core.geometry
{
	public class TestFailed : NUnit.Framework.TestFixtureAttribute
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
		public virtual void TestCenterXY()
		{
			com.esri.core.geometry.Envelope env = new com.esri.core.geometry.Envelope(-130, 30, -70, 50);
			NUnit.Framework.Assert.AreEqual(-100, env.GetCenterX(), 0);
			NUnit.Framework.Assert.AreEqual(40, env.GetCenterY(), 0);
		}

		[NUnit.Framework.Test]
		public virtual void TestGeometryOperationSupport()
		{
			com.esri.core.geometry.Geometry baseGeom = new com.esri.core.geometry.Point(-130, 10);
			com.esri.core.geometry.Geometry comparisonGeom = new com.esri.core.geometry.Point(-130, 10);
			com.esri.core.geometry.SpatialReference sr = com.esri.core.geometry.SpatialReference.Create(4326);
			com.esri.core.geometry.Geometry diffGeom = null;
			int noException = 1;
			// no exception
			try
			{
				diffGeom = com.esri.core.geometry.GeometryEngine.Difference(baseGeom, comparisonGeom, sr);
			}
			catch (System.ArgumentException)
			{
				noException = 0;
			}
			catch (com.esri.core.geometry.GeometryException)
			{
				noException = 0;
			}
			NUnit.Framework.Assert.AreEqual(noException, 1);
		}

		[NUnit.Framework.Test]
		public virtual void TestIntersection()
		{
			com.esri.core.geometry.OperatorIntersects op = (com.esri.core.geometry.OperatorIntersects)com.esri.core.geometry.OperatorFactoryLocal.GetInstance().GetOperator(com.esri.core.geometry.Operator.Type.Intersects);
			com.esri.core.geometry.Polygon polygon = new com.esri.core.geometry.Polygon();
			// outer ring1
			polygon.StartPath(0, 0);
			polygon.LineTo(10, 10);
			polygon.LineTo(20, 0);
			com.esri.core.geometry.Point point1 = new com.esri.core.geometry.Point(15, 10);
			com.esri.core.geometry.Point point2 = new com.esri.core.geometry.Point(2, 10);
			com.esri.core.geometry.Point point3 = new com.esri.core.geometry.Point(5, 5);
			bool res = op.Execute(polygon, point1, null, null);
			NUnit.Framework.Assert.IsTrue(!res);
			res = op.Execute(polygon, point2, null, null);
			NUnit.Framework.Assert.IsTrue(!res);
			res = op.Execute(polygon, point3, null, null);
			NUnit.Framework.Assert.IsTrue(res);
		}
	}
}
