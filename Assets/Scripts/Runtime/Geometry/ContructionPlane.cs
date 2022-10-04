using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniDini
{
    /// <summary>
    /// ConstructionPlane represents the plane being used to construct an object, may also be used to segment space into front/back for operations
    /// involving slicing
    /// </summary>
   	[System.Serializable]
    public class ConstructionPlane
    {
        public Vector3 point = Vector3.zero;
        public Vector3 normal = Vector3.forward;
        public Vector3 up = Vector3.up;
        public Vector3 right = Vector3.right;
    }

}