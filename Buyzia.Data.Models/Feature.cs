namespace Buyzia.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Feature
    {
        public int Id { get; set; }

        [Required]
        public Guid ItemId { get; set; }

        public virtual Item Item { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
