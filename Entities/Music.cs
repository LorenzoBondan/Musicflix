using System.Collections.Generic;

namespace Music_Flix.Entities
{
    public class Music
    {
        public long id { get; set; }
        public string name { get; set; }
        public char isExplicit { get; set;}
        public int year { get; set; }
        public int minutes { get; set; }
        public int seconds { get; set; }
        public Style style { get; set; }
        public Album album { get; set; }

        public List<Author> authors { get; set; } = new List<Author>();
        public List<Review> reviews { get; set; } = new List<Review>();
        public List<User> usersFavorites { get; set; } = new List<User>();

        public Music() { }

        public Music(long id, string name, char isExplicit, int year, int minutes, int seconds, Album album, Style style)
        {
            this.id = id;
            this.name = name;
            this.isExplicit = isExplicit;
            this.year = year;
            this.minutes = minutes;
            this.seconds = seconds;
            this.album = album;
            this.style = style;
        }

        public double getAverageScore()
        { 
            if (reviews.Count == 0)
                return 0;

            double totalScore = 0;
            foreach (Review review in reviews)
            {
                totalScore += review.score;
            }

            return totalScore / reviews.Count;
        }
    }
}
