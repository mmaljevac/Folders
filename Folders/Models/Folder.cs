using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Folders.Models
{
    public class Folder
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public int Depth { get; set; }
        
        public virtual List<File> Files { get; set; }

        public virtual List<Folder> ChildFolders { get; set; }

        public virtual Folder ParentFolder { get; set; }

        public virtual List<Permission> Permissions { get; set; }
    }
}
