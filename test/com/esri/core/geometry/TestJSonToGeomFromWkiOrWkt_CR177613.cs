using Sharpen;

namespace com.esri.core.geometry
{
	public class TestJSonToGeomFromWkiOrWkt_CR177613 : NUnit.Framework.TestCase
	{
		internal org.codehaus.jackson.JsonFactory factory = new org.codehaus.jackson.JsonFactory();

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

		/// <exception cref="org.codehaus.jackson.JsonParseException"/>
		/// <exception cref="System.IO.IOException"/>
		[NUnit.Framework.Test]
		public virtual void TestPolygonWithEmptyWKT_NoWKI()
		{
			string jsonStringPg = "{ \"rings\" :[  [ [-97.06138,32.837], [-97.06133,32.836], " + "[-97.06124,32.834], [-97.06127,32.832], [-97.06138,32.837] ],  " + "[ [-97.06326,32.759], [-97.06298,32.755], [-97.06153,32.749], [-97.06326,32.759] ]], " + "\"spatialReference\" : {\"wkt\" : \"\"}}";
			org.codehaus.jackson.JsonParser jsonParserPg = factory.CreateJsonParser(jsonStringPg);
			jsonParserPg.NextToken();
			com.esri.core.geometry.MapGeometry mapGeom = com.esri.core.geometry.GeometryEngine.JsonToGeometry(jsonParserPg);
			com.esri.core.geometry.Utils.ShowProjectedGeometryInfo(mapGeom);
			com.esri.core.geometry.SpatialReference sr = mapGeom.GetSpatialReference();
			NUnit.Framework.Assert.IsTrue(sr == null);
		}

		/// <exception cref="org.codehaus.jackson.JsonParseException"/>
		/// <exception cref="System.IO.IOException"/>
		[NUnit.Framework.Test]
		public virtual void TestOnlyWKI()
		{
			string jsonStringSR = "{\"wkid\" : 4326}";
			org.codehaus.jackson.JsonParser jsonParserSR = factory.CreateJsonParser(jsonStringSR);
			jsonParserSR.NextToken();
			com.esri.core.geometry.MapGeometry mapGeom = com.esri.core.geometry.GeometryEngine.JsonToGeometry(jsonParserSR);
			com.esri.core.geometry.Utils.ShowProjectedGeometryInfo(mapGeom);
			com.esri.core.geometry.SpatialReference sr = mapGeom.GetSpatialReference();
			NUnit.Framework.Assert.IsTrue(sr == null);
		}

		/// <exception cref="System.Exception"/>
		[NUnit.Framework.Test]
		public virtual void TestMP2onCR175871()
		{
			com.esri.core.geometry.Polygon pg = new com.esri.core.geometry.Polygon();
			pg.StartPath(-50, 10);
			pg.LineTo(-50, 12);
			pg.LineTo(-45, 12);
			pg.LineTo(-45, 10);
			com.esri.core.geometry.Polygon pg1 = new com.esri.core.geometry.Polygon();
			pg1.StartPath(-45, 10);
			pg1.LineTo(-40, 10);
			pg1.LineTo(-40, 8);
			pg.Add(pg1, false);
			try
			{
				string jSonStr = com.esri.core.geometry.GeometryEngine.GeometryToJson(4326, pg);
				org.codehaus.jackson.JsonFactory jf = new org.codehaus.jackson.JsonFactory();
				org.codehaus.jackson.JsonParser jp = jf.CreateJsonParser(jSonStr);
				jp.NextToken();
				com.esri.core.geometry.MapGeometry mg = com.esri.core.geometry.GeometryEngine.JsonToGeometry(jp);
				com.esri.core.geometry.Geometry gm = mg.GetGeometry();
				NUnit.Framework.Assert.AreEqual(com.esri.core.geometry.Geometry.Type.Polygon, gm.GetType());
				com.esri.core.geometry.Polygon pgNew = (com.esri.core.geometry.Polygon)gm;
				NUnit.Framework.Assert.AreEqual(pgNew.GetPathCount(), pg.GetPathCount());
				NUnit.Framework.Assert.AreEqual(pgNew.GetPointCount(), pg.GetPointCount());
				NUnit.Framework.Assert.AreEqual(pgNew.GetSegmentCount(), pg.GetSegmentCount());
				NUnit.Framework.Assert.AreEqual(pgNew.GetPoint(0).GetX(), pg.GetPoint(0).GetX(), 0.000000001);
				NUnit.Framework.Assert.AreEqual(pgNew.GetPoint(1).GetX(), pg.GetPoint(1).GetX(), 0.000000001);
				NUnit.Framework.Assert.AreEqual(pgNew.GetPoint(2).GetX(), pg.GetPoint(2).GetX(), 0.000000001);
				NUnit.Framework.Assert.AreEqual(pgNew.GetPoint(3).GetX(), pg.GetPoint(3).GetX(), 0.000000001);
				NUnit.Framework.Assert.AreEqual(pgNew.GetPoint(0).GetY(), pg.GetPoint(0).GetY(), 0.000000001);
				NUnit.Framework.Assert.AreEqual(pgNew.GetPoint(1).GetY(), pg.GetPoint(1).GetY(), 0.000000001);
				NUnit.Framework.Assert.AreEqual(pgNew.GetPoint(2).GetY(), pg.GetPoint(2).GetY(), 0.000000001);
				NUnit.Framework.Assert.AreEqual(pgNew.GetPoint(3).GetY(), pg.GetPoint(3).GetY(), 0.000000001);
			}
			catch (System.Exception ex)
			{
				string err = ex.Message;
				System.Console.Out.Write(err);
				throw;
			}
		}

		/// <exception cref="org.codehaus.jackson.JsonParseException"/>
		/// <exception cref="System.IO.IOException"/>
		public static int FromJsonToWkid(org.codehaus.jackson.JsonParser parser)
		{
			int wkid = 0;
			if (parser.GetCurrentToken() != org.codehaus.jackson.JsonToken.START_OBJECT)
			{
				return 0;
			}
			while (parser.NextToken() != org.codehaus.jackson.JsonToken.END_OBJECT)
			{
				string fieldName = parser.GetCurrentName();
				if ("wkid".Equals(fieldName))
				{
					parser.NextToken();
					wkid = parser.GetIntValue();
				}
			}
			return wkid;
		}
	}
}
