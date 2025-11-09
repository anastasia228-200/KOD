using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;


namespace TestingPlatform.domain.Models
{
    public class Direction
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Group> Groups { get; set; }
        public List<Test> Tests { get; set; }
    }
}
