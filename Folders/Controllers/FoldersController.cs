using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Folders.Data;
using Folders.Models;
using System.Security.Claims;

namespace Folders.Controllers
{
    public class FoldersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FoldersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Folders
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Folders.Include(f => f.ParentFolder);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Folders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var folder = await _context.Folders
                .Include(f => f.ParentFolder)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (folder == null)
            {
                return NotFound();
            }

            return View(folder);
        }

        // GET: Folders/Create
        public IActionResult Create(int? id)
        {
            if (!id.HasValue)
            {
                ViewData["ParentId"] = new SelectList(_context.Folders, "Id", "Name");
            }
            else
            {
                ViewData["ParentName"] = _context.Folders.First(i => i.Id == id).Name;
            }
            return View(new Folder() { ParentId = id.HasValue ? id.Value : 0 });
        }

        // POST: Folders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ParentId,Name,Depth")] Folder folder, int? id)
        {
            if (ModelState.IsValid)
            {
                if (!id.HasValue)
                {
                    folder.Depth = 0;
                    folder.ParentId = 19;
                }
                else
                {
                    var parentFolder = _context.Folders.First(i => i.Id == id);
                    folder.Depth = parentFolder.Depth + 1;
                    folder.ParentId = parentFolder.Id;
                }
                _context.Add(folder);
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _context.Permissions.Add(new Permission { Folder = folder, UserId = userId });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), "Home", new { id = folder.Id });
            }
            ViewData["ParentId"] = new SelectList(_context.Folders, "Id", "Id", folder.ParentId);
            return View(folder);
        }

        // GET: Folders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var folder = await _context.Folders.FindAsync(id);
            if (folder == null)
            {
                return NotFound();
            }
            ViewData["ParentId"] = new SelectList(_context.Folders, "Id", "Name", folder.ParentId);
            return View(folder);
        }

        // POST: Folders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ParentId,Name,Depth")] Folder folder)
        {
            if (id != folder.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(folder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FolderExists(folder.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), "Home", new { id = folder.Id });
            }
            ViewData["ParentId"] = new SelectList(_context.Folders, "Id", "Id", folder.ParentId);
            return View(folder);
        }

        // GET: Folders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var folder = await _context.Folders
                .Include(f => f.ParentFolder)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (folder == null)
            {
                return NotFound();
            }

            return View(folder);
        }

        // POST: Folders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var folder = await _context.Folders.FindAsync(id);
            DeleteChildFoldersAndFiles(folder, _context);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), "Home", new { id = folder.ParentId });
        }

        private void DeleteChildFoldersAndFiles(Folder folder, ApplicationDbContext _context)
        {
            foreach (Folder child in _context.Folders.Where(i => i.ParentId == folder.Id))
            {
                DeleteChildFoldersAndFiles(child, _context);
            }
            foreach (File f in _context.Files.Where(i => i.FolderId == folder.Id))
            {
                _context.Files.Remove(f);
            }
            _context.Folders.Remove(folder);
        }

        private bool FolderExists(int id)
        {
            return _context.Folders.Any(e => e.Id == id);
        }
    }
}
