using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MakingSolutions.Desenv.WebApi.Infra.Migrations
{
    public partial class Add_Prop_Message_Body : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Body",
                table: "Message",
                type: "nvarchar(MAX)",
                maxLength: 2147483647,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Body",
                table: "Message");
        }
    }
}
