using Sharpen;

namespace org.json
{
	/// <summary>
	/// This provides static methods to convert comma delimited text into a
	/// JSONArray, and to convert a JSONArray into comma delimited text.
	/// </summary>
	/// <remarks>
	/// This provides static methods to convert comma delimited text into a
	/// JSONArray, and to convert a JSONArray into comma delimited text. Comma
	/// delimited text is a very popular format for data interchange. It is
	/// understood by most database, spreadsheet, and organizer programs.
	/// <p>
	/// Each row of text represents a row in a table or a data record. Each row
	/// ends with a NEWLINE character. Each row contains one or more values.
	/// Values are separated by commas. A value can contain any character except
	/// for comma, unless is is wrapped in single quotes or double quotes.
	/// <p>
	/// The first row usually contains the names of the columns.
	/// <p>
	/// A comma delimited list can be converted into a JSONArray of JSONObjects.
	/// The names for the elements in the JSONObjects can be taken from the names
	/// in the first row.
	/// </remarks>
	/// <author>JSON.org</author>
	/// <version>2015-05-01</version>
	public class CDL
	{
		/*
		Copyright (c) 2002 JSON.org
		
		Permission is hereby granted, free of charge, to any person obtaining a copy
		of this software and associated documentation files (the "Software"), to deal
		in the Software without restriction, including without limitation the rights
		to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
		copies of the Software, and to permit persons to whom the Software is
		furnished to do so, subject to the following conditions:
		
		The above copyright notice and this permission notice shall be included in all
		copies or substantial portions of the Software.
		
		The Software shall be used for Good, not Evil.
		
		THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
		IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
		FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
		AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
		LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
		OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
		SOFTWARE.
		*/
		/// <summary>Get the next value.</summary>
		/// <remarks>
		/// Get the next value. The value can be wrapped in quotes. The value can
		/// be empty.
		/// </remarks>
		/// <param name="x">A JSONTokener of the source text.</param>
		/// <returns>The value string, or null if empty.</returns>
		/// <exception cref="JSONException">if the quoted string is badly formed.</exception>
		/// <exception cref="org.json.JSONException"/>
		private static string getValue(org.json.JSONTokener x)
		{
			char c;
			char q;
			System.Text.StringBuilder sb;
			do
			{
				c = x.next();
			}
			while (c == ' ' || c == '\t');
			switch (c)
			{
				case 0:
				{
					return null;
				}

				case '"':
				case '\'':
				{
					q = c;
					sb = new System.Text.StringBuilder();
					for (; ; )
					{
						c = x.next();
						if (c == q)
						{
							break;
						}
						if (c == 0 || c == '\n' || c == '\r')
						{
							throw x.syntaxError("Missing close quote '" + q + "'.");
						}
						sb.Append(c);
					}
					return sb.ToString();
				}

				case ',':
				{
					x.back();
					return string.Empty;
				}

				default:
				{
					x.back();
					return x.nextTo(',');
				}
			}
		}

		/// <summary>Produce a JSONArray of strings from a row of comma delimited values.</summary>
		/// <param name="x">A JSONTokener of the source text.</param>
		/// <returns>A JSONArray of strings.</returns>
		/// <exception cref="JSONException"/>
		/// <exception cref="org.json.JSONException"/>
		public static org.json.JSONArray rowToJSONArray(org.json.JSONTokener x)
		{
			org.json.JSONArray ja = new org.json.JSONArray();
			for (; ; )
			{
				string value = getValue(x);
				char c = x.next();
				if (value == null || (ja.length() == 0 && value.Length == 0 && c != ','))
				{
					return null;
				}
				ja.put(value);
				for (; ; )
				{
					if (c == ',')
					{
						break;
					}
					if (c != ' ')
					{
						if (c == '\n' || c == '\r' || c == 0)
						{
							return ja;
						}
						throw x.syntaxError("Bad character '" + c + "' (" + (int)c + ").");
					}
					c = x.next();
				}
			}
		}

		/// <summary>
		/// Produce a JSONObject from a row of comma delimited text, using a
		/// parallel JSONArray of strings to provides the names of the elements.
		/// </summary>
		/// <param name="names">
		/// A JSONArray of names. This is commonly obtained from the
		/// first row of a comma delimited text file using the rowToJSONArray
		/// method.
		/// </param>
		/// <param name="x">A JSONTokener of the source text.</param>
		/// <returns>A JSONObject combining the names and values.</returns>
		/// <exception cref="JSONException"/>
		/// <exception cref="org.json.JSONException"/>
		public static org.json.JSONObject rowToJSONObject(org.json.JSONArray names, org.json.JSONTokener
			 x)
		{
			org.json.JSONArray ja = rowToJSONArray(x);
			return ja != null ? ja.toJSONObject(names) : null;
		}

