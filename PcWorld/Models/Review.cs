namespace PcWorld.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public int KeyboardId { get; set; }
        public Keyboard Keyboard { get; set; }
        public int MouseId { get; set; }
        public Mouse Mouse { get; set; }
        public int SetId { get; set; }
        public Set Set { get; set; }
    }
}
