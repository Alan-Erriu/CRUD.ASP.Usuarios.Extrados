namespace AccesData.DTOs.AuthDTOs
{
    public class RefreshTokenDTO
    {
        public int iduser_tokenhistory { get; set; }
        public string token_tokenhistory { get; set; }
        public string refresh_Token_tokenhistory { get; set; }
        public long expiration_date_tokenhistory { get; set; }
        public string msg { get; set; }

    }
}
