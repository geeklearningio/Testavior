namespace GeekLearning.Testavior.Mvc
{
    using System;
    using System.Collections.Concurrent;

    public class ViewModelRepository
    {
        private readonly ConcurrentDictionary<Type, object> repository = new ConcurrentDictionary<Type, object>();

        public void Add<TModel>(TModel model) where TModel : class
        {
            this.repository.AddOrUpdate(model.GetType(), model, (t, o) => model);            
        } 

        public TModel Get<TModel>() where TModel : class
        {
			this.repository.TryGetValue(typeof(TModel), out object value);

			return value as TModel;
        }
    }
}
