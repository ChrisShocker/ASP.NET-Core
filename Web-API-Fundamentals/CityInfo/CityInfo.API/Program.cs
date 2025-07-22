using Microsoft.AspNetCore.StaticFiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder
    .Services.AddControllers(options =>
    {
        //input formatters can also be configured here to enforce specific formats

        // return 406 Not Acceptable on invalid format requests
        options.ReturnHttpNotAcceptable = true;
    })
    .AddNewtonsoftJson()
    //add support for xml serialization
    .AddXmlDataContractSerializerFormatters();

// add ProblemDetails middleware
// this allows manipulation of the ProblemDetails response
//builder.Services.AddProblemDetails(options =>
//{
//    options.CustomizeProblemDetails = (ctx) =>
//    {
//        ctx.ProblemDetails.Extensions.Add("server", Environment.MachineName);
//    };
//});

builder.Services.AddProblemDetails();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// add support for getting file content types based on file extensions
builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

var app = builder.Build();

/* Everything below is middleware and the order it's declared in matters */

if (!app.Environment.IsDevelopment())
{
    // this will add the exception handler middleware to the app
    // it will handle exceptions thrown in all middleware after this
    // and return a ProblemDetails response
    app.UseExceptionHandler("/api/errors");
}

// Configure the HTTP request pipeline.
// this is the default env setup, other envs can be used to configure different middleware
if (app.Environment.IsDevelopment())
{
    // only add the following middleware when the app environment is development

    // this will add the swagger generator to the app
    app.UseSwagger();
    app.UseSwaggerUI();
}

// redirect http calls to be https
app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

// No routes are specified, so we have to setup the routing manually using attribute based routing
app.MapControllers();

app.Run();
