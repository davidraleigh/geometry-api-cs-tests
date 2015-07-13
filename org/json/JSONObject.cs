using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using Sharpen;
using java.lang;
using java.lang.reflect;
using java.util;

namespace org.json
{
	/// <summary>A JSONObject is an unordered collection of name/value pairs.</summary>
	/// <remarks>
	/// A JSONObject is an unordered collection of name/value pairs. Its external
	/// form is a string wrapped in curly braces with colons between the names and
	/// values, and commas between the values and names. The internal form is an
	/// object having <code>get</code> and <code>opt</code> methods for accessing
	/// the values by name, and <code>put</code> methods for adding or replacing
	/// values by name. The values can be any of these types: <code>Boolean</code>,
	/// <code>JSONArray</code>, <code>JSONObject</code>, <code>Number</code>,
	/// <code>String</code>, or the <code>JSONObject.NULL</code> object. A
	/// JSONObject constructor can be used to convert an external form JSON text
	/// into an internal form whose values can be retrieved with the
	/// <code>get</code> and <code>opt</code> methods, or to convert values into a
	/// JSON text using the <code>put</code> and <code>toString</code> methods. A
	/// <code>get</code> method returns a value if one can be found, and throws an
	/// exception if one cannot be found. An <code>opt</code> method returns a
	/// default value instead of throwing an exception, and so is useful for
	/// obtaining optional values.
	/// <p>
	/// The generic <code>get()</code> and <code>opt()</code> methods return an
	/// object, which you can cast or query for type. There are also typed
	/// <code>get</code> and <code>opt</code> methods that do type checking and type
	/// coercion for you. The opt methods differ from the get methods in that they
	/// do not throw. Instead, they return a specified value, such as null.
	/// <p>
	/// The <code>put</code> methods add or replace values in an object. For
	/// example,
	/// <pre>
	/// myString = new JSONObject()
	/// .put(&quot;JSON&quot;, &quot;Hello, World!&quot;).toString();
	/// </pre>
	/// produces the string <code>{"JSON": "Hello, World"}</code>.
	/// <p>
	/// The texts produced by the <code>toString</code> methods strictly conform to
	/// the JSON syntax rules. The constructors are more forgiving in the texts they
	/// will accept:
	/// <ul>
	/// <li>An extra <code>,</code>&nbsp;<small>(comma)</small> may appear just
	/// before the closing brace.</li>
	/// <li>Strings may be quoted with <code>'</code>&nbsp;<small>(single
	/// quote)</small>.</li>
	/// <li>Strings do not need to be quoted at all if they do not begin with a
	/// quote or single quote, and if they do not contain leading or trailing
	/// spaces, and if they do not contain any of these characters:
	/// <code>{ } [ ] / \ : , #</code> and if they do not look like numbers and
	/// if they are not the reserved words <code>true</code>, <code>false</code>,
	/// or <code>null</code>.</li>
	/// </ul>
	/// </remarks>
	/// <author>JSON.org</author>
	/// <version>2015-05-05</version>
	public class JSONObject
	{
		/// <summary>
		/// JSONObject.NULL is equivalent to the value that JavaScript calls null,
		/// whilst Java's null is equivalent to the value that JavaScript calls
		/// undefined.
		/// </summary>
		private sealed class Null
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
			/// There is only intended to be a single instance of the NULL object,
			/// so the clone method returns itself.
			/// </summary>
			/// <returns>NULL.</returns>
			protected internal object Clone()
			{
				return this;
			}

			/// <summary>A Null object is equal to the null value and to itself.</summary>
			/// <param name="object">An object to test for nullness.</param>
			/// <returns>
			/// true if the object parameter is the JSONObject.NULL object or
			/// null.
			/// </returns>
			public override bool Equals(object @object)
			{
				return @object == null || @object == this;
			}

			/// <summary>Get the "null" string value.</summary>
			/// <returns>The string "null".</returns>
			public override string ToString()
			{
				return "null";
			}
		}

		/// <summary>The map where the JSONObject's properties are kept.</summary>
		private readonly IDictionary<string, object> map;

		/// <summary>
		/// It is sometimes more convenient and less ambiguous to have a
		/// <code>NULL</code> object than to use Java's <code>null</code> value.
		/// </summary>
		/// <remarks>
		/// It is sometimes more convenient and less ambiguous to have a
		/// <code>NULL</code> object than to use Java's <code>null</code> value.
		/// <code>JSONObject.NULL.equals(null)</code> returns <code>true</code>.
		/// <code>JSONObject.NULL.toString()</code> returns <code>"null"</code>.
		/// </remarks>
		public static readonly object NULL = new JSONObject.Null();

		/// <summary>Construct an empty JSONObject.</summary>
		public JSONObject()
		{
			this.map = new Dictionary<string, object>();
		}

		/// <summary>Construct a JSONObject from a subset of another JSONObject.</summary>
		/// <remarks>
		/// Construct a JSONObject from a subset of another JSONObject. An array of
		/// strings is used to identify the keys that should be copied. Missing keys
		/// are ignored.
		/// </remarks>
		/// <param name="jo">A JSONObject.</param>
		/// <param name="names">An array of strings.</param>
		/// <exception cref="JSONException"/>
		/// <exception>
		/// JSONException
		/// If a value is a non-finite number or if a name is
		/// duplicated.
		/// </exception>
		public JSONObject(JSONObject jo, string[] names)
			: this()
		{
			for (int i = 0; i < names.Length; i += 1)
			{
				try
				{
					this.PutOnce(names[i], jo.Opt(names[i]));
				}
				catch (Exception)
				{
				}
			}
		}

		/// <summary>Construct a JSONObject from a JSONTokener.</summary>
		/// <param name="x">A JSONTokener object containing the source string.</param>
		/// <exception cref="JSONException">
		/// If there is a syntax error in the source string or a
		/// duplicated key.
		/// </exception>
		/// <exception cref="org.json.JSONException"/>
		public JSONObject(JSONTokener x)
			: this()
		{
			char c;
			string key;
			if (x.NextClean() != '{')
			{
				throw x.SyntaxError("A JSONObject text must begin with '{'");
			}
			for (; ; )
			{
				c = x.NextClean();
				switch (c)
				{
					case 0:
					{
						throw x.SyntaxError("A JSONObject text must end with '}'");
					}

					case '}':
					{
						return;
					}

					default:
					{
						x.Back();
						key = x.NextValue().ToString();
						break;
					}
				}
				// The key is followed by ':'.
				c = x.NextClean();
				if (c != ':')
				{
					throw x.SyntaxError("Expected a ':' after a key");
				}
				this.PutOnce(key, x.NextValue());
				switch (x.NextClean())
				{
					case ';':
					case ',':
					{
						// Pairs are separated by ','.
						if (x.NextClean() == '}')
						{
							return;
						}
						x.Back();
						break;
					}

					case '}':
					{
						return;
					}

					default:
					{
						throw x.SyntaxError("Expected a ',' or '}'");
					}
				}
			}
		}

