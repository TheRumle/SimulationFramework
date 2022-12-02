using System.Threading.Tasks.Dataflow;
using SahptSimulation.ProducerConsumer.Consumer;

namespace SahptSimulation.ProducerConsumer.SimulationBuilder;

public class EndStep<T>
{
    protected readonly List<ISimulationConsumer<T>> InternalConsumers;
    public IReadOnlyList<ISimulationConsumer<T>> Consumers => InternalConsumers;

    public readonly BufferBlock<T> CommonQueue;

    protected EndStep(List<ISimulationConsumer<T>> internalConsumers, BufferBlock<T> commonQueue)
    {
        InternalConsumers = internalConsumers;
        CommonQueue = commonQueue;
    }

    protected EndStep(ISimulationConsumer<T> consumer, BufferBlock<T> commonQueue)
    {
        InternalConsumers = new List<ISimulationConsumer<T>> { consumer };
        CommonQueue = commonQueue;
    }
}