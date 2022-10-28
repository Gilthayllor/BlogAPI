using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BlogAPI.Extensions
{
    public static class ModelStateExtension
    {
        public static List<string> GetErrors(this ModelStateDictionary modelState)
        {
            var errors = new List<string>();
            
            foreach(var val in modelState.Values)
            {
                errors.AddRange(val.Errors.Select(x => x.ErrorMessage));
            }

            return errors;  
        }
    }
}
