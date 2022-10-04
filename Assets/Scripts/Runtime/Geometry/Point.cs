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
	}

}
