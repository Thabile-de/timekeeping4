namespace TimeKeeping4.Model
{
    public class AuthResponseModel : BaseResponse
    {
        public int UserId { get; set; } 
        public string? UserName { get; set; }
        public string EmailAddress { get; set; }
        public string Tokens { get; set; }
    }
}