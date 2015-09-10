/*
Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

For additional information, contact:
Environmental Systems Research Institute, Inc.
Attn: Contracts Dept
380 New York Street
Redlands, California, USA 92373

email: contracts@esri.com
*/


namespace com.esri.core.geometry
{
	public class TestGeomToGeoJson : NUnit.Framework.TestFixtureAttribute
	{
		internal com.esri.core.geometry.OperatorFactoryLocal factory = com.esri.core.geometry.OperatorFactoryLocal.GetInstance();

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
		public virtual void TestPoint()
		{
			com.esri.core.geometry.Point p = new com.esri.core.geometry.Point(10.0, 20.0);
			com.esri.core.geometry.OperatorExportToGeoJson exporter = (com.esri.core.geometry.OperatorExportToGeoJson)factory.GetOperator(com.esri.core.geometry.Operator.Type.ExportToGeoJson);
			string result = exporter.Execute(p);
			NUnit.Framework.Assert.AreEqual("{\"type\":\"Point\",\"coordinates\":[10.0,20.0]}", result);
		}

		[NUnit.Framework.Test]
		public virtual void TestEmptyPoint()
		{
			com.esri.core.geometry.Point p = new com.esri.core.geometry.Point();
			com.esri.core.geometry.OperatorExportToGeoJson exporter = (com.esri.core.geometry.OperatorExportToGeoJson)factory.GetOperator(com.esri.core.geometry.Operator.Type.ExportToGeoJson);
			string result = exporter.Execute(p);
			NUnit.Framework.Assert.AreEqual("{\"type\":\"Point\",\"coordinates\":null}", result);
		}

		[NUnit.Framework.Test]
		public virtual void TestPointGeometryEngine()
		{
			com.esri.core.geometry.Point p = new com.esri.core.geometry.Point(10.0, 20.0);
			string result = com.esri.core.geometry.GeometryEngine.GeometryToGeoJson(p);
			NUnit.Framework.Assert.AreEqual("{\"type\":\"Point\",\"coordinates\":[10.0,20.0]}", result);
		}

		[NUnit.Framework.Test]
		public virtual void TestOGCPoint()
		{
			com.esri.core.geometry.Point p = new com.esri.core.geometry.Point(10.0, 20.0);
			com.esri.core.geometry.ogc.OGCGeometry ogcPoint = new com.esri.core.geometry.ogc.OGCPoint(p, null);
			string result = ogcPoint.AsGeoJson();
			NUnit.Framework.Assert.AreEqual("{\"type\":\"Point\",\"coordinates\":[10.0,20.0]}", result);
		}

		[NUnit.Framework.Test]
		public virtual void TestMultiPoint()
		{
			com.esri.core.geometry.MultiPoint mp = new com.esri.core.geometry.MultiPoint();
			mp.Add(10.0, 20.0);
			mp.Add(20.0, 30.0);
			com.esri.core.geometry.OperatorExportToGeoJson exporter = (com.esri.core.geometry.OperatorExportToGeoJson)factory.GetOperator(com.esri.core.geometry.Operator.Type.ExportToGeoJson);
			string result = exporter.Execute(mp);
			NUnit.Framework.Assert.AreEqual("{\"type\":\"MultiPoint\",\"coordinates\":[[10.0,20.0],[20.0,30.0]]}", result);
		}

		[NUnit.Framework.Test]
		public virtual void TestEmptyMultiPoint()
		{
			com.esri.core.geometry.MultiPoint mp = new com.esri.core.geometry.MultiPoint();
			com.esri.core.geometry.OperatorExportToGeoJson exporter = (com.esri.core.geometry.OperatorExportToGeoJson)factory.GetOperator(com.esri.core.geometry.Operator.Type.ExportToGeoJson);
			string result = exporter.Execute(mp);
			NUnit.Framework.Assert.AreEqual("{\"type\":\"MultiPoint\",\"coordinates\":null}", result);
		}

		[NUnit.Framework.Test]
		public virtual void TestMultiPointGeometryEngine()
		{
			com.esri.core.geometry.MultiPoint mp = new com.esri.core.geometry.MultiPoint();
			mp.Add(10.0, 20.0);
			mp.Add(20.0, 30.0);
			string result = com.esri.core.geometry.GeometryEngine.GeometryToGeoJson(mp);
			NUnit.Framework.Assert.AreEqual("{\"type\":\"MultiPoint\",\"coordinates\":[[10.0,20.0],[20.0,30.0]]}", result);
		}

