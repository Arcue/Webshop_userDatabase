namespace UserAPI.Dto
{
    public class TableUserDto
    {
        
        public string Email { get; set; }
        public string name { get; set; }
        public string Password { get; set; }
        public string Adress { get; set; }
        public int Postnummer { get; set; }
        public string Stad { get; set; }
        public string Salt { get; set; }
        public string Hashedpassword { get; set; }
    }
}