using Microsoft.SemanticKernel;


// ... instantiating  kernel
var builder = new KernelBuilder();

builder.WithAzureTextCompletionService(
            "", // Azure OpenAI Deployment Name
            "", // Azure OpenAI Endpoint
            "");      // Azure OpenAI Key

var kernel = builder.Build();

// Import Semantic Plugins
var pluginsDirectory = Path.Combine("Path to your plugin directory");
var orchestrationPlugin = kernel.ImportSemanticSkillFromDirectory(pluginsDirectory, "OrchestrationPlugin");

// Create a new context and set the input, history, and options variables.
var context = kernel.CreateNewContext();
context["input"] = "I want to park my bike can you help me find an available slot?";
context["options"] = "FetchAvailableParkingSlot, FindMyParkedVehicle, OutOfContext";

// Run the GetIntent function with the context.
var result = await orchestrationPlugin["GetIntent"].InvokeAsync(context);

Console.WriteLine(result);

var input = "User: I drive an EV car";
var vehiclePlugin = kernel.ImportSemanticSkillFromDirectory(pluginsDirectory, "VehiclePlugin");
var fuelResult = await vehiclePlugin["GetFuelType"].InvokeAsync(input);

Console.WriteLine(fuelResult);

var vehicleResult = await vehiclePlugin["GetVehicleType"].InvokeAsync(input);

Console.WriteLine(vehicleResult);



