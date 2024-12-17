﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace serverApp.Migrations
{
    [DbContext(typeof(MainDbContext))]
    [Migration("20241128102957_updateProductCategory")]
    partial class updateProductCategory
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Entity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.HasKey("Id");

                    b.ToTable((string)null);

                    b.UseTpcMappingStrategy();
                });

            modelBuilder.Entity("RatingEntity", b =>
                {
                    b.Property<Guid>("ProductCategoryId")
                        .HasColumnType("uuid")
                        .HasColumnName("category_id");

                    b.Property<int>("TotalRating")
                        .HasColumnType("integer")
                        .HasColumnName("total_rating");

                    b.HasKey("ProductCategoryId");

                    b.ToTable("ratings", (string)null);
                });

            modelBuilder.Entity("RefreshTokenEntity", b =>
                {
                    b.Property<string>("TokenHash")
                        .HasColumnType("text")
                        .HasColumnName("token_hash");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("TokenHash");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("refresh_tokens", (string)null);

                    b.UseTpcMappingStrategy();
                });

            modelBuilder.Entity("CreditCardEntity", b =>
                {
                    b.HasBaseType("Entity");

                    b.Property<decimal>("Many")
                        .HasColumnType("numeric")
                        .HasColumnName("many");

                    b.Property<string>("NumberHash")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("number_hash");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid")
                        .HasColumnName("owner_id");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.HasIndex("OwnerId")
                        .IsUnique();

                    b.ToTable("credit_cards", (string)null);
                });

            modelBuilder.Entity("DeliveryCompanyEntity", b =>
                {
                    b.HasBaseType("Entity");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("name");

                    b.Property<string>("WebSite")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("website");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("WebSite")
                        .IsUnique();

                    b.ToTable("delivery_companies", (string)null);
                });

            modelBuilder.Entity("ImageEntity", b =>
                {
                    b.HasBaseType("Entity");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("fileName");

                    b.Property<string>("MimeType")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("mime_type");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("path");

                    b.Property<Guid>("ProductCategoryId")
                        .HasColumnType("uuid")
                        .HasColumnName("category_id");

                    b.HasIndex("ProductCategoryId");

                    b.ToTable("images", (string)null);
                });

            modelBuilder.Entity("ProductCategoryEntity", b =>
                {
                    b.HasBaseType("Entity");

                    b.Property<Guid>("DeliveryCompanyId")
                        .HasColumnType("uuid")
                        .HasColumnName("delivery_company_id");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("description");

                    b.Property<int>("EstimationCount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0)
                        .HasColumnName("estimation_count");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(24)
                        .HasColumnType("character varying(24)")
                        .HasColumnName("name");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid")
                        .HasColumnName("seller_id");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric")
                        .HasColumnName("price");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer")
                        .HasColumnName("quantity");

                    b.Property<int>("TotalEstimation")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0)
                        .HasColumnName("total_estimation");

                    b.HasIndex("DeliveryCompanyId");

                    b.HasIndex("Name");

                    b.HasIndex("OwnerId");

                    b.ToTable("product_categories", null, t =>
                        {
                            t.HasCheckConstraint("CK_ProductCategories_EstimationCount", "estimation_count >= 0");

                            t.HasCheckConstraint("CK_ProductCategories_TotalEstimation", "total_estimation >= 0 AND total_estimation <= 10");
                        });
                });

            modelBuilder.Entity("PurchasedProductEntity", b =>
                {
                    b.HasBaseType("Entity");

                    b.Property<Guid?>("BuyerId")
                        .HasColumnType("uuid")
                        .HasColumnName("buyer_id");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uuid")
                        .HasColumnName("category_id");

                    b.Property<DateTime?>("DeliveredDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("delivered_date");

                    b.Property<DateTime>("MustDeliveredBefore")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("must_delivered_before");

                    b.Property<DateTime>("PurchasedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("purchased_date");

                    b.Property<int>("PurchasedQuantity")
                        .HasColumnType("integer")
                        .HasColumnName("purchased_quantity");

                    b.Property<decimal>("TotalSum")
                        .HasColumnType("numeric")
                        .HasColumnName("total_sum");

                    b.HasIndex("BuyerId");

                    b.HasIndex("CategoryId");

                    b.ToTable("purchased_products", (string)null);
                });

            modelBuilder.Entity("RatingFromCustomerEntity", b =>
                {
                    b.HasBaseType("Entity");

                    b.Property<Guid>("CommonRatingId")
                        .HasColumnType("uuid")
                        .HasColumnName("common_ratting_id");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid")
                        .HasColumnName("customer_id");

                    b.Property<int>("Rating")
                        .HasColumnType("integer")
                        .HasColumnName("ratting");

                    b.HasIndex("CommonRatingId");

                    b.HasIndex("CustomerId");

                    b.ToTable("rating_from_customers", (string)null);
                });

            modelBuilder.Entity("ReviewEntity", b =>
                {
                    b.HasBaseType("Entity");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uuid")
                        .HasColumnName("category_id");

                    b.Property<int>("Estimation")
                        .HasColumnType("integer")
                        .HasColumnName("estimation");

                    b.Property<Guid>("ReviewOwnerId")
                        .HasColumnType("uuid")
                        .HasColumnName("owner_id");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(1500)
                        .HasColumnType("character varying(1500)")
                        .HasColumnName("text");

                    b.HasIndex("CategoryId");

                    b.HasIndex("ReviewOwnerId");

                    b.ToTable("reviews", (string)null);
                });

            modelBuilder.Entity("UserEntity", b =>
                {
                    b.HasBaseType("Entity");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("email");

                    b.Property<bool>("EmailVerify")
                        .HasColumnType("boolean")
                        .HasColumnName("email_verify");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("character varying(25)")
                        .HasColumnName("name");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password_hash");

                    b.ToTable((string)null);
                });

            modelBuilder.Entity("CustomerEntity", b =>
                {
                    b.HasBaseType("UserEntity");

                    b.ToTable("customers", (string)null);
                });

            modelBuilder.Entity("SellerEntity", b =>
                {
                    b.HasBaseType("UserEntity");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("description");

                    b.ToTable("sellers", (string)null);
                });

            modelBuilder.Entity("RatingEntity", b =>
                {
                    b.HasOne("ProductCategoryEntity", "ProductCategory")
                        .WithOne()
                        .HasForeignKey("RatingEntity", "ProductCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProductCategory");
                });

            modelBuilder.Entity("RefreshTokenEntity", b =>
                {
                    b.HasOne("UserEntity", null)
                        .WithOne()
                        .HasForeignKey("RefreshTokenEntity", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("user_constraint");
                });

            modelBuilder.Entity("CreditCardEntity", b =>
                {
                    b.HasOne("CustomerEntity", "Owner")
                        .WithOne("CreditCard")
                        .HasForeignKey("CreditCardEntity", "OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_customers_credit_cards");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("DeliveryCompanyEntity", b =>
                {
                    b.OwnsOne("PhoneNumberValueObject", "PhoneNumber", b1 =>
                        {
                            b1.Property<Guid>("DeliveryCompanyEntityId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Number")
                                .IsRequired()
                                .HasMaxLength(20)
                                .HasColumnType("character varying(20)")
                                .HasColumnName("phone_number");

                            b1.HasKey("DeliveryCompanyEntityId");

                            b1.HasIndex("Number")
                                .IsUnique();

                            b1.ToTable("delivery_companies");

                            b1.WithOwner()
                                .HasForeignKey("DeliveryCompanyEntityId");
                        });

                    b.Navigation("PhoneNumber")
                        .IsRequired();
                });

            modelBuilder.Entity("ImageEntity", b =>
                {
                    b.HasOne("ProductCategoryEntity", "ProductCategory")
                        .WithMany("Images")
                        .HasForeignKey("ProductCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("category_constraint");

                    b.Navigation("ProductCategory");
                });

            modelBuilder.Entity("ProductCategoryEntity", b =>
                {
                    b.HasOne("DeliveryCompanyEntity", "DeliveryCompany")
                        .WithMany()
                        .HasForeignKey("DeliveryCompanyId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired()
                        .HasConstraintName("delivery_company_constraint");

                    b.HasOne("SellerEntity", "Owner")
                        .WithMany("ProductsCategories")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("seller_constraint");

                    b.OwnsOne("TagsValueObject", "Tags", b1 =>
                        {
                            b1.Property<Guid>("ProductCategoryEntityId")
                                .HasColumnType("uuid");

                            b1.Property<List<string>>("Tags")
                                .IsRequired()
                                .HasColumnType("varchar[]")
                                .HasColumnName("tags");

                            b1.HasKey("ProductCategoryEntityId");

                            b1.ToTable("product_categories");

                            b1.WithOwner()
                                .HasForeignKey("ProductCategoryEntityId");
                        });

                    b.Navigation("DeliveryCompany");

                    b.Navigation("Owner");

                    b.Navigation("Tags")
                        .IsRequired();
                });

            modelBuilder.Entity("PurchasedProductEntity", b =>
                {
                    b.HasOne("CustomerEntity", "Buyer")
                        .WithMany("Purchases")
                        .HasForeignKey("BuyerId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("buyer_constraint");

                    b.HasOne("ProductCategoryEntity", "Category")
                        .WithMany("PurchasedProducts")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired()
                        .HasConstraintName("purchased_products_constraint");

                    b.Navigation("Buyer");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("RatingFromCustomerEntity", b =>
                {
                    b.HasOne("RatingEntity", "CommonRating")
                        .WithMany("RattingFromCustomers")
                        .HasForeignKey("CommonRatingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CustomerEntity", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CommonRating");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("ReviewEntity", b =>
                {
                    b.HasOne("ProductCategoryEntity", "Category")
                        .WithMany("Reviews")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CustomerEntity", "ReviewOwner")
                        .WithMany()
                        .HasForeignKey("ReviewOwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("ReviewOwner");
                });

            modelBuilder.Entity("RatingEntity", b =>
                {
                    b.Navigation("RattingFromCustomers");
                });

            modelBuilder.Entity("ProductCategoryEntity", b =>
                {
                    b.Navigation("Images");

                    b.Navigation("PurchasedProducts");

                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("CustomerEntity", b =>
                {
                    b.Navigation("CreditCard")
                        .IsRequired();

                    b.Navigation("Purchases");
                });

            modelBuilder.Entity("SellerEntity", b =>
                {
                    b.Navigation("ProductsCategories");
                });
#pragma warning restore 612, 618
        }
    }
}
