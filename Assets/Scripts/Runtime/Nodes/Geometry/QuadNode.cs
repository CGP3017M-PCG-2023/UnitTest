
using System.Collections.Generic;
using UnityEngine;

namespace MiniDini.Nodes
{
    /// <summary>
    /// <see cref="Node"/> that has a list of children.
    /// </summary>
    [System.Serializable]
    public class QuadNode : Node
    {
        [SerializeField]
        protected ConstructionPlane editplane = new ConstructionPlane();
        [SerializeField]
        protected float width = 2.0f;
        [SerializeField]
        protected float height = 2.0f;

        #region Overrides of Node

        public override string GetDescription() { return "A single Quad"; }

        /// <summary>
        /// Get the geometry for this Node.
        /// </summary>
        /// <returns>A geometry object</returns>
        public override Geometry GetGeometry()
        {
            if (m_geometry == null)
            {
                Debug.Log("QuadNode:Geometry was null in GetGeometry, so creating");
                // create new geometry container
                m_geometry = new Geometry();
            }

            m_geometry.Empty();

            // here is where we construct the geometry for a quad (4 points, one primitive with FOUR indices)
            // try constructing otherwise and see if the unit tests capture the failure!

            return m_geometry;
        }


        #endregion
    }
}