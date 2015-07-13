using System.Collections.Generic;
using System.Text;
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
		private static object Parse(XMLTokener x, bool arrayForm, JSONArray ja)
		{
			string attribute;
			char c;
			string closeTag = null;
			int i;
			JSONArray newja = null;
			JSONObject newjo = null;
			object token;
			string tagName = null;
			// Test for and skip past these forms:
			//      <!-- ... -->
			//      <![  ... ]]>
			//      <!   ...   >
			//      <?   ...  ?>
			while (true)
			{
				if (!x.More())
				{
					throw x.SyntaxError("Bad XML");
				}
				token = x.NextContent();
				if (token == XML.LT)
				{
					token = x.NextToken();
					if (token is char)
					{
						if (token == XML.SLASH)
						{
							// Close tag </
							token = x.NextToken();
							if (!(token is string))
							{
								throw new JSONException("Expected a closing name instead of '" + token + "'.");
							}
							if (x.NextToken() != XML.GT)
							{
								throw x.SyntaxError("Misshaped close tag");
							}
							return token;
						}
						else
						{
							if (token == XML.BANG)
							{
								// <!
								c = x.Next();
								if (c == '-')
								{
									if (x.Next() == '-')
									{
										x.SkipPast("-->");
									}
									else
									{
										x.Back();
									}
								}
								else
								{
									if (c == '[')
									{
										token = x.NextToken();
										if (token.Equals("CDATA") && x.Next() == '[')
										{
											if (ja != null)
											{
												ja.Put(x.NextCDATA());
											}
										}
										else
										{
											throw x.SyntaxError("Expected 'CDATA['");
										}
									}
									else
									{
										i = 1;
										do
										{
											token = x.NextMeta();
											if (token == null)
											{
												throw x.SyntaxError("Missing '>' after '<!'.");
											}
											else
											{
												if (token == XML.LT)
												{
													i += 1;
												}
												else
												{
													if (token == XML.GT)
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
								if (token == XML.QUEST)
								{
									// <?
									x.SkipPast("?>");
								}
								else
								{
									throw x.SyntaxError("Misshaped tag");
								}
							}
						}
					}
					else
					{
						// Open tag <
						if (!(token is string))
						{
							throw x.SyntaxError("Bad tagName '" + token + "'.");
						}
						tagName = (string)token;
						newja = new JSONArray();
						newjo = new JSONObject();
						if (arrayForm)
						{
							newja.Put(tagName);
							if (ja != null)
							{
								ja.Put(newja);
							}
						}
						else
						{
							newjo.Put("tagName", tagName);
							if (ja != null)
							{
								ja.Put(newjo);
							}
						}
						token = null;
						for (; ; )
						{
							if (token == null)
							{
								token = x.NextToken();
							}
							if (token == null)
							{
								throw x.SyntaxError("Misshaped tag");
							}
							if (!(token is string))
							{
								break;
							}
							// attribute = value
							attribute = (string)token;
							if (!arrayForm && ("tagName".Equals(attribute) || "childNode".Equals(attribute)))
							{
								throw x.SyntaxError("Reserved attribute.");
							}
							token = x.NextToken();
							if (token == XML.EQ)
							{
								token = x.NextToken();
								if (!(token is string))
								{
									throw x.SyntaxError("Missing value");
								}
								newjo.Accumulate(attribute, XML.StringToValue((string)token));
								token = null;
							}
							else
							{
								newjo.Accumulate(attribute, string.Empty);
							}
						}
						if (arrayForm && newjo.Length() > 0)
						{
							newja.Put(newjo);
						}
						// Empty tag <.../>
						if (token == XML.SLASH)
						{
							if (x.NextToken() != XML.GT)
							{
								throw x.SyntaxError("Misshaped tag");
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
							if (token != XML.GT)
							{
								throw x.SyntaxError("Misshaped tag");
							}
							closeTag = (string)Parse(x, arrayForm, newja);
							if (closeTag != null)
							{
								if (!closeTag.Equals(tagName))
								{
									throw x.SyntaxError("Mismatched '" + tagName + "' and '" + closeTag + "'");
								}
								tagName = null;
								if (!arrayForm && newja.Length() > 0)
								{
									newjo.Put("childNodes", newja);
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
						ja.Put(token is string ? XML.StringToValue((string)token) : token);
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
		public static JSONArray ToJSONArray(string @string)
		{
			return ToJSONArray(new XMLTokener(@string));
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
		public static JSONArray ToJSONArray(XMLTokener x)
		{
			return (JSONArray)Parse(x, true, null);
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
		public static JSONObject ToJSONObject(XMLTokener x)
		{
			return (JSONObject)Parse(x, false, null);
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
		public static JSONObject ToJSONObject(string @string)
		{
			return ToJSONObject(new XMLTokener(@string));
		}

		/// <summary>Reverse the JSONML transformation, making an XML text from a JSONArray.</summary>
		/// <param name="ja">A JSONArray.</param>
		/// <returns>An XML string.</returns>
		/// <exception cref="JSONException"/>
		/// <exception cref="org.json.JSONException"/>
		public static string ToString(JSONArray ja)
		{
			int i;
			JSONObject jo;
			string key;
			IEnumerator<string> keys;
			int length;
			object @object;
			StringBuilder sb = new StringBuilder();
			string tagName;
			string value;
			// Emit <tagName
			tagName = ja.GetString(0);
			XML.NoSpace(tagName);
			tagName = XML.Escape(tagName);
			sb.Append('<');
			sb.Append(tagName);
			@object = ja.Opt(1);
			if (@object is JSONObject)
			{
				i = 2;
				jo = (JSONObject)@object;
				// Emit the attributes
				keys = jo.Keys();
				while (keys.HasNext())
				{
					key = keys.Next();
					XML.NoSpace(key);
					value = jo.OptString(key);
					if (value != null)
					{
						sb.Append(' ');
						sb.Append(XML.Escape(key));
						sb.Append('=');
						sb.Append('"');
						sb.Append(XML.Escape(value));
						sb.Append('"');
					}
				}
			}
			else
			{
				i = 1;
			}
			// Emit content in body
			length = ja.Length();
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
					@object = ja.Get(i);
					i += 1;
					if (@object != null)
					{
						if (@object is string)
						{
							sb.Append(XML.Escape(@object.ToString()));
						}
						else
						{
							if (@object is JSONObject)
							{
								sb.Append(ToString((JSONObject)@object));
							}
							else
							{
								if (@object is JSONArray)
								{
									sb.Append(ToString((JSONArray)@object));
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
		public static string ToString(JSONObject jo)
		{
			StringBuilder sb = new StringBuilder();
			int i;
			JSONArray ja;
			string key;
			IEnumerator<string> keys;
			int length;
			object @object;
			string tagName;
			string value;
			//Emit <tagName
			tagName = jo.OptString("tagName");
			if (tagName == null)
			{
				return XML.Escape(jo.ToString());
			}
			XML.NoSpace(tagName);
			tagName = XML.Escape(tagName);
			sb.Append('<');
			sb.Append(tagName);
			//Emit the attributes
			keys = jo.Keys();
			while (keys.HasNext())
			{
				key = keys.Next();
				if (!"tagName".Equals(key) && !"childNodes".Equals(key))
				{
					XML.NoSpace(key);
					value = jo.OptString(key);
					if (value != null)
					{
						sb.Append(' ');
						sb.Append(XML.Escape(key));
						sb.Append('=');
						sb.Append('"');
						sb.Append(XML.Escape(value));
						sb.Append('"');
					}
				}
			}
			//Emit content in body
			ja = jo.OptJSONArray("childNodes");
			if (ja == null)
			{
				sb.Append('/');
				sb.Append('>');
			}
			else
			{
				sb.Append('>');
				length = ja.Length();
				for (i = 0; i < length; i += 1)
				{
					@object = ja.Get(i);
					if (@object != null)
					{
						if (@object is string)
						{
							sb.Append(XML.Escape(@object.ToString()));
						}
						else
						{
							if (@object is JSONObject)
							{
								sb.Append(ToString((JSONObject)@object));
							}
							else
							{
								if (@object is JSONArray)
								{
									sb.Append(ToString((JSONArray)@object));
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
