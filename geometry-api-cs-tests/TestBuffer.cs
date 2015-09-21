using NUnit.Framework;

namespace com.esri.core.geometry
{
	public class TestBuffer : NUnit.Framework.TestFixtureAttribute
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
		public virtual void TestBufferPoint()
		{
			com.esri.core.geometry.SpatialReference sr = com.esri.core.geometry.SpatialReference.Create(4326);
			com.esri.core.geometry.Point inputGeom = new com.esri.core.geometry.Point(12, 120);
			com.esri.core.geometry.OperatorBuffer buffer = (com.esri.core.geometry.OperatorBuffer)com.esri.core.geometry.OperatorFactoryLocal.GetInstance().GetOperator(com.esri.core.geometry.Operator.Type.Buffer);
			com.esri.core.geometry.OperatorSimplify simplify = (com.esri.core.geometry.OperatorSimplify)com.esri.core.geometry.OperatorFactoryLocal.GetInstance().GetOperator(com.esri.core.geometry.Operator.Type.Simplify);
			com.esri.core.geometry.Geometry result = buffer.Execute(inputGeom, sr, 40.0, null);
			NUnit.Framework.Assert.IsTrue(result.GetType().Value() == com.esri.core.geometry.Geometry.GeometryType.Polygon);
			com.esri.core.geometry.Polygon poly = (com.esri.core.geometry.Polygon)result;
			int pathCount = poly.GetPathCount();
			NUnit.Framework.Assert.IsTrue(pathCount == 1);
			int pointCount = poly.GetPointCount();
			NUnit.Framework.Assert.IsTrue(System.Math.Abs(pointCount - 100.0) < 10);
			com.esri.core.geometry.Envelope2D env2D = new com.esri.core.geometry.Envelope2D();
			result.QueryEnvelope2D(env2D);
			NUnit.Framework.Assert.IsTrue(System.Math.Abs(env2D.GetWidth() - 80) < 0.01 && System.Math.Abs(env2D.GetHeight() - 80) < 0.01);
			NUnit.Framework.Assert.IsTrue(System.Math.Abs(env2D.GetCenterX() - 12) < 0.001 && System.Math.Abs(env2D.GetCenterY() - 120) < 0.001);
			com.esri.core.geometry.NonSimpleResult nsr = new com.esri.core.geometry.NonSimpleResult();
			bool is_simple = simplify.IsSimpleAsFeature(result, sr, true, nsr, null);
			NUnit.Framework.Assert.IsTrue(is_simple);
			{
				result = buffer.Execute(inputGeom, sr, 0, null);
				NUnit.Framework.Assert.IsTrue(result.GetType().Value() == com.esri.core.geometry.Geometry.GeometryType.Polygon);
				NUnit.Framework.Assert.IsTrue(result.IsEmpty());
			}
			{
				result = buffer.Execute(inputGeom, sr, -1, null);
				NUnit.Framework.Assert.IsTrue(result.GetType().Value() == com.esri.core.geometry.Geometry.GeometryType.Polygon);
				NUnit.Framework.Assert.IsTrue(result.IsEmpty());
			}
		}

