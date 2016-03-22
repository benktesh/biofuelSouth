using System;

namespace BiofuelSouth.Models.Entity
{
    public class FeedBackEntity
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public String Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Message { get; set; }

    }
}