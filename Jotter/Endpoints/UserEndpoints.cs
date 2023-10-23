using Jotter.Configs;
using Jotter.Models.DTO;
using System.Net;

namespace Jotter.Endpoints
{
    public static class UserEndpoints
    {
        public static void ConfigureUsers(this WebApplication app)
        {
            app.MapGet("/api/user/login", LogInUser)
                .Produces<ResponseDTO>(200);
        }

        private static IResult LogInUser() 
        {
            ResponseDTO response = new() { IsSuccess = true, StatusCode = HttpStatusCode.OK, Result = MockLoggedInUser.LogIn() };
            return Results.Ok(response);
        }
    }
}
