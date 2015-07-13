using Sharpen;

namespace org.json
{
	/// <summary>A JSONArray is an ordered sequence of values.</summary>
	/// <remarks>
	/// A JSONArray is an ordered sequence of values. Its external text form is a
	/// string wrapped in square brackets with commas separating the values. The
	/// internal form is an object having <code>get</code> and <code>opt</code>
	/// methods for accessing the values by index, and <code>put</code> methods for
	/// adding or replacing values. The values can be any of these types:
	/// <code>Boolean</code>, <code>JSONArray</code>, <code>JSONObject</code>,
	/// <code>Number</code>, <code>String</code>, or the
	/// <code>JSONObject.NULL object</code>.
	/// <p>
	/// The constructor can convert a JSON text into a Java object. The
	/// <code>toString</code> method converts to JSON text.
	/// <p>
	/// A <code>get</code> method returns a value if one can be found, and throws an
	/// exception if one cannot be found. An <code>opt</code> method returns a
	/// default value instead of throwing an exception, and so is useful for
	/// obtaining optional values.
	/// <p>
	/// The generic <code>get()</code> and <code>opt()</code> methods return an
	/// object which you can cast or query for type. There are also typed
	/// <code>get</code> and <code>opt</code> methods that do type checking and type
	/// coercion for you.
	/// <p>
	/// The texts produced by the <code>toString</code> methods strictly conform to
	/// JSON syntax rules. The constructors are more forgiving in the texts they will
	/// accept:
	/// <ul>
	/// <li>An extra <code>,</code>&nbsp;<small>(comma)</small> may appear just
	/// before the closing bracket.</li>
	/// <li>The <code>null</code> value will be inserted when there is <code>,</code>
	/// &nbsp;<small>(comma)</small> elision.</li>
	/// <li>Strings may be quoted with <code>'</code>&nbsp;<small>(single
	/// quote)</small>.</li>
	/// <li>Strings do not need to be quoted at all if they do not begin with a quote
	/// or single quote, and if they do not contain leading or trailing spaces, and
	/// if they do not contain any of these characters:
	/// <code>{ } [ ] / \ : , #</code> and if they do not look like numbers and
	/// if they are not the reserved words <code>true</code>, <code>false</code>, or
	/// <code>null</code>.</li>
	/// </ul>
	/// </remarks>
	/// <author>JSON.org</author>
	/// <version>2015-06-04</version>
	public class JSONArray : System.Collections.Generic.IEnumerable<object>
	{
		/// <summary>The arrayList where the JSONArray's properties are kept.</summary>
		private readonly Sharpen.AList<object> myArrayList;

		/// <summary>Construct an empty JSONArray.</summary>
		public JSONArray()
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
			this.myArrayList = new Sharpen.AList<object>();
		}

		/// <summary>Construct a JSONArray from a JSONTokener.</summary>
		/// <param name="x">A JSONTokener</param>
		/// <exception cref="JSONException">If there is a syntax error.</exception>
		/// <exception cref="org.json.JSONException"/>
		public JSONArray(org.json.JSONTokener x)
			: this()
		{
			if (x.NextClean() != '[')
			{
				throw x.SyntaxError("A JSONArray text must start with '['");
			}
			if (x.NextClean() != ']')
			{
				x.Back();
				for (; ; )
				{
					if (x.NextClean() == ',')
					{
						x.Back();
						this.myArrayList.Add(org.json.JSONObject.NULL);
					}
					else
					{
						x.Back();
						this.myArrayList.Add(x.NextValue());
					}
					switch (x.NextClean())
					{
						case ',':
						{
							if (x.NextClean() == ']')
							{
								return;
							}
							x.Back();
							break;
						}

						case ']':
						{
							return;
						}

						default:
						{
							throw x.SyntaxError("Expected a ',' or ']'");
						}
					}
				}
			}
		}

