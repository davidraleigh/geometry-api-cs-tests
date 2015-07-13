using Sharpen;

namespace org.json
{
	/// <summary>Convert a web browser cookie specification to a JSONObject and back.</summary>
	/// <remarks>
	/// Convert a web browser cookie specification to a JSONObject and back.
	/// JSON and Cookies are both notations for name/value pairs.
	/// </remarks>
	/// <author>JSON.org</author>
	/// <version>2014-05-03</version>
	public class Cookie
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
		/// <summary>
		/// Produce a copy of a string in which the characters '+', '%', '=', ';'
		/// and control characters are replaced with "%hh".
		/// </summary>
		/// <remarks>
		/// Produce a copy of a string in which the characters '+', '%', '=', ';'
		/// and control characters are replaced with "%hh". This is a gentle form
		/// of URL encoding, attempting to cause as little distortion to the
		/// string as possible. The characters '=' and ';' are meta characters in
		/// cookies. By convention, they are escaped using the URL-encoding. This is
		/// only a convention, not a standard. Often, cookies are expected to have
		/// encoded values. We encode '=' and ';' because we must. We encode '%' and
		/// '+' because they are meta characters in URL encoding.
		/// </remarks>
		/// <param name="string">The source string.</param>
		/// <returns>The escaped result.</returns>
		public static string Escape(string @string)
		{
			char c;
			string s = Sharpen.Extensions.Trim(@string);
			int length = s.Length;
			System.Text.StringBuilder sb = new System.Text.StringBuilder(length);
			for (int i = 0; i < length; i += 1)
			{
				c = s[i];
				if (c < ' ' || c == '+' || c == '%' || c == '=' || c == ';')
				{
					sb.Append('%');
					sb.Append(char.ForDigit((char)(((char)(((uchar)c) >> 4)) & unchecked((int)(0x0f))
						), 16));
					sb.Append(char.ForDigit((char)(c & unchecked((int)(0x0f))), 16));
				}
				else
				{
					sb.Append(c);
				}
			}
			return sb.ToString();
		}

		/// <summary>Convert a cookie specification string into a JSONObject.</summary>
		/// <remarks>
		/// Convert a cookie specification string into a JSONObject. The string
		/// will contain a name value pair separated by '='. The name and the value
		/// will be unescaped, possibly converting '+' and '%' sequences. The
		/// cookie properties may follow, separated by ';', also represented as
		/// name=value (except the secure property, which does not have a value).
		/// The name will be stored under the key "name", and the value will be
		/// stored under the key "value". This method does not do checking or
		/// validation of the parameters. It only converts the cookie string into
		/// a JSONObject.
		/// </remarks>
		/// <param name="string">The cookie specification string.</param>
		/// <returns>
		/// A JSONObject containing "name", "value", and possibly other
		/// members.
		/// </returns>
		/// <exception cref="JSONException"/>
		/// <exception cref="org.json.JSONException"/>
		public static org.json.JSONObject ToJSONObject(string @string)
		{
			string name;
			org.json.JSONObject jo = new org.json.JSONObject();
			object value;
			org.json.JSONTokener x = new org.json.JSONTokener(@string);
			jo.Put("name", x.NextTo('='));
			x.Next('=');
			jo.Put("value", x.NextTo(';'));
			x.Next();
			while (x.More())
			{
				name = Unescape(x.NextTo("=;"));
				if (x.Next() != '=')
				{
					if (name.Equals("secure"))
					{
						value = true;
					}
					else
					{
						throw x.SyntaxError("Missing '=' in cookie parameter.");
					}
				}
				else
				{
					value = Unescape(x.NextTo(';'));
					x.Next();
				}
				jo.Put(name, value);
			}
			return jo;
		}

		/// <summary>Convert a JSONObject into a cookie specification string.</summary>
		/// <remarks>
		/// Convert a JSONObject into a cookie specification string. The JSONObject
		/// must contain "name" and "value" members.
		/// If the JSONObject contains "expires", "domain", "path", or "secure"
		/// members, they will be appended to the cookie specification string.
		/// All other members are ignored.
		/// </remarks>
		/// <param name="jo">A JSONObject</param>
		/// <returns>A cookie specification string</returns>
		/// <exception cref="JSONException"/>
		/// <exception cref="org.json.JSONException"/>
		public static string ToString(org.json.JSONObject jo)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append(Escape(jo.GetString("name")));
			sb.Append("=");
			sb.Append(Escape(jo.GetString("value")));
			if (jo.Has("expires"))
			{
				sb.Append(";expires=");
				sb.Append(jo.GetString("expires"));
			}
			if (jo.Has("domain"))
			{
				sb.Append(";domain=");
				sb.Append(Escape(jo.GetString("domain")));
			}
			if (jo.Has("path"))
			{
				sb.Append(";path=");
				sb.Append(Escape(jo.GetString("path")));
			}
			if (jo.OptBoolean("secure"))
			{
				sb.Append(";secure");
			}
			return sb.ToString();
		}

		/// <summary>
		/// Convert <code>%</code><i>hh</i> sequences to single characters, and
		/// convert plus to space.
		/// </summary>
		/// <param name="string">
		/// A string that may contain
		/// <code>+</code>&nbsp;<small>(plus)</small> and
		/// <code>%</code><i>hh</i> sequences.
		/// </param>
		/// <returns>The unescaped string.</returns>
		public static string Unescape(string @string)
		{
			int length = @string.Length;
			System.Text.StringBuilder sb = new System.Text.StringBuilder(length);
			for (int i = 0; i < length; ++i)
			{
				char c = @string[i];
				if (c == '+')
				{
					c = ' ';
				}
				else
				{
					if (c == '%' && i + 2 < length)
					{
						int d = org.json.JSONTokener.Dehexchar(@string[i + 1]);
						int e = org.json.JSONTokener.Dehexchar(@string[i + 2]);
						if (d >= 0 && e >= 0)
						{
							c = (char)(d * 16 + e);
							i += 2;
						}
					}
				}
				sb.Append(c);
			}
			return sb.ToString();
		}
	}
}
