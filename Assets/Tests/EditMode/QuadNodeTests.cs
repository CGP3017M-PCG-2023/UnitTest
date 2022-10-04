using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using MiniDini.Nodes;
using MiniDini;
public class QuadNodeTests
{
    static QuadNode quadnode = null;
    static Geometry geom = null;

    static void MakeNodeAndGeometry()
    {
        if (quadnode == null)
            quadnode = (QuadNode)ScriptableObject.CreateInstance<QuadNode>();
        if (geom == null)
            geom = quadnode.GetGeometry();
    }

    [Test]
    public void QuadNodeIsNotNull()
    {
        MakeNodeAndGeometry();

        Assert.NotNull(quadnode, "Quad Node must not be null");
    }
    [Test]
    public void QuadNodeGeometryIsNotNull()
    {
        MakeNodeAndGeometry();

        Assert.NotNull(geom, "Geometry must not be null");
    }

    // what tests do we need here?
    // what conventions do we need for constructing a quad?

    // four points
    [Test]
    public void QuadNodeHasFourPoints()
    {
        MakeNodeAndGeometry();

        Assert.NotNull(geom.points, "Geometry.points must not be null");
        // Use the Assert class to test conditions, in this case, do we have FOUR points for a quad?
        Assert.AreEqual(4, geom.points.Count);
    }

    // four points that all lie on the same plane!! (i.e. they form a planar quad)
    [Test]
    public void QuadNodePointsAreCoplanar()
    {
        MakeNodeAndGeometry();

        Assert.NotNull(geom.points, "Geometry.points must not be null");
        Plane p;
        List<Vector3> points = geom.getPointList();
        bool bPlanar = GeometryUtility.TryCreatePlaneFromPolygon(points.ToArray(), out p);
        Assert.IsTrue(bPlanar, "Failed Coplanar test");
    }


    [Test]
    public void QuadNodeHasOnePrim()
    {
        MakeNodeAndGeometry();

        Assert.NotNull(geom.prims, "Geometry.prims must not be null");
        // Use the Assert class to test conditions, in this case, do we have one primitive for a quad?
        Assert.AreEqual(1, geom.prims.Count);
    }


    /// <summary>
    /// Quad is A,B,C,D
    /// 
    /// A---B
    /// !   !
    /// D---C
    /// 
    /// 
    /// </summary>


    [Test]
    public void QuadNodeInnerAnglesAreEqual()
    {
        MakeNodeAndGeometry();

        Vector3 a = geom.points[0].position;
        Vector3 b = geom.points[1].position;
        Vector3 c = geom.points[2].position;
        Vector3 d = geom.points[3].position;

        Vector3 ab = (b - a).normalized;
        Vector3 ad = (d - a).normalized;

        Vector3 ba = (a - b).normalized;
        Vector3 bc = (c - b).normalized;

        Vector3 cd = (d - c).normalized;
        Vector3 cb = (b - c).normalized;

        Vector3 da = (a - d).normalized;
        Vector3 dc = (c - d).normalized;

        float angle_at_a = Vector3.Angle(ab, ad);
        float angle_at_b = Vector3.Angle(ba, bc);
        float angle_at_c = Vector3.Angle(cd, cb);
        float angle_at_d = Vector3.Angle(da, dc);

        //    Debug.Log(angle_at_a);
        //    Debug.Log(angle_at_b);
        //    Debug.Log(angle_at_c);

        Assert.AreEqual(angle_at_a, angle_at_b, 0.001d);
        Assert.AreEqual(angle_at_b, angle_at_c, 0.001d);
        Assert.AreEqual(angle_at_c, angle_at_a, 0.001d);
        Assert.AreEqual(angle_at_d, angle_at_a, 0.001d);

    }

    [Test]
    public void QuadNodeInnerAngles360()
    {
        MakeNodeAndGeometry();

        Vector3 a = geom.points[0].position;
        Vector3 b = geom.points[1].position;
        Vector3 c = geom.points[2].position;
        Vector3 d = geom.points[3].position;

        Vector3 ab = (b - a).normalized;
        Vector3 ad = (d - a).normalized;

        Vector3 ba = (a - b).normalized;
        Vector3 bc = (c - b).normalized;

        Vector3 cd = (d - c).normalized;
        Vector3 cb = (b - c).normalized;

        Vector3 da = (a - d).normalized;
        Vector3 dc = (c - d).normalized;

        float angle_at_a = Vector3.Angle(ab, ad);
        float angle_at_b = Vector3.Angle(ba, bc);
        float angle_at_c = Vector3.Angle(cd, cb);
        float angle_at_d = Vector3.Angle(da, dc);

        float cumulative_angle = angle_at_a + angle_at_b + angle_at_c + angle_at_d;

        Assert.AreEqual(360.0f, cumulative_angle, 0.001d);

    }


}
