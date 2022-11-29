using System.Threading.Tasks.Dataflow;
using SahptSimulation.ProducerConsumer.Consumer;

namespace SahptSimulation.ProducerConsumer.Simulators;

public class EndStep<T>
{
    private readonly List<ISimulationConsumer<T>> _consumers;
    protected readonly BufferBlock<T> CommonQueue;

    protected EndStep(List<ISimulationConsumer<T>> consumers, BufferBlock<T> commonQueue)
    {
        _consumers = consumers;
        CommonQueue = commonQueue;
    }

    protected EndStep(ISimulationConsumer<T> consumer, BufferBlock<T> commonQueue)
    {
        _consumers = new List<ISimulationConsumer<T>> { consumer };
        CommonQueue = commonQueue;
    }

    public IEnumerable<ISimulationConsumer<T>> Consumers => _consumers;

    public void AddConsumer(ISimulationConsumer<T> consumer)
    {
        _consumers.Add(consumer);
    }
}