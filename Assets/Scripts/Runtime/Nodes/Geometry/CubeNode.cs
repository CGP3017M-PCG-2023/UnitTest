
using System.Collections.Generic;
using UnityEngine;

namespace MiniDini.Nodes
{
    /// <summary>
    /// <see cref="Node"/> that has a list of children.
    /// </summary>
    [System.Serializable]
    public class CubeNode : Node
    {

        #region Overrides of Node

        [SerializeField]
        protected ConstructionPlane editplane = new ConstructionPlane();
        [SerializeField]
        protected float size = 1.0f;
        [SerializeField]
        public Color colour = Color.red;

        public override string GetDescription() { return "A single cube"; }

        /// <summary>
        /// Get the geometry for this Node.
        /// </summary>
        /// <returns>A geometry object</returns>
        public override Geometry GetGeometry()
        {
            if (m_geometry == null)
            {
                Debug.Log("CubeNode:Geometry was null in GetGeometry, so creating");
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