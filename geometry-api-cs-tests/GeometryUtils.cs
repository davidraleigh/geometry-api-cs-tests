

namespace com.esri.core.geometry
{
	public class GeometryUtils
	{
		public static string GetGeometryType(com.esri.core.geometry.Geometry geomIn)
		{
			// there are five types: esriGeometryPoint
			// esriGeometryMultipoint
			// esriGeometryPolyline
			// esriGeometryPolygon
			// esriGeometryEnvelope
			if (geomIn is com.esri.core.geometry.Point)
			{
				return "esriGeometryPoint";
			}
			if (geomIn is com.esri.core.geometry.MultiPoint)
			{
				return "esriGeometryMultipoint";
			}
			if (geomIn is com.esri.core.geometry.Polyline)
			{
				return "esriGeometryPolyline";
			}
			if (geomIn is com.esri.core.geometry.Polygon)
			{
				return "esriGeometryPolygon";
			}
			if (geomIn is com.esri.core.geometry.Envelope)
			{
				return "esriGeometryEnvelope";
			}
			else
			{
				return null;
			}
		}

		internal static com.esri.core.geometry.Geometry GetGeometryFromJSon(string jsonStr)
		{
			org.codehaus.jackson.JsonFactory jf = new org.codehaus.jackson.JsonFactory();
			try
			{
				org.codehaus.jackson.JsonParser jp = jf.CreateJsonParser(jsonStr);
				jp.NextToken();
				com.esri.core.geometry.Geometry geom = com.esri.core.geometry.GeometryEngine.JsonToGeometry(jp).GetGeometry();
				return geom;
			}
			catch (System.Exception)
			{
				return null;
			}
		}

		public enum SpatialRelationType
		{
			esriGeometryRelationCross,
			esriGeometryRelationDisjoint,
			esriGeometryRelationIn,
			esriGeometryRelationInteriorIntersection,
			esriGeometryRelationIntersection,
			esriGeometryRelationLineCoincidence,
			esriGeometryRelationLineTouch,
			esriGeometryRelationOverlap,
			esriGeometryRelationPointTouch,
			esriGeometryRelationTouch,
			esriGeometryRelationWithin,
			esriGeometryRelationRelation
		}

		internal static string GetJSonStringFromGeometry(com.esri.core.geometry.Geometry geomIn, com.esri.core.geometry.SpatialReference sr)
		{
			string jsonStr4Geom = com.esri.core.geometry.GeometryEngine.GeometryToJson(sr, geomIn);
			string jsonStrNew = "{\"geometryType\":\"" + GetGeometryType(geomIn) + "\",\"geometries\":[" + jsonStr4Geom + "]}";
			return jsonStrNew;
		}

		/// <exception cref="System.IO.FileNotFoundException"/>
		public static com.esri.core.geometry.Geometry LoadFromTextFileDbg(string textFileName)
		{
			string fullPath = textFileName;
			// string fullCSVPathName = System.IO.Path.Combine( directoryPath ,
			// CsvFileName);
			java.io.File fileInfo = new java.io.File(fullPath);
			java.util.Scanner scanner = new java.util.Scanner(fileInfo);
			com.esri.core.geometry.Geometry geom = null;
			// grab first line
			string line = scanner.NextLine();
			string geomTypeString = Sharpen.Runtime.Substring(line, 1);
			if (Sharpen.Runtime.EqualsIgnoreCase(geomTypeString, "polygon"))
			{
				geom = new com.esri.core.geometry.Polygon();
			}
			else
			{
				if (Sharpen.Runtime.EqualsIgnoreCase(geomTypeString, "polyline"))
				{
					geom = new com.esri.core.geometry.Polyline();
				}
				else
				{
					if (Sharpen.Runtime.EqualsIgnoreCase(geomTypeString, "multipoint"))
					{
						geom = new com.esri.core.geometry.MultiPoint();
					}
					else
					{
						if (Sharpen.Runtime.EqualsIgnoreCase(geomTypeString, "point"))
						{
							geom = new com.esri.core.geometry.Point();
						}
					}
				}
			}
			while (line.StartsWith("*"))
			{
				if (scanner.HasNextLine())
				{
					line = scanner.NextLine();
				}
			}
			int j = 0;
			com.esri.core.geometry.Geometry.Type geomType = geom.GetType();
			while (scanner.HasNextLine())
			{
				string[] parsedLine = line.Split("\\s+");
				double xVal = double.Parse(parsedLine[0]);
				double yVal = double.Parse(parsedLine[1]);
				if (j == 0 && (geomType == com.esri.core.geometry.Geometry.Type.Polygon || geomType == com.esri.core.geometry.Geometry.Type.Polyline))
				{
					((com.esri.core.geometry.MultiPath)geom).StartPath(xVal, yVal);
				}
				else
				{
					if (geomType == com.esri.core.geometry.Geometry.Type.Polygon || geomType == com.esri.core.geometry.Geometry.Type.Polyline)
					{
						((com.esri.core.geometry.MultiPath)geom).LineTo(xVal, yVal);
					}
					else
					{
						if (geomType == com.esri.core.geometry.Geometry.Type.MultiPoint)
						{
							((com.esri.core.geometry.MultiPoint)geom).Add(xVal, yVal);
						}
					}
				}
				// else if(geomType == Geometry.Type.Point)
				// Point geom = null;//new Point(xVal, yVal);
				j++;
				line = scanner.NextLine();
			}
			scanner.Close();
			return geom;
		}
	}
}
