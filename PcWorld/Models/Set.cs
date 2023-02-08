namespace PcWorld.Models
{
    public class Set
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int KeyboardId { get; set; }
        public Keyboard Keyboard { get; set; }
        public int MouseId { get; set; }
        public Mouse Mouse { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
