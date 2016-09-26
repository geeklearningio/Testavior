namespace GeekLearning.Test.Integration.Mvc
{
    using System;
    using System.Collections.Concurrent;

    public class ViewModelRepository
    {
        private readonly ConcurrentDictionary<Type, object> repository = new ConcurrentDictionary<Type, object>();

        public void Add<TModel>(TModel model) where TModel : class
        {
            if (!this.repository.TryAdd(model.GetType(), model))
            {
                throw new ArgumentException($"The model {model.GetType().Name} is already registered");
            }
        }

        public TModel Get<TModel>() where TModel : class
        {
            object value;
            this.repository.TryGetValue(typeof(TModel), out value);

            return value as TModel;
        }
    }
}
