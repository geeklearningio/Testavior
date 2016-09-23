namespace GeekLearning.Test.Integration.Mvc
{
    using System;
    using System.Collections.Concurrent;

    public class ViewModelRepository
    {
        private readonly ConcurrentDictionary<Type, object> repository = new ConcurrentDictionary<Type, object>();

        public void Add<TModel>(TModel model) where TModel : class
        {
            if (!this.repository.TryAdd(typeof(TModel), model))
            {
                throw new ArgumentException($"The model {typeof(TModel).Name} is already registered");
            }
        }

        internal TModel Get<TModel>() where TModel : class
        {
            object value;
            this.repository.TryGetValue(typeof(TModel), out value);

            return value as TModel;
        }
    }
}
