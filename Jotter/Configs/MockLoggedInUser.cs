using Jotter.Models;

namespace Jotter.Configs
{
    public static class MockLoggedInUser
    {
        private static User loggedInUser;


        public static User LogIn() 
        {
            loggedInUser = new User { Id = "1", Email = "GuestUser@gmail.com", UserName = "GuestUser" };
            return loggedInUser;
        }


        public static string getUserId() 
        {
            return loggedInUser.Id;
        }
    } 
}
