namespace MyVideoEditor.DTOs
{
    public class Media
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FullName { get; set; }
        public List<MediaVideo> Videos { get; set; } = new List<MediaVideo>();
        public List<MediaAudio> Audios { get; set; } = new List<MediaAudio>();
    }
}