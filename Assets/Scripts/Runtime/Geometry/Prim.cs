using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniDini
{
    /// <summary>
    /// Prim is a primitive, basically indexes into a list of points
    /// It should have either 3 (triangle) or 4 (quad) points
    /// Has a normal so we can render with facets rather than smooth
    /// Holds selection state
    /// </summary>
   	[System.Serializable]
    public class Prim
    {
        public List<int> points = new List<int>();
        public Vector3 normal;
        public bool selected = false;


        // default constructor
        public Prim()
		{
            // does nothing
            points.Clear();
        }

        // copy constructor
        public Prim(Prim p)
		{
            points.Clear();
            normal = p.normal;
            points = p.points;
            selected = p.selected;
        }

        public Prim Clone()
        {
            points.Clear();
            Prim p = new Prim();
            p.normal = normal;
            p.points = points;
            p.selected = selected;
            return p;
        }
    }



}