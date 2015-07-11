using Sharpen;

namespace org.json
{
	/// <summary>
	/// The XMLTokener extends the JSONTokener to provide additional methods
	/// for the parsing of XML texts.
	/// </summary>
	/// <author>JSON.org</author>
	/// <version>2014-05-03</version>
	public class XMLTokener : org.json.JSONTokener
	{
		/// <summary>The table of entity values.</summary>
		/// <remarks>
		/// The table of entity values. It initially contains Character values for
		/// amp, apos, gt, lt, quot.
		/// </remarks>
		public static readonly System.Collections.Generic.Dictionary<string, char> entity;

		static XMLTokener()
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
			entity = new System.Collections.Generic.Dictionary<string, char>(8);
			entity["amp"] = org.json.XML.AMP;
			entity["apos"] = org.json.XML.APOS;
			entity["gt"] = org.json.XML.GT;
			entity["lt"] = org.json.XML.LT;
			entity["quot"] = org.json.XML.QUOT;
		}

		/// <summary>Construct an XMLTokener from a string.</summary>
		/// <param name="s">A source string.</param>
		public XMLTokener(string s)
			: base(s)
		{
		}

		/// <summary>Get the text in the CDATA block.</summary>
		/// <returns>The string up to the <code>]]&gt;</code>.</returns>
		/// <exception cref="JSONException">If the <code>]]&gt;</code> is not found.</exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual string nextCDATA()
		{
			char c;
			int i;
			java.lang.StringBuilder sb = new java.lang.StringBuilder();
			for (; ; )
			{
				c = next();
				if (end())
				{
					throw syntaxError("Unclosed CDATA");
				}
				sb.Append(c);
				i = sb.Length - 3;
				if (i >= 0 && sb[i] == ']' && sb[i + 1] == ']' && sb[i + 2] == '>')
				{
					sb.Length = i;
					return sb.ToString();
				}
			}
		}

		/// <summary>Get the next XML outer token, trimming whitespace.</summary>
		/// <remarks>
		/// Get the next XML outer token, trimming whitespace. There are two kinds
		/// of tokens: the '&lt;' character which begins a markup tag, and the content
		/// text between markup tags.
		/// </remarks>
		/// <returns>
		/// A string, or a '&lt;' Character, or null if there is no more
		/// source text.
		/// </returns>
		/// <exception cref="JSONException"/>
		/// <exception cref="org.json.JSONException"/>
		public virtual object nextContent()
		{
			char c;
			java.lang.StringBuilder sb;
			do
			{
				c = next();
			}
			while (char.IsWhiteSpace(c));
			if (c == 0)
			{
				return null;
			}
			if (c == '<')
			{
				return org.json.XML.LT;
			}
			sb = new java.lang.StringBuilder();
			for (; ; )
			{
				if (c == '<' || c == 0)
				{
					back();
					return sb.ToString().Trim();
				}
				if (c == '&')
				{
					sb.Append(nextEntity(c));
				}
				else
				{
					sb.Append(c);
				}
				c = next();
			}
		}

		/// <summary>Return the next entity.</summary>
		/// <remarks>
		/// Return the next entity. These entities are translated to Characters:
		/// <code>&amp;  &apos;  &gt;  &lt;  &quot;</code>.
		/// </remarks>
		/// <param name="ampersand">An ampersand character.</param>
		/// <returns>A Character or an entity String if the entity is not recognized.</returns>
		/// <exception cref="JSONException">If missing ';' in XML entity.</exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual object nextEntity(char ampersand)
		{
			java.lang.StringBuilder sb = new java.lang.StringBuilder();
			for (; ; )
			{
				char c = next();
				if (char.isLetterOrDigit(c) || c == '#')
				{
					sb.Append(char.toLowerCase(c));
				}
				else
				{
					if (c == ';')
					{
						break;
					}
					else
					{
						throw syntaxError("Missing ';' in XML entity: &" + sb);
					}
				}
			}
			string @string = sb.ToString();
			object @object = entity[@string];
			return @object != null ? @object : ampersand + @string + ";";
		}

