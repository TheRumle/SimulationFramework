using System.Threading.Tasks.Dataflow;

namespace SahptSimulation.ProducerConsumer.Producer;

public interface ISimulationProducer<T>
{
    public BufferBlock<T> ProduceQueue { get; set; }
    public TimeSpan TimeToProduce { get; }
    public void Produce();

}