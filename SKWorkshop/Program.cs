using Microsoft.SemanticKernel;
using SKWorkshop.Plugin.OrchestrationPlugin;
using SKWorkshop.Plugin.CustomPlugin;
using System.Data;

System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
// ... instantiating  kernel
var builder = new KernelBuilder();

builder.WithAzureTextCompletionService(
            "", "", "");      // Azure OpenAI Key

var kernel = builder.Build();

// Import Semantic Plugins
var pluginsDirectory = Path.Combine("C:\\Users\\pramishra\\Desktop\\MLTrainingMaterial\\Semantic-Kernel-Workshop\\SKWorkshop\\Plugin\\");
var orchestrationPluginDirectory = kernel.ImportSemanticSkillFromDirectory(pluginsDirectory, "OrchestrationPlugin");
var vehiclePluginDirectory = kernel.ImportSemanticSkillFromDirectory(pluginsDirectory, "VehiclePlugin");

// Import the native function
var vehicleParkingPlugin = kernel.ImportSkill(new VehicleParking(kernel), "VehicleParking");
var orchestrationPlugin = kernel.ImportSkill(new OrchestrationPlugin(kernel), "OrchestrationPlugin");

// Make a request that runs the FetchAvailableParkingSlot function
var availableParkingSlots = await orchestrationPlugin["RouteRequest"].InvokeAsync("Hi, My name is Ananya, I drive an electric car. Could you please help me find an available parking slot?");
Console.WriteLine(availableParkingSlots);

// Create a new context and set the input, history, and options variables.
var context = kernel.CreateNewContext();
context["input"] = "I want to park my bike can you help me find an available slot?";
context["options"] = "FetchAvailableParkingSlot, FindMyParkedVehicle, OutOfContext";

// Run the GetIntent function with the context.
var result = await orchestrationPlugin["GetIntent"].InvokeAsync(context);

Console.WriteLine(result);

var input = "User: I drive an Electric Car";
var vehiclePlugin = kernel.ImportSemanticSkillFromDirectory(pluginsDirectory, "VehiclePlugin");
var fuelResult = await vehiclePlugin["GetFuelType"].InvokeAsync(input);

Console.WriteLine(fuelResult);

var vehicleResult = await vehiclePlugin["GetVehicleType"].InvokeAsync(input);

Console.WriteLine(vehicleResult);

// code for displaying available parking slot //
var customPlugin = kernel.ImportSemanticSkillFromDirectory(pluginsDirectory, "CustomPlugin");
DataTable dtresult = new DataTable();
VehicleParking vehicleParking = new VehicleParking(kernel);
dtresult = vehicleParking.FetchAvailableParkingSlot(context);
if (dtresult.Rows.Count > 0)
{
    foreach (DataRow dr in dtresult.Rows)
    {
        foreach (var item in dr.ItemArray)
        {
            Console.Write(item + " ");
        }
        Console.WriteLine();
    }
}