		/// <summary>Produce a comma delimited text row from a JSONArray.</summary>
		/// <remarks>
		/// Produce a comma delimited text row from a JSONArray. Values containing
		/// the comma character will be quoted. Troublesome characters may be
		/// removed.
		/// </remarks>
		/// <param name="ja">A JSONArray of strings.</param>
		/// <returns>A string ending in NEWLINE.</returns>
		public static string rowToString(org.json.JSONArray ja)
		{
			java.lang.StringBuilder sb = new java.lang.StringBuilder();
			for (int i = 0; i < ja.length(); i += 1)
			{
				if (i > 0)
				{
					sb.Append(',');
				}
				object @object = ja.opt(i);
				if (@object != null)
				{
					string @string = @object.ToString();
					if (@string.Length > 0 && (@string.IndexOf(',') >= 0 || @string.IndexOf('\n') >= 
						0 || @string.IndexOf('\r') >= 0 || @string.IndexOf(0) >= 0 || @string[0] == '"'))
					{
						sb.Append('"');
						int length = @string.Length;
						for (int j = 0; j < length; j += 1)
						{
							char c = @string[j];
							if (c >= ' ' && c != '"')
							{
								sb.Append(c);
							}
						}
						sb.Append('"');
					}
					else
					{
						sb.Append(@string);
					}
				}
			}
			sb.Append('\n');
			return sb.ToString();
		}

		/// <summary>
		/// Produce a JSONArray of JSONObjects from a comma delimited text string,
		/// using the first row as a source of names.
		/// </summary>
		/// <param name="string">The comma delimited text.</param>
		/// <returns>A JSONArray of JSONObjects.</returns>
		/// <exception cref="JSONException"/>
		/// <exception cref="org.json.JSONException"/>
		public static org.json.JSONArray toJSONArray(string @string)
		{
			return toJSONArray(new org.json.JSONTokener(@string));
		}

		/// <summary>
		/// Produce a JSONArray of JSONObjects from a comma delimited text string,
		/// using the first row as a source of names.
		/// </summary>
		/// <param name="x">The JSONTokener containing the comma delimited text.</param>
		/// <returns>A JSONArray of JSONObjects.</returns>
		/// <exception cref="JSONException"/>
		/// <exception cref="org.json.JSONException"/>
		public static org.json.JSONArray toJSONArray(org.json.JSONTokener x)
		{
			return toJSONArray(rowToJSONArray(x), x);
		}

		/// <summary>
		/// Produce a JSONArray of JSONObjects from a comma delimited text string
		/// using a supplied JSONArray as the source of element names.
		/// </summary>
		/// <param name="names">A JSONArray of strings.</param>
		/// <param name="string">The comma delimited text.</param>
		/// <returns>A JSONArray of JSONObjects.</returns>
		/// <exception cref="JSONException"/>
		/// <exception cref="org.json.JSONException"/>
		public static org.json.JSONArray toJSONArray(org.json.JSONArray names, string @string
			)
		{
			return toJSONArray(names, new org.json.JSONTokener(@string));
		}

		/// <summary>
		/// Produce a JSONArray of JSONObjects from a comma delimited text string
		/// using a supplied JSONArray as the source of element names.
		/// </summary>
		/// <param name="names">A JSONArray of strings.</param>
		/// <param name="x">A JSONTokener of the source text.</param>
		/// <returns>A JSONArray of JSONObjects.</returns>
		/// <exception cref="JSONException"/>
		/// <exception cref="org.json.JSONException"/>
		public static org.json.JSONArray toJSONArray(org.json.JSONArray names, org.json.JSONTokener
			 x)
		{
			if (names == null || names.length() == 0)
			{
				return null;
			}
			org.json.JSONArray ja = new org.json.JSONArray();
			for (; ; )
			{
				org.json.JSONObject jo = rowToJSONObject(names, x);
				if (jo == null)
				{
					break;
				}
				ja.put(jo);
			}
			if (ja.length() == 0)
			{
				return null;
			}
			return ja;
		}

		/// <summary>Produce a comma delimited text from a JSONArray of JSONObjects.</summary>
		/// <remarks>
		/// Produce a comma delimited text from a JSONArray of JSONObjects. The
		/// first row will be a list of names obtained by inspecting the first
		/// JSONObject.
		/// </remarks>
		/// <param name="ja">A JSONArray of JSONObjects.</param>
		/// <returns>A comma delimited text.</returns>
		/// <exception cref="JSONException"/>
		/// <exception cref="org.json.JSONException"/>
		public static string toString(org.json.JSONArray ja)
		{
			org.json.JSONObject jo = ja.optJSONObject(0);
			if (jo != null)
			{
				org.json.JSONArray names = jo.names();
				if (names != null)
				{
					return rowToString(names) + toString(names, ja);
				}
			}
			return null;
		}

		/// <summary>
		/// Produce a comma delimited text from a JSONArray of JSONObjects using
		/// a provided list of names.
		/// </summary>
		/// <remarks>
		/// Produce a comma delimited text from a JSONArray of JSONObjects using
		/// a provided list of names. The list of names is not included in the
		/// output.
		/// </remarks>
		/// <param name="names">A JSONArray of strings.</param>
		/// <param name="ja">A JSONArray of JSONObjects.</param>
		/// <returns>A comma delimited text.</returns>
		/// <exception cref="JSONException"/>
		/// <exception cref="org.json.JSONException"/>
		public static string toString(org.json.JSONArray names, org.json.JSONArray ja)
		{
			if (names == null || names.length() == 0)
			{
				return null;
			}
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			for (int i = 0; i < ja.length(); i += 1)
			{
				org.json.JSONObject jo = ja.optJSONObject(i);
				if (jo != null)
				{
					sb.Append(rowToString(jo.toJSONArray(names)));
				}
			}
			return sb.ToString();
		}
	}
}
