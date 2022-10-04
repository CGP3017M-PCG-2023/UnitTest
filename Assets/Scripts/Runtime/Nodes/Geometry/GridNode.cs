
using System.Collections.Generic;
using UnityEngine;

namespace MiniDini.Nodes
{
    /// <summary>
    /// <see cref="Node"/> that has a list of children.
    /// </summary>
    [System.Serializable]
    public class GridNode : Node
    {
        [SerializeField]
        protected ConstructionPlane editplane = new ConstructionPlane();
        [SerializeField]
        protected float width = 10.0f;
        [SerializeField]
        protected float height = 10.0f;
        [SerializeField]
        protected uint rows = 1;
        [SerializeField]
        protected uint columns = 1;


        #region Overrides of Node

        public override string GetDescription() { return "A grid made of NxM quads"; }

        /// <summary>
        /// Get the geometry for this Node.
        /// </summary>
        /// <returns>A geometry object</returns>
        public override Geometry GetGeometry()
        {
            if (m_geometry == null)
            {
                Debug.Log("GridNode:Geometry was null in GetGeometry, so creating");
                // create new geometry container
                m_geometry = new Geometry();
            }

            m_geometry.Empty();

            // here is where we construct the geometry for a grid


            return m_geometry;
        }


        #endregion
    }
}