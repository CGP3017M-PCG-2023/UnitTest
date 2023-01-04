
using System.Collections.Generic;
using System;
using UnityEngine;

namespace MiniDini.Nodes
{
    /// <summary>
    /// <see cref="Node"/>
    /// This is a grid of quads with points' z values offset by a noise value.
    /// This is a good way to make a heightmap, or a terrain.
    /// It can also be fed into a CopyToPoints node to randomise the position of objects.
    /// The noise value is based on the x and y coordinates of the grid.
    /// It is generated using the Perlin noise function.
    /// The noise value is scaled by the frequency and strength fields.
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