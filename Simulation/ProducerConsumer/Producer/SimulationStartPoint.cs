using System.Threading.Tasks.Dataflow;
using SahptSimulation.ProducerConsumer.SimulationBuilder;

namespace SahptSimulation.ProducerConsumer.Producer;

public abstract class SimulationStartPoint<T> : ISimulationProducer<T>
{
    private readonly int _maxToProduct;

    public SimulationStartPoint(TimeSpan timeToProduce, int queueSize, int maxToProduct = 10)
    {
        _maxToProduct = maxToProduct;
        ProduceQueue = new BufferBlock<T>(new DataflowBlockOptions
        {
            BoundedCapacity = queueSize
        });
        TimeToProduce = timeToProduce;
    }

    public BufferBlock<T> ProduceQueue { get; set; }
    public TimeSpan TimeToProduce { get; }


    public void Produce()
    {
        for (int a = 0; a <= _maxToProduct; a++)
        {
            ProduceQueue.SendAsync(Create());
            Thread.Sleep(TimeToProduce);
        }
    }

    protected abstract T Create();

    public static SimulationStep<T> CreateSimulationPipeLine(SimulationStartPoint<T> startPoint)
    {
        return SimulationStep<T>.StartConfigurationBuild(startPoint, new List<Prosumer<T>>());
    }
}