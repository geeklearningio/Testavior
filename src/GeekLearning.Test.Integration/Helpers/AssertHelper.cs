namespace GeekLearning.Test.Integration
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class AssertHelper
    {
        public static void IsEqual<TEntity>(this TEntity actual, TEntity expected, params string[] ignoredProperties)
            where TEntity : class
        {
            var actualType = actual.GetType();
            if (actualType != typeof(string) && actualType.GetInterfaces().Contains(typeof(IEnumerable)))
            {
                IsEqual((IEnumerable)actual, (IEnumerable)expected, ignoredProperties);
                return;
            }

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

                if (expectedValue == null && actualValue == null)
                {
                    continue;
                }

                if (expectedValue == null && actualValue != null || expectedValue != null && actualValue == null)
                {
                    throw new AssertionException($"Property {property.DeclaringType.Name}.{property.Name} does not match. Expected: {expectedValue} but was: {actualValue}");
                }

                var expectedValueType = expectedValue.GetType();
                if ((expectedValueType.IsValueType || expectedValueType == typeof(string)) && !object.Equals(expectedValue, actualValue))
                {
                    throw new AssertionException($"Property {property.DeclaringType.Name}.{property.Name} does not match. Expected: {expectedValue} but was: {actualValue}");
                }
                else if (expectedValueType.GetInterfaces().Contains(typeof(IEnumerable)))
                {
                    IsEqual((IEnumerable)expectedValue, (IEnumerable)actualValue, ignoredProperties);
                }
                else
                {
                    IsEqual(expectedValue, actualValue);
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
        public static void IsEqual(this IEnumerable expected, IEnumerable actual, params string[] ignoredProperties)
        {
            if (expected == null && actual == null)
            {
                return;
            }

            var expectedEnumerator = expected.GetEnumerator();
            var actualEnumerator = actual.GetEnumerator();

            bool expectedEnumeratorMoved = expectedEnumerator.MoveNext();
            bool actualEnumeratorMoved = actualEnumerator.MoveNext();
            while (expectedEnumeratorMoved && actualEnumeratorMoved)
            {
                expectedEnumerator.Current.IsEqual(actualEnumerator.Current, ignoredProperties);
                expectedEnumeratorMoved = expectedEnumerator.MoveNext();
                actualEnumeratorMoved = actualEnumerator.MoveNext();
            }

            if (expectedEnumeratorMoved != actualEnumeratorMoved)
            {
                throw new AssertionException($"Expected and actual list count are not equal");
            }
        }
    }

    public class AssertionException : Exception
    {
        public AssertionException() {}

        public AssertionException(string messsage) : base(messsage) {}
    }
}
