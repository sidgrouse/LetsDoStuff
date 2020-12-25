using Microsoft.EntityFrameworkCore.Migrations;

namespace LetsDoStuff.Domain.Migrations
{
    public partial class RenamingIsParticipant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsParticipante",
                table: "ParticipantsTicket",
                newName: "IsParticipant");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsParticipant",
                table: "ParticipantsTicket",
                newName: "IsParticipante");
        }
    }
}
