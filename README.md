System.AggregateException: One or more errors occurred. (Unable to load DLL 'libwkhtmltox' or one of its dependencies: Не найден указанный модуль. (0x8007007E))
 ---> System.DllNotFoundException: Unable to load DLL 'libwkhtmltox' or one of its dependencies: Не найден указанный модуль. (0x8007007E)
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
   at DocCreator.Services.DrawDocumentService`1.DrawDocument(String model, String typeName) in D:\source\repos\GiteaCsharp\DocCreator\Services\DrawDocumentService.cs:line 25
   at DocCreator.Services.DocCreatorService.GenerateDocument(String model, String objectType) in D:\source\repos\GiteaCsharp\DocCreator\Services\DocCreatorService.cs:line 21
   at DocCreator.Controllers.DocCreatorController.GenerateDocument(String objectType, JsonElement body) in D:\source\repos\GiteaCsharp\DocCreator\Controllers\DocCreatorController.cs:line 27
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
   at DocCreator.Extensions.CustomMiddlewares.ApiKeyMiddleware.InvokeAsync(HttpContext context) in D:\source\repos\GiteaCsharp\DocCreator\Extensions\CustomMiddlewares\ApiKeyMiddleware.cs:line 33
   at Swashbuckle.AspNetCore.SwaggerUI.SwaggerUIMiddleware.Invoke(HttpContext httpContext)
   at Swashbuckle.AspNetCore.Swagger.SwaggerMiddleware.Invoke(HttpContext httpContext, ISwaggerProvider swaggerProvider)
   at Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddleware.Invoke(HttpContext context)
