Oracle.ManagedDataAccess.Client.OracleException (0x80004005): ORA-06502: PL/SQL:  ошибка числа или значения
ORA-06512: на  "CARDCOLV.P_REFERENCE_FORSERVICE10", line 47
ORA-06512: на  line 1
https://docs.oracle.com/error-help/db/ora-06502/
   at OracleInternal.ServiceObjects.OracleConnectionImpl.VerifyExecution(Int32& cursorId, Boolean bThrowArrayBindRelatedErrors, SqlStatementType sqlStatementType, Int32 arrayBindCount, OracleException& exceptionForArrayBindDML, Boolean& hasMoreRowsInDB, Boolean bFirstIterationDone)
   at OracleInternal.ServiceObjects.OracleCommandImpl.VerifyExecution(OracleConnectionImpl connectionImpl, Int32& cursorId, Boolean bThrowArrayBindRelatedErrors, OracleException& exceptionForArrayBindDML, Boolean& hasMoreRowsInDB, Boolean bFirstIterationDone)
   at OracleInternal.ServiceObjects.OracleCommandImpl.ExecuteNonQueryAsync(String commandText, OracleParameterCollection paramColl, CommandType commandType, OracleConnectionImpl connectionImpl, Int32 longFetchSize, Int64 clientInitialLOBFS, OracleDependencyImpl orclDependencyImpl, OracleConnection connection, ENQ_RefAndOutParamArgCtx enq_refOutArgCtx, Boolean isFromEF, Boolean bAsync)
   at Oracle.ManagedDataAccess.Client.OracleCommand.ExecuteNonQueryInternalAsync(Boolean bAsync, CancellationToken cancellationToken)
   at Oracle.ManagedDataAccess.Client.OracleCommand.ExecuteNonQueryAsyncHelper(CancellationToken cancellationToken)
   at Dapper.SqlMapper.ExecuteImplAsync(IDbConnection cnn, CommandDefinition command, Object param) in /_/Dapper/SqlMapper.Async.cs:line 662
   at UserInfoService.Repositories.UserRepository.GetBranchInfoAsync(String login, Int32 level) in C:\Users\00055864\source\repos\UserInfoService\Repositories\UserRepository.cs:line 52
   at UserInfoService.Services.UserService.GetBranchInfoAsync(String login, String level) in C:\Users\00055864\source\repos\UserInfoService\Services\UserService.cs:line 28
   at UserInfoService.Controllers.UsersController.GetBranchInfo(String login, String level) in C:\Users\00055864\source\repos\UserInfoService\Controllers\UsersController.cs:line 29
   at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.TaskOfIActionResultExecutor.Execute(ActionContext actionContext, IActionResultTypeMapper mapper, ObjectMethodExecutor executor, Object controller, Object[] arguments)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeActionMethodAsync>g__Awaited|12_0(ControllerActionInvoker invoker, ValueTask`1 actionResultValueTask)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeNextActionFilterAsync>g__Awaited|10_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeInnerFilterAsync>g__Awaited|13_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeFilterPipelineAsync>g__Awaited|20_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Logged|17_1(ResourceInvoker invoker)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Logged|17_1(ResourceInvoker invoker)
   at Microsoft.AspNetCore.Routing.EndpointMiddleware.<Invoke>g__AwaitRequestTask|7_0(Endpoint endpoint, Task requestTask, ILogger logger)
   at Swashbuckle.AspNetCore.SwaggerUI.SwaggerUIMiddleware.Invoke(HttpContext httpContext)
   at Swashbuckle.AspNetCore.Swagger.SwaggerMiddleware.Invoke(HttpContext httpContext, ISwaggerProvider swaggerProvider)
   at Microsoft.AspNetCore.Authorization.AuthorizationMiddleware.Invoke(HttpContext context)
   at Microsoft.AspNetCore.Authentication.AuthenticationMiddleware.Invoke(HttpContext context)
   at Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddlewareImpl.Invoke(HttpContext context)
