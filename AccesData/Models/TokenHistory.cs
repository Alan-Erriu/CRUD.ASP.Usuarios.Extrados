namespace AccesData.Models
{
    public class TokenHistory
    {
        public int id_tokenhistory { get; set; }
        public int iduser_tokenhistory { get; set; }
        public string token_tokenhistory { get; set; }
        public string refreshToken_tokenhistory { get; set; }
        public long expiration_date_tokenhistory { get; set; }

    }
}
