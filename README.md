System.InvalidOperationException: Unable to resolve service for type 'DinkToPdf.BasicConverter' while attempting to activate 'DocCreator.Services.DrawDocumentService`1[DocCreator.Models.Document]'.
   at Microsoft.Extensions.DependencyInjection.ServiceLookup.CallSiteFactory.CreateArgumentCallSites(ServiceIdentifier serviceIdentifier, Type implementationType, CallSiteChain callSiteChain, ParameterInfo[] parameters, Boolean throwIfCallSiteNotFound)
   at Microsoft.Extensions.DependencyInjection.ServiceLookup.CallSiteFactory.CreateConstructorCallSite(ResultCache lifetime, ServiceIdentifier serviceIdentifier, Type implementationType, CallSiteChain callSiteChain)
   at Microsoft.Extensions.DependencyInjection.ServiceLookup.CallSiteFactory.TryCreateOpenGeneric(ServiceDescriptor descriptor, ServiceIdentifier serviceIdentifier, CallSiteChain callSiteChain, Int32 slot, Boolean throwOnConstraintViolation)
   at Microsoft.Extensions.DependencyInjection.ServiceLookup.CallSiteFactory.TryCreateOpenGeneric(ServiceIdentifier serviceIdentifier, CallSiteChain callSiteChain)
   at Microsoft.Extensions.DependencyInjection.ServiceLookup.CallSiteFactory.CreateCallSite(ServiceIdentifier serviceIdentifier, CallSiteChain callSiteChain)
   at Microsoft.Extensions.DependencyInjection.ServiceProvider.CreateServiceAccessor(ServiceIdentifier serviceIdentifier)
   at System.Collections.Concurrent.ConcurrentDictionary`2.GetOrAdd(TKey key, Func`2 valueFactory)
   at Microsoft.Extensions.DependencyInjection.ServiceProvider.GetService(ServiceIdentifier serviceIdentifier, ServiceProviderEngineScope serviceProviderEngineScope)
   at Microsoft.Extensions.DependencyInjection.ServiceLookup.ServiceProviderEngineScope.GetService(Type serviceType)
   at DocCreator.Services.Abstract.DrawDocumentServiceFactory.Create(String typeName) in D:\source\repos\GiteaCsharp\DocCreator\Services\DrawDocumentServiceFactory.cs:line 20
   at DocCreator.Services.DocCreatorService.GenerateDocument(String model, String objectType) in D:\source\repos\GiteaCsharp\DocCreator\Services\DocCreatorService.cs:line 19
   at DocCreator.Controllers.DocCreatorController.GenerateDocument(String objectType, JsonElement body) in D:\source\repos\GiteaCsharp\DocCreator\Controllers\DocCreatorController.cs:line 27
   at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.TaskOfIActionResultExecutor.Execute(IActionResultTypeMapper mapper, ObjectMethodExecutor executor, Object controller, Object[] arguments)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeActionMethodAsync>g__Awaited|12_0(ControllerActionInvoker invoker, ValueTask`1 actionResultValueTask)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeNextActionFilterAsync>g__Awaited|10_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeInnerFilterAsync()
--- End of stack trace from previous location ---
   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeFilterPipelineAsync>g__Awaited|20_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Logged|17_1(ResourceInvoker invoker)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Logged|17_1(ResourceInvoker invoker)
   at Microsoft.AspNetCore.Routing.EndpointMiddleware.<Invoke>g__AwaitRequestTask|6_0(Endpoint endpoint, Task requestTask, ILogger logger)
   at DocCreator.Extensions.CustomMiddlewares.ApiKeyMiddleware.InvokeAsync(HttpContext context) in D:\source\repos\GiteaCsharp\DocCreator\Extensions\CustomMiddlewares\ApiKeyMiddleware.cs:line 33
   at Swashbuckle.AspNetCore.SwaggerUI.SwaggerUIMiddleware.Invoke(HttpContext httpContext)
   at Swashbuckle.AspNetCore.Swagger.SwaggerMiddleware.Invoke(HttpContext httpContext, ISwaggerProvider swaggerProvider)
   at Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddleware.Invoke(HttpContext context)


        public async Task<object> Create(string typeName) 
        {
            Type type = Type.GetType($"{Assembly.GetExecutingAssembly().GetName().Name}.Models.{typeName}");
            if (type == null) 
            {
                throw new ArgumentNullException(nameof(typeName));
            }
            Type genericType = typeof(IDrawDocumentService<>).MakeGenericType(type); 
            return _serviceProvider.GetService(genericType);
        }
                public async Task<byte[]> GenerateDocument(string model, string objectType) 
        {
            var drawDocumentService = await _factory.Create(objectType);
            var method = drawDocumentService.GetType().GetMethod("DrawDocument");
            return await (Task<byte[]>)method.Invoke(drawDocumentService, new object[] { model, objectType });
            //return await _uploaderService.UploadDocument(docBytes);
        }
