using EBISX_POS.API.Helper;
using EBISX_POS.API.Models;
using EBISX_POS.API.Models.Utils;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace EBISX_POS.API.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using (var context = new DataContext(
                serviceProvider.GetRequiredService<DbContextOptions<DataContext>>()))
            {
                // Look for any data already in the database.
                if (context.User.Any() || context.Category.Any() || context.Menu.Any() || context.Timestamp.Any() || context.Order.Any() || context.Item.Any())
                {
                    return;   // DB has been seeded
                }

                var users = new User[]
                {
                    new User { UserEmail = "user1@example.com", UserFName = "John", UserLName = "Doe", UserRole = "Manager" },
                    new User { UserEmail = "user2@example.com", UserFName = "Jane", UserLName = "Doe", UserRole = "Cashier" },
                    new User { UserEmail = "user3@example.com", UserFName = "Alice", UserLName = "Smith", UserRole = "Manager" },
                    new User { UserEmail = "user4@example.com", UserFName = "Bob", UserLName = "Brown", UserRole = "RGM" },
                    new User { UserEmail = "user5@example.com", UserFName = "Charlie", UserLName = "Davis", UserRole = "Cashier" },
                    new User { UserEmail = "user6@example.com", UserFName = "David", UserLName = "Evans", UserRole = "Manager" },
                    new User { UserEmail = "user7@example.com", UserFName = "Eve", UserLName = "Foster", UserRole = "Cashier" },
                    new User { UserEmail = "user8@example.com", UserFName = "Frank", UserLName = "Green", UserRole = "Cashier" },
                    new User { UserEmail = "user9@example.com", UserFName = "Grace", UserLName = "Hill", UserRole = "Cashier" },
                    new User { UserEmail = "user10@example.com", UserFName = "Hank", UserLName = "Ivy", UserRole = "Manager" }
                };
                context.User.AddRange(users);

                var drinkTypes = new DrinkType[] {
                    new DrinkType { DrinkTypeName = "Hot" },
                    new DrinkType { DrinkTypeName = "Cold" },
                };
                context.DrinkType.AddRange(drinkTypes);

                var addOnTypes = new AddOnType[]
                {
                    new AddOnType{ AddOnTypeName = "Sides" },
                    new AddOnType{ AddOnTypeName = "Desserts" },
                };
                context.AddOnType.AddRange(addOnTypes);

                var categories = new Category[]
                {
                    new Category { CtgryName = "Burger" },
                    new Category { CtgryName = "Spaghetti" },
                    new Category { CtgryName = "Chickensad" },
                    new Category { CtgryName = "Sandwich" },
                    new Category { CtgryName = "Drinks" },
                    new Category { CtgryName = "Desserts" },
                    new Category { CtgryName = "Fries" },
                };
                context.Category.AddRange(categories);

                var menus = new Menu[]
                {
                    // Burgers
                    new Menu { MenuName = "Cheeseburger", MenuPrice = 4.99m, Category = categories[0], HasDrink = false, HasAddOn = false },
                    new Menu { MenuName = "Cheeseburger", MenuPrice = 5.99m, Category = categories[0]},
                    new Menu { MenuName = "Burger Ka Sakin", MenuPrice = 4.99m, Category = categories[0], HasDrink = false, HasAddOn = false },
                    new Menu { MenuName = "Burger Ka Sakin", MenuPrice = 5.99m, Category = categories[0]},
                    new Menu { MenuName = "Cheese", MenuPrice = 0.99m, Category = categories[0], IsAddOn = true }, // Addon
                    new Menu { MenuName = "Bacon", MenuPrice = 1.49m, Category = categories[0], IsAddOn = true },  // Addon

                    // Spaghetti
                    new Menu { MenuName = "Spaghetti Bolognese", MenuPrice = 6.99m, Category = categories[1], HasDrink = false, HasAddOn = false },
                    new Menu { MenuName = "Spaghetti Bolognese", MenuPrice = 7.99m, Category = categories[1]},
                    new Menu { MenuName = "Spaghetti", MenuPrice = 5.99m, Category = categories[1], HasDrink = false, HasAddOn = false },
                    new Menu { MenuName = "Spaghetti", MenuPrice = 6.99m, Category = categories[1]},
                    new Menu { MenuName = "Spaghetti w/ Chickensad", MenuPrice = 5.99m, Category = categories[1], HasDrink = false, HasAddOn = false },
                    new Menu { MenuName = "Spaghetti w/ Chickensad", MenuPrice = 6.99m, Category = categories[1]},

                    //  Chickensad
                    new Menu { MenuName = "Chickensad", MenuPrice = 5.99m, Category = categories[2], HasDrink = false, HasAddOn = false },
                    new Menu { MenuName = "Chickensad", MenuPrice = 6.99m, Category = categories[2] },
                    new Menu { MenuName = "Chicken Sandwich", MenuPrice = 5.99m, Category = categories[2], HasDrink = false, HasAddOn = false },
                    new Menu { MenuName = "Chicken Sandwich", MenuPrice = 6.99m, Category = categories[2] },
                    new Menu { MenuName = "Grilled Chicken", MenuPrice = 9.99m, Category = categories[2], HasDrink = false, HasAddOn = false },
                    new Menu { MenuName = "Grilled Chicken", MenuPrice = 10.99m, Category = categories[2] },
                    new Menu { MenuName = "Rice", MenuPrice = 1, Category = categories[2], HasDrink = false, HasAddOn = false },

                    // Sandwich
                    new Menu { MenuName = "Club Sandwich", MenuPrice = 5.99m, Category = categories[3], HasDrink = false, HasAddOn = false },
                    new Menu { MenuName = "Club Sandwich", MenuPrice = 6.99m, Category = categories[3] },

                    // Drinks
                    new Menu { MenuName = "Coke", MenuPrice = 1.99m, Category = categories[4], Size = MenuSize.R.ToString(), DrinkType = drinkTypes[1], HasDrink = false, HasAddOn = false, IsAddOn = true },
                    new Menu { MenuName = "Coke", MenuPrice = 2.99m, Category = categories[4], Size = MenuSize.M.ToString(), DrinkType = drinkTypes[1], HasDrink = false, HasAddOn = false, IsAddOn = true },
                    new Menu { MenuName = "Coke", MenuPrice = 3.99m, Category = categories[4], Size = MenuSize.L.ToString(), DrinkType = drinkTypes[1], HasDrink = false, HasAddOn = false, IsAddOn = true },
                    new Menu { MenuName = "Sprite", MenuPrice = 1.99m, Category = categories[4], Size = MenuSize.R.ToString(), DrinkType = drinkTypes[1], HasDrink = false, HasAddOn = false, IsAddOn = true },
                    new Menu { MenuName = "Sprite", MenuPrice = 2.99m, Category = categories[4], Size = MenuSize.M.ToString(), DrinkType = drinkTypes[1], HasDrink = false, HasAddOn = false, IsAddOn = true },
                    new Menu { MenuName = "Sprite", MenuPrice = 3.99m, Category = categories[4], Size = MenuSize.L.ToString(), DrinkType = drinkTypes[1], HasDrink = false, HasAddOn = false, IsAddOn = true },
                    new Menu { MenuName = "Royal", MenuPrice = 1.99m, Category = categories[4], Size = MenuSize.R.ToString(), DrinkType = drinkTypes[1], HasDrink = false, HasAddOn = false, IsAddOn = true },
                    new Menu { MenuName = "Royal", MenuPrice = 2.99m, Category = categories[4], Size = MenuSize.M.ToString(), DrinkType = drinkTypes[1], HasDrink = false, HasAddOn = false, IsAddOn = true },
                    new Menu { MenuName = "Royal", MenuPrice = 3.99m, Category = categories[4], Size = MenuSize.L.ToString(), DrinkType = drinkTypes[1], HasDrink = false, HasAddOn = false, IsAddOn = true },
                    new Menu { MenuName = "Tea", MenuPrice = 1.99m, Category = categories[4], Size = MenuSize.R.ToString(), DrinkType = drinkTypes[1], HasDrink = false, HasAddOn = false, IsAddOn = true },
                    new Menu { MenuName = "Tea", MenuPrice = 2.99m, Category = categories[4], Size = MenuSize.M.ToString(), DrinkType = drinkTypes[1], HasDrink = false, HasAddOn = false, IsAddOn = true },
                    new Menu { MenuName = "Tea", MenuPrice = 3.99m, Category = categories[4], Size = MenuSize.L.ToString(), DrinkType = drinkTypes[1], HasDrink = false, HasAddOn = false, IsAddOn = true },
                    new Menu { MenuName = "Kape Letse", MenuPrice = 1.99m, Category = categories[4], Size = MenuSize.R.ToString(), DrinkType = drinkTypes[0], HasDrink = false, HasAddOn = false, IsAddOn = true },
                    new Menu { MenuName = "Kape Letse", MenuPrice = 2.99m, Category = categories[4], Size = MenuSize.M.ToString(), DrinkType = drinkTypes[0], HasDrink = false, HasAddOn = false, IsAddOn = true },
                    new Menu { MenuName = "Kape Letse", MenuPrice = 3.99m, Category = categories[4], Size = MenuSize.L.ToString(), DrinkType = drinkTypes[0], HasDrink = false, HasAddOn = false, IsAddOn = true },
                    new Menu { MenuName = "Pinaig na Mais", MenuPrice = 1.99m, Category = categories[4], Size = MenuSize.R.ToString(), DrinkType = drinkTypes[0], HasDrink = false, HasAddOn = false, IsAddOn = true },
                    new Menu { MenuName = "Pinaig na Mais", MenuPrice = 2.99m, Category = categories[4], Size = MenuSize.M.ToString(), DrinkType = drinkTypes[0], HasDrink = false, HasAddOn = false, IsAddOn = true },
                    new Menu { MenuName = "Pinaig na Mais", MenuPrice = 3.99m, Category = categories[4], Size = MenuSize.L.ToString(), DrinkType = drinkTypes[0], HasDrink = false, HasAddOn = false, IsAddOn = true },

                    // Desserts
                    new Menu { MenuName = "Ice Cream", MenuPrice = 2.99m, Category = categories[5], Size = MenuSize.R.ToString(), HasDrink = false, HasAddOn = false, IsAddOn = true, AddOnType = addOnTypes[1]  },
                    new Menu { MenuName = "Ice Cream", MenuPrice = 3.99m, Category = categories[5], Size = MenuSize.M.ToString(), HasDrink = false, HasAddOn = false, IsAddOn = true, AddOnType = addOnTypes[1]  },
                    new Menu { MenuName = "Ice Cream", MenuPrice = 4.99m, Category = categories[5], Size = MenuSize.L.ToString(), HasDrink = false, HasAddOn = false, IsAddOn = true, AddOnType = addOnTypes[1]  },
                    new Menu { MenuName = "Durian Pie", MenuPrice = 2.99m, Category = categories[5], Size = MenuSize.R.ToString(), HasDrink = false, HasAddOn = false, IsAddOn = true, AddOnType = addOnTypes[1]  },
                    new Menu { MenuName = "Durian Pie", MenuPrice = 3.99m, Category = categories[5], Size = MenuSize.M.ToString(), HasDrink = false, HasAddOn = false, IsAddOn = true, AddOnType = addOnTypes[1]  },
                    new Menu { MenuName = "Durian Pie", MenuPrice = 4.99m, Category = categories[5], Size = MenuSize.L.ToString(), HasDrink = false, HasAddOn = false, IsAddOn = true, AddOnType = addOnTypes[1]  },

                    // Fries
                    new Menu { MenuName = "Rice", MenuPrice = 0.49m, Category = categories[6], HasDrink = false, HasAddOn = false, IsAddOn = true, AddOnType = addOnTypes[0] },
                    new Menu { MenuName = "Bisaya Fries", MenuPrice = 2.49m, Category = categories[6], Size = MenuSize.R.ToString(), HasDrink = false, HasAddOn = false, IsAddOn = true, AddOnType = addOnTypes[0]   },
                    new Menu { MenuName = "Bisaya Fries", MenuPrice = 3.49m, Category = categories[6], Size = MenuSize.M.ToString(), HasDrink = false, HasAddOn = false, IsAddOn = true, AddOnType = addOnTypes[0]  },
                    new Menu { MenuName = "Bisaya Fries", MenuPrice = 4.49m, Category = categories[6], Size = MenuSize.L.ToString(), HasDrink = false, HasAddOn = false, IsAddOn = true, AddOnType = addOnTypes[0]  },


                };
                foreach (var menu in menus)
                {
                        try
                        {
                            // Save the image to a local folder (e.g., "wwwroot/images")
                            menu.MenuImagePath = await ImageHelper.DownloadAndSaveImageAsync("https://ebisx.com/assets/img/items/1700209804370249939_701199564837023_606470247253043161_n.png", "C:/Users/Acer/Picture/Images");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error downloading image for {menu.MenuName}: {ex.Message}");
                            // Optionally set a default image path or leave it unchanged.
                        }
                    
                }

                context.Menu.AddRange(menus);

                var couponPromos = new CouponPromo[]
{
                    // Promo type entries: No associated menus, use PromoCode.
                    new CouponPromo
                    {
                        Description = "Summer Sale: 15% off on all orders",
                        PromoCode = "SUMMER15",
                        CouponCode = null,
                        PromoAmount = 15.0m,
                        CouponMenus = null, // No menus for promo type
                        ExpirationTime = DateTimeOffset.UtcNow.AddMonths(1),
                    },
                    new CouponPromo
                    {
                        Description = "Winter Discount: 10% off storewide",
                        PromoCode = "WINTER10",
                        CouponCode = null,
                        PromoAmount = 10.0m,
                        CouponMenus = null,
                        ExpirationTime = DateTimeOffset.UtcNow.AddMonths(2),
                    },
                    new CouponPromo
                    {
                        Description = "Spring Promo: 20% off for new customers",
                        PromoCode = "SPRING20",
                        CouponCode = null,
                        PromoAmount = 20.0m,
                        CouponMenus = null,
                        ExpirationTime = DateTimeOffset.UtcNow.AddDays(45),
                    },
                    new CouponPromo
                    {
                        Description = "Exclusive Online Promo: 25% off on select items",
                        PromoCode = "ONLINE25",
                        CouponCode = null,
                        PromoAmount = 25.0m,
                        CouponMenus = null,
                        ExpirationTime = DateTimeOffset.UtcNow.AddDays(30),
                    },
                    new CouponPromo
                    {
                        Description = "Limited Time Promo: 5% off your purchase",
                        PromoCode = "LIMITED5",
                        CouponCode = null,
                        PromoAmount = 5.0m,
                        CouponMenus = null,
                        ExpirationTime = DateTimeOffset.UtcNow.AddDays(10),
                    },

                    // Coupon type entries: Have CouponCode and associated menu items.
                    new CouponPromo
                    {
                        Description = "Free Cheeseburger Coupon",
                        PromoCode = null,
                        CouponCode = "FREE_CHEESE",
                        PromoAmount = 4.99m,
                        CouponItemQuantity = 6,
                        CouponMenus = new List<Menu> { menus[0] },
                        ExpirationTime = DateTimeOffset.UtcNow.AddDays(20),
                    },
                    new CouponPromo
                    {
                        Description = "Discount Coupon: $1 off on Cheeseburger (second price)",
                        PromoCode = null,
                        CouponCode = "DISC_CHEESE",
                        PromoAmount = 1.00m,
                        CouponItemQuantity = 3,
                        CouponMenus = new List<Menu> { menus[1] },
                        ExpirationTime = DateTimeOffset.UtcNow.AddDays(25),
                    },
                    new CouponPromo
                    {
                        Description = "Combo Coupon: Burger Ka Sakin with free add-on",
                        PromoCode = null,
                        CouponCode = "BKS_COUPON",
                        PromoAmount = 2.50m,
                        CouponItemQuantity = 1,
                        CouponMenus = new List<Menu> { menus[2], menus[3] },
                        ExpirationTime = DateTimeOffset.UtcNow.AddDays(30),
                    },
                    new CouponPromo
                    {
                        Description = "Discount Coupon: $1.49 off on Bacon",
                        PromoCode = null,
                        CouponCode = "BACON_SAVE",
                        CouponItemQuantity = 2,
                        PromoAmount = 1.49m,
                        CouponMenus = new List<Menu> { menus[5], menus[3] },
                        ExpirationTime = DateTimeOffset.UtcNow.AddDays(35),
                    },
                    new CouponPromo
                    {
                        Description = "Special Coupon: Free Club Sandwich",
                        PromoCode = null,
                        CouponCode = "FREE_CLUB",
                        CouponItemQuantity = 2,
                        PromoAmount = 5.99m, // Assuming coupon value equals item price
                        CouponMenus = new List<Menu> { menus[18], menus[19] },
                        ExpirationTime = DateTimeOffset.UtcNow.AddDays(40),
                    }
                };

                context.CouponPromo.AddRange(couponPromos);
                context.SaveChanges();

                context.SaveChanges();
            }
        }
    }
}
