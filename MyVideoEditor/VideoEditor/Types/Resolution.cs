namespace VideoEditor.Types
{
    public class Resolution
    {
        public Resolution()
        {
            Width = 1920;
            Height = 1080;
        }
        public Resolution(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Width { get; set; }
        public int Height { get; set; }

        public static bool TryParse(int? width, int? height, out Resolution? resolution)
        {
            resolution = null;
            if (width == null || height == null) return false;
            resolution = new Resolution(width.Value, height.Value);
            return true;
        }
        public override string ToString()
        {
            return $"{Width}x{Height}";
        }
    }
}