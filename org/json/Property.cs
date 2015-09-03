using Sharpen;

namespace org.json
{
	/// <summary>Converts a Property file data into JSONObject and back.</summary>
	/// <author>JSON.org</author>
	/// <version>2015-05-05</version>
	public class Property
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
		/// <summary>Converts a property file object into a JSONObject.</summary>
		/// <remarks>Converts a property file object into a JSONObject. The property file object is a table of name value pairs.</remarks>
		/// <param name="properties">java.util.Properties</param>
		/// <returns>JSONObject</returns>
		/// <exception cref="JSONException"/>
		/// <exception cref="org.json.JSONException"/>
		public static org.json.JSONObject ToJSONObject(java.util.Properties properties)
		{
			org.json.JSONObject jo = new org.json.JSONObject();
			if (properties != null && !properties.IsEmpty())
			{
				java.util.Enumeration<object> enumProperties = properties.PropertyNames();
				while (enumProperties.MoveNext())
				{
					string name = (string)enumProperties.Current;
					jo.Put(name, properties.GetProperty(name));
				}
			}
			return jo;
		}

		/// <summary>Converts the JSONObject into a property file object.</summary>
		/// <param name="jo">JSONObject</param>
		/// <returns>java.util.Properties</returns>
		/// <exception cref="JSONException"/>
		/// <exception cref="org.json.JSONException"/>
		public static java.util.Properties ToProperties(org.json.JSONObject jo)
		{
			java.util.Properties properties = new java.util.Properties();
			if (jo != null)
			{
				System.Collections.Generic.IEnumerator<string> keys = jo.Keys();
				while (keys.HasNext())
				{
					string name = keys.Next();
					properties[name] = jo.GetString(name);
				}
			}
			return properties;
		}
	}
}
