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

    static void MakeNodeAndGeometry()
    {
        if (gridnode == null)
		{
            gridnode = (GridNode)ScriptableObject.CreateInstance<GridNode>();
            gridnode.rows = 3;
            gridnode.columns = 3;
        }

        if (geom == null)
            geom = gridnode.GetGeometry();
    }

    static bool PrimIsAQuad(Prim p)
	{
        return p.points.Count == 4;
	}

    static void CalculatePrimInnerAngles(Prim p,Geometry g, ref List<float> innerangles )
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


    [Test]
    public void GridNodeIsNotNull()
    {
        MakeNodeAndGeometry();

        Assert.NotNull(gridnode, "Quad Node must not be null");
    }
    [Test]
    public void GridNodeGeometryIsNotNull()
    {
        MakeNodeAndGeometry();

        Assert.NotNull(geom, "Geometry must not be null");
    }

    // what tests do we need here?
    // what conventions do we need for constructing a grid?

    // a 3x3 grid (default) has sixteen points
    [Test]
    public void GridNodeHasSixteenPoints()
    {
        MakeNodeAndGeometry();

        Assert.NotNull(geom.points, "Geometry.points must not be null");
        // Use the Assert class to test conditions, in this case, do we have FOUR points for a quad?
        Assert.AreEqual(16, geom.points.Count);
    }

    // all grid points must lie on the same plane!!
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
        foreach(Vector3 point in points)
		{
            float distance = p.GetDistanceToPoint(point);
            if (Mathf.Abs(distance) > maxdistance)
			{
                maxdistance = Mathf.Abs(distance);
			}
		}
        
        Assert.IsTrue(maxdistance < 0.0001f, "Failed Coplanar test");
    }


    [Test]
    public void GridNodeHasNinePrims()
    {
        MakeNodeAndGeometry();

        Assert.NotNull(geom.prims, "Geometry.prims must not be null");
        // Use the Assert class to test conditions, in this case, do we have one primitive for a quad?
        Assert.AreEqual(9, geom.prims.Count);
    }


    [Test]
    public void GridNodeAllPrimsAreQuads()
    {
        MakeNodeAndGeometry();

        Assert.NotNull(geom.prims, "Geometry.prims must not be null");
        Assert.NotZero(geom.prims.Count, "Geometry should have some primitives");

        foreach (Prim p in geom.prims)
        {
            // Use the Assert class to test conditions, in this case, do we have one primitive for a quad?
            Assert.AreEqual(4, p.points.Count);
        }
    }


    [Test]
    public void GridNodeAllPrimsInnerAnglesAreEqual()
    {
        MakeNodeAndGeometry();

        Assert.NotNull(geom.prims, "Geometry.prims must not be null");

        if (geom.prims.Count == 0)
		{
            Assert.Fail("No Prims in Geometry");
		}

        foreach(Prim p in geom.prims)
		{
            List<float> innerangles = new List<float>();

            CalculatePrimInnerAngles(p, geom,ref innerangles);

            float testAngle = innerangles[0];
            foreach (float angle in innerangles)
            {
                Assert.AreEqual(testAngle, angle, 0.001d);  //Tests all angles against each other and allows for float error
            }

            /*Assert.True(
                innerangles[0] == innerangles[1] && 
                innerangles[1] == innerangles[2] && 
                innerangles[2] == innerangles[3] && 
                innerangles[3] == innerangles[0], "AllAnglesAreEqual");*/
        }
        

    }

    [Test]
    public void GridNodeAllPrimsInnerAngles360()
    {
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
