using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using MiniDini.Nodes;
using MiniDini;

public class BlastNodeTests
{
    static GridNode gridnode = null;
    static SelectNode selnode = null;
    static BlastNode blastnode = null;

    static void MakeNodesAndGeometry()
    {
        if (gridnode == null)
        {
            gridnode = (GridNode)ScriptableObject.CreateInstance<GridNode>();
            gridnode.rows = 3;
            gridnode.columns = 3;
        }
        if (selnode == null)
        {
            selnode = (SelectNode)ScriptableObject.CreateInstance<SelectNode>();
            selnode.AddParent(gridnode);
        }
        if(blastnode == null)
		{
            blastnode = (BlastNode)ScriptableObject.CreateInstance<BlastNode>();
            blastnode.AddParent(selnode);
        }

    }


    [Test]
    public void BlastNodeIsNotNull()
    {
        MakeNodesAndGeometry();

        Assert.NotNull(blastnode, "Node must not be null");
    }
    [Test]
    public void BlastNodeReturnsParentGeometryWhenBypassed()
    {
        MakeNodesAndGeometry();

        Geometry originalgeom = selnode.GetGeometry();
        blastnode.bypass = true;
        Geometry geom = blastnode.GetGeometry();


        Assert.NotNull(geom, "Geometry must not be null");
        Assert.NotNull(originalgeom, "Input Geometry must not be null");
        Assert.True(originalgeom.points.Count > 0, "Input Geometry must not be empty");
        Assert.True(originalgeom.points.Count == geom.points.Count, "Geometry point count from bypassed blast must match input");
        Assert.True(originalgeom.prims.Count == geom.prims.Count, "Geometry prims from bypassed blast must have same input prims count");
    }

    /// <summary>
    /// This test moves the selection to the extreme top left corner of the grid, sets selection mode to point, with a small radius
    /// so we should only select a single point and then remove it.. should reduce points by 1 and prims by 1 compare to input
    /// </summary>

    [Test]
    public void BlastNodeRemovesSelectionSinglePoint()
    {
        MakeNodesAndGeometry();
        // basically select only a single point by setting selection radius small
        selnode.radius = 0.1f;
        {
            // move the selection to the top left
            selnode.point = new Vector3(-1, 1, 0);
            selnode.seltype = SelectNode.SelectionType.PointsOnly;
            selnode.selmode = SelectNode.SelectionMode.Inside;
            Geometry originalgeom = selnode.GetGeometry();
            Geometry geom = blastnode.GetGeometry();
            Assert.NotNull(geom, "Geometry must not be null");
            Assert.NotNull(originalgeom, "Input Geometry must not be null");
            Assert.True(originalgeom.points.Count > 0, "Input Geometry must not be empty");
            Assert.True(geom.points.Count == originalgeom.points.Count - 1, "Geometry from blast should have reduced point count by 1");
            Assert.True(geom.prims.Count == originalgeom.prims.Count - 1, "Geometry from blast should have reduced prim count by 1");
        }
    }

    /// <summary>
    /// This test moves the selection to the extreme top left corner of the grid, sets selection mode to point, with a slightly larger radius
    /// so we should only select a single point and then remove it.. should reduce points by 1 and prims by 1 compare to input
    /// </summary>

    [Test]
    public void BlastNodeRemovesSelectionMediumRadius()
    {
        MakeNodesAndGeometry();
        // select a larger radius (again from top left) and check if we remove the correct amount of points and prims
        selnode.radius = 0.8f;
        {
            // move the selection to the top left
            selnode.point = new Vector3(-1, 1, 0);
            selnode.seltype = SelectNode.SelectionType.PointsOnly;
            selnode.selmode = SelectNode.SelectionMode.Inside;
            Geometry originalgeom = selnode.GetGeometry();
            Geometry geom = blastnode.GetGeometry();
            Assert.NotNull(geom, "Geometry must not be null");
            Assert.NotNull(originalgeom, "Input Geometry must not be null");
            Assert.True(originalgeom.points.Count > 0, "Input Geometry must not be empty");
            Assert.True(geom.points.Count == originalgeom.points.Count - 3, "Geometry from blast should have reduced point count by 3");
            Assert.True(geom.prims.Count == originalgeom.prims.Count - 3, "Geometry from blast should have reduced prim count by 3");
        }
    }


    /// <summary>
    /// This test moves the selection to the extreme top left corner of the grid, sets selection mode to point, with a slightly larger radius
    /// so we should only select a single point and then remove it.. should reduce points by 1 and prims by 1 compare to input
    /// </summary>

    [Test]
    public void BlastNodeRemovesSelectionLargeRadius()
    {
        MakeNodesAndGeometry();
        // select a larger radius (again from top left) and check if we remove the correct amount of points and prims
        selnode.radius = 1.1f;
        {
            // move the selection to the top left
            selnode.point = new Vector3(-1, 1, 0);
            selnode.seltype = SelectNode.SelectionType.PointsOnly;
            selnode.selmode = SelectNode.SelectionMode.Inside;
            Geometry originalgeom = selnode.GetGeometry();
            Geometry geom = blastnode.GetGeometry();
            Assert.NotNull(geom, "Geometry must not be null");
            Assert.NotNull(originalgeom, "Input Geometry must not be null");
            Assert.True(originalgeom.points.Count > 0, "Input Geometry must not be empty");
            Assert.True(geom.points.Count == originalgeom.points.Count - 4, "Geometry from blast should have reduced point count by 3");
            Assert.True(geom.prims.Count == originalgeom.prims.Count - 4, "Geometry from blast should have reduced prim count by 3");
        }
    }

    /// <summary>
    /// This test moves the selection to the extreme top left corner of the grid, sets selection mode to point, 
    /// with a large enough radius to remove everything.. so we should see no points or prims in the resulting geom
    /// </summary>

    [Test]
    public void BlastNodeRemovesAllGeometry()
    {
        MakeNodesAndGeometry();
        // select a larger radius (again from top left) and check if we remove the correct amount of points and prims
        selnode.radius = 3.8f;
        {
            // move the selection to the top left
            selnode.point = new Vector3(-1, 1, 0);
            selnode.seltype = SelectNode.SelectionType.PointsOnly;
            selnode.selmode = SelectNode.SelectionMode.Inside;
            Geometry originalgeom = selnode.GetGeometry();
            Geometry geom = blastnode.GetGeometry();
            Assert.NotNull(geom, "Geometry must not be null");
            Assert.NotNull(originalgeom, "Input Geometry must not be null");
            Assert.True(originalgeom.points.Count > 0, "Input Geometry must not be empty");
            Assert.True(geom.points.Count == 0, "Geometry from blast should have removed all points");
            Assert.True(geom.prims.Count == 0, "Geometry from blast should have removed all prims");
        }
    }


}
