namespace Buyzia.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Picture
    {
        public Picture()
        {
            this.CreatedOn = DateTime.Now;
        }
        
        [Key]
        public int Id { get; set; }

        public byte[] Content { get; set; }

        public Guid ItemId { get; set; }

        public virtual Item Item { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsMainPicture { get; set; }
    }
}
