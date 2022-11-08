using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using MiniDini.Nodes;
using MiniDini;

public class TransformNodeTests
{
    static CubeNode cubenode = null;
    static TriangleNode trinode = null;
    static TransformNode transformnode = null;
    static QuadNode quadnode = null;
    static GridNode gridnode = null;

    static void MakeNodesAndGeometry(int setup = 0)
    {
        if (cubenode == null)
        {
            cubenode = (CubeNode)ScriptableObject.CreateInstance<CubeNode>();

        }

        if (quadnode == null)
        {
            quadnode = (QuadNode)ScriptableObject.CreateInstance<QuadNode>();

        }

        if (trinode == null)
        {
            trinode = (TriangleNode)ScriptableObject.CreateInstance<TriangleNode>();

        }

        if (gridnode == null)
        {
            gridnode = (GridNode)ScriptableObject.CreateInstance<GridNode>();

        }

        if (transformnode == null)
        {
            transformnode = (TransformNode)ScriptableObject.CreateInstance<TransformNode>();
        }
        transformnode.RemoveAllParents();
        transformnode.rotation = new Vector3(0, 0, 0);
        transformnode.translation = new Vector3(0, 0, 0);

        switch (setup)
        {
            case 0:
                transformnode.AddParent(trinode);
                break;
            case 1:
                transformnode.AddParent(quadnode);
                break;
            case 2:
                transformnode.AddParent(cubenode);
                break;

        }



    }


    [Test]
    public void TransformNodeIsNotNull()
    {
        MakeNodesAndGeometry();

        Assert.NotNull(transformnode, "Node must not be null");
    }

    /// <summary>
    /// Basically translates/rotates geometry using different geometry and different tests (rotation, translation)
    /// </summary>

    [Test]
    public void TransformNodeTranslatesTriangle()
    {
        MakeNodesAndGeometry(0);
        Vector3 translate = new Vector3(100, 0, 0);
        transformnode.translation = translate;

        Geometry trianglegeom = trinode.GetGeometry(); 
        Geometry geom = transformnode.GetGeometry();


        Assert.NotNull(geom, "Geometry must not be null");
        Assert.True(geom.points.Count == 3, "Geometry count should have 3 points for a single tri");
        Assert.True(geom.prims.Count == 1, "Geometry prims should be one for single tri");
        for(int i = 0; i < 3; i++)
		{
            Assert.True(geom.points[i].position == trianglegeom.points[i].position + translate, "Geometry points should be moved by translation value");
        }
        
    }

    /// <summary>
    /// Basically rotates a single triangle, which should lay it flat in the Y
    /// </summary>

    [Test]
    public void TransformNodeRotatesTriangle()
    {
        MakeNodesAndGeometry(0);
        // rotation around the X axis means that we should have a flat (in the Y axis) triangle!
        Vector3 rotate = new Vector3(90, 0, 0);
        transformnode.rotation = rotate;

        Geometry geom = transformnode.GetGeometry();

        Assert.NotNull(geom, "Geometry must not be null");
        Assert.True(geom.points.Count == 3, "Geometry count should have 3 points for a single tri");
        Assert.True(geom.prims.Count == 1, "Geometry prims should be one for single tri");

        for (int i = 0; i < 3; i++)
        {
            float y = geom.points[i].position.y;
            // what a palaver! rotation floating point innacuracy means that we need to test this way.. erk!
            Assert.True( ((y >= -0.01f)&&(y <= 0.01f)) , "Geometry points should be rotated to lie on Y plane");
        }
    }


}
