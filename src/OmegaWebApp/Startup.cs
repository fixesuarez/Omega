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
using OmegaWebApp.ProviderDeezer;

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
            services.AddOptions();

            string secretKey = Configuration["JwtBearer:SigningKey"];
            SymmetricSecurityKey signingKey = new SymmetricSecurityKey( Encoding.ASCII.GetBytes( secretKey ) );

            services.Configure<TokenProviderOptions>( o =>
            {
                o.Audience = Configuration["JwtBearer:Audience"];
                o.Issuer = Configuration["JwtBearer:Issuer"];
                o.SigningCredentials = new SigningCredentials( signingKey, SecurityAlgorithms.HmacSha256 );
            } );
            // Add framework services.
            services.AddMvc();
            services.AddTransient( _ => new UserGateway( Configuration[ "data:azure:ConnectionString" ] ) );
            services.AddTransient( _ => new PlaylistGateway( Configuration[ "data:azure:ConnectionString" ] ) );
            services.AddTransient( _ => new AmbianceGateway( Configuration["data:azure:ConnectionString"] ) );
            services.AddTransient( _ => new EventGroupGateway( Configuration["data:azure:ConnectionString"] ) );
            services.AddTransient( _ => new CleanTrackGateway( Configuration["data:azure:ConnectionString"] ) );
            services.AddTransient( _ => new TrackGateway( Configuration[ "data:azure:ConnectionString" ] ) );
            services.AddTransient<PasswordHasher>();
            services.AddTransient<UserService>();
            services.AddTransient<PlaylistService>();
            services.AddTransient<TrackService>();
            services.AddSingleton<TokenService>();
            services.AddSingleton<AmbianceService>();
            services.AddSingleton<CleanTrackService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure( IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory )
        {
            loggerFactory.AddConsole( Configuration.GetSection( "Logging" ) );

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler( "/Home/Error" );
            }

            app.UseMiddleware<BreakPointMiddleware>();

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

            ExternalAuthenticationEvents facebookAuthenticationEvents = new ExternalAuthenticationEvents(
                new FacebookExternalAuthenticationManager( app.ApplicationServices.GetRequiredService<UserService>() ) );
            ExternalAuthenticationEvents spotifyAuthenticationEvents = new ExternalAuthenticationEvents(
                new SpotifyExternalAuthenticationManager( app.ApplicationServices.GetRequiredService<UserService>() ) );
            ExternalAuthenticationEvents deezerAuthenticationEvents = new ExternalAuthenticationEvents(
                new DeezerExternalAuthenticationManager( app.ApplicationServices.GetRequiredService<UserService>() ) );

            app.UseFacebookAuthentication( c =>
            {
                c.SignInScheme = CookieAuthentication.AuthenticationScheme;
                c.ClientId = Configuration["Authentication:Facebook:ClientId"];
                c.ClientSecret = Configuration["Authentication:Facebook:ClientSecret"];
                c.Events = new OAuthEvents
                {
                    OnCreatingTicket = facebookAuthenticationEvents.OnCreatingTicket,
                };
            } );

            SpotifyAuthenticationOptions spotifyOptions = new SpotifyAuthenticationOptions
            {
                ClientId = Configuration["Authentication:Spotify:ClientId"],
                ClientSecret = Configuration["Authentication:Spotify:ClientSecret"],
                SignInScheme = CookieAuthentication.AuthenticationScheme,
                Events = new OAuthEvents
                {
                    OnCreatingTicket = spotifyAuthenticationEvents.OnCreatingTicket,
                    
                }
        };
            spotifyOptions.Scope.Add( "user-read-email" );// if email is needed.
            spotifyOptions.Scope.Add( "playlist-read-private" );
            spotifyOptions.Scope.Add( "playlist-read-collaborative" );
            spotifyOptions.Scope.Add( "user-library-read" );

            app.UseSpotifyAuthentication( spotifyOptions );

            app.UseDeezerAuthentication( o =>
            {
                o.ClientId = Configuration["Authentication:Deezer:ClientId"];
                o.ClientSecret = Configuration["Authentication:Deezer:ClientSecret"];
                o.SignInScheme = CookieAuthentication.AuthenticationScheme;
                o.Events = new OAuthEvents
                {
                    OnCreatingTicket = deezerAuthenticationEvents.OnCreatingTicket
                };
                o.Scope.Add( "basic_access" );
                o.Scope.Add( "email" );
            } );

            app.UseMvc( routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}" );
            } );
        }
    }
}