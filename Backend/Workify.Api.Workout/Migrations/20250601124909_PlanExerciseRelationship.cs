using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Workify.Api.Workout.Migrations
{
    /// <inheritdoc />
    public partial class PlanExerciseRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExercisePlan");

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
                table: "PredefinedPlans",
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

            migrationBuilder.DeleteData(
                table: "AllExercises",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AllExercises",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AllExercises",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AllExercises",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "PredefinedPlans",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PredefinedPlans",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PredefinedPlans",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.CreateTable(
                name: "ExercisePlan",
                columns: table => new
                {
                    ExercisesId = table.Column<int>(type: "integer", nullable: false),
                    PlanId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExercisePlan", x => new { x.ExercisesId, x.PlanId });
                    table.ForeignKey(
                        name: "FK_ExercisePlan_AllExercises_ExercisesId",
                        column: x => x.ExercisesId,
                        principalTable: "AllExercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExercisePlan_PlanId",
                table: "ExercisePlan",
                column: "PlanId");
        }
    }
}
