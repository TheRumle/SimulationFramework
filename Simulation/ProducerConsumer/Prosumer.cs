using System.Threading.Tasks.Dataflow;
using SahptSimulation.ProducerConsumer.Consumer;
using SahptSimulation.ProducerConsumer.Producer;

namespace SahptSimulation.ProducerConsumer;

public class Prosumer<T> : ISimulationConsumer<T>, ISimulationProducer<T>
{

    public Prosumer( TimeSpan timeToConsume, TimeSpan timeToProduce, BufferBlock<T> consumeQueue, BufferBlock<T> produceQueue)
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

        Thread.Sleep(TimeToProduce);
        return null;
    }

    public BufferBlock<T> ProduceQueue { get; set; }
    public TimeSpan TimeToProduce { get; set; }
    public void Produce()
    {
        throw new NotImplementedException();
    }
    
    public static Prosumer<T> Create( TimeSpan timeToConsume, TimeSpan timeToProduce, BufferBlock<T> consumeQueue, BufferBlock<T> produceQueue)
    {
        return new Prosumer<T>(timeToConsume, timeToProduce, consumeQueue, produceQueue);
    }   
    
    public Prosumer<TQueue> Create<TProsumer, TQueue>(
        BufferBlock<TQueue> consumeQueue,
        TimeSpan timeToConsume,
        TimeSpan timeToProduce,
        int maxCapacity)
        where TProsumer : Prosumer<TQueue>, ISimulationConsumer<TQueue>, ISimulationProducer<TQueue>, new()
    {
        return new TProsumer()
        {
            ConsumeQueue = consumeQueue,
            TimeToConsume = timeToConsume,
            TimeToProduce = timeToProduce,
            ProduceQueue = BufferFactory.Create<TQueue>(maxCapacity)
        };
    }
    
}