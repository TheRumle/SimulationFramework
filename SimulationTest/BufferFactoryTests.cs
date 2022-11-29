using System.Threading.Tasks.Dataflow;
using SahptSimulation.ProducerConsumer;

namespace SimulationTest;

public class BufferFactoryTests
{
    [SetUp]
    public void Setup()
    {
    }

    [TestCase(1)]
    [TestCase(10)]
    [TestCase(50)]
    [TestCase(short.MaxValue)]
    public void SetsCapacityOnCreation(int value)
    {
        BufferBlock<int> queue = BufferFactory.Create<int>(value);
        var ints = new List<int>();
        for (int i = 0; i < value; i++)
        {
            queue.SendAsync(1).Wait();
            ints.Add(queue.Receive());
        }

        Assert.That(value, Is.EqualTo(ints.Count));
    }

    [TestCase(0)]
    [TestCase(-1)]
    public void ThrowsOnNegativeValue(int value)
    {
        Assert.Throws<Exception>(() => BufferFactory.Create<int>(value));
    }
}