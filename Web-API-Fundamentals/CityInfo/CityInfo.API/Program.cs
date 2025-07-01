var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


/* Everything below is middleware and the order it's declared in matters */

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

// No routes are specefied, so we have to setup the routing manually using attribute based routing 
app.MapControllers();

app.Run();
