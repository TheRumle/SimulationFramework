using System.Threading.Tasks.Dataflow;

namespace SahptSimulation.ProducerConsumer.Consumer;

public class SimulationConsumer<T> : ISimulationConsumer<T>
{
    public SimulationConsumer(BufferBlock<T> consumeQueue, TimeSpan timeToConsume)
    {
        ConsumeQueue = consumeQueue;
        TimeToConsume = timeToConsume;
    }

    public BufferBlock<T> ConsumeQueue { get; set; }
    public TimeSpan TimeToConsume { get; }

    public virtual async Task Consume()
    {
        while (await ConsumeQueue.OutputAvailableAsync())
        {
            T result = await ConsumeQueue.ReceiveAsync();
            Thread.Sleep(TimeToConsume);
            Console.WriteLine(result + "  reached endpoint");
        }
    }
}