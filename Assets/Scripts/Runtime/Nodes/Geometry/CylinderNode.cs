using System.Collections.Generic;
using UnityEngine;

namespace MiniDini.Nodes
{
    ///<summary>
    /// <see cref="Node"/>
    /// This node generates a cylinder geometry that takes a radius, height, and number of sides.
    /// Changing the number of sides will affect the smoothness of the cylinder while changing the radius and height will affect the size of the cylinder.
    /// The cylinder geometry is centered at the origin and is oriented along the z-axis, which can be translated and rotated using the transform node.
    /// Some use cases of this node include creating a cylinder for a tree trunk, a cylinder for a column, or a cylinder for a pipe.
    /// Additionally reducing the number of sides will create a polygonal cylinder, which can be used to create a low poly model.
    /// </summary>
    [System.Serializable]
    public class CylinderNode : Node
    {

        #region Overrides of Node

        [SerializeField]
        protected ConstructionPlane editplane = new ConstructionPlane();
        [SerializeField]
        public int sides = 16;
        [SerializeField]
        public float radius = 1.0f;
        [SerializeField]
        public float height = 1.0f;
        [SerializeField]
        public Color colour = Color.red;

        public override string GetDescription() { return "A single cylinder"; }

        /// <summary>
        /// Get the geometry for this Node.
        /// </summary>
        /// <returns>A geometry object</returns>
        public override Geometry GetGeometry()
        {
            if (m_geometry == null)
            {
                Debug.Log("CylinderNode:Geometry was null in GetGeometry, so creating");
                // create new geometry container
                m_geometry = new Geometry();
            }

            m_geometry.Empty();

            // here is where we construct the geometry for a cube

            
            return m_geometry;
        }

        #endregion
    }
}