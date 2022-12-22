namespace TimeKeeping4.Model
{
    public class UserConfirmSignUpModel
    {
        public string? ClientId { get; set; }
        public string? UserId { get; set; }
        public string? ConfirmationCode { get; set; }
        public string? UserName { get; set; }
        public string? EmailAddress { get; set; }
    }
}
