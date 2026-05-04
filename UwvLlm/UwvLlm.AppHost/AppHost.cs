var builder = DistributedApplication.CreateBuilder(args);

// Storage API
var storage = builder.AddProject<Projects.UwvLlm_Storage>("storage")
    .WithExternalHttpEndpoints(); // expose https://localhost:7099

// Fabric node (console app)
var fabric = builder.AddProject<Projects.UwvLlm_Fabric>("fabric");

// Core API
var api = builder.AddProject<Projects.UwvLlm_Api>("api")
    .WithReference(storage)              // dependency injection / service discovery
    .WithReference(fabric)
    .WithExternalHttpEndpoints();        // expose https://localhost:7281

// (optioneel) environment variables voor duidelijke endpoints
api.WithEnvironment("STORAGE__BASEURL", storage.GetEndpoint("https"));
api.WithEnvironment("FABRIC__HOST", "localhost");
api.WithEnvironment("FABRIC__PORT", "9494");

builder.Build().Run();