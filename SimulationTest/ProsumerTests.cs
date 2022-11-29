using System.Threading.Tasks.Dataflow;
using SahptSimulation.ProducerConsumer;
using SimulationTest.Utility;

namespace SimulationTest;

public class ProsumerTests
{

    class TestProsumer : Prosumer<int>
    {
        
        public TestProsumer(TimeSpan timeToConsume, TimeSpan timeToProduce, BufferBlock<int> consumeQueue, BufferBlock<int> produceQueue) : base(timeToConsume, timeToProduce, consumeQueue, produceQueue)
        {
        }

        public override Task Consume()  => 
            throw new ShouldNotBeTestedException();

        public override void Produce() => 
            throw new ShouldNotBeTestedException();
        
    }
    
    private static TimeSpan _produceTime = TimeSpan.FromTicks(2);
    private static TimeSpan _consumeTime = TimeSpan.FromTicks(5);
    private static BufferBlock<int> CreateQueue() => new(new DataflowBlockOptions
    {
        BoundedCapacity = 10
    });

    private BufferBlock<int> _produceQueue = null!;
    private BufferBlock<int> _consumeQueue = null!;

    private Prosumer<int> _prosumer = null!;

    [SetUp]
    public void Setup()
    {
        _produceQueue = CreateQueue();
        _consumeQueue = CreateQueue();
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