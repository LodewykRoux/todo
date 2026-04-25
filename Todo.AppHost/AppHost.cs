var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres");
var postgresdb = postgres.AddDatabase("postgresdb");

builder.AddProject<Projects.Todo_Api>("todo-api")
    .WaitFor(postgresdb)
    .WithReference(postgresdb);

builder.Build().Run();