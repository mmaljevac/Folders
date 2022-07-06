using Folders.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Folders.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Folders.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index(int id)
        {
            ViewBag.id = id;

            IEnumerable<Folder> foldersList = _context.Folders.ToList();
            List<File> filesList = _context.Files.Where(i => i.FolderId == id).ToList();
            filesList = filesList.OrderBy(i => i.Name).ToList();
            var folderViewModels = new List<FolderViewModel>();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<Permission> permissionsList = _context.Permissions.Where(p => p.UserId == userId);
            List<int> folderPermissionIds = permissionsList.Select(p => p.FolderId).ToList();

            int maxDepth = foldersList.Max(i => i.Depth);

            for (int depth = 0; depth <= maxDepth; depth++)
            {
                foreach (Folder folder in foldersList.Where(f => f.Depth == depth))
                {
                    if (depth == 0 && folder.ParentId != null && folderPermissionIds.Contains(folder.Id)) // folder permissions
                    {
                        folderViewModels.Add(new FolderViewModel
                        {
                            Id = folder.Id,
                            ParentId = folder.ParentId,
                            Name = folder.Name,
                            Depth = folder.Depth,
                            ChildFolders = new List<FolderViewModel>(),
                            //Files = new List<File>(filesList.Where(file => file.FolderId == folder.Id))
                        });
                    }
                    else
                    {
                        FindParent(folder, folderViewModels, depth, filesList);
                    }
                }
            }

            folderViewModels = SortFolders(folderViewModels);

            FolderViewModel selectedFolderModel = null;

            if (id != 0)
            {
                Folder selectedFolder = foldersList.Single(i => i.Id == id);
                selectedFolderModel = new FolderViewModel { Id = selectedFolder.Id, Name = selectedFolder.Name, Files = filesList };
            }

            return View(new MainViewModel() {Folders = folderViewModels, SelectedFolder = selectedFolderModel } );
        }

        public void FindParent(Folder folder, List<FolderViewModel> folderViewModels, int depth, IEnumerable<File> filesList)
        {
            var parentFolder = folderViewModels.Find(i => i.Id == folder.ParentId);
            if (parentFolder != null)
            {
                FolderViewModel childFolder = new FolderViewModel
                {
                    Id = folder.Id,
                    ParentId = folder.ParentId,
                    Name = folder.Name,
                    Depth = folder.Depth,
                    ChildFolders = new List<FolderViewModel>(),
                    //Files = new List<File>(filesList.Where(file => file.FolderId == folder.Id))
                };
                parentFolder.ChildFolders.Add(childFolder);
            }
            else
            {
                foreach (var folderViewModel in folderViewModels)
                {
                    FindParent(folder, folderViewModel.ChildFolders, depth, filesList);
                }
            }
        }

        public List<FolderViewModel> SortFolders(List<FolderViewModel> folderViewModels)
        {
            folderViewModels = folderViewModels.OrderBy(i => i.Name).ToList();
            foreach (var folder in folderViewModels)
            {
                folder.ChildFolders = SortFolders(folder.ChildFolders);
            }
            return folderViewModels;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
