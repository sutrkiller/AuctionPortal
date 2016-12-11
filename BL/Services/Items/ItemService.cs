using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BL.DTOs.ItemImages;
using BL.DTOs.Items;
using BL.Queries;
using BL.Repositories;
using DAL.Entities;

namespace BL.Services.Items
{
    public class ItemService : BaseService, IItemService
    {
        private const string ImagesFolder = "Images\\";

        #region Dependencies

        private readonly ItemRepository _itemRepository;
        private readonly AuctionRepository _auctionRepository;
        private readonly ItemImageRepository _itemImageRepository;
        private readonly ItemImageListQuery _itemImageListQuery;
        private readonly ItemListQuery _itemListQuery;

        public ItemService(ItemRepository itemRepository, AuctionRepository auctionRepository,
            ItemImageRepository itemImageRepository, ItemImageListQuery itemImageListQuery, ItemListQuery itemListQuery)
        {
            _itemRepository = itemRepository;
            _auctionRepository = auctionRepository;
            _itemImageRepository = itemImageRepository;
            _itemImageListQuery = itemImageListQuery;
            _itemListQuery = itemListQuery;
        }

        #endregion

        public long CreateItem(ItemDTO itemDTO)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var item = Mapper.Map<Item>(itemDTO);
                item.Auction = _auctionRepository.GetById(itemDTO.AuctionId);

                _itemRepository.Insert(item);
                uow.Commit();
                return item.ID;
            }
        }

        public void CreateItemImage(ItemImageDTO itemImageDTO)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var image = Mapper.Map<ItemImage>(itemImageDTO);
                image.Item = _itemRepository.GetById(itemImageDTO.ItemId);

                //var path = Enumerable.Range(1, 100)
                //    .Select(
                //        x =>
                //            Path.Combine(ImagesFolder,
                //                image.Item.ID + $"_{x}.{itemImageDTO.Image.RawFormat.ToString().ToLower()}"))
                //    .FirstOrDefault(x => !File.Exists(x));
                //if (path == null) throw new FormatException("Unable to safe file.");
                //try
                //{
                //    itemImageDTO.Image.Save(path);
                //}
                //catch
                //{
                //    throw new FormatException("Unable to safe file.");
                //}
                //image.ImagePath = path;
                _itemImageRepository.Insert(image);
                uow.Commit();
            }
        }

        public void DeleteItemImage(long itemImageId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                _itemImageRepository.Delete(itemImageId);
                uow.Commit();
            }
        }

        public void DeleteItem(long itemId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                _itemRepository.Delete(itemId);
                uow.Commit();
            }
        }

        public void EditItem(ItemDTO itemDTO)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var item = _itemRepository.GetById(itemDTO.ID);
                Mapper.Map(itemDTO, item);

                _itemRepository.Update(item);
                uow.Commit();
            }
        }

        public ItemDTO GetItem(long itemId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var item = _itemRepository.GetById(itemId, i => i.ItemImages, i => i.Auction);
                return item != null ? Mapper.Map<ItemDTO>(item) : null;
            }
        }

        public ItemImageDTO GetItemImage(long itemImageId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var image = _itemImageRepository.GetById(itemImageId, i => i.Item);
                if (image == null) return null;
                var dto = Mapper.Map<ItemImageDTO>(image);
                //try
                //{
                //    dto.Image = Image.FromFile(image.ImagePath);
                //}
                //catch
                //{
                //    return null;
                //}
                
                return dto;
            }
        }

        public IEnumerable<ItemDTO> GetAllItems(long auctionId = 0)
        {
            using (UnitOfWorkProvider.Create())
            {
                _itemListQuery.AuctionId = auctionId;
                return _itemListQuery.Execute() ?? new List<ItemDTO>();

            }
        }

        public IEnumerable<ItemImageDTO> GetItemImages(long itemId)
        {
            using (UnitOfWorkProvider.Create())
            {
                
                _itemImageListQuery.ItemId = itemId;
                var images = _itemImageListQuery.Execute() ?? new List<ItemImageDTO>();
                return images.Select(x => GetItemImage(x.ID)).Where(x=> x!= null);
            }
        }
    }
}
