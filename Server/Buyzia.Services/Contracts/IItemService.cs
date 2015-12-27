namespace Buyzia.Services.Contracts
{
    using System.Linq;
    using Data.Models;

    public interface IItemService
    {
        IQueryable<Item> All();
    }
}
