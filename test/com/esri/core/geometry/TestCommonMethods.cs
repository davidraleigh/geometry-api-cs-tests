using Sharpen;

namespace com.esri.core.geometry
{
	public class TestCommonMethods : NUnit.Framework.TestCase
	{
		public static bool CompareDouble(double a, double b, double tol)
		{
			double diff = System.Math.Abs(a - b);
			return diff <= tol * System.Math.Abs(a) + tol;
		}

		public static com.esri.core.geometry.Point[] PointsFromMultiPath(com.esri.core.geometry.MultiPath geom)
		{
			int numberOfPoints = geom.GetPointCount();
			com.esri.core.geometry.Point[] points = new com.esri.core.geometry.Point[numberOfPoints];
			for (int i = 0; i < geom.GetPointCount(); i++)
			{
				points[i] = geom.GetPoint(i);
			}
			return points;
		}

		[NUnit.Framework.Test]
		public static void CompareGeometryContent(com.esri.core.geometry.MultiVertexGeometry geom1, com.esri.core.geometry.MultiVertexGeometry geom2)
		{
			// Geometry types
			NUnit.Framework.Assert.IsTrue(geom1.GetType().Value() == geom2.GetType().Value());
			// Envelopes
			com.esri.core.geometry.Envelope2D env1 = new com.esri.core.geometry.Envelope2D();
			geom1.QueryEnvelope2D(env1);
			com.esri.core.geometry.Envelope2D env2 = new com.esri.core.geometry.Envelope2D();
			geom2.QueryEnvelope2D(env2);
			NUnit.Framework.Assert.IsTrue(env1.xmin == env2.xmin && env1.xmax == env2.xmax && env1.ymin == env2.ymin && env1.ymax == env2.ymax);
			int type = geom1.GetType().Value();
			if (type == com.esri.core.geometry.Geometry.GeometryType.Polyline || type == com.esri.core.geometry.Geometry.GeometryType.Polygon)
			{
				// Part Count
				int partCount1 = ((com.esri.core.geometry.MultiPath)geom1).GetPathCount();
				int partCount2 = ((com.esri.core.geometry.MultiPath)geom2).GetPathCount();
				NUnit.Framework.Assert.IsTrue(partCount1 == partCount2);
				// Part indices
				for (int i = 0; i < partCount1; i++)
				{
					int start1 = ((com.esri.core.geometry.MultiPath)geom1).GetPathStart(i);
					int start2 = ((com.esri.core.geometry.MultiPath)geom2).GetPathStart(i);
					NUnit.Framework.Assert.IsTrue(start1 == start2);
					int end1 = ((com.esri.core.geometry.MultiPath)geom1).GetPathEnd(i);
					int end2 = ((com.esri.core.geometry.MultiPath)geom2).GetPathEnd(i);
					NUnit.Framework.Assert.IsTrue(end1 == end2);
				}
			}
			// Point count
			int pointCount1 = geom1.GetPointCount();
			int pointCount2 = geom2.GetPointCount();
			NUnit.Framework.Assert.IsTrue(pointCount1 == pointCount2);
			if (type == com.esri.core.geometry.Geometry.GeometryType.MultiPoint || type == com.esri.core.geometry.Geometry.GeometryType.Polyline || type == com.esri.core.geometry.Geometry.GeometryType.Polygon)
			{
				// POSITION
				com.esri.core.geometry.AttributeStreamBase positionStream1 = ((com.esri.core.geometry.MultiVertexGeometryImpl)geom1._getImpl()).GetAttributeStreamRef(com.esri.core.geometry.VertexDescription.Semantics.POSITION);
				com.esri.core.geometry.AttributeStreamOfDbl position1 = (com.esri.core.geometry.AttributeStreamOfDbl)(positionStream1);
				com.esri.core.geometry.AttributeStreamBase positionStream2 = ((com.esri.core.geometry.MultiVertexGeometryImpl)geom2._getImpl()).GetAttributeStreamRef(com.esri.core.geometry.VertexDescription.Semantics.POSITION);
				com.esri.core.geometry.AttributeStreamOfDbl position2 = (com.esri.core.geometry.AttributeStreamOfDbl)(positionStream2);
				for (int i = 0; i < pointCount1; i++)
				{
					double x1 = position1.Read(2 * i);
					double x2 = position2.Read(2 * i);
					NUnit.Framework.Assert.IsTrue(x1 == x2);
					double y1 = position1.Read(2 * i + 1);
					double y2 = position2.Read(2 * i + 1);
					NUnit.Framework.Assert.IsTrue(y1 == y2);
				}
				// Zs
				bool bHasZs1 = geom1.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.Z);
				bool bHasZs2 = geom2.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.Z);
				NUnit.Framework.Assert.IsTrue(bHasZs1 == bHasZs2);
				if (bHasZs1)
				{
					com.esri.core.geometry.AttributeStreamBase zStream1 = ((com.esri.core.geometry.MultiVertexGeometryImpl)geom1._getImpl()).GetAttributeStreamRef(com.esri.core.geometry.VertexDescription.Semantics.Z);
					com.esri.core.geometry.AttributeStreamOfDbl zs1 = (com.esri.core.geometry.AttributeStreamOfDbl)(zStream1);
					com.esri.core.geometry.AttributeStreamBase zStream2 = ((com.esri.core.geometry.MultiVertexGeometryImpl)geom2._getImpl()).GetAttributeStreamRef(com.esri.core.geometry.VertexDescription.Semantics.Z);
					com.esri.core.geometry.AttributeStreamOfDbl zs2 = (com.esri.core.geometry.AttributeStreamOfDbl)(zStream2);
					for (int i_1 = 0; i_1 < pointCount1; i_1++)
					{
						double z1 = zs1.Read(i_1);
						double z2 = zs2.Read(i_1);
						NUnit.Framework.Assert.IsTrue(z1 == z2);
					}
				}
				// Ms
				bool bHasMs1 = geom1.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.M);
				bool bHasMs2 = geom2.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.M);
				NUnit.Framework.Assert.IsTrue(bHasMs1 == bHasMs2);
				if (bHasMs1)
				{
					com.esri.core.geometry.AttributeStreamBase mStream1 = ((com.esri.core.geometry.MultiVertexGeometryImpl)geom1._getImpl()).GetAttributeStreamRef(com.esri.core.geometry.VertexDescription.Semantics.M);
					com.esri.core.geometry.AttributeStreamOfDbl ms1 = (com.esri.core.geometry.AttributeStreamOfDbl)(mStream1);
					com.esri.core.geometry.AttributeStreamBase mStream2 = ((com.esri.core.geometry.MultiVertexGeometryImpl)geom2._getImpl()).GetAttributeStreamRef(com.esri.core.geometry.VertexDescription.Semantics.M);
					com.esri.core.geometry.AttributeStreamOfDbl ms2 = (com.esri.core.geometry.AttributeStreamOfDbl)(mStream2);
					for (int i_1 = 0; i_1 < pointCount1; i_1++)
					{
						double m1 = ms1.Read(i_1);
						double m2 = ms2.Read(i_1);
						NUnit.Framework.Assert.IsTrue(m1 == m2);
					}
				}
				// IDs
				bool bHasIDs1 = geom1.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.ID);
				bool bHasIDs2 = geom2.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.ID);
				NUnit.Framework.Assert.IsTrue(bHasIDs1 == bHasIDs2);
				if (bHasIDs1)
				{
					com.esri.core.geometry.AttributeStreamBase idStream1 = ((com.esri.core.geometry.MultiVertexGeometryImpl)geom1._getImpl()).GetAttributeStreamRef(com.esri.core.geometry.VertexDescription.Semantics.ID);
					com.esri.core.geometry.AttributeStreamOfInt32 ids1 = (com.esri.core.geometry.AttributeStreamOfInt32)(idStream1);
					com.esri.core.geometry.AttributeStreamBase idStream2 = ((com.esri.core.geometry.MultiVertexGeometryImpl)geom2._getImpl()).GetAttributeStreamRef(com.esri.core.geometry.VertexDescription.Semantics.ID);
					com.esri.core.geometry.AttributeStreamOfInt32 ids2 = (com.esri.core.geometry.AttributeStreamOfInt32)(idStream2);
					for (int i_1 = 0; i_1 < pointCount1; i_1++)
					{
						int id1 = ids1.Read(i_1);
						int id2 = ids2.Read(i_1);
						NUnit.Framework.Assert.IsTrue(id1 == id2);
					}
				}
			}
		}

		[NUnit.Framework.Test]
		public static void CompareGeometryContent(com.esri.core.geometry.MultiPoint geom1, com.esri.core.geometry.MultiPoint geom2)
		{
			// Geometry types
			NUnit.Framework.Assert.IsTrue(geom1.GetType().Value() == geom2.GetType().Value());
			// Envelopes
			com.esri.core.geometry.Envelope env1 = new com.esri.core.geometry.Envelope();
			geom1.QueryEnvelope(env1);
			com.esri.core.geometry.Envelope env2 = new com.esri.core.geometry.Envelope();
			geom2.QueryEnvelope(env2);
			NUnit.Framework.Assert.IsTrue(env1.GetXMin() == env2.GetXMin() && env1.GetXMax() == env2.GetXMax() && env1.GetYMin() == env2.GetYMin() && env1.GetYMax() == env2.GetYMax());
			// Point count
			int pointCount1 = geom1.GetPointCount();
			int pointCount2 = geom2.GetPointCount();
			NUnit.Framework.Assert.IsTrue(pointCount1 == pointCount2);
			com.esri.core.geometry.Point point1;
			com.esri.core.geometry.Point point2;
			for (int i = 0; i < pointCount1; i++)
			{
				point1 = geom1.GetPoint(i);
				point2 = geom2.GetPoint(i);
				double x1 = point1.GetX();
				double x2 = point2.GetX();
				NUnit.Framework.Assert.IsTrue(x1 == x2);
				double y1 = point1.GetY();
				double y2 = point2.GetY();
				NUnit.Framework.Assert.IsTrue(y1 == y2);
			}
		}

		[NUnit.Framework.Test]
		public static void TestNothing()
		{
		}

		public static bool WriteObjectToFile(string fileName, object obj)
		{
			try
			{
				java.io.File f = new java.io.File(fileName);
				f.SetWritable(true);
				java.io.FileOutputStream fout = new java.io.FileOutputStream(f);
				java.io.ObjectOutputStream oo = new java.io.ObjectOutputStream(fout);
				oo.WriteObject(obj);
				fout.Close();
				return true;
			}
			catch (System.Exception)
			{
				return false;
			}
		}

		public static object ReadObjectFromFile(string fileName)
		{
			try
			{
				java.io.File f = new java.io.File(fileName);
				f.SetReadable(true);
				java.io.FileInputStream streamIn = new java.io.FileInputStream(f);
				java.io.ObjectInputStream ii = new java.io.ObjectInputStream(streamIn);
				object obj = ii.ReadObject();
				streamIn.Close();
				return obj;
			}
			catch (System.Exception)
			{
				return null;
			}
		}

		public static com.esri.core.geometry.MapGeometry FromJson(string jsonString)
		{
			org.codehaus.jackson.JsonFactory factory = new org.codehaus.jackson.JsonFactory();
			try
			{
				org.codehaus.jackson.JsonParser jsonParser = factory.CreateJsonParser(jsonString);
				jsonParser.NextToken();
				com.esri.core.geometry.OperatorImportFromJson importer = (com.esri.core.geometry.OperatorImportFromJson)com.esri.core.geometry.OperatorFactoryLocal.GetInstance().GetOperator(com.esri.core.geometry.Operator.Type.ImportFromJson);
				return importer.Execute(com.esri.core.geometry.Geometry.Type.Unknown, jsonParser);
			}
			catch (System.Exception)
			{
			}
			return null;
		}
	}
}