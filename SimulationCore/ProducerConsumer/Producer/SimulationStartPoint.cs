using System.Threading.Tasks.Dataflow;

namespace SahptSimulation.ProducerConsumer.Producer;


public abstract class SimulationStartPoint<T> : ISimulationProducer<T>
{
    public int MaxToProduce { get; }

    public SimulationStartPoint(TimeSpan timeToProduce, int queueSize, int maxToProduce = 10)
    {
        MaxToProduce = maxToProduce;
        ProduceQueue = new BufferBlock<T>(new DataflowBlockOptions
        {
            BoundedCapacity = queueSize
        });
        TimeToProduce = timeToProduce;
    }

    public BufferBlock<T> ProduceQueue { get; set; }
    public TimeSpan TimeToProduce { get; }
    public abstract void Produce();
}