		/// <summary>Construct a JSONObject from a Map.</summary>
		/// <param name="map">
		/// A map object that can be used to initialize the contents of
		/// the JSONObject.
		/// </param>
		/// <exception cref="JSONException"/>
		public JSONObject(IDictionary<string, object> map)
		{
			this.map = new Dictionary<string, object>();
			if (map != null)
			{
				IEnumerator<KeyValuePair<string, object>> i = map.GetEnumerator();
				while (i.HasNext())
				{
					KeyValuePair<string, object> entry = i.Next();
					object value = entry.Value;
					if (value != null)
					{
						this.map[entry.Key] = Wrap(value);
					}
				}
			}
		}

		/// <summary>Construct a JSONObject from an Object using bean getters.</summary>
		/// <remarks>
		/// Construct a JSONObject from an Object using bean getters. It reflects on
		/// all of the public methods of the object. For each of the methods with no
		/// parameters and a name starting with <code>"get"</code> or
		/// <code>"is"</code> followed by an uppercase letter, the method is invoked,
		/// and a key and the value returned from the getter method are put into the
		/// new JSONObject.
		/// The key is formed by removing the <code>"get"</code> or <code>"is"</code>
		/// prefix. If the second remaining character is not upper case, then the
		/// first character is converted to lower case.
		/// For example, if an object has a method named <code>"getName"</code>, and
		/// if the result of calling <code>object.getName()</code> is
		/// <code>"Larry Fine"</code>, then the JSONObject will contain
		/// <code>"name": "Larry Fine"</code>.
		/// </remarks>
		/// <param name="bean">
		/// An object that has getter methods that should be used to make
		/// a JSONObject.
		/// </param>
		public JSONObject(object bean)
			: this()
		{
			this.PopulateMap(bean);
		}

		/// <summary>
		/// Construct a JSONObject from an Object, using reflection to find the
		/// public members.
		/// </summary>
		/// <remarks>
		/// Construct a JSONObject from an Object, using reflection to find the
		/// public members. The resulting JSONObject's keys will be the strings from
		/// the names array, and the values will be the field values associated with
		/// those keys in the object. If a key is not found or not visible, then it
		/// will not be copied into the new JSONObject.
		/// </remarks>
		/// <param name="object">
		/// An object that has fields that should be used to make a
		/// JSONObject.
		/// </param>
		/// <param name="names">
		/// An array of strings, the names of the fields to be obtained
		/// from the object.
		/// </param>
		public JSONObject(object @object, string[] names)
			: this()
		{
			Type c = @object.GetType();
			for (int i = 0; i < names.Length; i += 1)
			{
				string name = names[i];
				try
				{
					this.PutOpt(name, c.GetField(name).GetValue(@object));
				}
				catch (Exception)
				{
				}
			}
		}

		/// <summary>Construct a JSONObject from a source JSON text string.</summary>
		/// <remarks>
		/// Construct a JSONObject from a source JSON text string. This is the most
		/// commonly used JSONObject constructor.
		/// </remarks>
		/// <param name="source">
		/// A string beginning with <code>{</code>&nbsp;<small>(left
		/// brace)</small> and ending with <code>}</code>
		/// &nbsp;<small>(right brace)</small>.
		/// </param>
		/// <exception>
		/// JSONException
		/// If there is a syntax error in the source string or a
		/// duplicated key.
		/// </exception>
		/// <exception cref="org.json.JSONException"/>
		public JSONObject(string source)
			: this(new JSONTokener(source))
		{
		}

		/// <summary>Construct a JSONObject from a ResourceBundle.</summary>
		/// <param name="baseName">The ResourceBundle base name.</param>
		/// <param name="locale">The Locale to load the ResourceBundle for.</param>
		/// <exception cref="JSONException">If any JSONExceptions are detected.</exception>
		/// <exception cref="org.json.JSONException"/>
		public JSONObject(string baseName, CultureInfo locale)
			: this()
		{
			ResourceBundle bundle = ResourceBundle.GetBundle(baseName, locale, java.lang.Thread
				.CurrentThread().GetContextClassLoader());
			// Iterate through the keys in the bundle.
			Enumeration<string> keys = bundle.GetKeys();
			while (keys.MoveNext())
			{
				object key = keys.Current;
				if (key != null)
				{
					// Go through the path, ensuring that there is a nested JSONObject for each
					// segment except the last. Add the value using the last segment's name into
					// the deepest nested JSONObject.
					string[] path = ((string)key).Split("\\.");
					int last = path.Length - 1;
					JSONObject target = this;
					for (int i = 0; i < last; i += 1)
					{
						string segment = path[i];
						JSONObject nextTarget = target.OptJSONObject(segment);
						if (nextTarget == null)
						{
							nextTarget = new JSONObject();
							target.Put(segment, nextTarget);
						}
						target = nextTarget;
					}
					target.Put(path[last], bundle.GetString((string)key));
				}
			}
		}

		/// <summary>Accumulate values under a key.</summary>
		/// <remarks>
		/// Accumulate values under a key. It is similar to the put method except
		/// that if there is already an object stored under the key then a JSONArray
		/// is stored under the key to hold all of the accumulated values. If there
		/// is already a JSONArray, then the new value is appended to it. In
		/// contrast, the put method replaces the previous value.
		/// If only one value is accumulated that is not a JSONArray, then the result
		/// will be the same as using put. But if multiple values are accumulated,
		/// then the result will be like append.
		/// </remarks>
		/// <param name="key">A key string.</param>
		/// <param name="value">An object to be accumulated under the key.</param>
		/// <returns>this.</returns>
		/// <exception cref="JSONException">If the value is an invalid number or if the key is null.
		/// 	</exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual JSONObject Accumulate(string key, object value)
		{
			TestValidity(value);
			object @object = this.Opt(key);
			if (@object == null)
			{
				this.Put(key, value is JSONArray ? new JSONArray().Put(value) : value);
			}
			else
			{
				if (@object is JSONArray)
				{
					((JSONArray)@object).Put(value);
				}
				else
				{
					this.Put(key, new JSONArray().Put(@object).Put(value));
				}
			}
			return this;
		}

