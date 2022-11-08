
using System.Collections.Generic;
using UnityEngine;

// For the array node, we basically want to be able to make copies of geometry
// either translated or rotated
// so if radial == true, then we rotate (vec is rotation around x,y,z in degrees)
// otherwise, vec is basically just a vector to transform each copy by
// if numcopies = 4, then we create 4 copies starting at the original
// and transforming by the amount in vec


namespace MiniDini.Nodes
{
    /// <summary>
    /// <see cref="Node"/> that transforms geometry
    /// </summary>
    [System.Serializable]
    public class ArrayNode : Node
    {
        [SerializeField]
        public Vector3 vec;

        [SerializeField]
        public bool radial = false;

        [SerializeField]
        [Range(1, 600)]
        public int numcopies = 1;



        #region Overrides of Node

        public override string GetDescription() { return "A node that transforms selected geometry and creates copies along the way"; }

        /// <summary>
        /// Get the geometry for this Node.
        /// </summary>
        /// <returns>A geometry object</returns>
        public override Geometry GetGeometry()
        {
            if (m_geometry == null)
            {
                Debug.Log("ArrayNode:Geometry was null in GetGeometry, so creating");
                // create new geometry container
                m_geometry = new Geometry();
            }

            m_geometry.Empty();

            // here is where we construct the geometry 
            List<Node> parents = GetParents();

            if (parents.Count > 0)
            {
                Geometry parent_geometry = parents[0].GetGeometry();
				// here we have the parent geometry, so we need to create copies of it
				// (numcopies) times, either translating or rotating as we go...
                
                
            }

            return m_geometry;
        }


        #endregion
    }
}