using System.Threading.Tasks.Dataflow;
using SahptSimulation.ProducerConsumer;

namespace SimulationTest.Prosumer;

public abstract class ProsumerTests<T>
{
    protected abstract Prosumer<T> CreateProsumer(
        TimeSpan consumeTime, TimeSpan produceTime,
        BufferBlock<T> consumeQueue, BufferBlock<T> produceQueue);

    protected TimeSpan ConsumeTime = TimeSpan.FromTicks(2);
    protected TimeSpan ProduceTime = TimeSpan.FromTicks(2);
    protected abstract BufferBlock<T> CreateProduceQueue();
    protected abstract BufferBlock<T> CreateConsumeQueue();

    protected Prosumer<T> Prosumer = null!;
    protected BufferBlock<T> ProduceQueue = BufferFactory.Create<T>(10);
    protected BufferBlock<T> ConsumeQueue = BufferFactory.Create<T>(10);


    [SetUp]
    public void Setup()
    {
        Prosumer = CreateProsumer(
            ConsumeTime, ProduceTime,
            ConsumeQueue, ProduceQueue);
    }

    [Test]
    public void SetsCorrectConsumeQueueOnCreation()
        => Assert.That(Prosumer.ConsumeQueue, Is.EqualTo(ConsumeQueue));


    [Test]
    public void SetsCorrectProduceQueueOnCreation()
        => Assert.That(Prosumer.ProduceQueue, Is.EqualTo(ProduceQueue));

    [Test]
    public void SetsCorrectConsumeTimeOnCreation()
        => Assert.That(Prosumer.TimeToConsume, Is.EqualTo(ConsumeTime));

    [Test]
    public void SetsCorrectProduceTimeOnCreation()
        => Assert.That(Prosumer.TimeToProduce, Is.EqualTo(ProduceTime));
    
    
}
