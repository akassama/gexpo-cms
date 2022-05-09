using NgoExpoApp.Models;
using System.Linq;
using Xunit;

namespace GexpoCmsUnitTests
{
    public class AuthenticationTests 
    {
        AppFunctions functions = new AppFunctions();
        DBConnection _context;

        public string Email = "admin@example.com";
        public string Password = "Admin123";


        /// <summary>
        /// Test to see if password is returned
        /// </summary>
        [Fact]
        public void HasPassword()
        {
           _context = new DBConnection();

            // check password
            var query = _context.Accounts.Where(s => s.Email == Email);
            string hashedPassword = (query.Any()) ? query.FirstOrDefault().Password : "";

            Assert.True(!string.IsNullOrEmpty(hashedPassword));
        }


        /// <summary>
        /// Test to check for valid login
        /// </summary>
        [Fact]
        public void ValidLogin()
        {
            _context = new DBConnection();

            // check password
            var query = _context.Accounts.Where(s => s.Email == Email);
            string hashedPassword = (query.Any()) ? query.FirstOrDefault().Password : "";

            Assert.True(BCrypt.Net.BCrypt.Verify(Password, hashedPassword));
        }


        /// <summary>
        /// Test to check for invalid login
        /// </summary>
        [Fact]
        public void InValidLogin()
        {
            _context = new DBConnection();

            Password = "WrongPassword"; //ovveride valid password

            // check password
            var query = _context.Accounts.Where(s => s.Email == Email);
            string hashedPassword = (query.Any()) ? query.FirstOrDefault().Password : "";

            Assert.False(BCrypt.Net.BCrypt.Verify(Password, hashedPassword));
        }
    }
}
