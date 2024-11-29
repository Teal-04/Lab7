using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
namespace Lab6
{
    [TestClass]
    public class BookTests
    {
        private string booksfilepath = "";
        private List<Book> books;


        [TestInitialize]
        public void Setup()
        {
            // This part just pissed me off cause it would default to the bin/Debug/net8.0/Data, found this online its pretty neat
            var projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
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
        }


        [TestMethod]
        public void TestAddBook()
        {
            // Arrange
            var initialCount = books.Count;
            var newBook = new Book { Id = books.Max(b => b.Id) + 1, Title = "New Book", Author = "New Author", ISBN = "9999" };
            // Act
            books.Add(newBook);
            // Assert
            Assert.AreEqual(initialCount + 1, books.Count, "the books count should have increased by 1 -_-");
            Assert.AreEqual("New Book", books.Last().Title, "the last book added does not match the expected title -_-");
        }


        [TestMethod]
        public void TestEditBook()
        {
            // Arrange
            var bookToEdit = books.First();
            var newTitle = "Edited Title";
            var newAuthor = "Edited Author";
            // Act
            bookToEdit.Title = newTitle;
            bookToEdit.Author = newAuthor;
            // Assert
            Assert.AreEqual(newTitle, bookToEdit.Title, "the book title was not updated correctly -_-");
            Assert.AreEqual(newAuthor, bookToEdit.Author, "the book author was not updated correctly -_-");
        }


        [TestMethod]
        public void TestDeleteBook()
        {
            // Arrange
            var bookToDelete = books.First();
            var initialCount = books.Count;
            // Act
            books.Remove(bookToDelete);
            // Assert
            Assert.AreEqual(initialCount - 1, books.Count, "the books count did not decrease after deletion -_-");
            Assert.IsFalse(books.Any(b => b.Id == bookToDelete.Id), "the delete book still exists in the list -_-");
        }


        [TestMethod]
        public void TestListBooks()
        {
            // Arrange & Act
            var bookList = books.Select(b => $"{b.Id}: {b.Title} by {b.Author} (ISBN: {b.ISBN})").ToList();
            // Assert
            Assert.IsTrue(bookList.Count > 0, "the book list should not be empty -_-");
        }
    }
}
