using System.Threading.Tasks.Dataflow;
using SahptSimulation.ProducerConsumer.Consumer;
using SahptSimulation.ProducerConsumer.Producer;

namespace SahptSimulation.ProducerConsumer;

public abstract class Prosumer<T> : ISimulationConsumer<T>, ISimulationProducer<T>
{
    public Prosumer(TimeSpan timeToConsume,
        TimeSpan timeToProduce, BufferBlock<T> consumeQueue, BufferBlock<T> produceQueue)
    {
        TimeToProduce = timeToProduce;
        ConsumeQueue = consumeQueue;
        ProduceQueue = produceQueue;
        TimeToConsume = timeToConsume;
    }

    public BufferBlock<T> ConsumeQueue { get; set; }

    public TimeSpan TimeToConsume { get; set; }

    public Task Consume()
    {
        return Task.Run(() => Thread.Sleep(TimeToConsume));
    }

    public BufferBlock<T> ProduceQueue { get; set; }
    public TimeSpan TimeToProduce { get; set; }
    
}