		[NUnit.Framework.Test]
		public virtual void TestOGCMultiPoint()
		{
			com.esri.core.geometry.MultiPoint mp = new com.esri.core.geometry.MultiPoint();
			mp.Add(10.0, 20.0);
			mp.Add(20.0, 30.0);
			com.esri.core.geometry.ogc.OGCMultiPoint ogcMultiPoint = new com.esri.core.geometry.ogc.OGCMultiPoint(mp, null);
			string result = ogcMultiPoint.AsGeoJson();
			NUnit.Framework.Assert.AreEqual("{\"type\":\"MultiPoint\",\"coordinates\":[[10.0,20.0],[20.0,30.0]]}", result);
		}

		[NUnit.Framework.Test]
		public virtual void TestPolyline()
		{
			com.esri.core.geometry.Polyline p = new com.esri.core.geometry.Polyline();
			p.StartPath(100.0, 0.0);
			p.LineTo(101.0, 0.0);
			p.LineTo(101.0, 1.0);
			p.LineTo(100.0, 1.0);
			com.esri.core.geometry.OperatorExportToGeoJson exporter = (com.esri.core.geometry.OperatorExportToGeoJson)factory.GetOperator(com.esri.core.geometry.Operator.Type.ExportToGeoJson);
			string result = exporter.Execute(p);
			NUnit.Framework.Assert.AreEqual("{\"type\":\"LineString\",\"coordinates\":[[100.0,0.0],[101.0,0.0],[101.0,1.0],[100.0,1.0]]}", result);
		}

		[NUnit.Framework.Test]
		public virtual void TestEmptyPolyline()
		{
			com.esri.core.geometry.Polyline p = new com.esri.core.geometry.Polyline();
			com.esri.core.geometry.OperatorExportToGeoJson exporter = (com.esri.core.geometry.OperatorExportToGeoJson)factory.GetOperator(com.esri.core.geometry.Operator.Type.ExportToGeoJson);
			string result = exporter.Execute(p);
			NUnit.Framework.Assert.AreEqual("{\"type\":\"LineString\",\"coordinates\":null}", result);
		}

		[NUnit.Framework.Test]
		public virtual void TestPolylineGeometryEngine()
		{
			com.esri.core.geometry.Polyline p = new com.esri.core.geometry.Polyline();
			p.StartPath(100.0, 0.0);
			p.LineTo(101.0, 0.0);
			p.LineTo(101.0, 1.0);
			p.LineTo(100.0, 1.0);
			string result = com.esri.core.geometry.GeometryEngine.GeometryToGeoJson(p);
			NUnit.Framework.Assert.AreEqual("{\"type\":\"LineString\",\"coordinates\":[[100.0,0.0],[101.0,0.0],[101.0,1.0],[100.0,1.0]]}", result);
		}

		[NUnit.Framework.Test]
		public virtual void TestOGCLineString()
		{
			com.esri.core.geometry.Polyline p = new com.esri.core.geometry.Polyline();
			p.StartPath(100.0, 0.0);
			p.LineTo(101.0, 0.0);
			p.LineTo(101.0, 1.0);
			p.LineTo(100.0, 1.0);
			com.esri.core.geometry.ogc.OGCLineString ogcLineString = new com.esri.core.geometry.ogc.OGCLineString(p, 0, null);
			string result = ogcLineString.AsGeoJson();
			NUnit.Framework.Assert.AreEqual("{\"type\":\"LineString\",\"coordinates\":[[100.0,0.0],[101.0,0.0],[101.0,1.0],[100.0,1.0]]}", result);
		}

		[NUnit.Framework.Test]
		public virtual void TestPolygon()
		{
			com.esri.core.geometry.Polygon p = new com.esri.core.geometry.Polygon();
			p.StartPath(100.0, 0.0);
			p.LineTo(101.0, 0.0);
			p.LineTo(101.0, 1.0);
			p.LineTo(100.0, 1.0);
			p.ClosePathWithLine();
			com.esri.core.geometry.OperatorExportToGeoJson exporter = (com.esri.core.geometry.OperatorExportToGeoJson)factory.GetOperator(com.esri.core.geometry.Operator.Type.ExportToGeoJson);
			string result = exporter.Execute(p);
			NUnit.Framework.Assert.AreEqual("{\"type\":\"Polygon\",\"coordinates\":[[[100.0,0.0],[101.0,0.0],[101.0,1.0],[100.0,1.0],[100.0,0.0]]]}", result);
		}