		[NUnit.Framework.Test]
		public virtual void TestBufferEnvelope()
		{
			com.esri.core.geometry.SpatialReference sr = com.esri.core.geometry.SpatialReference.Create(4326);
			com.esri.core.geometry.Envelope inputGeom = new com.esri.core.geometry.Envelope(1, 0, 200, 400);
			com.esri.core.geometry.OperatorBuffer buffer = (com.esri.core.geometry.OperatorBuffer)com.esri.core.geometry.OperatorFactoryLocal.GetInstance().GetOperator(com.esri.core.geometry.Operator.Type.Buffer);
			com.esri.core.geometry.OperatorSimplify simplify = (com.esri.core.geometry.OperatorSimplify)com.esri.core.geometry.OperatorFactoryLocal.GetInstance().GetOperator(com.esri.core.geometry.Operator.Type.Simplify);
			com.esri.core.geometry.Geometry result = buffer.Execute(inputGeom, sr, 40.0, null);
			NUnit.Framework.Assert.IsTrue(result.GetType().Value() == com.esri.core.geometry.Geometry.GeometryType.Polygon);
			com.esri.core.geometry.Polygon poly = (com.esri.core.geometry.Polygon)(result);
			com.esri.core.geometry.Envelope2D env2D = new com.esri.core.geometry.Envelope2D();
			result.QueryEnvelope2D(env2D);
			NUnit.Framework.Assert.IsTrue(System.Math.Abs(env2D.GetWidth() - (80 + 199)) < 0.001 && System.Math.Abs(env2D.GetHeight() - (80 + 400)) < 0.001);
			NUnit.Framework.Assert.IsTrue(System.Math.Abs(env2D.GetCenterX() - 201.0 / 2) < 0.001 && System.Math.Abs(env2D.GetCenterY() - 400 / 2.0) < 0.001);
			int pathCount = poly.GetPathCount();
			NUnit.Framework.Assert.IsTrue(pathCount == 1);
			int pointCount = poly.GetPointCount();
			NUnit.Framework.Assert.IsTrue(System.Math.Abs(pointCount - 104.0) < 10);
			com.esri.core.geometry.NonSimpleResult nsr = new com.esri.core.geometry.NonSimpleResult();
			NUnit.Framework.Assert.IsTrue(simplify.IsSimpleAsFeature(result, sr, true, nsr, null));
			{
				result = buffer.Execute(inputGeom, sr, -200.0, null);
				NUnit.Framework.Assert.IsTrue(result.GetType().Value() == com.esri.core.geometry.Geometry.GeometryType.Polygon);
				NUnit.Framework.Assert.IsTrue(result.IsEmpty());
			}
			{
				result = buffer.Execute(inputGeom, sr, -200.0, null);
				NUnit.Framework.Assert.IsTrue(result.GetType().Value() == com.esri.core.geometry.Geometry.GeometryType.Polygon);
				NUnit.Framework.Assert.IsTrue(result.IsEmpty());
			}
			{
				result = buffer.Execute(inputGeom, sr, -199 / 2.0, null);
				NUnit.Framework.Assert.IsTrue(result.GetType().Value() == com.esri.core.geometry.Geometry.GeometryType.Polygon);
				NUnit.Framework.Assert.IsTrue(result.IsEmpty());
			}
			{
				result = buffer.Execute(inputGeom, sr, -50.0, null);
				poly = (com.esri.core.geometry.Polygon)(result);
				result.QueryEnvelope2D(env2D);
				NUnit.Framework.Assert.IsTrue(System.Math.Abs(env2D.GetWidth() - (199 - 100)) < 0.001 && System.Math.Abs(env2D.GetHeight() - (400 - 100)) < 0.001);
				NUnit.Framework.Assert.IsTrue(System.Math.Abs(env2D.GetCenterX() - 201.0 / 2) < 0.001 && System.Math.Abs(env2D.GetCenterY() - 400 / 2.0) < 0.001);
				pathCount = poly.GetPathCount();
				NUnit.Framework.Assert.IsTrue(pathCount == 1);
				pointCount = poly.GetPointCount();
				NUnit.Framework.Assert.IsTrue(System.Math.Abs(pointCount - 4.0) < 10);
				NUnit.Framework.Assert.IsTrue(simplify.IsSimpleAsFeature(result, sr, null));
			}
		}

