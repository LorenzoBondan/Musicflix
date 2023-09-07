using Music_Flix.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music_Flix.Dtos
{
    public class ReviewDTO
    {
        public long id { get; set; }
        public string text { get; set; }
        public string moment { get; set; }
        public double score { get; set; }
        public long userId { get; set; }
        public long musicId { get; set; }

        public ReviewDTO() { }

        public ReviewDTO(Review entity)
        {
            this.id = entity.id;
            this.text = entity.text;
            this.moment = entity.moment;
            this.score = entity.score;
            this.userId = entity.user.id;
            this.musicId = entity.music.id;
        }

        public ReviewDTO(long id, string text, string moment, double score, long userId, long musicId)
        {
            this.id = id;
            this.text = text;
            this.moment = moment;
            this.score = score;
            this.userId = userId;
            this.musicId = musicId;
        }
    }
}
