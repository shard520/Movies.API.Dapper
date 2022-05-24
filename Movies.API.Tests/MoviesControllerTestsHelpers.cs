using Microsoft.AspNetCore.Mvc;

namespace Movies.API.Tests
{
    internal static class MoviesControllerTestsHelpers
    {

        public static T GetObjectResult<T>(this ActionResult<T> result)
        {
            if (result.Result != null)
                return (T)((ObjectResult)result.Result).Value;
            return result.Value;
        }
    }
}