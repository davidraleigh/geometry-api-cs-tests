

namespace com.esri.core.geometry
{
	public class TestOffset : NUnit.Framework.TestFixtureAttribute
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
		public virtual void TestOffsetPoint()
		{
			try
			{
				com.esri.core.geometry.Point point = new com.esri.core.geometry.Point();
				point.SetXY(0, 0);
				com.esri.core.geometry.OperatorOffset offset = (com.esri.core.geometry.OperatorOffset)com.esri.core.geometry.OperatorFactoryLocal.GetInstance().GetOperator(com.esri.core.geometry.Operator.Type.Offset);
				com.esri.core.geometry.Geometry outputGeom = offset.Execute(point, null, 2, com.esri.core.geometry.OperatorOffset.JoinType.Round, 2, 0, null);
				NUnit.Framework.Assert.IsNull(outputGeom);
			}
			catch (System.Exception)
			{
			}
			try
			{
				com.esri.core.geometry.MultiPoint mp = new com.esri.core.geometry.MultiPoint();
				mp.Add(0, 0);
				mp.Add(10, 10);
				com.esri.core.geometry.OperatorOffset offset = (com.esri.core.geometry.OperatorOffset)com.esri.core.geometry.OperatorFactoryLocal.GetInstance().GetOperator(com.esri.core.geometry.Operator.Type.Offset);
				com.esri.core.geometry.Geometry outputGeom = offset.Execute(mp, null, 2, com.esri.core.geometry.OperatorOffset.JoinType.Round, 2, 0, null);
				NUnit.Framework.Assert.IsNull(outputGeom);
			}
			catch (System.Exception)
			{
			}
		}

		[NUnit.Framework.Test]
		public virtual void TestOffsetPolyline()
		{
			for (long i = -5; i <= 5; i++)
			{
				try
				{
					OffsetPolyline_(i, com.esri.core.geometry.OperatorOffset.JoinType.Round);
				}
				catch (System.Exception)
				{
					Fail("OffsetPolyline(Round) failure");
				}
				try
				{
					OffsetPolyline_(i, com.esri.core.geometry.OperatorOffset.JoinType.Miter);
				}
				catch (System.Exception)
				{
					Fail("OffsetPolyline(Miter) failure");
				}
				try
				{
					OffsetPolyline_(i, com.esri.core.geometry.OperatorOffset.JoinType.Bevel);
				}
				catch (System.Exception)
				{
					Fail("OffsetPolyline(Bevel) failure");
				}
				try
				{
					OffsetPolyline_(i, com.esri.core.geometry.OperatorOffset.JoinType.Square);
				}
				catch (System.Exception)
				{
					Fail("OffsetPolyline(Square) failure");
				}
			}
		}

		public virtual void OffsetPolyline_(double distance, com.esri.core.geometry.OperatorOffset.JoinType joins)
		{
			com.esri.core.geometry.Polyline polyline = new com.esri.core.geometry.Polyline();
			polyline.StartPath(0, 0);
			polyline.LineTo(6, 0);
			polyline.LineTo(6, 1);
			polyline.LineTo(4, 1);
			polyline.LineTo(4, 2);
			polyline.LineTo(10, 2);
			com.esri.core.geometry.OperatorOffset offset = (com.esri.core.geometry.OperatorOffset)com.esri.core.geometry.OperatorFactoryLocal.GetInstance().GetOperator(com.esri.core.geometry.Operator.Type.Offset);
			com.esri.core.geometry.Geometry outputGeom = offset.Execute(polyline, null, distance, joins, 2, 0, null);
			NUnit.Framework.Assert.IsNotNull(outputGeom);
		}

		[NUnit.Framework.Test]
		public virtual void TestOffsetPolygon()
		{
			for (long i = -5; i <= 5; i++)
			{
				try
				{
					OffsetPolygon_(i, com.esri.core.geometry.OperatorOffset.JoinType.Round);
				}
				catch (System.Exception)
				{
					Fail("OffsetPolyline(Round) failure");
				}
				try
				{
					OffsetPolygon_(i, com.esri.core.geometry.OperatorOffset.JoinType.Miter);
				}
				catch (System.Exception)
				{
					Fail("OffsetPolyline(Miter) failure");
				}
				try
				{
					OffsetPolygon_(i, com.esri.core.geometry.OperatorOffset.JoinType.Bevel);
				}
				catch (System.Exception)
				{
					Fail("OffsetPolyline(Bevel) failure");
				}
				try
				{
					OffsetPolygon_(i, com.esri.core.geometry.OperatorOffset.JoinType.Square);
				}
				catch (System.Exception)
				{
					Fail("OffsetPolyline(Square) failure");
				}
			}
		}

		public virtual void OffsetPolygon_(double distance, com.esri.core.geometry.OperatorOffset.JoinType joins)
		{
			com.esri.core.geometry.Polygon polygon = new com.esri.core.geometry.Polygon();
			polygon.StartPath(0, 0);
			polygon.LineTo(0, 16);
			polygon.LineTo(16, 16);
			polygon.LineTo(16, 11);
			polygon.LineTo(10, 10);
			polygon.LineTo(10, 12);
			polygon.LineTo(3, 12);
			polygon.LineTo(3, 4);
			polygon.LineTo(10, 4);
			polygon.LineTo(10, 6);
			polygon.LineTo(16, 5);
			polygon.LineTo(16, 0);
			com.esri.core.geometry.OperatorOffset offset = (com.esri.core.geometry.OperatorOffset)com.esri.core.geometry.OperatorFactoryLocal.GetInstance().GetOperator(com.esri.core.geometry.Operator.Type.Offset);
			com.esri.core.geometry.Geometry outputGeom = offset.Execute(polygon, null, distance, joins, 2, 0, null);
			NUnit.Framework.Assert.IsNotNull(outputGeom);
			if (distance > 2)
			{
				NUnit.Framework.Assert.IsTrue(outputGeom.IsEmpty());
			}
		}
	}
}
