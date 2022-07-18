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
            // Adicionar o contexto de conex�o do banco

            services.AddDbContext<ContextDB>(op => op.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

           // Habilitando o recurso de api atrav�s de Controller  
            services.AddControllers();
            // adiciona o gerador do swagger atrav�s dos controller cria a documenta��o do swagger tendo a vers�o , titulo...
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TaskManagerAPI", Version = "v1" });
            });

            // Configura��o de autentica��o do JWT

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
            // adicionando ao escopo a interce e quem sera sua implementa��o
            services.AddScoped<IUserRepository, UserRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // configura��o do servidor
        // atrav�s do IWebHostEnvironment ele verifica qual o ambiente que est� rodando a aplica��o 
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // verifica se o ambiente est� em desenvolvimento
            if (env.IsDevelopment())
            {
                // pagina de error
                app.UseDeveloperExceptionPage();
               // adicionado o swagger 
                app.UseSwagger();
               //Interface gr�fica UseSwaggerUI adiciona a url obtem o arquivo gerado na configure services AddSwagger e transforma na interface gr�fica
               //para que seja visualizado todos os m�todos.
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
            
            // utiliza��o de autoriza��es
            app.UseAuthorization();

            // utilizando os enpoints mapeando todos os controllers
            app.UseEndpoints(endpoints =>
            {
                // escanea todos que tenham api controller como identifica��o
                endpoints.MapControllers();
            });
        }
    }
}