		[NUnit.Framework.Test]
		public virtual void TestPolygonWithHole()
		{
			com.esri.core.geometry.Polygon p = new com.esri.core.geometry.Polygon();
			//exterior ring - has to be clockwise for Esri
			p.StartPath(100.0, 0.0);
			p.LineTo(100.0, 1.0);
			p.LineTo(101.0, 1.0);
			p.LineTo(101.0, 0.0);
			p.ClosePathWithLine();
			//hole - counterclockwise for Esri
			p.StartPath(100.2, 0.2);
			p.LineTo(100.8, 0.2);
			p.LineTo(100.8, 0.8);
			p.LineTo(100.2, 0.8);
			p.ClosePathWithLine();
			com.esri.core.geometry.OperatorExportToGeoJson exporter = (com.esri.core.geometry.OperatorExportToGeoJson)factory.GetOperator(com.esri.core.geometry.Operator.Type.ExportToGeoJson);
			string result = exporter.Execute(p);
			NUnit.Framework.Assert.AreEqual("{\"type\":\"Polygon\",\"coordinates\":[[[100.0,0.0],[100.0,1.0],[101.0,1.0],[101.0,0.0],[100.0,0.0]],[[100.2,0.2],[100.8,0.2],[100.8,0.8],[100.2,0.8],[100.2,0.2]]]}", result);
		}

		[NUnit.Framework.Test]
		public virtual void TestPolygonWithHoleReversed()
		{
			com.esri.core.geometry.Polygon p = new com.esri.core.geometry.Polygon();
			//exterior ring - has to be clockwise for Esri
			p.StartPath(100.0, 0.0);
			p.LineTo(100.0, 1.0);
			p.LineTo(101.0, 1.0);
			p.LineTo(101.0, 0.0);
			p.ClosePathWithLine();
			//hole - counterclockwise for Esri
			p.StartPath(100.2, 0.2);
			p.LineTo(100.8, 0.2);
			p.LineTo(100.8, 0.8);
			p.LineTo(100.2, 0.8);
			p.ClosePathWithLine();
			p.ReverseAllPaths();
			//make it reversed. Exterior ring - ccw, hole - cw.
			com.esri.core.geometry.OperatorExportToGeoJson exporter = (com.esri.core.geometry.OperatorExportToGeoJson)factory.GetOperator(com.esri.core.geometry.Operator.Type.ExportToGeoJson);
			string result = exporter.Execute(p);
			NUnit.Framework.Assert.AreEqual("{\"type\":\"Polygon\",\"coordinates\":[[[100.0,0.0],[101.0,0.0],[101.0,1.0],[100.0,1.0],[100.0,0.0]],[[100.2,0.2],[100.2,0.8],[100.8,0.8],[100.8,0.2],[100.2,0.2]]]}", result);
		}

