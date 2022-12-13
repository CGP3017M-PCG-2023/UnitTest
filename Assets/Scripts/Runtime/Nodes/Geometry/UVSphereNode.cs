
using System.Collections.Generic;
using UnityEngine;

namespace MiniDini.Nodes
{
    /// <summary>
    /// <see cref="Node"/> This is the UV sphere node it is used to creat a sphere.
    /// The sphere is created by breaking it down into two attributes the segments and the rings.
    /// The points are then placed on the sphere using the formula: x = r * sin(theta) * cos(phi), y = r * cos(theta), z = r * sin(theta) * sin(phi)
    /// Where theta is the angle around the y axis and phi is the angle around the x axis.
    /// The points are then connected to create the quads which make up the prims of the sphere.
    /// </summary>
    [System.Serializable]
    public class UVSphereNode : Node
    {
        #region Overrides of Node

        [SerializeField]
        public float radius = 2.5f;

        [SerializeField]
        public int segments = 32;

        [SerializeField]
        public int rings = 32;

        public override string GetDescription() { return "Creates a UV sphere."; }

        /// <summary>
        /// Creating a UV sphere using the geomerty system.
        /// </summary>
        /// <returns>A geometry object</returns>
        public override Geometry GetGeometry()
        {
            if (m_geometry == null)
            {
                Debug.Log("SphereNode:Geometry was null in GetGeometry, so creating");
                // create new geometry container
                m_geometry = new Geometry();
            }

            m_geometry.Empty();

            // here is where we construct the geometry for the UV sphere
            // starting with the points

            // create a list of points
            int[] points = new int[segments * rings];

            for (int i = 0; i < segments; i++)
            {
                for (int j = 0; j < rings; j++)
                {
                    // create a point
                    Point point = new();

                    // calculate the angle for the point
                    float theta = (float)i / (float)(segments - 1) * Mathf.PI;
                    float phi = (float)j / (float)(rings - 1) * Mathf.PI * 2.0f;

                    // calculate the position of the point
                    point.position = new Vector3(radius * Mathf.Sin(theta) * Mathf.Cos(phi), radius * Mathf.Cos(theta), radius * Mathf.Sin(theta) * Mathf.Sin(phi));

                    // add the point to the geometry
                    points[i * segments + j] = m_geometry.AddPoint(point);
                }
            }

            // now create the prims
            for (int i = 0; i < segments - 1; i++)
            {
                for (int j = 0; j < segments - 1; j++)
                {
                    // create a quad
                    Prim quad = new();

                    quad.points.Add(points[i * segments + j]);
                    quad.points.Add(points[i * segments + j + 1]);
                    quad.points.Add(points[(i + 1) * segments + j + 1]);
                    quad.points.Add(points[(i + 1) * segments + j]);

                    m_geometry.AddPrim(quad);
                }
            }

            return m_geometry;
        }

        #endregion
    }
}