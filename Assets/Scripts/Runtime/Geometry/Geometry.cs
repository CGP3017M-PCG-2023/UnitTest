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

	}

}