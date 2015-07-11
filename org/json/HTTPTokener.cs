using Sharpen;

namespace org.json
{
	/// <summary>
	/// The HTTPTokener extends the JSONTokener to provide additional methods
	/// for the parsing of HTTP headers.
	/// </summary>
	/// <author>JSON.org</author>
	/// <version>2014-05-03</version>
	public class HTTPTokener : org.json.JSONTokener
	{
		/// <summary>Construct an HTTPTokener from a string.</summary>
		/// <param name="string">A source string.</param>
		public HTTPTokener(string @string)
			: base(@string)
		{
		}

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
		/// <summary>Get the next token or string.</summary>
		/// <remarks>Get the next token or string. This is used in parsing HTTP headers.</remarks>
		/// <exception cref="JSONException"/>
		/// <returns>A String.</returns>
		/// <exception cref="org.json.JSONException"/>
		public virtual string nextToken()
		{
			char c;
			char q;
			java.lang.StringBuilder sb = new java.lang.StringBuilder();
			do
			{
				c = next();
			}
			while (char.IsWhiteSpace(c));
			if (c == '"' || c == '\'')
			{
				q = c;
				for (; ; )
				{
					c = next();
					if (c < ' ')
					{
						throw syntaxError("Unterminated string.");
					}
					if (c == q)
					{
						return sb.ToString();
					}
					sb.Append(c);
				}
			}
			for (; ; )
			{
				if (c == 0 || char.IsWhiteSpace(c))
				{
					return sb.ToString();
				}
				sb.Append(c);
				c = next();
			}
		}
	}
}
