namespace Buyzia.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
                Content = PicturesHelper.ResizeImageByLongestSide(url, Constants.PICTURE_LONGEST_SIDE, Constants.IMAGE_FORMAT),
                IsMainPicture = isMainPicture
            };

            this.picturesRepo.Add(pictureToAdd);
            this.picturesRepo.SaveChanges();

            return pictureToAdd.Id;
        }

        public ICollection<string> GetAllPictureUrlsForGivenItem(Guid itemId)
        {
             var result = this.picturesRepo
                .All()
                .Where(x => x.ItemId == itemId)
                .Select(x => Constants.SERVER_URL_PREFIX + Constants.PICTURES_ROUTE_URL + x.Id)
                .ToList();

            return result;
        }

        public byte[] GetPictureById(int pictureId)
        {
            return this.picturesRepo.FindById(pictureId).Content;
        }

        public string GetMainPictureLinkByItemId(Guid itemId)
        {
            var mainPictureUrl = this.picturesRepo
               .All()
               .Where(x => x.ItemId == itemId && x.IsMainPicture)
               .Select(x => Constants.SERVER_URL_PREFIX + Constants.PICTURES_ROUTE_URL + x.Id)
               .FirstOrDefault();


            if (mainPictureUrl == null)
            {
                throw new ArgumentNullException("Main picture cannot be null");
            }

            return mainPictureUrl;
        }
    }
}
