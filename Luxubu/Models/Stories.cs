using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Luxubu.Models
{
    public partial class Stories
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Cover { get; set; }
        public string Desc { get; set; }
    }

    public partial class Chapters
    {
        public long Id { get; set; }
        public long StoryId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public short Status { get; set; }
    }
}