		/// <summary>Construct a JSONArray from a source JSON text.</summary>
		/// <param name="source">
		/// A string that begins with <code>[</code>&nbsp;<small>(left
		/// bracket)</small> and ends with <code>]</code>
		/// &nbsp;<small>(right bracket)</small>.
		/// </param>
		/// <exception cref="JSONException">If there is a syntax error.</exception>
		/// <exception cref="org.json.JSONException"/>
		public JSONArray(string source)
			: this(new org.json.JSONTokener(source))
		{
		}

		/// <summary>Construct a JSONArray from a Collection.</summary>
		/// <param name="collection">A Collection.</param>
		public JSONArray(System.Collections.Generic.ICollection<object> collection)
		{
			this.myArrayList = new Sharpen.AList<object>();
			if (collection != null)
			{
				System.Collections.Generic.IEnumerator<object> iter = collection.GetEnumerator();
				while (iter.HasNext())
				{
					this.myArrayList.Add(org.json.JSONObject.Wrap(iter.Next()));
				}
			}
		}

		/// <summary>Construct a JSONArray from an array</summary>
		/// <exception cref="JSONException">If not an array.</exception>
		/// <exception cref="org.json.JSONException"/>
		public JSONArray(object array)
			: this()
		{
			if (Sharpen.Runtime.GetClassForObject(array).IsArray())
			{
				int length = java.lang.reflect.Array.GetLength(array);
				for (int i = 0; i < length; i += 1)
				{
					this.Put(org.json.JSONObject.Wrap(java.lang.reflect.Array.Get(array, i)));
				}
			}
			else
			{
				throw new org.json.JSONException("JSONArray initial value should be a string or collection or array."
					);
			}
		}

		public override System.Collections.Generic.IEnumerator<object> GetEnumerator()
		{
			return myArrayList.GetEnumerator();
		}

		/// <summary>Get the object value associated with an index.</summary>
		/// <param name="index">The index must be between 0 and length() - 1.</param>
		/// <returns>An object value.</returns>
		/// <exception cref="JSONException">If there is no value for the index.</exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual object Get(int index)
		{
			object @object = this.Opt(index);
			if (@object == null)
			{
				throw new org.json.JSONException("JSONArray[" + index + "] not found.");
			}
			return @object;
		}

		/// <summary>Get the boolean value associated with an index.</summary>
		/// <remarks>
		/// Get the boolean value associated with an index. The string values "true"
		/// and "false" are converted to boolean.
		/// </remarks>
		/// <param name="index">The index must be between 0 and length() - 1.</param>
		/// <returns>The truth.</returns>
		/// <exception cref="JSONException">
		/// If there is no value for the index or if the value is not
		/// convertible to boolean.
		/// </exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual bool GetBoolean(int index)
		{
			object @object = this.Get(index);
			if (@object.Equals(false) || (@object is string && Sharpen.Runtime.EqualsIgnoreCase
				(((string)@object), "false")))
			{
				return false;
			}
			else
			{
				if (@object.Equals(true) || (@object is string && Sharpen.Runtime.EqualsIgnoreCase
					(((string)@object), "true")))
				{
					return true;
				}
			}
			throw new org.json.JSONException("JSONArray[" + index + "] is not a boolean.");
		}

		/// <summary>Get the double value associated with an index.</summary>
		/// <param name="index">The index must be between 0 and length() - 1.</param>
		/// <returns>The value.</returns>
		/// <exception cref="JSONException">
		/// If the key is not found or if the value cannot be converted
		/// to a number.
		/// </exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual double GetDouble(int index)
		{
			object @object = this.Get(index);
			try
			{
				return @object is java.lang.Number ? ((java.lang.Number)@object) : double.Parse((
					string)@object);
			}
			catch (System.Exception)
			{
				throw new org.json.JSONException("JSONArray[" + index + "] is not a number.");
			}
		}

