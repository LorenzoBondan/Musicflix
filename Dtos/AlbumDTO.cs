using Music_Flix.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music_Flix.Dtos
{
    public class AlbumDTO
    {
        public long id { get; set; }
        public string name { get; set; }
        public int year { get; set; }
        public string imgUrl { get; set; }
        public double averageScore { get; set; }

        public List<MusicDTO> musics { get; } = new List<MusicDTO>();
        public List<long> authorsIds { get; } = new List<long>();

        public AlbumDTO() { }

        public AlbumDTO(Album entity)
        {
            this.id = entity.id;
            this.name = entity.name;
            this.year = entity.year;
            this.imgUrl = entity.imgUrl;
            this.averageScore = entity.getAverageScore();

            this.musics.Clear();
            foreach(Music music in entity.musics){
                MusicDTO musicDTO = new MusicDTO(music);
                this.musics.Add(musicDTO);
            }

            this.authorsIds.Clear();
            foreach (Author author in entity.authors)
            {
                long authorId = author.id;
                this.authorsIds.Add(authorId);
            }
        }

        public AlbumDTO(long id, string name, int year, string imgUrl, double averageScore)
        {
            this.id = id;
            this.name = name;
            this.year = year;
            this.imgUrl = imgUrl;
            this.averageScore = averageScore;
        }
    }
}
