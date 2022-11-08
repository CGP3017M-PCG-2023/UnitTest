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

        float DebugDrawDuration = 0.0f;
        Color debugcolor = Color.white;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
        /// </summary>
        private void Start()
        {
            if (!graph) return;
            //graph = graph.Clone();
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            if (!graph) return;

            Render();
            //graph.Update();
        }

        // debug draw selection stuff - todo: improve this!!! ideally we need a shader/material that supports per face colours?
        private void DebugDrawSelectedPoint(Vector3 pos)
        {
            float halfpointsize = 0.05f;
            Debug.DrawLine(pos + (Vector3.up * halfpointsize), pos + (-Vector3.up * halfpointsize), debugcolor, DebugDrawDuration);
            Debug.DrawLine(pos + (Vector3.forward * halfpointsize), pos + (-Vector3.forward * halfpointsize), debugcolor, DebugDrawDuration);
            Debug.DrawLine(pos + (Vector3.left * halfpointsize), pos + (-Vector3.left * halfpointsize), debugcolor, DebugDrawDuration);
        }

        private void DebugDrawSelectedTriangle(Vector3 a, Vector3 b, Vector3 c)
		{
            Debug.DrawLine(a,b, debugcolor, DebugDrawDuration);
            Debug.DrawLine(b,c, debugcolor, DebugDrawDuration);
            Debug.DrawLine(c,a, debugcolor, DebugDrawDuration);
        }

        private void DebugDrawSelectedQuad(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            Debug.DrawLine(a, b, debugcolor, DebugDrawDuration);
            Debug.DrawLine(b, c, debugcolor, DebugDrawDuration);
            Debug.DrawLine(c, d, debugcolor, DebugDrawDuration);
            Debug.DrawLine(d, a, debugcolor, DebugDrawDuration);
        }

		// from https://catlikecoding.com/unity/tutorials/procedural-meshes/creating-a-mesh/

		void OnEnable()
        {
            if(GetComponent<MeshFilter>().mesh == null)
			{
                var genmesh = new Mesh
                {
                    name = "Procedural Mesh"
                };

                genmesh.Clear();

                GetComponent<MeshFilter>().mesh = genmesh;
            }
            return;
            var mesh = GetComponent<MeshFilter>().mesh;


            /*
            mesh.vertices = new Vector3[] { Vector3.zero, Vector3.right, Vector3.up };
            mesh.normals = new Vector3[] { Vector3.back, Vector3.back, Vector3.back };
            mesh.uv = new Vector2[] { Vector2.zero, Vector2.right, Vector2.up  };
            mesh.triangles = new int[] { 0, 2, 1 };
            */

            if (graph && graph.outputNode)
			{
                Geometry geom = graph.outputNode.GetGeometry();
                List<Vector3> pointslist = new List<Vector3>();
                List<Color> pointcolourslist = new List<Color>();
                foreach(Point p in geom.points)
				{
                    pointslist.Add(p.position);
                    if(p.selected)
					{
                        pointcolourslist.Add(Color.yellow);
                        DebugDrawSelectedPoint(p.position);
					}
                    else
					{
                        pointcolourslist.Add(Color.blue);
					}
				}
                mesh.vertices = pointslist.ToArray();
                //mesh.SetColors(pointcolourslist);
                List<int> indexlist = new List<int>();
                foreach (Prim pr in geom.prims)
				{
                    // triangle prims are simply added to the list of point indices for each tri
                    if (pr.points.Count == 3)
					{
                        indexlist.AddRange(pr.points);

                        if(pr.selected)
                            DebugDrawSelectedTriangle(geom.points[pr.points[0]].position, geom.points[pr.points[1]].position, geom.points[pr.points[2]].position);
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

                        if (pr.selected)
                            DebugDrawSelectedQuad(geom.points[pr.points[0]].position, geom.points[pr.points[1]].position, geom.points[pr.points[2]].position, geom.points[pr.points[3]].position);
                    }
				}
                mesh.triangles = indexlist.ToArray();
                mesh.SetColors(pointcolourslist,0,pointcolourslist.Count,UnityEngine.Rendering.MeshUpdateFlags.Default);
            }

        }

        void Render()
        {

            var mesh = GetComponent<MeshFilter>().mesh;
            mesh.Clear();

            /*
            mesh.vertices = new Vector3[] { Vector3.zero, Vector3.right, Vector3.up };
            mesh.normals = new Vector3[] { Vector3.back, Vector3.back, Vector3.back };
            mesh.uv = new Vector2[] { Vector2.zero, Vector2.right, Vector2.up  };
            mesh.triangles = new int[] { 0, 2, 1 };
            */

            if (graph && graph.outputNode)
            {
                Geometry geom = graph.outputNode.GetGeometry();
                List<Vector3> pointslist = new List<Vector3>();
                List<Color> pointcolourslist = new List<Color>();
                foreach (Point p in geom.points)
                {
                    pointslist.Add(p.position);
                    if (p.selected)
                    {
                        pointcolourslist.Add(Color.yellow);
                        DebugDrawSelectedPoint(p.position);
                    }
                    else
                    {
                        pointcolourslist.Add(p.col);
                    }
                }
                mesh.vertices = pointslist.ToArray();
                //mesh.SetColors(pointcolourslist);
                List<int> indexlist = new List<int>();
                foreach (Prim pr in geom.prims)
                {
                    // triangle prims are simply added to the list of point indices for each tri
                    if (pr.points.Count == 3)
                    {
                        indexlist.AddRange(pr.points);

                        if (pr.selected)
                            DebugDrawSelectedTriangle(geom.points[pr.points[0]].position, geom.points[pr.points[1]].position, geom.points[pr.points[2]].position);
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

                        if (pr.selected)
                            DebugDrawSelectedQuad(geom.points[pr.points[0]].position, geom.points[pr.points[1]].position, geom.points[pr.points[2]].position, geom.points[pr.points[3]].position);
                    }
                }
                mesh.triangles = indexlist.ToArray();
                mesh.SetColors(pointcolourslist, 0, pointcolourslist.Count, UnityEngine.Rendering.MeshUpdateFlags.Default);
            }

        }


    }
}