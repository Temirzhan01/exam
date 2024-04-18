System.AggregateException
  HResult=0x80131500
  Message=Some services are not able to be constructed (Error while validating the service descriptor 'ServiceType: Microsoft.Extensions.Hosting.IHostedService Lifetime: Singleton ImplementationType: LegalCashOperationsWorker.Worker': Unable to resolve service for type 'System.String' while attempting to activate 'LegalCashOperationsWorker.Worker'.)
  Source=Microsoft.Extensions.DependencyInjection
  StackTrace:
   at Microsoft.Extensions.DependencyInjection.ServiceProvider..ctor(ICollection`1 serviceDescriptors, ServiceProviderOptions options)
   at Microsoft.Extensions.DependencyInjection.ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(IServiceCollection services, ServiceProviderOptions options)
   at Microsoft.Extensions.Hosting.Internal.ServiceFactoryAdapter`1.CreateServiceProvider(Object containerBuilder)
   at Microsoft.Extensions.Hosting.HostBuilder.CreateServiceProvider()
   at Microsoft.Extensions.Hosting.HostBuilder.Build()
   at Program.<<Main>$>d__0.MoveNext() in D:\source\repos\CustomServices\LegalCashOperationsWorker\Program.cs:line 15

  This exception was originally thrown at this call stack:
    [External Code]

Inner Exception 1:
InvalidOperationException: Error while validating the service descriptor 'ServiceType: Microsoft.Extensions.Hosting.IHostedService Lifetime: Singleton ImplementationType: LegalCashOperationsWorker.Worker': Unable to resolve service for type 'System.String' while attempting to activate 'LegalCashOperationsWorker.Worker'.

Inner Exception 2:
InvalidOperationException: Unable to resolve service for type 'System.String' while attempting to activate 'LegalCashOperationsWorker.Worker'.
