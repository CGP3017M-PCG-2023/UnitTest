using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using MiniDini.Nodes;
using MiniDini;

public class MergeNodeTests
{
    static CubeNode cubenode = null;
    static TriangleNode trinode = null;
    static MergeNode mergenode = null;
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


        if (mergenode == null)
        {
            mergenode = (MergeNode)ScriptableObject.CreateInstance<MergeNode>();
        }
        mergenode.RemoveAllParents();
        switch(setup)
		{
            case 0:
                mergenode.AddParent(quadnode);
                mergenode.AddParent(trinode);
                break;
            case 1:
                mergenode.AddParent(cubenode);
                mergenode.AddParent(trinode);
                break;
            case 2:
                mergenode.AddParent(cubenode);
                mergenode.AddParent(cubenode);
                mergenode.AddParent(cubenode);
                mergenode.AddParent(cubenode);
                break;
        }
            
        

    }


    [Test]
    public void MergeNodeIsNotNull()
    {
        MakeNodesAndGeometry();

        Assert.NotNull(mergenode, "Node must not be null");
    }

    /// <summary>
    /// Basically merge different types of geometry in these tests
    /// </summary>

    [Test]
    public void MergeNodeMergesTriangleAndQuad()
    {
        MakeNodesAndGeometry(0);

        Geometry geom = mergenode.GetGeometry();


        Assert.NotNull(geom, "Geometry must not be null");
        Assert.True(geom.points.Count == 7, "Geometry count should have 3 points for tri and 4 points for quad");
        Assert.True(geom.prims.Count == 2, "Geometry prims should be 2, one for tri, one for quad");
    }

    [Test]
    public void MergeNodeMergesTriangleAndCube()
    {
        MakeNodesAndGeometry(1);

        Geometry geom = mergenode.GetGeometry();


        Assert.NotNull(geom, "Geometry must not be null");
        Assert.True(geom.points.Count == 11, "Geometry count should have 3 points for tri and 8 points for cube");
        Assert.True(geom.prims.Count == 7, "Geometry prims should be 7, one for tri, six for cube");
    }

    [Test]
    public void MergeNodeMergesFourCubes()
    {
        MakeNodesAndGeometry(2);

        Geometry geom = mergenode.GetGeometry();


        Assert.NotNull(geom, "Geometry must not be null");
        Assert.True(geom.points.Count == (8 * 4), "Geometry count should have 32 points for three cubes and 8 points for each cube");
        Assert.True(geom.prims.Count == (6 * 4), "Geometry prims should be 24, six for each of four cubes");
    }

}
