using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTwitter.Models
{
   public class Tweet
    {
        public DateTime CreateAt { get; set; }
        public String Id { get; set; }
        public String Text { get; set; }
        public String UserId { get; set; }
        public String UserName { get; set; }
        public String UserScreenName { get; set; }
        public String UserBackgroundImageUrl { get; set; }
        public String UserProfileImageUrl { get; set; }
        public List<String> Hashtags { get; set; }
        //public List<String> UserMentions { get; set; }
        public List<String> Urls { get; set; }
    }
}
