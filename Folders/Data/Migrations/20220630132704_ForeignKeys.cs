using Microsoft.EntityFrameworkCore.Migrations;

namespace Folders.Data.Migrations
{
    public partial class ForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Permission_FolderId",
                table: "Permission",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Folder_ParentId",
                table: "Folder",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_File_FolderId",
                table: "File",
                column: "FolderId");

            migrationBuilder.AddForeignKey(
                name: "FK_File_Folder_FolderId",
                table: "File",
                column: "FolderId",
                principalTable: "Folder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Folder_Folder_ParentId",
                table: "Folder",
                column: "ParentId",
                principalTable: "Folder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Permission_Folder_FolderId",
                table: "Permission",
                column: "FolderId",
                principalTable: "Folder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_File_Folder_FolderId",
                table: "File");

            migrationBuilder.DropForeignKey(
                name: "FK_Folder_Folder_ParentId",
                table: "Folder");

            migrationBuilder.DropForeignKey(
                name: "FK_Permission_Folder_FolderId",
                table: "Permission");

            migrationBuilder.DropIndex(
                name: "IX_Permission_FolderId",
                table: "Permission");

            migrationBuilder.DropIndex(
                name: "IX_Folder_ParentId",
                table: "Folder");

            migrationBuilder.DropIndex(
                name: "IX_File_FolderId",
                table: "File");
        }
    }
}
