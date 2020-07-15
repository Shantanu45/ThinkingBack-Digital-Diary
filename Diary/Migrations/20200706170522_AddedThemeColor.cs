using Microsoft.EntityFrameworkCore.Migrations;

namespace Diary.Migrations
{
    public partial class AddedThemeColor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "themeColor",
                table: "Post",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "themeColor",
                table: "Post");
        }
    }
}
