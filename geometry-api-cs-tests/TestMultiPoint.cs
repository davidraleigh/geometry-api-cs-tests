using NUnit.Framework;

namespace com.esri.core.geometry
{
	public class TestMultiPoint : NUnit.Framework.TestFixtureAttribute
	{
		/// <exception cref="System.Exception"/>
		protected override void SetUp()
		{
			
		}

		/// <exception cref="System.Exception"/>
		protected override void TearDown()
		{
			
		}

		internal static void SimpleTest(com.esri.core.geometry.Geometry point)
		{
			NUnit.Framework.Assert.IsTrue(point != null);
			// point->AddAttribute(VertexDescription::Semantics::Z);
			// assertTrue(point->HasAttribute(VertexDescription::Semantics::POSITION));
			// assertTrue(point.->HasAttribute(VertexDescription::Semantics::Z));
			// point->AddAttribute(VertexDescription::Semantics::Z);//duplicate call
			// assertTrue(point->GetDescription()->GetAttributeCount() == 2);
			// assertTrue(point->GetDescription()->GetSemantics(1) ==
			// VertexDescription::Semantics::Z);
			// point->DropAttribute(VertexDescription::Semantics::Z);
			// assertFalse(point->HasAttribute(VertexDescription::Semantics::Z));
			// point->DropAttribute(VertexDescription::Semantics::Z);//duplicate
			// call
			// assertFalse(point->HasAttribute(VertexDescription::Semantics::Z));
			// assertTrue(point->GetDescription()->GetAttributeCount() == 1);
			// assertTrue(point->GetDescription()->GetSemantics(0) ==
			// VertexDescription::Semantics::POSITION);
			// point->AddAttribute(VertexDescription::Semantics::M);
			// assertTrue(point->HasAttribute(VertexDescription::Semantics::POSITION));
			// assertFalse(point->HasAttribute(VertexDescription::Semantics::Z));
			// assertTrue(point->HasAttribute(VertexDescription::Semantics::M));
			// point->DropAttribute(VertexDescription::Semantics::M);
			// assertFalse(point->HasAttribute(VertexDescription::Semantics::M));
			//
			// point->AddAttribute(VertexDescription::Semantics::ID);
			// assertTrue(point->HasAttribute(VertexDescription::Semantics::POSITION));
			// assertFalse(point->HasAttribute(VertexDescription::Semantics::Z));
			// assertFalse(point->HasAttribute(VertexDescription::Semantics::M));
			// point->DropAttribute(VertexDescription::Semantics::ID);
			// assertFalse(point->HasAttribute(VertexDescription::Semantics::ID));
			// /
			// assertTrue(point->IsEmpty());
			// assertTrue(point->GetPointCount() == 0);
			// assertTrue(point->GetPartCount() == 0);
			point = null;
			NUnit.Framework.Assert.IsFalse(point != null);
		}

