using System.Threading.Tasks.Dataflow;
using SahptSimulation.ProducerConsumer.Consumer;
using SahptSimulation.ProducerConsumer.Producer;
using SahptSimulation.ProducerConsumer.Simulators;

namespace SahptSimulation.ProducerConsumer.SimulationBuilder;

public class SimulationStep<T> : EndStep<T>
{
    private readonly List<Prosumer<T>> _consumers;
    private readonly List<ISimulationProducer<T>> _producers;

    private SimulationStep(List<ISimulationProducer<T>> sources, Prosumer<T> newConsumer, BufferBlock<T> sharedQueue)
        : base(newConsumer, sharedQueue)
    {
        _producers = sources;
        _consumers = new List<Prosumer<T>>
        {
            newConsumer
        };

        SetupSharedQueue();
    }

    private SimulationStep(List<ISimulationProducer<T>> sources, List<Prosumer<T>> newConsumers,
        BufferBlock<T> sharedQueue)
        : base(newConsumers.ToList<ISimulationConsumer<T>>(), sharedQueue)
    {
        _producers = sources;
        _consumers = newConsumers;
        SetupSharedQueue();
    }

    private void SetupSharedQueue()
    {
        foreach (ISimulationProducer<T> simulationProducer in _producers) simulationProducer.ProduceQueue = CommonQueue;
        foreach (Prosumer<T> simulationProducer in _consumers) simulationProducer.ConsumeQueue = CommonQueue;
    }


    public SimulationStep<T> ConfigNewSimulationStep<TProsumer>(BufferBlock<T> newProduceQueue)
        where TProsumer : Prosumer<T>, new()
    {
        var prosumer = new TProsumer();
        prosumer.ConsumeQueue = CommonQueue;
        prosumer.ProduceQueue = newProduceQueue;
        return new SimulationStep<T>(_producers, prosumer, newProduceQueue);
    }

    public SimulationStep<T> ConfigNewSimulationStep(Prosumer<T> prosumer, BufferBlock<T> sharedQueue)
    {
        /*
         * The old producers become the new sources, the prosumer becomes the new consumer using the shared queue.
         */
        return new SimulationStep<T>(_producers, prosumer, sharedQueue);
    }

    public SimulationStep<T> ConfigNewSimulationStep(Prosumer<T> prosumer)
    {
        return ConfigNewSimulationStep(prosumer, new BufferBlock<T>());
    }

    public SimulationStep<T> ConfigNewSimulationStep(Prosumer<T> prosumer, int capacity)
    {
        return ConfigNewSimulationStep(prosumer, new BufferBlock<T>(new DataflowBlockOptions
            {
                BoundedCapacity = capacity
            }
        ));
    }

    public SimulationStep<T> AddConsumer(Prosumer<T> consumer)
    {
        _consumers.Add(consumer);
        return this;
    }

    public SimulationStep<T> AddProducer(ISimulationProducer<T> producer)
    {
        _producers.Add(producer);
        return this;
    }

    public static SimulationStep<T> StartConfigurationBuild(
        SimulationStartPoint<T> startPoint,
        List<Prosumer<T>> consumeProduces)
    {
        var producerList = new List<ISimulationProducer<T>>
        {
            startPoint
        };

        return new SimulationStep<T>(producerList, consumeProduces, startPoint.ProduceQueue);
    }

    public EndStep<T> ToEndConfiguration()
    {
        return this;
    }
}