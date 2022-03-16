using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using shop.Models;
using shop.Classes;

namespace shop.Controllers
{
    public class HomeController : Controller
    {
        private DBCLassesDataContext context;

        // Пользователь сайта
        private static Users USER = null;

        // Корзина клиента
        public static List<int> TECHNICS = new List<int>();

        public HomeController()
        {
            context = new DBCLassesDataContext();

            // Добавление начальных данных в таблицы.
            ShopFactory.AddCategories(context);
            ShopFactory.AddCountries(context);
            ShopFactory.AddBrands(context);
            ShopFactory.AddTechnics(context);
            ShopFactory.AddLaws(context);
            ShopFactory.AddUsers(context);
            ShopFactory.AddStates(context);
        }

        public ActionResult Index()
        {
            ViewBag.USER = USER;

            return View();
        }

        public ActionResult Info()
        {
            ViewBag.USER = USER;

            return View();
        }

        public ActionResult PageReports()
        {
            ViewBag.USER = USER;
            ViewBag.Message = "";
            ViewBag.Rep = 0;
            DateTime dat1;
                DateTime.TryParse(Request.Params["dat1"], out dat1);
            DateTime dat2;
            DateTime.TryParse(Request.Params["dat2"], out dat2);
            int year;
            int.TryParse(Request.Params["year"], out year);

            if (Request.Params["report1"] !=null)
            {

                
                var query = from order in context.Orders
                        from record in context.Records
                        from technic in context.Technics
                        where record.Record_Order_Id == order.Id
                        where order.Order_paid == 1
                        where order.Order_date >= dat1
                        where order.Order_date <= dat2

                        group new { technic.Technic_name, record.Record_count, technic.Technic_cost } 
                        by technic.Technic_name into result

                        select new ReportObj1
                        {
                            Key = result.Key.ToString(),
                            Amount = result.Sum(p => p.Record_count),
                            Sum = result.Sum(p => p.Record_count * p.Technic_cost),
                            NDS = result.Sum(p => p.Record_count * p.Technic_cost * 20 / 100),
                            Itogo = result.Sum(p => p.Record_count * p.Technic_cost + p.Record_count * p.Technic_cost * 20 / 100)
                        };

                ViewBag.Rep = 1;
                ViewBag.query = query.ToList();
                ViewBag.dat1 = dat1;
                ViewBag.dat2 = dat2;
            }

            if (Request.Params["report2"] != null)
            {

                var query = from order in context.Orders
                            from record in context.Records
                            from produkt in context.Technics
                            where record.Record_Order_Id == order.Id
                            where order.Order_paid == 1
                            where order.Order_date.Year == year

                            group new { record.Record_count, produkt.Technic_cost }
                            by order.Order_date.Month into result

                            select new ReportObj2
                            {
                                Key = result.Key,
                                Amount = result.Sum(p => p.Record_count),
                                Sum = result.Sum(p => p.Record_count * p.Technic_cost),
                                NDS = result.Sum(p => p.Record_count * p.Technic_cost * 20 / 100),
                                Itogo = result.Sum(p => p.Record_count * p.Technic_cost + p.Record_count * p.Technic_cost * 20 / 100)
                            };


                ViewBag.Rep = 2;
                ViewBag.query = query.ToList();
                ViewBag.year = year;
            }


            if (Request.Params["report3"] != null)
            {
                var query = from order in context.Orders
                            from record in context.Records
                            from produkt in context.Technics
                            from category in context.Categories
                            where record.Record_Order_Id == order.Id
                            where produkt.Technic_Category_Id == category.Id
                            where order.Order_paid == 1
                            where order.Order_date >= dat1
                            where order.Order_date <= dat2

                            group new { category.Category_name, record.Record_count, produkt.Technic_cost }
                            by category.Category_name into result

                            select new ReportObj1
                            {
                                Key = result.Key.ToString(),
                                Amount = result.Sum(p => p.Record_count),
                                Sum = result.Sum(p => p.Record_count * p.Technic_cost),
                                NDS = result.Sum(p => p.Record_count * p.Technic_cost * 20 / 100),
                                Itogo = result.Sum(p => p.Record_count * p.Technic_cost + p.Record_count * p.Technic_cost * 20 / 100)
                            };


                ViewBag.Rep = 3;
                ViewBag.query = query.ToList();
                ViewBag.dat1 = dat1;
                ViewBag.dat2 = dat2;
            }

            if (Request.Params["report4"] != null)
            {

                var query = from order in context.Orders
                            from record in context.Records
                            from produkt in context.Technics
                            from state in context.States

                            where record.Record_Order_Id == order.Id
                            where order.Order_State_Id == state.Id

                            where order.Order_date >= dat1
                            where order.Order_date <= dat2

                            group new { state.State_name, record.Record_count, produkt.Technic_cost }
                            by state.State_name into result

                            select new ReportObj3
                            {
                                Key = result.Key.ToString(),
                                AmountZ = result.Count(),
                                AmountE = result.Sum(p => p.Record_count),
                                Sum = result.Sum(p => p.Record_count * p.Technic_cost),
                                NDS = result.Sum(p => p.Record_count * p.Technic_cost * 20 / 100),
                                Itogo = result.Sum(p => p.Record_count * p.Technic_cost + p.Record_count * p.Technic_cost * 20 / 100)
                            };



                ViewBag.Rep = 4;
                ViewBag.query = query.ToList();
                ViewBag.dat1 = dat1;
                ViewBag.dat2 = dat2;
            }

            if (Request.Params["report5"] != null)
            {

                var query = from order in context.Orders
                            from record in context.Records
                            from produkt in context.Technics
                            from user in context.Users

                            where record.Record_Order_Id == order.Id
                            where order.Order_User_Id == user.Id

                            where order.Order_date >= dat1
                            where order.Order_date <= dat2

                            group new { user.User_fio, user.User_address, record.Record_count, produkt.Technic_cost }
                            by new { user.User_fio, user.User_address } into result

                            select new ReportObj3
                            {
                                Key = result.Key.User_fio.ToString() + ", " + result.Key.User_address.ToString(),
                                AmountZ = result.Count(),
                                AmountE = result.Sum(p => p.Record_count),
                                Sum = result.Sum(p => p.Record_count * p.Technic_cost),
                                NDS = result.Sum(p => p.Record_count * p.Technic_cost * 20 / 100),
                                Itogo = result.Sum(p => p.Record_count * p.Technic_cost + p.Record_count * p.Technic_cost * 20 / 100)
                            };



                ViewBag.Rep = 5;
                ViewBag.query = query.ToList();
                ViewBag.dat1 = dat1;
                ViewBag.dat2 = dat2;
            }

            if (Request.Params["report6"] != null)
            {

                var query = from branch in context.Brands
                            from produkt in context.Technics

                            where branch.Id == produkt.Technic_Brand_Id

                            group new { branch.Brand_name, produkt.Technic_year }
                            by new { branch.Brand_name } into result

                            select new ReportObj4
                            {
                                Key = result.Key.Brand_name.ToString(),
                                Amount = result.Count(),
                                y1 = result.Min(p => p.Technic_year),
                                y2 = result.Max(p => p.Technic_year)
                            };



                ViewBag.Rep = 6;
                ViewBag.query = query.ToList();
                ViewBag.dat1 = dat1;
                ViewBag.dat2 = dat2;
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Автоматизированная система SoftCorp";
            ViewBag.USER = USER;

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Автоматизированная система SoftCorp";
            ViewBag.USER = USER;

            return View();
        }

        public ActionResult Login()
        {

            String logParam = Request.Params["User_log"];
            String pasParam = Request.Params["User_pas"];

            ViewBag.Message = "";

            if (logParam == null || pasParam == null)
            {
                return View();
            }

            if (logParam.Length < 1 || pasParam.Length<1)
            {
                ViewBag.Message = "Логин и(или) пароль не могуть быть пустыми";
                return View();
            }

            var users = context.Users.Where(p => p.User_log.Equals(logParam) && p.User_pas.Equals(pasParam)).ToList();

            if (users.Count != 1)
            {
                ViewBag.Message = "Ошибка логина и(или) пароля";
                return View();
            }

            USER = users.First();

            switch (USER.User_Law_Id)
            {
                case 1: return Redirect("/Home/Admin"); break;
                case 2: return Redirect("/Home/Client"); break;
                default:
                    {
                        ViewBag.Message = "Ошибка логина и(или) пароля";
                        return View();
                    }
            }

            
        }

        public ActionResult Exit()
        {
            USER = null;

            return Redirect("/Home/Index");
        }

        public ActionResult OkReg()
        {
            return View();
        }

        public ActionResult Reg()
        {
            String fioParam = Request.Params["User_fio"];
            String addressParam = Request.Params["User_address"];
            String pasportParam = Request.Params["User_pasport"];
            String telParam = Request.Params["User_tel"];
            String emailParam = Request.Params["User_email"];
            String logParam = Request.Params["User_log"];
            String pasParam = Request.Params["User_pas"];

            // Первая загрузка страницы.
            if (fioParam == null)
            {
                ViewBag.Message = "";
                return View();
            }

            // Далее, если послана форма с данными.
            if (fioParam.Trim().Length < 1)
            {
                ViewBag.Message = "Необходимо указать Ваше полное ФИО";
                return View();
            }

            if (addressParam.Trim().Length<1)
            {
                ViewBag.Message = "Необходимо указать Ваш адрес";
                return View();
            }

            if (pasportParam.Trim().Length < 1)
            {
                ViewBag.Message = "Необходимо указать Ваши паспортные данные";
                return View();
            }

            if (telParam.Trim().Length < 1)
            {
                ViewBag.Message = "Необходимо указать Ваш телефон для связи";
                return View();
            }

            if (logParam.Length < 1)
            {
                ViewBag.Message = "Необходимо указать логин";
                return View();
            }

            if (pasParam.Length < 1)
            {
                ViewBag.Message = "Необходимо указать пароль";
                return View();
            }

            if (logParam.Length < 6)
            {
                ViewBag.Message = "Необходимо указать логин из 6 латинских символов";
                return View();
            }

            if (pasParam.Length < 6)
            {
                ViewBag.Message = "Необходимо указать пароль из 6 латинских символов";
                return View();
            }

            if (context.Users.Where (p => p.User_log.Equals(logParam)).Any())
            {
                ViewBag.Message = "Логин занят";
                return View();
            }

            Users clientUser = new Users
            {
                User_fio = fioParam,
                User_address = addressParam,
                User_pasport = pasportParam,
                User_tel = telParam,
                User_log = logParam,
                User_pas = pasParam,
                User_email = emailParam,
                User_Law_Id = 2,
                Mark_del = 0
            };

            context.Users.InsertOnSubmit(clientUser);
            context.Users.Context.SubmitChanges();

            return Redirect("/Home/OkReg");
        }

        public ActionResult PageProfile ()
        {
            String fioParam = Request.Params["User_fio"];
            String addressParam = Request.Params["User_address"];
            String pasportParam = Request.Params["User_pasport"];
            String telParam = Request.Params["User_tel"];
            String emailParam = Request.Params["User_email"];
            String logParam = Request.Params["User_log"];
            String pasParam = Request.Params["User_pas"];

            ViewBag.USER = USER;

            // Первая загрузка страницы.
            if (fioParam == null)
            {
                ViewBag.USER = USER;
                ViewBag.Message = "";
                return View();
            }

            // Далее, если посланы данные.
            if (fioParam.Trim().Length < 1)
            {
                ViewBag.Message = "Необходимо указать Ваше полное ФИО";
                return View();
            }

            if (addressParam.Trim().Length < 1)
            {
                ViewBag.Message = "Необходимо указать Ваш адрес";
                return View();
            }

            if (pasportParam.Trim().Length < 1)
            {
                ViewBag.Message = "Необходимо указать Ваши паспортные данные";
                return View();
            }

            if (telParam.Trim().Length < 1)
            {
                ViewBag.Message = "Необходимо указать Ваш телефон для связи";
                return View();
            }

            if (logParam.Length < 1)
            {
                ViewBag.Message = "Необходимо указать логин";
                return View();
            }

            if (pasParam.Length < 1)
            {
                ViewBag.Message = "Необходимо указать пароль";
                return View();
            }

            if (logParam.Length < 6)
            {
                ViewBag.Message = "Необходимо указать логин из 6 латинских символов";
                return View();
            }

            if (pasParam.Length < 6)
            {
                ViewBag.Message = "Необходимо указать пароль из 6 латинских символов";
                return View();
            }

            if (context.Users.Where(p => p.User_log.Equals(logParam) && p.Id != USER.Id).Any())
            {
                ViewBag.Message = "Логин занят";
                return View();
            }

            USER.User_fio = fioParam;
            USER.User_address = addressParam;
            USER.User_pasport = pasportParam;
            USER.User_tel = telParam;
            USER.User_log = logParam;
            USER.User_pas = pasParam;
            USER.User_email = emailParam;            

            context.Users.Context.SubmitChanges();

            return Index(); 
        }

        public ActionResult Admin()
        {
            ViewBag.Message = "Admin";
            ViewBag.USER = USER;

            return View();
        }

        public ActionResult Client()
        {
            ViewBag.Message = "Client";
            ViewBag.USER = USER;

            return View();
        }

        public ActionResult PageAll()
        {
            ViewBag.Message = "";
            ViewBag.USER = USER;

            return View();
        }


        // update = true, если обновление информации.
        // update = false, если добавление новой информации.
        public bool GetCatalogFromParam(bool update, out Technics technic)
        {
            bool Ok = true;

            // Для режима обновления информации.
            if (update)
            {
                int Id = int.Parse(Request.Params["Id"]);
                technic = context.Technics.Where(p => p.Id == Id).First();
            }
            // Если добавление новой записи.
            else
            {
                technic = new Technics();
            }

            // Проверка поля.
            if ((technic.Technic_name = Request.Params["Technic_name"]).Trim().Length<1)
            {
                Ok = false;
                if (ViewBag.Message.Trim().Length > 0) ViewBag.Message += ", ";
                ViewBag.Message += "Название компьютерной техники";
            }

            // Проверка поля.
            if ((technic.Technic_model = Request.Params["Technic_model"]).Trim().Length < 1)
            {
                Ok = false;
                if (ViewBag.Message.Trim().Length > 0) ViewBag.Message += ", ";
                ViewBag.Message += "Модель компьютерной техники";
            }

            technic.Technic_param = Request.Params["Technic_param"];




            // Проверка поля.
            try
            {
                technic.Technic_cost = double.Parse(Request.Params["Technic_cost"]);
            }
            catch
            {
                Ok = false;
                if (ViewBag.Message.Trim().Length > 0) ViewBag.Message += ", ";
                ViewBag.Message += "Стоимость единицы";
                technic.Technic_cost = 0;
            }

            // Проверка поля.
            if (int.Parse(Request.Params["Technic_year"]) < 1)
            {
                Ok = false;
                if (ViewBag.Message.Trim().Length > 0) ViewBag.Message += ", ";
                ViewBag.Message += "Год выпуска";
                technic.Technic_year = 2020;
            }
            else
            {
                technic.Technic_year = int.Parse(Request.Params["Technic_year"]);
            }


            // Проверка поля.
            if (int.Parse(Request.Params["Brand_Id"])<1)
            {
                Ok = false;
                if (ViewBag.Message.Trim().Length > 0) ViewBag.Message += ", ";
                ViewBag.Message += "ОС";
                technic.Technic_Brand_Id = 0;
            }
            else
            {
                technic.Technic_Brand_Id = int.Parse(Request.Params["Brand_Id"]);
            }


            // Проверка поля.
            if (int.Parse(Request.Params["Category_Id"]) < 1)
            {
                Ok = false;
                if (ViewBag.Message.Trim().Length > 0) ViewBag.Message += ", ";
                ViewBag.Message += "Категория";
                technic.Technic_Category_Id = 0;
            }
            else
            {
                technic.Technic_Category_Id = int.Parse(Request.Params["Category_Id"]);
            }

            // Для режима обновления информации.
            if (update)
            {                
                    technic.Technic_availability = int.Parse(Request.Params["Technic_availability"]);
            }
            else
            {
                technic.Technic_availability = 1;
                technic.Mark_del = 0;
            }

            if (!Ok)
            {
                ViewBag.Message = "Ошибка ввода полей: " + ViewBag.Message;
            }

            return Ok;
        }


        // add - переход на формы добавления
        // edit - переход форму изменения

        // insert - команда добавить
        // update - команда изменить
        // delete - команда удалить
        public ActionResult EditCatalog()
        {
                        
            ViewBag.USER = USER;
            ViewBag.Message = "";
            ViewBag.Caption = "";

            // Списки для формы.
            ViewBag.Categories = context.Categories.Where(p => p.Mark_del == 0).ToList();
            ViewBag.Brands = context.Brands.Where(p => p.Mark_del == 0).ToList();
            ViewBag.Countries = context.Countries.Where(p => p.Mark_del == 0).ToList();




            // Команда удаления.
            if (Request.Params["delete"] != null)
            {
                int Id = int.Parse(Request.Params["Id"]);
                Technics technic = context.Technics.Where(p => p.Id == Id).First();
                context.Technics.DeleteOnSubmit(technic);
                context.Technics.Context.SubmitChanges();
                return Redirect("/Home/Catalog");
            }




            // Команда удаления.
            if (Request.Params["cancel"] != null)
            {
                ViewBag.Category_Id = Request.Params["Category_Id"];
                return Redirect("/Home/Catalog");
            }



            // Команда изменения.
            if (Request.Params["update"] != null)
            {
                Technics technic;
                if (GetCatalogFromParam(true, out technic))
                {
                    context.Technics.Context.SubmitChanges();
                    return Redirect("/Home/Catalog");
                }
                // Ошибка ввода данных в поля формы.
                else
                {
                    ViewBag.Caption = "Изменить";
                    ViewBag.Mode = "update";
                    ViewBag.technic = technic;
                    return View();
                }
            }




            // Команда добавления.
            if (Request.Params["insert"] != null)
            {
                Technics technic;
                if (GetCatalogFromParam(false, out technic))
                {
                    context.Technics.InsertOnSubmit(technic);
                    context.Technics.Context.SubmitChanges();

                    return Redirect("/Home/Catalog");
                }
                // Ошибка ввода данных в поля формы.
                else
                {
                    // Режим формы - add
                    ViewBag.Caption = "Добавить";
                    ViewBag.Mode = "insert";
                    ViewBag.Technic = technic;
                    // Добавление в указанную категорию.
                    ViewBag.Category_Id = Request.Params["Category_Id"];
                    return View();
                }
            }




            // Режим формы - edit
            if (Request.Params["edit"] != null)
            {
                ViewBag.Caption = "Изменить";
                ViewBag.Mode = "update";
                int Id = int.Parse(Request.Params["Id"]);
                ViewBag.technic = context.Technics.Where(p => p.Id == Id).First();
                return View();
            }




            // Режим формы - add
            ViewBag.Caption = "Добавить";
            ViewBag.Mode = "insert";
            ViewBag.Technic = new Technics
            {
                Technic_name = "",
                Technic_model = "",
                Technic_param = "",
                Technic_year = 0,
                Technic_cost = 0.0,
                Technic_Brand_Id = 0,
                Technic_Country_Id = 0,
                Technic_Category_Id = int.Parse(Request.Params["Category_Id"]),
                Technic_availability = 0,
                Mark_del = 0
            };
            // Добавление в указанную категорию.
            ViewBag.Category_Id = Request.Params["Category_Id"];            
            return View();            
        }




        // select - отбор записей
        public ActionResult Catalog()
        {

            ViewBag.USER = USER;
            ViewBag.Message = "Каталог решений";
            ViewBag.Categories = context.Categories.Where(p => p.Mark_del == 0).ToList();

            IQueryable<Technics> technics = context.Technics.Where(p => p.Mark_del == 0);

            int Category_Id = 0;
            int Technic_year = 0;
            String nameParam = "";
            String modelParam = "";
            String paramParam = "";


         

            return View();
        }

      

        // ==================== Law =================================
        // ==========================================================

        // update = true, если обновление информации.
        // update = false, если добавление новой информации.
        public bool GetLawFromParam(bool update, out Laws law)
        {
            bool Ok = true;

            // Для режима обновления информации.
            if (update)
            {
                int Id = int.Parse(Request.Params["Id"]);
                law = context.Laws.Where(p => p.Id == Id).First();
            }
            // Если добавление новой записи.
            else
            {
                law = new Laws();
            }

            // Проверка поля.
            if ((law.Law_name = Request.Params["Law_name"]).Trim().Length < 1)
            {
                Ok = false;
                if (ViewBag.Message.Trim().Length > 0) ViewBag.Message += ", ";
                ViewBag.Message += "Наименование права (должности)";
            }


            if (!Ok)
            {
                ViewBag.Message = "Ошибка ввода полей: " + ViewBag.Message;
            }

            return Ok;
        }


        // add - переход на формы добавления
        // edit - переход форму изменения

        // insert - команда добавить
        // update - команда изменить
        // delete - команда удалить
        public ActionResult EditLaw()
        {

            ViewBag.USER = USER;
            ViewBag.Message = "";
            ViewBag.Caption = "";


            // Команда удаления.
            if (Request.Params["delete"] != null)
            {
                int Id = int.Parse(Request.Params["Id"]);
                Laws law = context.Laws.Where(p => p.Id == Id).First();
                context.Laws.DeleteOnSubmit(law);
                context.Laws.Context.SubmitChanges();
                return Redirect("/Home/PageLaws");
            }


            // Команда удаления.
            if (Request.Params["cancel"] != null)
            {
                ViewBag.Law_Id = Request.Params["Law_Id"];
                return Redirect("/Home/PageLaws");
            }

            // Команда изменения.
            if (Request.Params["update"] != null)
            {
                Laws law;
                if (GetLawFromParam(true, out law))
                {
                    context.Laws.Context.SubmitChanges();
                    return Redirect("/Home/PageLaws");
                }
                // Ошибка ввода данных в поля формы.
                else
                {
                    ViewBag.Caption = "Изменить";
                    ViewBag.Mode = "update";
                    ViewBag.law = law;
                    return View();
                }
            }




            // Команда добавления.
            if (Request.Params["insert"] != null)
            {
                Laws law;
                if (GetLawFromParam(false, out law))
                {
                    context.Laws.InsertOnSubmit(law);
                    context.Laws.Context.SubmitChanges();

                    return Redirect("/Home/PageLaws");
                }
                // Ошибка ввода данных в поля формы.
                else
                {
                    // Режим формы - add
                    ViewBag.Caption = "Добавить";
                    ViewBag.Mode = "insert";
                    ViewBag.law = law;
                    return View();
                }
            }




            // Режим формы - edit
            if (Request.Params["edit"] != null)
            {
                ViewBag.Caption = "Изменить";
                ViewBag.Mode = "update";
                int Id = int.Parse(Request.Params["Id"]);
                ViewBag.law = context.Laws.Where(p => p.Id == Id).First();
                return View();
            }




            // Режим формы - add
            ViewBag.Caption = "Добавить";
            ViewBag.Mode = "insert";
            ViewBag.Law = new Laws
            {
                Law_name = "",
                Mark_del = 0
            };
            return View();
        }




        // select - отбор записей
        public ActionResult PageLaws()
        {

            ViewBag.USER = USER;
            ViewBag.Message = "Каталог прав (должностей) пользователей";

            IQueryable<Laws> laws = context.Laws.Where(p => p.Mark_del == 0);

            String Law_name = "";

            // Нажата кнопка Отобрать

            if (Request.Params["select"] != null)
            {

                // Название
                Law_name = Request.Params["Law_name"];

                if (Law_name != null && Law_name.Trim().Length > 1)
                {
                    laws = laws.Where(p => p.Law_name.Contains(Law_name.Trim()));
                }
            }


            ViewBag.Law_name = Law_name;
            ViewBag.Laws = laws;

            return View();
        }

        // ==================== Brand =================================
        // ==========================================================

        // update = true, если обновление информации.
        // update = false, если добавление новой информации.
        public bool GetBrandFromParam(bool update, out Brands brand)
        {
            bool Ok = true;

            // Для режима обновления информации.
            if (update)
            {
                int Id = int.Parse(Request.Params["Id"]);
                brand = context.Brands.Where(p => p.Id == Id).First();
            }
            // Если добавление новой записи.
            else
            {
                brand = new Brands();
            }

            // Проверка поля.
            if ((brand.Brand_name = Request.Params["Brand_name"]).Trim().Length < 1)
            {
                Ok = false;
                if (ViewBag.Message.Trim().Length > 0) ViewBag.Message += ", ";
                ViewBag.Message += "Наименование ОС";
            }


            if (!Ok)
            {
                ViewBag.Message = "Ошибка ввода полей: " + ViewBag.Message;
            }

            return Ok;
        }


        // add - переход на формы добавления
        // edit - переход форму изменения

        // insert - команда добавить
        // update - команда изменить
        // delete - команда удалить
        public ActionResult EditBrand()
        {

            ViewBag.USER = USER;
            ViewBag.Message = "";
            ViewBag.Caption = "";


            // Команда удаления.
            if (Request.Params["delete"] != null)
            {
                int Id = int.Parse(Request.Params["Id"]);
                Brands brand = context.Brands.Where(p => p.Id == Id).First();
                context.Brands.DeleteOnSubmit(brand);
                context.Brands.Context.SubmitChanges();
                return Redirect("/Home/PageBrands");
            }


            // Команда удаления.
            if (Request.Params["cancel"] != null)
            {
                ViewBag.Brand_Id = Request.Params["Brand_Id"];
                return Redirect("/Home/PageBrands");
            }

            // Команда изменения.
            if (Request.Params["update"] != null)
            {
                Brands brand;
                if (GetBrandFromParam(true, out brand))
                {
                    context.Brands.Context.SubmitChanges();
                    return Redirect("/Home/PageBrands");
                }
                // Ошибка ввода данных в поля формы.
                else
                {
                    ViewBag.Caption = "Изменить";
                    ViewBag.Mode = "update";
                    ViewBag.brand = brand;
                    return View();
                }
            }




            // Команда добавления.
            if (Request.Params["insert"] != null)
            {
                Brands brand;
                if (GetBrandFromParam(false, out brand))
                {
                    context.Brands.InsertOnSubmit(brand);
                    context.Brands.Context.SubmitChanges();

                    return Redirect("/Home/PageBrands");
                }
                // Ошибка ввода данных в поля формы.
                else
                {
                    // Режим формы - add
                    ViewBag.Caption = "Добавить";
                    ViewBag.Mode = "insert";
                    ViewBag.brand = brand;
                    return View();
                }
            }




            // Режим формы - edit
            if (Request.Params["edit"] != null)
            {
                ViewBag.Caption = "Изменить";
                ViewBag.Mode = "update";
                int Id = int.Parse(Request.Params["Id"]);
                ViewBag.brand = context.Brands.Where(p => p.Id == Id).First();
                return View();
            }




            // Режим формы - add
            ViewBag.Caption = "Добавить";
            ViewBag.Mode = "insert";
            ViewBag.Brand = new Brands
            {
                Brand_name = "",
                Mark_del = 0
            };
            return View();
        }




        // select - отбор записей
        public ActionResult PageBrands()
        {

            ViewBag.USER = USER;
            ViewBag.Message = "Каталог ОС";

            IQueryable<Brands> brands = context.Brands.Where(p => p.Mark_del == 0);

            String Brand_name = "";

            // Нажата кнопка Отобрать

            if (Request.Params["select"] != null)
            {

                // Название
                Brand_name = Request.Params["Brand_name"];

                if (Brand_name != null && Brand_name.Trim().Length > 1)
                {
                    brands = brands.Where(p => p.Brand_name.Contains(Brand_name.Trim()));
                }
            }


            ViewBag.Brand_name = Brand_name;
            ViewBag.Brands = brands;

            return View();
        }

        // ==================== Ways =================================
        // ==========================================================

        // update = true, если обновление информации.
        // update = false, если добавление новой информации.
        public bool GetWayFromParam(bool update, out Ways way)
        {
            bool Ok = true;

            // Для режима обновления информации.
            if (update)
            {
                int Id = int.Parse(Request.Params["Id"]);
                way = context.Ways.Where(p => p.Id == Id).First();
            }
            // Если добавление новой записи.
            else
            {
                way = new Ways();
            }

            // Проверка поля.
            if ((way.Ways_name = Request.Params["Way_name"]).Trim().Length < 1)
            {
                Ok = false;
                if (ViewBag.Message.Trim().Length > 0) ViewBag.Message += ", ";
                ViewBag.Message += "Наименование способа оплаты";
            }


            if (!Ok)
            {
                ViewBag.Message = "Ошибка ввода полей: " + ViewBag.Message;
            }

            return Ok;
        }


        // add - переход на формы добавления
        // edit - переход форму изменения

        // insert - команда добавить
        // update - команда изменить
        // delete - команда удалить
        public ActionResult EditWay()
        {

            ViewBag.USER = USER;
            ViewBag.Message = "";
            ViewBag.Caption = "";


            // Команда удаления.
            if (Request.Params["delete"] != null)
            {
                int Id = int.Parse(Request.Params["Id"]);
                Ways way = context.Ways.Where(p => p.Id == Id).First();
                context.Ways.DeleteOnSubmit(way);
                context.Ways.Context.SubmitChanges();
                return Redirect("/Home/PageWays");
            }


            // Команда удаления.
            if (Request.Params["cancel"] != null)
            {
                ViewBag.Way_Id = Request.Params["Way_Id"];
                return Redirect("/Home/PageWays");
            }

            // Команда изменения.
            if (Request.Params["update"] != null)
            {
                Ways way;
                if (GetWayFromParam(true, out way))
                {
                    context.Ways.Context.SubmitChanges();
                    return Redirect("/Home/PageWays");
                }
                // Ошибка ввода данных в поля формы.
                else
                {
                    ViewBag.Caption = "Изменить";
                    ViewBag.Mode = "update";
                    ViewBag.way = way;
                    return View();
                }
            }




            // Команда добавления.
            if (Request.Params["insert"] != null)
            {
                Ways way;
                if (GetWayFromParam(false, out way))
                {
                    context.Ways.InsertOnSubmit(way);
                    context.Ways.Context.SubmitChanges();

                    return Redirect("/Home/PageWays");
                }
                // Ошибка ввода данных в поля формы.
                else
                {
                    // Режим формы - add
                    ViewBag.Caption = "Добавить";
                    ViewBag.Mode = "insert";
                    ViewBag.way = way;
                    return View();
                }
            }




            // Режим формы - edit
            if (Request.Params["edit"] != null)
            {
                ViewBag.Caption = "Изменить";
                ViewBag.Mode = "update";
                int Id = int.Parse(Request.Params["Id"]);
                ViewBag.way = context.Ways.Where(p => p.Id == Id).First();
                return View();
            }




            // Режим формы - add
            ViewBag.Caption = "Добавить";
            ViewBag.Mode = "insert";
            ViewBag.Way = new Ways
            {
                Ways_name = "",
                Mark_del = 0
            };
            return View();
        }




        // select - отбор записей
        public ActionResult PageWays()
        {

            ViewBag.USER = USER;
            ViewBag.Message = "Каталог способов оплаты";

            IQueryable<Ways> ways = context.Ways.Where(p => p.Mark_del == 0);

            String Way_name = "";

            // Нажата кнопка Отобрать

            if (Request.Params["select"] != null)
            {

                // Название
                Way_name = Request.Params["Way_name"];

                if (Way_name != null && Way_name.Trim().Length > 1)
                {
                    ways = ways.Where(p => p.Ways_name.Contains(Way_name.Trim()));
                }
            }


            ViewBag.Way_name = Way_name;
            ViewBag.Ways = ways;

            return View();
        }

        // ==================== Method =================================
        // ==========================================================



        // update = true, если обновление информации.
        // update = false, если добавление новой информации.
        public bool GetMethodFromParam(bool update, out Methods method)
        {
            bool Ok = true;

            // Для режима обновления информации.
            if (update)
            {
                int Id = int.Parse(Request.Params["Id"]);
                method = context.Methods.Where(p => p.Id == Id).First();
            }
            // Если добавление новой записи.
            else
            {
                method = new Methods();
            }

            // Проверка поля.
            if ((method.Method_name = Request.Params["Method_name"]).Trim().Length < 1)
            {
                Ok = false;
                if (ViewBag.Message.Trim().Length > 0) ViewBag.Message += ", ";
                ViewBag.Message += "Наименование типа доработки";
            }


            if (!Ok)
            {
                ViewBag.Message = "Ошибка ввода полей: " + ViewBag.Message;
            }

            return Ok;
        }


        // add - переход на формы добавления
        // edit - переход форму изменения

        // insert - команда добавить
        // update - команда изменить
        // delete - команда удалить
        public ActionResult EditMethod()
        {

            ViewBag.USER = USER;
            ViewBag.Message = "";
            ViewBag.Caption = "";


            // Команда удаления.
            if (Request.Params["delete"] != null)
            {
                int Id = int.Parse(Request.Params["Id"]);
                Methods method = context.Methods.Where(p => p.Id == Id).First();
                context.Methods.DeleteOnSubmit(method);
                context.Methods.Context.SubmitChanges();
                return Redirect("/Home/PageMethods");
            }


            // Команда удаления.
            if (Request.Params["cancel"] != null)
            {
                ViewBag.Method_Id = Request.Params["Method_Id"];
                return Redirect("/Home/PageMethods");
            }

            // Команда изменения.
            if (Request.Params["update"] != null)
            {
                Methods method;
                if (GetMethodFromParam(true, out method))
                {
                    context.Methods.Context.SubmitChanges();
                    return Redirect("/Home/PageMethods");
                }
                // Ошибка ввода данных в поля формы.
                else
                {
                    ViewBag.Caption = "Изменить";
                    ViewBag.Mode = "update";
                    ViewBag.method = method;
                    return View();
                }
            }




            // Команда добавления.
            if (Request.Params["insert"] != null)
            {
                Methods method;
                if (GetMethodFromParam(false, out method))
                {
                    context.Methods.InsertOnSubmit(method);
                    context.Methods.Context.SubmitChanges();

                    return Redirect("/Home/PageMethods");
                }
                // Ошибка ввода данных в поля формы.
                else
                {
                    // Режим формы - add
                    ViewBag.Caption = "Добавить";
                    ViewBag.Mode = "insert";
                    ViewBag.method = method;
                    return View();
                }
            }




            // Режим формы - edit
            if (Request.Params["edit"] != null)
            {
                ViewBag.Caption = "Изменить";
                ViewBag.Mode = "update";
                int Id = int.Parse(Request.Params["Id"]);
                ViewBag.method = context.Methods.Where(p => p.Id == Id).First();
                return View();
            }




            // Режим формы - add
            ViewBag.Caption = "Добавить";
            ViewBag.Mode = "insert";
            ViewBag.Method = new Methods
            {
                Method_name = "",
                Mark_del = 0
            };
            return View();
        }




        // select - отбор записей
        public ActionResult PageMethods()
        {

            ViewBag.USER = USER;
            ViewBag.Message = "Каталог типов доработок";

            IQueryable<Methods> methods = context.Methods.Where(p => p.Mark_del == 0);

            String Method_name = "";

            // Нажата кнопка Отобрать

            if (Request.Params["select"] != null)
            {

                // Название
                Method_name = Request.Params["Method_name"];

                if (Method_name != null && Method_name.Trim().Length > 1)
                {
                    methods = methods.Where(p => p.Method_name.Contains(Method_name.Trim()));
                }
            }


            ViewBag.Method_name = Method_name;
            ViewBag.Methods = methods;

            return View();
        }


        // ======================== User ==================
        // =========================================================


        // update = true, если обновление информации.
        // update = false, если добавление новой информации.
        public bool GetUserFromParam(bool update, out Users user)
        {
            bool Ok = true;

            // Для режима обновления информации.
            if (update)
            {
                int Id = int.Parse(Request.Params["Id"]);
                user = context.Users.Where(p => p.Id == Id).First();
            }
            // Если добавление новой записи.
            else
            {
                user = new Users();
            }

            // Проверка поля.
            if ((user.User_fio = Request.Params["User_fio"]).Trim().Length < 1)
            {
                Ok = false;
                if (ViewBag.Message.Trim().Length > 0) ViewBag.Message += ", ";
                ViewBag.Message += "ФИО пользователя";
            }

            // Проверка поля.
            if ((user.User_log = Request.Params["User_log"]).Trim().Length < 1)
            {
                Ok = false;
                if (ViewBag.Message.Trim().Length > 0) ViewBag.Message += ", ";
                ViewBag.Message += "Логин";
            }

            // Проверка поля.
            if ((user.User_pas = Request.Params["User_pas"]).Trim().Length < 1)
            {
                Ok = false;
                if (ViewBag.Message.Trim().Length > 0) ViewBag.Message += ", ";
                ViewBag.Message += "Пароль";
            }

            // Проверка поля.
            if ((user.User_address = Request.Params["User_address"]).Trim().Length < 1)
            {
                Ok = false;
                if (ViewBag.Message.Trim().Length > 0) ViewBag.Message += ", ";
                ViewBag.Message += "Адрес";
            }


            // Проверка поля.
            if ((user.User_tel = Request.Params["User_tel"]).Trim().Length < 1)
            {
                Ok = false;
                if (ViewBag.Message.Trim().Length > 0) ViewBag.Message += ", ";
                ViewBag.Message += "Телефон";
            }

            // Проверка поля.
            if ((user.User_email = Request.Params["User_email"]).Trim().Length < 1)
            {
                Ok = false;
                if (ViewBag.Message.Trim().Length > 0) ViewBag.Message += ", ";
                ViewBag.Message += "E-mail";
            }

            // Проверка поля.
            if ((user.User_pasport = Request.Params["User_pasport"]).Trim().Length < 1)
            {
                Ok = false;
                if (ViewBag.Message.Trim().Length > 0) ViewBag.Message += ", ";
                ViewBag.Message += "Паспорт";
            }


            // Проверка поля.
            if (int.Parse(Request.Params["Law_Id"]) < 1)
            {
                Ok = false;
                if (ViewBag.Message.Trim().Length > 0) ViewBag.Message += ", ";
                ViewBag.Message += "Право";
                user.User_Law_Id = 0;
            }
            else
            {
                user.User_Law_Id = int.Parse(Request.Params["Law_Id"]);
            }


            // Для режима обновления информации.
            if (update)
            {
                
            }
            else
            {
                user.Mark_del = 0;
            }

            if (!Ok)
            {
                ViewBag.Message = "Ошибка ввода полей: " + ViewBag.Message;
            }

            return Ok;
        }


        // add - переход на формы добавления
        // edit - переход форму изменения

        // insert - команда добавить
        // update - команда изменить
        // delete - команда удалить
        public ActionResult EditUser()
        {

            ViewBag.USER = USER;
            ViewBag.Message = "";
            ViewBag.Caption = "";

            // Списки для формы.
            ViewBag.Laws = context.Laws.Where(p => p.Mark_del == 0).ToList();




            // Команда удаления.
            if (Request.Params["delete"] != null)
            {
                int Id = int.Parse(Request.Params["Id"]);
                Users user = context.Users.Where(p => p.Id == Id).First();
                context.Users.DeleteOnSubmit(user);
                context.Users.Context.SubmitChanges();
                return Redirect("/Home/PageUsers");
            }




            // Команда удаления.
            if (Request.Params["cancel"] != null)
            {
                ViewBag.Law_Id = Request.Params["Law_Id"];
                return Redirect("/Home/PageUsers");
            }



            // Команда изменения.
            if (Request.Params["update"] != null)
            {
                Users user;
                if (GetUserFromParam(true, out user))
                {
                    context.Users.Context.SubmitChanges();
                    return Redirect("/Home/PageUsers");
                }
                // Ошибка ввода данных в поля формы.
                else
                {
                    ViewBag.Caption = "Изменить";
                    ViewBag.Mode = "update";
                    ViewBag.User0 = user;
                    return View();
                }
            }




            // Команда добавления.
            if (Request.Params["insert"] != null)
            {
                Users user;
                if (GetUserFromParam(false, out user))
                {
                    context.Users.InsertOnSubmit(user);
                    context.Users.Context.SubmitChanges();

                    return Redirect("/Home/PageUsers");
                }
                // Ошибка ввода данных в поля формы.
                else
                {
                    // Режим формы - add
                    ViewBag.Caption = "Добавить";
                    ViewBag.Mode = "insert";
                    ViewBag.User0 = user;
                    // Добавление в указанную категорию.
                    ViewBag.Law_Id = Request.Params["Law_Id"];
                    return View();
                }
            }




            // Режим формы - edit
            if (Request.Params["edit"] != null)
            {
                ViewBag.Caption = "Изменить";
                ViewBag.Mode = "update";
                int Id = int.Parse(Request.Params["Id"]);
                ViewBag.User0 = context.Users.Where(p => p.Id == Id).First();
                return View();
            }




            // Режим формы - add
            ViewBag.Caption = "Добавить";
            ViewBag.Mode = "insert";
            ViewBag.User0 = new Users
            {
                User_fio = "",
                User_address = "г. Гомель",
                User_pasport = "НВ",
                User_email = "@mail.ru",
                User_tel = "+375 (29)",
                User_log = "login ...",
                User_pas = "password ...",
                User_Law_Id = int.Parse(Request.Params["Law_Id"]),
                Mark_del = 0
            };
            // Добавление в указанную категорию.
            ViewBag.Law_Id = Request.Params["Law_Id"];
            return View();
        }




        // select - отбор записей
        public ActionResult PageUsers()
        {

            ViewBag.USER = USER;
            ViewBag.Message = "Справочник пользователей";
            ViewBag.Laws = context.Laws.Where(p => p.Mark_del == 0).ToList();

            IQueryable<Users> users = context.Users.Where(p => p.Mark_del == 0);

            int Law_Id = 0;
            String addressParam = "";
            String telParam = "";
            String emailParam = "";
            String fioParam = "";
            String pasportParam = "";
            String loginParam = "";

            // Нажата кнопка Отобрать

            if (Request.Params["select"] != null)
            {

                // Право
                if (int.TryParse(Request.Params["Law_Id"], out Law_Id) && Law_Id > 0)
                {
                    users = users.Where(p => p.User_Law_Id == Law_Id);
                }


                // ФИО

                fioParam = Request.Params["User_fio"];

                if (fioParam != null && fioParam.Trim().Length > 1)
                {
                    users = users.Where(p => p.User_fio.Contains(fioParam.Trim()));
                }


                // Адрес

                addressParam = Request.Params["User_address"];

                if (addressParam != null && addressParam.Trim().Length > 1)
                {
                    users = users.Where(p => p.User_address.Contains(addressParam.Trim()));
                }

                // Телефон

                telParam = Request.Params["User_tel"];

                if (telParam != null && telParam.Trim().Length > 1)
                {
                    users = users.Where(p => p.User_tel.Contains(telParam.Trim()));
                }

                // Паспорт

                pasportParam = Request.Params["User_pasport"];

                if (pasportParam != null && pasportParam.Trim().Length > 1)
                {
                    users = users.Where(p => p.User_pasport.Contains(telParam.Trim()));
                }

                // Логин

                loginParam = Request.Params["User_log"];

                if (loginParam != null && loginParam.Trim().Length > 1)
                {
                    users = users.Where(p => p.User_log.Contains(loginParam.Trim()));
                }

                // Email

                emailParam = Request.Params["User_email"];

                if (emailParam != null && emailParam.Trim().Length > 1)
                {
                    users = users.Where(p => p.User_email.Contains(emailParam.Trim()));
                }

            }

            ViewBag.Users = users.ToList();
            ViewBag.Law_Id = Law_Id;
            ViewBag.User_fio = fioParam;
            ViewBag.User_address = addressParam;
            ViewBag.User_tel = telParam;
            ViewBag.User_email = emailParam;
            ViewBag.User_log = loginParam;
            ViewBag.User_pasport = pasportParam;

            return View();
        }




        public ActionResult PageRecords()
        {
            ViewBag.USER = USER;
            int Order_Id = int.Parse(Request.Params["Id"]);

            List<Records> records = context.Records.Where(p => p.Record_Order_Id == Order_Id).ToList();
            ViewBag.Records = records;
            return View();
        }



        // ========================== Bag Order

        public ActionResult PageOrders()
        {
            ViewBag.USER = USER;

            if (Request.Params["delete"] != null)
            {
                int Order_Id = int.Parse(Request.Params["Id"]);

                // Возврат списка техники в корзину и удаление.
                List<Records> records = context.Records.Where(p => p.Record_Order_Id == Order_Id).ToList();
                records.ForEach(p => {
                    context.Records.DeleteOnSubmit(p);
                    context.Records.Context.SubmitChanges();
                });

                // Удаление заказа из списков заказов.
                Orders order = context.Orders.Where(p => p.Id == Order_Id).First();
                context.Orders.DeleteOnSubmit(order);
                context.Orders.Context.SubmitChanges();

              
            }

            if (Request.Params["ret"] != null)
            {
                int Order_Id = int.Parse(Request.Params["Id"]);

                /* Возврат списка техники в корзину и удаление.
                List<Records> records = context.Records.Where(p => p.Record_Order_Id == Order_Id).ToList();
                records.ForEach(p => {
                    TECHNICS.Add(p.Record_Technic_Id);
                    context.Records.DeleteOnSubmit(p);
                    context.Records.Context.SubmitChanges();
                });
                */

                // Удаление заказа из списков заказов.
                Orders order = context.Orders.Where(p => p.Id == Order_Id).First();
                context.Orders.DeleteOnSubmit(order);
                context.Orders.Context.SubmitChanges();

                return Redirect("/Home/PageBag");
            }

            // Администрирование.

            if (USER!=null && USER.User_Law_Id == 1)
            {
                ViewBag.orders = context.Orders.Where(p => p.Mark_del == 0).ToList();
            }
            else
            {
                ViewBag.orders = context.Orders.Where(p => p.Mark_del == 0 && p.Order_User_Id == USER.Id).ToList();
            }

            return View();
        }



        
        public ActionResult PageBag()
        {
            ViewBag.USER = USER;

            List<Categories> technics = new List<Categories>();



            // Нажата кнопка ПОДАТЬ ЗАЯВКУ

            if (Request.Params["order"] !=null)
            {

                // Составляем 
                foreach (int Id in TECHNICS)
                {


                    // Составляем данные заявки.
                    Orders order = new Orders
                {
                    Order_User_Id = USER.Id,
                    Order_date = DateTime.Now,
                    Order_paid = 0,
                    Order_cat = Id, 
                    Order_State_Id = 1,
                    Order_Way_Id = int.Parse(Request.Params["Way_Id"]),
                    Order_delivery = null,
                    Mark_del = 0
                };

                context.Orders.InsertOnSubmit(order);
                context.Orders.Context.SubmitChanges();

                }

                // Очищаем корзину
                TECHNICS.Clear();

                // Перенаправляем на страницу Мои заказы
                return Redirect("/Home/PageOrders");
            }



            // Вывод корзины

            foreach (int Id in TECHNICS)
            {
                technics.Add (context.Categories.Where(p => p.Id == Id).First());
            }

            ViewBag.Technics = technics ;
            ViewBag.Ways = context.Ways.Where(p => p.Mark_del == 0).ToList();
            return View();
        }



       // только обновление информации.
        public bool GetOrderFromParam(out Orders order)
        {
            bool Ok = true;

            int Id = int.Parse(Request.Params["Id"]);
            order = context.Orders.Where(p => p.Id == Id).First();


            // Проверка поля.
            try
            {
                order.Order_delivery = DateTime.Parse(Request.Params["Order_delivery"]);
            }
            catch
            {
                Ok = false;
                if (ViewBag.Message.Trim().Length > 0) ViewBag.Message += ", ";
                ViewBag.Message += "Дата доставки";
                order.Order_delivery = DateTime.Now;
            }

            order.Order_paid = int.Parse(Request.Params["Order_paid"]);

            // Проверка поля.
            if (int.Parse(Request.Params["State_Id"])<1)
            {
                Ok = false;
                if (ViewBag.Message.Trim().Length > 0) ViewBag.Message += ", ";
                ViewBag.Message += "Статус заказа";
                order.Order_State_Id = 0;
            }
            else
            {
                order.Order_State_Id = int.Parse(Request.Params["State_Id"]);
            }

            if (!Ok)
            {
                ViewBag.Message = "Ошибка ввода полей: " + ViewBag.Message;
            }

            return Ok;
        }


        // только изменение
        // update - команда изменить
        public ActionResult EditOrder()
        {
                        
            ViewBag.USER = USER;
            ViewBag.Message = "";
            ViewBag.Caption = "";

            // Списки для формы.
            ViewBag.States = context.States.Where(p => p.Mark_del == 0).ToList();



            // Команда отмены.
            if (Request.Params["cancel"] != null)
            {
                return Redirect("/Home/PageOrders");
            }



            // Команда изменения.
            if (Request.Params["update"] != null)
            {
                Orders order;
                if (GetOrderFromParam(out order))
                {
                    context.Orders.Context.SubmitChanges();
                    return Redirect("/Home/PageOrders");
                }
                // Ошибка ввода данных в поля формы.
                else
                {
                    ViewBag.Caption = "Изменить";
                    ViewBag.Mode = "update";
                    ViewBag.Order = order;
                    return View();
                }
            }







            // Режим формы - edit
            if (Request.Params["edit"] != null)
            {
                ViewBag.Caption = "Изменить";
                ViewBag.Mode = "update";
                int Id = int.Parse(Request.Params["Id"]);
                ViewBag.Order = context.Orders.Where(p => p.Id == Id).First();
                return View();
            }

         
            return View();            
        }




        // ========================= Category ======================
        // ================================================


        // update = true, если обновление информации.
        // update = false, если добавление новой информации.
        public bool GetCategoryFromParam(bool update, out Categories category)
        {
            bool Ok = true;

            // Для режима обновления информации.
            if (update)
            {
                int Id = int.Parse(Request.Params["Id"]);
                category = context.Categories.Where(p => p.Id == Id).First();
            }
            // Если добавление новой записи.
            else
            {
                category = new Categories();
            }

            // Проверка поля.
            if ((category.Category_name = Request.Params["Category_name"]).Trim().Length < 1)
            {
                Ok = false;
                if (ViewBag.Message.Trim().Length > 0) ViewBag.Message += ", ";
                ViewBag.Message += "Наименование категории";
            }


            if (!Ok)
            {
                ViewBag.Message = "Ошибка ввода полей: " + ViewBag.Message;
            }

            return Ok;
        }


        // add - переход на формы добавления
        // edit - переход форму изменения

        // insert - команда добавить
        // update - команда изменить
        // delete - команда удалить
        public ActionResult EditCategory()
        {

            ViewBag.USER = USER;
            ViewBag.Message = "";
            ViewBag.Caption = "";


            // Команда удаления.
            if (Request.Params["delete"] != null)
            {
                int Id = int.Parse(Request.Params["Id"]);
                Categories category = context.Categories.Where(p => p.Id == Id).First();
                context.Categories.DeleteOnSubmit(category);
                context.Categories.Context.SubmitChanges();
                return Redirect("/Home/PageCategories");
            }


            // Команда удаления.
            if (Request.Params["cancel"] != null)
            {
                ViewBag.Category_Id = Request.Params["Category_Id"];
                return Redirect("/Home/PageCategories");
            }

            // Команда изменения.
            if (Request.Params["update"] != null)
            {
                Categories category;
                if (GetCategoryFromParam(true, out category))
                {
                    context.Categories.Context.SubmitChanges();
                    return Redirect("/Home/PageCategories");
                }
                // Ошибка ввода данных в поля формы.
                else
                {
                    ViewBag.Caption = "Изменить";
                    ViewBag.Mode = "update";
                    ViewBag.category = category;
                    return View();
                }
            }




            // Команда добавления.
            if (Request.Params["insert"] != null)
            {
                Categories category;
                if (GetCategoryFromParam(false, out category))
                {
                    context.Categories.InsertOnSubmit(category);
                    context.Categories.Context.SubmitChanges();

                    return Redirect("/Home/PageCategories");
                }
                // Ошибка ввода данных в поля формы.
                else
                {
                    // Режим формы - add
                    ViewBag.Caption = "Добавить";
                    ViewBag.Mode = "insert";
                    ViewBag.category = category;
                    return View();
                }
            }




            // Режим формы - edit
            if (Request.Params["edit"] != null)
            {
                ViewBag.Caption = "Изменить";
                ViewBag.Mode = "update";
                int Id = int.Parse(Request.Params["Id"]);
                ViewBag.category = context.Categories.Where(p => p.Id == Id).First();
                return View();
            }




            // Режим формы - add
            ViewBag.Caption = "Добавить";
            ViewBag.Mode = "insert";
            ViewBag.Category = new Categories
            {
                Category_name = "",
                Mark_del = 0
            };
            return View();
        }




        // select - отбор записей
        public ActionResult PageCategories()
        {

            ViewBag.USER = USER;
            ViewBag.Message = "Каталог категорий";


            // Кнопка В корзину
            if (Request.Params["bag"] != null)
            {
                TECHNICS.Add(int.Parse(Request.Params["Id"]));
            }
         



            IQueryable<Categories> categories = context.Categories.Where(p => p.Mark_del == 0);

            String Category_name = "";

            // Нажата кнопка Отобрать

            if (Request.Params["select"] != null)
            {

                // Название
                Category_name = Request.Params["Category_name"];

                if (Category_name != null && Category_name.Trim().Length > 1)
                {
                    categories = categories.Where(p => p.Category_name.Contains(Category_name.Trim()));
                }
            }


            ViewBag.Category_name = Category_name;
            ViewBag.Categories = categories;

            return View();
        }










        // ========================= State ======================
        // ================================================


        // update = true, если обновление информации.
        // update = false, если добавление новой информации.
        public bool GetStateFromParam(bool update, out States state)
        {
            bool Ok = true;

            // Для режима обновления информации.
            if (update)
            {
                int Id = int.Parse(Request.Params["Id"]);
                state = context.States.Where(p => p.Id == Id).First();
            }
            // Если добавление новой записи.
            else
            {
                state = new States();
            }

            // Проверка поля.
            if ((state.State_name = Request.Params["State_name"]).Trim().Length < 1)
            {
                Ok = false;
                if (ViewBag.Message.Trim().Length > 0) ViewBag.Message += ", ";
                ViewBag.Message += "Наименование статуса";
            }


            if (!Ok)
            {
                ViewBag.Message = "Ошибка ввода полей: " + ViewBag.Message;
            }

            return Ok;
        }


        // add - переход на формы добавления
        // edit - переход форму изменения

        // insert - команда добавить
        // update - команда изменить
        // delete - команда удалить
        public ActionResult EditState()
        {

            ViewBag.USER = USER;
            ViewBag.Message = "";
            ViewBag.Caption = "";


            // Команда удаления.
            if (Request.Params["delete"] != null)
            {
                int Id = int.Parse(Request.Params["Id"]);
                States state = context.States.Where(p => p.Id == Id).First();
                context.States.DeleteOnSubmit(state);
                context.States.Context.SubmitChanges();
                return Redirect("/Home/PageStates");
            }


            // Команда удаления.
            if (Request.Params["cancel"] != null)
            {
                ViewBag.State_Id = Request.Params["State_Id"];
                return Redirect("/Home/PageStates");
            }

            // Команда изменения.
            if (Request.Params["update"] != null)
            {
                States state;
                if (GetStateFromParam(true, out state))
                {
                    context.States.Context.SubmitChanges();
                    return Redirect("/Home/PageStates");
                }
                // Ошибка ввода данных в поля формы.
                else
                {
                    ViewBag.Caption = "Изменить";
                    ViewBag.Mode = "update";
                    ViewBag.state = state;
                    return View();
                }
            }




            // Команда добавления.
            if (Request.Params["insert"] != null)
            {
                States state;
                if (GetStateFromParam(false, out state))
                {
                    context.States.InsertOnSubmit(state);
                    context.States.Context.SubmitChanges();

                    return Redirect("/Home/PageStates");
                }
                // Ошибка ввода данных в поля формы.
                else
                {
                    // Режим формы - add
                    ViewBag.Caption = "Добавить";
                    ViewBag.Mode = "insert";
                    ViewBag.state = state;
                    return View();
                }
            }




            // Режим формы - edit
            if (Request.Params["edit"] != null)
            {
                ViewBag.Caption = "Изменить";
                ViewBag.Mode = "update";
                int Id = int.Parse(Request.Params["Id"]);
                ViewBag.state = context.States.Where(p => p.Id == Id).First();
                return View();
            }




            // Режим формы - add
            ViewBag.Caption = "Добавить";
            ViewBag.Mode = "insert";
            ViewBag.State = new States
            {
                State_name = "",
                Mark_del = 0
            };
            return View();
        }




        // select - отбор записей
        public ActionResult PageStates()
        {

            ViewBag.USER = USER;
            ViewBag.Message = "Каталог статусов";

            IQueryable<States> states = context.States.Where(p => p.Mark_del == 0);

            String State_name = "";

            // Нажата кнопка Отобрать

            if (Request.Params["select"] != null)
            {

                // Название
                State_name = Request.Params["State_name"];

                if (State_name != null && State_name.Trim().Length > 1)
                {
                    states = states.Where(p => p.State_name.Contains(State_name.Trim()));
                }
            }


            ViewBag.State_name = State_name;
            ViewBag.States = states;

            return View();
        }


    }
}