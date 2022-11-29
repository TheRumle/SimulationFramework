using System.Threading.Tasks.Dataflow;
using SahptSimulation.ProducerConsumer;

namespace SimulationTest;

public class ProsumerTests
{

    class TestProsumer : Prosumer<int>
    {
        public TestProsumer(TimeSpan timeToConsume, TimeSpan timeToProduce, BufferBlock<int> consumeQueue, BufferBlock<int> produceQueue) : base(timeToConsume, timeToProduce, consumeQueue, produceQueue)
        {
        }

    }
    
    private static TimeSpan _produceTime = TimeSpan.FromTicks(2);
    private static TimeSpan _consumeTime = TimeSpan.FromTicks(5);
    private static BufferBlock<int> CreateQueue() => new(new DataflowBlockOptions
    {
        BoundedCapacity = 10
    });

    private BufferBlock<int> _produceQueue = CreateQueue();
    private BufferBlock<int> _consumeQueue = CreateQueue();

    private Prosumer<int> _prosumer = null!;

    [SetUp]
    public void Setup()
    {
        _prosumer = new TestProsumer(
            _consumeTime, _produceTime,
            _consumeQueue, _produceQueue);
    }

    [Test]
    public void SetsCorrectConsumeQueueOnCorrection() => Assert.That(_prosumer.ConsumeQueue, Is.EqualTo(_consumeQueue));
    [Test]
    public void SetsCorrectProduceQueueOnCorrection() => Assert.That(_prosumer.ProduceQueue, Is.EqualTo(_produceQueue));
    [Test]
    public void SetsCorrectConsumeTimeOnCorrection() => Assert.That(_prosumer.TimeToConsume, Is.EqualTo(_consumeTime));
    [Test]
    public void SetsCorrectProduceTimeOnCorrection() => Assert.That(_prosumer.TimeToProduce, Is.EqualTo(_produceTime));
    

}