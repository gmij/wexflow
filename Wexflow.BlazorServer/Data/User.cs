namespace Wexflow.BlazorServer.Data
{
    public enum UserProfile
    {
        SuperAdministrator,
        Administrator,
        Restricted
    }

    public class User
    {

        public string Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public UserProfile UserProfile { get; set; }

        public string Email { get; set; }

        //public double CreatedOn { get; set; }
        public string CreatedOn { get; set; }

        //public double ModifiedOn { get; set; }
        public string ModifiedOn { get; set; }

        internal string GetDbId()
        {
            return "-1";
        }
    }
}
