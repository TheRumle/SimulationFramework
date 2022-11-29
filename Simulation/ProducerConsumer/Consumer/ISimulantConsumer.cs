using System.Threading.Tasks.Dataflow;

namespace SahptSimulation.ProducerConsumer.Consumer;

public interface ISimulationConsumer<T>
{
    public BufferBlock<T> ConsumeQueue { get; set; }
    public TimeSpan TimeToConsume { get; }
    public Task Consume();
}