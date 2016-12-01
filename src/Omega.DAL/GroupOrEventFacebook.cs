namespace Omega.DAL
{
    public class GroupOrEventFacebook
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Cover { get; set; }

        public GroupOrEventFacebook( string id, string name, string cover )
        {
            Id = id;
            Name = name;
            Cover = cover;
        }
    }
}
