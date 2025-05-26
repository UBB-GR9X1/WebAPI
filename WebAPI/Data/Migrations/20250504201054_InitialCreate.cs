using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    userId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    mail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "User"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    birthDate = table.Column<DateOnly>(type: "date", nullable: false),
                    cnp = table.Column<string>(type: "nchar(13)", fixedLength: true, maxLength: 13, nullable: false),
                    address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    phoneNumber = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true),
                    registrationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.userId);
                });

            migrationBuilder.CreateTable(
                name: "Doctors",
                columns: table => new
                {
                    userId = table.Column<int>(type: "int", nullable: false),
                    departmentId = table.Column<int>(type: "int", nullable: false),
                    doctorRating = table.Column<double>(type: "float", nullable: false, defaultValue: 0.0),
                    licenseNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctors", x => x.userId);
                    // Foreign keys will be added later outside this block
                });

            // Create indexes and foreign keys for Doctors *after* table creation:
            migrationBuilder.CreateIndex(
                name: "IX_Doctors_departmentId",
                table: "Doctors",
                column: "departmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Departments_departmentId",
                table: "Doctors",
                column: "departmentId",
                principalTable: "Departments",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Users_userId",
                table: "Doctors",
                column: "userId",
                principalTable: "Users",
                principalColumn: "userId",
                onDelete: ReferentialAction.Cascade);

            // Repeat the same pattern for other tables like Logs, Notifications, Patients:
            // Create tables, then create indexes and add foreign keys outside of CreateTable call.

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    logId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<int>(type: "int", nullable: true),
                    actionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    timestamp = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.logId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Logs_userId",
                table: "Logs",
                column: "userId",
                filter: "[userId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Users_userId",
                table: "Logs",
                column: "userId",
                principalTable: "Users",
                principalColumn: "userId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    notificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<int>(type: "int", nullable: false),
                    deliveryDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    message = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.notificationId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_userId",
                table: "Notifications",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_userId",
                table: "Notifications",
                column: "userId",
                principalTable: "Users",
                principalColumn: "userId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    userId = table.Column<int>(type: "int", nullable: false),
                    bloodType = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    emergencyContact = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false),
                    allergies = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    weight = table.Column<double>(type: "float", nullable: false),
                    height = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.userId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Users_userId",
                table: "Patients",
                column: "userId",
                principalTable: "Users",
                principalColumn: "userId",
                onDelete: ReferentialAction.Cascade);

            // Unique indexes on Users
            migrationBuilder.CreateIndex(
                name: "IX_Users_cnp",
                table: "Users",
                column: "cnp",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_mail",
                table: "Users",
                column: "mail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_username",
                table: "Users",
                column: "username",
                unique: true);
        }

    }
}
