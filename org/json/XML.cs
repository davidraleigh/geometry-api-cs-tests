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
		public static string Escape(string @string)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder(@string.Length);
			for (int i = 0, length = @string.Length; i < length; i++)
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
		public static void NoSpace(string @string)
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
		private static bool Parse(org.json.XMLTokener x, org.json.JSONObject context, string
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
			token = x.NextToken();
			// <!
			if (token == BANG)
			{
				c = x.Next();
				if (c == '-')
				{
					if (x.Next() == '-')
					{
						x.SkipPast("-->");
						return false;
					}
					x.Back();
				}
				else
				{
					if (c == '[')
					{
						token = x.NextToken();
						if ("CDATA".Equals(token))
						{
							if (x.Next() == '[')
							{
								@string = x.NextCDATA();
								if (@string.Length > 0)
								{
									context.Accumulate("content", @string);
								}
								return false;
							}
						}
						throw x.SyntaxError("Expected 'CDATA['");
					}
				}
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
					x.SkipPast("?>");
					return false;
				}
				else
				{
					if (token == SLASH)
					{
						// Close tag </
						token = x.NextToken();
						if (name == null)
						{
							throw x.SyntaxError("Mismatched close tag " + token);
						}
						if (!token.Equals(name))
						{
							throw x.SyntaxError("Mismatched " + name + " and " + token);
						}
						if (x.NextToken() != GT)
						{
							throw x.SyntaxError("Misshaped close tag");
						}
						return true;
					}
					else
					{
						if (token is char)
						{
							throw x.SyntaxError("Misshaped tag");
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
									token = x.NextToken();
								}
								// attribute = value
								if (token is string)
								{
									@string = (string)token;
									token = x.NextToken();
									if (token == EQ)
									{
										token = x.NextToken();
										if (!(token is string))
										{
											throw x.SyntaxError("Missing value");
										}
										jsonobject.Accumulate(@string, org.json.XML.StringToValue((string)token));
										token = null;
									}
									else
									{
										jsonobject.Accumulate(@string, string.Empty);
									}
								}
								else
								{
									// Empty tag <.../>
									if (token == SLASH)
									{
										if (x.NextToken() != GT)
										{
											throw x.SyntaxError("Misshaped tag");
										}
										if (jsonobject.Length() > 0)
										{
											context.Accumulate(tagName, jsonobject);
										}
										else
										{
											context.Accumulate(tagName, string.Empty);
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
												token = x.NextContent();
												if (token == null)
												{
													if (tagName != null)
													{
														throw x.SyntaxError("Unclosed tag " + tagName);
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
															jsonobject.Accumulate("content", org.json.XML.StringToValue(@string));
														}
													}
													else
													{
														// Nested element
														if (token == LT)
														{
															if (Parse(x, jsonobject, tagName))
															{
																if (jsonobject.Length() == 0)
																{
																	context.Accumulate(tagName, string.Empty);
																}
																else
																{
																	if (jsonobject.Length() == 1 && jsonobject.Opt("content") != null)
																	{
																		context.Accumulate(tagName, jsonobject.Opt("content"));
																	}
																	else
																	{
																		context.Accumulate(tagName, jsonobject);
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
											throw x.SyntaxError("Misshaped tag");
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
		public static object StringToValue(string @string)
		{
			if (Sharpen.Runtime.EqualsIgnoreCase("true", @string))
			{
				return true;
			}
			if (Sharpen.Runtime.EqualsIgnoreCase("false", @string))
			{
				return false;
			}
			if (Sharpen.Runtime.EqualsIgnoreCase("null", @string))
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
		public static org.json.JSONObject ToJSONObject(string @string)
		{
			org.json.JSONObject jo = new org.json.JSONObject();
			org.json.XMLTokener x = new org.json.XMLTokener(@string);
			while (x.More() && x.SkipPast("<"))
			{
				Parse(x, jo, null);
			}
			return jo;
		}

		/// <summary>Convert a JSONObject into a well-formed, element-normal XML string.</summary>
		/// <param name="object">A JSONObject.</param>
		/// <returns>A string.</returns>
		/// <exception cref="JSONException"/>
		/// <exception cref="org.json.JSONException"/>
		public static string ToString(object @object)
		{
			return ToString(@object, null);
		}

		/// <summary>Convert a JSONObject into a well-formed, element-normal XML string.</summary>
		/// <param name="object">A JSONObject.</param>
		/// <param name="tagName">The optional name of the enclosing tag.</param>
		/// <returns>A string.</returns>
		/// <exception cref="JSONException"/>
		/// <exception cref="org.json.JSONException"/>
		public static string ToString(object @object, string tagName)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
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
				keys = jo.Keys();
				while (keys.HasNext())
				{
					key = keys.Next();
					value = jo.Opt(key);
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
							length = ja.Length();
							for (i = 0; i < length; i += 1)
							{
								if (i > 0)
								{
									sb.Append('\n');
								}
								sb.Append(Escape(ja.Get(i).ToString()));
							}
						}
						else
						{
							sb.Append(Escape(value.ToString()));
						}
					}
					else
					{
						// Emit an array of similar keys
						if (value is org.json.JSONArray)
						{
							ja = (org.json.JSONArray)value;
							length = ja.Length();
							for (i = 0; i < length; i += 1)
							{
								value = ja.Get(i);
								if (value is org.json.JSONArray)
								{
									sb.Append('<');
									sb.Append(key);
									sb.Append('>');
									sb.Append(ToString(value));
									sb.Append("</");
									sb.Append(key);
									sb.Append('>');
								}
								else
								{
									sb.Append(ToString(value, key));
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
								sb.Append(ToString(value, key));
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
				if (@object.GetType().IsArray)
				{
					@object = new org.json.JSONArray(@object);
				}
				if (@object is org.json.JSONArray)
				{
					ja = (org.json.JSONArray)@object;
					length = ja.Length();
					for (i = 0; i < length; i += 1)
					{
						sb.Append(ToString(ja.Opt(i), tagName == null ? "array" : tagName));
					}
					return sb.ToString();
				}
				else
				{
					@string = (@object == null) ? "null" : Escape(@object.ToString());
					return (tagName == null) ? "\"" + @string + "\"" : (@string.Length == 0) ? "<" + 
						tagName + "/>" : "<" + tagName + ">" + @string + "</" + tagName + ">";
				}
			}
		}
	}
}
