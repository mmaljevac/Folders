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

        public IActionResult Index()
        {
            IEnumerable<Folder> foldersList = _context.Folders.ToList();
            var folderViewModels = new List<FolderViewModel>();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<Permission> permissionsList = _context.Permissions.Where(p => p.UserId == userId);
            List<int> folderPermissionIds = permissionsList.Select(p => p.FolderId).ToList();

            int maxDepth = foldersList.Max(i => i.Depth);

            for (int depth = 0; depth <= maxDepth; depth++)
            {
                foreach (Folder folder in foldersList.Where(f => f.Depth == depth))
                {
                    if (depth == 0 && folderPermissionIds.Contains(folder.Id)) // folder permissions
                    {
                        folderViewModels.Add(new FolderViewModel
                        {
                            Id = folder.Id,
                            ParentId = folder.ParentId,
                            Name = folder.Name,
                            Depth = folder.Depth,
                            ChildFolders = new List<FolderViewModel>()
                        });
                    }
                    else
                    {
                        findParent(folder, folderViewModels, depth);
                    }
                }
            }

            folderViewModels = sortFolders(folderViewModels);

            return View(folderViewModels);
        }

        public void findParent(Folder folder, List<FolderViewModel> folderViewModels, int depth)
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
                    ChildFolders = new List<FolderViewModel>()
                };
                parentFolder.ChildFolders.Add(childFolder);
            }
            else
            {
                foreach (var folderViewModel in folderViewModels)
                {
                    findParent(folder, folderViewModel.ChildFolders, depth);
                }
            }
        }

        public List<FolderViewModel> sortFolders(List<FolderViewModel> folderViewModels)
        {
            folderViewModels = folderViewModels.OrderBy(i => i.Name).ToList();
            foreach (var folder in folderViewModels)
            {
                folder.ChildFolders = sortFolders(folder.ChildFolders);
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
