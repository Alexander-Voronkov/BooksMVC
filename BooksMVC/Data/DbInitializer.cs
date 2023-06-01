using BooksMVC.Models;
using System.Net;
using static System.Net.WebRequestMethods;

namespace BooksMVC.Data
{
    public static class DbInitializer
    {
        public static async Task Init(BookContext context)
        {
            if(!context.Authors.Any()||!context.Books.Any())
            {
                var client = new WebClient();
                byte[] img1 = client.DownloadData(new Uri("https://mobimg.b-cdn.net/v3/fetch/9d/9db2d4683d92f5f2045e9142fbd82633.jpeg"));
                byte[] img2 = client.DownloadData("https://bipbap.ru/wp-content/uploads/2017/04/0_7c779_5df17311_orig.jpg");
                byte[] img3 = client.DownloadData("https://avatars.mds.yandex.net/i?id=dc7361b95e9b0527c543cbb558a72055_l-5878560-images-thumbs&n=27&h=384&w=480");
                context.Books.AddRange(
                    new Book()
                    {
                        Title = "War and peace",
                        Style = "Novel",
                        Authors = new List<Author> { new Author() { Name = "Alexander Voronkov" } },
                        Cover = img1,
                        PublishDate = DateTime.Now.AddDays(20)
                    },
                    new Book()
                    {
                        Title = "1984",
                        Style = "Utopy",
                        Authors = new List<Author> { new Author() { Name = "Rusak Marina" } },
                        Cover = img2,
                        PublishDate = DateTime.Now.AddDays(10)
                    },
                    new Book()
                    {
                        Title = "AAAA",
                        Style = "GGGG",
                        Authors = new List<Author> { new Author() { Name = "Beluha Anton" } },
                        Cover = img3,
                        PublishDate = DateTime.Now
                    });

                context.SaveChanges();
            }
        }
    }
}
