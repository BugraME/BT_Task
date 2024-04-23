using BT.ServiceHelper;
using BuroTime.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<LogService>();
builder.Services.AddScoped<Logger>();
builder.Services.AddControllers()
	//Swagger UI'da enum'ların string olarak gösterilmesini sağlar.
	.AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
	options.SwaggerDoc("v1", new OpenApiInfo {
		Title = "bürotime Doviz Servisi",
		Description =
		"<b>Login</b> -> JWT Token alabilmek için gerekli servis [bürotime , 123] <br/>" +
		"------------------------------------------------- <br/>" +
		"<b>GetCurrency</b> -> Parametre olarak girilen tarihe göre o günün kur değerlerini (EURO, USD, GBP) listeler. <br/>" +
		"<b>Compare Dates</b> *> Bugünün kuru ile girilen kur farkını hesaplayarak yüzdesel değişim oranını ve fark tutarını listeler.",
		License = new() {
			Name = "BugraME",
			Url = new Uri("https://www.linkedin.com/in/bugrame/")
		},
	});

	options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey,
		In = ParameterLocation.Header,
		Scheme = "Bearer",
		BearerFormat = "JWT",
		Description = "Servisleri kullanabilmek için JWT kimlik doğrulama gerçekleştirmelisiniz.<br/>" +
		"<b>Auth\\\\Login</b> metoduyla \"Token\" almanız ve <b>Bearer {Token}</b> şeklinde başına Bearer ekledikten sonra boşluk bırakarak yazmanız gerekmektedir."
	});
	options.AddSecurityRequirement(new OpenApiSecurityRequirement  {
		{ new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }}, new List<string>() }
	});
});


builder.Services.AddHttpClient("tcmb", c => { c.BaseAddress = new Uri("https://www.tcmb.gov.tr/"); });

builder.Services.AddAuthentication(options => {
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
	options.RequireHttpsMetadata = false;
	options.TokenValidationParameters = new TokenValidationParameters {
		ValidateIssuer = false,
		ValidateAudience = false,
		ValidateIssuerSigningKey = true,
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ED236AE5-1841-4FFC-9D61-228B3A08336D")),
	};
});
builder.Services.AddAuthorization();

WebApplication app = builder.Build();

app.UseStaticFiles(new StaticFileOptions {
	FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot")),
});

app.UseSwagger();
app.UseSwaggerUI(options => {
	options.SwaggerEndpoint("/swagger/v1/swagger.json", "BüroTime REST API Service");
	options.InjectJavascript("/js/swagger.js"); // Özel JavaScript dosyasını Swagger UI'a enjekte eder.
	options.InjectStylesheet("/css/swagger.css"); // Özel CSS dosyasını Swagger UI'a enjekte eder.
	options.DisplayRequestDuration(); // İsteklerin sürelerini Swagger UI'da gösterir.
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();