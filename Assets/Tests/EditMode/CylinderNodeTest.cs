using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using MiniDini.Nodes;
using MiniDini;
public class CylinderNodeTests
{
    static CylinderNode cylindernode = null;
    static Geometry geom = null;

    static void MakeNodeAndGeometry()
    {
        if (cylindernode == null)
		{
            cylindernode = (CylinderNode)ScriptableObject.CreateInstance<CylinderNode>();
            cylindernode.radius = 1.0f;
            cylindernode.height = 2.0f;
            cylindernode.sides = 16;
        }

        if (geom == null)
            geom = cylindernode.GetGeometry();
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
    public void CylinderNodeIsNotNull()
    {
        MakeNodeAndGeometry();

        Assert.NotNull(cylindernode, "cylinder Node must not be null");
    }
    [Test]
    public void CylinderNodeGeometryIsNotNull()
    {
        MakeNodeAndGeometry();

        Assert.NotNull(geom, "Geometry must not be null");
    }

    // what tests do we need here?
    // what conventions do we need for constructing a cylinder?

    // 34 vertices
    [Test]
    public void CylinderNodeHasThirtyFourPoints()
    {
        MakeNodeAndGeometry();

        Assert.NotNull(geom.points, "Geometry.points must not be null");
        // Use the Assert class to test conditions, in this case, do we have FOUR points for a quad?
        Assert.AreEqual(34, geom.points.Count);
    }
    
    //64 prims

    [Test]
    public void CylinderNodeHasSixtyFourPrims()
    {
        MakeNodeAndGeometry();

        Assert.NotNull(geom.prims, "Geometry.prims must not be null");
        // Use the Assert class to test conditions, in this case, do we have six prims?
        Assert.AreEqual(64, geom.prims.Count);
    }


}
