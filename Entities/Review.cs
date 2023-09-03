using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music_Flix.Entities
{
    public class Review
    {
        public long id { get; set; }
        public string text { get; set; }
        public DateTime moment { get; set; }
        public double score { get; set; }
        public User user { get; set; }
        public Music music { get; set; }

        public Review() { }

        public Review(long id, string text, DateTime moment, double score, Music music, User user)
        {
            this.id = id;
            this.text = text;
            this.moment = moment;
            this.score = score;
            this.music = music;
            this.user = user;
        }
    }
}
