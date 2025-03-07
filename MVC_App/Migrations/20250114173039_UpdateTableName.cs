using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC_App.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Calls",
                columns: table => new
                {
                    Call_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Calls__19E6F4EBB48E76CB", x => x.Call_ID);
                });

            migrationBuilder.CreateTable(
                name: "Programms",
                columns: table => new
                {
                    ProgrammName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Benfits = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Charge = table.Column<decimal>(type: "decimal(5,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Programs__4F925711BFB93566", x => x.ProgrammName);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    User_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    First_Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Last_Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users__206A9DF89E4F90D1", x => x.User_id);
                });

            migrationBuilder.CreateTable(
                name: "Phones",
                columns: table => new
                {
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    ProgrammName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Phones__85FB4E395D793215", x => x.PhoneNumber);
                    table.ForeignKey(
                        name: "FK__Phones__ProgramN__440B1D61",
                        column: x => x.ProgrammName,
                        principalTable: "Programms",
                        principalColumn: "ProgrammName");
                });

            migrationBuilder.CreateTable(
                name: "Admin",
                columns: table => new
                {
                    Admin_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Admin__4A3006F7F49AC148", x => x.Admin_Id);
                    table.ForeignKey(
                        name: "FK__Admin__User_id__3F466844",
                        column: x => x.User_id,
                        principalTable: "Users",
                        principalColumn: "User_id");
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Client_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AFM = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    phoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    User_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Clients__75A5D7182B365D98", x => x.Client_ID);
                    table.ForeignKey(
                        name: "FK__Clients__User_id__3C69FB99",
                        column: x => x.User_id,
                        principalTable: "Users",
                        principalColumn: "User_id");
                });

            migrationBuilder.CreateTable(
                name: "Sellers",
                columns: table => new
                {
                    Seller_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Sellers__016148B1D10B493F", x => x.Seller_id);
                    table.ForeignKey(
                        name: "FK__Sellers__User_id__398D8EEE",
                        column: x => x.User_id,
                        principalTable: "Users",
                        principalColumn: "User_id");
                });

            migrationBuilder.CreateTable(
                name: "Bills",
                columns: table => new
                {
                    Bill_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Costs = table.Column<decimal>(type: "decimal(7,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Bills__CF6E7D4369376886", x => x.Bill_ID);
                    table.ForeignKey(
                        name: "FK__Bills__PhoneNumb__46E78A0C",
                        column: x => x.PhoneNumber,
                        principalTable: "Phones",
                        principalColumn: "PhoneNumber");
                });

            migrationBuilder.CreateTable(
                name: "BillsCalls",
                columns: table => new
                {
                    Bill_ID = table.Column<int>(type: "int", nullable: false),
                    Call_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__BillsCal__6EF0120DAF4D5708", x => new { x.Bill_ID, x.Call_ID });
                    table.ForeignKey(
                        name: "FK__BillsCall__Bill___4BAC3F29",
                        column: x => x.Bill_ID,
                        principalTable: "Bills",
                        principalColumn: "Bill_ID");
                    table.ForeignKey(
                        name: "FK__BillsCall__Call___4CA06362",
                        column: x => x.Call_ID,
                        principalTable: "Calls",
                        principalColumn: "Call_ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admin_User_id",
                table: "Admin",
                column: "User_id");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_PhoneNumber",
                table: "Bills",
                column: "PhoneNumber");

            migrationBuilder.CreateIndex(
                name: "IX_BillsCalls_Call_ID",
                table: "BillsCalls",
                column: "Call_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_User_id",
                table: "Clients",
                column: "User_id");

            migrationBuilder.CreateIndex(
                name: "IX_Phones_ProgrammName",
                table: "Phones",
                column: "ProgrammName");

            migrationBuilder.CreateIndex(
                name: "IX_Sellers_User_id",
                table: "Sellers",
                column: "User_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admin");

            migrationBuilder.DropTable(
                name: "BillsCalls");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Sellers");

            migrationBuilder.DropTable(
                name: "Bills");

            migrationBuilder.DropTable(
                name: "Calls");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Phones");

            migrationBuilder.DropTable(
                name: "Programms");
        }
    }
}
