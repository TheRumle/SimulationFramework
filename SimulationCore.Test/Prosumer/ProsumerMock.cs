using System.Threading.Tasks.Dataflow;
using NSubstitute;
using SahptSimulation.ProducerConsumer;

namespace SimulationTest.Prosumer;

public static class ProsumerMock<T>
{
    public static Prosumer<T> Create()
    {
        TimeSpan consumeTime  = TimeSpan.FromTicks(1);
        TimeSpan produceTime  = TimeSpan.FromTicks(1);
        BufferBlock<int>? consumeQueue = new BufferBlock<int>();
        BufferBlock<int>? produceQueue = new BufferBlock<int>();

        return Substitute.For<Prosumer<T>>(consumeTime, produceTime, consumeQueue, produceQueue);
    }
}