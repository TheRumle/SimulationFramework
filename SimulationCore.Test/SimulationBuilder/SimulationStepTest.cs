using System.Threading.Tasks.Dataflow;
using NSubstitute;
using SahptSimulation.ProducerConsumer;
using SahptSimulation.ProducerConsumer.Consumer;
using SahptSimulation.ProducerConsumer.Producer;
using SahptSimulation.ProducerConsumer.SimulationBuilder;
using SimulationTest.Prosumer;

namespace SimulationTest.SimulationBuilder;

public class SimulationConfigurationStep
{
    private SimulationStartPoint<int> _startPointProducer = null!;
    private SimulationConfigurationStep<int> _simulationBuilder = null!;
    private List<Prosumer<int>> _consumerList = null!;

    [SetUp]
    public void Setup()
    {
        _consumerList = new List<Prosumer<int>>();
        _startPointProducer = Substitute.For<SimulationStartPoint<int>>(TimeSpan.FromMinutes(1), 10, 10);
        _simulationBuilder = Simulation.StartConfiguration(_startPointProducer, _consumerList);
    }

    [Test]
    public void OnOneStartProducerGivesOneStartProducerInStep() =>
        Assert.That(_simulationBuilder.Producers, Has.Count.EqualTo(1));

    [Test]
    public void AssignsStartProducerCorrectly() =>
        Assert.That(_startPointProducer, Is.EqualTo(_simulationBuilder.Producers.First()));

    [Test]
    public void AddsConsumerCorrectly()
    {
        ISimulationConsumer<int> consumer = CreateConsumer();

        _simulationBuilder.AddConsumer(consumer);

        Assert.That(_simulationBuilder.Consumers.First(), Is.EqualTo(consumer));
    }

    [Test]
    public void AddsProducerCorrectly()
    {
        ISimulationProducer<int> producer = CreateProducer();
        _simulationBuilder.AddProducer(producer);
        Assert.That(_simulationBuilder.Producers.Skip(1).First(), Is.EqualTo(producer));
    }

    [Test]
    public void AddsPresumerToConsumerListCorrectly()
    {
        Prosumer<int> consumer = ProsumerMock<int>.Create();
        _simulationBuilder.AddConsumer(consumer);
        Assert.That(_simulationBuilder.Consumers.First(), Is.EqualTo(consumer));
    }

    [Test]
    public void CanConvertToEndStep() => _simulationBuilder.EndConfiguration();


    [Test]
    public void ProducersBecomesConsumersOnConverge()
    {
        Prosumer<int> consumer = ProsumerMock<int>.Create();

        SimulationConfigurationStep<int> newStep = _simulationBuilder.AddConvergentPoint(consumer);

        Assert.That(_consumerList, Is.EqualTo(newStep.Producers));
    }

    [Test]
    public void NewBufferBecomesProduceQueueOnConverge()
    {
        Prosumer<int> consumer = ProsumerMock<int>.Create();
        BufferBlock<int> buffer = new BufferBlock<int>();

        SimulationConfigurationStep<int> newStep = _simulationBuilder.AddConvergentPoint(consumer, buffer);

        Assert.That(newStep.CommonQueue, Is.EqualTo(buffer));
    }
    
    [Test]
    public void NewBufferBecomesProduceQueueOnConvergeWhenGivenMAxCapacity()
    {
        Prosumer<int> consumer = ProsumerMock<int>.Create();

        SimulationConfigurationStep<int> newStep = _simulationBuilder.AddConvergentPoint(consumer, 10);

        Assert.That(newStep.CommonQueue, Is.Not.EqualTo(_simulationBuilder.CommonQueue));
    }

    [Test]
    public void CorrectlyFiltersAwayNonProsumersForConvergence()
    {
        ISimulationConsumer<int> consumer = CreateConsumer();
        Prosumer<int> prosumer = ProsumerMock<int>.Create();

        SimulationConfigurationStep<int>
            newStep = _simulationBuilder.AddConsumer(consumer).AddConvergentPoint(prosumer);

        Assert.That(newStep.Producers, Has.No.Member(consumer));
    }

    private ISimulationConsumer<int> CreateConsumer() => Substitute.For<ISimulationConsumer<int>>();
    private ISimulationProducer<int> CreateProducer() => Substitute.For<ISimulationProducer<int>>();
}