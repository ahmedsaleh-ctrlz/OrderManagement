using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedWarehouseUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseUser_Users_UserId",
                table: "WarehouseUser");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseUser_Warehouses_WarehouseId",
                table: "WarehouseUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WarehouseUser",
                table: "WarehouseUser");

            migrationBuilder.RenameTable(
                name: "WarehouseUser",
                newName: "WarehouseUsers");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseUser_WarehouseId",
                table: "WarehouseUsers",
                newName: "IX_WarehouseUsers_WarehouseId");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseUser_UserId",
                table: "WarehouseUsers",
                newName: "IX_WarehouseUsers_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WarehouseUsers",
                table: "WarehouseUsers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseUsers_Users_UserId",
                table: "WarehouseUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseUsers_Warehouses_WarehouseId",
                table: "WarehouseUsers",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseUsers_Users_UserId",
                table: "WarehouseUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseUsers_Warehouses_WarehouseId",
                table: "WarehouseUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WarehouseUsers",
                table: "WarehouseUsers");

            migrationBuilder.RenameTable(
                name: "WarehouseUsers",
                newName: "WarehouseUser");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseUsers_WarehouseId",
                table: "WarehouseUser",
                newName: "IX_WarehouseUser_WarehouseId");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseUsers_UserId",
                table: "WarehouseUser",
                newName: "IX_WarehouseUser_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WarehouseUser",
                table: "WarehouseUser",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseUser_Users_UserId",
                table: "WarehouseUser",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseUser_Warehouses_WarehouseId",
                table: "WarehouseUser",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
