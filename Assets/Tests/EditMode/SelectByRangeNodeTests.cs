using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using MiniDini.Nodes;
using MiniDini;

public class SelectByRangeNodeTests
{
    static GridNode gridnode = null;

    static SelectByRangeNode selnode = null;

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
            selnode = (SelectByRangeNode)ScriptableObject.CreateInstance<SelectByRangeNode>();
            selnode.AddParent(gridnode);
        }

    }

    static int GetSelectedPointCount(Geometry geom)
    {
        int count = 0;

        foreach (Point p in geom.points)
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
    public void SelectByRangeNodeIsNotNull()
    {
        MakeNodesAndGeometry();

        Assert.NotNull(selnode, "Node must not be null");
    }
    [Test]
    public void SelectByRangeNodeReturnsParentGeometry()
    {
        MakeNodesAndGeometry();

        Geometry originalgeom = gridnode.GetGeometry();

        Geometry geom = selnode.GetGeometry();


        Assert.NotNull(geom, "Geometry must not be null");
        Assert.NotNull(originalgeom, "Input Geometry must not be null");
        Assert.True(originalgeom.points.Count > 0, "Input Geometry must not be empty");
        Assert.True(originalgeom.points.Count == geom.points.Count, "Geometry from select must have same input point count");
        Assert.True(originalgeom.prims.Count == geom.prims.Count, "Geometry from select must have same input prims count");
    }

    /// <summary>
    /// Here we select the first quad and every third quad so in essence we select the first column of a grid 
    /// </summary>

    [Test]
    public void SelectByRangeNodeSelectsFirstColumn()
    {
        MakeNodesAndGeometry();
        selnode.step = 4;
        selnode.range_start = 0;
        selnode.range_end = 9;
        selnode.seltype = SelectByRangeNode.SelectionType.PrimsOnly;
        Geometry geom = selnode.GetGeometry();
        Assert.NotNull(geom, "Geometry must not be null");
        int count = GetSelectedPrimCount(geom);
        Assert.True(count == 3, "Geometry from selectbyrange should return three prims");

    }

    /// <summary>
    /// Select the whole grid
    /// </summary>

    [Test]
    public void SelectByRangeNodeReturnsAllPrims()
    {
        MakeNodesAndGeometry();
        // basically select everything of the grid
        selnode.step = 1;
        selnode.range_start = 0;
        selnode.range_end = 9;
        selnode.seltype = SelectByRangeNode.SelectionType.PrimsOnly;
        Geometry geom = selnode.GetGeometry();
        Assert.NotNull(geom, "Geometry must not be null");
        int count = GetSelectedPrimCount(geom);
        Assert.True(count == 9, "Geometry from selectbyrange should return all prims");
    }
 
}
