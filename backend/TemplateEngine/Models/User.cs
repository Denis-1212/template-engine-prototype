using NPetrovich;

namespace TemplateEngine.Models
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public Gender Gender { get; set; }
        public bool AutoDetectGender { get; set; } = true;

        public User(string firstName, string lastName, string middleName, Gender? gender = null)
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            if (gender.HasValue)
            {
                Gender = gender.Value;
                AutoDetectGender = false;
            }
        }
    }
}
