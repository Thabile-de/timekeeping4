namespace TimeKeeping4.Model
{
    public class TokenModel
    {
        public string IdToken { get; set; }
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }
        public string RefreshToken { get; set; }

        public static implicit operator string(TokenModel v)
        {
            throw new NotImplementedException();
        }
    }
}