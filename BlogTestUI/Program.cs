using BlogDataLibrary.Data;
using BlogDataLibrary.Database;
using BlogDataLibrary.Models;
using Microsoft.Extensions.Configuration;

namespace BlogTestUI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SqlData db = GetConnection();
            Authenticate(db);
            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }

        private static SqlData GetConnection()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            IConfiguration config = builder.Build();
            SqlDataAccess dba = new SqlDataAccess(config);
            SqlData db = new SqlData(dba);

            return db;
        }

        private static UserModel GetCurrentUser(SqlData db)
        {
            Console.Write("Username: ");
            string username = Console.ReadLine();

            Console.Write("Password: ");
            string password = Console.ReadLine();

            UserModel user = db.Authenticate(username, password);
            return user;
        }

        public static void Authenticate(SqlData db)
        {
            UserModel user = GetCurrentUser(db);

            if (user == null)
            {
                Console.WriteLine("Invalid credentials.");
            }
            else
            {
                Console.WriteLine($"Welcome, {user.UserName}");
            }
        }
        public static void Register(SqlData db)
        {
            Console.Write("Enter new username: ");
            var username = Console.ReadLine();

            Console.Write("Enter new password: ");
            var password = Console.ReadLine();

            Console.Write("Enter first name: ");
            var firstName = Console.ReadLine();

            Console.Write("Enter last name: ");
            var lastName = Console.ReadLine();

            db.Register(username, firstName, lastName, password);
            Console.WriteLine("Registration successful!");
        }
        private static void AddPost(SqlData db)
        {
            UserModel user = GetCurrentUser(db);

            Console.Write("Title: ");
            string title = Console.ReadLine();

            Console.WriteLine("Write body: ");
            string body = Console.ReadLine();

            PostModel post = new PostModel
            {
                Title = title,
                Body = body,
                DateCreated = DateTime.Now,
                UserId = user.Id
            };

            db.AddPost(post);
            Console.WriteLine("Post added successfully!");
        }
        public static void ListPosts(SqlData db)
        {
            List<ListPostModel> posts = db.ListPosts();

            foreach (ListPostModel post in posts)
            {
                Console.WriteLine($"[{post.Id}]: Title: {post.Title} by {post.UserName}");
                Console.WriteLine($"    {post.Body.Substring(0, 20)}...");
            }
        }
        private static void ShowPostDetails(SqlData db)
        {
            Console.Write("Enter a post ID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var post = db.ShowPostDetails(id);

                if (post != null)
                {
                    Console.WriteLine($"This post was written by {post.FirstName} {post.LastName}");
                    Console.WriteLine($"UserName: {post.UserName}");
                    Console.WriteLine(post.Body);
                    Console.WriteLine(post.DateCreated.ToString("MMMM d yyyy"));
                }
                else
                {
                    Console.WriteLine("Post not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }
        }


    }
}
