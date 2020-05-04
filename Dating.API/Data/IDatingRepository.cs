using System.Collections.Generic;
using System.Threading.Tasks;
using Dating.API.Models;
using Dating.API.Helpers;

namespace Dating.API.Data
{
    public interface IDatingRepository
    {
         void Add<T>(T entity) where T: class;

         void Delete<T>(T entity) where T: class;
         Task<bool> SaveAll();
        Task<PageList<User>> GetUsers(UserParams userParams);
        Task<User> GetUser(int id);
        Task<Photo> GetPhoto(int id);
        Task<Photo> GetMainPhotoForUser(int userId);
        Task<Likes> GetLikes(int userId, int recipientId);
        Task<Message> GetMessage(int id);
        Task<PageList<Message>> GetMessagesForUser(MessageParams messageParams);
        Task<IEnumerable<Message>> GetMessagesThread(int userId, int recipientId);
    }
}