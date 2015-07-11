using Sharpen;

namespace org.json
{
	/// <summary>
	/// This provides static methods to convert an XML text into a JSONObject,
	/// and to covert a JSONObject into an XML text.
	/// </summary>
	/// <author>JSON.org</author>
	/// <version>2014-05-03</version>
	public class XML
	{
		/// <summary>The Character '&amp;'.</summary>
		public static readonly char AMP = '&';

		/// <summary>The Character '''.</summary>
		public static readonly char APOS = '\'';

		/// <summary>The Character '!'.</summary>
		public static readonly char BANG = '!';

		/// <summary>The Character '='.</summary>
		public static readonly char EQ = '=';

		/// <summary>The Character '&gt;'.</summary>
		public static readonly char GT = '>';

		/// <summary>The Character '&lt;'.</summary>
		public static readonly char LT = '<';

		/// <summary>The Character '?'.</summary>
		public static readonly char QUEST = '?';

		/// <summary>The Character '"'.</summary>
		public static readonly char QUOT = '"';

		/// <summary>The Character '/'.</summary>
		public static readonly char SLASH = '/';

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
		/// <summary>
		/// Replace special characters with XML escapes:
		/// <pre>
		/// &amp; <small>(ampersand)</small> is replaced by &amp;amp;
		/// &lt; <small>(less than)</small> is replaced by &amp;lt;
		/// &gt; <small>(greater than)</small> is replaced by &amp;gt;
		/// &quot; <small>(double quote)</small> is replaced by &amp;quot;
		/// </pre>
		/// </summary>
		/// <param name="string">The string to be escaped.</param>
		/// <returns>The escaped string.</returns>
		public static string escape(string @string)
		{
			java.lang.StringBuilder sb = new java.lang.StringBuilder(@string.Length);
			for (int i = 0; i < length; i++)
			{
				char c = @string[i];
				switch (c)
				{
					case '&':
					{
						sb.Append("&amp;");
						break;
					}

					case '<':
					{
						sb.Append("&lt;");
						break;
					}

					case '>':
					{
						sb.Append("&gt;");
						break;
					}

					case '"':
					{
						sb.Append("&quot;");
						break;
					}

					case '\'':
					{
						sb.Append("&apos;");
						break;
					}

					default:
					{
						sb.Append(c);
						break;
					}
				}
			}
			return sb.ToString();
		}

		/// <summary>Throw an exception if the string contains whitespace.</summary>
		/// <remarks>
		/// Throw an exception if the string contains whitespace.
		/// Whitespace is not allowed in tagNames and attributes.
		/// </remarks>
		/// <param name="string">A string.</param>
		/// <exception cref="JSONException"/>
		/// <exception cref="org.json.JSONException"/>
		public static void noSpace(string @string)
		{
			int i;
			int length = @string.Length;
			if (length == 0)
			{
				throw new org.json.JSONException("Empty string.");
			}
			for (i = 0; i < length; i += 1)
			{
				if (char.IsWhiteSpace(@string[i]))
				{
					throw new org.json.JSONException("'" + @string + "' contains a space character.");
				}
			}
		}