		[NUnit.Framework.Test]
		public static void TestCreation()
		{
			{
				// simple create
				com.esri.core.geometry.MultiPoint mpoint = new com.esri.core.geometry.MultiPoint();
				NUnit.Framework.Assert.IsTrue(mpoint.GetType() == typeof(com.esri.core.geometry.MultiPoint));
				// assertFalse(mpoint.getClass() == Polyline.class);
				NUnit.Framework.Assert.IsTrue(mpoint != null);
				NUnit.Framework.Assert.IsTrue(mpoint.GetType() == com.esri.core.geometry.Geometry.Type.MultiPoint);
				NUnit.Framework.Assert.IsTrue(mpoint.IsEmpty());
				NUnit.Framework.Assert.IsTrue(mpoint.GetPointCount() == 0);
				mpoint = null;
				NUnit.Framework.Assert.IsFalse(mpoint != null);
			}
			{
				// play with default attributes
				com.esri.core.geometry.MultiPoint mpoint = new com.esri.core.geometry.MultiPoint();
				SimpleTest(mpoint);
			}
			{
				// simple create 2D
				com.esri.core.geometry.MultiPoint mpoint = new com.esri.core.geometry.MultiPoint();
				NUnit.Framework.Assert.IsTrue(mpoint != null);
				com.esri.core.geometry.MultiPoint mpoint1 = new com.esri.core.geometry.MultiPoint();
				NUnit.Framework.Assert.IsTrue(mpoint1 != null);
				mpoint.SetEmpty();
				com.esri.core.geometry.Point pt = new com.esri.core.geometry.Point(0, 0);
				mpoint.Add(pt);
				com.esri.core.geometry.Point pt3 = mpoint.GetPoint(0);
				NUnit.Framework.Assert.IsTrue(pt3.GetX() == 0 && pt3.GetY() == 0);
				// assertFalse(mpoint->HasAttribute(VertexDescription::Semantics::Z));
				// pt3.setZ(115.0);
				mpoint.SetPoint(0, pt3);
				pt3 = mpoint.GetPoint(0);
				NUnit.Framework.Assert.IsTrue(pt3.GetX() == 0 && pt3.GetY() == 0);
			}
			{
				/* && pt3.getZ() == 115 */
				// assertTrue(mpoint->HasAttribute(VertexDescription::Semantics::Z));
				// CompareGeometryContent(mpoint, &pt, 1);
				// move 3d
				com.esri.core.geometry.MultiPoint mpoint = new com.esri.core.geometry.MultiPoint();
				NUnit.Framework.Assert.IsTrue(mpoint != null);
				com.esri.core.geometry.Point pt = new com.esri.core.geometry.Point(0, 0);
				mpoint.Add(pt);
				com.esri.core.geometry.Point pt3 = mpoint.GetPoint(0);
				NUnit.Framework.Assert.IsTrue(pt3.GetX() == 0 && pt3.GetY() == 0);
			}
			{
				/* && pt3.getZ() == 0 */
				// test QueryInterval
				com.esri.core.geometry.MultiPoint mpoint = new com.esri.core.geometry.MultiPoint();
				com.esri.core.geometry.Point pt1 = new com.esri.core.geometry.Point(0.0, 0.0);
				// pt1.setZ(-1.0);
				com.esri.core.geometry.Point pt2 = new com.esri.core.geometry.Point(0.0, 0.0);
				// pt2.setZ(1.0);
				mpoint.Add(pt1);
				mpoint.Add(pt2);
				// Envelope1D e =
				// mpoint->QueryInterval(enum_value2(VertexDescription, Semantics,
				// Z), 0);
				com.esri.core.geometry.Envelope e = new com.esri.core.geometry.Envelope();
				mpoint.QueryEnvelope(e);
			}
			{
				// assertTrue(e.get == -1.0 && e.vmax == 1.0);
				com.esri.core.geometry.MultiPoint geom = new com.esri.core.geometry.MultiPoint();
			}
			{
				// int sz = sizeof(openString) / sizeof(openString[0]);
				// for (int i = 0; i < sz; i++)
				// geom.add(openString[i]);
				// CompareGeometryContent(geom, openString, sz);
				com.esri.core.geometry.MultiPoint geom = new com.esri.core.geometry.MultiPoint();
			}
			{
				// int sz = sizeof(openString) / sizeof(openString[0]);
				// Point point = GCNEW Point;
				// for (int i = 0; i < sz; i++)
				// {
				// point.setXY(openString[i]);
				// geom.add(point);
				// }
				// CompareGeometryContent(geom, openString, sz);
				// Test AddPoints
				com.esri.core.geometry.MultiPoint geom = new com.esri.core.geometry.MultiPoint();
			}
			{
				// int sz = sizeof(openString) / sizeof(openString[0]);
				// geom.addPoints(openString, sz, 0, -1);
				// CompareGeometryContent((MultiVertexGeometry)geom, openString,
				// sz);
				// Test InsertPoint(Point2D)
				com.esri.core.geometry.MultiPoint mpoint = new com.esri.core.geometry.MultiPoint();
				com.esri.core.geometry.Point pt0 = new com.esri.core.geometry.Point(0.0, 0.0);
				// pt0.setZ(-1.0);
				// pt0.setID(7);
				com.esri.core.geometry.Point pt1 = new com.esri.core.geometry.Point(0.0, 0.0);
				// pt1.setZ(1.0);
				// pt1.setID(11);
				com.esri.core.geometry.Point pt2 = new com.esri.core.geometry.Point(0.0, 1.0);
				// pt2.setZ(1.0);
				// pt2.setID(13);
				mpoint.Add(pt0);
				mpoint.Add(pt1);
				mpoint.Add(pt2);
				com.esri.core.geometry.Point pt3 = new com.esri.core.geometry.Point(-11.0, -13.0);
				mpoint.Add(pt3);
				mpoint.InsertPoint(1, pt3);
				NUnit.Framework.Assert.IsTrue(mpoint.GetPointCount() == 5);
				com.esri.core.geometry.Point pt;
				pt = mpoint.GetPoint(0);
				NUnit.Framework.Assert.IsTrue(pt.GetX() == pt0.GetX() && pt.GetY() == pt0.GetY());
				/*
				* &&
				* pt.
				* getZ
				* () ==
				* pt0
				* .getZ
				* ()
				*/
				pt = mpoint.GetPoint(1);
				NUnit.Framework.Assert.IsTrue(pt.GetX() == pt3.GetX() && pt.GetY() == pt3.GetY());
				pt = mpoint.GetPoint(2);
				NUnit.Framework.Assert.IsTrue(pt.GetX() == pt1.GetX() && pt.GetY() == pt1.GetY());
				/*
				* &&
				* pt.
				* getZ
				* () ==
				* pt1
				* .getZ
				* ()
				*/
				pt = mpoint.GetPoint(3);
				NUnit.Framework.Assert.IsTrue(pt.GetX() == pt2.GetX() && pt.GetY() == pt2.GetY());
				/*
				* &&
				* pt.
				* getZ
				* () ==
				* pt2
				* .getZ
				* ()
				*/
				com.esri.core.geometry.Point point = new com.esri.core.geometry.Point();
				point.SetXY(17.0, 19.0);
				// point.setID(12);
				// point.setM(5);
				mpoint.InsertPoint(2, point);
				mpoint.Add(point);
				NUnit.Framework.Assert.IsTrue(mpoint.GetPointCount() == 7);
			}
			// double m;
			// int id;
			// pt = mpoint.getXYZ(2);
			// assertTrue(pt.x == 17.0 && pt.y == 19.0 && pt.z == defaultZ);
			// m = mpoint.getAttributeAsDbl(enum_value2(VertexDescription,
			// Semantics, M), 2, 0);
			// assertTrue(m == 5);
			// id = mpoint.getAttributeAsInt(enum_value2(VertexDescription,
			// Semantics, ID), 2, 0);
			// assertTrue(id == 23);
			//
			// pt = mpoint.getXYZ(3);
			// assertTrue(pt.x == pt1.x && pt.y == pt1.y && pt.z == pt1.z);
			// m = mpoint.getAttributeAsDbl(enum_value2(VertexDescription,
			// Semantics, M), 3, 0);
			// assertTrue(NumberUtils::IsNaN(m));
			// id = mpoint.getAttributeAsInt(enum_value2(VertexDescription,
			// Semantics, ID), 3, 0);
			// assertTrue(id == 11);
			com.esri.core.geometry.MultiPoint mpoint_1 = new com.esri.core.geometry.MultiPoint();
			com.esri.core.geometry.Point pt0_1 = new com.esri.core.geometry.Point(0.0, 0.0, -1.0);
			com.esri.core.geometry.Point pt1_1 = new com.esri.core.geometry.Point(0.0, 0.0, 1.0);
			com.esri.core.geometry.Point pt2_1 = new com.esri.core.geometry.Point(0.0, 1.0, 1.0);
			mpoint_1.Add(pt0_1);
			mpoint_1.Add(pt1_1);
			mpoint_1.Add(pt2_1);
			mpoint_1.RemovePoint(1);
			com.esri.core.geometry.Point pt_1;
			pt_1 = mpoint_1.GetPoint(0);
			NUnit.Framework.Assert.IsTrue(pt_1.GetX() == pt0_1.GetX() && pt_1.GetY() == pt0_1.GetY());
			pt_1 = mpoint_1.GetPoint(1);
			NUnit.Framework.Assert.IsTrue(pt_1.GetX() == pt2_1.GetX() && pt_1.GetY() == pt2_1.GetY());
			NUnit.Framework.Assert.IsTrue(mpoint_1.GetPointCount() == 2);
		}

