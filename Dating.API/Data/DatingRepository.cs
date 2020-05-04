using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dating.API.Models;
using Dating.API.Helpers;
using Microsoft.EntityFrameworkCore;
using System;

namespace Dating.API.Data
{
    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext _context;

        public DatingRepository(DataContext context)
        {
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<Photo> GetMainPhotoForUser(int userId)
        {
            return await _context.Photos.Where(u => u.UserId == userId).FirstOrDefaultAsync(p => p.IsMain);
        }

        public async Task<Photo> GetPhoto(int id)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);

            return photo;
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<Likes> GetLikes(int userId, int recipientId) 
        {
            return await _context.Likes.FirstOrDefaultAsync(u => u.LikerId == userId && u.LikeeID == recipientId);
        }

        public async Task<PageList<User>> GetUsers(UserParams userParams)
        {
           var users = _context.Users.Include(p => p.Photos).OrderByDescending(u => u.LastActive).AsQueryable();

           users = users.Where(u => u.Id != userParams.UserId);

           users = users.Where(u => u.Gender == userParams.Gender);

           if (userParams.Likers)
           {
               var userLikers = await GetUserLikes(userParams.UserId, userParams.Likers);
               users = users.Where(u => userLikers.Contains(u.Id));
           }

           if (userParams.Likees)
           {
               var userLikees = await GetUserLikes(userParams.UserId, userParams.Likers);
               users = users.Where(u => userLikees.Contains(u.Id));
           }

           if(userParams.MinAge != 18 || userParams.MaxAge != 99)
           {
               var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);

               var maxDob = DateTime.Today.AddYears(-userParams.MinAge);

               users = users.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);
           }

           if (!string.IsNullOrEmpty(userParams.OrderBy))
           {
               switch (userParams.OrderBy)
               {
                   case "created":
                        users = users.OrderByDescending(u => u.Created);
                        break;
                    default: 
                        users = users.OrderByDescending(u => u.LastActive);
                        break;
               }
           }

           return await PageList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }

        private async Task<IEnumerable<int>> GetUserLikes(int id, bool likers)
        {
            var user = await _context.Users.Include(x => x.Likers).Include(x => x.Likee).FirstOrDefaultAsync(u => u.Id == id);

            if (likers)
            {
                return user.Likers.Where(u => u.LikeeID == id).Select(i => i.LikerId);
            }
            else
            {
                return user.Likee.Where(u => u.LikerId == id).Select(i => i.LikeeID);
            }
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Message> GetMessage(int id) {
            return await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<PageList<Message>> GetMessagesForUser(MessageParams messageParams) 
        {
            var messages = _context.Messages
                .Include(u => u.Sender)
                .ThenInclude(p => p.Photos)
                .Include(u => u.Recipient)
                .ThenInclude(p => p.Photos)
                .AsQueryable();

            switch (messageParams.MessageContainer)
            {
                case "Inbox":
                    messages = messages.Where(u => u.RecipientId == messageParams.UserId && u.RecipientDeleted == false);
                    break;
                case "Outbox":
                    messages = messages.Where(u => u.SenderId == messageParams.UserId && u.SenderDeleted == false);
                    break;
                default:
                    messages = messages.Where(u => u.RecipientId == messageParams.UserId && u.RecipientDeleted == false && u.IsRead == false);
                    break;
            }

            messages = messages.OrderByDescending(d => d.MessageSent);
            
            return await PageList<Message>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<Message>> GetMessagesThread(int userId, int recipientId)
        {
            var messages = await _context.Messages
                .Include(u => u.Sender)
                .ThenInclude(p => p.Photos)
                .Include(u => u.Recipient)
                .ThenInclude(p => p.Photos)
                .Where(m => m.RecipientId == userId && m.RecipientDeleted == false && m.SenderId == recipientId 
                    || m.RecipientId == recipientId && m.SenderId == userId && m.SenderDeleted == false)
                .OrderByDescending(m => m.MessageSent)
                .ToArrayAsync();

            return messages;
        }
    }
}