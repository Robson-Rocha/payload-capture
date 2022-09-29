# Payload Capture Azure Function Demo

This repository contains the source code for a very basic HTTP Triggered Azure Function that captures any payload posted to it and saves it, along with any request headers, to an Azure Storage Blob Container.

It also demonstrates Dynamic Output Binding, as the both blobs are created with Output Bindings added at runtime.

## Requirements

[.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

[Azure Function Core Tools v4](https://github.com/Azure/azure-functions-core-tools)

[VS Code](https://code.visualstudio.com/)

[Azure Functions extension for VS Code](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azurefunctions)

[Azurite extension for VS Code](https://marketplace.visualstudio.com/items?itemName=Azurite.azurite)

[Azure Storage Explorer](https://azure.microsoft.com/en-us/products/storage/storage-explorer)

[Postman](https://www.postman.com/downloads/)

## Usage (Local)

1. Clone the repository

2. Open the "src" folder in VS Code and wait until VS Code initializes the code

3. [Start Azurite](https://marketplace.visualstudio.com/items?itemName=Azurite.azurite) from VS Code

4. Run the code by pressing F5 in VS Code

5. Using Postman, send a POST request (an "application/json" will work best, anything else will be treated as text) to http://localhost:7071/api/Capture; optionally add some custom headers as well

6. Using Azure Storage Explorer, open the Blob Container in "Local & Attached > Storage Accounts > (Emulator - Default Ports) > Blob Containers > captured-payloads"

7. Check the two files created: one containing the actual payload, the other containing all request headers

## Usage (Azure)

1. [Create a new .NET 6, v4 Function App in Azure using VS Code](https://learn.microsoft.com/en-us/azure/azure-functions/functions-develop-vs-code?tabs=csharp#publish-to-azure) (you can create it from the portal if you want, just make sure to sign in VS Code into your subscription).
   Make sure you get the generated URL from your new function app
2. [Deploy the application](https://learn.microsoft.com/en-us/azure/azure-functions/functions-develop-vs-code?tabs=csharp#republish-project-files)
3. Using Postman, send a POST request to https://[your function app name].azurewebsites.net/api/Capture; optionally add some custom headers as well
4. Using Azure Storage Explorer, open the Blob Container in "[Your Azure Subscription] > Storage Accounts > [your function app name] > Blob Containers > captured-payloads"

5. Check the two files created: one containing the actual payload, the other containing all request headers