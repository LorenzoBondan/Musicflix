using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music_Flix.Entities
{
    public class Album
    {
        public long id { get; set; }
        public string name { get; set; }
        public int year { get; set; }
        public string imgUrl { get; set; }
        public List<Music> musics { get; } = new List<Music>();
        public List<Author> authors { get; } = new List<Author>();

        public Album() { }

        public Album(long id, string name, int year, string imgUrl)
        {
            this.id = id;
            this.name = name;
            this.year = year;
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
    }
}
