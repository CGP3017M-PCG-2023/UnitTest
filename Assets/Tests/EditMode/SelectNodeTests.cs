using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using MiniDini.Nodes;
using MiniDini;

public class SelectNodeTests
{
    static GridNode gridnode = null;

    static SelectNode selnode = null;

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

    }

    static int GetSelectedPointCount(Geometry geom)
	{
        int count = 0;

        foreach(Point p in geom.points)
		{
            if (p.selected) count++;
		}

        return count;
	}

    static int GetSelectedPrimCount(Geometry geom)
    {
        int count = 0;

        foreach (Prim p in geom.prims)
        {
            if (p.selected) count++;
        }

        return count;
    }



    [Test]
    public void SelectNodeIsNotNull()
    {
        MakeNodesAndGeometry();

        Assert.NotNull(selnode, "Node must not be null");
    }
    [Test]
    public void SelectNodeReturnsParentGeometry()
    {
        MakeNodesAndGeometry();

        Geometry originalgeom = gridnode.GetGeometry();

        Geometry geom = selnode.GetGeometry();


        Assert.NotNull(geom, "Geometry must not be null");
        Assert.True(originalgeom.points.Count == geom.points.Count,"Geometry from select must have same input point count");
        Assert.True(originalgeom.prims.Count == geom.prims.Count, "Geometry from select must have same input prims count");
    }

    /// <summary>
    /// This test moves the selection to the extreme corners of the grid, sets selection mode to point, with a small radius
    /// so we should only select a single point (but we test all four corners)
    /// </summary>

    [Test]
    public void SelectNodeReturnsSinglePoint()
    {
        MakeNodesAndGeometry();
        // basically select only a single point by setting selection radius small
        selnode.radius = 0.1f;
		{
            // move the selection to the top left
            selnode.point = new Vector3(-1, 1, 0);
            selnode.seltype = SelectNode.SelectionType.PointsOnly;
            selnode.selmode = SelectNode.SelectionMode.Inside;
            Geometry geom = selnode.GetGeometry();
            Assert.NotNull(geom, "Geometry must not be null");
            int count = GetSelectedPointCount(geom);
            Assert.True(count == 1, "Geometry from select should return a single point");
        }
        {
            // move the selection to the top right
            selnode.point = new Vector3(1, 1, 0);
            selnode.seltype = SelectNode.SelectionType.PointsOnly;
            selnode.selmode = SelectNode.SelectionMode.Inside;
            Geometry geom = selnode.GetGeometry();
            Assert.NotNull(geom, "Geometry must not be null");
            int count = GetSelectedPointCount(geom);
            Assert.True(count == 1, "Geometry from select should return a single point");
        }
        {
            // move the selection to the bottom right
            selnode.point = new Vector3(-1, 1, 0);
            selnode.seltype = SelectNode.SelectionType.PointsOnly;
            selnode.selmode = SelectNode.SelectionMode.Inside;
            Geometry geom = selnode.GetGeometry();
            Assert.NotNull(geom, "Geometry must not be null");
            int count = GetSelectedPointCount(geom);
            Assert.True(count == 1, "Geometry from select should return a single point");
        }
        {
            // move the selection to the bottom left
            selnode.point = new Vector3(-1, -1, 0);
            selnode.seltype = SelectNode.SelectionType.PointsOnly;
            selnode.selmode = SelectNode.SelectionMode.Inside;
            Geometry geom = selnode.GetGeometry();
            Assert.NotNull(geom, "Geometry must not be null");
            int count = GetSelectedPointCount(geom);
            Assert.True(count == 1, "Geometry from select should return a single point");
        }


    }

    /// <summary>
    /// Test that if we set selection mode to outside, with the selection top left with tiny radius, we select all but one point
    /// in selectionmode = outside (i.e. select everything outside of a radius)
    /// </summary>

    [Test]
    public void SelectNodeReturnsAllButSinglePoint()
    {
        MakeNodesAndGeometry();
        // basically select everything but the top left point (small radius selects one point)
        selnode.radius = 0.1f;
        {
            // move the selection to the top left
            selnode.point = new Vector3(-1, 1, 0);
            selnode.seltype = SelectNode.SelectionType.PointsOnly;
            selnode.selmode = SelectNode.SelectionMode.Outside;
            Geometry geom = selnode.GetGeometry();
            Assert.NotNull(geom, "Geometry must not be null");
            int count = GetSelectedPointCount(geom);
            Assert.True(count == geom.points.Count - 1, "Geometry from select should return all but one point");
        }
    }

    /// <summary>
    /// Select a single primitive (top left point we selected selects one prim too)
    /// </summary>

    [Test]
    public void SelectNodeReturnsSinglePrim()
    {
        MakeNodesAndGeometry();
        // basically select everything but the top left point (small radius selects one point)
        selnode.radius = 0.1f;
        {
            // move the selection to the top left
            selnode.point = new Vector3(-1, 1, 0);
            selnode.seltype = SelectNode.SelectionType.PointsAndPrims;
            selnode.selmode = SelectNode.SelectionMode.Inside;
            Geometry geom = selnode.GetGeometry();
            Assert.NotNull(geom, "Geometry must not be null");
            int count = GetSelectedPrimCount(geom);
            Assert.True(count == 1, "Geometry from select prim should return single prim");
        }
    }

    /// <summary>
    /// Basically the test above, but we invert from inside to outside
    /// note: outside means ALL points outside
    /// </summary>


    [Test]
    public void SelectNodeReturnsAllButOnePrim()
    {
        MakeNodesAndGeometry();
        // basically select everything but the top left point (small radius selects one point)
        selnode.radius = 0.1f;
        {
            // move the selection to the top left
            selnode.point = new Vector3(-1, 1, 0);
            selnode.seltype = SelectNode.SelectionType.PrimsOnly;
            selnode.selmode = SelectNode.SelectionMode.Outside;
            Geometry geom = selnode.GetGeometry();
            Assert.NotNull(geom, "Geometry must not be null");
            int count = GetSelectedPrimCount(geom);
            Assert.True(count == geom.prims.Count - 1, "Geometry from select prims should return all but one prim");
        }
    }


}