		/// <summary>Get the int value associated with an index.</summary>
		/// <param name="index">The index must be between 0 and length() - 1.</param>
		/// <returns>The value.</returns>
		/// <exception cref="JSONException">If the key is not found or if the value is not a number.
		/// 	</exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual int GetInt(int index)
		{
			object @object = this.Get(index);
			try
			{
				return @object is java.lang.Number ? ((java.lang.Number)@object) : System.Convert.ToInt32
					((string)@object);
			}
			catch (System.Exception)
			{
				throw new org.json.JSONException("JSONArray[" + index + "] is not a number.");
			}
		}

		/// <summary>Get the JSONArray associated with an index.</summary>
		/// <param name="index">The index must be between 0 and length() - 1.</param>
		/// <returns>A JSONArray value.</returns>
		/// <exception cref="JSONException">
		/// If there is no value for the index. or if the value is not a
		/// JSONArray
		/// </exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual org.json.JSONArray GetJSONArray(int index)
		{
			object @object = this.Get(index);
			if (@object is org.json.JSONArray)
			{
				return (org.json.JSONArray)@object;
			}
			throw new org.json.JSONException("JSONArray[" + index + "] is not a JSONArray.");
		}

		/// <summary>Get the JSONObject associated with an index.</summary>
		/// <param name="index">subscript</param>
		/// <returns>A JSONObject value.</returns>
		/// <exception cref="JSONException">
		/// If there is no value for the index or if the value is not a
		/// JSONObject
		/// </exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual org.json.JSONObject GetJSONObject(int index)
		{
			object @object = this.Get(index);
			if (@object is org.json.JSONObject)
			{
				return (org.json.JSONObject)@object;
			}
			throw new org.json.JSONException("JSONArray[" + index + "] is not a JSONObject.");
		}

		/// <summary>Get the long value associated with an index.</summary>
		/// <param name="index">The index must be between 0 and length() - 1.</param>
		/// <returns>The value.</returns>
		/// <exception cref="JSONException">
		/// If the key is not found or if the value cannot be converted
		/// to a number.
		/// </exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual long GetLong(int index)
		{
			object @object = this.Get(index);
			try
			{
				return @object is java.lang.Number ? ((java.lang.Number)@object) : System.Convert.ToInt64
					((string)@object);
			}
			catch (System.Exception)
			{
				throw new org.json.JSONException("JSONArray[" + index + "] is not a number.");
			}
		}

		/// <summary>Get the string associated with an index.</summary>
		/// <param name="index">The index must be between 0 and length() - 1.</param>
		/// <returns>A string value.</returns>
		/// <exception cref="JSONException">If there is no string value for the index.</exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual string GetString(int index)
		{
			object @object = this.Get(index);
			if (@object is string)
			{
				return (string)@object;
			}
			throw new org.json.JSONException("JSONArray[" + index + "] not a string.");
		}

		/// <summary>Determine if the value is null.</summary>
		/// <param name="index">The index must be between 0 and length() - 1.</param>
		/// <returns>true if the value at the index is null, or if there is no value.</returns>
		public virtual bool IsNull(int index)
		{
			return org.json.JSONObject.NULL.Equals(this.Opt(index));
		}

		/// <summary>Make a string from the contents of this JSONArray.</summary>
		/// <remarks>
		/// Make a string from the contents of this JSONArray. The
		/// <code>separator</code> string is inserted between each element. Warning:
		/// This method assumes that the data structure is acyclical.
		/// </remarks>
		/// <param name="separator">A string that will be inserted between the elements.</param>
		/// <returns>a string.</returns>
		/// <exception cref="JSONException">If the array contains an invalid number.</exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual string Join(string separator)
		{
			int len = this.Length();
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			for (int i = 0; i < len; i += 1)
			{
				if (i > 0)
				{
					sb.Append(separator);
				}
				sb.Append(org.json.JSONObject.ValueToString(this.myArrayList[i]));
			}
			return sb.ToString();
		}

		/// <summary>Get the number of elements in the JSONArray, included nulls.</summary>
		/// <returns>The length (or size).</returns>
		public virtual int Length()
		{
			return this.myArrayList.Count;
		}

