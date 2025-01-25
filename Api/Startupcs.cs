//using Autofac;
//using Autofac.Integration.WebApi;
//using System;
//using Microsoft.Owin;
//using Owin;
//using System.Web.Http;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Serialization;
//using Microsoft.Owin.Security.OAuth;
//using Microsoft.Owin.Cors;
//using CrossCutting;
//using System.Reflection;
//using Swashbuckle.Application;
//using System.Web;
//using System.Text;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.Owin.Security.Jwt;
//using Microsoft.Owin.Security;
//using ApplicationService.Utility;
//using Infrastructure.Transaction;
//using Microsoft.AspNetCore.Cors.Infrastructure;
//using System.ComponentModel;

//[assembly: OwinStartup(typeof(Api.Startup))]

//namespace Api
//{
//    public partial class Startup
//    {
//        public void Configuration(IAppBuilder app)
//        {

//            var key = Encoding.ASCII.GetBytes("36B7247D-535D-4D46-8AA1-EBC5A01A696B");

//            // Habilitar CORS
//            app.UseCors(CorsOptions.AllowAll);

//            // Configuração do JWT Authentication Middleware
//            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
//            {
//                AuthenticationMode = AuthenticationMode.Active,
//                TokenValidationParameters = new TokenValidationParameters
//                {
//                    ValidateIssuerSigningKey = true,
//                    IssuerSigningKey = new SymmetricSecurityKey(key), // Chave simétrica
//                    ValidateIssuer = false, // Não validar emissor
//                    ValidateAudience = false, // Não validar audiência
//                    ValidateLifetime = true, // Validar expiração do token
//                    ClockSkew = TimeSpan.Zero // Desativar tolerância no tempo de expiração
//                }
//            });

//            // Adicionar mais middleware como Web API


//            var config = new HttpConfiguration();

//            // Configure Dependency Injection
//            var containerBuilder = Bootstrapper.Config();
//            var container = ConfigureDependencyInjection(config, containerBuilder);

//            // config.MessageHandlers.Add(new RateLimitingHandler(50, TimeSpan.FromMinutes(1)));  // Limite de 50 requisições por minuto




//            ConfigureWebApi(config);
//            ConfigureOAuth(app, container.Resolve<IUnitOfWork>());



//            app.UseWebApi(config);


//            // Configurando o Swagger para gerar a documentação da API 
//            // config.EnableSwagger(c => { c.SingleApiVersion("v1", "API"); c.RootUrl(req => req.RequestUri.GetLeftPart(UriPartial.Authority) + VirtualPathUtility.ToAbsolute("~/").TrimEnd('/')); }).EnableSwaggerUi();
//        }

//        public static IContainer ConfigureDependencyInjection(HttpConfiguration config, ContainerBuilder containerBuild)
//        {
//            containerBuild.RegisterApiControllers(Assembly.GetExecutingAssembly());
//            var container = containerBuild.Build();
//            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

//            return container;
//        }

//        public static void ConfigureWebApi(HttpConfiguration config)
//        {
//            // Remove o XML
//            var formatters = config.Formatters;
//            formatters.Remove(formatters.XmlFormatter);

//            // Modifica a identação
//            var jsonSettings = formatters.JsonFormatter.SerializerSettings;
//            jsonSettings.Formatting = Formatting.Indented;
//            jsonSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

//            // Modifica a serialização
//            formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling =
//                ReferenceLoopHandling.Ignore;

//            // necessário para salvar as datas com o timezone local
//            formatters.JsonFormatter.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;

//            // Web API routes
//            config.MapHttpAttributeRoutes();

//            config.Routes.MapHttpRoute(
//                "DefaultApi",
//                "api/{controller}/{id}",
//                new { id = RouteParameter.Optional }
//            );

//        }

//        public void ConfigureOAuth(IAppBuilder app, IUnitOfWork uow)
//        {
//            app.UseCors(CorsOptions.AllowAll);
//            OAuthAuthorizationServerOptions oAuthServerOptions = new OAuthAuthorizationServerOptions()
//            {
//                AllowInsecureHttp = true,//Permite conexão não seguras http
//                TokenEndpointPath = new PathString("/api/v1/security/token"),//Local de onde o token vai ser disponibilizado
//                AccessTokenExpireTimeSpan = TimeSpan.FromHours(2),
//                //Provider = new AuthorizationServerProvider(uow, repository, ctrRepository)
//            };

//            // Geraçaõ do Token
//            app.UseOAuthAuthorizationServer(oAuthServerOptions);
//            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());


//        }


//    }
//}
