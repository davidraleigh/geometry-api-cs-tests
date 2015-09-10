

namespace com.esri.core.geometry
{
	public class TestSerialization : NUnit.Framework.TestFixtureAttribute
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
		public virtual void TestSerializePoint()
		{
			try
			{
				java.io.ByteArrayOutputStream streamOut = new java.io.ByteArrayOutputStream();
				java.io.ObjectOutputStream oo = new java.io.ObjectOutputStream(streamOut);
				com.esri.core.geometry.Point pt = new com.esri.core.geometry.Point(10, 40);
				oo.WriteObject(pt);
				System.IO.BinaryWriter streamIn = new System.IO.BinaryWriter(streamOut.ToByteArray());
				java.io.ObjectInputStream ii = new java.io.ObjectInputStream(streamIn);
				com.esri.core.geometry.Point ptRes = (com.esri.core.geometry.Point)ii.ReadObject();
				NUnit.Framework.Assert.IsTrue(ptRes.Equals(pt));
			}
			catch (System.Exception)
			{
				Fail("Point serialization failure");
			}
			// try
			// {
			// FileOutputStream streamOut = new FileOutputStream(m_thisDirectory +
			// "savedPoint.txt");
			// ObjectOutputStream oo = new ObjectOutputStream(streamOut);
			// Point pt = new Point(10, 40);
			// oo.writeObject(pt);
			// }
			// catch(Exception ex)
			// {
			// fail("Point serialization failure");
			// }
			try
			{
				java.io.InputStream s = typeof(com.esri.core.geometry.TestSerialization).GetResourceAsStream("savedPoint.txt");
				java.io.ObjectInputStream ii = new java.io.ObjectInputStream(s);
				com.esri.core.geometry.Point ptRes = (com.esri.core.geometry.Point)ii.ReadObject();
				NUnit.Framework.Assert.IsTrue(ptRes.GetX() == 10 && ptRes.GetY() == 40);
			}
			catch (System.Exception)
			{
				Fail("Point serialization failure");
			}
		}

		[NUnit.Framework.Test]
		public virtual void TestSerializePolygon()
		{
			try
			{
				java.io.ByteArrayOutputStream streamOut = new java.io.ByteArrayOutputStream();
				java.io.ObjectOutputStream oo = new java.io.ObjectOutputStream(streamOut);
				com.esri.core.geometry.Polygon pt = new com.esri.core.geometry.Polygon();
				pt.StartPath(10, 10);
				pt.LineTo(100, 100);
				pt.LineTo(200, 100);
				oo.WriteObject(pt);
				System.IO.BinaryWriter streamIn = new System.IO.BinaryWriter(streamOut.ToByteArray());
				java.io.ObjectInputStream ii = new java.io.ObjectInputStream(streamIn);
				com.esri.core.geometry.Polygon ptRes = (com.esri.core.geometry.Polygon)ii.ReadObject();
				NUnit.Framework.Assert.IsTrue(ptRes.Equals(pt));
			}
			catch (System.Exception)
			{
				Fail("Polygon serialization failure");
			}
			try
			{
				java.io.ByteArrayOutputStream streamOut = new java.io.ByteArrayOutputStream();
				java.io.ObjectOutputStream oo = new java.io.ObjectOutputStream(streamOut);
				com.esri.core.geometry.Polygon pt = new com.esri.core.geometry.Polygon();
				pt.StartPath(10, 10);
				pt.LineTo(100, 100);
				pt.LineTo(200, 100);
				pt = (com.esri.core.geometry.Polygon)com.esri.core.geometry.GeometryEngine.Simplify(pt, null);
				oo.WriteObject(pt);
				System.IO.BinaryWriter streamIn = new System.IO.BinaryWriter(streamOut.ToByteArray());
				java.io.ObjectInputStream ii = new java.io.ObjectInputStream(streamIn);
				com.esri.core.geometry.Polygon ptRes = (com.esri.core.geometry.Polygon)ii.ReadObject();
				NUnit.Framework.Assert.IsTrue(ptRes.Equals(pt));
			}
			catch (System.Exception)
			{
				Fail("Polygon serialization failure");
			}
			// try
			// {
			// FileOutputStream streamOut = new FileOutputStream(m_thisDirectory +
			// "savedPolygon.txt");
			// ObjectOutputStream oo = new ObjectOutputStream(streamOut);
			// Polygon pt = new Polygon();
			// pt.startPath(10, 10);
			// pt.lineTo(100, 100);
			// pt.lineTo(200, 100);
			// pt = (Polygon)GeometryEngine.simplify(pt, null);
			// oo.writeObject(pt);
			// }
			// catch(Exception ex)
			// {
			// fail("Polygon serialization failure");
			// }
			try
			{
				java.io.InputStream s = typeof(com.esri.core.geometry.TestSerialization).GetResourceAsStream("savedPolygon.txt");
				java.io.ObjectInputStream ii = new java.io.ObjectInputStream(s);
				com.esri.core.geometry.Polygon ptRes = (com.esri.core.geometry.Polygon)ii.ReadObject();
				NUnit.Framework.Assert.IsTrue(ptRes != null);
			}
			catch (System.Exception)
			{
				Fail("Polygon serialization failure");
			}
		}

