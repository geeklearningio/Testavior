# Testavior
*Testavior* is a set of [NuGet libraries]() to help  you develop **Behavior** Tests for **ASP.NET Core**.  

>Behavior Tests are a way of testing your application features applying different types of behaviors to cover a **functional scenario**.  

It provides a simple and efficient approach to write automated tests for your ASP.NET Core application.  
For more information about *Behavior Testing* with ASP.NET Core, please take a look here http://geeklearning.io/a-different-approach-to-test-your-asp-net-core-application  

### Features
*Testavior* provides 2 libraries:
* **Configuration**: Helps you configure your application to easily integrate behavior tests for your scenarios.
* **Integration**: Provides a featured and highly configurable test environment for your behavior tests:
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
* Install the **GeekLearning.SceneTest.Configuration** nuget package
  ```
  > dotnet add package GeekLearning.SceneTest.Configuration
  ```

On your .NET Core Unit Test project
* Install the **GeekLearning.SceneTest** nuget package
  ```
  > dotnet add package GeekLearning.SceneTest
  ```
* Add your ASP.NET Core web project as a project reference
### Configuration
The Test environment provided by *SceneTest* is based on a **Startup Configuration Service** that let you separate the **Production** environment configuration from the **Test** environment configuration.
This configuration service is represented by a contract **IStartupConfigurationService** which define 3 methods: *Configure, ConfigureEnvironment, ConfigureService* that have to be called within the **Startup Routine** to inject environment dependent configuration.  

1 - In your **ASP.NET Core** project:
* Add a *StartupConfigurationService* class (change name if you wish) to your web project.
* Implement the **IStartupConfigurationService** interface (optionally, inherit from *DefaultStartupConfigurationService* to use the default empty implementation)
* Implement the configuration specific to the Production environment and which must not be executed in the Test environment:
  * *ConfigureServices*: implement the configuration options that are specific to the Production environment
  * *Configure*: implement the *middleware* configuration specific to the Production environment
  * *ConfigureEnvironment*: implement what has to be executed before anything

 Sample:
 ```csharp
 public class StartupConfigurationService : DefaultStartupConfigurationService
{
    public override void ConfigureServices(IServiceCollection services, IConfigurationRoot configuration)
    {
        base.ConfigureServices(services, configuration);

        var connection = "CONNECTION_STRING";
            
        services.AddDbContext<Data.BloggingContext>(options =>
            options.UseSqlServer(connection));
    }
}
 ```
 
 2 - In your **Program** class:  
 Inject your *StartupConfigurationService* by calling the **ConfigureStartup** method on your **WebHostBuilder**:
 ```csharp
 new WebHostBuilder()
    ...
    .UseStartup<Startup>()
    .ConfigureStartup<StartupConfigurationService>()
 ```

 3 - In your **Startup** class:
 * Inject the *IStartupConfigurationService* interface into the Startup class
 * Call the *ConfigureEnvironment* method at the end of the Startup constructor
 * Call the *ConfigureServices* method at the end of the original Startup ConfigureServices method
 * Call the *Configure* method at the beginning of the original Startup Configure method
 
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


### Writing Tests
A specific *IStartupConfigurationService* is required for the **Test** environment if you want to implement **Test Specific** configuration.  
*Testavior* comes with a test specific *IStartupConfigurationService* implementation: **TestStartupConfigurationService** which provide a **Test Environment** full of useful features (see **Features** section).  
Of course you can implement your own *Test StartupConfigurationService* (by using the onboard TestStartupConfigurationService or not).  
To create a *Test Environment*, just instanciate the **TestEnvironment** class by passing it your ASP.NET Core application *Startup*, your *IStartupConfigurationService* implementation and the type of your EF Core ObjectContext
```csharp
var testEnvironment = new TestEnvironment<Startup, TestStartupConfigurationService<[EF_DB_CONTEXT]>>(
    Path.Combine(System.AppContext.BaseDirectory, @"[PATH_TO_WEB_APP]"));
```
Then you can write your test by just sending web requests using the *Test Environment*:
```csharp
var response = testEnvironment.Client.GetAsync("/api/data").Result;
response.EnsureSuccessStatusCode();

// test the response.Content
// ...
```
