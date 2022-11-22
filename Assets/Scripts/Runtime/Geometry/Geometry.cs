using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniDini
{
	/// <summary>
	/// Geometry is the collection of primitives and points
	/// </summary>
	/// 
	[System.Serializable]
	public class Geometry
	{

		public List<Point> points = new List<Point>();
		public List<Prim> prims = new List<Prim>();


		public Geometry()
		{
			points.Clear();
			prims.Clear();
		}

		// copy constructor (theoretically makes a deep copy!)
		public Geometry(Geometry other)
		{
			points.Clear();
			prims.Clear();

			Merge(other);
		}

		// merge other geometry into this one without clearing
		public void Merge(Geometry other)
		{
			foreach (Point p in other.points)
			{
				Point pn = new Point(p);
				points.Add(pn);
			}
			foreach (Prim pr in other.prims)
			{
				Prim prn = new Prim(pr);
				prims.Add(prn);
			}
		}

		public virtual int AddPoint(Point p) 
		{
			points.Add(p);
			return points.Count-1;
		}

		public virtual int AddPrim(Prim p)
		{
			prims.Add(p);
			return prims.Count-1;
		}

		public void Empty()
		{
			points.Clear();
			prims.Clear();
		}

		public List<Vector3> getPointList()
		{
			List<Vector3> mypoints = new List<Vector3>();
			foreach(Point p in points)
			{
				mypoints.Add(p.position);
			}
			return mypoints;
		}

		// pac note - C# has issues with deep copies of classes, see ICloneable and the like.. basically it uses pass by reference
		// and that royally messes with copies of data within classes (because you get one instance messing with data referenced by another)
		
		public void Copy(Geometry other)
		{
			Geometry copy = new Geometry(other);
			Empty();
			Merge(copy);
		}

		// translate all points in the geometry by the given vector
		public void Translate(Vector3 offset)
		{
			foreach (Point p in points)
			{
				p.position += offset;
			}
		}
		
		// for debugging
		public void Describe(string name)
		{
			Debug.Log("Geometry: " + name + " Points: " + points.Count.ToString() + " Prims: " + prims.Count.ToString());
			foreach(Point p in points)
			{
				Debug.Log("Point: " + p.GetHashCode().ToString() + " Point.position = " + p.position.ToString());
			}
			foreach(Prim pr in prims)
			{
				string vals = "";
				foreach(int i in pr.points)
				{
					vals += i.ToString() + " ";
				}
				Debug.Log("Prim: " + pr.GetHashCode().ToString() + " Indices = " + vals);
			}
		}

	}

}