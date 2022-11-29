using System.Threading.Tasks.Dataflow;
using SahptSimulation.ProducerConsumer.Consumer;
using SahptSimulation.ProducerConsumer.Producer;

namespace SahptSimulation.ProducerConsumer.Simulators;

public class EndStep<T> 
{
    
}

public class SimulationStep<T>
{
    protected readonly List<Prosumer<T>> OldConsumers;
    protected readonly List<ISimulationProducer<T>> Producers;
    protected readonly BufferBlock<T> CommonQueue;

    private SimulationStep(List<ISimulationProducer<T>> sources, Prosumer<T> newConsumer, BufferBlock<T> sharedQueue)
    {
        CommonQueue = sharedQueue;
        Producers = sources;
        OldConsumers = new List<Prosumer<T>>()
        {
            newConsumer
        };

        SetupSharedQueue();
    }

    private SimulationStep(List<ISimulationProducer<T>> sources, List<Prosumer<T>> newConsumers,
        BufferBlock<T> sharedQueue)
    {
        CommonQueue = sharedQueue;
        Producers = sources;
        OldConsumers = newConsumers;
        SetupSharedQueue();
    }

    private void SetupSharedQueue()
    {
        foreach (var simulationProducer in Producers) simulationProducer.ProduceQueue = CommonQueue;
        foreach (var simulationProducer in OldConsumers) simulationProducer.ConsumeQueue = CommonQueue;
    }


    public SimulationStep<T> ConfigNewSimulationStep<TProsumer>(BufferBlock<T> newProduceQueue)
        where TProsumer : Prosumer<T>, new()
    {
        var prosumer = new TProsumer();
        prosumer.ConsumeQueue = CommonQueue;
        prosumer.ProduceQueue = newProduceQueue;
        return new SimulationStep<T>(Producers, prosumer, newProduceQueue);
    }
    
    public SimulationStep<T> ConfigNewSimulationStep(Prosumer<T> prosumer, BufferBlock<T> sharedQueue)
    {
        /*
         * The old producers become the new sources, the prosumer becomes the new consumer using the shared queue.
         */
        return new SimulationStep<T>(Producers, prosumer, sharedQueue);
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
        OldConsumers.Add(consumer);
        return this;
    }

    public SimulationStep<T> AddProducer(ISimulationProducer<T> producer)
    {
        Producers.Add(producer);
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