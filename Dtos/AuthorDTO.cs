using Music_Flix.Entities;
using System.Collections.Generic;

namespace Music_Flix.Dtos
{
    public class AuthorDTO
    {
        public long id { get; set; }
        public string name { get; set; }
        public string birthDate { get; set; }
        public string imgUrl { get; set; }
        public double averageScore { get; set; }

        public List<long> musicsIds { get; set; } = new List<long>();
        public List<AlbumDTO> albums { get; set; } = new List<AlbumDTO>();
        public List<long> topThreeMusicsIds { get; set; } = new List<long>(); // method

        public AuthorDTO() { }

        public AuthorDTO(Author entity)
        {
            this.id = entity.id;
            this.name = entity.name;
            this.birthDate = entity.birthDate;
            this.imgUrl = entity.imgUrl;
            this.averageScore = entity.getAverageScore();

            this.musicsIds.Clear();
            foreach (Music music in entity.musics)
            {
                this.musicsIds.Add(music.id);
            }

            this.albums.Clear();
            foreach (Album album in entity.albums)
            {
                AlbumDTO albumDTO = new AlbumDTO(album);
                this.albums.Add(albumDTO);
            }

            this.topThreeMusicsIds.Clear();
            foreach (MusicDTO music in entity.getTopThreeMusics())
            {
                this.topThreeMusicsIds.Add(music.id);
            }
        }

        public AuthorDTO(long id, string name, string birthDate, string imgUrl, double averageScore)
        {
            this.id = id;
            this.name = name;
            this.birthDate = birthDate;
            this.imgUrl = imgUrl;
            this.averageScore = averageScore;
        }
    }
}
