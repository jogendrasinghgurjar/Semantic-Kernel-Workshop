using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.SkillDefinition;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKWorkshop.Plugin.OrchestrationPlugin
{
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
            var getFuelType = _kernel.Skills.GetFunction("VehiclePlugin", "GetFuelType");
            var getVehicleType = _kernel.Skills.GetFunction("VehiclePlugin", "GetVehicleType");
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
                getFuelType,
                getVehicleType,
                vehicleParkingFunction,
                CreateResponse);

            return output["input"];
        }
    }
}
