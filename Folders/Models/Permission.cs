using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Folders.Models
{
    public class Permission
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int FolderId { get; set; }

        public virtual Folder Folder { get; set; }
    }
}
