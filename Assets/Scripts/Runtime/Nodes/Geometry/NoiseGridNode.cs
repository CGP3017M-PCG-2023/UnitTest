
using System.Collections.Generic;
using System;
using UnityEngine;

namespace MiniDini.Nodes
{
    /// <summary>
    /// <see cref="Node"/> that has a list of children.
    /// </summary>
    [System.Serializable]
    public class NoiseGridNode : GridNode
    {
        #region Overrides of Node

        public override string GetDescription() { return "A noisy grid (heightmap) made of NxM quads"; }

        [SerializeField]
        [Range(0.1f, 9.0f)]
        public float frequency = 3.0f;

        [SerializeField]
        [Range(0.1f, 10.0f)]
        public float strength = 1.0f;

        /// <summary>
        /// Get the geometry for this Node.
        /// </summary>
        /// <returns>A geometry object</returns>
        public override Geometry GetGeometry()
        {
            m_geometry = base.GetGeometry();

            // offset the grid points by a noise value here

            return m_geometry;
        }


        #endregion
    }
}