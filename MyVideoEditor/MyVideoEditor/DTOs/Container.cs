namespace MyVideoEditor.DTOs
{
    public class Container
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FullName { get; set; }
        public List<ContainerVideo> Videos { get; set; } = new List<ContainerVideo>();
        public List<ContainerAudio> Audios { get; set; } = new List<ContainerAudio>();
    }
}