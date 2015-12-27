namespace Buyzia.Services
{
    using System;
    using Common;
    using Contracts;
    using Data.Models;
    using Data.Repositories;
    using Helpers;

    public class PictureService : IPictureService
    {
        private readonly IRepository<Picture> picturesRepo;

        public PictureService(IRepository<Picture> picturesRepo)
        {
            this.picturesRepo = picturesRepo;
        }

        public int Add(Guid toItemId, string url, bool isMainPicture = false)
        {
            var pictureToAdd = new Picture
            {
                ItemId = toItemId,
                Content = ImagesHelper.ResizeImageByLongestSide(url, Constants.PICTURE_LONGEST_SIDE, Constants.IMAGE_FORMAT),
                IsMainPicture = isMainPicture
            };

            this.picturesRepo.Add(pictureToAdd);
            this.picturesRepo.SaveChanges();

            return pictureToAdd.Id;
        }

        public byte[] GetPictureById(int pictureId)
        {
            return this.picturesRepo.FindById(pictureId).Content;
        }
    }
}
