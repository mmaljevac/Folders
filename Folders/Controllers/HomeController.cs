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
            IEnumerable<Folder> foldersList = _context.Folders;
            IEnumerable<Permission> permissionsList = _context.Permissions;

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            foreach (Permission p in permissionsList)
            {
                if (p.UserId == userId)
                {
                    int folderId = p.FolderId;
                    ViewBag.folderPermissionId = folderId;
                }
            }

            return View(foldersList);
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
