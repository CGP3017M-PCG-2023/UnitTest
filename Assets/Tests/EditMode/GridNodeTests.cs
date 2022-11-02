using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using MiniDini.Nodes;
using MiniDini;

public class GridNodeTests
{
    static GridNode gridnode = null;
    static Geometry geom = null;

    /// <summary>
    /// Makes a default sized Grid Node
    /// </summary>
    static void MakeNodeAndGeometry()
    {
        if (gridnode == null)
        {
            gridnode = (GridNode) ScriptableObject.CreateInstance<GridNode>();
            gridnode.rows = 3;
            gridnode.columns = 3;
        }

        if (geom == null)
            geom = gridnode.GetGeometry();
    }

    /// <summary>
    /// Checks a prim is a quad by having 4 points.
    /// </summary>
    /// <param name="p">Primitive to be tested</param>
    /// <returns>True if the primitive is a quad.</returns>
    static bool PrimIsAQuad(Prim p)
    {
        return p.points.Count == 4;
    }

    /// <summary>
    /// Calculates the inner angles of the primitive.
    /// </summary>
    /// <param name="p"></param>
    /// <param name="g"></param>
    /// <param name="innerangles"></param>
    static void CalculatePrimInnerAngles(Prim p, Geometry g, ref List<float> innerangles)
    {
        Vector3 a = geom.points[p.points[0]].position;
        Vector3 b = geom.points[p.points[1]].position;
        Vector3 c = geom.points[p.points[2]].position;
        Vector3 d = geom.points[p.points[3]].position;

        Vector3 ab = (b - a).normalized;
        Vector3 ad = (d - a).normalized;

        Vector3 ba = (a - b).normalized;
        Vector3 bc = (c - b).normalized;

        Vector3 cd = (d - c).normalized;
        Vector3 cb = (b - c).normalized;

        Vector3 da = (a - d).normalized;
        Vector3 dc = (c - d).normalized;

        innerangles.Add(Vector3.Angle(ab, ad));
        innerangles.Add(Vector3.Angle(ba, bc));
        innerangles.Add(Vector3.Angle(cd, cb));
        innerangles.Add(Vector3.Angle(da, dc));
    }

    /// <summary>
    /// Ensures the Grid Node isn't Null 
    /// </summary>
    [Test]
    public void GridNodeIsNotNull()
    {
        MakeNodeAndGeometry();

        Assert.NotNull(gridnode, "Quad Node must not be null");
    }

    /// <summary>
    /// Check if the size of the Grid Node is the default expectation.
    /// This will provide a pre-warning to indicate why other tests may fail.
    /// </summary>
    [Test]
    public void GridNodeIsUsingDefaultSize()
    {
        // Creates a 2 unit by 2 unit grid, with 3x3 row/cols.
        MakeNodeAndGeometry();

        Assert.NotNull(gridnode, "Quad Node must not be null");
        Assert.AreEqual(3, gridnode.columns);
        Assert.AreEqual(3, gridnode.rows);
        Assert.AreEqual(2, gridnode.width);
        Assert.AreEqual(2, gridnode.height);
    }

    /// <summary>
    /// Checks geometry of the grid node isn't null.
    /// </summary>
    [Test]
    public void GridNodeGeometryIsNotNull()
    {
        MakeNodeAndGeometry();

        Assert.NotNull(geom, "Geometry must not be null");
    }

    /// <summary>
    /// Checks that the default (3x3) grid has sixteen points
    /// </summary>
    [Test]
    public void GridNodeHasSixteenPoints()
    {
        MakeNodeAndGeometry();

        Assert.NotNull(geom.points, "Geometry.points must not be null");
        // Use the Assert class to test conditions, in this case, do we have FOUR points for a quad?
        Assert.AreEqual(16, geom.points.Count);
    }

    /// <summary>
    /// Checks that all grid points must lie on the same plane!!
    /// </summary>
    [Test]
    public void GridNodePointsAreCoplanar()
    {
        MakeNodeAndGeometry();

        Assert.NotNull(geom.points, "Geometry.points must not be null");
        List<Vector3> points = geom.getPointList();

        // uncomment this to check that the coplanar test works (we basically wibble the first vertex to knock the plane around)
        //points[0] = points[0] + new Vector3(0, 0, 0.01f);

        Plane p = new Plane();
        // create a plane from 3 of the grid points..
        p.Set3Points(points[0], points[1], points[4]);
        float maxdistance = 0.0f;
        // loop over all the points and see if they lie on the same plane.. (potentially depends on the starting 3 points eeek!)
        foreach (Vector3 point in points)
        {
            float distance = p.GetDistanceToPoint(point);
            if (Mathf.Abs(distance) > maxdistance)
            {
                maxdistance = Mathf.Abs(distance);
            }
        }

        Assert.IsTrue(maxdistance < 0.0001f, "Failed Coplanar test");
    }
    
    /// <summary>
    /// This tests takes in the default 3x3 grid.
    /// This then checks if it has 9 primitive shapes.
    /// </summary>
    [Test]
    public void GridNodeHasNinePrims()
    {
        // Creates a default sized (3x3) grid.
        MakeNodeAndGeometry();
        Assert.NotNull(geom.prims, "Geometry.prims must not be null");

        // Use the Assert class to test conditions, in this case, do we have one primitive for a quad?
        Assert.AreEqual(9, geom.prims.Count);
    }

    /// <summary>
    /// Checks all the primitives are quads.
    /// </summary>
    [Test]
    public void GridNodeAllPrimsAreQuads()
    {
        // Creates a default sized (3x3) grid.
        MakeNodeAndGeometry();

        Assert.NotNull(geom.prims, "Geometry.prims must not be null");
        Assert.NotZero(geom.prims.Count, "Geometry should have some primitives");

        foreach (Prim p in geom.prims)
        {
            // Use the Assert class to test conditions, in this case, do we have one primitive for a quad?
            Assert.AreEqual(4, p.points.Count);
        }
    }

    /// <summary>
    /// Checks all interior angles are equal (rectangular)
    /// </summary>
    [Test]
    public void GridNodeAllPrimsInnerAnglesAreEqual()
    {
        // Creates a default sized (3x3) grid.
        MakeNodeAndGeometry();

        Assert.NotNull(geom.prims, "Geometry.prims must not be null");

        if (geom.prims.Count == 0)
        {
            Assert.Fail("No Prims in Geometry");
        }

        foreach (Prim p in geom.prims)
        {
            List<float> innerangles = new List<float>();

            CalculatePrimInnerAngles(p, geom, ref innerangles);

            Assert.True(
                innerangles[0] == innerangles[1] &&
                innerangles[1] == innerangles[2] &&
                innerangles[2] == innerangles[3] &&
                innerangles[3] == innerangles[0], "AllAnglesAreEqual");
        }
    }

    /// <summary>
    /// Checks all the inner angles add together to 360 degrees.
    /// </summary>
    [Test]
    public void GridNodeAllPrimsInnerAngles360()
    {
        // Creates a default sized (3x3) grid.
        MakeNodeAndGeometry();

        Assert.NotNull(geom.prims, "Geometry.prims must not be null");

        if (geom.prims.Count == 0)
        {
            Assert.Fail("No Prims in Geometry");
        }

        foreach (Prim p in geom.prims)
        {
            List<float> innerangles = new List<float>();

            CalculatePrimInnerAngles(p, geom, ref innerangles);

            float cumulative_angle = innerangles[0] + innerangles[1] + innerangles[2] + innerangles[3];
            Assert.AreEqual(360.0f, cumulative_angle, 0.001d);
        }
    }
}