		/// <summary>Get the optional object value associated with an index.</summary>
		/// <param name="index">The index must be between 0 and length() - 1.</param>
		/// <returns>An object value, or null if there is no object at that index.</returns>
		public virtual object Opt(int index)
		{
			return (index < 0 || index >= this.Length()) ? null : this.myArrayList[index];
		}

		/// <summary>Get the optional boolean value associated with an index.</summary>
		/// <remarks>
		/// Get the optional boolean value associated with an index. It returns false
		/// if there is no value at that index, or if the value is not Boolean.TRUE
		/// or the String "true".
		/// </remarks>
		/// <param name="index">The index must be between 0 and length() - 1.</param>
		/// <returns>The truth.</returns>
		public virtual bool OptBoolean(int index)
		{
			return this.OptBoolean(index, false);
		}

		/// <summary>Get the optional boolean value associated with an index.</summary>
		/// <remarks>
		/// Get the optional boolean value associated with an index. It returns the
		/// defaultValue if there is no value at that index or if it is not a Boolean
		/// or the String "true" or "false" (case insensitive).
		/// </remarks>
		/// <param name="index">The index must be between 0 and length() - 1.</param>
		/// <param name="defaultValue">A boolean default.</param>
		/// <returns>The truth.</returns>
		public virtual bool OptBoolean(int index, bool defaultValue)
		{
			try
			{
				return this.GetBoolean(index);
			}
			catch (System.Exception)
			{
				return defaultValue;
			}
		}

		/// <summary>Get the optional double value associated with an index.</summary>
		/// <remarks>
		/// Get the optional double value associated with an index. NaN is returned
		/// if there is no value for the index, or if the value is not a number and
		/// cannot be converted to a number.
		/// </remarks>
		/// <param name="index">The index must be between 0 and length() - 1.</param>
		/// <returns>The value.</returns>
		public virtual double OptDouble(int index)
		{
			return this.OptDouble(index, double.NaN);
		}

		/// <summary>Get the optional double value associated with an index.</summary>
		/// <remarks>
		/// Get the optional double value associated with an index. The defaultValue
		/// is returned if there is no value for the index, or if the value is not a
		/// number and cannot be converted to a number.
		/// </remarks>
		/// <param name="index">subscript</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value.</returns>
		public virtual double OptDouble(int index, double defaultValue)
		{
			try
			{
				return this.GetDouble(index);
			}
			catch (System.Exception)
			{
				return defaultValue;
			}
		}

		/// <summary>Get the optional int value associated with an index.</summary>
		/// <remarks>
		/// Get the optional int value associated with an index. Zero is returned if
		/// there is no value for the index, or if the value is not a number and
		/// cannot be converted to a number.
		/// </remarks>
		/// <param name="index">The index must be between 0 and length() - 1.</param>
		/// <returns>The value.</returns>
		public virtual int OptInt(int index)
		{
			return this.OptInt(index, 0);
		}

		/// <summary>Get the optional int value associated with an index.</summary>
		/// <remarks>
		/// Get the optional int value associated with an index. The defaultValue is
		/// returned if there is no value for the index, or if the value is not a
		/// number and cannot be converted to a number.
		/// </remarks>
		/// <param name="index">The index must be between 0 and length() - 1.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value.</returns>
		public virtual int OptInt(int index, int defaultValue)
		{
			try
			{
				return this.GetInt(index);
			}
			catch (System.Exception)
			{
				return defaultValue;
			}
		}

		/// <summary>Get the optional JSONArray associated with an index.</summary>
		/// <param name="index">subscript</param>
		/// <returns>
		/// A JSONArray value, or null if the index has no value, or if the
		/// value is not a JSONArray.
		/// </returns>
		public virtual org.json.JSONArray OptJSONArray(int index)
		{
			object o = this.Opt(index);
			return o is org.json.JSONArray ? (org.json.JSONArray)o : null;
		}