		/// <summary>Append values to the array under a key.</summary>
		/// <remarks>
		/// Append values to the array under a key. If the key does not exist in the
		/// JSONObject, then the key is put in the JSONObject with its value being a
		/// JSONArray containing the value parameter. If the key was already
		/// associated with a JSONArray, then the value parameter is appended to it.
		/// </remarks>
		/// <param name="key">A key string.</param>
		/// <param name="value">An object to be accumulated under the key.</param>
		/// <returns>this.</returns>
		/// <exception cref="JSONException">
		/// If the key is null or if the current value associated with
		/// the key is not a JSONArray.
		/// </exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual JSONObject Append(string key, object value)
		{
			TestValidity(value);
			object @object = this.Opt(key);
			if (@object == null)
			{
				this.Put(key, new JSONArray().Put(value));
			}
			else
			{
				if (@object is JSONArray)
				{
					this.Put(key, ((JSONArray)@object).Put(value));
				}
				else
				{
					throw new JSONException("JSONObject[" + key + "] is not a JSONArray.");
				}
			}
			return this;
		}

		/// <summary>Produce a string from a double.</summary>
		/// <remarks>
		/// Produce a string from a double. The string "null" will be returned if the
		/// number is not finite.
		/// </remarks>
		/// <param name="d">A double.</param>
		/// <returns>A String.</returns>
		public static string DoubleToString(double d)
		{
			if (double.IsInfinite(d) || double.IsNaN(d))
			{
				return "null";
			}
			// Shave off trailing zeros and decimal point, if possible.
			string @string = d.ToString();
			if (@string.IndexOf('.') > 0 && @string.IndexOf('e') < 0 && @string.IndexOf('E') 
				< 0)
			{
				while (@string.EndsWith("0"))
				{
					@string = Sharpen.Runtime.Substring(@string, 0, @string.Length - 1);
				}
				if (@string.EndsWith("."))
				{
					@string = Sharpen.Runtime.Substring(@string, 0, @string.Length - 1);
				}
			}
			return @string;
		}

		/// <summary>Get the value object associated with a key.</summary>
		/// <param name="key">A key string.</param>
		/// <returns>The object associated with the key.</returns>
		/// <exception cref="JSONException">if the key is not found.</exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual object Get(string key)
		{
			if (key == null)
			{
				throw new JSONException("Null key.");
			}
			object @object = this.Opt(key);
			if (@object == null)
			{
				throw new JSONException("JSONObject[" + Quote(key) + "] not found.");
			}
			return @object;
		}

		/// <summary>Get the boolean value associated with a key.</summary>
		/// <param name="key">A key string.</param>
		/// <returns>The truth.</returns>
		/// <exception cref="JSONException">
		/// if the value is not a Boolean or the String "true" or
		/// "false".
		/// </exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual bool GetBoolean(string key)
		{
			object @object = this.Get(key);
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
			throw new JSONException("JSONObject[" + Quote(key) + "] is not a Boolean.");
		}

		/// <summary>Get the double value associated with a key.</summary>
		/// <param name="key">A key string.</param>
		/// <returns>The numeric value.</returns>
		/// <exception cref="JSONException">
		/// if the key is not found or if the value is not a Number
		/// object and cannot be converted to a number.
		/// </exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual double GetDouble(string key)
		{
			object @object = this.Get(key);
			try
			{
				return @object is Number ? ((Number)@object) : double.Parse((string)@object);
			}
			catch (Exception)
			{
				throw new JSONException("JSONObject[" + Quote(key) + "] is not a number.");
			}
		}

		/// <summary>Get the int value associated with a key.</summary>
		/// <param name="key">A key string.</param>
		/// <returns>The integer value.</returns>
		/// <exception cref="JSONException">
		/// if the key is not found or if the value cannot be converted
		/// to an integer.
		/// </exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual int GetInt(string key)
		{
			object @object = this.Get(key);
			try
			{
				return @object is Number ? ((Number)@object) : System.Convert.ToInt32((string)@object
					);
			}
			catch (Exception)
			{
				throw new JSONException("JSONObject[" + Quote(key) + "] is not an int.");
			}
		}

		/// <summary>Get the JSONArray value associated with a key.</summary>
		/// <param name="key">A key string.</param>
		/// <returns>A JSONArray which is the value.</returns>
		/// <exception cref="JSONException">if the key is not found or if the value is not a JSONArray.
		/// 	</exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual JSONArray GetJSONArray(string key)
		{
			object @object = this.Get(key);
			if (@object is JSONArray)
			{
				return (JSONArray)@object;
			}
			throw new JSONException("JSONObject[" + Quote(key) + "] is not a JSONArray.");
		}

		/// <summary>Get the JSONObject value associated with a key.</summary>
		/// <param name="key">A key string.</param>
		/// <returns>A JSONObject which is the value.</returns>
		/// <exception cref="JSONException">if the key is not found or if the value is not a JSONObject.
		/// 	</exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual JSONObject GetJSONObject(string key)
		{
			object @object = this.Get(key);
			if (@object is JSONObject)
			{
				return (JSONObject)@object;
			}
			throw new JSONException("JSONObject[" + Quote(key) + "] is not a JSONObject.");
		}

		/// <summary>Get the long value associated with a key.</summary>
		/// <param name="key">A key string.</param>
		/// <returns>The long value.</returns>
		/// <exception cref="JSONException">
		/// if the key is not found or if the value cannot be converted
		/// to a long.
		/// </exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual long GetLong(string key)
		{
			object @object = this.Get(key);
			try
			{
				return @object is Number ? ((Number)@object) : System.Convert.ToInt64((string)@object
					);
			}
			catch (Exception)
			{
				throw new JSONException("JSONObject[" + Quote(key) + "] is not a long.");
			}
		}

		/// <summary>Get an array of field names from a JSONObject.</summary>
		/// <returns>An array of field names, or null if there are no names.</returns>
		public static string[] GetNames(JSONObject jo)
		{
			int length = jo.Length();
			if (length == 0)
			{
				return null;
			}
			IEnumerator<string> iterator = jo.Keys();
			string[] names = new string[length];
			int i = 0;
			while (iterator.HasNext())
			{
				names[i] = iterator.Next();
				i += 1;
			}
			return names;
		}

		/// <summary>Get an array of field names from an Object.</summary>
		/// <returns>An array of field names, or null if there are no names.</returns>
		public static string[] GetNames(object @object)
		{
			if (@object == null)
			{
				return null;
			}
			Type klass = @object.GetType();
			FieldInfo[] fields = klass.GetFields();
			int length = fields.Length;
			if (length == 0)
			{
				return null;
			}
			string[] names = new string[length];
			for (int i = 0; i < length; i += 1)
			{
				names[i] = fields[i].Name;
			}
			return names;
		}

		/// <summary>Get the string associated with a key.</summary>
		/// <param name="key">A key string.</param>
		/// <returns>A string which is the value.</returns>
		/// <exception cref="JSONException">if there is no string value for the key.</exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual string GetString(string key)
		{
			object @object = this.Get(key);
			if (@object is string)
			{
				return (string)@object;
			}
			throw new JSONException("JSONObject[" + Quote(key) + "] not a string.");
		}

