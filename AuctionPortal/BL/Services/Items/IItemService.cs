using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.DTOs.ItemImages;
using BL.DTOs.Items;

namespace BL.Services.Items
{
    public interface IItemService
    {
        long CreateItem(ItemDTO itemDTO);
        void CreateItemImage(ItemImageDTO itemImageDTO);
        void DeleteItemImage(long itemImageId);
        void DeleteItem(long itemId);
        void EditItem(ItemDTO itemDTO);
        ItemDTO GetItem(long itemId);
        ItemImageDTO GetItemImage(long itemImageId);

        IEnumerable<ItemDTO> GetAllItems(long auctionId = 0);

        IEnumerable<ItemImageDTO> GetItemImages(long itemId);

    }
}
