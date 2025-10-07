using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserAPI.Migrations
{
    /// <inheritdoc />
    public partial class MyFirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BoeingUsers",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "varchar(25)", nullable: true),
                    LastName = table.Column<string>(type: "varchar(25)", nullable: true),
                    Email = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoeingUsers", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "BoeingRoles",
                columns: table => new
                {
                    RoleId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "varchar(20)", nullable: false),
                    UserIdFK = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoeingRoles", x => x.RoleId);
                    table.ForeignKey(
                        name: "FK_BoeingRoles_BoeingUsers_UserIdFK",
                        column: x => x.UserIdFK,
                        principalTable: "BoeingUsers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoeingRoles_UserIdFK",
                table: "BoeingRoles",
                column: "UserIdFK");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoeingRoles");

            migrationBuilder.DropTable(
                name: "BoeingUsers");
        }
    }
}
