using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataCloud.PipelineDesigner.Repositories.Migrations
{
    public partial class DbUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Pipline",
                table: "BaseEntities",
                newName: "Workflow");

            migrationBuilder.AddColumn<string>(
                name: "Owner",
                table: "BaseEntities",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Owner",
                table: "BaseEntities");

            migrationBuilder.RenameColumn(
                name: "Workflow",
                table: "BaseEntities",
                newName: "Pipline");
        }
    }
}