		/// <summary>Scan the content following the named tag, attaching it to the context.</summary>
		/// <param name="x">The XMLTokener containing the source string.</param>
		/// <param name="context">The JSONObject that will include the new material.</param>
		/// <param name="name">The tag name.</param>
		/// <returns>true if the close tag is processed.</returns>
		/// <exception cref="JSONException"/>
		/// <exception cref="org.json.JSONException"/>
		private static bool parse(org.json.XMLTokener x, org.json.JSONObject context, string
			 name)
		{
			char c;
			int i;
			org.json.JSONObject jsonobject = null;
			string @string;
			string tagName;
			object token;
			// Test for and skip past these forms:
			//      <!-- ... -->
			//      <!   ...   >
			//      <![  ... ]]>
			//      <?   ...  ?>
			// Report errors for these forms:
			//      <>
			//      <=
			//      <<
			token = x.nextToken();
			// <!
			if (token == BANG)
			{
				c = x.next();
				if (c == '-')
				{
					if (x.next() == '-')
					{
						x.skipPast("-->");
						return false;
					}
					x.back();
				}
				else
				{
					if (c == '[')
					{
						token = x.nextToken();
						if ("CDATA".Equals(token))
						{
							if (x.next() == '[')
							{
								@string = x.nextCDATA();
								if (@string.Length > 0)
								{
									context.accumulate("content", @string);
								}
								return false;
							}
						}
						throw x.syntaxError("Expected 'CDATA['");
					}
				}
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
						if (token == LT)
						{
							i += 1;
						}
						else
						{
							if (token == GT)
							{
								i -= 1;
							}
						}
					}
				}
				while (i > 0);
				return false;
			}
			else
			{
				if (token == QUEST)
				{
					// <?
					x.skipPast("?>");
					return false;
				}
				else
				{
					if (token == SLASH)
					{
						// Close tag </
						token = x.nextToken();
						if (name == null)
						{
							throw x.syntaxError("Mismatched close tag " + token);
						}
						if (!token.Equals(name))
						{
							throw x.syntaxError("Mismatched " + name + " and " + token);
						}
						if (x.nextToken() != GT)
						{
							throw x.syntaxError("Misshaped close tag");
						}
						return true;
					}
					else
					{
						if (token is char)
						{
							throw x.syntaxError("Misshaped tag");
						}
						else
						{
							// Open tag <
							tagName = (string)token;
							token = null;
							jsonobject = new org.json.JSONObject();
							for (; ; )
							{
								if (token == null)
								{
									token = x.nextToken();
								}
								// attribute = value
								if (token is string)
								{
									@string = (string)token;
									token = x.nextToken();
									if (token == EQ)
									{
										token = x.nextToken();
										if (!(token is string))
										{
											throw x.syntaxError("Missing value");
										}
										jsonobject.accumulate(@string, org.json.XML.stringToValue((string)token));
										token = null;
									}
									else
									{
										jsonobject.accumulate(@string, string.Empty);
									}
								}
								else
								{
									// Empty tag <.../>
									if (token == SLASH)
									{
										if (x.nextToken() != GT)
										{
											throw x.syntaxError("Misshaped tag");
										}
										if (jsonobject.length() > 0)
										{
											context.accumulate(tagName, jsonobject);
										}
										else
										{
											context.accumulate(tagName, string.Empty);
										}
										return false;
									}
									else
									{
										// Content, between <...> and </...>
										if (token == GT)
										{
											for (; ; )
											{
												token = x.nextContent();
												if (token == null)
												{
													if (tagName != null)
													{
														throw x.syntaxError("Unclosed tag " + tagName);
													}
													return false;
												}
												else
												{
													if (token is string)
													{
														@string = (string)token;
														if (@string.Length > 0)
														{
															jsonobject.accumulate("content", org.json.XML.stringToValue(@string));
														}
													}
													else
													{
														// Nested element
														if (token == LT)
														{
															if (parse(x, jsonobject, tagName))
															{
																if (jsonobject.length() == 0)
																{
																	context.accumulate(tagName, string.Empty);
																}
																else
																{
																	if (jsonobject.length() == 1 && jsonobject.opt("content") != null)
																	{
																		context.accumulate(tagName, jsonobject.opt("content"));
																	}
																	else
																	{
																		context.accumulate(tagName, jsonobject);
																	}
																}
																return false;
															}
														}
													}
												}
											}
										}
										else
										{
											throw x.syntaxError("Misshaped tag");
										}
									}
								}
							}
						}
					}
				}
			}
		}

		/// <summary>Try to convert a string into a number, boolean, or null.</summary>
		/// <remarks>
		/// Try to convert a string into a number, boolean, or null. If the string
		/// can't be converted, return the string. This is much less ambitious than
		/// JSONObject.stringToValue, especially because it does not attempt to
		/// convert plus forms, octal forms, hex forms, or E forms lacking decimal
		/// points.
		/// </remarks>
		/// <param name="string">A String.</param>
		/// <returns>A simple JSON value.</returns>
		public static object stringToValue(string @string)
		{
			if (Sharpen.Runtime.equalsIgnoreCase("true", @string))
			{
				return true;
			}
			if (Sharpen.Runtime.equalsIgnoreCase("false", @string))
			{
				return false;
			}
			if (Sharpen.Runtime.equalsIgnoreCase("null", @string))
			{
				return org.json.JSONObject.NULL;
			}
			// If it might be a number, try converting it, first as a Long, and then as a
			// Double. If that doesn't work, return the string.
			try
			{
				char initial = @string[0];
				if (initial == '-' || (initial >= '0' && initial <= '9'))
				{
					long value = System.Convert.ToInt64(@string);
					if (value.ToString().Equals(@string))
					{
						return value;
					}
				}
			}
			catch (System.Exception)
			{
				try
				{
					double value = System.Convert.ToDouble(@string);
					if (value.ToString().Equals(@string))
					{
						return value;
					}
				}
				catch (System.Exception)
				{
				}
			}
			return @string;
		}

		/// <summary>
		/// Convert a well-formed (but not necessarily valid) XML string into a
		/// JSONObject.
		/// </summary>
		/// <remarks>
		/// Convert a well-formed (but not necessarily valid) XML string into a
		/// JSONObject. Some information may be lost in this transformation
		/// because JSON is a data format and XML is a document format. XML uses
		/// elements, attributes, and content text, while JSON uses unordered
		/// collections of name/value pairs and arrays of values. JSON does not
		/// does not like to distinguish between elements and attributes.
		/// Sequences of similar elements are represented as JSONArrays. Content
		/// text may be placed in a "content" member. Comments, prologs, DTDs, and
		/// <code>&lt;[ [ ]]&gt;</code> are ignored.
		/// </remarks>
		/// <param name="string">The source string.</param>
		/// <returns>A JSONObject containing the structured data from the XML string.</returns>
		/// <exception cref="JSONException"/>
		/// <exception cref="org.json.JSONException"/>
		public static org.json.JSONObject toJSONObject(string @string)
		{
			org.json.JSONObject jo = new org.json.JSONObject();
			org.json.XMLTokener x = new org.json.XMLTokener(@string);
			while (x.more() && x.skipPast("<"))
			{
				parse(x, jo, null);
			}
			return jo;
		}

		/// <summary>Convert a JSONObject into a well-formed, element-normal XML string.</summary>
		/// <param name="object">A JSONObject.</param>
		/// <returns>A string.</returns>
		/// <exception cref="JSONException"/>
		/// <exception cref="org.json.JSONException"/>
		public static string toString(object @object)
		{
			return toString(@object, null);
		}

		/// <summary>Convert a JSONObject into a well-formed, element-normal XML string.</summary>
		/// <param name="object">A JSONObject.</param>
		/// <param name="tagName">The optional name of the enclosing tag.</param>
		/// <returns>A string.</returns>
		/// <exception cref="JSONException"/>
		/// <exception cref="org.json.JSONException"/>
		public static string toString(object @object, string tagName)
		{
			java.lang.StringBuilder sb = new java.lang.StringBuilder();
			int i;
			org.json.JSONArray ja;
			org.json.JSONObject jo;
			string key;
			System.Collections.Generic.IEnumerator<string> keys;
			int length;
			string @string;
			object value;
			if (@object is org.json.JSONObject)
			{
				// Emit <tagName>
				if (tagName != null)
				{
					sb.Append('<');
					sb.Append(tagName);
					sb.Append('>');
				}
				// Loop thru the keys.
				jo = (org.json.JSONObject)@object;
				keys = jo.keys();
				while (keys.MoveNext())
				{
					key = keys.Current;
					value = jo.opt(key);
					if (value == null)
					{
						value = string.Empty;
					}
					@string = value is string ? (string)value : null;
					// Emit content in body
					if ("content".Equals(key))
					{
						if (value is org.json.JSONArray)
						{
							ja = (org.json.JSONArray)value;
							length = ja.length();
							for (i = 0; i < length; i += 1)
							{
								if (i > 0)
								{
									sb.Append('\n');
								}
								sb.Append(escape(ja.get(i).ToString()));
							}
						}
						else
						{
							sb.Append(escape(value.ToString()));
						}
					}
					else
					{
						// Emit an array of similar keys
						if (value is org.json.JSONArray)
						{
							ja = (org.json.JSONArray)value;
							length = ja.length();
							for (i = 0; i < length; i += 1)
							{
								value = ja.get(i);
								if (value is org.json.JSONArray)
								{
									sb.Append('<');
									sb.Append(key);
									sb.Append('>');
									sb.Append(toString(value));
									sb.Append("</");
									sb.Append(key);
									sb.Append('>');
								}
								else
								{
									sb.Append(toString(value, key));
								}
							}
						}
						else
						{
							if (string.Empty.Equals(value))
							{
								sb.Append('<');
								sb.Append(key);
								sb.Append("/>");
							}
							else
							{
								// Emit a new tag <k>
								sb.Append(toString(value, key));
							}
						}
					}
				}
				if (tagName != null)
				{
					// Emit the </tagname> close tag
					sb.Append("</");
					sb.Append(tagName);
					sb.Append('>');
				}
				return sb.ToString();
			}
			else
			{
				// XML does not have good support for arrays. If an array appears in a place
				// where XML is lacking, synthesize an <array> element.
				if (Sharpen.Runtime.getClassForObject(@object).isArray())
				{
					@object = new org.json.JSONArray(@object);
				}
				if (@object is org.json.JSONArray)
				{
					ja = (org.json.JSONArray)@object;
					length = ja.length();
					for (i = 0; i < length; i += 1)
					{
						sb.Append(toString(ja.opt(i), tagName == null ? "array" : tagName));
					}
					return sb.ToString();
				}
				else
				{
					@string = (@object == null) ? "null" : escape(@object.ToString());
					return (tagName == null) ? "\"" + @string + "\"" : (@string.Length == 0) ? "<" + 
						tagName + "/>" : "<" + tagName + ">" + @string + "</" + tagName + ">";
				}
			}
		}
	}
}
