using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicalFlow.Migrations
{
    /// <inheritdoc />
    public partial class AddPatientApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_ApplicationUsers_ApplicationUserId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_ApplicationUserId",
                table: "Patients");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_ApplicationUserId",
                table: "Patients",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_ApplicationUsers_ApplicationUserId",
                table: "Patients",
                column: "ApplicationUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "ApplicationUserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_ApplicationUsers_ApplicationUserId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_ApplicationUserId",
                table: "Patients");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_ApplicationUserId",
                table: "Patients",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_ApplicationUsers_ApplicationUserId",
                table: "Patients",
                column: "ApplicationUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "ApplicationUserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
