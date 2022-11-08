
using System.Collections.Generic;
using UnityEngine;

namespace MiniDini.Nodes
{
    /// <summary>
    /// <see cref="Node"/> that transforms geometry
    /// </summary>
    [System.Serializable]
    public class TransformNode : Node
    {
        [SerializeField]
        public Vector3 translation = new Vector3(0, 0, 0);

        [SerializeField]
        public Vector3 rotation = new Vector3(0, 0, 0);

        [SerializeField]
        public Vector3 scale = new Vector3(1, 1, 1);



        #region Overrides of Node

        public override string GetDescription() { return "A node that transforms selected geometry"; }

        /// <summary>
        /// Get the geometry for this Node.
        /// </summary>
        /// <returns>A geometry object</returns>
        public override Geometry GetGeometry()
        {
            if (m_geometry == null)
            {
                Debug.Log("TransformNode:Geometry was null in GetGeometry, so creating");
                // create new geometry container
                m_geometry = new Geometry();
            }

            m_geometry.Empty();

            // here is where we construct the geometry 
            List<Node> parents = GetParents();

            if (parents.Count > 0)
            {
               	// simple geometry transformation
				// look at Matrix4x4 and Quaternion classes!
            }

            return m_geometry;
        }


        #endregion
    }
}