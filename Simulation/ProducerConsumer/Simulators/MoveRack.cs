using System.Threading.Tasks.Dataflow;

namespace SahptSimulation.ProducerConsumer.Simulators;

public class MoveRack<T> : Prosumer<T>
{
    public MoveRack(TimeSpan timeToConsume, TimeSpan timeToProduce, BufferBlock<T> consumeQueue, BufferBlock<T> produceQueue)
        : base(timeToConsume, timeToProduce, consumeQueue, produceQueue)
    {
    }

}