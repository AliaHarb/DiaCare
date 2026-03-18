using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiaCare.Domain.Entities
{
    public class Article
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
       
        public string Content { get; set; }
       
        public string Category { get; set; }
       
        public string? ImageUrl { get; set; }
        public DateTime PublishDate { get; set; } = DateTime.Now; 
    }
}
