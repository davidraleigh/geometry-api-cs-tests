using Sharpen;

namespace org.json
{
	/// <summary>JSONWriter provides a quick and convenient way of producing JSON text.</summary>
	/// <remarks>
	/// JSONWriter provides a quick and convenient way of producing JSON text.
	/// The texts produced strictly conform to JSON syntax rules. No whitespace is
	/// added, so the results are ready for transmission or storage. Each instance of
	/// JSONWriter can produce one JSON text.
	/// <p>
	/// A JSONWriter instance provides a <code>value</code> method for appending
	/// values to the
	/// text, and a <code>key</code>
	/// method for adding keys before values in objects. There are <code>array</code>
	/// and <code>endArray</code> methods that make and bound array values, and
	/// <code>object</code> and <code>endObject</code> methods which make and bound
	/// object values. All of these methods return the JSONWriter instance,
	/// permitting a cascade style. For example, <pre>
	/// new JSONWriter(myWriter)
	/// .object()
	/// .key("JSON")
	/// .value("Hello, World!")
	/// .endObject();</pre> which writes <pre>
	/// {"JSON":"Hello, World!"}</pre>
	/// <p>
	/// The first method called must be <code>array</code> or <code>object</code>.
	/// There are no methods for adding commas or colons. JSONWriter adds them for
	/// you. Objects and arrays can be nested up to 20 levels deep.
	/// <p>
	/// This can sometimes be easier than using a JSONObject to build a string.
	/// </remarks>
	/// <author>JSON.org</author>
	/// <version>2011-11-24</version>
	public class JSONWriter
	{
		private const int maxdepth = 200;

		/// <summary>
		/// The comma flag determines if a comma should be output before the next
		/// value.
		/// </summary>
		private bool comma;

		/// <summary>The current mode.</summary>
		/// <remarks>
		/// The current mode. Values:
		/// 'a' (array),
		/// 'd' (done),
		/// 'i' (initial),
		/// 'k' (key),
		/// 'o' (object).
		/// </remarks>
		protected internal char mode;

		/// <summary>The object/array stack.</summary>
		private readonly org.json.JSONObject[] stack;

		/// <summary>The stack top index.</summary>
		/// <remarks>The stack top index. A value of 0 indicates that the stack is empty.</remarks>
		private int top;

		/// <summary>The writer that will receive the output.</summary>
		protected internal System.IO.TextWriter writer;

