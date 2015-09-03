using Sharpen;

namespace org.json
{
	/// <summary>Convert an HTTP header to a JSONObject and back.</summary>
	/// <author>JSON.org</author>
	/// <version>2014-05-03</version>
	public class HTTP
	{
		/// <summary>Carriage return/line feed.</summary>
		public const string CRLF = "\r\n";

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
		/// <summary>Convert an HTTP header string into a JSONObject.</summary>
		/// <remarks>
		/// Convert an HTTP header string into a JSONObject. It can be a request
		/// header or a response header. A request header will contain
		/// <pre>{
		/// Method: "POST" (for example),
		/// "Request-URI": "/" (for example),
		/// "HTTP-Version": "HTTP/1.1" (for example)
		/// }</pre>
		/// A response header will contain
		/// <pre>{
		/// "HTTP-Version": "HTTP/1.1" (for example),
		/// "Status-Code": "200" (for example),
		/// "Reason-Phrase": "OK" (for example)
		/// }</pre>
		/// In addition, the other parameters in the header will be captured, using
		/// the HTTP field names as JSON names, so that <pre>
		/// Date: Sun, 26 May 2002 18:06:04 GMT
		/// Cookie: Q=q2=PPEAsg--; B=677gi6ouf29bn&b=2&f=s
		/// Cache-Control: no-cache</pre>
		/// become
		/// <pre>{...
		/// Date: "Sun, 26 May 2002 18:06:04 GMT",
		/// Cookie: "Q=q2=PPEAsg--; B=677gi6ouf29bn&b=2&f=s",
		/// "Cache-Control": "no-cache",
		/// ...}</pre>
		/// It does no further checking or conversion. It does not parse dates.
		/// It does not do '%' transforms on URLs.
		/// </remarks>
		/// <param name="string">An HTTP header string.</param>
		/// <returns>
		/// A JSONObject containing the elements and attributes
		/// of the XML string.
		/// </returns>
		/// <exception cref="JSONException"/>
		/// <exception cref="org.json.JSONException"/>
		public static org.json.JSONObject ToJSONObject(string @string)
		{
			org.json.JSONObject jo = new org.json.JSONObject();
			org.json.HTTPTokener x = new org.json.HTTPTokener(@string);
			string token;
			token = x.NextToken();
			if (token.ToUpper().StartsWith("HTTP"))
			{
				// Response
				jo.Put("HTTP-Version", token);
				jo.Put("Status-Code", x.NextToken());
				jo.Put("Reason-Phrase", x.NextTo('\0'));
				x.Next();
			}
			else
			{
				// Request
				jo.Put("Method", token);
				jo.Put("Request-URI", x.NextToken());
				jo.Put("HTTP-Version", x.NextToken());
			}
			// Fields
			while (x.More())
			{
				string name = x.NextTo(':');
				x.Next(':');
				jo.Put(name, x.NextTo('\0'));
				x.Next();
			}
			return jo;
		}

		/// <summary>Convert a JSONObject into an HTTP header.</summary>
		/// <remarks>
		/// Convert a JSONObject into an HTTP header. A request header must contain
		/// <pre>{
		/// Method: "POST" (for example),
		/// "Request-URI": "/" (for example),
		/// "HTTP-Version": "HTTP/1.1" (for example)
		/// }</pre>
		/// A response header must contain
		/// <pre>{
		/// "HTTP-Version": "HTTP/1.1" (for example),
		/// "Status-Code": "200" (for example),
		/// "Reason-Phrase": "OK" (for example)
		/// }</pre>
		/// Any other members of the JSONObject will be output as HTTP fields.
		/// The result will end with two CRLF pairs.
		/// </remarks>
		/// <param name="jo">A JSONObject</param>
		/// <returns>An HTTP header string.</returns>
		/// <exception cref="JSONException">
		/// if the object does not contain enough
		/// information.
		/// </exception>
		/// <exception cref="org.json.JSONException"/>
		public static string ToString(org.json.JSONObject jo)
		{
			System.Collections.Generic.IEnumerator<string> keys = jo.Keys();
			string @string;
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			if (jo.Has("Status-Code") && jo.Has("Reason-Phrase"))
			{
				sb.Append(jo.GetString("HTTP-Version"));
				sb.Append(' ');
				sb.Append(jo.GetString("Status-Code"));
				sb.Append(' ');
				sb.Append(jo.GetString("Reason-Phrase"));
			}
			else
			{
				if (jo.Has("Method") && jo.Has("Request-URI"))
				{
					sb.Append(jo.GetString("Method"));
					sb.Append(' ');
					sb.Append('"');
					sb.Append(jo.GetString("Request-URI"));
					sb.Append('"');
					sb.Append(' ');
					sb.Append(jo.GetString("HTTP-Version"));
				}
				else
				{
					throw new org.json.JSONException("Not enough material for an HTTP header.");
				}
			}
			sb.Append(CRLF);
			while (keys.HasNext())
			{
				@string = keys.Next();
				if (!"HTTP-Version".Equals(@string) && !"Status-Code".Equals(@string) && !"Reason-Phrase".Equals(@string) && !"Method".Equals(@string) && !"Request-URI".Equals(@string) && !jo.IsNull(@string))
				{
					sb.Append(@string);
					sb.Append(": ");
					sb.Append(jo.GetString(@string));
					sb.Append(CRLF);
				}
			}
			sb.Append(CRLF);
			return sb.ToString();
		}
	}
}
