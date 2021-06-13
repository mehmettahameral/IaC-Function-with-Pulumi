using Pulumi;
using Azure = Pulumi.Azure;

class MyStack : Stack
{
    public const string _functionResourceGroupName = "rgdevci";
    public const string _stroageName = "rgdevstorage";
    public const string _appPlanName = "rgdevappplan";
    public const string _functionName = "rgdevfunction";

    public MyStack()
    {
        var resourceGroup = new Azure.Core.ResourceGroup(_functionResourceGroupName, new Azure.Core.ResourceGroupArgs
        {
            Location = "uksouth",
        });

        var exampleAccount = new Azure.Storage.Account(_stroageName, new Azure.Storage.AccountArgs
        {
            ResourceGroupName = resourceGroup.Name,
            Location = resourceGroup.Location,
            AccountTier = "Standard",
            AccountReplicationType = "LRS",
        });

        // consumption plan
        var examplePlan = new Azure.AppService.Plan(_appPlanName, new Azure.AppService.PlanArgs
        {
            Location = resourceGroup.Location,
            ResourceGroupName = resourceGroup.Name,
            Kind = "FunctionApp",
            Sku = new Azure.AppService.Inputs.PlanSkuArgs
            {
                Tier = "Dynamic",
                Size = "Y1",
            },
        });
        var exampleFunctionApp = new Azure.AppService.FunctionApp(_functionName, new Azure.AppService.FunctionAppArgs
        {
            Location = resourceGroup.Location,
            ResourceGroupName = resourceGroup.Name,
            AppServicePlanId = examplePlan.Id,
            StorageAccountName = exampleAccount.Name,
            StorageAccountAccessKey = exampleAccount.PrimaryAccessKey,
        });
    }
}
