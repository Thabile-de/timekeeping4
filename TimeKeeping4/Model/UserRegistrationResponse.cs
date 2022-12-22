namespace TimeKeeping4.Model
{
    public class UserRegistrationResponse
    {
        public bool IsSuccess { get; internal set; }
        public string Message { get; internal set; }
        public object EmailAddress { get; internal set; }
        public object UserId { get; internal set; }
    }
}