
using System.Collections.Generic;
using UnityEngine;

namespace MiniDini.Nodes
{
    /// <summary>
    /// <see cref="Node"/> that has a list of children.
    /// The purpose of blast node is to remove geometry
    /// If we have any points selected, then those points should be removed and any primitives that reference those points
    /// should also be removed
    /// One important point of note.. as we remove a point, the indices in the primitives needs to change to match
    /// i.e. if we have four points a,b,c,d and remove b then triangle a,b,c should be removed but triangle a,c,d should remain
    /// but note that the indices of a,c,d that remains have changed, so the prims need updating to reflect the new vertex indices
    /// As we remove prims, we can basically just delete them as they simply index points BUT
    /// We also need to detect any "orphan" points (points with no prims referencing them) and remove them
    /// 
    /// Additional note: It can be useful to use List methods like RemoveAll here, its a good point to learn about lambda's and predicates!
    /// which allow you to use the language in fewer lines of code see: https://www.dotnetperls.com/lambda
    /// or any decent C# language reference - See also LINQ although we wouldn't use that here, its really fun!
    /// </summary>
    [System.Serializable]
    public class BlastNode : Node
    {

        [SerializeField]
        public bool bypass = false;

        #region Overrides of Node

        public override string GetDescription() { return "Remove selected points and/or prims from geometry"; }

        /// <summary>
        /// Get the geometry for this Node.
        /// </summary>
        /// <returns>A geometry object</returns>
        public override Geometry GetGeometry()
        {
            if (m_geometry == null)
            {
                Debug.Log("BlastNode:Geometry was null in GetGeometry, so creating");
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