		[NUnit.Framework.Test]
		public virtual void TestBufferMultiPoint()
		{
			com.esri.core.geometry.SpatialReference sr = com.esri.core.geometry.SpatialReference.Create(4326);
			com.esri.core.geometry.OperatorBuffer buffer = (com.esri.core.geometry.OperatorBuffer)com.esri.core.geometry.OperatorFactoryLocal.GetInstance().GetOperator(com.esri.core.geometry.Operator.Type.Buffer);
			com.esri.core.geometry.OperatorSimplify simplify = (com.esri.core.geometry.OperatorSimplify)com.esri.core.geometry.OperatorFactoryLocal.GetInstance().GetOperator(com.esri.core.geometry.Operator.Type.Simplify);
			com.esri.core.geometry.MultiPoint inputGeom = new com.esri.core.geometry.MultiPoint();
			inputGeom.Add(12, 120);
			inputGeom.Add(20, 120);
			com.esri.core.geometry.Geometry result = buffer.Execute(inputGeom, sr, 40.0, null);
			NUnit.Framework.Assert.IsTrue(result.GetType().Value() == com.esri.core.geometry.Geometry.GeometryType.Polygon);
			com.esri.core.geometry.Polygon poly = (com.esri.core.geometry.Polygon)(result);
			com.esri.core.geometry.Envelope2D env2D = new com.esri.core.geometry.Envelope2D();
			result.QueryEnvelope2D(env2D);
			NUnit.Framework.Assert.IsTrue(System.Math.Abs(env2D.GetWidth() - 80 - 8) < 0.001 && System.Math.Abs(env2D.GetHeight() - 80) < 0.001);
			NUnit.Framework.Assert.IsTrue(System.Math.Abs(env2D.GetCenterX() - 16) < 0.001 && System.Math.Abs(env2D.GetCenterY() - 120) < 0.001);
			int pathCount = poly.GetPathCount();
			NUnit.Framework.Assert.IsTrue(pathCount == 1);
			int pointCount = poly.GetPointCount();
			NUnit.Framework.Assert.IsTrue(System.Math.Abs(pointCount - 108.0) < 10);
			NUnit.Framework.Assert.IsTrue(simplify.IsSimpleAsFeature(result, sr, null));
			{
				result = buffer.Execute(inputGeom, sr, 0, null);
				NUnit.Framework.Assert.IsTrue(result.GetType().Value() == com.esri.core.geometry.Geometry.GeometryType.Polygon);
				NUnit.Framework.Assert.IsTrue(result.IsEmpty());
			}
			{
				result = buffer.Execute(inputGeom, sr, -1, null);
				NUnit.Framework.Assert.IsTrue(result.GetType().Value() == com.esri.core.geometry.Geometry.GeometryType.Polygon);
				NUnit.Framework.Assert.IsTrue(result.IsEmpty());
			}
		}

		[NUnit.Framework.Test]
		public virtual void TestBufferLine()
		{
			com.esri.core.geometry.SpatialReference sr = com.esri.core.geometry.SpatialReference.Create(4326);
			com.esri.core.geometry.Line inputGeom = new com.esri.core.geometry.Line(12, 120, 20, 120);
			com.esri.core.geometry.OperatorBuffer buffer = (com.esri.core.geometry.OperatorBuffer)com.esri.core.geometry.OperatorFactoryLocal.GetInstance().GetOperator(com.esri.core.geometry.Operator.Type.Buffer);
			com.esri.core.geometry.OperatorSimplify simplify = (com.esri.core.geometry.OperatorSimplify)com.esri.core.geometry.OperatorFactoryLocal.GetInstance().GetOperator(com.esri.core.geometry.Operator.Type.Simplify);
			com.esri.core.geometry.Geometry result = buffer.Execute(inputGeom, sr, 40.0, null);
			NUnit.Framework.Assert.IsTrue(result.GetType().Value() == com.esri.core.geometry.Geometry.GeometryType.Polygon);
			com.esri.core.geometry.Polygon poly = (com.esri.core.geometry.Polygon)(result);
			com.esri.core.geometry.Envelope2D env2D = new com.esri.core.geometry.Envelope2D();
			result.QueryEnvelope2D(env2D);
			NUnit.Framework.Assert.IsTrue(System.Math.Abs(env2D.GetWidth() - 80 - 8) < 0.001 && System.Math.Abs(env2D.GetHeight() - 80) < 0.001);
			NUnit.Framework.Assert.IsTrue(System.Math.Abs(env2D.GetCenterX() - 16) < 0.001 && System.Math.Abs(env2D.GetCenterY() - 120) < 0.001);
			int pathCount = poly.GetPathCount();
			NUnit.Framework.Assert.IsTrue(pathCount == 1);
			int pointCount = poly.GetPointCount();
			NUnit.Framework.Assert.IsTrue(System.Math.Abs(pointCount - 100.0) < 10);
			NUnit.Framework.Assert.IsTrue(simplify.IsSimpleAsFeature(result, sr, null));
			{
				result = buffer.Execute(inputGeom, sr, 0, null);
				NUnit.Framework.Assert.IsTrue(result.GetType().Value() == com.esri.core.geometry.Geometry.GeometryType.Polygon);
				NUnit.Framework.Assert.IsTrue(result.IsEmpty());
			}
			{
				result = buffer.Execute(inputGeom, sr, -1, null);
				NUnit.Framework.Assert.IsTrue(result.GetType().Value() == com.esri.core.geometry.Geometry.GeometryType.Polygon);
				NUnit.Framework.Assert.IsTrue(result.IsEmpty());
			}
		}

