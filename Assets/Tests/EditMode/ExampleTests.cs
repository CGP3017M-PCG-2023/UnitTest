using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ExampleTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void ExampleTestSimplePasses()
    {
		// Use the Assert class to test conditions
		Assert.Pass("Testing");
    }

    [Test]
    public void ExampleTestSimpleFails()
    {
        // Use the Assert class to test conditions
        Assert.Fail("Testing");
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator ExampleTestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
