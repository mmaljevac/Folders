using Microsoft.EntityFrameworkCore.Migrations;

namespace Folders.Data.Migrations
{
    public partial class FolderViewModelsCRUD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FolderViewModelId",
                table: "File",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FolderViewModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Depth = table.Column<int>(type: "int", nullable: false),
                    FolderViewModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FolderViewModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FolderViewModel_FolderViewModel_FolderViewModelId",
                        column: x => x.FolderViewModelId,
                        principalTable: "FolderViewModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_File_FolderViewModelId",
                table: "File",
                column: "FolderViewModelId");

            migrationBuilder.CreateIndex(
                name: "IX_FolderViewModel_FolderViewModelId",
                table: "FolderViewModel",
                column: "FolderViewModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_File_FolderViewModel_FolderViewModelId",
                table: "File",
                column: "FolderViewModelId",
                principalTable: "FolderViewModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_File_FolderViewModel_FolderViewModelId",
                table: "File");

            migrationBuilder.DropTable(
                name: "FolderViewModel");

            migrationBuilder.DropIndex(
                name: "IX_File_FolderViewModelId",
                table: "File");

            migrationBuilder.DropColumn(
                name: "FolderViewModelId",
                table: "File");
        }
    }
}
