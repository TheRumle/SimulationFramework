using System.Threading.Tasks.Dataflow;
using SahptSimulation.ProducerConsumer.Consumer;
using SahptSimulation.ProducerConsumer.Producer;

namespace SahptSimulation.ProducerConsumer.SimulationBuilder;

public class SimulationConfigurationStep<T> : EndStep<T>
{
    private readonly List<ISimulationProducer<T>> _producers;

    internal SimulationConfigurationStep(List<ISimulationProducer<T>> sources, Prosumer<T> newConsumer,
        BufferBlock<T> sharedQueue)
        : base(newConsumer, sharedQueue)
    {
        _producers = sources;
        InternalConsumers.Add(newConsumer);
        SetupSharedQueue();
    }

    internal SimulationConfigurationStep(List<ISimulationProducer<T>> sources, List<Prosumer<T>> newConsumers,
        BufferBlock<T> sharedQueue)
        : base(newConsumers.ToList<ISimulationConsumer<T>>(), sharedQueue)
    {
        _producers = sources;
        SetupSharedQueue();
    }


    private void SetupSharedQueue()
    {
        foreach (ISimulationProducer<T> simulationProducer in _producers) simulationProducer.ProduceQueue = CommonQueue;
        foreach (ISimulationConsumer<T> simulationProducer in InternalConsumers)
            simulationProducer.ConsumeQueue = CommonQueue;
    }


    public SimulationConfigurationStep<T> AddConvergentPoint<TProsumer>(BufferBlock<T> newProduceQueue)
        where TProsumer : Prosumer<T>, new()
    {
        TProsumer prosumer = new TProsumer()
        {
            ConsumeQueue = CommonQueue,
            ProduceQueue = newProduceQueue
        };

        return new SimulationConfigurationStep<T>(_producers, prosumer, newProduceQueue);
    }

    public SimulationConfigurationStep<T> AddConvergentPoint(Prosumer<T> newConsumer, BufferBlock<T> sharedQueue)
    {
        /*
         * The old producers become the new sources, the prosumer becomes the new consumer using the shared queue.
         */

        List<ISimulationProducer<T>> previousConsumers = InternalConsumers
            .Where(consumer => consumer is Prosumer<T>)
            .Map(consumer => (Prosumer<T>) consumer)
            .ToList<ISimulationProducer<T>>();

        return new SimulationConfigurationStep<T>(previousConsumers, newConsumer, sharedQueue);
    }

    public SimulationConfigurationStep<T> AddConvergentPoint(Prosumer<T> newConsumer)
    {
        return AddConvergentPoint(newConsumer, new BufferBlock<T>());
    }

    public SimulationConfigurationStep<T> AddConvergentPoint(Prosumer<T> newConsumer, int capacity)
    {
        return AddConvergentPoint(newConsumer, BufferFactory.Create<T>(capacity));
    }

    public IReadOnlyCollection<ISimulationProducer<T>> Producers => _producers;
    public SimulationConfigurationStep<T> AddConsumer(params ISimulationConsumer<T>[] consumers)
    {
        InternalConsumers.AddRange(consumers);
        return this;
    }

    public SimulationConfigurationStep<T> AddProducer(params ISimulationProducer<T>[] producers)
    {
        _producers.AddRange(producers);
        return this;
    }

    public EndStep<T> EndConfiguration()
    {
        return this;
    }
}