		[NUnit.Framework.Test]
		public virtual void TestBufferPolyline()
		{
			com.esri.core.geometry.SpatialReference sr = com.esri.core.geometry.SpatialReference.Create(4326);
			com.esri.core.geometry.Polyline inputGeom = new com.esri.core.geometry.Polyline();
			com.esri.core.geometry.OperatorBuffer buffer = (com.esri.core.geometry.OperatorBuffer)com.esri.core.geometry.OperatorFactoryLocal.GetInstance().GetOperator(com.esri.core.geometry.Operator.Type.Buffer);
			com.esri.core.geometry.OperatorSimplify simplify = (com.esri.core.geometry.OperatorSimplify)com.esri.core.geometry.OperatorFactoryLocal.GetInstance().GetOperator(com.esri.core.geometry.Operator.Type.Simplify);
			inputGeom.StartPath(0, 0);
			inputGeom.LineTo(50, 50);
			inputGeom.LineTo(50, 0);
			inputGeom.LineTo(0, 50);
			{
				com.esri.core.geometry.Geometry result = buffer.Execute(inputGeom, sr, 0, null);
				NUnit.Framework.Assert.IsTrue(result.GetType().Value() == com.esri.core.geometry.Geometry.GeometryType.Polygon);
				NUnit.Framework.Assert.IsTrue(result.IsEmpty());
			}
			{
				com.esri.core.geometry.Geometry result = buffer.Execute(inputGeom, sr, -1, null);
				NUnit.Framework.Assert.IsTrue(result.GetType().Value() == com.esri.core.geometry.Geometry.GeometryType.Polygon);
				NUnit.Framework.Assert.IsTrue(result.IsEmpty());
			}
			{
				com.esri.core.geometry.Geometry result = buffer.Execute(inputGeom, sr, 40.0, null);
				NUnit.Framework.Assert.IsTrue(result.GetType().Value() == com.esri.core.geometry.Geometry.GeometryType.Polygon);
				com.esri.core.geometry.Polygon poly = (com.esri.core.geometry.Polygon)(result);
				com.esri.core.geometry.Envelope2D env2D = new com.esri.core.geometry.Envelope2D();
				result.QueryEnvelope2D(env2D);
				NUnit.Framework.Assert.IsTrue(System.Math.Abs(env2D.GetWidth() - 80 - 50) < 0.1 && System.Math.Abs(env2D.GetHeight() - 80 - 50) < 0.1);
				NUnit.Framework.Assert.IsTrue(System.Math.Abs(env2D.GetCenterX() - 25) < 0.1 && System.Math.Abs(env2D.GetCenterY() - 25) < 0.1);
				int pathCount = poly.GetPathCount();
				NUnit.Framework.Assert.IsTrue(pathCount == 1);
				int pointCount = poly.GetPointCount();
				NUnit.Framework.Assert.IsTrue(System.Math.Abs(pointCount - 171.0) < 10);
				NUnit.Framework.Assert.IsTrue(simplify.IsSimpleAsFeature(result, sr, null));
			}
			{
				com.esri.core.geometry.Geometry result = buffer.Execute(inputGeom, sr, 4.0, null);
				NUnit.Framework.Assert.IsTrue(result.GetType().Value() == com.esri.core.geometry.Geometry.GeometryType.Polygon);
				com.esri.core.geometry.Polygon poly = (com.esri.core.geometry.Polygon)(result);
				com.esri.core.geometry.Envelope2D env2D = new com.esri.core.geometry.Envelope2D();
				result.QueryEnvelope2D(env2D);
				NUnit.Framework.Assert.IsTrue(System.Math.Abs(env2D.GetWidth() - 8 - 50) < 0.1 && System.Math.Abs(env2D.GetHeight() - 8 - 50) < 0.1);
				NUnit.Framework.Assert.IsTrue(System.Math.Abs(env2D.GetCenterX() - 25) < 0.1 && System.Math.Abs(env2D.GetCenterY() - 25) < 0.1);
				int pathCount = poly.GetPathCount();
				NUnit.Framework.Assert.IsTrue(pathCount == 2);
				int pointCount = poly.GetPointCount();
				NUnit.Framework.Assert.IsTrue(System.Math.Abs(pointCount - 186.0) < 10);
				NUnit.Framework.Assert.IsTrue(simplify.IsSimpleAsFeature(result, sr, null));
			}
			{
				inputGeom = new com.esri.core.geometry.Polyline();
				inputGeom.StartPath(0, 0);
				inputGeom.LineTo(50, 50);
				inputGeom.StartPath(50, 0);
				inputGeom.LineTo(0, 50);
				com.esri.core.geometry.Geometry result = buffer.Execute(inputGeom, sr, 4.0, null);
				NUnit.Framework.Assert.IsTrue(result.GetType().Value() == com.esri.core.geometry.Geometry.GeometryType.Polygon);
				com.esri.core.geometry.Polygon poly = (com.esri.core.geometry.Polygon)(result);
				com.esri.core.geometry.Envelope2D env2D = new com.esri.core.geometry.Envelope2D();
				result.QueryEnvelope2D(env2D);
				NUnit.Framework.Assert.IsTrue(System.Math.Abs(env2D.GetWidth() - 8 - 50) < 0.1 && System.Math.Abs(env2D.GetHeight() - 8 - 50) < 0.1);
				NUnit.Framework.Assert.IsTrue(System.Math.Abs(env2D.GetCenterX() - 25) < 0.1 && System.Math.Abs(env2D.GetCenterY() - 25) < 0.1);
				int pathCount = poly.GetPathCount();
				NUnit.Framework.Assert.IsTrue(pathCount == 1);
				int pointCount = poly.GetPointCount();
				NUnit.Framework.Assert.IsTrue(System.Math.Abs(pointCount - 208.0) < 10);
				NUnit.Framework.Assert.IsTrue(simplify.IsSimpleAsFeature(result, sr, null));
			}
		}

