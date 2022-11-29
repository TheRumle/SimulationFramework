namespace SahptSimulation.ProducerConsumer.Producer;

public class BoxInputStartPoint : SimulationStartPoint<int>
{
    public BoxInputStartPoint(TimeSpan timeToProduce, int queueSize, int maxToProduct = 10) : base(timeToProduce,
        queueSize, maxToProduct)
    {
    }

    protected override int Create()
    {
        return 10;
    }
}