System.Exception
  HResult=0x80131500
  Message=Исключение в процессе (см. внутренее исключение)
  Source=HalykBank.WWF
  StackTrace:
   at HalykBank.WWF.WwfManager.WaitAndCatchError() in D:\source\repos\SpmProcesses\SpmProcesses\Src\Core\WWF\WwfManager.cs:line 87
   at HalykBank.WWF.WwfManager.StartAndMakeStep() in D:\source\repos\SpmProcesses\SpmProcesses\Src\Core\WWF\WwfManager.cs:line 31
   at HalykBank.SpmApi.Host.Controllers.ProcessController.Start(String bpVersion, String pAction) in D:\source\repos\SpmProcesses\SpmProcesses\src\core\SpmApi\Controllers\ProcessController.cs:line 60
   at System.Web.Http.Controllers.ReflectedHttpActionDescriptor.ActionExecutor.<>c__DisplayClass10.<GetExecutor>b__9(Object instance, Object[] methodParameters)
   at System.Web.Http.Controllers.ReflectedHttpActionDescriptor.ActionExecutor.Execute(Object instance, Object[] arguments)
   at System.Web.Http.Controllers.ReflectedHttpActionDescriptor.ExecuteAsync(HttpControllerContext controllerContext, IDictionary`2 arguments, CancellationToken cancellationToken)

Inner Exception 1:
DirectoryNotFoundException: Не удалось найти часть пути "e:\IISFiles\Persistent\Sessions\00055864".