		[NUnit.Framework.Test]
		public virtual void TestSerializePolyline()
		{
			try
			{
				java.io.ByteArrayOutputStream streamOut = new java.io.ByteArrayOutputStream();
				java.io.ObjectOutputStream oo = new java.io.ObjectOutputStream(streamOut);
				com.esri.core.geometry.Polyline pt = new com.esri.core.geometry.Polyline();
				pt.StartPath(10, 10);
				pt.LineTo(100, 100);
				pt.LineTo(200, 100);
				oo.WriteObject(pt);
				System.IO.BinaryWriter streamIn = new System.IO.BinaryWriter(streamOut.ToByteArray());
				java.io.ObjectInputStream ii = new java.io.ObjectInputStream(streamIn);
				com.esri.core.geometry.Polyline ptRes = (com.esri.core.geometry.Polyline)ii.ReadObject();
				NUnit.Framework.Assert.IsTrue(ptRes.Equals(pt));
			}
			catch (System.Exception)
			{
				Fail("Polyline serialization failure");
			}
			// try
			// {
			// FileOutputStream streamOut = new FileOutputStream(m_thisDirectory +
			// "savedPolyline.txt");
			// ObjectOutputStream oo = new ObjectOutputStream(streamOut);
			// Polyline pt = new Polyline();
			// pt.startPath(10, 10);
			// pt.lineTo(100, 100);
			// pt.lineTo(200, 100);
			// oo.writeObject(pt);
			// }
			// catch(Exception ex)
			// {
			// fail("Polyline serialization failure");
			// }
			try
			{
				java.io.InputStream s = typeof(com.esri.core.geometry.TestSerialization).GetResourceAsStream("savedPolyline.txt");
				java.io.ObjectInputStream ii = new java.io.ObjectInputStream(s);
				com.esri.core.geometry.Polyline ptRes = (com.esri.core.geometry.Polyline)ii.ReadObject();
				NUnit.Framework.Assert.IsTrue(ptRes != null);
			}
			catch (System.Exception)
			{
				Fail("Polyline serialization failure");
			}
		}

