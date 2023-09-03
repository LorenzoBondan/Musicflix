using Music_Flix.Entities;
using System.Collections.Generic;

namespace Music_Flix.Dtos
{
    public class MusicDTO
    {
        public long id { get; set; }
        public string name { get; set; }
        public char isExplicit { get; set; }
        public int year { get; set; }
        public int minutes { get; set; }
        public int seconds { get; set; }
        public long styleId { get; set; }
        public long albumId { get; set; }
        public double averageScore { get; set; }

        public List<long> authorsIds { get; } = new List<long>();
        public List<ReviewDTO> reviews { get; } = new List<ReviewDTO>();
        public List<long> usersFavoritedIds { get; } = new List<long>();

        public MusicDTO() { }

        public MusicDTO(Music entity)
        {
            this.id = entity.id;
            this.name = entity.name;
            this.isExplicit = entity.isExplicit;
            this.year = entity.year;
            this.minutes = entity.minutes;
            this.seconds = entity.seconds;
            this.styleId = entity.style.id;
            this.albumId = entity.album.id;
            this.averageScore = entity.getAverageScore();

            this.authorsIds.Clear();
            foreach (Author author in entity.authors)
            {
                this.authorsIds.Add(author.id);
            }

            this.reviews.Clear();
            foreach (Review review in entity.reviews)
            {
                ReviewDTO reviewDTO = new ReviewDTO(review);
                this.reviews.Add(reviewDTO);
            }

            this.usersFavoritedIds.Clear();
            foreach (User user in entity.usersFavorites)
            {
                this.usersFavoritedIds.Add(user.id);
            }
        }

        public MusicDTO(long id, string name, char isExplicit, int year, int minutes, int seconds, long albumId, long styleId, double averageScore)
        {
            this.id = id;
            this.name = name;
            this.isExplicit = isExplicit;
            this.year = year;
            this.minutes = minutes;
            this.seconds = seconds;
            this.albumId = albumId;
            this.styleId = styleId;
            this.averageScore = averageScore;
        }
    }
}
