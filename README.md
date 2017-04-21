# GeekLearning.SceneTest
*SceneTest* is a set of [NuGet libraries]() to help  you develop **Behavior** Tests for **ASP.NET Core**.  

>Behavior Tests are a way of testing your application features applying different types of behaviors to cover a **functional scenario**.  

It provides a simple and efficient approach to write automated tests for your ASP.NET Core application.  
For more information about *Behavior Testing* with ASP.NET Core, please take a look here http://geeklearning.io/a-different-approach-to-test-your-asp-net-core-application  

### Features
[NAME] provides 2 libraries:
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
* Add your ASP.NET Core project as a project reference
### Configuration
The Test environment provided by *SceneTest* is based on a Startup configuration service that let you separate the **Production** environment configuration from the **Test** environment







### Writing Tests
 
