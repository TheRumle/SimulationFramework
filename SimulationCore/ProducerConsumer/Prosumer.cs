using System.Threading.Tasks.Dataflow;
using SahptSimulation.ProducerConsumer.Consumer;
using SahptSimulation.ProducerConsumer.Producer;

namespace SahptSimulation.ProducerConsumer;


public abstract class Prosumer<T> : ISimulationConsumer<T>, ISimulationProducer<T>
{
    public TimeSpan TimeToConsume { get; }
    public abstract Task Consume();

    public TimeSpan TimeToProduce { get; }
    public abstract void Produce();  
    
    protected Prosumer(TimeSpan timeToConsume,
        TimeSpan timeToProduce, BufferBlock<T> consumeQueue, BufferBlock<T> produceQueue)
    {
        TimeToProduce = timeToProduce;
        TimeToConsume = timeToConsume;
        ConsumeQueue = consumeQueue;
        ProduceQueue = produceQueue;
    }

    public BufferBlock<T> ConsumeQueue { get; set; }
    public BufferBlock<T> ProduceQueue { get; set; }
}