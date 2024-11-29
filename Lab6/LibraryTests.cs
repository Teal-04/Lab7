using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
namespace Lab6
{
    [TestClass]
    public class LibraryTests
    {
        private string booksfilepath = "";
        private string usersfilepath = "";
        private List<Book> books;
        private List<User> users;
        private Dictionary<User, List<Book>> borrowedBooks;


        [TestInitialize]
        public void Setup()
        {
            // This part just pissed me off cause it would default to the bin/Debug/net8.0/Data, found this online its pretty neat
            var projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            usersfilepath = Path.Combine(projectDirectory, "Data", "Users.csv");
            booksfilepath = Path.Combine(projectDirectory, "Data", "Books.csv");
            books = new List<Book>();
            foreach (var line in File.ReadLines(booksfilepath))
            {
                var fields = line.Split(',');
                if (fields.Length >= 4)
                {
                    books.Add(new Book{Id = int.Parse(fields[0].Trim()), Title = fields[1].Trim(), Author = fields[2].Trim(),ISBN = fields[3].Trim()});
                }
            }
            users = new List<User>();
            foreach (var line in File.ReadLines(usersfilepath))
            {
                var fields = line.Split(',');
                if (fields.Length >= 3)
                {
                    users.Add(new User{Id = int.Parse(fields[0].Trim()), Name = fields[1].Trim(), Email = fields[2].Trim()});
                }
            }
            borrowedBooks = new Dictionary<User, List<Book>>();
        }


        [TestMethod]
        public void TestReadBooks()
        {
            // Act
            var bookCount = books.Count;
            // Assert
            Assert.IsTrue(bookCount > 0, "books should be loaded from the CSV file -_-");
            Assert.AreEqual("Book 1", books.First().Title, "first book title does not match the expected value -_-");
        }


        [TestMethod]
        public void TestReadUsers()
        {
            // Act
            var firstUser = users.FirstOrDefault();
            // Assert
            Assert.IsNotNull(firstUser, "the users list shouldnt be empty -_-");
            Assert.AreEqual("Mason", firstUser.Name, "first user name does not match the expected value -_-");
        }


        [TestMethod]
        public void TestBorrowBook()
        {
            // Arrange
            var user = users.First(); 
            var bookToBorrow = books.First(); 
            var initialBookCount = books.Count;
            // Act
            if (!borrowedBooks.ContainsKey(user))
            {
                borrowedBooks[user] = new List<Book>();
            }
            borrowedBooks[user].Add(bookToBorrow);
            books.Remove(bookToBorrow);
            // Assert
            Assert.AreEqual(initialBookCount - 1, books.Count, "book count should decrease after borrowing");
            Assert.AreEqual(1, borrowedBooks[user].Count, "user should have 1 borrowed book -_-");
            Assert.AreEqual(bookToBorrow.Title, borrowedBooks[user].First().Title, "the borrowed book title does not match -_-");
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestBorrowBookNoBooksAvailable()
        {
            // Arrange
            var user = users.First();
            var emptyBooks = new List<Book>();
            var borrowedBooks = new Dictionary<User, List<Book>>();
            // Act
            var bookToBorrow = emptyBooks.First();
            if (!borrowedBooks.ContainsKey(user))
            {
                borrowedBooks[user] = new List<Book>();
            }
            borrowedBooks[user].Add(bookToBorrow);
        }


        [TestMethod]
        public void TestReturnBook()
        {
            // Arrange
            var user = users.First();
            var bookToReturn = books.First();
            borrowedBooks[user] = new List<Book> { bookToReturn };
            books.Remove(bookToReturn); 
            var initialBookCount = books.Count;
            // Act
            borrowedBooks[user].Remove(bookToReturn);
            books.Add(bookToReturn);
            // Assert
            Assert.AreEqual(initialBookCount + 1, books.Count, "book count should increase after returning a book -_-");
            Assert.AreEqual(0, borrowedBooks[user].Count, "user should have no borrowed books after returning -_-");
        }
    }
}