using Azure.Storage.Blobs;
using BlobStorage.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddSingleton<BlobServiceClient>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var connectionString = config["AzureBlob:ConnectionString"];
    return new BlobServiceClient(connectionString);
});

builder.Services.AddScoped<BlobStorageService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();