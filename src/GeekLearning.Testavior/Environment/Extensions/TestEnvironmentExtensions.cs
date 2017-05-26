namespace GeekLearning.Testavior.Environment
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System.Collections.Generic;

    public static class TestEnvironmentExtensions
    {
        public static void InsertTestEntities<TEntity, TDbContext>(this ITestEnvironment testEnvironment, IEnumerable<TEntity> entites)
            where TEntity : class
            where TDbContext : DbContext
        {
            using (var serviceScope = testEnvironment.ServiceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<TDbContext>();
                dbContext.Set<TEntity>().AddRange(entites);
                dbContext.SaveChanges();
            }
        }
    }
}
