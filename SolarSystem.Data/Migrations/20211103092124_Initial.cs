using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SolarSystem.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(name: "First Name", type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(name: "Last Name", type: "nvarchar(max)", nullable: true),
                    DayofBirth = table.Column<DateTime>(name: "Day of Birth", type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(name: "Phone Number", type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Components",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(name: "Created At", type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(name: "Updated At", type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Components", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DistanceToTheSunAU = table.Column<double>(name: "Distance To The Sun (AU)", type: "float", nullable: false),
                    CreatedAt = table.Column<DateTime>(name: "Created At", type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(name: "Updated At", type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bodies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EarthMassAU = table.Column<double>(name: "Earth Mass (AU)", type: "float", nullable: false),
                    DistanceToTheSunAU = table.Column<double>(name: "Distance To The Sun (AU)", type: "float", nullable: false),
                    CreatedAt = table.Column<DateTime>(name: "Created At", type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(name: "Updated At", type: "datetime2", nullable: false),
                    ComponentId = table.Column<int>(name: "Component Id", type: "int", nullable: false),
                    RegionId = table.Column<int>(name: "Region Id", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bodies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bodies_Components_Component Id",
                        column: x => x.ComponentId,
                        principalTable: "Components",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bodies_Regions_Region Id",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "D8E9592C-A965-4F5D-BD48-99F35902FD24", "eea82b05-2268-4336-84fc-e2ad8e409e61", "Admin", "ADMIN" },
                    { "cc3695c4-a351-43f6-a70f-81bfe584d262", "a70a0a99-4b8b-4382-b0f7-cb07cbcba04b", "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "Components",
                columns: new[] { "Id", "Created At", "Name", "Type", "Updated At" },
                values: new object[,]
                {
                    { 1, new DateTime(2021, 11, 3, 16, 21, 23, 478, DateTimeKind.Local).AddTicks(9247), "Star", "G2 main-sequence star", new DateTime(2021, 11, 3, 16, 21, 23, 478, DateTimeKind.Local).AddTicks(9623) },
                    { 2, new DateTime(2021, 11, 3, 16, 21, 23, 478, DateTimeKind.Local).AddTicks(9937), "Rocky Planet", "Rocky Planet", new DateTime(2021, 11, 3, 16, 21, 23, 478, DateTimeKind.Local).AddTicks(9943) },
                    { 3, new DateTime(2021, 11, 3, 16, 21, 23, 478, DateTimeKind.Local).AddTicks(9944), "Gas Planet", "Gas Planet", new DateTime(2021, 11, 3, 16, 21, 23, 478, DateTimeKind.Local).AddTicks(9945) }
                });

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "Id", "Created At", "Distance To The Sun (AU)", "Name", "Updated At" },
                values: new object[,]
                {
                    { 1, new DateTime(2021, 11, 3, 16, 21, 23, 475, DateTimeKind.Local).AddTicks(6085), 5.0, "Inner Solar System", new DateTime(2021, 11, 3, 16, 21, 23, 476, DateTimeKind.Local).AddTicks(5444) },
                    { 2, new DateTime(2021, 11, 3, 16, 21, 23, 476, DateTimeKind.Local).AddTicks(5932), 30.100000000000001, "Outer Solar System", new DateTime(2021, 11, 3, 16, 21, 23, 476, DateTimeKind.Local).AddTicks(5938) },
                    { 3, new DateTime(2021, 11, 3, 16, 21, 23, 476, DateTimeKind.Local).AddTicks(5941), 68.0, "Trans-Neptunian", new DateTime(2021, 11, 3, 16, 21, 23, 476, DateTimeKind.Local).AddTicks(5942) }
                });

            migrationBuilder.InsertData(
                table: "Bodies",
                columns: new[] { "Id", "Component Id", "Created At", "Distance To The Sun (AU)", "Earth Mass (AU)", "Name", "Region Id", "Updated At" },
                values: new object[] { 1, 1, new DateTime(2021, 11, 3, 16, 21, 23, 484, DateTimeKind.Local).AddTicks(1253), 0.0, 332900.0, "Sun", 1, new DateTime(2021, 11, 3, 16, 21, 23, 484, DateTimeKind.Local).AddTicks(1599) });

            migrationBuilder.InsertData(
                table: "Bodies",
                columns: new[] { "Id", "Component Id", "Created At", "Distance To The Sun (AU)", "Earth Mass (AU)", "Name", "Region Id", "Updated At" },
                values: new object[] { 2, 2, new DateTime(2021, 11, 3, 16, 21, 23, 484, DateTimeKind.Local).AddTicks(2020), 1.0, 1321.0, "Earth", 1, new DateTime(2021, 11, 3, 16, 21, 23, 484, DateTimeKind.Local).AddTicks(2028) });

            migrationBuilder.InsertData(
                table: "Bodies",
                columns: new[] { "Id", "Component Id", "Created At", "Distance To The Sun (AU)", "Earth Mass (AU)", "Name", "Region Id", "Updated At" },
                values: new object[] { 3, 3, new DateTime(2021, 11, 3, 16, 21, 23, 484, DateTimeKind.Local).AddTicks(2030), 5.2000000000000002, 332900.0, "Jupiter", 2, new DateTime(2021, 11, 3, 16, 21, 23, 484, DateTimeKind.Local).AddTicks(2032) });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Bodies_Component Id",
                table: "Bodies",
                column: "Component Id");

            migrationBuilder.CreateIndex(
                name: "IX_Bodies_Region Id",
                table: "Bodies",
                column: "Region Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Bodies");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Components");

            migrationBuilder.DropTable(
                name: "Regions");
        }
    }
}
