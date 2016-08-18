namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class AssertHelper
    {
        public static void AreEqual<TEntity>(this Assert assert, TEntity expected, TEntity actual, params string[] ignoredProperties)
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
                    Assert.Fail("Property {0}.{1} does not match. Expected: {2} but was: {3}", new object[]
                    {
                        property.DeclaringType.Name,
                        property.Name,
                        expectedValue,
                        actualValue
                    });
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
        public static void AreEqual<TEntity>(this Assert assert, IEnumerable<TEntity> expected, IEnumerable<TEntity> actual, params string[] ignoredProperties)
        {
            Assert.AreEqual<int>((expected != null) ? expected.Count<TEntity>() : 0, (actual != null) ? actual.Count<TEntity>() : 0);
            if ((expected == null || expected.Count<TEntity>() == 0) && (actual == null || actual.Count<TEntity>() == 0))
            {
                return;
            }

            List<TEntity> expectedList = expected.ToList<TEntity>();
            List<TEntity> actualList = actual.ToList<TEntity>();
            for (int i = 0; i < expectedList.Count; i++)
            {
                AreEqual<TEntity>(assert, expectedList[i], actualList[i], ignoredProperties);
            }
        }
    }
}
