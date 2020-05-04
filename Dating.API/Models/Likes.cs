namespace Dating.API.Models
{
    public class Likes
    {
        public int LikerId { get; set; }
        public int LikeeID { get; set; }
        public User Likers { get; set; }
        public User Likee { get; set; }
    }
}