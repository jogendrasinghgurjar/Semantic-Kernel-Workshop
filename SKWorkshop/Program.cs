using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Orchestration;
using SKWorkshop.Plugin;
using System.Data;
System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);


// 1... instantiating  kernel
var builder = new KernelBuilder();

builder.WithAzureTextCompletionService(
            "",                // Model Deployment Name 
            "",  // Azure OpenAI Endpoint
            "");      // Azure OpenAI Key

var kernel = builder.Build();

// 2... Semantic plugin- a. Get user's intent

var pluginsDirectory = Path.Combine("C:\\Users\\pramishra\\Desktop\\MLTrainingMaterial\\Semantic-Kernel-Workshop\\SKWorkshop\\Plugin\\");

// Import the OrchestratorPlugin from the plugins directory.
var orchestratorPlugin = kernel.ImportSemanticSkillFromDirectory(pluginsDirectory, "OrchestrationPlugin");

// Get the GetIntent function from the OrchestratorPlugin and run it
var intent = await orchestratorPlugin["GetIntent"]
     .InvokeAsync("I want to park my vehicle can you help me find an available slot?");

Console.WriteLine(intent);

// Intent with defined options

var context = kernel.CreateNewContext();
context["options"] = "FetchAvailableParkingSlot, FindMyParkedVehicle, OutOfContext";
context["input"] = "I want to park my vehicle can you help me find an available slot?";

intent = await orchestratorPlugin["GetIntent"]
     .InvokeAsync(context);

Console.WriteLine(intent);

// b. Get vehicle type

// Import the VehiclePlugin from the plugins directory.
var vehiclePlugin = kernel.ImportSemanticSkillFromDirectory(pluginsDirectory, "VehiclePlugin");

// Get the GetVehicleType function and run it
var vehicleType = await vehiclePlugin["GetVehicleType"].InvokeAsync("Hi, Please tell me available parking slots for Bike");

Console.WriteLine(vehicleType);

// 3... Native function

// Import the VehicleParking native function
var vehicleParkingFunction = kernel.ImportSkill(new VehicleParking(), "VehicleParking");

// Create context variable to pass the native code parameters
var vehicleContext = kernel.CreateNewContext();
vehicleContext["vehicleType"] = "Car";
vehicleContext["fuelType"] = "Electric";
vehicleContext["userName"] = "Ananya";

// Make a request that runs the FetchAvailableParkingSlot function
var availableSlots = await vehicleParkingFunction["FetchAvailableParkingSlot"].InvokeAsync(vehicleContext);
Console.WriteLine(availableSlots);

// 4. Function chaining

/*// Import Semantic Plugins
var orchestrationPlugin = kernel.ImportSemanticSkillFromDirectory(pluginsDirectory, "OrchestrationPlugin");
var vehiclePlugin = kernel.ImportSemanticSkillFromDirectory(pluginsDirectory, "VehiclePlugin");

// Import the native function
var vehicleParkingFunction = kernel.ImportSkill(new VehicleParking(), "VehicleParking");
var orchestrationFunction = kernel.ImportSkill(new OrchestrationPlugin(kernel), "OrchestrationPlugin");

// Make a request that runs the FetchAvailableParkingSlot function
var availableParkingSlots = await orchestrationFunction["RouteRequest"].InvokeAsync("Hi, My name is Ananya, I drive an electric car. Could you please help me find an available parking slot?");
Console.WriteLine(availableParkingSlots); */