[![NuGet Version](http://img.shields.io/nuget/v/GeekLearning.Testavior.svg?style=flat-square&label=NuGet:%20Testavior)](https://www.nuget.org/packages/GeekLearning.Testavior/)
[![NuGet Version](http://img.shields.io/nuget/v/GeekLearning.Testavior.Configuration.svg?style=flat-square&label=NuGet:%20Testavior.Configuration)](https://www.nuget.org/packages/GeekLearning.Testavior.Configuration/)
[![Build Status](https://geeklearning.visualstudio.com/_apis/public/build/definitions/f841b266-7595-4d01-9ee1-4864cf65aa73/62/badge)](#)
# Testavior
*Testavior* is a [lightweight solution](https://www.nuget.org/packages?q=Testavior) to help  you develop **Behavior** Tests for **ASP.NET Core**.  

>Behavior Tests are a way of testing your application features applying different types of behaviors to cover a **functional scenario**.  

It provides a simple and efficient approach to write automated tests for your ASP.NET Core application.  
For more information about *Behavior Testing* with ASP.NET Core, please take a look here http://geeklearning.io/a-different-approach-to-test-your-asp-net-core-application  

### Features
*Testavior* provides 2 libraries:
* **Testavior.Configuration**: Helps you configure your application to easily integrate behavior tests for your scenarios.
* **Testavior**: Provides a featured and highly configurable test environment for your behavior tests:
  * Configured Test WebHost
  * Configured authentication context
    * Test authentication middleware 
    * Configurable test identity
    * Identity claims helper
  * Configured Entity Framework Core context using SQLite provider 
  * Serialization helper to handle URL encoded content 
  * Set of HTTP tools to handle *CSRF* protection (very useful to test edition scenarios)
  * Assertion helper

### Installation
On your ASP.NET Core project
* Install the **GeekLearning.Testavior.Configuration** nuget package
  ```
  > dotnet add package GeekLearning.Testavior.Configuration
  ```

On your .NET Core Unit Test project
* Install the **GeekLearning.Testavior** nuget package
  ```
  > dotnet add package GeekLearning.Testavior
  ```
* Add your ASP.NET Core web project as a project reference
### Configuration
The Test environment provided by *Testavior* is based on a **Startup Configuration Service** that let you separate the **Production** environment configuration from the **Test** environment configuration.
This configuration service is represented by a contract `IStartupConfigurationService` which define 3 methods: `Configure` - `ConfigureEnvironment -  ConfigureService` that have to be called within the **Startup Routine** to inject environment dependent configuration.  

1 - In your **ASP.NET Core** project:
* Add a `StartupConfigurationService` class (change name if you wish) to your web project.
* Implement the `IStartupConfigurationService` interface (optionally, inherit from `DefaultStartupConfigurationService` to use the default empty implementation)
* Implement the configuration specific to the Production environment and which must not be executed in the Test environment:
  * `ConfigureServices`: implement the configuration options that are specific to the Production environment
  * `Configure`: implement the *middleware* configuration specific to the Production environment
  * `ConfigureEnvironment`: implement what has to be executed before anything

 Sample:
 ```csharp
 public class StartupConfigurationService : DefaultStartupConfigurationService
{
    public override void ConfigureServices(IServiceCollection services, IConfigurationRoot configuration)
    {
        base.ConfigureServices(services, configuration);

        var connection = "CONNECTION_STRING";
            
        services.AddDbContext<[EF_DB_CONTEXT]>(options =>
            options.UseSqlServer(connection));
    }
}
 ```
 
 2 - In your **Program** class:  
 Inject your `StartupConfigurationService` by calling the `ConfigureStartup` method on your `WebHostBuilder`:
 ```csharp
 new WebHostBuilder()
    ...
    .UseStartup<Startup>()
    .ConfigureStartup<StartupConfigurationService>()
 ```

 3 - In your `Startup` class:
 * Inject the `IStartupConfigurationService` interface into the `Startup` class
 * Call the `ConfigureEnvironment` method at the end of the `Startup` constructor
 * Call the `ConfigureServices` method at the end of the original `Startup.ConfigureServices` method
 * Call the `Configure` method at the beginning of the original `Startup.Configure` method
 
 Sample:
 ```csharp
public class Startup
{
    private IStartupConfigurationService externalStartupConfiguration;

    public Startup(IHostingEnvironment env, IStartupConfigurationService externalStartupConfiguration = null)
    {
        this.externalStartupConfiguration = externalStartupConfiguration;
        this.externalStartupConfiguration.ConfigureEnvironment(env);
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc()

        // Pass configuration (IConfigurationRoot) to the configuration service if needed
        this.externalStartupConfiguration.ConfigureServices(services, null);
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
        this.externalStartupConfiguration.Configure(app, env, loggerFactory);

        app.UseMvc();
    }
}
```

4 - In your test project file:  
The **Razor** engine uses dependency files (.deps.json) to resolve some references at runtime. So in order to test the **MVC** part of a application, it is necessary to import these files. To do it, add the following section to your `.csproj`:
```xml
<Target Name="CopyDepsFiles" AfterTargets="Build" Condition="'$(TargetFramework)'!=''">
    <ItemGroup>
      <DepsFilePaths Include="$([System.IO.Path]::ChangeExtension('%(_ResolvedProjectReferencePaths.FullPath)', '.deps.json'))" />
    </ItemGroup>

    <Copy SourceFiles="%(DepsFilePaths.FullPath)" DestinationFolder="$(OutputPath)" Condition="Exists('%(DepsFilePaths.FullPath)')" />
</Target>
```

5 - For **xUnit** users  
If you intend to use xUnit, first follow the [official documention](https://xunit.github.io/docs/getting-started-dotnet-core), then add a `xunit.runner.json` file to your test project:
```json
{
  "shadowCopy": false
}
```
and add the following section to your `.csproj`:
```xml
<ItemGroup>
  <None Include="xunit.runner.json" CopyToOutputDirectory="PreserveNewest" />
</ItemGroup>
```

### Writing Tests
A specific `IStartupConfigurationService` is required for the **Test** environment if you want to implement **Test Specific** configuration.  
*Testavior* comes with a test specific `IStartupConfigurationService` implementation: `TestStartupConfigurationService` which provide a **Test Environment** full of useful features (see **Features** section).  
Of course you can implement your own Startup configuration service (by using the onboard `TestStartupConfigurationService` or not).  

To create a *Test Environment*, just instanciate the `TestEnvironment` class by passing it your ASP.NET Core application `Startup`, your `IStartupConfigurationService` implementation, the type of your EF Core ObjectContext and the relative path to your ASP.NET Core project (required to resolve MVC views).
```csharp
var testEnvironment = new TestEnvironment<Startup, TestStartupConfigurationService<[EF_DB_CONTEXT]>>(
    Path.Combine(System.AppContext.BaseDirectory, @"[PATH_TO_WEB_APP]"));
```

#### API Test
Write your API test by just sending web requests using the *Test Environment*:
```csharp
[TestMethod]
public void ScenarioShouldBeOk()
{
    var testEnvironment = new TestEnvironment<Startup, TestStartupConfigurationService<[EF_DB_CONTEXT]>>(
       Path.Combine(System.AppContext.BaseDirectory, @"[PATH_TO_WEB_APP]"));

    var response = testEnvironment.Client.GetAsync("/api/data").Result;
    response.EnsureSuccessStatusCode();

    // Test result content
    var result = JsonConvert.DeserializeObject<Data[]>(response.Content.ReadAsStringAsync().Result);

    Assert.AreEqual("data", result.Data);
}
```

#### MVC Test
Write a MVC test is almost as easy as testing an API except that you might want to test the **Model** returned by the server and not the **View**.  
To do that, *Testavior* provides a **ViewModel Repository** that will intercept and store the view's models returned by the server.

You can access to the this repository using the ASP.NET Core dependency injection mechanism:

```csharp
[TestMethod]
public void ScenarioShouldBeOk()
{
    var testEnvironment = new TestEnvironment<Startup, TestStartupConfigurationService<[EF_DB_CONTEXT]>>(
       Path.Combine(System.AppContext.BaseDirectory, @"[PATH_TO_WEB_APP]"));

    testEnvironment.Client.GetAsync("/").Result.EnsureSuccessStatusCode();

    var viewModel = testEnvironment
                        .ServiceProvider
                        .GetRequiredService<ViewModelRepository>()
                        .Get<[VIEWMODEL_TYPE]>();

    Assert.AreEqual("data", viewModel.Data);
}
```

And feel free to take a look at the [Samples](https://github.com/geeklearningio/gl-dotnet-test-integration/tree/develop/sample) section ;)

Happy testing ! :)