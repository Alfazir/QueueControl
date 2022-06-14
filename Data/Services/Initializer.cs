using Microsoft.AspNetCore.Identity;
using QueueControlServer.Models;
using System.Linq;
using System.Threading.Tasks;

namespace QueueControlServer.Services
{
    public class Initializer
    {
        public static async Task InitializeUsersRoles(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            string adminEmail = "admin@gmail.com";
            string password = "Admin_1234";

            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new Role("admin"));
            }
            if (await roleManager.FindByNameAsync("LocalAdmin") == null)
            {
                await roleManager.CreateAsync(new Role("LocalAdmin"));
            }
            if (await roleManager.FindByNameAsync("Supervisor") == null)
            {
                await roleManager.CreateAsync(new Role("Supervisor"));
            }
            if (await roleManager.FindByNameAsync("Terminal") == null)
            {
                await roleManager.CreateAsync(new Role("Terminal"));
            }
            if (await roleManager.FindByNameAsync("Driver") == null)
            {
                await roleManager.CreateAsync(new Role("Driver"));
            }


            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User { Email = adminEmail, UserName = adminEmail };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }

        }

        public static void InitializeGoodsParameter(ApplicationDBContext context)  // TODO после определения идентификаторов, добавить идентификаторы GUID к параметрам
        {
            if (!context.Brands.Any())
            {
                context.Brands.AddRange(
                    new Brand
                    {
                        BrandName = "ЦЕМ I 52,5Н"
                    },
                    new Brand
                    {
                        BrandName = "ЦЕМ I 42,5Н",
                    },
                    new Brand
                    {
                        BrandName = "ЦЕМ I 42,5Б СС"
                    },
                    new Brand
                    {
                        BrandName = "ЦЕМ II/А-П 42,5Н СС"
                    });
                context.SaveChanges();
            }

            if (!context.Packages.Any())
            {
                context.Packages.AddRange(
                    new Package
                    {
                        PackageName = "Навал"
                    },
                    new Package
                    {
                        PackageName = "Тара 1500 кг МКР"
                    },

                    new Package
                    {
                        PackageName = "Тара 25 кг мешки"
                    },
                    new Package
                    {
                        PackageName = "Тара 25 кг на палетах по 1400 кг"
                    },
                    new Package
                    {
                        PackageName = "Тара 50 кг на палетах по 1500 кг"
                    },
                    new Package
                    {
                        PackageName = "Тара 50 кг мешки"
                    }
                    
                    );
                context.SaveChanges();
            }
        }

        public static void InitializeOrganizations (ApplicationDBContext context)
        {
            if (!context.Carriers.Any())
            {
                context.Carriers.AddRange(
                    new Carrier
                    {
                        OrganizationName = "———"
                        
                    }) ;
                context.SaveChanges();
            }
        }

        #region DemoVer
        public static void InitializeDemo(ApplicationDBContext context, UserManager<User> userManager, RoleManager<Role> roleManager)
        {

            if (!context.Factories.Any())
            {
                context.Factories.AddRange(
                    new Factory
                    {
                        OrganizationName = "DemoDemo",
                        OrganizationId= System.Guid.Parse("ce0b611a-e8a4-4785-9cfe-3c5805f8734a")

                        
                    });
                context.SaveChanges();
            }

         /*   string demoEmail = "demo@gmail.com";
            string password = "demodemo";

            if (await userManager.FindByNameAsync(demoEmail) == null)
            {
                Supervisor demo = new Supervisor { Email = demoEmail, UserName = demoEmail, Organization =  };
                IdentityResult result = await userManager.CreateAsync(demo, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(demo, "Supervisor");
                }
            }*/
         if (!context.Queue.Any())
            {
                context.Queue.AddRange(
                    new Queue
                    {
                        Factory = context.Factories.Find(System.Guid.Parse("ce0b611a-e8a4-4785-9cfe-3c5805f8734a")),
                        Brand = context.Brands.FirstOrDefault(),
                        Package = context.Packages.FirstOrDefault(),
                        QueueName = $"{context.Brands.FirstOrDefault().BrandName} {context.Packages.FirstOrDefault().PackageName}"
                    }

                    ) ;

                context.SaveChanges();

            }
         if (!context.QueueItems.Any())
            {
                context.QueueItems.AddRange(
                new QueueItems
                {
                    Factory = context.Factories.Find(System.Guid.Parse("ce0b611a-e8a4-4785-9cfe-3c5805f8734a")),
                    Brand = context.Brands.FirstOrDefault(),
                    Pacakege = context.Packages.FirstOrDefault(),
                    Queue = context.Queue.FirstOrDefault(),
                    CarNumber = "Р111Х93",
                    Carriers = context.Carriers.FirstOrDefault(),
                    DriverName = "Иванов И.И.",
                    CarrierName = context.Carriers.FirstOrDefault().OrganizationName,
                    FactoryName = context.Factories.FirstOrDefault().OrganizationName,
                    QueueName = context.Queue.FirstOrDefault().QueueName,
                    OderLine= 1

                },
                new QueueItems
                {
                    Factory = context.Factories.Find(System.Guid.Parse("ce0b611a-e8a4-4785-9cfe-3c5805f8734a")),
                    Brand = context.Brands.FirstOrDefault(),
                    Pacakege = context.Packages.FirstOrDefault(),
                    Queue = context.Queue.FirstOrDefault(),
                    CarNumber = "Р222ТХ93",
                    Carriers = context.Carriers.FirstOrDefault(),
                    DriverName = "Сидоров С.С.",
                    CarrierName = context.Carriers.FirstOrDefault().OrganizationName,
                    FactoryName = context.Factories.FirstOrDefault().OrganizationName,
                    QueueName = context.Queue.FirstOrDefault().QueueName,
                    OderLine = 2
                },
                new QueueItems
                {
                    Factory = context.Factories.Find(System.Guid.Parse("ce0b611a-e8a4-4785-9cfe-3c5805f8734a")),
                    Brand = context.Brands.FirstOrDefault(),
                    Pacakege = context.Packages.FirstOrDefault(),
                    Queue = context.Queue.FirstOrDefault(),
                    CarNumber = "Р333ТХ93",
                    Carriers = context.Carriers.FirstOrDefault(),
                    DriverName = "Петров П.П.",
                    CarrierName = context.Carriers.FirstOrDefault().OrganizationName,
                    FactoryName = context.Factories.FirstOrDefault().OrganizationName,
                    QueueName = context.Queue.FirstOrDefault().QueueName,
                    OderLine = 3
                }

                ) ;

               context.SaveChanges();
            }
                

        }
        #endregion
    }
}
