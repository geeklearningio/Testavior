namespace GeekLearning.Test.Integration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class AssertHelper
    {
        public static void IsEqual<TEntity>(this TEntity actual, TEntity expected, params string[] ignoredProperties)
            where TEntity : class
        {
            PropertyInfo[] properties = expected.GetType().GetProperties();
            if (ignoredProperties != null)
            {
                properties = (from p in properties
                              where !ignoredProperties.Contains(p.Name)
                              select p).ToArray<PropertyInfo>();
            }

            PropertyInfo[] array = properties;
            for (int i = 0; i < array.Length; i++)
            {
                PropertyInfo property = array[i];
                object expectedValue = property.GetValue(expected, null);
                object actualValue = property.GetValue(actual, null);
                if (!object.Equals(expectedValue, actualValue))
                {
                    throw new AssertionException($"Property {property.DeclaringType.Name}.{property.Name} does not match. Expected: {expectedValue} but was: {actualValue}");
                }
            }
        }

        /// <summary>
        /// Asserts the lists are equal.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="expected">The expected.</param>
        /// <param name="actual">The actual.</param>
        /// <param name="ignoredProperties">The ignored properties.</param>
        public static void AreEqual<TEntity>(IEnumerable<TEntity> expected, IEnumerable<TEntity> actual, params string[] ignoredProperties)
            where TEntity : class
        {
            if (expected?.Count() != actual?.Count())
            {
                throw new AssertionException($"Expected and actual list count are not equal");
            }

            if ((expected == null || expected.Count<TEntity>() == 0) && (actual == null || actual.Count<TEntity>() == 0))
            {
                return;
            }
      
            List<TEntity> expectedList = expected.ToList<TEntity>();
            List<TEntity> actualList = actual.ToList<TEntity>();
            for (int i = 0; i < expectedList.Count; i++)
            {
                actualList[i].IsEqual(expectedList[i], ignoredProperties);
            }
        }
    }

    public class AssertionException : Exception
    {
        public AssertionException() {}

        public AssertionException(string messsage) : base(messsage) {}
    }
}
