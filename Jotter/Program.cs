using FluentValidation;
using Jotter.Configs;
using Jotter.Endpoints;
using Jotter.Repository;
using Jotter.Repository.IRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<NoteRepository>();
builder.Services.AddSingleton<AuditRepository>();
builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<IAuditRepository, AuditRepository>();
builder.Services.AddAutoMapper(typeof(AutoMapperConfig));
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var AllowlOrigins = "_AllowOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowlOrigins,
                      builder =>
                      {
                          builder.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
                      });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.ConfigureNotes();
app.ConfigureAudits();
app.ConfigureUsers();

app.UseCors(AllowlOrigins);

MockLoggedInUser.LogIn();

app.Run();