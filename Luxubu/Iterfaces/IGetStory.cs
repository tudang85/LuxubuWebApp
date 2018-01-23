using Luxubu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Luxubu.Iterfaces
{
    public interface IGetStory
    {
        Chapters[] GetChapters(long lngStoryId);
        Stories GetInfo();
        bool Download();
    }
}
