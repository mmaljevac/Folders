using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Folders.Models;

namespace Folders
{
    public class FolderViewModel
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public int Depth { get; set; }
        public List<FolderViewModel> ChildFolders { get; set; }
        public List<File> Files { get; set; }
    }
}
