
using System.Collections.Generic;
using UnityEngine;

namespace MiniDini.Nodes
{
    /// <summary>
    /// <see cref="Node"/> that has a list of children.
    /// </summary>
    [System.Serializable]
    public class SelectByRangeNode : Node
    {
        public enum SelectionType
        {
            PointsOnly,
            PrimsOnly
        }


        #region Overrides of Node

        /// the idea here, is that we can select points or prims as part of a pattern
        /// for instance, we can select based on step size (remember stride?) so we start at range_start
        /// end at range_end and select points or prims based on the step size (so we skip step points/prims)
        /// It allows us to delete every Nth point/prim for instance!

        [SerializeField]
        [Range(0, 600)]
        public int range_start = 0;
        [SerializeField]
        [Range(0, 600)]
        public int range_end = 0;
        [SerializeField]
        [Range(1, 100)]
        public int step = 1;

        [SerializeField]
        public SelectionType seltype = SelectionType.PrimsOnly;




        public override string GetDescription() { return "Select incoming geometry within range of start/end with step size"; }

        /// <summary>
        /// Get the geometry for this Node.
        /// </summary>
        /// <returns>A geometry object</returns>
        public override Geometry GetGeometry()
        {
            if (m_geometry == null)
            {
                Debug.Log("SelectByRangeNode:Geometry was null in GetGeometry, so creating");

                // create new geometry container if we don't have one from parent just so we return something
                if (m_geometry == null)
                    m_geometry = new Geometry();
            }

            m_geometry.Empty();

            //Debug.Log(point.ToString());

            // here is where we construct the geometry 
            List<Node> parents = GetParents();

            if (parents.Count > 0)
            {
 				Geometry parent_geometry = parents[0].GetGeometry();
                // make a copy of first parents geometry (we should only have one parent!)
                m_geometry.Copy(parent_geometry);

                // do some simple maths to select points/prims here!
            }

            return m_geometry;
        }


        #endregion
    }
}