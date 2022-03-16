using shop.Models;
using System.Linq;

namespace shop.Classes
{
    public class ShopFactory
    {

        private static Categories keyboardCategory = new Categories
        {
            Category_name = "Клавиатура",
            Mark_del = 0
        };

        private static Categories mouseCategory = new Categories
        {
            Category_name = "Мышка",
            Mark_del = 0
        };

        private static Categories printerCategory = new Categories
        {
            Category_name = "Принтер",
            Mark_del = 0
        };

        private static Categories systemCategory = new Categories
        {
            Category_name = "Системный блок",
            Mark_del = 0
        };

        private static Categories motherboardCategory = new Categories
        {
            Category_name = "Материнская плата",
            Mark_del = 0
        };

        public static void AddCategories(DBCLassesDataContext context)
        {
            if (!context.Categories.Any())
            {
                context.Categories.InsertOnSubmit(keyboardCategory);
                context.Categories.InsertOnSubmit(mouseCategory);
                context.Categories.InsertOnSubmit(printerCategory);
                context.Categories.InsertOnSubmit(systemCategory);
                context.Categories.InsertOnSubmit(motherboardCategory);
                context.Categories.Context.SubmitChanges();
            }
        }

        private static Users adminUser = new Users
        {
            User_fio = "Парадня Роман",
            User_address = "no address",
            User_pasport = "no pasport",
            User_tel = "no telephone",
            User_log = "admin",
            User_pas = "123456",
            User_email = "no email",
            User_Law_Id = 1,
            Mark_del = 0
        };

        public static void AddUsers(DBCLassesDataContext context)
        {
            if (!context.Users.Any())
            {
                context.Users.InsertOnSubmit(adminUser);
                context.Users.Context.SubmitChanges();
            }
        }

        private static Laws adminLaw = new Laws
        {
            Law_name = "Администратор",
            Mark_del = 0
        };

        private static Laws clientLaw = new Laws
        {
            Law_name = "Клиент",
            Mark_del = 0
        };

        public static void AddLaws(DBCLassesDataContext context)
        {
            if (!context.Laws.Any())
            {
                context.Laws.InsertOnSubmit(adminLaw);
                context.Laws.InsertOnSubmit(clientLaw);
                context.Laws.Context.SubmitChanges();
            }
        }

        private static Countries byCountry = new Countries
        {
            Country_name = "Беларусь",
            Mark_del = 0
        };

        public static void AddCountries(DBCLassesDataContext context)
        {
            if (!context.Countries.Any())
            {
                context.Countries.InsertOnSubmit(byCountry);
                context.Countries.Context.SubmitChanges();
            }
        }


        private static States sendState = new States
        {
            State_name = "На оформлении",
            Mark_del = 0
        };

        public static void AddStates(DBCLassesDataContext context)
        {
            if (!context.States.Any())
            {
                context.States.InsertOnSubmit(sendState);
                context.States.Context.SubmitChanges();
            }
        }

        private static Brands msiBrand = new Brands
        {
            Brand_name = "MSI",
            Mark_del = 0
        };

        private static Brands stcBrand = new Brands
        {
            Brand_name = "STC",
            Mark_del = 0
        };


        public static void AddBrands(DBCLassesDataContext context)
        {
            if (!context.Brands.Any())
            {
                context.Brands.InsertOnSubmit(msiBrand);
                context.Brands.InsertOnSubmit(stcBrand);
                context.Brands.Context.SubmitChanges();
            }
        }


        private static Technics motherboardTechnic1 = new Technics
        {
            Technic_name = "Материнская плата MSI",
            Technic_model = "MS-7800",
            Technic_param = "mATX, сокет AMD FM2, чипсет AMD A75, память 2xDDR3",
            Technic_year = 2018,
            Technic_cost = 74.49,
            Technic_availability = 1,
            Technic_Brand_Id = 1,
            Technic_Country_Id = 1,
            Technic_Category_Id = 5,
            Mark_del = 0 // Материнская плата
        };


        private static Technics systemTechnic1 = new Technics
        {
            Technic_name = "Материнская плата STC",
            Technic_model = "ECOM 4135R",
            Technic_param = "Midi-Tower, Блок-питания: нет, куллера - 160 мм",
            Technic_year = 2017,
            Technic_cost = 43.47,
            Technic_availability = 1,
            Technic_Brand_Id = 2,
            Technic_Country_Id = 1,
            Technic_Category_Id = 5,// Материнская плата
            Mark_del = 0 
        };

        public static void AddTechnics(DBCLassesDataContext context)
        {
            if (!context.Technics.Any())
            {
                context.Technics.InsertOnSubmit(motherboardTechnic1);
                context.Technics.InsertOnSubmit(systemTechnic1);
                context.Technics.Context.SubmitChanges();
            }
        }
    }


}