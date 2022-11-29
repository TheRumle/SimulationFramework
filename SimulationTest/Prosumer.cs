using System.Threading.Tasks.Dataflow;
using SahptSimulation.ProducerConsumer;

namespace SimulationTest;

public class ProsumerTests
{
    private BufferBlock<int> _sharedQueue = new(new DataflowBlockOptions
    {
        BoundedCapacity = 10
    });

    private Prosumer<int> Prosumer { get; set; }

    [SetUp]
    public void Setup()
    {
    }
}