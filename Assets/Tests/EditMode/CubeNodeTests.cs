using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using MiniDini.Nodes;
using MiniDini;
public class CubeNodeTests
{
    static CubeNode cubenode = null;
    static Geometry geom = null;

    static void MakeNodeAndGeometry()
    {
        if (cubenode == null)
            cubenode = (CubeNode)ScriptableObject.CreateInstance<CubeNode>();
        if (geom == null)
            geom = cubenode.GetGeometry();
    }

    /// <summary>
    /// static function to get the vertex positions for each primitive
    /// </summary>
    /// <param name="pointslist"></param>
    /// <param name="p"></param>
    /// <returns></returns>
    static List<Vector3> GetPrimPoints(List<Vector3> pointslist, Prim p)
	{
        List<Vector3> primpoints = new List<Vector3>();
        foreach(int index in p.points)
		{
            primpoints.Add(pointslist[index]);
		}

        return primpoints;
	}


    [Test]
    public void CubeNodeIsNotNull()
    {
        MakeNodeAndGeometry();

        Assert.NotNull(cubenode, "Cube Node must not be null");
    }
    [Test]
    public void CubeNodeGeometryIsNotNull()
    {
        MakeNodeAndGeometry();

        Assert.NotNull(geom, "Geometry must not be null");
    }

    // what tests do we need here?
    // what conventions do we need for constructing a cube?

    // eight vertices
    [Test]
    public void CubeNodeHasEightPoints()
    {
        MakeNodeAndGeometry();

        Assert.NotNull(geom.points, "Geometry.points must not be null");
        // Use the Assert class to test conditions, in this case, do we have FOUR points for a quad?
        Assert.AreEqual(8, geom.points.Count);
    }

    

    [Test]
    public void CubeNodeHasSixPrims()
    {
        MakeNodeAndGeometry();

        Assert.NotNull(geom.prims, "Geometry.prims must not be null");
        // Use the Assert class to test conditions, in this case, do we have six prims?
        Assert.AreEqual(6, geom.prims.Count);
    }

    [Test]
    public void CubeNodeAllSixPrimsAreQuads()
    {
        MakeNodeAndGeometry();

        Assert.NotNull(geom.prims, "Geometry.prims must not be null");
        // Use the Assert class to test conditions, need six prims
        Assert.AreEqual(6, geom.prims.Count);
        // assert that each primitive is a quad (4 indices)
        foreach (Prim prm in geom.prims)
		{
            Assert.AreEqual(4, prm.points.Count);
        }
    }


    // 
    [Test]
    public void CubeNodeAllPrimPointsAreCoplanar()
    {
        MakeNodeAndGeometry();

        Assert.NotNull(geom.points, "Geometry.points must not be null");
        Assert.AreEqual(6, geom.prims.Count);
        List<Vector3> points = geom.getPointList();
        foreach(Prim prm in geom.prims)
		{
            Plane p;
            List<Vector3> primpoints = GetPrimPoints(points,prm);
            bool bPlanar = GeometryUtility.TryCreatePlaneFromPolygon(primpoints.ToArray(), out p);
            Assert.IsTrue(bPlanar, "Failed Coplanar test");
        }
        Assert.Pass("All prims are coplanar");
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
    public void CubeNodeAllPrimInnerAnglesAreEqual()
    {
        MakeNodeAndGeometry();

        Assert.NotNull(geom.points, "Geometry.points must not be null");
        Assert.NotNull(geom.prims, "Geometry.prims must not be null");
        Assert.AreEqual(6, geom.prims.Count);
        List<Vector3> points = geom.getPointList();

        foreach (Prim prm in geom.prims)
		{
            List<Vector3> primpoints = GetPrimPoints(points, prm);

            Vector3 a = primpoints[0];
            Vector3 b = primpoints[1];
            Vector3 c = primpoints[2];
            Vector3 d = primpoints[3];

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


    }

    [Test]
    public void CubeNodeAllPrimInnerAngles360()
    {
        MakeNodeAndGeometry();

        Assert.NotNull(geom.points, "Geometry.points must not be null");
        Assert.NotNull(geom.prims, "Geometry.prims must not be null");
        Assert.AreEqual(6, geom.prims.Count);
        List<Vector3> points = geom.getPointList();

        foreach (Prim prm in geom.prims)
        {
            List<Vector3> primpoints = GetPrimPoints(points, prm);

            Vector3 a = primpoints[0];
            Vector3 b = primpoints[1];
            Vector3 c = primpoints[2];
            Vector3 d = primpoints[3];

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


}
