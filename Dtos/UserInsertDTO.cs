
namespace Music_Flix.Dtos
{
    public class UserInsertDTO : UserDTO
    {
        public string password { get; set; }

        public UserInsertDTO() { }

        public UserInsertDTO(string password)
        {
            this.password = password;
        }
    }
}
