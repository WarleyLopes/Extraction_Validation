1-Get the extracted data file you want to validate in json format;

2-Rename it to exctracted_data.json;

3-Move it into this project folder under exctraction_validation\exctraction_validation\Data and replace the previous file there;

4-Select one json row to be the example to the others, so we can use a JSON Schema generator tool to validate all the others, or we could create a Schema ourselves with settings like fields that are required or not, type of the fields, regex match and others;

5-Place one json entry into https://jsonschema.net and click to "Infer Schema";

6-On the right part of the page, click on the 2nd icon to show the schema ready to be copied out and click the icon to copy to clipboard;

7-Create a json file named correct_row_json_schema.json with the schema generator by JsonSchema.net and save it under exctraction_validation\exctraction_validation\Data and replace the previous file there;

8-Install Visual Studio if you don't have it;

9-Open up exctraction_validation.sln file to open the project;

10-On the Solution Explorer window, right click the solution and click on Restore NuGet packages;

11-Build the project;

12-Open Test Explorer (CTRL+E,T) or Test->Windows->Test Explorer, run the test;

13-Check out the results in root folder of the project through the file index.html.