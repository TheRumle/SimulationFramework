using System.Threading.Tasks.Dataflow;

namespace SahptSimulation.ProducerConsumer;

public class BufferFactory
{
    public static BufferBlock<T> Create<T>(int capacity)
    {
        return new BufferBlock<T>(new DataflowBlockOptions
        {
            BoundedCapacity = 10
        });
    }
}