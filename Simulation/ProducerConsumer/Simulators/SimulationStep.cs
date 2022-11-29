using System.Threading.Tasks.Dataflow;
using SahptSimulation.ProducerConsumer.Consumer;
using SahptSimulation.ProducerConsumer.Producer;

namespace SahptSimulation.ProducerConsumer.Simulators;

public class SimulationStep<T>
{
    private readonly List<Prosumer<T>> _oldConsumers;
    private readonly List<ISimulationProducer<T>> _producers;
    private readonly BufferBlock<T> _commonQueue;

    private SimulationStep(List<ISimulationProducer<T>> sources, Prosumer<T> newConsumer, BufferBlock<T> sharedQueue)
    {
        _commonQueue = sharedQueue;
        _producers = sources;
        _oldConsumers = new List<Prosumer<T>>()
        {
            newConsumer
        };

        SetupSharedQueue();
    }

    private SimulationStep(List<ISimulationProducer<T>> sources, List<Prosumer<T>> newConsumers,
        BufferBlock<T> sharedQueue)
    {
        _commonQueue = sharedQueue;
        _producers = sources;
        _oldConsumers = newConsumers;
        SetupSharedQueue();
    }

    private void SetupSharedQueue()
    {
        foreach (var simulationProducer in _producers) simulationProducer.ProduceQueue = _commonQueue;
        foreach (var simulationProducer in _oldConsumers) simulationProducer.ConsumeQueue = _commonQueue;
    }


    public SimulationStep<T> ConfigNewSimulationStep<TProsumer>(BufferBlock<T> newProduceQueue)
        where TProsumer : Prosumer<T>, new()
    {
        var prosumer = new TProsumer();
        prosumer.ConsumeQueue = _commonQueue;
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

    public SimulationStep<T> ConfigNewSimulationStep(Prosumer<T> prosumer) =>
        ConfigNewSimulationStep(prosumer, new BufferBlock<T>());

    public SimulationStep<T> ConfigNewSimulationStep(Prosumer<T> prosumer, int capacity)
        => ConfigNewSimulationStep(prosumer, new BufferBlock<T>(new DataflowBlockOptions()
            {
                BoundedCapacity = capacity
            }
        ));
    
    public SimulationStep<T> AddConsumer(Prosumer<T> consumer)
    {
        _oldConsumers.Add(consumer);
        return this;
    }

    public SimulationStep<T> AddProducer(ISimulationProducer<T> producer)
    {
        _producers.Add(producer);
        return this;
    }

    public static SimulationStep<T> StartConfigurationBuild<T>(
        SimulationStartPoint<T> startPoint,
        List<Prosumer<T>> consumeProduces)
    {
        var producerList = new List<ISimulationProducer<T>>()
        {
            startPoint
        };

        return new SimulationStep<T>(producerList, consumeProduces, startPoint.ProduceQueue);
    }
}