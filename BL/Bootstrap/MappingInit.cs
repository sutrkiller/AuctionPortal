using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autentization;
using AutoMapper;
using BL.DTOs.Auctions;
using BL.DTOs.Bids;
using BL.DTOs.Categories;
using BL.DTOs.Comments;
using BL.DTOs.Deliveries;
using BL.DTOs.ItemImages;
using BL.DTOs.Items;
using BL.DTOs.UserAccount;
using BL.DTOs.Users;
using BL.Properties;
using DAL.Entities;

namespace BL.Bootstrap
{
    public static class MappingInit
    {
        public static void ConfigureMapping()
        {
            Mapper.Initialize(conf =>
            {
                conf.CreateMap<Category, CategoryDTO>().ReverseMap();
                conf.CreateMap<ItemImage, ItemImageDTO>()
                .ForMember(d=>d.ItemId,opts=>opts.MapFrom(src=>src.Item.ID))
                .ReverseMap();

                conf.CreateMap<Comment, CommentDTO>()
                    .ForMember(d => d.AuctionId, opts => opts.MapFrom(src => src.Auction.ID))
                    .ForMember(d => d.AuthorId, opts => opts.MapFrom(src => src.Author.ID))
                    .ForMember(d=>d.ParentId,opt=>opt.MapFrom(s=>s.ParentComment.ID))
                    .ForMember(d=>d.AuthorName,opt=>opt.MapFrom(s=>s.Author.UserAccount.FirstName +" "+ s.Author.UserAccount.LastName))
                    .ReverseMap();

                conf.CreateMap<CommentCreateDTO, Comment>()
                   .ForMember(d=>d.ID,opt => opt.Ignore())
                   .ForMember(d=>d.Author,opt => opt.Ignore())
                   .ForMember(d=>d.Auction,opt => opt.Ignore())
                   .ForMember(d=>d.ParentComment,opt => opt.Ignore())
                   .ForMember(d=>d.ChildComments,opt => opt.Ignore());

                conf.CreateMap<CommentEditDTO, Comment>()
                .ForMember(d => d.Time, opt => opt.Ignore())
                   .ForMember(d => d.Author, opt => opt.Ignore())
                   .ForMember(d => d.Auction, opt => opt.Ignore())
                   .ForMember(d => d.ParentComment, opt => opt.Ignore())
                   .ForMember(d => d.ChildComments, opt => opt.Ignore());

                conf.CreateMap<Item, ItemDTO>()
                    .ForMember(d => d.AuctionId, opt => opt.MapFrom(src => src.Auction.ID))
                    .ReverseMap();

                conf.CreateMap<User, UserDTO>()
                    .ForMember(d => d.FirstName, opts => opts.MapFrom(src => src.UserAccount.FirstName))
                    .ForMember(d => d.LastName, opts => opts.MapFrom(src => src.UserAccount.LastName))
                    .ForMember(d => d.BirthDate, opts => opts.MapFrom(src => src.UserAccount.BirthDate))
                    .ForMember(d => d.Email, opts => opts.MapFrom(src => src.UserAccount.Email))
                    .ForMember(d => d.Address, opts => opts.MapFrom(src => src.UserAccount.Address));
                    //.ReverseMap();  

                conf.CreateMap<UserDTO, UserAccount>()
                    .ForMember(d=>d.ID,opt=>opt.Ignore());

                conf.CreateMap<UserDTO, User>()
                    .ForMember(d => d.UserAccount, opt => opt.MapFrom(s=>s));

                conf.CreateMap<UserAccount, UserAccountDTO>().ReverseMap();

                conf.CreateMap<UserRegistrationDTO, UserAccount>();


                conf.CreateMap<AuctionCreateDTO, Auction>()
                    .ForMember(d => d.ID, opt => opt.Ignore())
                    .ForMember(d => d.AuctionViews, opt => opt.Ignore())
                    .ForMember(d => d.Delivery, opt => opt.Ignore())
                    .ForMember(d => d.Seller, opt => opt.Ignore())
                    .ForMember(d => d.Items, opt => opt.Ignore())
                    .ForMember(d => d.Bids, opt => opt.Ignore())
                    .ForMember(d => d.Category, opt => opt.Ignore())
                    .ForMember(d => d.Comments, opt => opt.Ignore());

                conf.CreateMap<Auction, AuctionDTO>()
                    .ForMember(d => d.SellerId, opt => opt.MapFrom(src => src.Seller.ID))
                    .ForMember(d=>d.CurrentPrice,opt=>opt.MapFrom(s=>s.Bids.Any() ? s.Bids.OrderByDescending(b=>b.BidTime).FirstOrDefault().Value : s.BasePrice))
                    .ForMember(d=>d.CoverImagePath,opt=>opt.MapFrom(s=>s.Items.SelectMany(i=>i.ItemImages).Select(x=>x.ImagePath).FirstOrDefault()))
                    .ForMember(d=>d.MinPrice,opt => opt.Ignore())
                    .ForMember(d=>d.OneClickPrice,opt => opt.Ignore())
                    .ReverseMap();

                conf.CreateMap<Delivery, DeliveryDTO>()
                    .ForMember(d => d.AuctionId, opt => opt.MapFrom(s => s.Auction.ID))
                    .ForMember(d => d.BuyerId, opt => opt.MapFrom(s => s.Buyer.ID))
                    .ForMember(d => d.SellerId, opt => opt.MapFrom(s => s.Seller.ID));

                conf.CreateMap<DeliveryEditDTO, Delivery>()
                    .ForMember(d => d.Seller, opt => opt.Ignore())
                    .ForMember(d => d.Buyer, opt => opt.Ignore())
                    .ForMember(d => d.Auction, opt => opt.Ignore());

                conf.CreateMap<Bid, BidDTO>()
                    .ForMember(d => d.AuctionId, opt => opt.MapFrom(s => s.Auction.ID))
                    .ForMember(d => d.BidderId, opt => opt.MapFrom(s => s.User.ID))
                    .ReverseMap();
            });

            //Mapper.AssertConfigurationIsValid();
        }
    }
}