		/// <summary>Get the optional JSONObject associated with an index.</summary>
		/// <remarks>
		/// Get the optional JSONObject associated with an index. Null is returned if
		/// the key is not found, or null if the index has no value, or if the value
		/// is not a JSONObject.
		/// </remarks>
		/// <param name="index">The index must be between 0 and length() - 1.</param>
		/// <returns>A JSONObject value.</returns>
		public virtual org.json.JSONObject OptJSONObject(int index)
		{
			object o = this.Opt(index);
			return o is org.json.JSONObject ? (org.json.JSONObject)o : null;
		}

		/// <summary>Get the optional long value associated with an index.</summary>
		/// <remarks>
		/// Get the optional long value associated with an index. Zero is returned if
		/// there is no value for the index, or if the value is not a number and
		/// cannot be converted to a number.
		/// </remarks>
		/// <param name="index">The index must be between 0 and length() - 1.</param>
		/// <returns>The value.</returns>
		public virtual long OptLong(int index)
		{
			return this.OptLong(index, 0);
		}

		/// <summary>Get the optional long value associated with an index.</summary>
		/// <remarks>
		/// Get the optional long value associated with an index. The defaultValue is
		/// returned if there is no value for the index, or if the value is not a
		/// number and cannot be converted to a number.
		/// </remarks>
		/// <param name="index">The index must be between 0 and length() - 1.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value.</returns>
		public virtual long OptLong(int index, long defaultValue)
		{
			try
			{
				return this.GetLong(index);
			}
			catch (System.Exception)
			{
				return defaultValue;
			}
		}

		/// <summary>Get the optional string value associated with an index.</summary>
		/// <remarks>
		/// Get the optional string value associated with an index. It returns an
		/// empty string if there is no value at that index. If the value is not a
		/// string and is not null, then it is coverted to a string.
		/// </remarks>
		/// <param name="index">The index must be between 0 and length() - 1.</param>
		/// <returns>A String value.</returns>
		public virtual string OptString(int index)
		{
			return this.OptString(index, string.Empty);
		}

		/// <summary>Get the optional string associated with an index.</summary>
		/// <remarks>
		/// Get the optional string associated with an index. The defaultValue is
		/// returned if the key is not found.
		/// </remarks>
		/// <param name="index">The index must be between 0 and length() - 1.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>A String value.</returns>
		public virtual string OptString(int index, string defaultValue)
		{
			object @object = this.Opt(index);
			return org.json.JSONObject.NULL.Equals(@object) ? defaultValue : @object.ToString
				();
		}

		/// <summary>Append a boolean value.</summary>
		/// <remarks>Append a boolean value. This increases the array's length by one.</remarks>
		/// <param name="value">A boolean value.</param>
		/// <returns>this.</returns>
		public virtual org.json.JSONArray Put(bool value)
		{
			this.Put(value ? true : false);
			return this;
		}

		/// <summary>
		/// Put a value in the JSONArray, where the value will be a JSONArray which
		/// is produced from a Collection.
		/// </summary>
		/// <param name="value">A Collection value.</param>
		/// <returns>this.</returns>
		public virtual org.json.JSONArray Put(System.Collections.Generic.ICollection<object
			> value)
		{
			this.Put(new org.json.JSONArray(value));
			return this;
		}

		/// <summary>Append a double value.</summary>
		/// <remarks>Append a double value. This increases the array's length by one.</remarks>
		/// <param name="value">A double value.</param>
		/// <exception cref="JSONException">if the value is not finite.</exception>
		/// <returns>this.</returns>
		/// <exception cref="org.json.JSONException"/>
		public virtual org.json.JSONArray Put(double value)
		{
			double d = value;
			org.json.JSONObject.TestValidity(d);
			this.Put(d);
			return this;
		}

		/// <summary>Append an int value.</summary>
		/// <remarks>Append an int value. This increases the array's length by one.</remarks>
		/// <param name="value">An int value.</param>
		/// <returns>this.</returns>
		public virtual org.json.JSONArray Put(int value)
		{
			this.Put(value);
			return this;
		}

