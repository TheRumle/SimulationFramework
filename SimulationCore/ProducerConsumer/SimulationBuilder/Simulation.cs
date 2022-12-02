using SahptSimulation.ProducerConsumer.Producer;

namespace SahptSimulation.ProducerConsumer.SimulationBuilder;

public static class Simulation
{
    public static SimulationConfigurationStep<T> StartConfiguration<T>(
        SimulationStartPoint<T> startPoint,
        List<Prosumer<T>> consumeProduces)
    {
        List<ISimulationProducer<T>> producerList = new List<ISimulationProducer<T>>
        {
            startPoint
        };

        return new SimulationConfigurationStep<T>(producerList, consumeProduces, startPoint.ProduceQueue);
    }
    
    public static SimulationConfigurationStep<T> StartConfiguration<T>(
        SimulationStartPoint<T> startPoint,
        Prosumer<T> consumeProduces)
    {
        var producerList = new List<ISimulationProducer<T>>
        {
            startPoint
        };

        return new SimulationConfigurationStep<T>(producerList, consumeProduces, startPoint.ProduceQueue);
    }
}