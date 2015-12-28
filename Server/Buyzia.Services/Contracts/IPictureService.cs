namespace Buyzia.Services.Contracts
{
    using System;
    using System.Collections.Generic;

    public interface IPictureService
    {
        int Add(Guid toItemId, string url, bool isMainPicture = false);

        byte[] GetPictureById(int pictureId);

        ICollection<string> GetAllPictureUrlsForGivenItem(Guid itemId);
    }
}
