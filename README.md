System.AggregateException: One or more errors occurred. (Unable to load shared library 'libwkhtmltox' or one of its dependencies. In order to help diagnose loading problems, consider setting the LD_DEBUG environment variable: liblibwkhtmltox: cannot open shared object file: No such file or directory)
 ---> System.DllNotFoundException: Unable to load shared library 'libwkhtmltox' or one of its dependencies. In order to help diagnose loading problems, consider setting the LD_DEBUG environment variable: liblibwkhtmltox: cannot open shared object file: No such file or directory
   at DinkToPdf.WkHtmlToXBindings.wkhtmltopdf_init(Int32 useGraphics)
   at DinkToPdf.PdfTools.Load()
   at DinkToPdf.BasicConverter.Convert(IDocument document)
   at DinkToPdf.SynchronizedConverter.<>n__0(IDocument document)
   at DinkToPdf.SynchronizedConverter.<>c__DisplayClass5_0.<Convert>b__0()
   at System.Threading.Tasks.Task`1.InnerInvoke()
   at System.Threading.Tasks.Task.<>c.<.cctor>b__272_0(Object obj)
   at System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state)
--- End of stack trace from previous location ---
   at System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state)
   at System.Threading.Tasks.Task.ExecuteWithThreadLocal(Task& currentTaskSlot, Thread threadPoolThread)
   --- End of inner exception stack trace ---
   at DinkToPdf.SynchronizedConverter.Invoke[TResult](Func`1 delegate)
   at DinkToPdf.SynchronizedConverter.Convert(IDocument document)
   at DocCreator.Services.DrawDocumentService`1.DrawDocument(String model, String typeName) in /src/Services/DrawDocumentService.cs:line 26
   at DocCreator.Services.DocCreatorService.GenerateDocumentAndSave(String model, String objectType) in /src/Services/DocCreatorService.cs:line 31
   at DocCreator.Controllers.DocCreatorController.GenerateDocument(String objectType, JsonElement body) in /src/Controllers/DocCreatorController.cs:line 39
   at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.TaskOfIActionResultExecutor.Execute(IActionResultTypeMapper mapper, ObjectMethodExecutor executor, Object controller, Object[] arguments)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeActionMethodAsync>g__Awaited|12_0(ControllerActionInvoker invoker, ValueTask`1 actionResultValueTask)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeNextActionFilterAsync>g__Awaited|10_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeInnerFilterAsync>g__Awaited|13_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeFilterPipelineAsync>g__Awaited|20_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Logged|17_1(ResourceInvoker invoker)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Logged|17_1(ResourceInvoker invoker)
   at Microsoft.AspNetCore.Routing.EndpointMiddleware.<Invoke>g__AwaitRequestTask|6_0(Endpoint endpoint, Task requestTask, ILogger logger)
   at DocCreator.Extensions.CustomMiddlewares.ApiKeyMiddleware.InvokeAsync(HttpContext context) in /src/Extensions/CustomMiddlewares/ApiKeyMiddleware.cs:line 33
   at Swashbuckle.AspNetCore.SwaggerUI.SwaggerUIMiddleware.Invoke(HttpContext httpContext)
   at Swashbuckle.AspNetCore.Swagger.SwaggerMiddleware.Invoke(HttpContext httpContext, ISwaggerProvider swaggerProvider)
   at Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddleware.Invoke(HttpContext context)

HEADERS
=======
Accept: */*
Host: halykbpm-dev-core.homebank.kz
User-Agent: PostmanRuntime/7.39.0
Accept-Encoding: gzip, deflate, br
Cache-Control: no-cache
Content-Type: application/json
Content-Length: 2016
X-Request-ID: 55c45d4708446b44b4cf2b5b7e0c4dac
X-Real-IP: 172.30.194.211
X-Forwarded-For: 172.30.194.211
X-Forwarded-Host: halykbpm-dev-core.homebank.kz
X-Forwarded-Port: 443
X-Forwarded-Proto: https
X-Forwarded-Scheme: https
X-Scheme: https
X-Content-Type-Options: nosniff
X-Api-Key: 71534376-07d6-4859-9466-8abfc3231689
Postman-Token: b6d814e3-1953-4a4d-81bb-e3aec9038b27
