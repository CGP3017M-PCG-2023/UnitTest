
using System.Collections.Generic;
using UnityEngine;

namespace MiniDini.Nodes
{
    /// <summary>
    /// <see cref="Node"/> that has a list of children.
    /// </summary>
    [System.Serializable]
    public class SphereNode : Node
    {
        #region Overrides of Node

        [SerializeField]
        protected float radius = 2.0f;
        [SerializeField]
        protected uint rows = 3;
        [SerializeField]
        protected uint columns = 3;


        public override string GetDescription() { return "A single Sphere"; }

        /// <summary>
        /// Get the geometry for this Node.
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

            // here is where we construct the geometry for a sphere
            

            return m_geometry;
        }

        #endregion
    }
}