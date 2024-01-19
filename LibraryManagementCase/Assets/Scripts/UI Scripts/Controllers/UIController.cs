using System.Collections.Generic;
using Managers;
using Objects;
using UI_Scripts.UI_List_Objects;
using UnityEngine;
using UnityEngine.UI;

namespace UI_Scripts.Controllers
{
    public class UIController : MonoBehaviour
    {
        #region UI Elements
    
        [Header("Module Buttons")]
        [SerializeField] private Button addNewBookButton;
        [SerializeField] private Button findTitleButton;
        [SerializeField] private Button findAuthorButton;
        [SerializeField] private Button lentBookButton;
        [SerializeField] private Button returnBookButton;
    
        [Header("List Buttons")]
        [SerializeField] private Button listAllLibraryBooksButton;
        [SerializeField] private Button listExpiredBookButton;
        [SerializeField] private Button listAllLendedBooksButton;

        [Header("Module Objects")] 
        [SerializeField] private GameObject addNewBookObject;
        [SerializeField] private GameObject findTitleObject;
        [SerializeField] private GameObject findAuthorObject;
        [SerializeField] private GameObject rentBookObject;
        [SerializeField] private GameObject returnBookObject;

        [Header("List Area")] 
        [SerializeField] private GameObject libraryScrollView;
        [SerializeField] private GameObject lendedBooksScrollView;
    
        [SerializeField] private LibraryBooksListObject libraryBooksListPrefab;
        [SerializeField] private LendedBooksListObject lendedBooksListPrefab;
    
        [SerializeField] private Transform libraryBooksListParent;
        [SerializeField] private Transform lendedBooksListParent;

        private List<LibraryBooksListObject> _libraryBooksListObjects = new();
        private List<LendedBooksListObject> _lendedBooksListObjects = new();
    
        private bool _allBooksFiltered;
        private bool _allLendedBooksFiltered;

        #endregion

        #region Unity Functions

        private void Start()
        {
            CloseAllModulePages();
            ButtonInitialize();
        
            libraryScrollView.SetActive(false);
            lendedBooksScrollView.SetActive(false);
        }

        private void OnEnable()
        {
            EventManager.Instance.OnAddedNewBook.AddListener(AddLibraryBookObjectToList);
            EventManager.Instance.OnAddedNewLendedBook.AddListener(AddLendedBookObjectToList);
            EventManager.Instance.OnReturnedLendedBook.AddListener(ReturnLendedBookToList);
            EventManager.Instance.OnSearchBookName.AddListener(FilterBookListByName);
            EventManager.Instance.OnSearchAuthorName.AddListener(FilterBookListByAuthor);
            EventManager.Instance.OnIncreaseBookCount.AddListener(IncreaseCountOfBooks);

        }

        private void OnDisable()
        {
            EventManager.Instance?.OnAddedNewBook.RemoveListener(AddLibraryBookObjectToList);
            EventManager.Instance?.OnAddedNewLendedBook.RemoveListener(AddLendedBookObjectToList);
            EventManager.Instance?.OnReturnedLendedBook.RemoveListener(ReturnLendedBookToList);
            EventManager.Instance?.OnSearchBookName.RemoveListener(FilterBookListByName);
            EventManager.Instance?.OnSearchAuthorName.RemoveListener(FilterBookListByAuthor);
            EventManager.Instance?.OnIncreaseBookCount.RemoveListener(IncreaseCountOfBooks);

        }

        #endregion

        #region List Functions

        private void ListAllLibraryBooks()
        {
            if (_allBooksFiltered)
            {
                foreach (var bookListObject in _libraryBooksListObjects)
                    bookListObject.gameObject.SetActive(true);
            
                _allBooksFiltered = false;
            }
            else
            {
                ClearLibraryBookList();
        
                var books = LibraryManager.Instance.GetAllBooks();

                for (int i = 0; i < books.Count; i++)
                {
                    var obj = Instantiate(libraryBooksListPrefab, libraryBooksListParent);
                    obj.SetListObject((i+1).ToString(), books[i].BookName, books[i].AuthorName, 
                        books[i].IsbnNo, books[i].CopyCount.ToString(), books[i].LendedCount.ToString());
                    _libraryBooksListObjects.Add(obj);
                }
            }
        
            ChangeList(BookListTypes.Library);
        }

        private void ListAllLendedBooks()
        {
            ClearLendedBookList();
            
            var lendedBooks = LibraryManager.Instance.GetAllLendedBooks();

            for (int i = 0; i < lendedBooks.Count; i++)
            {
                var obj = Instantiate(lendedBooksListPrefab, lendedBooksListParent);
                obj.SetListObject(i + 1.ToString(), lendedBooks[i].BookName, lendedBooks[i].AuthorName, lendedBooks[i].IsbnNo, 
                    lendedBooks[i].BorrowerName, lendedBooks[i].BorrowerTel, lendedBooks[i].LendingDate, lendedBooks[i].DeliveryDate, lendedBooks[i].IsDelivered);
                _lendedBooksListObjects.Add(obj);
            }
        
            if (_allLendedBooksFiltered)
            {
                foreach (var lendedBooksListObject in _lendedBooksListObjects)
                    lendedBooksListObject.gameObject.SetActive(true);

                _allLendedBooksFiltered = false;
            }
        
            ChangeList(BookListTypes.Lended);
        }

