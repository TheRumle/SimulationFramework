using SahptSimulation.ProducerConsumer.Producer;

namespace SahptSimulation.ProducerConsumer.Simulators;

public class BoxInput<T> : SimulationStartPoint<T> where T : new()
{
    public BoxInput(TimeSpan timeToProduce, int queueSize, int maxToProduct = 10) : base(timeToProduce, queueSize, maxToProduct)
    {
    }

    protected override T Create() => new T();
}