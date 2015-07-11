using Sharpen;

namespace org.json
{
	/// <summary>
	/// This provides static methods to convert an XML text into a JSONArray or
	/// JSONObject, and to covert a JSONArray or JSONObject into an XML text using
	/// the JsonML transform.
	/// </summary>
	/// <author>JSON.org</author>
	/// <version>2014-05-03</version>
	public class JSONML
	{
		/*
		Copyright (c) 2008 JSON.org
		
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
		/// <summary>Parse XML values and store them in a JSONArray.</summary>
		/// <param name="x">The XMLTokener containing the source string.</param>
		/// <param name="arrayForm">true if array form, false if object form.</param>
		/// <param name="ja">
		/// The JSONArray that is containing the current tag or null
		/// if we are at the outermost level.
		/// </param>
		/// <returns>A JSONArray if the value is the outermost tag, otherwise null.</returns>
		/// <exception cref="JSONException"/>
		/// <exception cref="org.json.JSONException"/>
		private static object parse(org.json.XMLTokener x, bool arrayForm, org.json.JSONArray
			 ja)
		{
			string attribute;
			char c;
			string closeTag = null;
			int i;
			org.json.JSONArray newja = null;
			org.json.JSONObject newjo = null;
			object token;
			string tagName = null;
			// Test for and skip past these forms:
			//      <!-- ... -->
			//      <![  ... ]]>
			//      <!   ...   >
			//      <?   ...  ?>
			while (true)
			{
				if (!x.more())
				{
					throw x.syntaxError("Bad XML");
				}
				token = x.nextContent();
				if (token == org.json.XML.LT)
				{
					token = x.nextToken();
					if (token is char)
					{
						if (token == org.json.XML.SLASH)
						{
							// Close tag </
							token = x.nextToken();
							if (!(token is string))
							{
								throw new org.json.JSONException("Expected a closing name instead of '" + token +
									 "'.");
							}
							if (x.nextToken() != org.json.XML.GT)
							{
								throw x.syntaxError("Misshaped close tag");
							}
							return token;
						}
						else
						{
							if (token == org.json.XML.BANG)
							{
								// <!
								c = x.next();
								if (c == '-')
								{
									if (x.next() == '-')
									{
										x.skipPast("-->");
									}
									else
									{
										x.back();
									}
								}
								else
								{
									if (c == '[')
									{
										token = x.nextToken();
										if (token.Equals("CDATA") && x.next() == '[')
										{
											if (ja != null)
											{
												ja.put(x.nextCDATA());
											}
										}
										else
										{
											throw x.syntaxError("Expected 'CDATA['");
										}
									}
									else
									{
										i = 1;
										do
										{
											token = x.nextMeta();
											if (token == null)
											{
												throw x.syntaxError("Missing '>' after '<!'.");
											}
											else
											{
												if (token == org.json.XML.LT)
												{
													i += 1;
												}
												else
												{
													if (token == org.json.XML.GT)
													{
														i -= 1;
													}
												}
											}
										}
										while (i > 0);
									}
								}
							}
							else
							{
								if (token == org.json.XML.QUEST)
								{
									// <?
									x.skipPast("?>");
								}
								else
								{
									throw x.syntaxError("Misshaped tag");
								}
							}
						}
					}
					else
					{
						// Open tag <
						if (!(token is string))
						{
							throw x.syntaxError("Bad tagName '" + token + "'.");
						}
						tagName = (string)token;
						newja = new org.json.JSONArray();
						newjo = new org.json.JSONObject();
						if (arrayForm)
						{
							newja.put(tagName);
							if (ja != null)
							{
								ja.put(newja);
							}
						}
						else
						{
							newjo.put("tagName", tagName);
							if (ja != null)
							{
								ja.put(newjo);
							}
						}
						token = null;
						for (; ; )
						{
							if (token == null)
							{
								token = x.nextToken();
							}
							if (token == null)
							{
								throw x.syntaxError("Misshaped tag");
							}
							if (!(token is string))
							{
								break;
							}
							// attribute = value
							attribute = (string)token;
							if (!arrayForm && ("tagName".Equals(attribute) || "childNode".Equals(attribute)))
							{
								throw x.syntaxError("Reserved attribute.");
							}
							token = x.nextToken();
							if (token == org.json.XML.EQ)
							{
								token = x.nextToken();
								if (!(token is string))
								{
									throw x.syntaxError("Missing value");
								}
								newjo.accumulate(attribute, org.json.XML.stringToValue((string)token));
								token = null;
							}
							else
							{
								newjo.accumulate(attribute, string.Empty);
							}
						}
						if (arrayForm && newjo.length() > 0)
						{
							newja.put(newjo);
						}
						// Empty tag <.../>
						if (token == org.json.XML.SLASH)
						{
							if (x.nextToken() != org.json.XML.GT)
							{
								throw x.syntaxError("Misshaped tag");
							}
							if (ja == null)
							{
								if (arrayForm)
								{
									return newja;
								}
								else
								{
									return newjo;
								}
							}
						}
						else
						{
							// Content, between <...> and </...>
							if (token != org.json.XML.GT)
							{
								throw x.syntaxError("Misshaped tag");
							}
							closeTag = (string)parse(x, arrayForm, newja);
							if (closeTag != null)
							{
								if (!closeTag.Equals(tagName))
								{
									throw x.syntaxError("Mismatched '" + tagName + "' and '" + closeTag + "'");
								}
								tagName = null;
								if (!arrayForm && newja.length() > 0)
								{
									newjo.put("childNodes", newja);
								}
								if (ja == null)
								{
									if (arrayForm)
									{
										return newja;
									}
									else
									{
										return newjo;
									}
								}
							}
						}
					}
				}
				else
				{
					if (ja != null)
					{
						ja.put(token is string ? org.json.XML.stringToValue((string)token) : token);
					}
				}
			}
		}

		/// <summary>
		/// Convert a well-formed (but not necessarily valid) XML string into a
		/// JSONArray using the JsonML transform.
		/// </summary>
		/// <remarks>
		/// Convert a well-formed (but not necessarily valid) XML string into a
		/// JSONArray using the JsonML transform. Each XML tag is represented as
		/// a JSONArray in which the first element is the tag name. If the tag has
		/// attributes, then the second element will be JSONObject containing the
		/// name/value pairs. If the tag contains children, then strings and
		/// JSONArrays will represent the child tags.
		/// Comments, prologs, DTDs, and <code>&lt;[ [ ]]&gt;</code> are ignored.
		/// </remarks>
		/// <param name="string">The source string.</param>
		/// <returns>A JSONArray containing the structured data from the XML string.</returns>
		/// <exception cref="JSONException"/>
		/// <exception cref="org.json.JSONException"/>
		public static org.json.JSONArray toJSONArray(string @string)
		{
			return toJSONArray(new org.json.XMLTokener(@string));
		}

		/// <summary>
		/// Convert a well-formed (but not necessarily valid) XML string into a
		/// JSONArray using the JsonML transform.
		/// </summary>
		/// <remarks>
		/// Convert a well-formed (but not necessarily valid) XML string into a
		/// JSONArray using the JsonML transform. Each XML tag is represented as
		/// a JSONArray in which the first element is the tag name. If the tag has
		/// attributes, then the second element will be JSONObject containing the
		/// name/value pairs. If the tag contains children, then strings and
		/// JSONArrays will represent the child content and tags.
		/// Comments, prologs, DTDs, and <code>&lt;[ [ ]]&gt;</code> are ignored.
		/// </remarks>
		/// <param name="x">An XMLTokener.</param>
		/// <returns>A JSONArray containing the structured data from the XML string.</returns>
		/// <exception cref="JSONException"/>
		/// <exception cref="org.json.JSONException"/>
		public static org.json.JSONArray toJSONArray(org.json.XMLTokener x)
		{
			return (org.json.JSONArray)parse(x, true, null);
		}

		/// <summary>
		/// Convert a well-formed (but not necessarily valid) XML string into a
		/// JSONObject using the JsonML transform.
		/// </summary>
		/// <remarks>
		/// Convert a well-formed (but not necessarily valid) XML string into a
		/// JSONObject using the JsonML transform. Each XML tag is represented as
		/// a JSONObject with a "tagName" property. If the tag has attributes, then
		/// the attributes will be in the JSONObject as properties. If the tag
		/// contains children, the object will have a "childNodes" property which
		/// will be an array of strings and JsonML JSONObjects.
		/// Comments, prologs, DTDs, and <code>&lt;[ [ ]]&gt;</code> are ignored.
		/// </remarks>
		/// <param name="x">An XMLTokener of the XML source text.</param>
		/// <returns>A JSONObject containing the structured data from the XML string.</returns>
		/// <exception cref="JSONException"/>
		/// <exception cref="org.json.JSONException"/>
		public static org.json.JSONObject toJSONObject(org.json.XMLTokener x)
		{
			return (org.json.JSONObject)parse(x, false, null);
		}

		/// <summary>
		/// Convert a well-formed (but not necessarily valid) XML string into a
		/// JSONObject using the JsonML transform.
		/// </summary>
		/// <remarks>
		/// Convert a well-formed (but not necessarily valid) XML string into a
		/// JSONObject using the JsonML transform. Each XML tag is represented as
		/// a JSONObject with a "tagName" property. If the tag has attributes, then
		/// the attributes will be in the JSONObject as properties. If the tag
		/// contains children, the object will have a "childNodes" property which
		/// will be an array of strings and JsonML JSONObjects.
		/// Comments, prologs, DTDs, and <code>&lt;[ [ ]]&gt;</code> are ignored.
		/// </remarks>
		/// <param name="string">The XML source text.</param>
		/// <returns>A JSONObject containing the structured data from the XML string.</returns>
		/// <exception cref="JSONException"/>
		/// <exception cref="org.json.JSONException"/>
		public static org.json.JSONObject toJSONObject(string @string)
		{
			return toJSONObject(new org.json.XMLTokener(@string));
		}

		/// <summary>Reverse the JSONML transformation, making an XML text from a JSONArray.</summary>
		/// <param name="ja">A JSONArray.</param>
		/// <returns>An XML string.</returns>
		/// <exception cref="JSONException"/>
		/// <exception cref="org.json.JSONException"/>
		public static string toString(org.json.JSONArray ja)
		{
			int i;
			org.json.JSONObject jo;
			string key;
			System.Collections.Generic.IEnumerator<string> keys;
			int length;
			object @object;
			java.lang.StringBuilder sb = new java.lang.StringBuilder();
			string tagName;
			string value;
			// Emit <tagName
			tagName = ja.getString(0);
			org.json.XML.noSpace(tagName);
			tagName = org.json.XML.escape(tagName);
			sb.Append('<');
			sb.Append(tagName);
			@object = ja.opt(1);
			if (@object is org.json.JSONObject)
			{
				i = 2;
				jo = (org.json.JSONObject)@object;
				// Emit the attributes
				keys = jo.keys();
				while (keys.MoveNext())
				{
					key = keys.Current;
					org.json.XML.noSpace(key);
					value = jo.optString(key);
					if (value != null)
					{
						sb.Append(' ');
						sb.Append(org.json.XML.escape(key));
						sb.Append('=');
						sb.Append('"');
						sb.Append(org.json.XML.escape(value));
						sb.Append('"');
					}
				}
			}
			else
			{
				i = 1;
			}
			// Emit content in body
			length = ja.length();
			if (i >= length)
			{
				sb.Append('/');
				sb.Append('>');
			}
			else
			{
				sb.Append('>');
				do
				{
					@object = ja.get(i);
					i += 1;
					if (@object != null)
					{
						if (@object is string)
						{
							sb.Append(org.json.XML.escape(@object.ToString()));
						}
						else
						{
							if (@object is org.json.JSONObject)
							{
								sb.Append(toString((org.json.JSONObject)@object));
							}
							else
							{
								if (@object is org.json.JSONArray)
								{
									sb.Append(toString((org.json.JSONArray)@object));
								}
								else
								{
									sb.Append(@object.ToString());
								}
							}
						}
					}
				}
				while (i < length);
				sb.Append('<');
				sb.Append('/');
				sb.Append(tagName);
				sb.Append('>');
			}
			return sb.ToString();
		}

		/// <summary>Reverse the JSONML transformation, making an XML text from a JSONObject.
		/// 	</summary>
		/// <remarks>
		/// Reverse the JSONML transformation, making an XML text from a JSONObject.
		/// The JSONObject must contain a "tagName" property. If it has children,
		/// then it must have a "childNodes" property containing an array of objects.
		/// The other properties are attributes with string values.
		/// </remarks>
		/// <param name="jo">A JSONObject.</param>
		/// <returns>An XML string.</returns>
		/// <exception cref="JSONException"/>
		/// <exception cref="org.json.JSONException"/>
		public static string toString(org.json.JSONObject jo)
		{
			java.lang.StringBuilder sb = new java.lang.StringBuilder();
			int i;
			org.json.JSONArray ja;
			string key;
			System.Collections.Generic.IEnumerator<string> keys;
			int length;
			object @object;
			string tagName;
			string value;
			//Emit <tagName
			tagName = jo.optString("tagName");
			if (tagName == null)
			{
				return org.json.XML.escape(jo.ToString());
			}
			org.json.XML.noSpace(tagName);
			tagName = org.json.XML.escape(tagName);
			sb.Append('<');
			sb.Append(tagName);
			//Emit the attributes
			keys = jo.keys();
			while (keys.MoveNext())
			{
				key = keys.Current;
				if (!"tagName".Equals(key) && !"childNodes".Equals(key))
				{
					org.json.XML.noSpace(key);
					value = jo.optString(key);
					if (value != null)
					{
						sb.Append(' ');
						sb.Append(org.json.XML.escape(key));
						sb.Append('=');
						sb.Append('"');
						sb.Append(org.json.XML.escape(value));
						sb.Append('"');
					}
				}
			}
			//Emit content in body
			ja = jo.optJSONArray("childNodes");
			if (ja == null)
			{
				sb.Append('/');
				sb.Append('>');
			}
			else
			{
				sb.Append('>');
				length = ja.length();
				for (i = 0; i < length; i += 1)
				{
					@object = ja.get(i);
					if (@object != null)
					{
						if (@object is string)
						{
							sb.Append(org.json.XML.escape(@object.ToString()));
						}
						else
						{
							if (@object is org.json.JSONObject)
							{
								sb.Append(toString((org.json.JSONObject)@object));
							}
							else
							{
								if (@object is org.json.JSONArray)
								{
									sb.Append(toString((org.json.JSONArray)@object));
								}
								else
								{
									sb.Append(@object.ToString());
								}
							}
						}
					}
				}
				sb.Append('<');
				sb.Append('/');
				sb.Append(tagName);
				sb.Append('>');
			}
			return sb.ToString();
		}
	}
}
