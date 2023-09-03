using Music_Flix.Dtos;
using Music_Flix.Repositories;
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

        public List<MusicDTO> getTopThreeMusics() 
        {
            AuthorRepository authorRepository = new AuthorRepository();
            MusicRepository musicRepository = new MusicRepository();
            List<MusicDTO> musics = new List<MusicDTO>();
            List<long> musicsIds = authorRepository.GetMusicsIds((int)this.id);
            foreach (long musicId in musicsIds)
            {
                MusicDTO music = musicRepository.FindById((int)musicId);
                musics.Add(music);
            }
            List<MusicDTO> sortedMusics = musics.OrderByDescending(music => music.averageScore).ToList();
            return sortedMusics.Take(3).ToList();
        }
    }
}
