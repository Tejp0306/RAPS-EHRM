using EHRM.Infrastructure.Models;
using Logger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.Infrastructure.Extension
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILoggerManager logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature != null)
                    {
                        string rootLogFolder = "Logs";
                        string monthFolderName = DateTime.Now.ToString("yyyy-MM");
                        string logFilePath = Path.Combine(rootLogFolder, monthFolderName, "error.log");

                        // Ensure the directory for the log file exists
                        Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));

                        using (StreamWriter writer = new StreamWriter(logFilePath, true))
                        {
                            writer.WriteLine();
                            writer.WriteLine($"Date and Time: {DateTime.Now}");
                            writer.WriteLine($"Error: {contextFeature.Error}");
                            writer.WriteLine($"Path: {contextFeature.Path}");
                            writer.WriteLine("******************************************************************");
                        }

                        var acceptHeader = context.Request.Headers["Accept"].ToString();

                        if (acceptHeader.Contains("application/json"))
                        {
                            context.Response.ContentType = "application/json";
                            await context.Response.WriteAsync(new ErrorDetails()
                            {
                                StatusCode = context.Response.StatusCode,
                                Message = "Internal Server Error"
                            }.ToString());
                        }
                        else
                        {
                            context.Response.ContentType = "text/html";
                            var errorPageHtml = @"
                    <!DOCTYPE html>
                    <html lang='en'>
                    <head>
                        <meta charset='UTF-8'>
                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                        <title>Error</title>
                        <style>
                            body {
                                font-family: Arial, sans-serif;
                                background-color: #f8f9fa;
                                color: #343a40;
                                text-align: center;
                                padding: 50px;
                            }
                            .container {
                                max-width: 500px;
                                margin: auto;
                                background: #fff;
                                padding: 20px;
                                box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                                border-radius: 8px;
                            }
                            h1 {
                                color: #dc3545;
                            }
                            a {
                                display: inline-block;
                                margin-top: 20px;
                                padding: 10px 20px;
                                background-color: #007bff;
                                color: #fff;
                                text-decoration: none;
                                border-radius: 5px;
                            }
                            a:hover {
                                background-color: #0056b3;
                            }
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <h1>Oops! Something went wrong.</h1>
                            <p>An unexpected error occurred. Please try again later.</p>
                            <a href='/Account/Login'>Go to Login Page</a>
                        </div>
                    </body>
                    </html>";
                            await context.Response.WriteAsync(errorPageHtml);
                        }
                    }
                });
            });
        }

        //public static void ConfigureExceptionHandler(this IApplicationBuilder app,  ILoggerManager logger)
        // {
        //     app.UseExceptionHandler(appError =>
        //     {
        //         appError.Run(async context =>
        //         {
        //             context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //             context.Response.ContentType = "application/json";

        //             var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
        //             if (contextFeature != null)
        //             {
        //                 string rootLogFolder = "Logs"; // Change this to your desired root log folder path
        //                 string monthFolderName = DateTime.Now.ToString("yyyy-MM"); // Get the current year and month as "yyyy-MM" format
        //                 string logFilePath = Path.Combine(rootLogFolder, monthFolderName, "error.log");

        //                 // Ensure the directory for the log file exists
        //                 Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));

        //                 using (StreamWriter writer = new StreamWriter(logFilePath, true))
        //                 {
        //                     writer.WriteLine(); // 
        //                     writer.WriteLine($"Date and Time: {DateTime.Now}");
        //                     writer.WriteLine($"Error: {contextFeature.Error}");
        //                     writer.WriteLine($"Path: {contextFeature.Path}");
        //                     writer.WriteLine($"Endpoint: {contextFeature.Endpoint}");
        //                     writer.WriteLine("******************************************************************"); // 
        //                 }

        //                 await context.Response.WriteAsync(new ErrorDetails()
        //                 {
        //                     StatusCode = context.Response.StatusCode,
        //                     Message = "Internal Server Error",
        //                 }.ToString());
        //             }
        //         });
        //     });
        // }

    }
}
