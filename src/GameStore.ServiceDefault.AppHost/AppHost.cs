using Azure.Provisioning.Storage;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var database = builder.AddAzurePostgresFlexibleServer("postgres")
.RunAsContainer(postgres =>
{
    postgres.WithHostPort(5432);
    postgres.WithImageTag("17.4");
    postgres.WithDataVolume();
    postgres.WithLifetime(ContainerLifetime.Persistent);
    postgres.WithPgAdmin(pgAdmin =>
    {
        pgAdmin.WithHostPort(5050);
        pgAdmin.WithLifetime(ContainerLifetime.Persistent);
    });
})
.AddDatabase("GameStoreDB", "gamestore");

var blobs = builder.AddAzureStorage("storage")
.ConfigureInfrastructure(infra =>
{
    var resources = infra.GetProvisionableResources();
    var storageAccount = resources.OfType<StorageAccount>().Single();
    storageAccount.AllowBlobPublicAccess = true;
})
.RunAsEmulator(storage =>
{
    storage.WithBlobPort(10000);
    storage.WithImageTag("3.34.0");
    storage.WithDataVolume();
    storage.WithLifetime(ContainerLifetime.Persistent);
})
.AddBlobs("Blobs");

var keycloak = builder.AddKeycloak("keycloak", port: 8080)
    .WithImageTag("26.0.7")
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent)
    .WithRealmImport("../../localinfra");

var keyCloakAuthority = ReferenceExpression.Create($"{keycloak.GetEndpoint("http").Property(EndpointProperty.Url)}/realms/gamestore");

builder.AddProject<GameStore_Api>("gamestore-api")
    .WithReference(database)
    .WaitFor(database)
    .WithReference(blobs)
    .WaitFor(blobs)
    .WaitFor(keycloak)
    .WithEnvironment("Authentication__Schemes__Keycloak__Authority", keyCloakAuthority);

builder.Build().Run();
