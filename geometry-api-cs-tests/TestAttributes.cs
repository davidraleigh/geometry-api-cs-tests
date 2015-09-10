using NUnit.Framework;

namespace com.esri.core.geometry
{
	public class TestAttributes : NUnit.Framework.TestFixtureAttribute
	{
		[NUnit.Framework.Test]
		public virtual void TestPoint()
		{
			com.esri.core.geometry.Point pt = new com.esri.core.geometry.Point();
			pt.SetXY(100, 200);
			NUnit.Framework.Assert.IsFalse(pt.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.M));
			pt.AddAttribute(com.esri.core.geometry.VertexDescription.Semantics.M);
			NUnit.Framework.Assert.IsTrue(pt.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.M));
			NUnit.Framework.Assert.IsTrue(double.IsNaN(pt.GetM()));
			pt.SetAttribute(com.esri.core.geometry.VertexDescription.Semantics.M, 0, 13);
			NUnit.Framework.Assert.IsTrue(pt.GetM() == 13);
			pt.AddAttribute(com.esri.core.geometry.VertexDescription.Semantics.Z);
			NUnit.Framework.Assert.IsTrue(pt.GetZ() == 0);
			NUnit.Framework.Assert.IsTrue(pt.GetM() == 13);
			pt.SetAttribute(com.esri.core.geometry.VertexDescription.Semantics.Z, 0, 11);
			NUnit.Framework.Assert.IsTrue(pt.GetZ() == 11);
			NUnit.Framework.Assert.IsTrue(pt.GetM() == 13);
			pt.AddAttribute(com.esri.core.geometry.VertexDescription.Semantics.ID);
			NUnit.Framework.Assert.IsTrue(pt.GetID() == 0);
			NUnit.Framework.Assert.IsTrue(pt.GetZ() == 11);
			NUnit.Framework.Assert.IsTrue(pt.GetM() == 13);
			pt.SetAttribute(com.esri.core.geometry.VertexDescription.Semantics.ID, 0, 1);
			NUnit.Framework.Assert.IsTrue(pt.GetID() == 1);
			NUnit.Framework.Assert.IsTrue(pt.GetZ() == 11);
			NUnit.Framework.Assert.IsTrue(pt.GetM() == 13);
			pt.DropAttribute(com.esri.core.geometry.VertexDescription.Semantics.M);
			NUnit.Framework.Assert.IsTrue(pt.GetID() == 1);
			NUnit.Framework.Assert.IsTrue(pt.GetZ() == 11);
			NUnit.Framework.Assert.IsFalse(pt.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.M));
			com.esri.core.geometry.Point pt1 = new com.esri.core.geometry.Point();
			NUnit.Framework.Assert.IsFalse(pt1.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.M));
			NUnit.Framework.Assert.IsFalse(pt1.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.Z));
			NUnit.Framework.Assert.IsFalse(pt1.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.ID));
			pt1.MergeVertexDescription(pt.GetDescription());
			NUnit.Framework.Assert.IsFalse(pt1.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.M));
			NUnit.Framework.Assert.IsTrue(pt1.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.Z));
			NUnit.Framework.Assert.IsTrue(pt1.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.ID));
		}

		[NUnit.Framework.Test]
		public virtual void TestEnvelope()
		{
			com.esri.core.geometry.Envelope env = new com.esri.core.geometry.Envelope();
			env.SetCoords(100, 200, 250, 300);
			NUnit.Framework.Assert.IsFalse(env.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.M));
			env.AddAttribute(com.esri.core.geometry.VertexDescription.Semantics.M);
			NUnit.Framework.Assert.IsTrue(env.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.M));
			NUnit.Framework.Assert.IsTrue(env.QueryInterval(com.esri.core.geometry.VertexDescription.Semantics.M, 0).IsEmpty());
			env.SetInterval(com.esri.core.geometry.VertexDescription.Semantics.M, 0, 1, 2);
			NUnit.Framework.Assert.IsTrue(env.QueryInterval(com.esri.core.geometry.VertexDescription.Semantics.M, 0).vmin == 1);
			NUnit.Framework.Assert.IsTrue(env.QueryInterval(com.esri.core.geometry.VertexDescription.Semantics.M, 0).vmax == 2);
			NUnit.Framework.Assert.IsFalse(env.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.Z));
			env.AddAttribute(com.esri.core.geometry.VertexDescription.Semantics.Z);
			NUnit.Framework.Assert.IsTrue(env.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.Z));
			NUnit.Framework.Assert.IsTrue(env.QueryInterval(com.esri.core.geometry.VertexDescription.Semantics.Z, 0).vmin == 0);
			NUnit.Framework.Assert.IsTrue(env.QueryInterval(com.esri.core.geometry.VertexDescription.Semantics.Z, 0).vmax == 0);
			env.SetInterval(com.esri.core.geometry.VertexDescription.Semantics.Z, 0, 3, 4);
			NUnit.Framework.Assert.IsTrue(env.QueryInterval(com.esri.core.geometry.VertexDescription.Semantics.Z, 0).vmin == 3);
			NUnit.Framework.Assert.IsTrue(env.QueryInterval(com.esri.core.geometry.VertexDescription.Semantics.Z, 0).vmax == 4);
			NUnit.Framework.Assert.IsFalse(env.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.ID));
			env.AddAttribute(com.esri.core.geometry.VertexDescription.Semantics.ID);
			NUnit.Framework.Assert.IsTrue(env.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.ID));
			NUnit.Framework.Assert.IsTrue(env.QueryInterval(com.esri.core.geometry.VertexDescription.Semantics.ID, 0).vmin == 0);
			NUnit.Framework.Assert.IsTrue(env.QueryInterval(com.esri.core.geometry.VertexDescription.Semantics.ID, 0).vmax == 0);
			env.SetInterval(com.esri.core.geometry.VertexDescription.Semantics.ID, 0, 5, 6);
			NUnit.Framework.Assert.IsTrue(env.QueryInterval(com.esri.core.geometry.VertexDescription.Semantics.ID, 0).vmin == 5);
			NUnit.Framework.Assert.IsTrue(env.QueryInterval(com.esri.core.geometry.VertexDescription.Semantics.ID, 0).vmax == 6);
			NUnit.Framework.Assert.IsTrue(env.QueryInterval(com.esri.core.geometry.VertexDescription.Semantics.Z, 0).vmin == 3);
			NUnit.Framework.Assert.IsTrue(env.QueryInterval(com.esri.core.geometry.VertexDescription.Semantics.Z, 0).vmax == 4);
			NUnit.Framework.Assert.IsTrue(env.QueryInterval(com.esri.core.geometry.VertexDescription.Semantics.M, 0).vmin == 1);
			NUnit.Framework.Assert.IsTrue(env.QueryInterval(com.esri.core.geometry.VertexDescription.Semantics.M, 0).vmax == 2);
			env.DropAttribute(com.esri.core.geometry.VertexDescription.Semantics.M);
			NUnit.Framework.Assert.IsFalse(env.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.M));
			NUnit.Framework.Assert.IsTrue(env.QueryInterval(com.esri.core.geometry.VertexDescription.Semantics.ID, 0).vmin == 5);
			NUnit.Framework.Assert.IsTrue(env.QueryInterval(com.esri.core.geometry.VertexDescription.Semantics.ID, 0).vmax == 6);
			NUnit.Framework.Assert.IsTrue(env.QueryInterval(com.esri.core.geometry.VertexDescription.Semantics.Z, 0).vmin == 3);
			NUnit.Framework.Assert.IsTrue(env.QueryInterval(com.esri.core.geometry.VertexDescription.Semantics.Z, 0).vmax == 4);
			com.esri.core.geometry.Envelope env1 = new com.esri.core.geometry.Envelope();
			env.CopyTo(env1);
			NUnit.Framework.Assert.IsFalse(env1.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.M));
			NUnit.Framework.Assert.IsTrue(env1.QueryInterval(com.esri.core.geometry.VertexDescription.Semantics.ID, 0).vmin == 5);
			NUnit.Framework.Assert.IsTrue(env1.QueryInterval(com.esri.core.geometry.VertexDescription.Semantics.ID, 0).vmax == 6);
			NUnit.Framework.Assert.IsTrue(env1.QueryInterval(com.esri.core.geometry.VertexDescription.Semantics.Z, 0).vmin == 3);
			NUnit.Framework.Assert.IsTrue(env1.QueryInterval(com.esri.core.geometry.VertexDescription.Semantics.Z, 0).vmax == 4);
		}

		[NUnit.Framework.Test]
		public virtual void TestLine()
		{
			com.esri.core.geometry.Line env = new com.esri.core.geometry.Line();
			env.SetStartXY(100, 200);
			env.SetEndXY(250, 300);
			NUnit.Framework.Assert.IsFalse(env.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.M));
			env.AddAttribute(com.esri.core.geometry.VertexDescription.Semantics.M);
			NUnit.Framework.Assert.IsTrue(env.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.M));
			env.SetStartAttribute(com.esri.core.geometry.VertexDescription.Semantics.M, 0, 1);
			env.SetEndAttribute(com.esri.core.geometry.VertexDescription.Semantics.M, 0, 2);
			NUnit.Framework.Assert.IsTrue(env.GetStartAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.M, 0) == 1);
			NUnit.Framework.Assert.IsTrue(env.GetEndAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.M, 0) == 2);
			NUnit.Framework.Assert.IsFalse(env.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.Z));
			env.AddAttribute(com.esri.core.geometry.VertexDescription.Semantics.Z);
			NUnit.Framework.Assert.IsTrue(env.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.Z));
			env.SetStartAttribute(com.esri.core.geometry.VertexDescription.Semantics.Z, 0, 3);
			env.SetEndAttribute(com.esri.core.geometry.VertexDescription.Semantics.Z, 0, 4);
			NUnit.Framework.Assert.IsTrue(env.GetStartAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.Z, 0) == 3);
			NUnit.Framework.Assert.IsTrue(env.GetEndAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.Z, 0) == 4);
			NUnit.Framework.Assert.IsFalse(env.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.ID));
			env.AddAttribute(com.esri.core.geometry.VertexDescription.Semantics.ID);
			NUnit.Framework.Assert.IsTrue(env.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.ID));
			env.SetStartAttribute(com.esri.core.geometry.VertexDescription.Semantics.ID, 0, 5);
			env.SetEndAttribute(com.esri.core.geometry.VertexDescription.Semantics.ID, 0, 6);
			NUnit.Framework.Assert.IsTrue(env.GetStartAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.ID, 0) == 5);
			NUnit.Framework.Assert.IsTrue(env.GetEndAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.ID, 0) == 6);
			NUnit.Framework.Assert.IsTrue(env.GetStartAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.M, 0) == 1);
			NUnit.Framework.Assert.IsTrue(env.GetEndAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.M, 0) == 2);
			NUnit.Framework.Assert.IsTrue(env.GetStartAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.Z, 0) == 3);
			NUnit.Framework.Assert.IsTrue(env.GetEndAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.Z, 0) == 4);
			env.DropAttribute(com.esri.core.geometry.VertexDescription.Semantics.M);
			NUnit.Framework.Assert.IsFalse(env.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.M));
			NUnit.Framework.Assert.IsTrue(env.GetStartAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.ID, 0) == 5);
			NUnit.Framework.Assert.IsTrue(env.GetEndAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.ID, 0) == 6);
			NUnit.Framework.Assert.IsTrue(env.GetStartAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.Z, 0) == 3);
			NUnit.Framework.Assert.IsTrue(env.GetEndAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.Z, 0) == 4);
			com.esri.core.geometry.Line env1 = new com.esri.core.geometry.Line();
			env.CopyTo(env1);
			NUnit.Framework.Assert.IsFalse(env1.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.M));
			NUnit.Framework.Assert.IsTrue(env.GetStartAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.ID, 0) == 5);
			NUnit.Framework.Assert.IsTrue(env.GetEndAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.ID, 0) == 6);
			NUnit.Framework.Assert.IsTrue(env.GetStartAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.Z, 0) == 3);
			NUnit.Framework.Assert.IsTrue(env.GetEndAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.Z, 0) == 4);
		}

		[NUnit.Framework.Test]
		public virtual void TestMultiPoint()
		{
			com.esri.core.geometry.MultiPoint mp = new com.esri.core.geometry.MultiPoint();
			mp.Add(new com.esri.core.geometry.Point(100, 200));
			mp.Add(new com.esri.core.geometry.Point(101, 201));
			mp.Add(new com.esri.core.geometry.Point(102, 202));
			NUnit.Framework.Assert.IsFalse(mp.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.M));
			mp.AddAttribute(com.esri.core.geometry.VertexDescription.Semantics.M);
			NUnit.Framework.Assert.IsTrue(mp.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.M));
			NUnit.Framework.Assert.IsTrue(double.IsNaN(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.M, 0, 0)));
			NUnit.Framework.Assert.IsTrue(double.IsNaN(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.M, 1, 0)));
			NUnit.Framework.Assert.IsTrue(double.IsNaN(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.M, 2, 0)));
			mp.SetAttribute(com.esri.core.geometry.VertexDescription.Semantics.M, 0, 0, 1);
			mp.SetAttribute(com.esri.core.geometry.VertexDescription.Semantics.M, 1, 0, 2);
			mp.SetAttribute(com.esri.core.geometry.VertexDescription.Semantics.M, 2, 0, 3);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.M, 0, 0) == 1);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.M, 1, 0) == 2);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.M, 2, 0) == 3);
			NUnit.Framework.Assert.IsFalse(mp.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.Z));
			mp.AddAttribute(com.esri.core.geometry.VertexDescription.Semantics.Z);
			NUnit.Framework.Assert.IsTrue(mp.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.Z));
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.Z, 0, 0) == 0);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.Z, 1, 0) == 0);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.Z, 2, 0) == 0);
			mp.SetAttribute(com.esri.core.geometry.VertexDescription.Semantics.Z, 0, 0, 11);
			mp.SetAttribute(com.esri.core.geometry.VertexDescription.Semantics.Z, 1, 0, 21);
			mp.SetAttribute(com.esri.core.geometry.VertexDescription.Semantics.Z, 2, 0, 31);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.M, 0, 0) == 1);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.M, 1, 0) == 2);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.M, 2, 0) == 3);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.Z, 0, 0) == 11);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.Z, 1, 0) == 21);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.Z, 2, 0) == 31);
			NUnit.Framework.Assert.IsFalse(mp.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.ID));
			mp.AddAttribute(com.esri.core.geometry.VertexDescription.Semantics.ID);
			NUnit.Framework.Assert.IsTrue(mp.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.ID));
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.ID, 0, 0) == 0);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.ID, 1, 0) == 0);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.ID, 2, 0) == 0);
			mp.SetAttribute(com.esri.core.geometry.VertexDescription.Semantics.ID, 0, 0, -11);
			mp.SetAttribute(com.esri.core.geometry.VertexDescription.Semantics.ID, 1, 0, -21);
			mp.SetAttribute(com.esri.core.geometry.VertexDescription.Semantics.ID, 2, 0, -31);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.M, 0, 0) == 1);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.M, 1, 0) == 2);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.M, 2, 0) == 3);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.Z, 0, 0) == 11);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.Z, 1, 0) == 21);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.Z, 2, 0) == 31);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.ID, 0, 0) == -11);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.ID, 1, 0) == -21);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.ID, 2, 0) == -31);
			mp.DropAttribute(com.esri.core.geometry.VertexDescription.Semantics.M);
			NUnit.Framework.Assert.IsFalse(mp.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.M));
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.Z, 0, 0) == 11);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.Z, 1, 0) == 21);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.Z, 2, 0) == 31);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.ID, 0, 0) == -11);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.ID, 1, 0) == -21);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.ID, 2, 0) == -31);
			com.esri.core.geometry.MultiPoint mp1 = new com.esri.core.geometry.MultiPoint();
			mp.CopyTo(mp1);
			NUnit.Framework.Assert.IsFalse(mp1.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.M));
			NUnit.Framework.Assert.IsTrue(mp1.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.Z, 0, 0) == 11);
			NUnit.Framework.Assert.IsTrue(mp1.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.Z, 1, 0) == 21);
			NUnit.Framework.Assert.IsTrue(mp1.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.Z, 2, 0) == 31);
			NUnit.Framework.Assert.IsTrue(mp1.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.ID, 0, 0) == -11);
			NUnit.Framework.Assert.IsTrue(mp1.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.ID, 1, 0) == -21);
			NUnit.Framework.Assert.IsTrue(mp1.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.ID, 2, 0) == -31);
			mp1.DropAllAttributes();
			mp1.MergeVertexDescription(mp.GetDescription());
			NUnit.Framework.Assert.IsTrue(mp1.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.Z, 0, 0) == 0);
			NUnit.Framework.Assert.IsTrue(mp1.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.Z, 1, 0) == 0);
			NUnit.Framework.Assert.IsTrue(mp1.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.Z, 2, 0) == 0);
			NUnit.Framework.Assert.IsTrue(double.IsNaN(mp1.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.M, 0, 0)));
			NUnit.Framework.Assert.IsTrue(double.IsNaN(mp1.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.M, 1, 0)));
			NUnit.Framework.Assert.IsTrue(double.IsNaN(mp1.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.M, 2, 0)));
			NUnit.Framework.Assert.IsTrue(mp1.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.ID, 0, 0) == 0);
			NUnit.Framework.Assert.IsTrue(mp1.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.ID, 1, 0) == 0);
			NUnit.Framework.Assert.IsTrue(mp1.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.ID, 2, 0) == 0);
		}

		[NUnit.Framework.Test]
		public virtual void TestPolygon()
		{
			com.esri.core.geometry.Polygon mp = new com.esri.core.geometry.Polygon();
			mp.StartPath(new com.esri.core.geometry.Point(100, 200));
			mp.LineTo(new com.esri.core.geometry.Point(101, 201));
			mp.LineTo(new com.esri.core.geometry.Point(102, 202));
			NUnit.Framework.Assert.IsFalse(mp.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.M));
			mp.AddAttribute(com.esri.core.geometry.VertexDescription.Semantics.M);
			NUnit.Framework.Assert.IsTrue(mp.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.M));
			NUnit.Framework.Assert.IsTrue(double.IsNaN(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.M, 0, 0)));
			NUnit.Framework.Assert.IsTrue(double.IsNaN(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.M, 1, 0)));
			NUnit.Framework.Assert.IsTrue(double.IsNaN(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.M, 2, 0)));
			mp.SetAttribute(com.esri.core.geometry.VertexDescription.Semantics.M, 0, 0, 1);
			mp.SetAttribute(com.esri.core.geometry.VertexDescription.Semantics.M, 1, 0, 2);
			mp.SetAttribute(com.esri.core.geometry.VertexDescription.Semantics.M, 2, 0, 3);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.M, 0, 0) == 1);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.M, 1, 0) == 2);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.M, 2, 0) == 3);
			NUnit.Framework.Assert.IsFalse(mp.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.Z));
			mp.AddAttribute(com.esri.core.geometry.VertexDescription.Semantics.Z);
			NUnit.Framework.Assert.IsTrue(mp.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.Z));
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.Z, 0, 0) == 0);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.Z, 1, 0) == 0);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.Z, 2, 0) == 0);
			mp.SetAttribute(com.esri.core.geometry.VertexDescription.Semantics.Z, 0, 0, 11);
			mp.SetAttribute(com.esri.core.geometry.VertexDescription.Semantics.Z, 1, 0, 21);
			mp.SetAttribute(com.esri.core.geometry.VertexDescription.Semantics.Z, 2, 0, 31);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.M, 0, 0) == 1);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.M, 1, 0) == 2);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.M, 2, 0) == 3);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.Z, 0, 0) == 11);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.Z, 1, 0) == 21);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.Z, 2, 0) == 31);
			NUnit.Framework.Assert.IsFalse(mp.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.ID));
			mp.AddAttribute(com.esri.core.geometry.VertexDescription.Semantics.ID);
			NUnit.Framework.Assert.IsTrue(mp.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.ID));
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.ID, 0, 0) == 0);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.ID, 1, 0) == 0);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.ID, 2, 0) == 0);
			mp.SetAttribute(com.esri.core.geometry.VertexDescription.Semantics.ID, 0, 0, -11);
			mp.SetAttribute(com.esri.core.geometry.VertexDescription.Semantics.ID, 1, 0, -21);
			mp.SetAttribute(com.esri.core.geometry.VertexDescription.Semantics.ID, 2, 0, -31);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.M, 0, 0) == 1);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.M, 1, 0) == 2);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.M, 2, 0) == 3);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.Z, 0, 0) == 11);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.Z, 1, 0) == 21);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.Z, 2, 0) == 31);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.ID, 0, 0) == -11);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.ID, 1, 0) == -21);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.ID, 2, 0) == -31);
			mp.DropAttribute(com.esri.core.geometry.VertexDescription.Semantics.M);
			NUnit.Framework.Assert.IsFalse(mp.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.M));
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.Z, 0, 0) == 11);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.Z, 1, 0) == 21);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.Z, 2, 0) == 31);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.ID, 0, 0) == -11);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.ID, 1, 0) == -21);
			NUnit.Framework.Assert.IsTrue(mp.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.ID, 2, 0) == -31);
			com.esri.core.geometry.Polygon mp1 = new com.esri.core.geometry.Polygon();
			mp.CopyTo(mp1);
			NUnit.Framework.Assert.IsFalse(mp1.HasAttribute(com.esri.core.geometry.VertexDescription.Semantics.M));
			NUnit.Framework.Assert.IsTrue(mp1.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.Z, 0, 0) == 11);
			NUnit.Framework.Assert.IsTrue(mp1.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.Z, 1, 0) == 21);
			NUnit.Framework.Assert.IsTrue(mp1.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.Z, 2, 0) == 31);
			NUnit.Framework.Assert.IsTrue(mp1.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.ID, 0, 0) == -11);
			NUnit.Framework.Assert.IsTrue(mp1.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.ID, 1, 0) == -21);
			NUnit.Framework.Assert.IsTrue(mp1.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.ID, 2, 0) == -31);
			mp1.DropAllAttributes();
			mp1.MergeVertexDescription(mp.GetDescription());
			NUnit.Framework.Assert.IsTrue(mp1.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.Z, 0, 0) == 0);
			NUnit.Framework.Assert.IsTrue(mp1.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.Z, 1, 0) == 0);
			NUnit.Framework.Assert.IsTrue(mp1.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.Z, 2, 0) == 0);
			NUnit.Framework.Assert.IsTrue(double.IsNaN(mp1.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.M, 0, 0)));
			NUnit.Framework.Assert.IsTrue(double.IsNaN(mp1.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.M, 1, 0)));
			NUnit.Framework.Assert.IsTrue(double.IsNaN(mp1.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.M, 2, 0)));
			NUnit.Framework.Assert.IsTrue(mp1.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.ID, 0, 0) == 0);
			NUnit.Framework.Assert.IsTrue(mp1.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.ID, 1, 0) == 0);
			NUnit.Framework.Assert.IsTrue(mp1.GetAttributeAsDbl(com.esri.core.geometry.VertexDescription.Semantics.ID, 2, 0) == 0);
		}
	}
}
