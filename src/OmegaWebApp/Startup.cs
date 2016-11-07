using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Omega.DAL;
using OmegaWebApp.Services;
using OmegaWebApp.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.OAuth;
using AspNet.Security.OAuth.Spotify;

namespace OmegaWebApp
{
    public class Startup
    {
        public Startup( IHostingEnvironment env )
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath( env.ContentRootPath )
                .AddJsonFile( "appsettings.json", optional: true, reloadOnChange: true )
                .AddJsonFile( $"appsettings.{env.EnvironmentName}.json", optional: true )
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices( IServiceCollection services )
        {
            // Add framework services.
            services.AddMvc();
            services.AddTransient( _ => new UserGateway( Configuration[ "data:azure:ConnectionString" ] ) );
            services.AddTransient<PasswordHasher>();
            services.AddTransient<UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure( IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory )
        {
            loggerFactory.AddConsole( Configuration.GetSection( "Logging" ) );
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler( "/Home/Error" );
            }

            app.UseStaticFiles();

            app.UseCookieAuthentication( new CookieAuthenticationOptions
            {
                AuthenticationScheme = CookieAuthentication.AuthenticationScheme
            } );

            string secretKey = Configuration["JwtBearer:SigningKey"];
            SymmetricSecurityKey signingKey = new SymmetricSecurityKey( Encoding.ASCII.GetBytes( secretKey ) );

            app.UseJwtBearerAuthentication( new JwtBearerOptions
            {
                AuthenticationScheme = JwtBearerAuthentication.AuthenticationScheme,
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,

                    ValidateIssuer = true,
                    ValidIssuer = Configuration["JwtBearer:Issuer"],

                    ValidateAudience = true,
                    ValidAudience = Configuration["JwtBearer:Audience"]
                }
            } );

            SpotifyAuthenticationEvents spotifyAuthenticationEvents = new SpotifyAuthenticationEvents( app.ApplicationServices.GetRequiredService<UserService>() );
            SpotifyAuthenticationOptions spotifyOptions = new SpotifyAuthenticationOptions
            {
                ClientId = Configuration["Authentication:Spotify:ClientId"],
                ClientSecret = Configuration["Authentication:Spotify:ClientSecret"],
                SignInScheme = CookieAuthentication.AuthenticationScheme,
                Events = new OAuthEvents
                {
                    OnCreatingTicket = spotifyAuthenticationEvents.OnCreatingTicket
                }
            };
            spotifyOptions.Scope.Add( "user-read-email" );// if email is needed.
            spotifyOptions.Scope.Add( "playlist-read-private" );
            spotifyOptions.Scope.Add( "playlist-read-collaborative" );
            spotifyOptions.Scope.Add( "user-library-read" );

            app.UseSpotifyAuthentication( spotifyOptions );

            app.UseMvc( routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}" );
            } );
        }
    }
}