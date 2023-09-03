using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music_Flix.Entities
{
    public class Style
    {
        public long id {  get; set; }
        public string description { get; set; }

        public List<Music> musics { get; } = new List<Music>();

        public Style() { }

        public Style(long id, string description)
        {
            this.id = id;
            this.description = description;
        }
    }
}
