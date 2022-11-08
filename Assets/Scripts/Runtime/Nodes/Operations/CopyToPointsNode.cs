
using System.Collections.Generic;
using UnityEngine;

namespace MiniDini.Nodes
{
    /// <summary>
    /// <see cref="Node"/> that has a list of children.
    /// Allows geometry from one node to be copied to the points of another resulting in duplicates of the first at the vertices of the second
    /// </summary>
    [System.Serializable]
    public class CopyToPointsNode : Node
    {

        #region Overrides of Node

        public override string GetDescription() { return "copies input geometry of first connection to points on second input geometry"; }

        /// <summary>
        /// Get the geometry for this Node.
        /// </summary>
        /// <returns>A geometry object</returns>
        public override Geometry GetGeometry()
        {
            if (m_geometry == null)
            {
                Debug.Log("CopyToPointsNode:Geometry was null in GetGeometry, so creating");
                // create new geometry container
                m_geometry = new Geometry();
            }

            m_geometry.Empty();

            // here is where we construct the geometry 


            return m_geometry;
        }

        #endregion
    }
}