		/// <summary>Append an long value.</summary>
		/// <remarks>Append an long value. This increases the array's length by one.</remarks>
		/// <param name="value">A long value.</param>
		/// <returns>this.</returns>
		public virtual org.json.JSONArray Put(long value)
		{
			this.Put(value);
			return this;
		}

		/// <summary>
		/// Put a value in the JSONArray, where the value will be a JSONObject which
		/// is produced from a Map.
		/// </summary>
		/// <param name="value">A Map value.</param>
		/// <returns>this.</returns>
		public virtual org.json.JSONArray Put(System.Collections.Generic.IDictionary<string
			, object> value)
		{
			this.Put(new org.json.JSONObject(value));
			return this;
		}

		/// <summary>Append an object value.</summary>
		/// <remarks>Append an object value. This increases the array's length by one.</remarks>
		/// <param name="value">
		/// An object value. The value should be a Boolean, Double,
		/// Integer, JSONArray, JSONObject, Long, or String, or the
		/// JSONObject.NULL object.
		/// </param>
		/// <returns>this.</returns>
		public virtual org.json.JSONArray Put(object value)
		{
			this.myArrayList.Add(value);
			return this;
		}

		/// <summary>Put or replace a boolean value in the JSONArray.</summary>
		/// <remarks>
		/// Put or replace a boolean value in the JSONArray. If the index is greater
		/// than the length of the JSONArray, then null elements will be added as
		/// necessary to pad it out.
		/// </remarks>
		/// <param name="index">The subscript.</param>
		/// <param name="value">A boolean value.</param>
		/// <returns>this.</returns>
		/// <exception cref="JSONException">If the index is negative.</exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual org.json.JSONArray Put(int index, bool value)
		{
			this.Put(index, value ? true : false);
			return this;
		}

