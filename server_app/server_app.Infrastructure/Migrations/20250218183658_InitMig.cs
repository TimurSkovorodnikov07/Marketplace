using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace server_app.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    email_verify = table.Column<bool>(type: "boolean", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "delivery_companies",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    website = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    phone_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_delivery_companies", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "refresh_tokens",
                columns: table => new
                {
                    token_hash = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_refresh_tokens", x => x.token_hash);
                });

            migrationBuilder.CreateTable(
                name: "sellers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    email_verify = table.Column<bool>(type: "boolean", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sellers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "credit_cards",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    number_hash = table.Column<string>(type: "text", nullable: false),
                    many = table.Column<decimal>(type: "numeric", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_cards", x => x.id);
                    table.ForeignKey(
                        name: "FK_customers_credit_cards",
                        column: x => x.owner_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_categories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    tags = table.Column<List<string>>(type: "varchar[]", nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    delivery_company_id = table.Column<Guid>(type: "uuid", nullable: false),
                    seller_id = table.Column<Guid>(type: "uuid", nullable: false),
                    total_estimation = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    estimation_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_categories", x => x.id);
                    table.CheckConstraint("CK_ProductCategories_EstimationCount", "estimation_count >= 0");
                    table.CheckConstraint("CK_ProductCategories_TotalEstimation", "total_estimation >= 0 AND total_estimation <= 10");
                    table.ForeignKey(
                        name: "delivery_company_constraint",
                        column: x => x.delivery_company_id,
                        principalTable: "delivery_companies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "seller_constraint",
                        column: x => x.seller_id,
                        principalTable: "sellers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "purchased_products",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    buyer_id = table.Column<Guid>(type: "uuid", nullable: true),
                    purchased_quantity = table.Column<int>(type: "integer", nullable: false),
                    total_sum = table.Column<decimal>(type: "numeric", nullable: false),
                    purchased_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    must_delivered_before = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    delivered_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_purchased_products", x => x.id);
                    table.ForeignKey(
                        name: "buyer_constraint",
                        column: x => x.buyer_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "purchased_products_constraint",
                        column: x => x.category_id,
                        principalTable: "product_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ratings",
                columns: table => new
                {
                    category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    total_rating = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ratings", x => x.category_id);
                    table.ForeignKey(
                        name: "FK_ratings_product_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "product_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reviews",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    text = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    estimation = table.Column<int>(type: "integer", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reviews", x => x.id);
                    table.ForeignKey(
                        name: "FK_reviews_customers_owner_id",
                        column: x => x.owner_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_reviews_product_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "product_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rating_from_customers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    ratting = table.Column<int>(type: "integer", nullable: false),
                    common_ratting_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rating_from_customers", x => x.id);
                    table.ForeignKey(
                        name: "FK_rating_from_customers_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_rating_from_customers_ratings_common_ratting_id",
                        column: x => x.common_ratting_id,
                        principalTable: "ratings",
                        principalColumn: "category_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "delivery_companies",
                columns: new[] { "id", "description", "name", "phone_number", "website" },
                values: new object[,]
                {
                    { new Guid("ab977dee-7ba0-4c8e-9700-763d702977a0"), "Description 1", "DeliveryCompanyNum 1", "+7 888 032 0324", "https://helloworld.gov/" },
                    { new Guid("ab977dee-7ba0-4c8e-9700-763d702977a1"), "Blahblahblah", "Transporter company", "+6 533 003 0002", "https://transporter.com/" },
                    { new Guid("ab977dee-7ba0-4c8e-9700-763d702977a2"), "Blah blah blah", "Some Dodecahedron", "+7 007 942 2390", "https://dodecahedron.org/" },
                    { new Guid("ab977dee-7ba0-4c8e-9700-763d702977a5"), "Blah123 blah blah...", "Some DC", "+1 117 955 0000", "https://metanit.com/sharp/aspnet6/" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_credit_cards_owner_id",
                table: "credit_cards",
                column: "owner_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_delivery_companies_name",
                table: "delivery_companies",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_delivery_companies_phone_number",
                table: "delivery_companies",
                column: "phone_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_delivery_companies_website",
                table: "delivery_companies",
                column: "website",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_product_categories_delivery_company_id",
                table: "product_categories",
                column: "delivery_company_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_categories_name",
                table: "product_categories",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_product_categories_seller_id",
                table: "product_categories",
                column: "seller_id");

            migrationBuilder.CreateIndex(
                name: "IX_purchased_products_buyer_id",
                table: "purchased_products",
                column: "buyer_id");

            migrationBuilder.CreateIndex(
                name: "IX_purchased_products_category_id",
                table: "purchased_products",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_rating_from_customers_common_ratting_id",
                table: "rating_from_customers",
                column: "common_ratting_id");

            migrationBuilder.CreateIndex(
                name: "IX_rating_from_customers_customer_id",
                table: "rating_from_customers",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_refresh_tokens_user_id",
                table: "refresh_tokens",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_reviews_category_id",
                table: "reviews",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_owner_id",
                table: "reviews",
                column: "owner_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "credit_cards");

            migrationBuilder.DropTable(
                name: "purchased_products");

            migrationBuilder.DropTable(
                name: "rating_from_customers");

            migrationBuilder.DropTable(
                name: "refresh_tokens");

            migrationBuilder.DropTable(
                name: "reviews");

            migrationBuilder.DropTable(
                name: "ratings");

            migrationBuilder.DropTable(
                name: "customers");

            migrationBuilder.DropTable(
                name: "product_categories");

            migrationBuilder.DropTable(
                name: "delivery_companies");

            migrationBuilder.DropTable(
                name: "sellers");
        }
    }
}
