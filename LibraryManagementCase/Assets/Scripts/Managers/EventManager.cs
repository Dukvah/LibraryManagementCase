using Objects;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public class EventManager : Singleton<EventManager>
    {
        [HideInInspector] public UnityEvent<Book> OnAddedNewBook = new();
        [HideInInspector] public UnityEvent<LoanedBook> OnAddedNewLendedBook = new();
        [HideInInspector] public UnityEvent OnReturnedLendedBook = new();
        [HideInInspector] public UnityEvent<string> OnSearchBookName = new();
        [HideInInspector] public UnityEvent<string> OnSearchAuthorName = new();
        [HideInInspector] public UnityEvent<string, int> OnIncreaseBookCount = new();

    }
}
