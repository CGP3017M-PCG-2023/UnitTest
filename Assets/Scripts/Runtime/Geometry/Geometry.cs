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

			foreach (Point p in other.points)
			{
				Point pn = new Point(p);
				points.Add(pn);
			}
			//points.AddRange(other.points);
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

		public Vector3 GetPoint(int index)
		{
			return points[index].position;
		}

		public virtual int FindPoint(Vector3 pos, bool addPointIfNull)
		{
			for (int i = 0; i < points.Count; i += 1)
			{
				if (points[i].position == pos)
				{
					return i;
				}
			}

			//Debug.Log("DIDN'T FIND THE POINT - " + pos);
			
			if (addPointIfNull)
			{
				Point point = new();

				point.position = pos;

				int j = AddPoint(point);

				//Debug.Log("GIVING " + pos + " AS NEW INDEX " + j);

				return j;
			}
			else
			{
				return -1;
			}

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

		public List<Prim> getPrimList()
		{
			List<Prim> myprims = new List<Prim>();
			foreach(Prim p in prims)
			{
				myprims.Add(p);
			}
			return myprims;
		}
		
		public void OrphanPoint(int index)
		{
			for (int i = 0; i < prims.Count; i += 1)
			{
				for (int j = 0; j < prims[i].points.Count; j += 1)
				{
					if (index == prims[i].points[j])
					{
						Debug.Log("OrphanPoint removal");
						prims[i].selected = true;
						prims[i].points.RemoveAt(j);
						j -= 1;
					}
				}
			}
		}

		public void RemoveOrphanedPoint(int index)
		{
			Debug.Log("Orphaned point index #" + index + " removed. Adjusting");
			points.RemoveAt(index);

			for (int i = 0; i < prims.Count; i += 1)
			{
				for (int j = 0; j < prims[i].points.Count; j += 1)
				{
					//if (prims[i].points[j] == index)
					//{
					//	Debug.Log("Point index #" + j + " of prim #" + i + " is the same index as " + index + " (was " + prims[i].points[j] + "). Removing");
					//	prims[i].points.RemoveAt(j);
					//	prims[i].positions.RemoveAt(j);
					//	j -= 1;
					//}
					
					if (prims[i].points[j] >= index)
					{
						Debug.Log("Point index #" + j + " of prim #" + i + " adjusted to " + (prims[i].points[j]-1) + " (was " + prims[i].points[j] + ")");
						prims[i].points[j] -= 1;
					}

					// removes point if point index becomes less than 0 - this shouldn't be possible, but who knows
					//if (prims[i].points[j] < 0)
					//{
					//	prims[i].points.RemoveAt(j);
					//	j -= 1;
					//}
				}
			}
		}

		public int getPrimsAttachedToPoint(int index)
		{
			int count = 0;

			for (int i = 0; i < prims.Count; i += 1)
			{
				//Debug.Log("Checking prim " + i);
				for (int j = 0; j < prims[i].points.Count; j += 1)
				{
					//Debug.Log("Checking point " + j + " of prim " + i);
					if (index == prims[i].points[j])
					{
						Debug.Log(index + " is used for point " + j + " of prim " + i);
						count += 1;
					}
				}
			}

			return count;
		}		

		// pac note - C# has issues with deep copies of classes, see ICloneable and the like.. basically it uses pass by reference
		// and that royally messes with copies of data within classes (because you get one instance messing with data referenced by another)
		
		public void Copy(Geometry other)
		{
			Geometry copy = new Geometry(other);
			Empty();
			foreach(Point p in copy.points)
			{
				Point pn = new Point(p);
				points.Add(pn);
			}
			//points.AddRange(other.points);
			foreach(Prim pr in copy.prims)
			{
				Prim prn = new Prim(pr);
				prims.Add(prn);
			}
			//prims.AddRange(other.prims);
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