		/// <summary>Determine if the JSONObject contains a specific key.</summary>
		/// <param name="key">A key string.</param>
		/// <returns>true if the key exists in the JSONObject.</returns>
		public virtual bool Has(string key)
		{
			return this.map.Contains(key);
		}

		/// <summary>Increment a property of a JSONObject.</summary>
		/// <remarks>
		/// Increment a property of a JSONObject. If there is no such property,
		/// create one with a value of 1. If there is such a property, and if it is
		/// an Integer, Long, Double, or Float, then add one to it.
		/// </remarks>
		/// <param name="key">A key string.</param>
		/// <returns>this.</returns>
		/// <exception cref="JSONException">
		/// If there is already a property with this name that is not an
		/// Integer, Long, Double, or Float.
		/// </exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual JSONObject Increment(string key)
		{
			object value = this.Opt(key);
			if (value == null)
			{
				this.Put(key, 1);
			}
			else
			{
				if (value is int)
				{
					this.Put(key, (int)value + 1);
				}
				else
				{
					if (value is long)
					{
						this.Put(key, (long)value + 1);
					}
					else
					{
						if (value is double)
						{
							this.Put(key, (double)value + 1);
						}
						else
						{
							if (value is float)
							{
								this.Put(key, (float)value + 1);
							}
							else
							{
								throw new JSONException("Unable to increment [" + Quote(key) + "].");
							}
						}
					}
				}
			}
			return this;
		}

		/// <summary>
		/// Determine if the value associated with the key is null or if there is no
		/// value.
		/// </summary>
		/// <param name="key">A key string.</param>
		/// <returns>
		/// true if there is no value associated with the key or if the value
		/// is the JSONObject.NULL object.
		/// </returns>
		public virtual bool IsNull(string key)
		{
			return JSONObject.NULL.Equals(this.Opt(key));
		}

		/// <summary>Get an enumeration of the keys of the JSONObject.</summary>
		/// <returns>An iterator of the keys.</returns>
		public virtual IEnumerator<string> Keys()
		{
			return this.KeySet().GetEnumerator();
		}

		/// <summary>Get a set of keys of the JSONObject.</summary>
		/// <returns>A keySet.</returns>
		public virtual ICollection<string> KeySet()
		{
			return this.map.Keys;
		}

		/// <summary>Get the number of keys stored in the JSONObject.</summary>
		/// <returns>The number of keys in the JSONObject.</returns>
		public virtual int Length()
		{
			return this.map.Count;
		}

		/// <summary>
		/// Produce a JSONArray containing the names of the elements of this
		/// JSONObject.
		/// </summary>
		/// <returns>
		/// A JSONArray containing the key strings, or null if the JSONObject
		/// is empty.
		/// </returns>
		public virtual JSONArray Names()
		{
			JSONArray ja = new JSONArray();
			IEnumerator<string> keys = this.Keys();
			while (keys.HasNext())
			{
				ja.Put(keys.Next());
			}
			return ja.Length() == 0 ? null : ja;
		}

		/// <summary>Produce a string from a Number.</summary>
		/// <param name="number">A Number</param>
		/// <returns>A String.</returns>
		/// <exception cref="JSONException">If n is a non-finite number.</exception>
		/// <exception cref="org.json.JSONException"/>
		public static string NumberToString(Number number)
		{
			if (number == null)
			{
				throw new JSONException("Null pointer");
			}
			TestValidity(number);
			// Shave off trailing zeros and decimal point, if possible.
			string @string = number.ToString();
			if (@string.IndexOf('.') > 0 && @string.IndexOf('e') < 0 && @string.IndexOf('E') 
				< 0)
			{
				while (@string.EndsWith("0"))
				{
					@string = Sharpen.Runtime.Substring(@string, 0, @string.Length - 1);
				}
				if (@string.EndsWith("."))
				{
					@string = Sharpen.Runtime.Substring(@string, 0, @string.Length - 1);
				}
			}
			return @string;
		}

		/// <summary>Get an optional value associated with a key.</summary>
		/// <param name="key">A key string.</param>
		/// <returns>An object which is the value, or null if there is no value.</returns>
		public virtual object Opt(string key)
		{
			return key == null ? null : this.map[key];
		}

		/// <summary>Get an optional boolean associated with a key.</summary>
		/// <remarks>
		/// Get an optional boolean associated with a key. It returns false if there
		/// is no such key, or if the value is not Boolean.TRUE or the String "true".
		/// </remarks>
		/// <param name="key">A key string.</param>
		/// <returns>The truth.</returns>
		public virtual bool OptBoolean(string key)
		{
			return this.OptBoolean(key, false);
		}

		/// <summary>Get an optional boolean associated with a key.</summary>
		/// <remarks>
		/// Get an optional boolean associated with a key. It returns the
		/// defaultValue if there is no such key, or if it is not a Boolean or the
		/// String "true" or "false" (case insensitive).
		/// </remarks>
		/// <param name="key">A key string.</param>
		/// <param name="defaultValue">The default.</param>
		/// <returns>The truth.</returns>
		public virtual bool OptBoolean(string key, bool defaultValue)
		{
			try
			{
				return this.GetBoolean(key);
			}
			catch (Exception)
			{
				return defaultValue;
			}
		}

		/// <summary>
		/// Get an optional double associated with a key, or NaN if there is no such
		/// key or if its value is not a number.
		/// </summary>
		/// <remarks>
		/// Get an optional double associated with a key, or NaN if there is no such
		/// key or if its value is not a number. If the value is a string, an attempt
		/// will be made to evaluate it as a number.
		/// </remarks>
		/// <param name="key">A string which is the key.</param>
		/// <returns>An object which is the value.</returns>
		public virtual double OptDouble(string key)
		{
			return this.OptDouble(key, double.NaN);
		}

		/// <summary>
		/// Get an optional double associated with a key, or the defaultValue if
		/// there is no such key or if its value is not a number.
		/// </summary>
		/// <remarks>
		/// Get an optional double associated with a key, or the defaultValue if
		/// there is no such key or if its value is not a number. If the value is a
		/// string, an attempt will be made to evaluate it as a number.
		/// </remarks>
		/// <param name="key">A key string.</param>
		/// <param name="defaultValue">The default.</param>
		/// <returns>An object which is the value.</returns>
		public virtual double OptDouble(string key, double defaultValue)
		{
			try
			{
				return this.GetDouble(key);
			}
			catch (Exception)
			{
				return defaultValue;
			}
		}

		/// <summary>
		/// Get an optional int value associated with a key, or zero if there is no
		/// such key or if the value is not a number.
		/// </summary>
		/// <remarks>
		/// Get an optional int value associated with a key, or zero if there is no
		/// such key or if the value is not a number. If the value is a string, an
		/// attempt will be made to evaluate it as a number.
		/// </remarks>
		/// <param name="key">A key string.</param>
		/// <returns>An object which is the value.</returns>
		public virtual int OptInt(string key)
		{
			return this.OptInt(key, 0);
		}

