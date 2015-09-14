using NUnit.Framework;

namespace com.esri.core.geometry
{
	public class TestUnion : NUnit.Framework.TestFixtureAttribute
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
		public static void TestUnion()
		{
			com.esri.core.geometry.Point pt = new com.esri.core.geometry.Point(10, 20);
			com.esri.core.geometry.Point pt2 = new com.esri.core.geometry.Point();
			pt2.SetXY(10, 10);
			com.esri.core.geometry.Envelope env1 = new com.esri.core.geometry.Envelope(10, 10, 30, 50);
			com.esri.core.geometry.Envelope env2 = new com.esri.core.geometry.Envelope(30, 10, 60, 50);
			com.esri.core.geometry.Geometry[] geomArray = new com.esri.core.geometry.Geometry[] { env1, env2 };
			com.esri.core.geometry.SimpleGeometryCursor inputGeometries = new com.esri.core.geometry.SimpleGeometryCursor(geomArray);
			com.esri.core.geometry.OperatorUnion union = (com.esri.core.geometry.OperatorUnion)com.esri.core.geometry.OperatorFactoryLocal.GetInstance().GetOperator(com.esri.core.geometry.Operator.Type.Union);
			com.esri.core.geometry.SpatialReference sr = com.esri.core.geometry.SpatialReference.Create(4326);
			com.esri.core.geometry.GeometryCursor outputCursor = union.Execute(inputGeometries, sr, null);
			com.esri.core.geometry.Geometry result = outputCursor.Next();
		}
	}
}
