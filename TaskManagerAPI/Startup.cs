using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
           // Habilitando o recurso de api atrav�s de Controller  
            services.AddControllers();
            // adiciona o gerador do swagger atrav�s dos controller cria a documenta��o do swagger tendo a vers�o , titulo...
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TaskManagerAPI", Version = "v1" });
            });
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
