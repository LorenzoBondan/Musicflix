using Music_Flix.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music_Flix.Dtos
{
    public class StyleDTO
    {
        public long id { get; set; }
        public string description { get; set; }

        public List<long> musicsIds { get; } = new List<long>();

        public StyleDTO() { }

        public StyleDTO(Style entity)
        {
            this.id = entity.id;
            this.description = entity.description;

            this.musicsIds.Clear();
            foreach (Music music in entity.musics)
            {
                this.musicsIds.Add(music.id);
            }
        }
    }
}
