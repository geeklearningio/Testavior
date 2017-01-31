namespace GeekLearning.Test.Integration.Mvc
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class SaveViewModelResultFilter : IResultFilter
    {
        private ViewModelRepository modelRepository;

        public SaveViewModelResultFilter(ViewModelRepository modelRepository)
        {
            this.modelRepository = modelRepository;
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            object model = null;
            var viewResult = context.Result as ViewResult;
            if (viewResult != null)
            {
                model = viewResult.Model;
            }
            else
            {
                PartialViewResult partialViewResult = context.Result as PartialViewResult;
                if (partialViewResult != null)
                {
                    model = partialViewResult.ViewData.Model;                    
                }
            }

            if (model != null)
            {
                this.modelRepository.Add(model);
            }
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
        }
    }
}
