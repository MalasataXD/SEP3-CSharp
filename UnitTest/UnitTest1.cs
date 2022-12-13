using Application.Logic;

namespace UnitTest;

public class Tests
{
    
    private WorkerLogic workerLogic = new WorkerLogic(null);

    private PrivateObject
    [SetUp]
    public void Setup()
    {
        WorkerLogic target = new WorkerLogic(null);
        PrivateType obj = new PrivateObject(target);
        var retVal = obj.Invoke("PrivateMethod");
        Assert.AreEqual(expectedVal, retVal);
    }

    [Test]
    public void Test1()
    {
        Assert.Pass(workerLogic.);
    }
}