		/// <summary>
		/// Get an optional int value associated with a key, or the default if there
		/// is no such key or if the value is not a number.
		/// </summary>
		/// <remarks>
		/// Get an optional int value associated with a key, or the default if there
		/// is no such key or if the value is not a number. If the value is a string,
		/// an attempt will be made to evaluate it as a number.
		/// </remarks>
		/// <param name="key">A key string.</param>
		/// <param name="defaultValue">The default.</param>
		/// <returns>An object which is the value.</returns>
		public virtual int OptInt(string key, int defaultValue)
		{
			try
			{
				return this.GetInt(key);
			}
			catch (Exception)
			{
				return defaultValue;
			}
		}

		/// <summary>Get an optional JSONArray associated with a key.</summary>
		/// <remarks>
		/// Get an optional JSONArray associated with a key. It returns null if there
		/// is no such key, or if its value is not a JSONArray.
		/// </remarks>
		/// <param name="key">A key string.</param>
		/// <returns>A JSONArray which is the value.</returns>
		public virtual JSONArray OptJSONArray(string key)
		{
			object o = this.Opt(key);
			return o is JSONArray ? (JSONArray)o : null;
		}

		/// <summary>Get an optional JSONObject associated with a key.</summary>
		/// <remarks>
		/// Get an optional JSONObject associated with a key. It returns null if
		/// there is no such key, or if its value is not a JSONObject.
		/// </remarks>
		/// <param name="key">A key string.</param>
		/// <returns>A JSONObject which is the value.</returns>
		public virtual JSONObject OptJSONObject(string key)
		{
			object @object = this.Opt(key);
			return @object is JSONObject ? (JSONObject)@object : null;
		}

		/// <summary>
		/// Get an optional long value associated with a key, or zero if there is no
		/// such key or if the value is not a number.
		/// </summary>
		/// <remarks>
		/// Get an optional long value associated with a key, or zero if there is no
		/// such key or if the value is not a number. If the value is a string, an
		/// attempt will be made to evaluate it as a number.
		/// </remarks>
		/// <param name="key">A key string.</param>
		/// <returns>An object which is the value.</returns>
		public virtual long OptLong(string key)
		{
			return this.OptLong(key, 0);
		}

		/// <summary>
		/// Get an optional long value associated with a key, or the default if there
		/// is no such key or if the value is not a number.
		/// </summary>
		/// <remarks>
		/// Get an optional long value associated with a key, or the default if there
		/// is no such key or if the value is not a number. If the value is a string,
		/// an attempt will be made to evaluate it as a number.
		/// </remarks>
		/// <param name="key">A key string.</param>
		/// <param name="defaultValue">The default.</param>
		/// <returns>An object which is the value.</returns>
		public virtual long OptLong(string key, long defaultValue)
		{
			try
			{
				return this.GetLong(key);
			}
			catch (Exception)
			{
				return defaultValue;
			}
		}

		/// <summary>Get an optional string associated with a key.</summary>
		/// <remarks>
		/// Get an optional string associated with a key. It returns an empty string
		/// if there is no such key. If the value is not a string and is not null,
		/// then it is converted to a string.
		/// </remarks>
		/// <param name="key">A key string.</param>
		/// <returns>A string which is the value.</returns>
		public virtual string OptString(string key)
		{
			return this.OptString(key, string.Empty);
		}

		/// <summary>Get an optional string associated with a key.</summary>
		/// <remarks>
		/// Get an optional string associated with a key. It returns the defaultValue
		/// if there is no such key.
		/// </remarks>
		/// <param name="key">A key string.</param>
		/// <param name="defaultValue">The default.</param>
		/// <returns>A string which is the value.</returns>
		public virtual string OptString(string key, string defaultValue)
		{
			object @object = this.Opt(key);
			return NULL.Equals(@object) ? defaultValue : @object.ToString();
		}

		private void PopulateMap(object bean)
		{
			Type klass = bean.GetType();
			// If klass is a System class then set includeSuperClass to false.
			bool includeSuperClass = klass.GetClassLoader() != null;
			MethodInfo[] methods = includeSuperClass ? klass.GetMethods() : Sharpen.Runtime.GetDeclaredMethods
				(klass);
			for (int i = 0; i < methods.Length; i += 1)
			{
				try
				{
					MethodInfo method = methods[i];
					if (Modifier.IsPublic(method.GetModifiers()))
					{
						string name = method.Name;
						string key = string.Empty;
						if (name.StartsWith("get"))
						{
							if ("getClass".Equals(name) || "getDeclaringClass".Equals(name))
							{
								key = string.Empty;
							}
							else
							{
								key = Sharpen.Runtime.Substring(name, 3);
							}
						}
						else
						{
							if (name.StartsWith("is"))
							{
								key = Sharpen.Runtime.Substring(name, 2);
							}
						}
						if (key.Length > 0 && System.Char.IsUpper(key[0]) && Sharpen.Runtime.GetParameterTypes
							(method).Length == 0)
						{
							if (key.Length == 1)
							{
								key = key.ToLower();
							}
							else
							{
								if (!System.Char.IsUpper(key[1]))
								{
									key = Sharpen.Runtime.Substring(key, 0, 1).ToLower() + Sharpen.Runtime.Substring(
										key, 1);
								}
							}
							object result = method.Invoke(bean, (object[])null);
							if (result != null)
							{
								this.map[key] = Wrap(result);
							}
						}
					}
				}
				catch (Exception)
				{
				}
			}
		}

		/// <summary>Put a key/boolean pair in the JSONObject.</summary>
		/// <param name="key">A key string.</param>
		/// <param name="value">A boolean which is the value.</param>
		/// <returns>this.</returns>
		/// <exception cref="JSONException">If the key is null.</exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual JSONObject Put(string key, bool value)
		{
			this.Put(key, value ? true : false);
			return this;
		}

		/// <summary>
		/// Put a key/value pair in the JSONObject, where the value will be a
		/// JSONArray which is produced from a Collection.
		/// </summary>
		/// <param name="key">A key string.</param>
		/// <param name="value">A Collection value.</param>
		/// <returns>this.</returns>
		/// <exception cref="JSONException"/>
		/// <exception cref="org.json.JSONException"/>
		public virtual JSONObject Put(string key, ICollection<object> value)
		{
			this.Put(key, new JSONArray(value));
			return this;
		}

		/// <summary>Put a key/double pair in the JSONObject.</summary>
		/// <param name="key">A key string.</param>
		/// <param name="value">A double which is the value.</param>
		/// <returns>this.</returns>
		/// <exception cref="JSONException">If the key is null or if the number is invalid.</exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual JSONObject Put(string key, double value)
		{
			this.Put(key, value);
			return this;
		}

