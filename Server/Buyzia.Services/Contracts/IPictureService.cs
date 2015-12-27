namespace Buyzia.Services.Contracts
{
    using System;

    public interface IPictureService
    {
        int Add(Guid toItemId, string url, bool isMainPicture = false);

        byte[] GetPictureById(int pictureId);
    }
}
