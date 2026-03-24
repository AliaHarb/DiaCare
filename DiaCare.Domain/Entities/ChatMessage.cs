using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiaCare.Domain.Entities
{
    public class ChatMessage
    {
        public int Id { get; set; }
        
        public string UserId { get; set; }
        
        public string Content { get; set; }
        
        public DateTime Timestamp { get; set; } = DateTime.Now; 

        public int SenderType { get; set; } // 0=User, 1=AI 
       //  Navigation
        public ApplicationUser User { get; set; }

    }
}
