using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Workify.Api.Workout.Database.Migrations
{
    /// <inheritdoc />
    public partial class ReCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AllExercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    BodyPart = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(1023)", maxLength: 1023, nullable: true),
                    ExerciseType = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllExercises", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "PlanExercise",
                columns: table => new
                {
                    PlanId = table.Column<int>(type: "integer", nullable: false),
                    ExerciseId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanExercise", x => new { x.ExerciseId, x.PlanId });
                    table.ForeignKey(
                        name: "FK_PlanExercise_AllExercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "AllExercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanExercise_Plan_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PredefinedPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PredefinedPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PredefinedPlans_Plan_Id",
                        column: x => x.Id,
                        principalTable: "Plan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPlans_Plan_Id",
                        column: x => x.Id,
                        principalTable: "Plan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AllExercises",
                columns: new[] { "Id", "BodyPart", "Description", "ExerciseType", "Name" },
                values: new object[,]
                {
                    { 1, 5, null, 0, "Bench press" },
                    { 2, 8, null, 0, "Squat" },
                    { 3, 8, null, 0, "Deadlift" },
                    { 4, 1, null, 0, "Rows" }
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

            migrationBuilder.InsertData(
                table: "PlanExercise",
                columns: new[] { "ExerciseId", "PlanId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 2, 1 },
                    { 2, 3 },
                    { 3, 1 },
                    { 3, 3 },
                    { 4, 1 },
                    { 4, 2 }
                });

            migrationBuilder.InsertData(
                table: "PredefinedPlans",
                column: "Id",
                values: new object[]
                {
                    1,
                    2,
                    3
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlanExercise_PlanId",
                table: "PlanExercise",
                column: "PlanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlanExercise");

            migrationBuilder.DropTable(
                name: "PredefinedPlans");

            migrationBuilder.DropTable(
                name: "UserPlans");

            migrationBuilder.DropTable(
                name: "AllExercises");

            migrationBuilder.DropTable(
                name: "Plan");
        }
    }
}
