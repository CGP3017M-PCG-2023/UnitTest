
using System.Collections.Generic;
using System;
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
        public ConstructionPlane editplane = new ConstructionPlane();
        [SerializeField]
        [Range(0.1f, 600.0f)]
        public float width = 2.0f;
        [SerializeField]
        [Range(0.1f, 600.0f)]
        public float height = 2.0f;
        [SerializeField]
        [Range(1, 600)]
        public uint rows = 3;
        [SerializeField]
        [Range(1, 600)]
        public uint columns = 3;
        [SerializeField]
        public Color colour = Color.blue;


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