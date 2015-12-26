namespace Buyzia.Services
{
    using System.Linq;
    using Data.Models;

    public interface IItemService
    {
        IQueryable<Item> All();
    }
}
