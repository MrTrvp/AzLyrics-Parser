using System.Collections.Generic;
using System.Linq;    

namespace AzLyricParser.Core.Models
{
    public class SearchResult
    {          
        public Song[] Entries { get; }

        public bool EntriesFound => Entries.Length != 0;

        public SearchResult(IEnumerable<Song> entries)
        {
            Entries = entries?.ToArray() ?? new Song[] {};
        }
    }
}