		/// <summary>Returns the next XML meta token.</summary>
		/// <remarks>
		/// Returns the next XML meta token. This is used for skipping over <!...>
		/// and <?...?> structures.
		/// </remarks>
		/// <returns>
		/// Syntax characters (<code>&lt; &gt; / = ! ?</code>) are returned as
		/// Character, and strings and names are returned as Boolean. We don't care
		/// what the values actually are.
		/// </returns>
		/// <exception cref="JSONException">
		/// If a string is not properly closed or if the XML
		/// is badly structured.
		/// </exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual object nextMeta()
		{
			char c;
			char q;
			do
			{
				c = next();
			}
			while (char.IsWhiteSpace(c));
			switch (c)
			{
				case 0:
				{
					throw syntaxError("Misshaped meta tag");
				}

				case '<':
				{
					return org.json.XML.LT;
				}

				case '>':
				{
					return org.json.XML.GT;
				}

				case '/':
				{
					return org.json.XML.SLASH;
				}

				case '=':
				{
					return org.json.XML.EQ;
				}

				case '!':
				{
					return org.json.XML.BANG;
				}

				case '?':
				{
					return org.json.XML.QUEST;
				}

				case '"':
				case '\'':
				{
					q = c;
					for (; ; )
					{
						c = next();
						if (c == 0)
						{
							throw syntaxError("Unterminated string");
						}
						if (c == q)
						{
							return true;
						}
					}
					goto default;
				}

				default:
				{
					for (; ; )
					{
						c = next();
						if (char.IsWhiteSpace(c))
						{
							return true;
						}
						switch (c)
						{
							case 0:
							case '<':
							case '>':
							case '/':
							case '=':
							case '!':
							case '?':
							case '"':
							case '\'':
							{
								back();
								return true;
							}
						}
					}
					break;
				}
			}
		}

		/// <summary>Get the next XML Token.</summary>
		/// <remarks>
		/// Get the next XML Token. These tokens are found inside of angle
		/// brackets. It may be one of these characters: <code>/ &gt; = ! ?</code> or it
		/// may be a string wrapped in single quotes or double quotes, or it may be a
		/// name.
		/// </remarks>
		/// <returns>a String or a Character.</returns>
		/// <exception cref="JSONException">If the XML is not well formed.</exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual object nextToken()
		{
			char c;
			char q;
			java.lang.StringBuilder sb;
			do
			{
				c = next();
			}
			while (char.IsWhiteSpace(c));
			switch (c)
			{
				case 0:
				{
					throw syntaxError("Misshaped element");
				}

				case '<':
				{
					throw syntaxError("Misplaced '<'");
				}

				case '>':
				{
					return org.json.XML.GT;
				}

				case '/':
				{
					return org.json.XML.SLASH;
				}

				case '=':
				{
					return org.json.XML.EQ;
				}

				case '!':
				{
					return org.json.XML.BANG;
				}

				case '?':
				{
					return org.json.XML.QUEST;
				}

				case '"':
				case '\'':
				{
					// Quoted string
					q = c;
					sb = new java.lang.StringBuilder();
					for (; ; )
					{
						c = next();
						if (c == 0)
						{
							throw syntaxError("Unterminated string");
						}
						if (c == q)
						{
							return sb.ToString();
						}
						if (c == '&')
						{
							sb.Append(nextEntity(c));
						}
						else
						{
							sb.Append(c);
						}
					}
					goto default;
				}

				default:
				{
					// Name
					sb = new java.lang.StringBuilder();
					for (; ; )
					{
						sb.Append(c);
						c = next();
						if (char.IsWhiteSpace(c))
						{
							return sb.ToString();
						}
						switch (c)
						{
							case 0:
							{
								return sb.ToString();
							}

							case '>':
							case '/':
							case '=':
							case '!':
							case '?':
							case '[':
							case ']':
							{
								back();
								return sb.ToString();
							}

							case '<':
							case '"':
							case '\'':
							{
								throw syntaxError("Bad character in a name");
							}
						}
					}
					break;
				}
			}
		}

		/// <summary>Skip characters until past the requested string.</summary>
		/// <remarks>
		/// Skip characters until past the requested string.
		/// If it is not found, we are left at the end of the source with a result of false.
		/// </remarks>
		/// <param name="to">A string to skip past.</param>
		/// <exception cref="JSONException"/>
		/// <exception cref="org.json.JSONException"/>
		public virtual bool skipPast(string to)
		{
			bool b;
			char c;
			int i;
			int j;
			int offset = 0;
			int length = to.Length;
			char[] circle = new char[length];
			/*
			* First fill the circle buffer with as many characters as are in the
			* to string. If we reach an early end, bail.
			*/
			for (i = 0; i < length; i += 1)
			{
				c = next();
				if (c == 0)
				{
					return false;
				}
				circle[i] = c;
			}
			/* We will loop, possibly for all of the remaining characters. */
			for (; ; )
			{
				j = offset;
				b = true;
				/* Compare the circle buffer with the to string. */
				for (i = 0; i < length; i += 1)
				{
					if (circle[j] != to[i])
					{
						b = false;
						break;
					}
					j += 1;
					if (j >= length)
					{
						j -= length;
					}
				}
				/* If we exit the loop with b intact, then victory is ours. */
				if (b)
				{
					return true;
				}
				/* Get the next character. If there isn't one, then defeat is ours. */
				c = next();
				if (c == 0)
				{
					return false;
				}
				/*
				* Shove the character in the circle buffer and advance the
				* circle offset. The offset is mod n.
				*/
				circle[offset] = c;
				offset += 1;
				if (offset >= length)
				{
					offset -= length;
				}
			}
		}
	}
}
