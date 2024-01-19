using TMPro;
using UnityEngine;

namespace UI_Scripts.UI_List_Objects
{
    public class LibraryBooksListObject : MonoBehaviour
    {
        #region Variables

        [SerializeField] private TextMeshProUGUI noText;
        [SerializeField] private TextMeshProUGUI bookNameText;
        [SerializeField] private TextMeshProUGUI authorNameText;
        [SerializeField] private TextMeshProUGUI isbnNoText;
        [SerializeField] private TextMeshProUGUI copyCountText;
        [SerializeField] private TextMeshProUGUI lendedCountText;

        public string BookName { get; private set; }
        public string AuthorName { get; private set; }
        public string IsbnNo { get; private set; }
        private int _copyCount;
        public int CopyCount
        {
            get => _copyCount;
            set
            {
                _copyCount = value;
                copyCountText.text = value.ToString();
            }
        }

        #endregion

        #region Functions

        public void SetListObject(string no, string bookName, string authorName, string isbnNo, string copyCount, string lendedCount)
        {
            noText.text = no;
            bookNameText.text = bookName;
            authorNameText.text = authorName;
            isbnNoText.text = isbnNo;
            copyCountText.text = copyCount;
            lendedCountText.text = lendedCount;

            BookName = bookName;
            AuthorName = authorName;
            IsbnNo = isbnNo;
        }

        #endregion
    }
}
