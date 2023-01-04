
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
        #endregion
    }
}
