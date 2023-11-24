using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using DatabaseLibrary;
using System.Data.SqlClient;

class Program
{
    static void Main()
    {
        string server = "DESKTOP-LTUDAGK\\SQLEXPRESS";
        string database = "Lab4";

        string connectionString = $"Data Source={server}; Initial Catalog={database}; Integrated Security=true";

        string forumUrl = "https://habr.com/ru/articles/";

        using (IWebDriver driver = new ChromeDriver())
        {
            driver.Navigate().GoToUrl(forumUrl);

            var articles = driver.FindElements(By.TagName("article"));

            DatabaseHelper dbHelper = new DatabaseHelper(server, database);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                dbHelper.DeleteAll();

                foreach (var articleElement in articles)
                {
                    int Id = int.Parse(articleElement.GetAttribute("id"));
                    string name = articleElement.FindElement(By.ClassName("tm-user-info__username")).Text;
                    string message = articleElement.FindElement(By.CssSelector(".article-formatted-body.article-formatted-body")).Text;

                    dbHelper.Add(Id, name, message);
                }
            }
        }
    }
}
