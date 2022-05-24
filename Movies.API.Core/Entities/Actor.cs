namespace Movies.API.Core.Entities
{
    public class Actor : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<MovieActor> MovieActors { get; set; }

        public Actor()
        {
            CreatedDate = DateTimeOffset.Now;
            UpdatedDate = DateTimeOffset.Now;
        }
    }
}
