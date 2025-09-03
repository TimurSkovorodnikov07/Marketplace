using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using server_app.Domain;
using server_app.Domain.Entities.ProductCategories;
using server_app.Domain.Entities.ProductCategories.DeliveryCompanies;
using server_app.Domain.Entities.ProductCategories.Ratings;
using server_app.Domain.Entities.ProductCategories.ValueObjects;
using server_app.Domain.Entities.Users.Customer;
using server_app.Domain.Entities.Users.Seller;
using server_app.Domain.Model.Dtos;
using server_app.Infrastructure;

namespace server_app.Tests.Intergration;
/*
 
   public static class DbContextForTests
   {
       public const string PathToFiles = "/home/path/to/files";
       public static readonly Guid FirstImageId = Guid.NewGuid();
   
       public static async Task<MainDbContext> CreateContext()
       {
           // Create dbcontext
           var options = new DbContextOptionsBuilder<MainDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .Options;
   
           var context = new MainDbContext(options);
           await context.Database.EnsureCreatedAsync();
   
           // Add delivery companies
           var deliveryCompany = DeliveryCompanyEntity.Create("FirstDeliveryCompany", "LLKAJDAS",
               new Uri("https://www.helloworld.com/"), PhoneNumberValueObject.Create("7 702 555 4321"));
           await context.Companies.AddAsync(deliveryCompany);
           await context.SaveChangesAsync();
   
           // Add users
           var firstCustomer = CustomerEntity.Create("customername1", "customeremail1@email.lol", "passwordHash");
           var secondCustomer = CustomerEntity.Create("customername2", "customeremail2@email.lol", "passwordHash");
   
           var firstSeller = SellerEntity.Create("sellername1", "laks dfjlasdj flkas djflk", "selleremail1@email.lol",
               "passwordHash");
           var secondSeller = SellerEntity.Create("sellername2", "...", "selleremail2@email.lol", "passwordHash");
   
           await context.Customers.AddAsync(firstCustomer);
           await context.Customers.AddAsync(secondCustomer);
           await context.Sellers.AddAsync(firstSeller);
           await context.Sellers.AddAsync(secondSeller);
           await context.SaveChangesAsync();
   
           // Create Product Categories
           await CreateCategory($"firstSeller.t", context, deliveryCompany, firstSeller, FirstImageId);
           for (int i = 0; i < 3; i++)
               await CreateCategory($"firstSeller.{i}", context, deliveryCompany, firstSeller, Guid.NewGuid());
           for (int i = 0; i < 3; i++)
               await CreateCategory($"secondSeller.{i}", context, deliveryCompany, secondSeller, Guid.NewGuid());
   
           await context.SaveChangesAsync();
           return context;
       }
   
       private static async Task CreateCategory(string name, MainDbContext context, DeliveryCompanyEntity deliveryCompany,
           SellerEntity owner, Guid imageId)
       {
           var content = "Fake File Content";
           var fileName = $"{name}.png";
           var stream = new MemoryStream();
           var writer = new StreamWriter(stream);
           await writer.WriteAsync(content);
   
           var category = ProductCategoryEntity.Create(new()
           {
               Name = $"Name{name}",
               Description = $"Description{name}",
               Tags = new TagsValueObject { Tags = ["1", "tag", "asdf"] },
               Price = 555,
               Quantity = 100,
               DeliveryCompany = deliveryCompany,
               Owner = owner,
               Images = [new FormFile(stream, 0, stream.Length, Guid.NewGuid().ToString(), fileName)]
           });
   
           await context.ProductsCategories.AddAsync(category);
           await context.SaveChangesAsync();
   
           var newImage = ImageDto.Create(imageId, fileName, category.Id, "image/png");
   
           await context.Images.AddAsync(newImage);
           await context.SaveChangesAsync();
   
           var newCommonRatting = RatingEntity.Create(category.Id);
           await context.Rattings.AddAsync(newCommonRatting);
           await context.SaveChangesAsync();
       }
   }
   
 */