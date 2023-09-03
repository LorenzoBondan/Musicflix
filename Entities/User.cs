using System.Collections.Generic;

namespace Music_Flix.Entities
{
    public class User
    {
        public int id { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public string admin { get; set; }
        public string email { get; set; }
        public string imgUrl { get; set; }

        public List<Review> reviews { get; set; } = new List<Review>();
        public List<Music> favoritedMusics { get; set;} = new List<Music>();

        public User() { }

        public User(int id, string name, string password, string admin, string email, string imgUrl)
        {
            this.id = id;
            this.name = name;
            this.password = password;
            this.admin = admin;
            this.email = email;
            this.imgUrl = imgUrl;
        }

        public void addReview(Review review)
        {
            this.reviews.Add(review);
        }
    }
}
