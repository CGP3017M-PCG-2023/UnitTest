using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniDini
{
	/// <summary>
	/// Point is the data class for 3D points (vertices) and has selection attributes too
	/// </summary>
	[System.Serializable]
	public class Point
	{
		public Vector3 position = Vector3.zero;
		public Vector3 normal = Vector3.up;
		public Vector2 uv1 = Vector2.zero;
		public Vector2 uv2 = Vector2.zero;
		public Color col = Color.gray;
		public bool selected = false;


		public Point()
		{
			// does nothing
		}

		// copy constructor
		public Point(Point p)
		{
			position = p.position;
			normal = p.normal;
			uv1 = p.uv1;
			uv2 = p.uv2;
			col = p.col;
			selected = p.selected;
		}

		public Point Clone()
		{
			Point p = new Point();
			p.position = position;
			p.normal = normal;
			p.uv1 = uv1;
			p.uv2 = uv2;
			p.col = col;
			p.selected = selected;
			return p;
		}
	}

}
