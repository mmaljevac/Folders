using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Folders.Models
{
    public class Folder
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }
        
    }
}
