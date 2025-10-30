# Game Store Backend
The .NET Backend for the Game Store application

## To create the Container Apps Environment
```powershell
az containerapp env create -n cae-gamestore-dev-eastus2-01 -g rg-gamestore-dev-eastus2-01 --location eastus2 --logs-destination none
```

## To create the Container App
```powershell
az containerapp create -n ca-gamestore-api-dev -g rg-gamestore-dev-eastus2-01 --environment cae-gamestore-dev-eastus2-01 --image crgamestoredev01.azurecr.io/gamestore-api:1.5 --registry-server crgamestoredev01.azurecr.io --registry-identity /subscriptions/77e57bb5-66a7-4111-8faf-680d7bdb9e13/resourcegroups/rg-gamestore-dev-eastus2-01/providers/Microsoft.ManagedIdentity/userAssignedIdentities/id-gamestore-dev-eastus2-01
```

## To update the container app
```powershell
$ImageTag = "1.6"
$registry = "crgamestoredev01.azurecr.io"

dotnet publish /t:PublishContainer -p ContainerImageTag=$imageTag -p
ContainerRegistry=$registry

az containerapp update -n ca-gamestore-api-dev -g
rg-gamestore-dev-eastus2-01 --image $registry/gamestore-api:$imageTag
```