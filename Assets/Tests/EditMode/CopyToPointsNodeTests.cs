using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using MiniDini.Nodes;
using MiniDini;

public class CopyToPointsNodeTests
{
    static GridNode gridnode = null;
    static TriangleNode trinode = null;
    static CubeNode cubenode = null;
    static CopyToPointsNode copytopointsnode = null;

    static void MakeNodesAndGeometry(bool usecube = false)
    {
        if (gridnode == null)
        {
            gridnode = (GridNode)ScriptableObject.CreateInstance<GridNode>();
            gridnode.rows = 3;
            gridnode.columns = 3;
        }
        if (trinode == null)
        {
            trinode = (TriangleNode)ScriptableObject.CreateInstance<TriangleNode>();
        }
        if (cubenode == null)
        {
            cubenode = (CubeNode)ScriptableObject.CreateInstance<CubeNode>();
        }

        if (copytopointsnode == null)
        {
            copytopointsnode = (CopyToPointsNode)ScriptableObject.CreateInstance<CopyToPointsNode>();
        }

        copytopointsnode.RemoveAllParents();

        if (!usecube)
        {
            copytopointsnode.AddParent(trinode);
        }
        else
        {
            copytopointsnode.AddParent(cubenode);
        }

        copytopointsnode.AddParent(gridnode);
    }


    [Test]
    public void CopyToPointsNodeIsNotNull()
    {
        MakeNodesAndGeometry(false);

        Assert.NotNull(copytopointsnode, "Node must not be null");
    }

    /// <summary>
    /// This test uses the triangle as input node and copies its contents to the points of a grid, so we should end up with
    /// (grid points * 3) points and (grid points * 1) prims
    /// using the defualt grid, we have a 3x3 grid with 16 points 
    /// </summary>

    [Test]
    public void CopyToPointsNodeCopiesGeometryTest()
    {
        MakeNodesAndGeometry(false);

        Geometry geom = copytopointsnode.GetGeometry();

        Assert.NotNull(geom, "Geometry must not be null");
        Assert.True(geom.points.Count == (16 * 3), "Geometry point count must be points * 3 (for each triangle copied)");
        Assert.True(geom.prims.Count == 16, "Geometry prims must equal the points from the grid (each is a triangle)");
    }


    /// <summary>
    /// This test uses the triangle as input node and copies its contents to the points of a grid, so we should end up with
    /// (grid points * 3) points and (grid points * 1) prims
    /// using a different grid configuration, so we should have a 10x10 grid with 16 points 
    /// </summary>

    [Test]
    public void CopyToPointsNodeCopiesToArbitraryGridGeometryTest()
    {
        MakeNodesAndGeometry(false);

        gridnode.rows = 10;
        gridnode.columns = 10;

        Geometry geom = copytopointsnode.GetGeometry();

        int numpoints = (int)((gridnode.rows + 1) * (gridnode.columns + 1));

        Assert.NotNull(geom, "Geometry must not be null");
        Assert.True(geom.points.Count == (numpoints * 3), "Geometry point count must be points * 3 (for each triangle copied)");
        Assert.True(geom.prims.Count == numpoints, "Geometry prims must equal the points from the grid (each is a triangle)");
    }

    /// <summary>
    /// This test uses the cube as input node and copies its contents to the points of a grid, so we should end up with
    /// (grid points * 8) points and (grid points * 6) prims
    /// using the defualt grid, we have a 3x3 grid with 16 points so thats 16 * 8 points and 16 * 6 prims
    /// </summary>

    [Test]
    public void CopyToPointsNodeCopiesCubeGeometryTest()
    {
        MakeNodesAndGeometry(true);

        Geometry geom = copytopointsnode.GetGeometry();

        Assert.NotNull(geom, "Geometry must not be null");
        Assert.True(geom.points.Count == (16 * 8), "Geometry point count must be points * 8 (for each triangle copied)");
        Assert.True(geom.prims.Count == (16 * 6), "Geometry prims must equal the points from the grid (each is a triangle)");
    }

}
