using System.Collections.Generic;
using System.Linq;

namespace Music_Flix.Entities
{
    public class Author
    {
        public long id { get; set; }
        public string name { get; set; }
        public string birthDate { get; set; }
        public string imgUrl { get; set; }
        public List<Music> musics { get; set; } = new List<Music>();
        public List<Album> albums { get; set; } = new List<Album>();

        public Author() { }

        public Author(long id, string name, string birthDate, string imgUrl)
        {
            this.id = id;
            this.name = name;
            this.birthDate = birthDate;
            this.imgUrl = imgUrl;
        }

        public double getAverageScore()
        {
            double averageScore = 0;
            foreach (Music music in musics)
            {
                averageScore += music.getAverageScore();
            }
            return averageScore;
        }

        public List<Music> getTopThreeMusics()
        {
            List<Music> sortedMusics = musics.OrderByDescending(music => music.getAverageScore()).ToList();
            return sortedMusics.Take(3).ToList();
        }
    }
}
