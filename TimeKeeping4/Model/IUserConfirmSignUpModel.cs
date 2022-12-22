namespace TimeKeeping4.Model
{
    public interface IUserConfirmSignUpModel
    {
        string ConfirmationCode { get; set; }
        string EmailAddress { get; set; }
    }
}