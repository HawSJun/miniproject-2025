﻿namespace ASPWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // HttpClient 등록
            builder.Services.AddHttpClient("PythonAiServer", client =>
            {
                client.BaseAddress = new Uri("http://localhost:8000");
            });

            // CORS 허용(로컬테스트용) -> 살제 운영시는 정확하게 설정할 것
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });

            builder.Services.AddControllers();
            var app = builder.Build();

            app.UseCors();
            app.UseDefaultFiles();          // index.html 처리
            app.UseStaticFiles();           // wwwroot 폴더 아래 파일 사용 설정
            app.MapControllers();           /// MVC 중 Controller 매핑
            app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}
