namespace Buyzia.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Item
    {
        private ICollection<Picture> pictures;
        private ICollection<Feature> features;

        public Item()
        {
            this.pictures = new HashSet<Picture>();
            this.features = new HashSet<Feature>();
            this.CreatedOn = DateTime.Now;

            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string OriginalName { get; set; }

        [Required(ErrorMessage = "OriginalUrl is mandatory")]
        [MaxLength(250)]
        public string OriginalUrl { get; set; }

        [Required(ErrorMessage = "AvailableQuantity is mandatory")]
        public int AvailableQuantity { get; set; }

        public decimal? OriginalDiscount { get; set; }

        [Required(ErrorMessage = "OriginalPrice is mandatory")]
        public decimal OriginalPrice { get; set; }

        public decimal OverchargePercentage { get; set; }

        public decimal SellingPrice { get; set; }

        public decimal Profit { get; set; }

        public decimal Taxes { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [MaxLength(3500)]
        public string DescriptionContent { get; set; }

        [NotMapped]
        public string DescriptionHtml { get; set; }

        [Required(ErrorMessage = "Pictures are mandatory")]
        public virtual ICollection<Picture> Pictures
        {
            get { return this.pictures; }
            set { this.pictures = value; }
        }

        public virtual ICollection<Feature> Features
        {
            get { return this.features; }
            set { this.features = value; }
        }
    }
}
