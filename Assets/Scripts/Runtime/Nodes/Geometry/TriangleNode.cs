
using System.Collections.Generic;
using UnityEngine;
using MiniDini;

namespace MiniDini.Nodes
{
    /// <summary>
    /// <see cref="Node"/> that has a list of children.
    /// </summary>
    [System.Serializable]
    public class TriangleNode : Node
    {
        [SerializeField] 
        protected ConstructionPlane editplane = new ConstructionPlane();
        [SerializeField]
        protected float radius = 2.0f;
        [SerializeField]
        public Color colour = Color.green;


        #region Overrides of Node

        public override string GetDescription() { return "A single Triangle"; }
        
        /// <summary>
        /// Get the geometry for this Node.
        /// </summary>
        /// <returns>A geometry object</returns>
        public override Geometry GetGeometry()
        {
            if (m_geometry == null)
			{
                Debug.Log("TriangleNode:Geometry was null in GetGeometry, so creating");
                // create new geometry container
                m_geometry = new Geometry();
            }

            m_geometry.Empty();

            // here is where we construct the geometry for a triangle (3 points, one primitive with three indices)
            // try constructing otherwise and see if the unit tests capture the failure!
            
            return m_geometry;
        }


        #endregion
    }
}