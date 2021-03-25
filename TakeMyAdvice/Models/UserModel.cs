using System;
using Realms;

namespace TakeMyAdvice.Models
{
    public class UserModel : RealmObject
    {
        public UserModel()
        {

        }

        public UserModel(string firstName, string lastName, string email, string userName, string password) : base()
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            UserName = userName;
            Password = password;
        }

        public bool IsActive { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        [PrimaryKey]
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
