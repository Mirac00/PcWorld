namespace PcWorld.Models
{
    public class Mouse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public decimal Price { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
