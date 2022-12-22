namespace TimeKeeping4.Model
{
    public class UserSignUpResponse
    {
        public string EmailAddress { get; internal set; }
        public object UserId { get; internal set; }
        public string Message { get; internal set; }
        public bool IsSuccess { get; internal set; }
    }
}