namespace TimeKeeping4.Model
{
    public class UserRegistrationModel
    {
        public string EmailAddress { get; internal set; }
        public string PhoneNumber { get; internal set; }
        public string GivenName { get; internal set; }
        public string Password { get; internal set; }
    }
}