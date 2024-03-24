using ELibrary.src.ELibrary.Api.Services;
using ELibrary.src.ELibrary.Api.Services.BookFileService;
using ELibrary.src.ELibrary.Api.Services.ImageService;
using ELibrary.src.ELibrary.Api.Services.TokenService;
using ELibrary.src.ELibrary.Domain.BookModel;
using ELibrary.src.ELibrary.Domain.CommentModel;
using ELibrary.src.ELibrary.Domain.GenreModel;
using ELibrary.src.ELibrary.Domain.RatingModel;
using ELibrary.src.ELibrary.Domain.UserModel;
using ELibrary.src.ELibrary.Infrastructure;
using ELibrary.src.ELibrary.Infrastructure.Data.BookModel;
using ELibrary.src.ELibrary.Infrastructure.Data.CommentModel;
using ELibrary.src.ELibrary.Infrastructure.Data.GenreModel;
using ELibrary.src.ELibrary.Infrastructure.Data.RatingModel;
using ELibrary.src.ELibrary.Infrastructure.Data.UserModel;
using ELibrary.src.ELibrary.Infrastructure.UoW;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace ELibrary.src.ELibrary.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //
            //var t = Configuration.GetSection("JwtToken:Secret").Value;
            //Console.WriteLine(t);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.
                    UTF8.GetBytes(Configuration.GetValue<string>("jwtSecret"))),
                };
            });
            services.AddAuthorization();
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddControllers();
            services.AddDbContext<ELibraryDbContext>(x =>
                x.UseSqlServer(Configuration.GetConnectionString("ELibraryConnection")));
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<ITokenService, TokenService>();
            //services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IBookFileService, BookFileService>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IRatingRepository, RatingRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "ELibrary", Version = "v1" }); });
            //services.
            services.AddCors(options =>
            {
                options.AddPolicy(name: "CorsPolicy",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000")
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ELibrary v1"));
            }
            //app.UseMiddleware<UserAuthenticationMiddleware>();
            app.UseRouting();            
            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