		[NUnit.Framework.Test]
		public static void TestCopy()
		{
			com.esri.core.geometry.MultiPoint mpoint = new com.esri.core.geometry.MultiPoint();
			com.esri.core.geometry.Point pt0 = new com.esri.core.geometry.Point(0.0, 0.0, -1.0);
			com.esri.core.geometry.Point pt1 = new com.esri.core.geometry.Point(0.0, 0.0, 1.0);
			com.esri.core.geometry.Point pt2 = new com.esri.core.geometry.Point(0.0, 1.0, 1.0);
			mpoint.Add(pt0);
			mpoint.Add(pt1);
			mpoint.Add(pt2);
			mpoint.RemovePoint(1);
			com.esri.core.geometry.MultiPoint mpCopy = (com.esri.core.geometry.MultiPoint)mpoint.Copy();
			NUnit.Framework.Assert.IsTrue(mpCopy.Equals(mpoint));
			com.esri.core.geometry.Point pt;
			pt = mpCopy.GetPoint(0);
			NUnit.Framework.Assert.IsTrue(pt.GetX() == pt0.GetX() && pt.GetY() == pt0.GetY());
			pt = mpCopy.GetPoint(1);
			NUnit.Framework.Assert.IsTrue(pt.GetX() == pt2.GetX() && pt.GetY() == pt2.GetY());
			NUnit.Framework.Assert.IsTrue(mpCopy.GetPointCount() == 2);
		}
	}
}
