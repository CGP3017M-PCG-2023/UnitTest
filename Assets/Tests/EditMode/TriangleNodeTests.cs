using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using MiniDini.Nodes;
using MiniDini;

public class TriangleNodeTests
{

    // we write a series of tests for each node
    static TriangleNode trinode = null;
    static Geometry geom = null;

    static void MakeNodeAndGeometry()
	{
        geom = null;
        trinode = null;

        if(trinode == null)
            trinode = (TriangleNode)ScriptableObject.CreateInstance<TriangleNode>();
        if(geom == null)
            geom = trinode.GetGeometry();
    }

    [Test]
    public void TriangleNodeIsNotNull()
    {
        MakeNodeAndGeometry();

        Assert.NotNull(trinode, "Triangle Node must not be null");
    }
    [Test]
    public void TriangleNodeGeometryIsNotNull()
    {
        MakeNodeAndGeometry();

        Assert.NotNull(geom, "Geometry must not be null");
        Assert.True(geom.points.Count > 0, "Triangle Geometry shouldn't be empty");
    }

    [Test]
    public void TriangleNodeHasThreePoints()
    {
        MakeNodeAndGeometry();

        Assert.NotNull(geom.points, "Geometry.points must not be null");
        // Use the Assert class to test conditions, in this case, do we have three points for the triangle?
        Assert.AreEqual(3,geom.points.Count);
    }


    [Test]
    public void TriangleNodeHasOnePrim()
    {
        MakeNodeAndGeometry();

        Assert.NotNull(geom.prims, "Geometry.prims must not be null");
        // Use the Assert class to test conditions, in this case, do we have one primitive for the triangle?
        Assert.AreEqual(1, geom.prims.Count);
    }

    [Test]
    public void TriangleNodePrimIndicesAreValidPoints()
    {
        MakeNodeAndGeometry();

        Assert.NotNull(geom.prims, "Geometry.prims must not be null");
        // we need to ensure that each index in the primitive, is a valid point (i.e. lies within the points list)
        Prim pr = geom.prims[0];
        foreach(int p in pr.points)
		{
            Assert.GreaterOrEqual(p, 0);
            Assert.LessOrEqual(p, geom.points.Count);
        }
    }


    [Test]
    public void TriangleNodePointsInnerAngles180()
    {
        MakeNodeAndGeometry();

        float cumulative_angle = 0.0f;
        Vector3 a = geom.points[0].position;
        Vector3 b = geom.points[1].position;
        Vector3 c = geom.points[2].position;

        Vector3 ab = (b - a).normalized;
        Vector3 ac = (c - a).normalized;
        float angle_at_a = Vector3.Angle(ab, ac);
        // uncomment these debug logs to see the inner angles
        //Debug.Log(angle_at_a);
        cumulative_angle += angle_at_a;
        Vector3 ba = (a - b).normalized;
        Vector3 cc = (c - b).normalized;
        float angle_at_b = Vector3.Angle(ba, cc);
        //Debug.Log(angle_at_b);
        cumulative_angle += angle_at_b;
        Vector3 ca = (a - c).normalized;
        Vector3 cb = (b - c).normalized;
        float angle_at_c = Vector3.Angle(ca, cb);
        cumulative_angle += angle_at_c;
        //Debug.Log(angle_at_c);

        Assert.AreEqual(180.0f, cumulative_angle,0.001d);

    }


    [Test]
    public void TriangleNodeInnerAnglesAreEqual()
    {
        MakeNodeAndGeometry();

        Vector3 a = geom.points[0].position;
        Vector3 b = geom.points[1].position;
        Vector3 c = geom.points[2].position;

        Vector3 ab = (b - a).normalized;
        Vector3 ac = (c - a).normalized;
        Vector3 ba = (a - b).normalized;
        Vector3 bc = (c - b).normalized;
        Vector3 ca = (a - c).normalized;
        Vector3 cb = (b - c).normalized;
        float angle_at_a = Vector3.Angle(ab, ac);
        float angle_at_b = Vector3.Angle(ba, bc);
        float angle_at_c = Vector3.Angle(ca, cb);

    //    Debug.Log(angle_at_a);
    //    Debug.Log(angle_at_b);
    //    Debug.Log(angle_at_c);

        Assert.AreEqual(angle_at_a, angle_at_b, 0.001d);
        Assert.AreEqual(angle_at_b, angle_at_c, 0.001d);
        Assert.AreEqual(angle_at_c, angle_at_a, 0.001d);

    }



}

