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

        public List<MusicDTO> musics { get; } = new List<MusicDTO>();
        public List<AlbumDTO> albums { get; } = new List<AlbumDTO>();
        public List<MusicDTO> topThreeMusics { get; } = new List<MusicDTO>(); // method

        public AuthorDTO() { }

        public AuthorDTO(Author entity)
        {
            this.id = entity.id;
            this.name = entity.name;
            this.birthDate = entity.birthDate;
            this.imgUrl = entity.imgUrl;
            this.averageScore = entity.getAverageScore();

            this.musics.Clear();
            foreach (Music music in entity.musics)
            {
                MusicDTO musicDTO = new MusicDTO(music);
                this.musics.Add(musicDTO);
            }

            this.albums.Clear();
            foreach (Album album in entity.albums)
            {
                AlbumDTO albumDTO = new AlbumDTO(album);
                this.albums.Add(albumDTO);
            }

            this.topThreeMusics.Clear();
            foreach (Music music in entity.getTopThreeMusics())
            {
                MusicDTO musicDTO = new MusicDTO(music);
                this.topThreeMusics.Add(musicDTO);
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
