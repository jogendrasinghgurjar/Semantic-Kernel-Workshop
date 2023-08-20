using System;
using System.ComponentModel;
using System.Data;
using ExcelDataReader;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.SkillDefinition;

namespace SKWorkshop.Plugin;

public class VehicleParking
{

    [SKFunction, Description("Fetches available parking slot.")]
    [SKParameter("vehicleType", "The type of vehicle")]
    [SKParameter("fuelType", "The type of fuel")]
    [SKParameter("userName", "The name of the user")]
    public string FetchAvailableParkingSlot(SKContext context)
	{
        DataTable? availableslots = new DataTable { TableName = "AvailableSlots" };

        string vehicleType = context["vehicleType"];
        string fuelType = context["fuelType"];
        string name = context["userName"];
        using (var stream = File.Open("C:\\Users\\pramishra\\Desktop\\MLTrainingMaterial\\Semantic-Kernel-Workshop\\SKWorkshop\\Data\\SemanticKernel_ParkingData.xlsx", FileMode.Open, FileAccess.Read))
        {
            // Auto-detect format, supports:
            //  - Binary Excel files (2.0-2003 format; *.xls)
            //  - OpenXml Excel files (2007 format; *.xlsx, *.xlsb)
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                // Choose one of either 1 or 2:

                // 1. Use the reader methods
                do
                {
                    while (reader.Read())
                    {
                        // reader.GetDouble(0);
                    }
                } while (reader.NextResult());

                // 2. Use the AsDataSet extension method
                // The result of each spreadsheet is in result.Tables
                //var result = reader.AsDataSet();

                var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true
                    }
                });

                // 3. verify if the user & vehicle details are valid . 
                for (int i = 0; i < result.Tables[0].Rows.Count; i++)
                {
                    if ((vehicleType.Trim() == result.Tables[0].Rows[i]["Vehicle Type"].ToString()) && (fuelType.Trim() == result.Tables[0].Rows[i]["Fuel Type"].ToString()) && (name.Trim()== result.Tables[0].Rows[i]["Employee"].ToString()))
                    {
                        // user details are valid . proceed to available parking slot(s) searching . 
                        string? buildingName = result.Tables[0].Rows!=null?result.Tables[0].Rows[i]["Outlook/Building Name"].ToString():string.Empty;
                        //loading the available slots in a DataTable for filtering.
                        DataTable parkingslots = new DataTable();
                        parkingslots = result.Tables[1];
                        IEnumerable<DataRow> query = from pslot in parkingslots.AsEnumerable()
                                                     where pslot.Field<string>("Building Name").ToLower().ToString() == buildingName.ToLower()
                                                     select pslot;
                        // Building Name , Floor no. , Bay no.
                        availableslots = query.CopyToDataTable<DataRow>();

                    }
                }
            }
        }
        string res = string.Join(Environment.NewLine, availableslots.Rows.OfType<DataRow>().Select(x => string.Join(" ; ", x.ItemArray)));
       
        return res;
    }
}
