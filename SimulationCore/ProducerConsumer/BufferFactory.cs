using System.Threading.Tasks.Dataflow;

namespace SahptSimulation.ProducerConsumer;

public class BufferFactory
{
    public static BufferBlock<T> Create<T>(int capacity)
    {
        if (capacity <= 0) throw new ArgumentOutOfRangeException($"Bufferblock cannot have capacity of {capacity}.");
        return new BufferBlock<T>(new DataflowBlockOptions
        {
            BoundedCapacity = capacity
        });
    }
}