		/// <exception cref="System.IO.IOException"/>
		[NUnit.Framework.Test]
		public virtual void TestMultiPolygon()
		{
			org.codehaus.jackson.JsonFactory jsonFactory = new org.codehaus.jackson.JsonFactory();
			string geoJsonPolygon = "{\"type\":\"MultiPolygon\",\"coordinates\":[[[[-100.0,-100.0],[-100.0,100.0],[100.0,100.0],[100.0,-100.0],[-100.0,-100.0]],[[-90.0,-90.0],[90.0,90.0],[-90.0,90.0],[90.0,-90.0],[-90.0,-90.0]]],[[[-10.0,-10.0],[-10.0,10.0],[10.0,10.0],[10.0,-10.0],[-10.0,-10.0]]]]}";
			string esriJsonPolygon = "{\"rings\": [[[-100, -100], [-100, 100], [100, 100], [100, -100], [-100, -100]], [[-90, -90], [90, 90], [-90, 90], [90, -90], [-90, -90]], [[-10, -10], [-10, 10], [10, 10], [10, -10], [-10, -10]]]}";
			org.codehaus.jackson.JsonParser parser = jsonFactory.CreateJsonParser(esriJsonPolygon);
			com.esri.core.geometry.MapGeometry parsedPoly = com.esri.core.geometry.GeometryEngine.JsonToGeometry(parser);
			//MapGeometry parsedPoly = GeometryEngine.geometryFromGeoJson(jsonPolygon, 0, Geometry.Type.Polygon);
			com.esri.core.geometry.Polygon poly = (com.esri.core.geometry.Polygon)parsedPoly.GetGeometry();
			com.esri.core.geometry.OperatorExportToGeoJson exporter = (com.esri.core.geometry.OperatorExportToGeoJson)factory.GetOperator(com.esri.core.geometry.Operator.Type.ExportToGeoJson);
			//String result = exporter.execute(parsedPoly.getGeometry());
			string result = exporter.Execute(poly);
			NUnit.Framework.Assert.AreEqual(geoJsonPolygon, result);
		}

		/// <exception cref="org.json.JSONException"/>
		[NUnit.Framework.Test]
		public virtual void TestEmptyPolygon()
		{
			com.esri.core.geometry.Polygon p = new com.esri.core.geometry.Polygon();
			com.esri.core.geometry.OperatorExportToGeoJson exporter = (com.esri.core.geometry.OperatorExportToGeoJson)factory.GetOperator(com.esri.core.geometry.Operator.Type.ExportToGeoJson);
			string result = exporter.Execute(p);
			NUnit.Framework.Assert.AreEqual("{\"type\":\"Polygon\",\"coordinates\":null}", result);
			com.esri.core.geometry.MapGeometry imported = com.esri.core.geometry.OperatorImportFromGeoJson.Local().Execute(0, com.esri.core.geometry.Geometry.Type.Unknown, result, null);
			NUnit.Framework.Assert.IsTrue(imported.GetGeometry().IsEmpty());
			NUnit.Framework.Assert.IsTrue(imported.GetGeometry().GetType() == com.esri.core.geometry.Geometry.Type.Polygon);
		}

		[NUnit.Framework.Test]
		public virtual void TestPolygonGeometryEngine()
		{
			com.esri.core.geometry.Polygon p = new com.esri.core.geometry.Polygon();
			p.StartPath(100.0, 0.0);
			p.LineTo(101.0, 0.0);
			p.LineTo(101.0, 1.0);
			p.LineTo(100.0, 1.0);
			p.ClosePathWithLine();
			string result = com.esri.core.geometry.GeometryEngine.GeometryToGeoJson(p);
			NUnit.Framework.Assert.AreEqual("{\"type\":\"Polygon\",\"coordinates\":[[[100.0,0.0],[101.0,0.0],[101.0,1.0],[100.0,1.0],[100.0,0.0]]]}", result);
		}

		[NUnit.Framework.Test]
		public virtual void TestOGCPolygon()
		{
			com.esri.core.geometry.Polygon p = new com.esri.core.geometry.Polygon();
			p.StartPath(100.0, 0.0);
			p.LineTo(101.0, 0.0);
			p.LineTo(101.0, 1.0);
			p.LineTo(100.0, 1.0);
			p.ClosePathWithLine();
			com.esri.core.geometry.ogc.OGCPolygon ogcPolygon = new com.esri.core.geometry.ogc.OGCPolygon(p, null);
			string result = ogcPolygon.AsGeoJson();
			NUnit.Framework.Assert.AreEqual("{\"type\":\"Polygon\",\"coordinates\":[[[100.0,0.0],[101.0,0.0],[101.0,1.0],[100.0,1.0],[100.0,0.0]]]}", result);
		}

