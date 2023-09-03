using Music_Flix.Entities;
using System.Collections.Generic;

namespace Music_Flix.Dtos
{
    public class UserDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string admin { get; set; }
        public string email { get; set; }
        public string imgUrl { get; set; }

        public List<ReviewDTO> reviews { get; set; } = new List<ReviewDTO>();
        public List<MusicDTO> favoritedMusics { get; set; } = new List<MusicDTO>();

        public UserDTO() { }

        public UserDTO(User entity)
        {
            this.id = entity.id;
            this.name = entity.name;
            this.admin = entity.admin;
            this.email = entity.email;

            this.reviews.Clear();
            foreach (Review review in entity.reviews)
            {
                ReviewDTO reviewDTO = new ReviewDTO(review);
                this.reviews.Add(reviewDTO);
            }

            this.favoritedMusics.Clear();
            foreach (Music music in entity.favoritedMusics)
            {
                MusicDTO musicDTO = new MusicDTO(music);
                this.favoritedMusics.Add(musicDTO);
            }
        }

        public UserDTO(int id, string name, string admin, string email, string imgUrl)
        {
            this.id = id;
            this.name = name;
            this.admin = admin;
            this.email = email;
            this.imgUrl = imgUrl;
        }
    }
}
