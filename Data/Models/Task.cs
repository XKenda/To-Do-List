using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace To_Do_List.Data.Models
{
    public class Task
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Discription { get; set; }

        public bool IsCompleted { get; set; }

        public int Priority { get; set; }

        public int UserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}