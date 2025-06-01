using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Workify.Api.Workout.Migrations
{
    /// <inheritdoc />
    public partial class MoveToTptDueToTpcProblems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "UserPlans");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "UserPlans");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "PredefinedPlans");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "PredefinedPlans");

            migrationBuilder.DropSequence(
                name: "PlanSequence");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "UserPlans",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValueSql: "nextval('\"PlanSequence\"')");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "PredefinedPlans",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValueSql: "nextval('\"PlanSequence\"')");

            migrationBuilder.CreateTable(
                name: "Plan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "character varying(1023)", maxLength: 1023, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plan", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Plan",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, null, "FBW" },
                    { 2, null, "Upper body" },
                    { 3, null, "Lower body" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_PlanExercise_Plan_PlanId",
                table: "PlanExercise",
                column: "PlanId",
                principalTable: "Plan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PredefinedPlans_Plan_Id",
                table: "PredefinedPlans",
                column: "Id",
                principalTable: "Plan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPlans_Plan_Id",
                table: "UserPlans",
                column: "Id",
                principalTable: "Plan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanExercise_Plan_PlanId",
                table: "PlanExercise");

            migrationBuilder.DropForeignKey(
                name: "FK_PredefinedPlans_Plan_Id",
                table: "PredefinedPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPlans_Plan_Id",
                table: "UserPlans");

            migrationBuilder.DropTable(
                name: "Plan");

            migrationBuilder.CreateSequence(
                name: "PlanSequence");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "UserPlans",
                type: "integer",
                nullable: false,
                defaultValueSql: "nextval('\"PlanSequence\"')",
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "UserPlans",
                type: "character varying(1023)",
                maxLength: 1023,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "UserPlans",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "PredefinedPlans",
                type: "integer",
                nullable: false,
                defaultValueSql: "nextval('\"PlanSequence\"')",
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "PredefinedPlans",
                type: "character varying(1023)",
                maxLength: 1023,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "PredefinedPlans",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "PredefinedPlans",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Name" },
                values: new object[] { null, "FBW" });

            migrationBuilder.UpdateData(
                table: "PredefinedPlans",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "Name" },
                values: new object[] { null, "Upper body" });

            migrationBuilder.UpdateData(
                table: "PredefinedPlans",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Name" },
                values: new object[] { null, "Lower body" });
        }
    }
}
