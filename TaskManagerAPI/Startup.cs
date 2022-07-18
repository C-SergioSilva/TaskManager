using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Infra;
using TaskManagerAPI.Infra.Interfaces;
using TaskManagerAPI.Infra.Repository;

namespace TaskManagerAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Adicionar o contexto de conexão do banco

            services.AddDbContext<ContextDB>(op => op.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

           // Habilitando o recurso de api através de Controller  
            services.AddControllers();
            // adiciona o gerador do swagger através dos controller cria a documentação do swagger tendo a versão , titulo...
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TaskManagerAPI", Version = "v1" });
            });

            // Configuração de autenticação do JWT

            var encryptionKey = Encoding.ASCII.GetBytes(KeyJwt.KeySecurity);
            services.AddAuthentication(authentication =>
            {
                authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(authentication =>
            {
                authentication.RequireHttpsMetadata = false;
                authentication.SaveToken = true;
                authentication.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(encryptionKey),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // libera ou restringi o acesso de dominios a API
            services.AddCors();
            // adicionando ao escopo a interce e quem sera sua implementação
            services.AddScoped<IUserRepository, UserRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // configuração do servidor
        // através do IWebHostEnvironment ele verifica qual o ambiente que está rodando a aplicação 
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // verifica se o ambiente está em desenvolvimento
            if (env.IsDevelopment())
            {
                // pagina de error
                app.UseDeveloperExceptionPage();
               // adicionado o swagger 
                app.UseSwagger();
               //Interface gráfica UseSwaggerUI adiciona a url obtem o arquivo gerado na configure services AddSwagger e transforma na interface gráfica
               //para que seja visualizado todos os métodos.
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskManagerAPI v1"));
            }

            // Hablitando os redirecionamento do https e http 
            app.UseHttpsRedirection();

            // Utilizando roteamento 
            app.UseRouting();
            app.UseCors(c =>
                c.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            );

            app.UseAuthentication();
            
            // utilização de autorizações
            app.UseAuthorization();

            // utilizando os enpoints mapeando todos os controllers
            app.UseEndpoints(endpoints =>
            {
                // escanea todos que tenham api controller como identificação
                endpoints.MapControllers();
            });
        }
    }
}
