using System;
using System.Collections.Generic;
using InventoryControl.Models;

namespace InventoryControl.Repositories
{
    public interface IAlbumRepository
    {
        IEnumerable<Album> Get();
        Album Get(Guid id);
        Album Update(Album album);
    }
}
