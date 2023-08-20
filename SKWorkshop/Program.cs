using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Orchestration;
using SKWorkshop.Plugin;
using System.Data;
System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);


// 1... instantiating  kernel
IKernel kernel = SemanticKernelConfiguration();

// 2... Semantic plugin
SKContext intent = await SemanticKernelSkills(kernel);


// 3... Memory context
intent = await SemanticKernelContextMemory(kernel, intent);

// 4... Native function & Connector
await SemanticKernel_NF_Connector(kernel);

// 5... Planner
await SemanticKernelPlanner(kernel);

//__________________________________________________________________________________________________

static IKernel SemanticKernelConfiguration()
{
    var builder = new KernelBuilder();

    builder.WithAzureTextCompletionService(
                "",                // Model Deployment Name 
                "",  // Azure OpenAI Endpoint
                "");      // Azure OpenAI Key

    var kernel = builder.Build();
    return kernel;
}

static async Task<SKContext> SemanticKernelSkills(IKernel kernel)
{
    var pluginsDirectory = Path.Combine("C:\\Users\\pramishra\\Desktop\\MLTrainingMaterial\\Semantic-Kernel-Workshop\\SKWorkshop\\Plugin\\");
    // Import the OrchestratorPlugin from the plugins directory.
    var orchestratorPlugin = kernel.ImportSemanticSkillFromDirectory(pluginsDirectory, "OrchestrationPlugin");

    // a. Get the GetIntent function from the OrchestratorPlugin and run it
    var intent = await orchestratorPlugin["GetIntent"]
         .InvokeAsync("I want to park my vehicle can you help me find an available slot?");

    Console.WriteLine(intent);

    // b. Get vehicle type

    // Import the VehiclePlugin from the plugins directory.
    var vehiclePlugin = kernel.ImportSemanticSkillFromDirectory(pluginsDirectory, "VehiclePlugin");

    // Get the GetVehicleType function and run it
    var vehicleType = await vehiclePlugin["GetVehicleType"].InvokeAsync("Hi, Please tell me available parking slots for Bike");

    Console.WriteLine(vehicleType);
    return intent;
}

static async Task<SKContext> SemanticKernelContextMemory(IKernel kernel, SKContext intent)
{
    var context = kernel.CreateNewContext();
    context["options"] = "FetchAvailableParkingSlot, FindMyParkedVehicle, OutOfContext";
    context["input"] = "I want to park my vehicle can you help me find an available slot?";

    var pluginsDirectory = Path.Combine("C:\\Users\\pramishra\\Desktop\\MLTrainingMaterial\\Semantic-Kernel-Workshop\\SKWorkshop\\Plugin\\");
    var orchestratorPlugin = kernel.ImportSemanticSkillFromDirectory(pluginsDirectory, "OrchestrationPlugin");
    intent = await orchestratorPlugin["GetIntent"]
         .InvokeAsync(context);

    Console.WriteLine(intent);
    return intent;
}

static async Task SemanticKernel_NF_Connector(IKernel kernel)
{
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
}

static async Task SemanticKernelPlanner(IKernel kernel)
{
    var pluginsDirectory = Path.Combine("C:\\Users\\pramishra\\Desktop\\MLTrainingMaterial\\Semantic-Kernel-Workshop\\SKWorkshop\\Plugin\\");
    // Import Semantic Plugins
    var orchestrationPlugin = kernel.ImportSemanticSkillFromDirectory(pluginsDirectory, "OrchestrationPlugin");
    var vehiclePlugin = kernel.ImportSemanticSkillFromDirectory(pluginsDirectory, "VehiclePlugin");

    // Import the native function
    var vehicleParkingFunction = kernel.ImportSkill(new VehicleParking(), "VehicleParking");
    var orchestrationFunction = kernel.ImportSkill(new OrchestrationPlugin(kernel), "OrchestrationPlugin");

    // Make a request that runs the FetchAvailableParkingSlot function
    var availableParkingSlots = await orchestrationFunction["RouteRequest"].InvokeAsync("Hi, My name is Ananya, I drive an electric car. Could you please help me find an available parking slot?");
    Console.WriteLine(availableParkingSlots);
}