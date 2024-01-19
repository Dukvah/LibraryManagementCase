using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Objects;
using UnityEngine;

namespace Managers
{
    public class LibraryManager : Singleton<LibraryManager>
    {
        #region Variables

        private string _libraryBooksPath;
        private string _loanedBooksPath;

        private List<Book> _currentBooks = new();
        private List<LoanedBook> _loanedBooks = new();

        #endregion

        #region Unity Functions

        private void Awake()
        {
            _libraryBooksPath = Application.persistentDataPath + "/Kitaplar.json";
            _loanedBooksPath = Application.persistentDataPath + "/ÖdünçVerilenKitaplar.json";
            LoadBooks(RegistrationTypes.Library);
            LoadBooks(RegistrationTypes.LoanedBooks);
        
        }

        #endregion

        #region Main Modules

        public void AddBook(Book newBook)
        {
            foreach (var currentBook in _currentBooks)
            {
                if (currentBook.IsbnNo == newBook.IsbnNo) 
                {
                    currentBook.CopyCount += newBook.CopyCount;
                    EventManager.Instance.OnIncreaseBookCount.Invoke(currentBook.IsbnNo, currentBook.CopyCount);
                    SaveBooks(RegistrationTypes.Library);
                    return;
                }
                /* If the book to be added is already in the library,
             the number of copies of the incoming book is added to the number of copies in the library. */
            }
        
            _currentBooks.Add(newBook);
            EventManager.Instance.OnAddedNewBook.Invoke(newBook);
            SaveBooks(RegistrationTypes.Library);
        }
    
        public bool LentBook(LoanedBook newBook)
        {
            foreach (var curBook in _currentBooks)
            {
                if (curBook.IsbnNo == newBook.IsbnNo)
                {
                    if (curBook.CopyCount > 0)
                    {
                        curBook.CopyCount--;
                        curBook.LendedCount++;
                        AddNewLendedBook(newBook);
                        SaveBooks(RegistrationTypes.Library);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return false;
        }
        private void AddNewLendedBook(LoanedBook newBook)
        {
            _loanedBooks.Add(newBook);
            EventManager.Instance.OnAddedNewLendedBook.Invoke(newBook);
            SaveBooks(RegistrationTypes.LoanedBooks);
        }
        public bool ReturnBook(LoanedBook returnedBook)
        {
            foreach (var loanedBook in _loanedBooks)
            {
                var isMatch = returnedBook.BookName == loanedBook.BookName &&
                              returnedBook.AuthorName == loanedBook.AuthorName &&
                              returnedBook.IsbnNo == loanedBook.IsbnNo &&
                              returnedBook.BorrowerName == loanedBook.BorrowerName &&
                              returnedBook.BorrowerTel == loanedBook.BorrowerTel;
            
                if (isMatch && !loanedBook.IsDelivered)
                {
                    foreach (var libraryBook in _currentBooks)
                    {
                        if (libraryBook.IsbnNo == returnedBook.IsbnNo)
                        {
                            libraryBook.CopyCount++;
                            libraryBook.LendedCount--;
                        
                            loanedBook.DeliveryDate = DateTime.Now;
                            loanedBook.IsDelivered = true;
                        
                            SaveBooks(RegistrationTypes.Library);
                            SaveBooks(RegistrationTypes.LoanedBooks);
                        
                            EventManager.Instance.OnReturnedLendedBook.Invoke();
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        #endregion

        #region Getters

        public List<Book> GetAllBooks()
        {
            return _currentBooks;
        }

        public List<LoanedBook> GetAllLendedBooks()
        {
            return _loanedBooks;
        }

        #endregion
    
        #region Save & Load

        private void SaveBooks(RegistrationTypes registrationType)
        {
            switch (registrationType)
            {
                case RegistrationTypes.Library:
                    var libraryJsonData = JsonConvert.SerializeObject(_currentBooks, Formatting.Indented);
                    File.WriteAllText(_libraryBooksPath, libraryJsonData);
                    break;
                case RegistrationTypes.LoanedBooks:
                    var loanedBooksJsonData = JsonConvert.SerializeObject(_loanedBooks, Formatting.Indented);
                    File.WriteAllText(_loanedBooksPath, loanedBooksJsonData);
                    break;
            }
        
        }
    
        private void LoadBooks(RegistrationTypes registrationType)
        {
            switch (registrationType)
            {
                case RegistrationTypes.Library:
                    if (File.Exists(_libraryBooksPath))
                    {
                        var json = File.ReadAllText(_libraryBooksPath);
                        _currentBooks =  JsonConvert.DeserializeObject<List<Book>>(json);
                    }
                    else
                    {
                        // dosya yok
                    }
                    break;
                case RegistrationTypes.LoanedBooks:
                    if (File.Exists(_loanedBooksPath))
                    {
                        var json = File.ReadAllText(_loanedBooksPath);
                        _loanedBooks =  JsonConvert.DeserializeObject<List<LoanedBook>>(json);
                    }
                    else
                    {
                        // dosya yok
                    }
                    break;
            }
        }

        private enum RegistrationTypes
        {
            Library,
            LoanedBooks
        }
        #endregion
    }
}
