using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace exctraction_validation.Tests
{
    [TestFixture]
    public class ExtractionValidation
    {
        [Test]
        public void Validate()
        {
            //reads the exctracted data in json format from Data folder
            string jsonString = System.Text.Encoding.UTF8.GetString(exctraction_validation.Data.Resources.exctracted_data);

            //reads json schema on how the data should be structured
            JSchema jSchema = JSchema.Parse(System.Text.Encoding.UTF8.GetString(exctraction_validation.Data.Resources.correct_row_json_schema));

            //transforms the string to a Newtonsoft.JsonObject for easier manipulation
            var jArray = JArray.Parse(jsonString);

            //counters to check how many were valid or invalid
            int countValids=0, countInvalids=0;

            //preparing a list of errrors to output later
            IList<string> errorList = new List<string>();

            //checks how many rows are not matching the expected schema
            foreach (var json in jArray)
            {
                //this library has a blocking of 1000 validations per hour
                if (countValids + countInvalids < Int16.Parse(ConfigurationManager.AppSettings["max_validations_per_hour"]))
                {
                    //checks if each row is matching the expected validation for the json schema
                    if (json.IsValid(jSchema, out IList<string> errorMessages))
                        countValids++;
                    else
                    {
                        //if fails the validation, count it and log an error explaining why
                        countInvalids++;
                        errorList = errorList.Concat(errorMessages).ToList();
                    }
                }
                //breaks the loop in case of reaching the maximum validations per hour
                else
                    break;
            }

            //prepares a test execution report
            var reporter = new ExtentHtmlReporter(Directory.GetCurrentDirectory()+@"\index.html");
            var extent = new ExtentReports();
            extent.AttachReporter(reporter);

            //logging succeeded validations
            extent.CreateTest("Succeeded Validations").Pass("Valid rows: " + countValids);

            //logging failed validation
            extent.CreateTest("Not Succeeded Validations").Fail("Possible Invalid rows: " + countInvalids + "<br><br>"
                + String.Join("<br><br>", errorList.ToArray()));

            //creates the test execution report file in root folder of the project
            extent.Flush();

            //asserts that every row matched the expected schema
            Assert.That(countInvalids==0);
        }
    }
}
