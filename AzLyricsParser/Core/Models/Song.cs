using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AzLyricParser.Core.Models
{
    public class Song
    {       
        public string Artist { get; }

        public string Name { get; }

        public string FullName { get; }

        public string Featuring { get; }

        private string Link { get; }

        public string Lyrics { get; private set; }

        private readonly HttpClient _client;

        public Song(HttpClient client)
        {                  
            _client = client;
        }

        public Song(string artist, string name, string featuring, string link, HttpClient client) : this(client)
        {
            Artist = artist;
            Name = name;
            FullName = GetFullName();
            Featuring = featuring;
            Link = link;      
        }

        public async Task<string> GetLyrics()
        {                                   
            if (string.IsNullOrWhiteSpace(Lyrics))            
                Lyrics = Helpers.AzLyricParser.ParseLyrics(await _client.GetStringAsync(Link)); 

            return Lyrics;
        }

        private string GetFullName()
        {
            var fullNameStringBuilder = new StringBuilder();
            fullNameStringBuilder.Append(Artist);

            fullNameStringBuilder.Append(" - ");
            fullNameStringBuilder.Append(Name);

            if (!string.IsNullOrWhiteSpace(Featuring))
            {
                fullNameStringBuilder.Append(" (");
                fullNameStringBuilder.Append(Featuring);
                fullNameStringBuilder.Append(")");
            }

            return fullNameStringBuilder.ToString();
        }

        public override string ToString()
        {
            return FullName;
        }
    }
}