		/// <summary>
		/// Put a value in the JSONArray, where the value will be a JSONArray which
		/// is produced from a Collection.
		/// </summary>
		/// <param name="index">The subscript.</param>
		/// <param name="value">A Collection value.</param>
		/// <returns>this.</returns>
		/// <exception cref="JSONException">If the index is negative or if the value is not finite.
		/// 	</exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual org.json.JSONArray Put(int index, System.Collections.Generic.ICollection
			<object> value)
		{
			this.Put(index, new org.json.JSONArray(value));
			return this;
		}

		/// <summary>Put or replace a double value.</summary>
		/// <remarks>
		/// Put or replace a double value. If the index is greater than the length of
		/// the JSONArray, then null elements will be added as necessary to pad it
		/// out.
		/// </remarks>
		/// <param name="index">The subscript.</param>
		/// <param name="value">A double value.</param>
		/// <returns>this.</returns>
		/// <exception cref="JSONException">If the index is negative or if the value is not finite.
		/// 	</exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual org.json.JSONArray Put(int index, double value)
		{
			this.Put(index, value);
			return this;
		}

		/// <summary>Put or replace an int value.</summary>
		/// <remarks>
		/// Put or replace an int value. If the index is greater than the length of
		/// the JSONArray, then null elements will be added as necessary to pad it
		/// out.
		/// </remarks>
		/// <param name="index">The subscript.</param>
		/// <param name="value">An int value.</param>
		/// <returns>this.</returns>
		/// <exception cref="JSONException">If the index is negative.</exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual org.json.JSONArray Put(int index, int value)
		{
			this.Put(index, value);
			return this;
		}

		/// <summary>Put or replace a long value.</summary>
		/// <remarks>
		/// Put or replace a long value. If the index is greater than the length of
		/// the JSONArray, then null elements will be added as necessary to pad it
		/// out.
		/// </remarks>
		/// <param name="index">The subscript.</param>
		/// <param name="value">A long value.</param>
		/// <returns>this.</returns>
		/// <exception cref="JSONException">If the index is negative.</exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual org.json.JSONArray Put(int index, long value)
		{
			this.Put(index, value);
			return this;
		}

		/// <summary>
		/// Put a value in the JSONArray, where the value will be a JSONObject that
		/// is produced from a Map.
		/// </summary>
		/// <param name="index">The subscript.</param>
		/// <param name="value">The Map value.</param>
		/// <returns>this.</returns>
		/// <exception cref="JSONException">
		/// If the index is negative or if the the value is an invalid
		/// number.
		/// </exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual org.json.JSONArray Put(int index, System.Collections.Generic.IDictionary
			<string, object> value)
		{
			this.Put(index, new org.json.JSONObject(value));
			return this;
		}

		/// <summary>Put or replace an object value in the JSONArray.</summary>
		/// <remarks>
		/// Put or replace an object value in the JSONArray. If the index is greater
		/// than the length of the JSONArray, then null elements will be added as
		/// necessary to pad it out.
		/// </remarks>
		/// <param name="index">The subscript.</param>
		/// <param name="value">
		/// The value to put into the array. The value should be a
		/// Boolean, Double, Integer, JSONArray, JSONObject, Long, or
		/// String, or the JSONObject.NULL object.
		/// </param>
		/// <returns>this.</returns>
		/// <exception cref="JSONException">
		/// If the index is negative or if the the value is an invalid
		/// number.
		/// </exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual org.json.JSONArray Put(int index, object value)
		{
			org.json.JSONObject.TestValidity(value);
			if (index < 0)
			{
				throw new org.json.JSONException("JSONArray[" + index + "] not found.");
			}
			if (index < this.Length())
			{
				this.myArrayList.Set(index, value);
			}
			else
			{
				while (index != this.Length())
				{
					this.Put(org.json.JSONObject.NULL);
				}
				this.Put(value);
			}
			return this;
		}

		/// <summary>Remove an index and close the hole.</summary>
		/// <param name="index">The index of the element to be removed.</param>
		/// <returns>
		/// The value that was associated with the index, or null if there
		/// was no value.
		/// </returns>
		public virtual object Remove(int index)
		{
			return index >= 0 && index < this.Length() ? this.myArrayList.Remove(index) : null;
		}

		/// <summary>Determine if two JSONArrays are similar.</summary>
		/// <remarks>
		/// Determine if two JSONArrays are similar.
		/// They must contain similar sequences.
		/// </remarks>
		/// <param name="other">The other JSONArray</param>
		/// <returns>true if they are equal</returns>
		public virtual bool Similar(object other)
		{
			if (!(other is org.json.JSONArray))
			{
				return false;
			}
			int len = this.Length();
			if (len != ((org.json.JSONArray)other).Length())
			{
				return false;
			}
			for (int i = 0; i < len; i += 1)
			{
				object valueThis = this.Get(i);
				object valueOther = ((org.json.JSONArray)other).Get(i);
				if (valueThis is org.json.JSONObject)
				{
					if (!((org.json.JSONObject)valueThis).Similar(valueOther))
					{
						return false;
					}
				}
				else
				{
					if (valueThis is org.json.JSONArray)
					{
						if (!((org.json.JSONArray)valueThis).Similar(valueOther))
						{
							return false;
						}
					}
					else
					{
						if (!valueThis.Equals(valueOther))
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		/// <summary>
		/// Produce a JSONObject by combining a JSONArray of names with the values of
		/// this JSONArray.
		/// </summary>
		/// <param name="names">
		/// A JSONArray containing a list of key strings. These will be
		/// paired with the values.
		/// </param>
		/// <returns>
		/// A JSONObject, or null if there are no names or if this JSONArray
		/// has no values.
		/// </returns>
		/// <exception cref="JSONException">If any of the names are null.</exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual org.json.JSONObject ToJSONObject(org.json.JSONArray names)
		{
			if (names == null || names.Length() == 0 || this.Length() == 0)
			{
				return null;
			}
			org.json.JSONObject jo = new org.json.JSONObject();
			for (int i = 0; i < names.Length(); i += 1)
			{
				jo.Put(names.GetString(i), this.Opt(i));
			}
			return jo;
		}

		/// <summary>Make a JSON text of this JSONArray.</summary>
		/// <remarks>
		/// Make a JSON text of this JSONArray. For compactness, no unnecessary
		/// whitespace is added. If it is not possible to produce a syntactically
		/// correct JSON text then null will be returned instead. This could occur if
		/// the array contains an invalid number.
		/// <p>
		/// Warning: This method assumes that the data structure is acyclical.
		/// </remarks>
		/// <returns>
		/// a printable, displayable, transmittable representation of the
		/// array.
		/// </returns>
		public override string ToString()
		{
			try
			{
				return this.ToString(0);
			}
			catch (System.Exception)
			{
				return null;
			}
		}

		/// <summary>Make a prettyprinted JSON text of this JSONArray.</summary>
		/// <remarks>
		/// Make a prettyprinted JSON text of this JSONArray. Warning: This method
		/// assumes that the data structure is acyclical.
		/// </remarks>
		/// <param name="indentFactor">The number of spaces to add to each level of indentation.
		/// 	</param>
		/// <returns>
		/// a printable, displayable, transmittable representation of the
		/// object, beginning with <code>[</code>&nbsp;<small>(left
		/// bracket)</small> and ending with <code>]</code>
		/// &nbsp;<small>(right bracket)</small>.
		/// </returns>
		/// <exception cref="JSONException"/>
		/// <exception cref="org.json.JSONException"/>
		public virtual string ToString(int indentFactor)
		{
			System.IO.StringWriter sw = new System.IO.StringWriter();
			lock (sw.GetBuffer())
			{
				return this.Write(sw, indentFactor, 0).ToString();
			}
		}

		/// <summary>Write the contents of the JSONArray as JSON text to a writer.</summary>
		/// <remarks>
		/// Write the contents of the JSONArray as JSON text to a writer. For
		/// compactness, no whitespace is added.
		/// <p>
		/// Warning: This method assumes that the data structure is acyclical.
		/// </remarks>
		/// <returns>The writer.</returns>
		/// <exception cref="JSONException"/>
		/// <exception cref="org.json.JSONException"/>
		public virtual System.IO.TextWriter Write(System.IO.TextWriter writer)
		{
			return this.Write(writer, 0, 0);
		}

		/// <summary>Write the contents of the JSONArray as JSON text to a writer.</summary>
		/// <remarks>
		/// Write the contents of the JSONArray as JSON text to a writer. For
		/// compactness, no whitespace is added.
		/// <p>
		/// Warning: This method assumes that the data structure is acyclical.
		/// </remarks>
		/// <param name="indentFactor">The number of spaces to add to each level of indentation.
		/// 	</param>
		/// <param name="indent">The indention of the top level.</param>
		/// <returns>The writer.</returns>
		/// <exception cref="JSONException"/>
		/// <exception cref="org.json.JSONException"/>
		internal virtual System.IO.TextWriter Write(System.IO.TextWriter writer, int indentFactor
			, int indent)
		{
			try
			{
				bool commanate = false;
				int length = this.Length();
				writer.Write('[');
				if (length == 1)
				{
					org.json.JSONObject.WriteValue(writer, this.myArrayList[0], indentFactor, indent);
				}
				else
				{
					if (length != 0)
					{
						int newindent = indent + indentFactor;
						for (int i = 0; i < length; i += 1)
						{
							if (commanate)
							{
								writer.Write(',');
							}
							if (indentFactor > 0)
							{
								writer.Write('\n');
							}
							org.json.JSONObject.Indent(writer, newindent);
							org.json.JSONObject.WriteValue(writer, this.myArrayList[i], indentFactor, newindent
								);
							commanate = true;
						}
						if (indentFactor > 0)
						{
							writer.Write('\n');
						}
						org.json.JSONObject.Indent(writer, indent);
					}
				}
				writer.Write(']');
				return writer;
			}
			catch (System.IO.IOException e)
			{
				throw new org.json.JSONException(e);
			}
		}
	}
}
