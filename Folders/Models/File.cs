using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Folders.Models
{
    public class File
    {
        public int Id { get; set; }
        public int FolderId { get; set; }
        public string Name { get; set; }

        public virtual Folder Folder { get; set; }
    }
}