		/// <summary>Put a key/int pair in the JSONObject.</summary>
		/// <param name="key">A key string.</param>
		/// <param name="value">An int which is the value.</param>
		/// <returns>this.</returns>
		/// <exception cref="JSONException">If the key is null.</exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual JSONObject Put(string key, int value)
		{
			this.Put(key, value);
			return this;
		}

		/// <summary>Put a key/long pair in the JSONObject.</summary>
		/// <param name="key">A key string.</param>
		/// <param name="value">A long which is the value.</param>
		/// <returns>this.</returns>
		/// <exception cref="JSONException">If the key is null.</exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual JSONObject Put(string key, long value)
		{
			this.Put(key, value);
			return this;
		}

		/// <summary>
		/// Put a key/value pair in the JSONObject, where the value will be a
		/// JSONObject which is produced from a Map.
		/// </summary>
		/// <param name="key">A key string.</param>
		/// <param name="value">A Map value.</param>
		/// <returns>this.</returns>
		/// <exception cref="JSONException"/>
		/// <exception cref="org.json.JSONException"/>
		public virtual JSONObject Put(string key, IDictionary<string, object> value)
		{
			this.Put(key, new JSONObject(value));
			return this;
		}

		/// <summary>Put a key/value pair in the JSONObject.</summary>
		/// <remarks>
		/// Put a key/value pair in the JSONObject. If the value is null, then the
		/// key will be removed from the JSONObject if it is present.
		/// </remarks>
		/// <param name="key">A key string.</param>
		/// <param name="value">
		/// An object which is the value. It should be of one of these
		/// types: Boolean, Double, Integer, JSONArray, JSONObject, Long,
		/// String, or the JSONObject.NULL object.
		/// </param>
		/// <returns>this.</returns>
		/// <exception cref="JSONException">If the value is non-finite number or if the key is null.
		/// 	</exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual JSONObject Put(string key, object value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("Null key.");
			}
			if (value != null)
			{
				TestValidity(value);
				this.map[key] = value;
			}
			else
			{
				this.Remove(key);
			}
			return this;
		}

		/// <summary>
		/// Put a key/value pair in the JSONObject, but only if the key and the value
		/// are both non-null, and only if there is not already a member with that
		/// name.
		/// </summary>
		/// <param name="key">string</param>
		/// <param name="value">object</param>
		/// <returns>this.</returns>
		/// <exception cref="JSONException">if the key is a duplicate</exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual JSONObject PutOnce(string key, object value)
		{
			if (key != null && value != null)
			{
				if (this.Opt(key) != null)
				{
					throw new JSONException("Duplicate key \"" + key + "\"");
				}
				this.Put(key, value);
			}
			return this;
		}

		/// <summary>
		/// Put a key/value pair in the JSONObject, but only if the key and the value
		/// are both non-null.
		/// </summary>
		/// <param name="key">A key string.</param>
		/// <param name="value">
		/// An object which is the value. It should be of one of these
		/// types: Boolean, Double, Integer, JSONArray, JSONObject, Long,
		/// String, or the JSONObject.NULL object.
		/// </param>
		/// <returns>this.</returns>
		/// <exception cref="JSONException">If the value is a non-finite number.</exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual JSONObject PutOpt(string key, object value)
		{
			if (key != null && value != null)
			{
				this.Put(key, value);
			}
			return this;
		}

		/// <summary>
		/// Produce a string in double quotes with backslash sequences in all the
		/// right places.
		/// </summary>
		/// <remarks>
		/// Produce a string in double quotes with backslash sequences in all the
		/// right places. A backslash will be inserted within &lt;/, producing &lt;\/,
		/// allowing JSON text to be delivered in HTML. In JSON text, a string cannot
		/// contain a control character or an unescaped quote or backslash.
		/// </remarks>
		/// <param name="string">A String</param>
		/// <returns>A String correctly formatted for insertion in a JSON text.</returns>
		public static string Quote(string @string)
		{
			StringWriter sw = new StringWriter();
			lock (sw.GetBuffer())
			{
				try
				{
					return Quote(@string, sw).ToString();
				}
				catch (IOException)
				{
					// will never happen - we are writing to a string writer
					return string.Empty;
				}
			}
		}

		/// <exception cref="System.IO.IOException"/>
		public static TextWriter Quote(string @string, TextWriter w)
		{
			if (@string == null || @string.Length == 0)
			{
				w.Write("\"\"");
				return w;
			}
			char b;
			char c = 0;
			string hhhh;
			int i;
			int len = @string.Length;
			w.Write('"');
			for (i = 0; i < len; i += 1)
			{
				b = c;
				c = @string[i];
				switch (c)
				{
					case '\\':
					case '"':
					{
						w.Write('\\');
						w.Write(c);
						break;
					}

					case '/':
					{
						if (b == '<')
						{
							w.Write('\\');
						}
						w.Write(c);
						break;
					}

					case '\b':
					{
						w.Write("\\b");
						break;
					}

					case '\t':
					{
						w.Write("\\t");
						break;
					}

					case '\n':
					{
						w.Write("\\n");
						break;
					}

					case '\f':
					{
						w.Write("\\f");
						break;
					}

					case '\r':
					{
						w.Write("\\r");
						break;
					}

					default:
					{
						if (c < ' ' || (c >= '\u0080' && c < '\u00a0') || (c >= '\u2000' && c < '\u2100'))
						{
							w.Write("\\u");
							hhhh = Sharpen.Extensions.ToHexString(c);
							w.Write("0000", 0, 4 - hhhh.Length);
							w.Write(hhhh);
						}
						else
						{
							w.Write(c);
						}
						break;
					}
				}
			}
			w.Write('"');
			return w;
		}

		/// <summary>Remove a name and its value, if present.</summary>
		/// <param name="key">The name to be removed.</param>
		/// <returns>
		/// The value that was associated with the name, or null if there was
		/// no value.
		/// </returns>
		public virtual object Remove(string key)
		{
			return Sharpen.Collections.Remove(this.map, key);
		}

		/// <summary>Determine if two JSONObjects are similar.</summary>
		/// <remarks>
		/// Determine if two JSONObjects are similar.
		/// They must contain the same set of names which must be associated with
		/// similar values.
		/// </remarks>
		/// <param name="other">The other JSONObject</param>
		/// <returns>true if they are equal</returns>
		public virtual bool Similar(object other)
		{
			try
			{
				if (!(other is JSONObject))
				{
					return false;
				}
				ICollection<string> set = this.KeySet();
				if (!set.Equals(((JSONObject)other).KeySet()))
				{
					return false;
				}
				IEnumerator<string> iterator = set.GetEnumerator();
				while (iterator.HasNext())
				{
					string name = iterator.Next();
					object valueThis = this.Get(name);
					object valueOther = ((JSONObject)other).Get(name);
					if (valueThis is JSONObject)
					{
						if (!((JSONObject)valueThis).Similar(valueOther))
						{
							return false;
						}
					}
					else
					{
						if (valueThis is JSONArray)
						{
							if (!((JSONArray)valueThis).Similar(valueOther))
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
			catch
			{
				return false;
			}
		}

		/// <summary>Try to convert a string into a number, boolean, or null.</summary>
		/// <remarks>
		/// Try to convert a string into a number, boolean, or null. If the string
		/// can't be converted, return the string.
		/// </remarks>
		/// <param name="string">A String.</param>
		/// <returns>A simple JSON value.</returns>
		public static object StringToValue(string @string)
		{
			double d;
			if (@string.Equals(string.Empty))
			{
				return @string;
			}
			if (Sharpen.Runtime.EqualsIgnoreCase(@string, "true"))
			{
				return true;
			}
			if (Sharpen.Runtime.EqualsIgnoreCase(@string, "false"))
			{
				return false;
			}
			if (Sharpen.Runtime.EqualsIgnoreCase(@string, "null"))
			{
				return JSONObject.NULL;
			}
			/*
			* If it might be a number, try converting it. If a number cannot be
			* produced, then the value will just be a string.
			*/
			char b = @string[0];
			if ((b >= '0' && b <= '9') || b == '-')
			{
				try
				{
					if (@string.IndexOf('.') > -1 || @string.IndexOf('e') > -1 || @string.IndexOf('E'
						) > -1)
					{
						d = double.Parse(@string);
						if (!d.IsInfinite() && !double.IsNaN(d))
						{
							return d;
						}
					}
					else
					{
						long myLong = System.Convert.ToInt64(@string);
						if (@string.Equals(myLong.ToString()))
						{
							if (myLong == myLong)
							{
								return myLong;
							}
							else
							{
								return myLong;
							}
						}
					}
				}
				catch (Exception)
				{
				}
			}
			return @string;
		}

		/// <summary>Throw an exception if the object is a NaN or infinite number.</summary>
		/// <param name="o">The object to test.</param>
		/// <exception cref="JSONException">If o is a non-finite number.</exception>
		/// <exception cref="org.json.JSONException"/>
		public static void TestValidity(object o)
		{
			if (o != null)
			{
				if (o is double)
				{
					if (((double)o).IsInfinite() || double.IsNaN(((double)o)))
					{
						throw new JSONException("JSON does not allow non-finite numbers.");
					}
				}
				else
				{
					if (o is float)
					{
						if (((float)o).IsInfinite() || float.IsNaN(((float)o)))
						{
							throw new JSONException("JSON does not allow non-finite numbers.");
						}
					}
				}
			}
		}

		/// <summary>
		/// Produce a JSONArray containing the values of the members of this
		/// JSONObject.
		/// </summary>
		/// <param name="names">
		/// A JSONArray containing a list of key strings. This determines
		/// the sequence of the values in the result.
		/// </param>
		/// <returns>A JSONArray of values.</returns>
		/// <exception cref="JSONException">If any of the values are non-finite numbers.</exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual JSONArray ToJSONArray(JSONArray names)
		{
			if (names == null || names.Length() == 0)
			{
				return null;
			}
			JSONArray ja = new JSONArray();
			for (int i = 0; i < names.Length(); i += 1)
			{
				ja.Put(this.Opt(names.GetString(i)));
			}
			return ja;
		}

		/// <summary>Make a JSON text of this JSONObject.</summary>
		/// <remarks>
		/// Make a JSON text of this JSONObject. For compactness, no whitespace is
		/// added. If this would not result in a syntactically correct JSON text,
		/// then null will be returned instead.
		/// <p>
		/// Warning: This method assumes that the data structure is acyclical.
		/// </remarks>
		/// <returns>
		/// a printable, displayable, portable, transmittable representation
		/// of the object, beginning with <code>{</code>&nbsp;<small>(left
		/// brace)</small> and ending with <code>}</code>&nbsp;<small>(right
		/// brace)</small>.
		/// </returns>
		public override string ToString()
		{
			try
			{
				return this.ToString(0);
			}
			catch (Exception)
			{
				return null;
			}
		}

		/// <summary>Make a prettyprinted JSON text of this JSONObject.</summary>
		/// <remarks>
		/// Make a prettyprinted JSON text of this JSONObject.
		/// <p>
		/// Warning: This method assumes that the data structure is acyclical.
		/// </remarks>
		/// <param name="indentFactor">The number of spaces to add to each level of indentation.
		/// 	</param>
		/// <returns>
		/// a printable, displayable, portable, transmittable representation
		/// of the object, beginning with <code>{</code>&nbsp;<small>(left
		/// brace)</small> and ending with <code>}</code>&nbsp;<small>(right
		/// brace)</small>.
		/// </returns>
		/// <exception cref="JSONException">If the object contains an invalid number.</exception>
		/// <exception cref="org.json.JSONException"/>
		public virtual string ToString(int indentFactor)
		{
			StringWriter w = new StringWriter();
			lock (w.GetBuffer())
			{
				return this.Write(w, indentFactor, 0).ToString();
			}
		}

		/// <summary>Make a JSON text of an Object value.</summary>
		/// <remarks>
		/// Make a JSON text of an Object value. If the object has an
		/// value.toJSONString() method, then that method will be used to produce the
		/// JSON text. The method is required to produce a strictly conforming text.
		/// If the object does not contain a toJSONString method (which is the most
		/// common case), then a text will be produced by other means. If the value
		/// is an array or Collection, then a JSONArray will be made from it and its
		/// toJSONString method will be called. If the value is a MAP, then a
		/// JSONObject will be made from it and its toJSONString method will be
		/// called. Otherwise, the value's toString method will be called, and the
		/// result will be quoted.
		/// <p>
		/// Warning: This method assumes that the data structure is acyclical.
		/// </remarks>
		/// <param name="value">The value to be serialized.</param>
		/// <returns>
		/// a printable, displayable, transmittable representation of the
		/// object, beginning with <code>{</code>&nbsp;<small>(left
		/// brace)</small> and ending with <code>}</code>&nbsp;<small>(right
		/// brace)</small>.
		/// </returns>
		/// <exception cref="JSONException">If the value is or contains an invalid number.</exception>
		/// <exception cref="org.json.JSONException"/>
		public static string ValueToString(object value)
		{
			if (value == null || value.Equals(null))
			{
				return "null";
			}
			if (value is JSONString)
			{
				object @object;
				try
				{
					@object = ((JSONString)value).ToJSONString();
				}
				catch (Exception e)
				{
					throw new JSONException(e);
				}
				if (@object is string)
				{
					return (string)@object;
				}
				throw new JSONException("Bad value from toJSONString: " + @object);
			}
			if (value is Number)
			{
				return NumberToString((Number)value);
			}
			if (value is bool || value is JSONObject || value is JSONArray)
			{
				return value.ToString();
			}
			if (value is IDictionary)
			{
				IDictionary<string, object> map = (IDictionary<string, object>)value;
				return new JSONObject(map).ToString();
			}
			if (value is ICollection)
			{
				ICollection<object> coll = (ICollection<object>)value;
				return new JSONArray(coll).ToString();
			}
			if (value.GetType().IsArray)
			{
				return new JSONArray(value).ToString();
			}
			return Quote(value.ToString());
		}

		/// <summary>Wrap an object, if necessary.</summary>
		/// <remarks>
		/// Wrap an object, if necessary. If the object is null, return the NULL
		/// object. If it is an array or collection, wrap it in a JSONArray. If it is
		/// a map, wrap it in a JSONObject. If it is a standard property (Double,
		/// String, et al) then it is already wrapped. Otherwise, if it comes from
		/// one of the java packages, turn it into a string. And if it doesn't, try
		/// to wrap it in a JSONObject. If the wrapping fails, then null is returned.
		/// </remarks>
		/// <param name="object">The object to wrap</param>
		/// <returns>The wrapped value</returns>
		public static object Wrap(object @object)
		{
			try
			{
				if (@object == null)
				{
					return NULL;
				}
				if (@object is JSONObject || @object is JSONArray || NULL.Equals(@object) || @object
					 is JSONString || @object is byte || @object is char || @object is short || @object
					 is int || @object is long || @object is bool || @object is float || @object is 
					double || @object is string)
				{
					return @object;
				}
				if (@object is ICollection)
				{
					ICollection<object> coll = (ICollection<object>)@object;
					return new JSONArray(coll);
				}
				if (@object.GetType().IsArray)
				{
					return new JSONArray(@object);
				}
				if (@object is IDictionary)
				{
					IDictionary<string, object> map = (IDictionary<string, object>)@object;
					return new JSONObject(map);
				}
				Assembly objectPackage = @object.GetType().Assembly;
				string objectPackageName = objectPackage != null ? objectPackage.GetName() : string.Empty;
				if (objectPackageName.StartsWith("java.") || objectPackageName.StartsWith("javax."
					) || @object.GetType().GetClassLoader() == null)
				{
					return @object.ToString();
				}
				return new JSONObject(@object);
			}
			catch (Exception)
			{
				return null;
			}
		}

		/// <summary>Write the contents of the JSONObject as JSON text to a writer.</summary>
		/// <remarks>
		/// Write the contents of the JSONObject as JSON text to a writer. For
		/// compactness, no whitespace is added.
		/// <p>
		/// Warning: This method assumes that the data structure is acyclical.
		/// </remarks>
		/// <returns>The writer.</returns>
		/// <exception cref="JSONException"/>
		/// <exception cref="org.json.JSONException"/>
		public virtual TextWriter Write(TextWriter writer)
		{
			return this.Write(writer, 0, 0);
		}

		/// <exception cref="org.json.JSONException"/>
		/// <exception cref="System.IO.IOException"/>
		internal static TextWriter WriteValue(TextWriter writer, object value, int indentFactor
			, int indent)
		{
			if (value == null || value.Equals(null))
			{
				writer.Write("null");
			}
			else
			{
				if (value is JSONObject)
				{
					((JSONObject)value).Write(writer, indentFactor, indent);
				}
				else
				{
					if (value is JSONArray)
					{
						((JSONArray)value).Write(writer, indentFactor, indent);
					}
					else
					{
						if (value is IDictionary)
						{
							IDictionary<string, object> map = (IDictionary<string, object>)value;
							new JSONObject(map).Write(writer, indentFactor, indent);
						}
						else
						{
							if (value is ICollection)
							{
								ICollection<object> coll = (ICollection<object>)value;
								new JSONArray(coll).Write(writer, indentFactor, indent);
							}
							else
							{
								if (value.GetType().IsArray)
								{
									new JSONArray(value).Write(writer, indentFactor, indent);
								}
								else
								{
									if (value is Number)
									{
										writer.Write(NumberToString((Number)value));
									}
									else
									{
										if (value is bool)
										{
											writer.Write(value.ToString());
										}
										else
										{
											if (value is JSONString)
											{
												object o;
												try
												{
													o = ((JSONString)value).ToJSONString();
												}
												catch (Exception e)
												{
													throw new JSONException(e);
												}
												writer.Write(o != null ? o.ToString() : Quote(value.ToString()));
											}
											else
											{
												Quote(value.ToString(), writer);
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return writer;
		}

		/// <exception cref="System.IO.IOException"/>
		internal static void Indent(TextWriter writer, int indent)
		{
			for (int i = 0; i < indent; i += 1)
			{
				writer.Write(' ');
			}
		}

		/// <summary>Write the contents of the JSONObject as JSON text to a writer.</summary>
		/// <remarks>
		/// Write the contents of the JSONObject as JSON text to a writer. For
		/// compactness, no whitespace is added.
		/// <p>
		/// Warning: This method assumes that the data structure is acyclical.
		/// </remarks>
		/// <returns>The writer.</returns>
		/// <exception cref="JSONException"/>
		/// <exception cref="org.json.JSONException"/>
		internal virtual TextWriter Write(TextWriter writer, int indentFactor, int indent
			)
		{
			try
			{
				bool commanate = false;
				int length = this.Length();
				IEnumerator<string> keys = this.Keys();
				writer.Write('{');
				if (length == 1)
				{
					object key = keys.Next();
					writer.Write(Quote(key.ToString()));
					writer.Write(':');
					if (indentFactor > 0)
					{
						writer.Write(' ');
					}
					WriteValue(writer, this.map[key], indentFactor, indent);
				}
				else
				{
					if (length != 0)
					{
						int newindent = indent + indentFactor;
						while (keys.HasNext())
						{
							object key = keys.Next();
							if (commanate)
							{
								writer.Write(',');
							}
							if (indentFactor > 0)
							{
								writer.Write('\n');
							}
							Indent(writer, newindent);
							writer.Write(Quote(key.ToString()));
							writer.Write(':');
							if (indentFactor > 0)
							{
								writer.Write(' ');
							}
							WriteValue(writer, this.map[key], indentFactor, newindent);
							commanate = true;
						}
						if (indentFactor > 0)
						{
							writer.Write('\n');
						}
						Indent(writer, indent);
					}
				}
				writer.Write('}');
				return writer;
			}
			catch (IOException exception)
			{
				throw new JSONException(exception);
			}
		}
	}
}
