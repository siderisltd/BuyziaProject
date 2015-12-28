﻿namespace Buyzia.Services.Contracts
{
    using System;
    using System.Linq;
    using Data.Models;

    public interface IItemService
    {
        IQueryable<Item> All();

        Guid Add(Item itemToAdd);

        string GetItemDescriptionById(object id);
    }
}