		[NUnit.Framework.Test]
		public virtual void TestPolygonWithHoleGeometryEngine()
		{
			com.esri.core.geometry.Polygon p = new com.esri.core.geometry.Polygon();
			p.StartPath(100.0, 0.0);
			//clockwise exterior
			p.LineTo(100.0, 1.0);
			p.LineTo(101.0, 1.0);
			p.LineTo(101.0, 0.0);
			p.ClosePathWithLine();
			p.StartPath(100.2, 0.2);
			//counterclockwise hole
			p.LineTo(100.8, 0.2);
			p.LineTo(100.8, 0.8);
			p.LineTo(100.2, 0.8);
			p.ClosePathWithLine();
			string result = com.esri.core.geometry.GeometryEngine.GeometryToGeoJson(p);
			NUnit.Framework.Assert.AreEqual("{\"type\":\"Polygon\",\"coordinates\":[[[100.0,0.0],[100.0,1.0],[101.0,1.0],[101.0,0.0],[100.0,0.0]],[[100.2,0.2],[100.8,0.2],[100.8,0.8],[100.2,0.8],[100.2,0.2]]]}", result);
		}

		[NUnit.Framework.Test]
		public virtual void TestPolylineWithTwoPaths()
		{
			com.esri.core.geometry.Polyline p = new com.esri.core.geometry.Polyline();
			p.StartPath(100.0, 0.0);
			p.LineTo(100.0, 1.0);
			p.StartPath(100.2, 0.2);
			p.LineTo(100.8, 0.2);
			string result = com.esri.core.geometry.GeometryEngine.GeometryToGeoJson(p);
			NUnit.Framework.Assert.AreEqual("{\"type\":\"MultiLineString\",\"coordinates\":[[[100.0,0.0],[100.0,1.0]],[[100.2,0.2],[100.8,0.2]]]}", result);
		}

		[NUnit.Framework.Test]
		public virtual void TestOGCPolygonWithHole()
		{
			com.esri.core.geometry.Polygon p = new com.esri.core.geometry.Polygon();
			p.StartPath(100.0, 0.0);
			p.LineTo(100.0, 1.0);
			p.LineTo(101.0, 1.0);
			p.LineTo(101.0, 0.0);
			p.ClosePathWithLine();
			p.StartPath(100.2, 0.2);
			p.LineTo(100.8, 0.2);
			p.LineTo(100.8, 0.8);
			p.LineTo(100.2, 0.8);
			p.ClosePathWithLine();
			com.esri.core.geometry.ogc.OGCPolygon ogcPolygon = new com.esri.core.geometry.ogc.OGCPolygon(p, null);
			string result = ogcPolygon.AsGeoJson();
			NUnit.Framework.Assert.AreEqual("{\"type\":\"Polygon\",\"coordinates\":[[[100.0,0.0],[100.0,1.0],[101.0,1.0],[101.0,0.0],[100.0,0.0]],[[100.2,0.2],[100.8,0.2],[100.8,0.8],[100.2,0.8],[100.2,0.2]]]}", result);
		}

		[NUnit.Framework.Test]
		public virtual void TestEnvelope()
		{
			com.esri.core.geometry.Envelope e = new com.esri.core.geometry.Envelope();
			e.SetCoords(-180.0, -90.0, 180.0, 90.0);
			com.esri.core.geometry.OperatorExportToGeoJson exporter = (com.esri.core.geometry.OperatorExportToGeoJson)factory.GetOperator(com.esri.core.geometry.Operator.Type.ExportToGeoJson);
			string result = exporter.Execute(e);
			NUnit.Framework.Assert.AreEqual("{\"bbox\":[-180.0,-90.0,180.0,90.0]}", result);
		}

		[NUnit.Framework.Test]
		public virtual void TestEmptyEnvelope()
		{
			com.esri.core.geometry.Envelope e = new com.esri.core.geometry.Envelope();
			com.esri.core.geometry.OperatorExportToGeoJson exporter = (com.esri.core.geometry.OperatorExportToGeoJson)factory.GetOperator(com.esri.core.geometry.Operator.Type.ExportToGeoJson);
			string result = exporter.Execute(e);
			NUnit.Framework.Assert.AreEqual("{\"bbox\":null}", result);
		}

		[NUnit.Framework.Test]
		public virtual void TestEnvelopeGeometryEngine()
		{
			com.esri.core.geometry.Envelope e = new com.esri.core.geometry.Envelope();
			e.SetCoords(-180.0, -90.0, 180.0, 90.0);
			string result = com.esri.core.geometry.GeometryEngine.GeometryToGeoJson(e);
			NUnit.Framework.Assert.AreEqual("{\"bbox\":[-180.0,-90.0,180.0,90.0]}", result);
		}
	}
}