		[NUnit.Framework.Test]
		public virtual void TestSerializeEnvelope()
		{
			try
			{
				java.io.ByteArrayOutputStream streamOut = new java.io.ByteArrayOutputStream();
				java.io.ObjectOutputStream oo = new java.io.ObjectOutputStream(streamOut);
				com.esri.core.geometry.Envelope pt = new com.esri.core.geometry.Envelope(10, 10, 400, 300);
				oo.WriteObject(pt);
				System.IO.BinaryWriter streamIn = new System.IO.BinaryWriter(streamOut.ToByteArray());
				java.io.ObjectInputStream ii = new java.io.ObjectInputStream(streamIn);
				com.esri.core.geometry.Envelope ptRes = (com.esri.core.geometry.Envelope)ii.ReadObject();
				NUnit.Framework.Assert.IsTrue(ptRes.Equals(pt));
			}
			catch (System.Exception)
			{
				Fail("Envelope serialization failure");
			}
			// try
			// {
			// FileOutputStream streamOut = new FileOutputStream(m_thisDirectory +
			// "savedEnvelope.txt");
			// ObjectOutputStream oo = new ObjectOutputStream(streamOut);
			// Envelope pt = new Envelope(10, 10, 400, 300);
			// oo.writeObject(pt);
			// }
			// catch(Exception ex)
			// {
			// fail("Envelope serialization failure");
			// }
			try
			{
				java.io.InputStream s = typeof(com.esri.core.geometry.TestSerialization).GetResourceAsStream("savedEnvelope.txt");
				java.io.ObjectInputStream ii = new java.io.ObjectInputStream(s);
				com.esri.core.geometry.Envelope ptRes = (com.esri.core.geometry.Envelope)ii.ReadObject();
				NUnit.Framework.Assert.IsTrue(ptRes.GetXMax() == 400);
			}
			catch (System.Exception)
			{
				Fail("Envelope serialization failure");
			}
		}

		[NUnit.Framework.Test]
		public virtual void TestSerializeMultiPoint()
		{
			try
			{
				java.io.ByteArrayOutputStream streamOut = new java.io.ByteArrayOutputStream();
				java.io.ObjectOutputStream oo = new java.io.ObjectOutputStream(streamOut);
				com.esri.core.geometry.MultiPoint pt = new com.esri.core.geometry.MultiPoint();
				pt.Add(10, 30);
				pt.Add(120, 40);
				oo.WriteObject(pt);
				System.IO.BinaryWriter streamIn = new System.IO.BinaryWriter(streamOut.ToByteArray());
				java.io.ObjectInputStream ii = new java.io.ObjectInputStream(streamIn);
				com.esri.core.geometry.MultiPoint ptRes = (com.esri.core.geometry.MultiPoint)ii.ReadObject();
				NUnit.Framework.Assert.IsTrue(ptRes.Equals(pt));
			}
			catch (System.Exception)
			{
				Fail("MultiPoint serialization failure");
			}
			// try
			// {
			// FileOutputStream streamOut = new FileOutputStream(m_thisDirectory +
			// "savedMultiPoint.txt");
			// ObjectOutputStream oo = new ObjectOutputStream(streamOut);
			// MultiPoint pt = new MultiPoint();
			// pt.add(10, 30);
			// pt.add(120, 40);
			// oo.writeObject(pt);
			// }
			// catch(Exception ex)
			// {
			// fail("MultiPoint serialization failure");
			// }
			try
			{
				java.io.InputStream s = typeof(com.esri.core.geometry.TestSerialization).GetResourceAsStream("savedMultiPoint.txt");
				java.io.ObjectInputStream ii = new java.io.ObjectInputStream(s);
				com.esri.core.geometry.MultiPoint ptRes = (com.esri.core.geometry.MultiPoint)ii.ReadObject();
				NUnit.Framework.Assert.IsTrue(ptRes.GetPoint(1).GetY() == 40);
			}
			catch (System.Exception)
			{
				Fail("MultiPoint serialization failure");
			}
		}

