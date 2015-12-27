namespace Buyzia.Server.Api.Models.Pictures
{
    using System;

    public class PicturesBindingModel
    {
        public Guid ToItemId { get; set; }

        public string Url { get; set; }

        public bool isMainPicture { get; set; }
    }
}