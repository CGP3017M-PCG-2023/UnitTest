// BehaviorTreeRunner.cs
// 05-13-2022
// James LaFritz

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MiniDini.Runner
{
    /// <summary>
    /// <a href="https://docs.unity3d.com/ScriptReference/MonoBehaviour.html" rel="external">"UnityEngine.MonoBehaviour"</a>
    /// That allows you to run a <see cref="NodeGraph"/> in Unity.
    /// </summary>
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class NodeGraphRunner : MonoBehaviour
    {
        /// <summary>
        /// The <see cref="NodeGraph"/> to run.
        /// </summary>
        [SerializeField] public NodeGraph graph = null;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
        /// </summary>
        private void Start()
        {
            if (!graph) return;
            graph = graph.Clone();
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            if (!graph) return;
            //graph.Update();
        }

        // from https://catlikecoding.com/unity/tutorials/procedural-meshes/creating-a-mesh/

        void OnEnable()
        {

            var mesh = new Mesh
            {
                name = "Procedural Mesh"
            };

            GetComponent<MeshFilter>().mesh = mesh;

            /*
            mesh.vertices = new Vector3[] { Vector3.zero, Vector3.right, Vector3.up };
            mesh.normals = new Vector3[] { Vector3.back, Vector3.back, Vector3.back };
            mesh.uv = new Vector2[] { Vector2.zero, Vector2.right, Vector2.up  };
            mesh.triangles = new int[] { 0, 2, 1 };
            */

            if(graph && graph.outputNode)
			{
                Geometry geom = graph.outputNode.GetGeometry();
                List<Vector3> pointslist = new List<Vector3>();
                foreach(Point p in geom.points)
				{
                    pointslist.Add(p.position);
				}
                mesh.vertices = pointslist.ToArray();
                foreach(Prim pr in geom.prims)
				{
                    List<int> indexlist = new List<int>();
                    // triangle prims are simply added to the list of point indices for each tri
                    if (pr.points.Count == 3)
					{
                        indexlist.AddRange(pr.points);
					}
                    // quad prims are treated as two triangles
                    if (pr.points.Count == 4)
                    {
                        indexlist.Add(pr.points[0]);
                        indexlist.Add(pr.points[1]);
                        indexlist.Add(pr.points[2]);
                        indexlist.Add(pr.points[0]);
                        indexlist.Add(pr.points[2]);
                        indexlist.Add(pr.points[3]);

                    }
                    mesh.triangles = indexlist.ToArray();
				}

			}

        }


    }
}