        private void AddLibraryBookObjectToList(Book book)
        {
            var obj = Instantiate(libraryBooksListPrefab, libraryBooksListParent);
            obj.SetListObject(_libraryBooksListObjects.Count+1.ToString(), book.BookName, book.AuthorName, 
                book.IsbnNo, book.CopyCount.ToString(), book.LendedCount.ToString());
            _libraryBooksListObjects.Add(obj);
        }

        private void AddLendedBookObjectToList(LoanedBook loanedBook)
        {
            var obj = Instantiate(lendedBooksListPrefab, lendedBooksListParent);
            obj.SetListObject(_lendedBooksListObjects.Count + 1.ToString(), loanedBook.BookName, loanedBook.AuthorName, loanedBook.IsbnNo, 
                loanedBook.BorrowerName, loanedBook.BorrowerTel, loanedBook.LendingDate, loanedBook.DeliveryDate, loanedBook.IsDelivered);
            _lendedBooksListObjects.Add(obj);
        
            ListAllLibraryBooks();
        }
    
        private void ReturnLendedBookToList()
        {
            ListAllLendedBooks();
        }
        private void IncreaseCountOfBooks(string isbnNo, int increaseCount)
        {
            foreach (var bookListObject in _libraryBooksListObjects)
            {
                if (bookListObject.IsbnNo == isbnNo)
                {
                    bookListObject.CopyCount += increaseCount;
                }
            }
        }
    
        private void ClearLibraryBookList()
        {
            /* Too many objects in the list may cause performance problems. This can be fixed with the help of a pool system.*/
        
            foreach (var bookListObject in _libraryBooksListObjects)
                Destroy(bookListObject.gameObject);
        
            _libraryBooksListObjects.Clear();
        }

        private void ClearLendedBookList()
        {
            foreach (var lendedBooksListObject in _lendedBooksListObjects)
                Destroy(lendedBooksListObject.gameObject);
        
            _lendedBooksListObjects.Clear();
        }

        #endregion

        #region Filter Functions

        private void FilterBookListByName(string filterName)
        {
            ChangeList(BookListTypes.Library);

            foreach (var bookListObject in _libraryBooksListObjects)
            {
                bookListObject.gameObject.SetActive(bookListObject.BookName.ToLower().Contains(filterName.ToLower()));
            }

            _allBooksFiltered = true;
        }
    
        private void FilterBookListByAuthor(string filterAuthor)
        {
            ChangeList(BookListTypes.Library);
        
            foreach (var bookListObject in _libraryBooksListObjects)
            {
                bookListObject.gameObject.SetActive(bookListObject.AuthorName.ToLower().Contains(filterAuthor.ToLower()));
            }

            _allBooksFiltered = true;
        }
    
        private void FilterExpiredBooks()
        {
            ChangeList(BookListTypes.Lended);
        
            foreach (var lendedBooksListObject in _lendedBooksListObjects)
            {
                lendedBooksListObject.gameObject.SetActive(lendedBooksListObject.IsExpired);
            }
        
            _allLendedBooksFiltered = true;
        }
    
        #endregion

        #region UI Functions

        private void ButtonInitialize()
        {
            addNewBookButton.onClick.AddListener(() => OpenModulePage(addNewBookObject));
            findTitleButton.onClick.AddListener(() => OpenModulePage(findTitleObject));
            findAuthorButton.onClick.AddListener(() => OpenModulePage(findAuthorObject));
            lentBookButton.onClick.AddListener(() => OpenModulePage(rentBookObject));
            returnBookButton.onClick.AddListener(() => OpenModulePage(returnBookObject));
        
            listAllLibraryBooksButton.onClick.AddListener(ListAllLibraryBooks);
            listExpiredBookButton.onClick.AddListener(FilterExpiredBooks);
            listAllLendedBooksButton.onClick.AddListener(ListAllLendedBooks);
        }

        private void OpenModulePage(GameObject page)
        {
            CloseAllModulePages();
            page.SetActive(true);
        }
        private void CloseAllModulePages()
        {
            addNewBookObject.SetActive(false);
            findTitleObject.SetActive(false);
            findAuthorObject.SetActive(false);
            rentBookObject.SetActive(false);
            returnBookObject.SetActive(false);
        }

        private void ChangeList(BookListTypes targetList)
        {
            switch (targetList)
            {
                case BookListTypes.Library:
                    lendedBooksScrollView.SetActive(false);
                    libraryScrollView.SetActive(true);
                    break;
                case BookListTypes.Lended:
                    libraryScrollView.SetActive(false);
                    lendedBooksScrollView.SetActive(true);
                    break;
            }
        }
        #endregion

        #region Enums

        private enum BookListTypes
        {
            Library,
            Lended
        }

        #endregion
    }
}
