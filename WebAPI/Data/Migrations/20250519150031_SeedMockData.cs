using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedMockData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "userId", "username", "password", "mail", "role", "name", "birthDate", "cnp", "address", "phoneNumber", "registrationDate" },
                values: new object[,]
                {
                    { 1, "robert", "robert", "robert.beres@stud.ubbfsega.com", "Patient", "Robycu", new DateTime(2004, 7, 6), "5040706230045", null, "0756504556", new DateTime(2025, 5, 19, 18, 0, 0) },
                    { 2, "balau", "balau", "mihai.balau@stud.utcn.com", "Doctor", "MihaiBombastik", new DateTime(2005, 7, 6), "5050706230045", null, "0754504556", new DateTime(2025, 5, 19, 18, 0, 0) },
                    { 3, "bolos", "bolos", "mihai.bolos@stud.ubbfsega.com", "Patient", "Mihai", new DateTime(2003, 5, 2), "5030502230045", null, "0744503556", new DateTime(2025, 5, 19, 18, 0, 0) },
                    { 4, "paul", "paul", "paul.berca@stud.ubbcs.com", "Admin", "Paulino", new DateTime(2002, 5, 3), "5020503230045", null, "0754644556", new DateTime(2025, 5, 19, 18, 0, 0) }
                }
            );

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "notificationId", "userId", "deliveryDateTime", "message" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 5, 19, 18, 0, 0), "Hey mr. Robert, you have an appointment!" },
                    { 2, 1, new DateTime(2025, 5, 19, 18, 0, 0), "Hello, you missed an appointment!" },
                    { 3, 1, new DateTime(2025, 5, 19, 18, 0, 0), "Bro, you are late for your appointment!" },
                    { 4, 2, new DateTime(2025, 5, 19, 18, 0, 0), "Hey mr. Robert, you have an appointment!" },
                    { 5, 2, new DateTime(2025, 5, 19, 18, 0, 0), "Hello, you missed an appointment!" },
                    { 6, 2, new DateTime(2025, 5, 19, 18, 0, 0), "Bro, you are late for your appointment!" }
                }
            );

            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "userId", "bloodType", "emergencyContact", "allergies", "weight", "height" },
                values: new object[,]
                {
                    { 1, "AB", "0743509334", "none", 90.0, 195 },
                    { 3, "B", "0744092333", "pollen", 80.0, 195 }
                }
            );

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "Cardiology" }
                }
            );

            migrationBuilder.InsertData(
                table: "Doctors",
                columns: new[] { "userId", "departmentId", "doctorRating", "licenseNumber" },
                values: new object[,]
                {
                    { 2, 1, 4.5, "UBBMED9933" }
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Delete from children tables first respecting FK constraints
            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "userId",
                keyValue: 2
            );

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "userId",
                keyValues: new object[] { 1, 3 }
            );

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "notificationId",
                keyValues: new object[] { 1, 2, 3, 4, 5, 6 }
            );

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "id",
                keyValue: 1
            );

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "userId",
                keyValues: new object[] { 1, 2, 3, 4 }
            );
        }
    }
}