		[NUnit.Framework.Test]
		public virtual void TestSerializeLine()
		{
			try
			{
				java.io.ByteArrayOutputStream streamOut = new java.io.ByteArrayOutputStream();
				java.io.ObjectOutputStream oo = new java.io.ObjectOutputStream(streamOut);
				com.esri.core.geometry.Line pt = new com.esri.core.geometry.Line();
				pt.SetStart(new com.esri.core.geometry.Point(10, 30));
				pt.SetEnd(new com.esri.core.geometry.Point(120, 40));
				oo.WriteObject(pt);
				System.IO.BinaryWriter streamIn = new System.IO.BinaryWriter(streamOut.ToByteArray());
				java.io.ObjectInputStream ii = new java.io.ObjectInputStream(streamIn);
				com.esri.core.geometry.Line ptRes = (com.esri.core.geometry.Line)ii.ReadObject();
				NUnit.Framework.Assert.IsTrue(ptRes.Equals(pt));
			}
			catch (System.Exception ex)
			{
				// fail("Line serialization failure");
				NUnit.Framework.Assert.AreEqual(ex.Message, "Cannot serialize this geometry");
			}
		}

		[NUnit.Framework.Test]
		public virtual void TestSerializeSR()
		{
			try
			{
				java.io.ByteArrayOutputStream streamOut = new java.io.ByteArrayOutputStream();
				java.io.ObjectOutputStream oo = new java.io.ObjectOutputStream(streamOut);
				com.esri.core.geometry.SpatialReference sr = com.esri.core.geometry.SpatialReference.Create(102100);
				oo.WriteObject(sr);
				System.IO.BinaryWriter streamIn = new System.IO.BinaryWriter(streamOut.ToByteArray());
				java.io.ObjectInputStream ii = new java.io.ObjectInputStream(streamIn);
				com.esri.core.geometry.SpatialReference ptRes = (com.esri.core.geometry.SpatialReference)ii.ReadObject();
				NUnit.Framework.Assert.IsTrue(ptRes.Equals(sr));
			}
			catch (System.Exception)
			{
				Fail("Spatial Reference serialization failure");
			}
		}

		[NUnit.Framework.Test]
		public virtual void TestSerializeEnvelope2D()
		{
			try
			{
				java.io.ByteArrayOutputStream streamOut = new java.io.ByteArrayOutputStream();
				java.io.ObjectOutputStream oo = new java.io.ObjectOutputStream(streamOut);
				com.esri.core.geometry.Envelope2D env = new com.esri.core.geometry.Envelope2D(1.213948734, 2.213948734, 11.213948734, 12.213948734);
				oo.WriteObject(env);
				System.IO.BinaryWriter streamIn = new System.IO.BinaryWriter(streamOut.ToByteArray());
				java.io.ObjectInputStream ii = new java.io.ObjectInputStream(streamIn);
				com.esri.core.geometry.Envelope2D envRes = (com.esri.core.geometry.Envelope2D)ii.ReadObject();
				NUnit.Framework.Assert.IsTrue(envRes.Equals(env));
			}
			catch (System.Exception)
			{
				Fail("Envelope2D serialization failure");
			}
			//		try
			//		{
			//			 FileOutputStream streamOut = new FileOutputStream(
			//			 "c:/temp/savedEnvelope2D.txt");
			//			 ObjectOutputStream oo = new ObjectOutputStream(streamOut);
			//			 Envelope2D e = new Envelope2D(177.123, 188.234, 999.122, 888.999);
			//			 oo.writeObject(e);
			//		 }
			//		 catch(Exception ex)
			//		 {
			//		   fail("Envelope2D serialization failure");
			//		 }
			try
			{
				java.io.InputStream s = typeof(com.esri.core.geometry.TestSerialization).GetResourceAsStream("savedEnvelope2D.txt");
				java.io.ObjectInputStream ii = new java.io.ObjectInputStream(s);
				com.esri.core.geometry.Envelope2D e = (com.esri.core.geometry.Envelope2D)ii.ReadObject();
				NUnit.Framework.Assert.IsTrue(e != null);
				NUnit.Framework.Assert.IsTrue(e.Equals(new com.esri.core.geometry.Envelope2D(177.123, 188.234, 999.122, 888.999)));
			}
			catch (System.Exception)
			{
				Fail("Envelope2D serialization failure");
			}
		}
	}
}
