using System.Collections.Generic;
using System.Text;
using Sharpen;

namespace org.json
{
	/// <summary>Convert a web browser cookie list string to a JSONObject and back.</summary>
	/// <author>JSON.org</author>
	/// <version>2014-05-03</version>
	public class CookieList
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
		/// <summary>Convert a cookie list into a JSONObject.</summary>
		/// <remarks>
		/// Convert a cookie list into a JSONObject. A cookie list is a sequence
		/// of name/value pairs. The names are separated from the values by '='.
		/// The pairs are separated by ';'. The names and the values
		/// will be unescaped, possibly converting '+' and '%' sequences.
		/// To add a cookie to a cooklist,
		/// cookielistJSONObject.put(cookieJSONObject.getString("name"),
		/// cookieJSONObject.getString("value"));
		/// </remarks>
		/// <param name="string">A cookie list string</param>
		/// <returns>A JSONObject</returns>
		/// <exception cref="JSONException"/>
		/// <exception cref="org.json.JSONException"/>
		public static JSONObject ToJSONObject(string @string)
		{
			JSONObject jo = new JSONObject();
			JSONTokener x = new JSONTokener(@string);
			while (x.More())
			{
				string name = Cookie.Unescape(x.NextTo('='));
				x.Next('=');
				jo.Put(name, Cookie.Unescape(x.NextTo(';')));
				x.Next();
			}
			return jo;
		}

		/// <summary>Convert a JSONObject into a cookie list.</summary>
		/// <remarks>
		/// Convert a JSONObject into a cookie list. A cookie list is a sequence
		/// of name/value pairs. The names are separated from the values by '='.
		/// The pairs are separated by ';'. The characters '%', '+', '=', and ';'
		/// in the names and values are replaced by "%hh".
		/// </remarks>
		/// <param name="jo">A JSONObject</param>
		/// <returns>A cookie list string</returns>
		/// <exception cref="JSONException"/>
		/// <exception cref="org.json.JSONException"/>
		public static string ToString(JSONObject jo)
		{
			bool b = false;
			IEnumerator<string> keys = jo.Keys();
			string @string;
			StringBuilder sb = new StringBuilder();
			while (keys.HasNext())
			{
				@string = keys.Next();
				if (!jo.IsNull(@string))
				{
					if (b)
					{
						sb.Append(';');
					}
					sb.Append(Cookie.Escape(@string));
					sb.Append("=");
					sb.Append(Cookie.Escape(jo.GetString(@string)));
					b = true;
				}
			}
			return sb.ToString();
		}
	}
}
