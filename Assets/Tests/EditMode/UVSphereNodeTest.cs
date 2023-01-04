using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using MiniDini.Nodes;
using MiniDini;
public class UVSphereNodeTests
{
    static UVSphereNode spherenode = null;
    static Geometry geom = null;

    static void MakeNodeAndGeometry()
    {
        if (spherenode == null)
            spherenode = (UVSphereNode)ScriptableObject.CreateInstance<UVSphereNode>();
            spherenode.radius = 2.5f;
            spherenode.segments = 32;
            spherenode.rings = 32;

        if (geom == null)
            geom = spherenode.GetGeometry();
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
    public void UVSphereNodeIsNotNull()
    {
        MakeNodeAndGeometry();

        Assert.NotNull(spherenode, "UV Sphere Node must not be null");
    }
    [Test]
    public void UVSphereNodeGeometryIsNotNull()
    {
        MakeNodeAndGeometry();

        Assert.NotNull(geom, "Geometry must not be null");
    }

    // what tests do we need here?
    // what conventions do we need for constructing a cube?

    // eight vertices
    [Test]
    public void UVSphereNodeHasCorrectPoints()
    {
        MakeNodeAndGeometry();

        Assert.NotNull(geom.points, "Geometry.points must not be null");
        // Use the Assert class to test conditions, in this case, do we have FOUR points for a quad?
        Assert.AreEqual(1024, geom.points.Count);
    }

    [Test]
    public void UVSphereNodeAllPrimsAreQuads()
    {
        MakeNodeAndGeometry();

        Assert.NotNull(geom.prims, "Geometry.prims must not be null");
        // assert that each primitive is a quad (4 indices)
        foreach (Prim prm in geom.prims)
		{
            Assert.AreEqual(4, prm.points.Count);
        }
    }

    /// <summary>
    /// Sphere is a sphere, so all points are equidistant from the center 
    /// </summary>


    [Test]
    public void UVSphereAllPrimPointsAreEquidistantFromCenter()
    {
        MakeNodeAndGeometry();

        Assert.NotNull(geom.points, "Geometry.points must not be null");
        List<Vector3> points = geom.getPointList();
        foreach (Prim prm in geom.prims)
        {
            List<Vector3> primpoints = GetPrimPoints(points, prm);
            foreach(Vector3 p in primpoints)
            {
                Assert.AreEqual(2.5f, p.magnitude);
            }
        }
    }
}