		[NUnit.Framework.Test]
		public virtual void TestBufferPolygon()
		{
			com.esri.core.geometry.SpatialReference sr = com.esri.core.geometry.SpatialReference.Create(4326);
			com.esri.core.geometry.Polygon inputGeom = new com.esri.core.geometry.Polygon();
			com.esri.core.geometry.OperatorBuffer buffer = (com.esri.core.geometry.OperatorBuffer)com.esri.core.geometry.OperatorFactoryLocal.GetInstance().GetOperator(com.esri.core.geometry.Operator.Type.Buffer);
			com.esri.core.geometry.OperatorSimplify simplify = (com.esri.core.geometry.OperatorSimplify)com.esri.core.geometry.OperatorFactoryLocal.GetInstance().GetOperator(com.esri.core.geometry.Operator.Type.Simplify);
			inputGeom.StartPath(0, 0);
			inputGeom.LineTo(50, 50);
			inputGeom.LineTo(50, 0);
			{
				com.esri.core.geometry.Geometry result = buffer.Execute(inputGeom, sr, 0, null);
				NUnit.Framework.Assert.IsTrue(result.GetType().Value() == com.esri.core.geometry.Geometry.GeometryType.Polygon);
				NUnit.Framework.Assert.IsTrue(result == inputGeom);
			}
			{
				com.esri.core.geometry.Geometry result = buffer.Execute(inputGeom, sr, 10, null);
				NUnit.Framework.Assert.IsTrue(result.GetType().Value() == com.esri.core.geometry.Geometry.GeometryType.Polygon);
				com.esri.core.geometry.Polygon poly = (com.esri.core.geometry.Polygon)(result);
				com.esri.core.geometry.Envelope2D env2D = new com.esri.core.geometry.Envelope2D();
				result.QueryEnvelope2D(env2D);
				NUnit.Framework.Assert.IsTrue(System.Math.Abs(env2D.GetWidth() - 20 - 50) < 0.1 && System.Math.Abs(env2D.GetHeight() - 20 - 50) < 0.1);
				NUnit.Framework.Assert.IsTrue(System.Math.Abs(env2D.GetCenterX() - 25) < 0.1 && System.Math.Abs(env2D.GetCenterY() - 25) < 0.1);
				int pathCount = poly.GetPathCount();
				NUnit.Framework.Assert.IsTrue(pathCount == 1);
				int pointCount = poly.GetPointCount();
				NUnit.Framework.Assert.IsTrue(System.Math.Abs(pointCount - 104.0) < 10);
				NUnit.Framework.Assert.IsTrue(simplify.IsSimpleAsFeature(result, sr, null));
			}
			{
				sr = com.esri.core.geometry.SpatialReference.Create(4326);
				inputGeom = new com.esri.core.geometry.Polygon();
				inputGeom.StartPath(0, 0);
				inputGeom.LineTo(50, 50);
				inputGeom.LineTo(50, 0);
				com.esri.core.geometry.Geometry result = buffer.Execute(inputGeom, sr, -10, null);
				NUnit.Framework.Assert.IsTrue(result.GetType().Value() == com.esri.core.geometry.Geometry.GeometryType.Polygon);
				com.esri.core.geometry.Polygon poly = (com.esri.core.geometry.Polygon)(result);
				com.esri.core.geometry.Envelope2D env2D = new com.esri.core.geometry.Envelope2D();
				result.QueryEnvelope2D(env2D);
				NUnit.Framework.Assert.IsTrue(System.Math.Abs(env2D.GetWidth() - 15.85) < 0.1 && System.Math.Abs(env2D.GetHeight() - 15.85) < 0.1);
				NUnit.Framework.Assert.IsTrue(System.Math.Abs(env2D.GetCenterX() - 32.07) < 0.1 && System.Math.Abs(env2D.GetCenterY() - 17.93) < 0.1);
				int pathCount = poly.GetPathCount();
				NUnit.Framework.Assert.IsTrue(pathCount == 1);
				int pointCount = poly.GetPointCount();
				NUnit.Framework.Assert.IsTrue(pointCount == 3);
				NUnit.Framework.Assert.IsTrue(simplify.IsSimpleAsFeature(result, sr, null));
			}
			{
				sr = com.esri.core.geometry.SpatialReference.Create(4326);
				inputGeom = new com.esri.core.geometry.Polygon();
				inputGeom.StartPath(0, 0);
				inputGeom.LineTo(0, 50);
				inputGeom.LineTo(50, 50);
				inputGeom.LineTo(50, 0);
				inputGeom.StartPath(10, 10);
				inputGeom.LineTo(40, 10);
				inputGeom.LineTo(40, 40);
				inputGeom.LineTo(10, 40);
				com.esri.core.geometry.Geometry result = buffer.Execute(inputGeom, sr, -2, null);
				NUnit.Framework.Assert.IsTrue(result.GetType().Value() == com.esri.core.geometry.Geometry.GeometryType.Polygon);
				com.esri.core.geometry.Polygon poly = (com.esri.core.geometry.Polygon)(result);
				com.esri.core.geometry.Envelope2D env2D = new com.esri.core.geometry.Envelope2D();
				result.QueryEnvelope2D(env2D);
				NUnit.Framework.Assert.IsTrue(System.Math.Abs(env2D.GetWidth() + 4 - 50) < 0.1 && System.Math.Abs(env2D.GetHeight() + 4 - 50) < 0.1);
				NUnit.Framework.Assert.IsTrue(System.Math.Abs(env2D.GetCenterX() - 25) < 0.1 && System.Math.Abs(env2D.GetCenterY() - 25) < 0.1);
				int pathCount = poly.GetPathCount();
				NUnit.Framework.Assert.IsTrue(pathCount == 2);
				int pointCount = poly.GetPointCount();
				NUnit.Framework.Assert.IsTrue(System.Math.Abs(pointCount - 108) < 10);
				NUnit.Framework.Assert.IsTrue(simplify.IsSimpleAsFeature(result, sr, null));
			}
		}
	}
}
