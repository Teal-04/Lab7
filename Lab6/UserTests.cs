using Lab6;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace Lab6
{
    [TestClass]
    public class UserTests
    {
        private string usersfilepath = "";
        private List<User> users;


        [TestInitialize]
        public void Setup()
        {
            // This part just pissed me off cause it would default to the bin/Debug/net8.0/Data, found this online its pretty neat
            var projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            usersfilepath = Path.Combine(projectDirectory, "Data", "Users.csv");
            users = new List<User>();
            foreach (var line in File.ReadLines(usersfilepath))
            {
                var fields = line.Split(',');
                if (fields.Length >= 3)
                {
                    users.Add(new User{Id = int.Parse(fields[0].Trim()), Name = fields[1].Trim(), Email = fields[2].Trim()});
                }
            }
        }


        [TestMethod]
        public void TestAddUser()
        {
            // Arrange
            var initialCount = users.Count; 
            var newUser = new User { Id = users.Max(u => u.Id) + 1, Name = "Macho Man Randy Savage", Email = "email@email.com" };
            // Act
            users.Add(newUser);
            // Assert
            Assert.AreEqual(initialCount + 1, users.Count, "the user count should have increased by 1 -_-");
            Assert.AreEqual("Macho Man Randy Savage", users.Last().Name, "the last user added does not match the expected name -_-");
            Assert.AreEqual("email@email.com", users.Last().Email, "the last user added does not match the expected email -_-");
        }


        [TestMethod]
        public void TestEditUser()
        {
            // Arrange
            var userToEdit = users.First();
            var newName = "Updated Name";
            var newEmail = "updatedemail@example.com";
            // Act
            userToEdit.Name = newName;
            userToEdit.Email = newEmail;
            // Assert
            Assert.AreEqual(newName, userToEdit.Name, "the users name was not updated correctly -_-");
            Assert.AreEqual(newEmail, userToEdit.Email, "the users email was not updated correctly -_-");
        }


        [TestMethod]
        public void TestDeleteUser()
        {
            // Arrange
            var userToDelete = users.First();
            var initialCount = users.Count;
            // Act
            users.Remove(userToDelete);
            // Assert
            Assert.AreEqual(initialCount - 1, users.Count, "the user count did not decrease after deletion -_-");
            Assert.IsFalse(users.Any(u => u.Id == userToDelete.Id), "the deleted user still exists in the list -_-");
        }


        [TestMethod]
        public void TestListUsers()
        {
            // Arrange & Act
            var userList = users.Select(u => $"{u.Id}: {u.Name} (Email: {u.Email})").ToList();
            // Assert
            Assert.IsTrue(userList.Count > 0, "the user list should not be empty -_-");
            Assert.IsTrue(userList.Any(u => u.Contains("Mason")), "the user list should contain Mason (I mean I created this shit) -_-");
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestEditNonexistentUser()
        {
            // Arrange
            var nonexistentUserId = 9999; 
            // Act
            var user = users.First(u => u.Id == nonexistentUserId); 
            user.Name = "Updated Name";
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestDeleteNonexistentUser()
        {
            // Arrange
            var nonexistentUserId = 9999; 
            // Act
            var user = users.FirstOrDefault(u => u.Id == nonexistentUserId);
            if (user == null)
            {
                throw new InvalidOperationException("user not found -_-");
            }
            users.Remove(user);
        }
    }
}
