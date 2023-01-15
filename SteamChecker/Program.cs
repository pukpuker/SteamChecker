using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Windows.Forms;
using System.Runtime.Remoting.Contexts;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.IO;

namespace SteamChecker
{
    internal class Program
    {
        //userinput
        public static string login_steam;
        public static string password_steam;
        //userinput
        //bools
        public static bool incorrect_password;
        public static bool mobileguard;
        public static bool emailguard;
        //bools
        //strings
        public static string email;
        public static string country;
        public static string money;
        public static string steamguard_status;
        public static string games;
        //strings
        IWebDriver driver;

        public bool SteamGuard_Checker()
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                var myElement = wait.Until(x => x.FindElement(By.CssSelector("#responsive_page_template_content > div.page_content > div:nth-child(1) > div > div > div > div.newlogindialog_FormContainer_3jLIH > div > div.newlogindialog_ProtectingAccount_1aop9 > div.newlogindialog_Description_QApnT")));
                return myElement.Displayed;
            }
            catch
            {
                return false;
            }
        }

        async void start_browser()
        {
            ChromeOptions options = new ChromeOptions();
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            //options.AddArgument("--window-size=1920,1080");
            //options.AddArgument("--no-sandbox");
            //options.AddArgument("--headless");
            //options.AddArgument("--disable-gpu");
            //options.AddArgument("--disable-crash-reporter");
            //options.AddArgument("--disable-extensions");
            //options.AddArgument("--disable-in-process-stack-traces");
            //options.AddArgument("--disable-logging");
            //options.AddArgument("--disable-dev-shm-usage");
            //options.AddArgument("--log-level=3");
            //options.AddArgument("--output=/dev/null");
            driver = new ChromeDriver(driverService, options);
            await checker();
        }
        void steam_guard_checker()
        {
            try
            {
                Task.Delay(4000).Wait();
                IWebElement steam_guard_checker = driver.FindElement(By.CssSelector("#responsive_page_template_content > div.page_content > div:nth-child(1) > div > div > div > div.newlogindialog_FormContainer_3jLIH > div > div.newlogindialog_ProtectingAccount_1aop9 > div.newlogindialog_Description_QApnT"));
                if (steam_guard_checker.Text == "У вас настроен мобильный аутентификатор для защиты аккаунта.")
                {
                    mobileguard = true;
                }
                //IWebElement steam_guardEMAIL_checker = driver.FindElement(By.CssSelector("#responsive_page_template_content > div > div:nth-child(1) > div > div > div > div.newlogindialog_FormContainer_3jLIH > form > div > div.newlogindialog_ProtectingAccount_1aop9 > div.newlogindialog_Description_QApnT"));
                //if (steam_guardEMAIL_checker.Text == "У вас настроен аутентификатор электронной почты для защиты аккаунта.")
                //{
                //    emailguard = true;
                //}
                else
                {
                    MessageBox.Show("error #4");
                }
            }
            catch
            {
                try
                {
                    IWebElement steam_guardEMAIL_checker = driver.FindElement(By.CssSelector("#responsive_page_template_content > div > div:nth-child(1) > div > div > div > div.newlogindialog_FormContainer_3jLIH > form > div > div.newlogindialog_ProtectingAccount_1aop9 > div.newlogindialog_Description_QApnT"));
                    if (steam_guardEMAIL_checker.Text == "У вас настроен аутентификатор электронной почты для защиты аккаунта.")
                    {
                        emailguard = true;
                    }
                }
                catch
                {
                    emailguard = true;
                }
                mobileguard = false;
                return;
            }
        }

        void games_checker()
        {
            try
            {
                driver.FindElement(By.CssSelector("#responsive_page_template_content > div.no_header.profile_page > div.profile_content > div > div.profile_rightcol > div.responsive_count_link_area > div.profile_item_links > div:nth-child(1) > a > span.count_link_label")).Click();
                IWebElement get_games = driver.FindElement(By.CssSelector("#games_list_rows"));
                games = get_games.Text;
                var oldLines = System.IO.File.ReadAllLines("games.txt");
                var newLines = oldLines.Where(line => !line.Contains("Ссылки"));
                var newLines2 = newLines.Where(line => !line.Contains("Статистика"));
                var newLines3 = newLines2.Where(line => !line.Contains("Написать"));
                File.Delete("games.txt");
                File.WriteAllLines("games.txt", newLines3);
                games = File.ReadAllText("games.txt");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        void checker_about_account()
        {
            try
            {
                driver.Navigate().GoToUrl("https://store.steampowered.com/account/");
                Task.Delay(1000).Wait();
                IWebElement get_email = driver.FindElement(By.CssSelector("#main_content > div.two_column.right > div:nth-child(4) > div:nth-child(1) > div:nth-child(2) > div:nth-child(1) > span.account_data_field"));
                IWebElement get_country = driver.FindElement(By.CssSelector("#main_content > div.two_column.right > div:nth-child(2) > div:nth-child(3) > div > p > span"));
                IWebElement get_money = driver.FindElement(By.CssSelector("#main_content > div.two_column.right > div:nth-child(2) > div:nth-child(1) > div.accountRow.accountBalance > div.accountData.price"));
                IWebElement get_steamguard_status = driver.FindElement(By.CssSelector("#main_content > div.two_column.right > div:nth-child(6) > div:nth-child(1) > div.account_security_block > div.account_data_field"));
                email = get_email.Text;
                country = get_country.Text;
                money = get_money.Text;
                steamguard_status = get_steamguard_status.Text;
                driver.Navigate().Back();
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        void check_password()
        {
            try
            {
                driver.Navigate().GoToUrl("https://steamcommunity.com/login/home/");
                Task.Delay(2000).Wait();
                IWebElement login_input = driver.FindElement(By.CssSelector("#responsive_page_template_content > div.page_content > div:nth-child(1) > div > div > div > div.newlogindialog_FormContainer_3jLIH > div > form > div:nth-child(1) > input"));
                login_input.SendKeys(login_steam);
                IWebElement password_input = driver.FindElement(By.CssSelector("#responsive_page_template_content > div.page_content > div:nth-child(1) > div > div > div > div.newlogindialog_FormContainer_3jLIH > div > form > div:nth-child(2) > input"));
                password_input.SendKeys(password_steam);
                driver.FindElement(By.CssSelector("#responsive_page_template_content > div.page_content > div:nth-child(1) > div > div > div > div.newlogindialog_FormContainer_3jLIH > div > form > div.newlogindialog_SignInButtonContainer_14fsn > button")).Click();
                Task.Delay(3000).Wait();
                IWebElement get_error_text = driver.FindElement(By.CssSelector("#responsive_page_template_content > div.page_content > div:nth-child(1) > div > div > div > div.newlogindialog_FormContainer_3jLIH > div > form > div.newlogindialog_FormError_1Mcy9"));
                if (get_error_text.Text == "Пожалуйста, проверьте свой пароль и имя аккаунта и попробуйте снова.")
                {
                    MessageBox.Show("Invalid Account", "Ragul Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    driver.Quit();
                    Environment.Exit(2);
                }
            }
            catch
            {
                incorrect_password = false;
                return;
            }
        }

        async Task checker()
        {
            try
            {
                check_password();
                if (incorrect_password == false)
                {
                    steam_guard_checker();
                    if (mobileguard == false && emailguard == false)
                    {
                        Console.WriteLine("[+] Auth");
                        checker_about_account();
                        Console.WriteLine("Email: " + email);
                        Console.WriteLine("Country: " + country);
                        Console.WriteLine("Money: " + money);
                        Console.WriteLine("Steam Guard Status: " + steamguard_status);
                        games_checker();
                        Console.WriteLine("\nGames:");
                        Console.WriteLine(games);
                        driver.Quit();
                        Console.ReadKey(true);
                        Environment.Exit(0);
                    }
                    else if (mobileguard == true && emailguard == false)
                    {
                        Console.WriteLine("[~] Account have Mobile Steam Guard. Confirm the session on your phone");
                        Console.ReadKey(true);
                    }
                    if (emailguard == true && mobileguard == false)
                    {
                        Console.Write("\n[~] Account have email guard, enter code: ");
                        string temp_email_guard = Console.ReadLine();
                        IWebElement input_email_guard = driver.FindElement(By.CssSelector("#responsive_page_template_content > div > div:nth-child(1) > div > div > div > div.newlogindialog_FormContainer_3jLIH > form > div > div.newlogindialog_FlexCol_1mhmm.newlogindialog_AlignItemsCenter_30P8x > div > input[type=text]:nth-child(1)"));
                        input_email_guard.SendKeys(temp_email_guard);
                        Task.Delay(1500).Wait();
                        Console.WriteLine("[+] Auth");
                        checker_about_account();
                        Console.WriteLine("Email: " + email);
                        Console.WriteLine("Country: " + country);
                        Console.WriteLine("Money: " + money);
                        Console.WriteLine("Steam Guard Status: " + steamguard_status);
                        games_checker();
                        Console.WriteLine("\nGames:");
                        Console.WriteLine(games);
                        driver.Quit();
                        Console.ReadKey(true);
                        Environment.Exit(0);
                    }
                }
                else
                {
                    MessageBox.Show("и как ты сюда попал рагуль?", "ошибка кода 2");
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        static async Task Main(string[] args)
        {
            Console.Title = "Ragul Checker";
            Program foo = new Program();
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(foo.CurrentDomain_ProcessExit);
            if (args.Length != 0)
            {
                login_steam = args[0];
                password_steam = args[1];
                foo.start_browser();
            }
            Console.Write("Enter login: ");
            login_steam = Console.ReadLine();
            Console.Write("Enter Password: ");
            password_steam = Console.ReadLine();
            foo.start_browser();
        }
        void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            try
            {
                driver.Close();
            }
            catch (ObjectDisposedException)
            {
                MessageBox.Show("Прощай.");
            }
        }
    }
}
