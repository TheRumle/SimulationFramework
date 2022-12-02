using NSubstitute;
using SahptSimulation.ProducerConsumer;
using SahptSimulation.ProducerConsumer.Producer;
using SahptSimulation.ProducerConsumer.SimulationBuilder;

namespace SimulationTest.SimulationBuilder;

public class SimulationTest
{
    [Test]
    public void CanBuild()
    {
        SimulationStartPoint<int>? startPointProducer = Substitute.For<SimulationStartPoint<int>>(TimeSpan.FromMinutes(1), 10, 10);
        
        Assert.DoesNotThrow(() =>  Simulation.StartConfiguration(startPointProducer, new List<Prosumer<int>>()));
    }
}