		/// <summary>Make a fresh JSONWriter.</summary>
		/// <remarks>Make a fresh JSONWriter. It can be used to build one JSON text.</remarks>
		public JSONWriter(System.IO.TextWriter w)
		{
			/*
			Copyright (c) 2006 JSON.org
			
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
			this.comma = false;
			this.mode = 'i';
			this.stack = new org.json.JSONObject[maxdepth];
			this.top = 0;
			this.writer = w;
		}

		/// <summary>Append a value.</summary>
		/// <param name="string">A string value.</param>
		/// <returns>this</returns>
		/// <exception cref="JSONException">If the value is out of sequence.</exception>
		/// <exception cref="org.json.JSONException"/>
		private org.json.JSONWriter Append(string @string)
		{
			if (@string == null)
			{
				throw new org.json.JSONException("Null pointer");
			}
			if (this.mode == 'o' || this.mode == 'a')
			{
				try
				{
					if (this.comma && this.mode == 'a')
					{
						this.writer.Write(',');
					}
					this.writer.Write(@string);
				}
				catch (System.IO.IOException e)
				{
					throw new org.json.JSONException(e);
				}
				if (this.mode == 'o')
				{
					this.mode = 'k';
				}
				this.comma = true;
				return this;
			}
			throw new org.json.JSONException("Value out of sequence.");
		}

		/// <summary>Begin appending a new array.</summary>
		/// <remarks>
		/// Begin appending a new array. All values until the balancing
		/// <code>endArray</code> will be appended to this array. The
		/// <code>endArray</code> method must be called to mark the array's end.
		/// </remarks>
		/// <returns>this</returns>
		/// <exception cref="JSONException">
		/// If the nesting is too deep, or if the object is
		/// started in the wrong place (for example as a key or after the end of the
		/// outermost array or object).
		/// </exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual org.json.JSONWriter Array()
		{
			if (this.mode == 'i' || this.mode == 'o' || this.mode == 'a')
			{
				this.Push(null);
				this.Append("[");
				this.comma = false;
				return this;
			}
			throw new org.json.JSONException("Misplaced array.");
		}

		/// <summary>End something.</summary>
		/// <param name="mode">Mode</param>
		/// <param name="c">Closing character</param>
		/// <returns>this</returns>
		/// <exception cref="JSONException">If unbalanced.</exception>
		/// <exception cref="org.json.JSONException"/>
		private org.json.JSONWriter End(char mode, char c)
		{
			if (this.mode != mode)
			{
				throw new org.json.JSONException(mode == 'a' ? "Misplaced endArray." : "Misplaced endObject."
					);
			}
			this.Pop(mode);
			try
			{
				this.writer.Write(c);
			}
			catch (System.IO.IOException e)
			{
				throw new org.json.JSONException(e);
			}
			this.comma = true;
			return this;
		}

		/// <summary>End an array.</summary>
		/// <remarks>
		/// End an array. This method most be called to balance calls to
		/// <code>array</code>.
		/// </remarks>
		/// <returns>this</returns>
		/// <exception cref="JSONException">If incorrectly nested.</exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual org.json.JSONWriter EndArray()
		{
			return this.End('a', ']');
		}

		/// <summary>End an object.</summary>
		/// <remarks>
		/// End an object. This method most be called to balance calls to
		/// <code>object</code>.
		/// </remarks>
		/// <returns>this</returns>
		/// <exception cref="JSONException">If incorrectly nested.</exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual org.json.JSONWriter EndObject()
		{
			return this.End('k', '}');
		}

		/// <summary>Append a key.</summary>
		/// <remarks>
		/// Append a key. The key will be associated with the next value. In an
		/// object, every value must be preceded by a key.
		/// </remarks>
		/// <param name="string">A key string.</param>
		/// <returns>this</returns>
		/// <exception cref="JSONException">
		/// If the key is out of place. For example, keys
		/// do not belong in arrays or if the key is null.
		/// </exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual org.json.JSONWriter Key(string @string)
		{
			if (@string == null)
			{
				throw new org.json.JSONException("Null key.");
			}
			if (this.mode == 'k')
			{
				try
				{
					this.stack[this.top - 1].PutOnce(@string, true);
					if (this.comma)
					{
						this.writer.Write(',');
					}
					this.writer.Write(org.json.JSONObject.Quote(@string));
					this.writer.Write(':');
					this.comma = false;
					this.mode = 'o';
					return this;
				}
				catch (System.IO.IOException e)
				{
					throw new org.json.JSONException(e);
				}
			}
			throw new org.json.JSONException("Misplaced key.");
		}

		/// <summary>Begin appending a new object.</summary>
		/// <remarks>
		/// Begin appending a new object. All keys and values until the balancing
		/// <code>endObject</code> will be appended to this object. The
		/// <code>endObject</code> method must be called to mark the object's end.
		/// </remarks>
		/// <returns>this</returns>
		/// <exception cref="JSONException">
		/// If the nesting is too deep, or if the object is
		/// started in the wrong place (for example as a key or after the end of the
		/// outermost array or object).
		/// </exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual org.json.JSONWriter Object()
		{
			if (this.mode == 'i')
			{
				this.mode = 'o';
			}
			if (this.mode == 'o' || this.mode == 'a')
			{
				this.Append("{");
				this.Push(new org.json.JSONObject());
				this.comma = false;
				return this;
			}
			throw new org.json.JSONException("Misplaced object.");
		}

		/// <summary>Pop an array or object scope.</summary>
		/// <param name="c">The scope to close.</param>
		/// <exception cref="JSONException">If nesting is wrong.</exception>
		/// <exception cref="org.json.JSONException"/>
		private void Pop(char c)
		{
			if (this.top <= 0)
			{
				throw new org.json.JSONException("Nesting error.");
			}
			char m = this.stack[this.top - 1] == null ? 'a' : 'k';
			if (m != c)
			{
				throw new org.json.JSONException("Nesting error.");
			}
			this.top -= 1;
			this.mode = this.top == 0 ? 'd' : this.stack[this.top - 1] == null ? 'a' : 'k';
		}

		/// <summary>Push an array or object scope.</summary>
		/// <param name="jo">The scope to open.</param>
		/// <exception cref="JSONException">If nesting is too deep.</exception>
		/// <exception cref="org.json.JSONException"/>
		private void Push(org.json.JSONObject jo)
		{
			if (this.top >= maxdepth)
			{
				throw new org.json.JSONException("Nesting too deep.");
			}
			this.stack[this.top] = jo;
			this.mode = jo == null ? 'a' : 'k';
			this.top += 1;
		}

		/// <summary>
		/// Append either the value <code>true</code> or the value
		/// <code>false</code>.
		/// </summary>
		/// <param name="b">A boolean.</param>
		/// <returns>this</returns>
		/// <exception cref="JSONException"/>
		/// <exception cref="org.json.JSONException"/>
		public virtual org.json.JSONWriter Value(bool b)
		{
			return this.Append(b ? "true" : "false");
		}

		/// <summary>Append a double value.</summary>
		/// <param name="d">A double.</param>
		/// <returns>this</returns>
		/// <exception cref="JSONException">If the number is not finite.</exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual org.json.JSONWriter Value(double d)
		{
			return this.Value(d);
		}

		/// <summary>Append a long value.</summary>
		/// <param name="l">A long.</param>
		/// <returns>this</returns>
		/// <exception cref="JSONException"/>
		/// <exception cref="org.json.JSONException"/>
		public virtual org.json.JSONWriter Value(long l)
		{
			return this.Append(System.Convert.ToString(l));
		}

		/// <summary>Append an object value.</summary>
		/// <param name="object">
		/// The object to append. It can be null, or a Boolean, Number,
		/// String, JSONObject, or JSONArray, or an object that implements JSONString.
		/// </param>
		/// <returns>this</returns>
		/// <exception cref="JSONException">If the value is out of sequence.</exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual org.json.JSONWriter Value(object @object)
		{
			return this.Append(org.json.JSONObject.ValueToString(@object));
		}
	}
}
