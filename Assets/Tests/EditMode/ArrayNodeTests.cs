using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using MiniDini.Nodes;
using MiniDini;

public class ArrayNodeTests
{
    static CubeNode cubenode = null;
    static TriangleNode trinode = null;
    static ArrayNode arraynode = null;
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


        if (arraynode == null)
        {
            arraynode = (ArrayNode)ScriptableObject.CreateInstance<ArrayNode>();
        }
        arraynode.RemoveAllParents();
        switch (setup)
        {
            case 0:
                arraynode.AddParent(trinode);
                break;
            case 1:
                arraynode.AddParent(cubenode);
                break;
            case 2:
                arraynode.AddParent(quadnode);
                break;
        }
        arraynode.numcopies = 1;
        arraynode.radial = false;


    }


    [Test]
    public void ArrayNodeIsNotNull()
    {
        MakeNodesAndGeometry(0);

        Assert.NotNull(arraynode, "Node must not be null");
    }

    /// <summary>
    /// Basically merge different types of geometry in these tests
    /// </summary>

    [Test]
    public void ArrayNodeHasOnePrimForOneCopy()
    {
        MakeNodesAndGeometry(0);

        arraynode.numcopies = 1;
        Geometry geom = arraynode.GetGeometry();

        Assert.NotNull(geom, "Geometry must not be null");
        Assert.True(geom.points.Count == 3, "Geometry count should have 3 points for a single tri");
        Assert.True(geom.prims.Count == 1, "Geometry prims should be one for a single tri");
    }

    [Test]
    public void ArrayNodeHasFourCopies()
    {
        MakeNodesAndGeometry(0);

        arraynode.numcopies = 4;
        arraynode.vec = new Vector3(20, 0, 0);
        Geometry geom = arraynode.GetGeometry();

        Assert.NotNull(geom, "Geometry must not be null");
        Assert.True(geom.points.Count == (4 * 3), "Geometry count should have 3 points for tri * 4 triangles");
        Assert.True(geom.prims.Count == 4, "Geometry prims should be 4");
    }

    [Test]
    public void ArrayNodeHasFourCopiesTranslatedOnXAxis()
    {
        MakeNodesAndGeometry(0);

        arraynode.numcopies = 4;
        arraynode.vec = new Vector3(20, 0, 0);
        Geometry geom = arraynode.GetGeometry();

        Assert.NotNull(geom, "Geometry must not be null");
        Assert.True(geom.points.Count == (4 * 3), "Geometry count should have 3 points for tri * 4 triangles");
        Assert.True(geom.prims.Count == 4, "Geometry prims should be 4");
        // find the highest X value for all points and check if any points have been translated beyond it (the last triangle should have)
        float xmax = 0.0f;
        foreach(Point p in geom.points)
		{
            if (p.position.x > xmax) xmax = p.position.x;
		}
        Assert.True(xmax > 60.0f, "Copied Geometry has been translated");
    }


    /// <summary>
    /// This one is a little strange.. rotating the triangle 4 times by 90 degrees on the x axis, means two triangles are flat on the Y 
    ///  and we count the number of vertices that are almost at Y=0.0f (with a bit of leeway for floating point errors)
    /// </summary>

    [Test]
    public void ArrayNodeHasFourCopiesRotatedOnXAxis()
    {
        MakeNodesAndGeometry(0);

        arraynode.numcopies = 4;
        arraynode.vec = new Vector3(90, 0, 0);
        arraynode.radial = true;
        Geometry geom = arraynode.GetGeometry();

        Assert.NotNull(geom, "Geometry must not be null");
        Assert.True(geom.points.Count == (4 * 3), "Geometry count should have 3 points for tri * 4 triangles");
        Assert.True(geom.prims.Count == 4, "Geometry prims should be 4");
        // find the highest X value for all points and check if they have been translated
        int num_almost_zero = 0;
        foreach (Point p in geom.points)
        {
            float y = p.position.y;
            if ((y >= -0.01f) && (y <= 0.01f)) num_almost_zero++;
        }
        Assert.True(num_almost_zero == 6, "Copied Geometry has been rotated");
    }


}
