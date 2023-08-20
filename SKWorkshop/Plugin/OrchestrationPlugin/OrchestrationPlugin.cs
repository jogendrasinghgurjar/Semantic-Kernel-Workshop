using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.SkillDefinition;
using Newtonsoft.Json.Linq;
using System.ComponentModel;

namespace SKWorkshop.Plugin;

public class OrchestrationPlugin
{
    IKernel _kernel;

    public OrchestrationPlugin(IKernel kernel)
    {
        this._kernel = kernel;
    }

    [SKFunction, Description("Routes the request to the appropriate function.")]
    public async Task<string> RouteRequest(SKContext context)
    {
        // Save the original user request
        string request = context["input"];

        // Add the list of available functions to the context
        context["options"] = "FetchAvailableParkingSlot, FindMyParkedVehicle, OutOfContext";

        // Retrieve the intent from the user request
        var GetIntent = _kernel.Skills.GetFunction("OrchestrationPlugin", "GetIntent");
        var CreateResponse = _kernel.Skills.GetFunction("OrchestrationPlugin", "CreateResponse");
        await GetIntent.InvokeAsync(context);
        string intent = context["input"].Trim();

        // Prepare the functions to be called in the pipeline
        var getVehicleinfo = _kernel.Skills.GetFunction("VehiclePlugin", "GetVehicleInfo");
        var extractVehicleInfoFromJson = _kernel.Skills.GetFunction("OrchestrationPlugin", "ExtractVehicleInfoFromJson");
        
        ISKFunction vehicleParkingFunction;

        // Prepare the math function based on the intent
        switch (intent)
        {
            case "FetchAvailableParkingSlot":
                vehicleParkingFunction = _kernel.Skills.GetFunction("VehicleParking", "FetchAvailableParkingSlot");
                break;
            case "FindMyParkedVehicle":
                vehicleParkingFunction = _kernel.Skills.GetFunction("CustomPlugin", "FindMyParkedVehicle");
                break;
            default:
                return "I'm sorry, I don't understand.";
        }

        // Create a new context object with the original request
        var pipelineContext = new ContextVariables(request);
        pipelineContext["original_request"] = request;

        // Run the functions in a pipeline
        var output = await _kernel.RunAsync(
            pipelineContext,
            getVehicleinfo,
            extractVehicleInfoFromJson,
            vehicleParkingFunction,
            CreateResponse);

        return output["input"];
    }

    [SKFunction, Description("Extracts user and vehicle information from JSON")]
    public SKContext ExtractVehicleInfoFromJson(SKContext context)
    {
        JObject vehicleInfo = JObject.Parse(context["input"]);

        // loop through numbers and add them to the context
        foreach (var info in vehicleInfo)
        {
            if (info.Key == "userName")
            {
                // add the first number to the input variable
                context["userName"] = info.Value.ToString();
                continue;
            }
            else if (info.Key == "vehicleType")
            {
                // add the rest of the numbers to the context
                context["vehicleType"] = info.Value.ToString();
            }
            else
            {
                context["fuelType"] = info.Value.ToString();
            }
        }
        return context;
    }
}
