// See https://aka.ms/new-console-template for more information


using SahptSimulation.ProducerConsumer;
using SahptSimulation.ProducerConsumer.Producer;
using SahptSimulation.ProducerConsumer.Simulators;

var startPoint = new BoxInput<int>(new TimeSpan(5), 50, 13);
var moveRack = new Prosumer<int>(startPoint.ProduceQueue, new); 


SimulationStartPoint<int>
    .CreateSimulationPipeLine(startPoint)
    .AddProducer().AddProducer()
    .AddConsumer().AddConsumer()
    .ConfigNewSimulationStep(moveRack)
    .ConfigNewSimulationStep()
    .