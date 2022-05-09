using GexpoTechCMS.Models.AppModels;
using NgoExpoApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace GexpoCmsUnitTests
{
    public class RegistrationTests
    {
        AppFunctions functions = new AppFunctions();
        DBConnection _context;

        public string Email = "test@email.com";

        /// <summary>
        /// Tests for valid form inputs
        /// </summary>
        [Fact]
        public void ValidInputs()
        {
            _context = new DBConnection();

            AccountModel accountsModel = new AccountModel();

            //set registration default values
            accountsModel.AccountID = functions.GetGuid();
            accountsModel.FirstName = "Test";
            accountsModel.LastName = "User";
            accountsModel.Email = "test@email.com";
            accountsModel.Password = "TestPassword";
            string ConfirmPassword = "TestPassword";
            accountsModel.DirectoryName = functions.GenerateDirectoryName(accountsModel.Email);
            accountsModel.Active = false;
            accountsModel.Oauth = false;
            accountsModel.EmailVerification = false;
            accountsModel.EmailNotification = false;
            accountsModel.UpdatedAt = DateTime.Now;
            accountsModel.CreatedAt = DateTime.Now;

            string[] ValidationInputs = { accountsModel.AccountID, accountsModel.FirstName, accountsModel.LastName, accountsModel.Email,
                accountsModel.Password, accountsModel.DirectoryName };
            bool validInputs = functions.ValidateInputs(ValidationInputs);
            bool passwordMatch = functions.PasswordsMatch(accountsModel.Password, ConfirmPassword);
            bool emailExists = _context.Accounts.Any(s => s.Email == accountsModel.Email);

            Assert.True(passwordMatch && !emailExists && validInputs);
        }


        /// <summary>
        /// Tests for valid registration
        /// </summary>
        [Fact]
        public async System.Threading.Tasks.Task ValidRegistrationAsync()
        {
            _context = new DBConnection();

            AccountModel accountsModel = new AccountModel();

            //set registration default values
            accountsModel.AccountID = functions.GetGuid();
            accountsModel.FirstName = "Test";
            accountsModel.LastName = "User";
            accountsModel.Email = "test@email.com";
            accountsModel.Password = "TestPassword";
            accountsModel.Password = BCrypt.Net.BCrypt.HashPassword(accountsModel.Password);
            accountsModel.DirectoryName = functions.GenerateDirectoryName(accountsModel.Email);
            accountsModel.Active = false;
            accountsModel.Oauth = false;
            accountsModel.EmailVerification = false;
            accountsModel.EmailNotification = false;
            accountsModel.UpdatedAt = DateTime.Now;
            accountsModel.CreatedAt = DateTime.Now;

            _context.Add(accountsModel);
            var createdRecord = await _context.SaveChangesAsync();

            //assert if a record is created
            Assert.True(createdRecord > 0);
        }


        /// <summary>
        /// Test to see if account data (id) exists
        /// </summary>
        [Fact]
        public void HasAccountData()
        {
            _context = new DBConnection();

            string Email = "test@email.com";
            var query = _context.Accounts.Where(s => s.Email == Email);
            string accountID = query.FirstOrDefault().AccountID;

            Assert.True(!string.IsNullOrEmpty(accountID));
        }


        /// <summary>
        /// Test to remove account record
        /// </summary>
        //[Fact]
        //public void RemoveAccountData()
        //{
        //    _context = new DBConnection();

        //    string Email = "test@email.com";
        //    var query = _context.Accounts.Where(s => s.Email == Email);
        //    string accountID = query.FirstOrDefault().AccountID;

        //    Assert.True(functions.DeleteTableData("Accounts", "accountID", accountID));
        //}

    }
}
