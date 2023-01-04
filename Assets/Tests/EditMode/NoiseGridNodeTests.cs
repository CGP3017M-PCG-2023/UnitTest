using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using MiniDini.Nodes;
using MiniDini;

public class NoiseGridNodeTests
{
    static NoiseGridNode gridnode = null;
    static Geometry geom = null;

    static void MakeNodeAndGeometry()
    {
        if (gridnode == null)
		{
            gridnode = (NoiseGridNode)ScriptableObject.CreateInstance<NoiseGridNode>();
            gridnode.width = 10;
            gridnode.height = 10;
            gridnode.rows = 10;
            gridnode.columns = 10;
            gridnode.frequency = 8.0f;
            gridnode.strength = 5.0f;
        }

        if (geom == null)
            geom = gridnode.GetGeometry();
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
    
    // we do not have to test the validity of the grid itself since that code is inherited from the GridNode

    [Test]
    public void NoiseGridNodeIsNotFlat()
    {
        MakeNodeAndGeometry();
        
        // simply check that the z values are not all the same
        var first_z = geom.points[0].position.z;
        bool flag = false;
        foreach (var point in geom.points)
        {
            if (point.position.z != first_z)
            {
                flag = true;
                break;
            }
        }
        Assert.True(flag, $"Grid must not be flat (all points should have randomised z values)");
    }


    [Test]
    public void NoiseGridNodeIsWellDistributed()
    {
        MakeNodeAndGeometry();

        // calculate the average z value
        float avg_z = 0.0f;
        foreach (var point in geom.points)
        {
            avg_z += point.position.z;
        }
        avg_z /= geom.points.Count;

        // check that the average z value is not too far from 0, accounting for the strength of the noise
        avg_z -= gridnode.strength / 2.0f;
        Assert.True(Mathf.Abs(avg_z) < 0.5f, $"Grid must be well distributed (average z value should be close to 0), average_z = {avg_z}");
    }


}
