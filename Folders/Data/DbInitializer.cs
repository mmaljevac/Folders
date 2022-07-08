using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Folders.Models;
using Microsoft.AspNetCore.Identity;

namespace Folders.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Folders.Any())
            {
                return;
            }

            var folders = new Folder[]
            {
                new Folder{Name="root",Depth=0,ParentId=null}, // Id: 1
                new Folder{Name="Shared",Depth=0,ParentId=1}, // Id: 2
                new Folder{Name="Matija",Depth=0,ParentId=1}, // Id: 3
                new Folder{Name="Ivan",Depth=0,ParentId=1}, // Id: 4
                new Folder{Name="Vito",Depth=0,ParentId=1}, // Id: 5
                new Folder{Name="Edo",Depth=0,ParentId=1}, // Id: 6
                new Folder{Name="Pictures",Depth=1,ParentId=3}, // Id: 7
                new Folder{Name="Videos",Depth=1,ParentId=3}, // Id: 8
                new Folder{Name="College",Depth=1,ParentId=3}, // Id: 9
                new Folder{Name="Other",Depth=1,ParentId=3}, // Id: 10
                new Folder{Name="Materials",Depth=2,ParentId=9}, // Id: 11
                new Folder{Name="Projects",Depth=2,ParentId=9}, // Id: 12
                new Folder{Name="Tests",Depth=2,ParentId=9}, // Id: 13
                new Folder{Name="Other",Depth=1,ParentId=2}, // Id: 14
            };

            foreach (Folder f in folders)
            {
                context.Folders.Add(f);
            }
            context.SaveChanges();

            var files = new File[]
            {
                new File{Name="Utilis terms and conditions.docx",FolderId=2}, // Id: 1
                new File{Name="wallpaper.jpg",FolderId=7}, // Id: 2
                new File{Name="sunset.jpg",FolderId=7}, // Id: 3
                new File{Name="cat.mp4",FolderId=8}, // Id: 4
                new File{Name="ASP.NET MVC Course.pdf",FolderId=11}, // Id: 5
                new File{Name="C# for Beginners.pdf",FolderId=11}, // Id: 6
                new File{Name="Presentation.pptx",FolderId=12}, // Id: 7
                new File{Name="C# test examples.pdf",FolderId=13}, // Id: 8
                new File{Name="notes.txt",FolderId=10}, // Id: 9
                new File{Name="todo.docx",FolderId=2}, // Id: 10
                new File{Name="ideas.txt",FolderId=2}, // Id: 11
                new File{Name="lunch_menu.jpg",FolderId=14}, // Id: 12
            };

            foreach (File f in files)
            {
                context.Files.Add(f);
            }
            context.SaveChanges();


            var user = new IdentityUser
            {
                UserName = "matija@utilis.biz",
                NormalizedUserName = "MATIJA@UTILIS.BIZ",
                Email = "matija@utilis.biz",
                NormalizedEmail = "MATIJA@UTILIS.BIZ",
                EmailConfirmed = true
            };
            string password = "matija123";
            PasswordHasher<IdentityUser> hasher = new PasswordHasher<IdentityUser>();
            user.PasswordHash = hasher.HashPassword(user, password);

            context.Users.Add(user);
            context.SaveChanges();

            var permissions = new Permission[]
            {
                new Permission{UserId=user.Id,FolderId=3}
            };

            foreach (Permission p in permissions)
            {
                context.Permissions.Add(p);
            }
            context.SaveChanges();
        }
    }
}
