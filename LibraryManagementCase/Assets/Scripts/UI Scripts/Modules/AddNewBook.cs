using Managers;
using Objects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI_Scripts.Modules
{
    public class AddNewBook : MonoBehaviour
    {
        #region UI Elements

        [Header("Inputs")] 
        [SerializeField] private TMP_InputField bookNameInput; 
        [SerializeField] private TMP_InputField authorNameInput;
        [SerializeField] private TMP_InputField isbnNoInput;
        [SerializeField] private TMP_InputField bookCountInput;

        [Header("Buttons")] 
        [SerializeField] private Button addButton;
        [SerializeField] private Button clearButton;

        [Header("Text Areas")]
        [SerializeField] private TextMeshProUGUI warningText;
    
        #endregion

        #region Unity Functions

        private void Start()
        {
            ButtonInitialize();
        }

        #endregion

        #region Main Functions

        private void TryAddNewBook()
        {
            if (!CheckInputs()) return;
        
            var bookName = bookNameInput.text;
            var authorName = authorNameInput.text;
            var isbnNo = isbnNoInput.text;
            var bookCount = int.Parse(bookCountInput.text);
        
            var newBook = new Book()
            {
                BookName = bookName,
                AuthorName = authorName,
                IsbnNo = isbnNo,
                CopyCount = bookCount,
                LendedCount = 0
            };
        
            LibraryManager.Instance.AddBook(newBook);
            SendWarning("Kitap başarılı bir şekilde eklendi", true);
            ClearAllInputs();
        }

        #endregion

        #region UI Functions

    
        private void ButtonInitialize()
        {
            addButton.onClick.AddListener(TryAddNewBook);
            clearButton.onClick.AddListener(() => ClearAllInputs());
        }
    
        private bool CheckInputs()
        {
            if (bookNameInput.text == "")
            {
                SendWarning("Lütfen eklemek istediğiniz kitabın adını yazınız.", false);
                return false;
            }

            if (authorNameInput.text == "")
            {
                SendWarning("Lütfen eklemek istediğiniz kitabın yazarını giriniz.", false);
                return false;
            }
        
            if (isbnNoInput.text == "")
            {
                SendWarning("Lütfen eklemek istediğiniz kitabın ISBN numarasını giriniz.", false);
                return false;
            }
        
            if (bookCountInput.text == "")
            {
                SendWarning("Lütfen eklemek istediğiniz kitabın kopya sayısını giriniz.", false);
                return false;
            }
        
            if (int.TryParse(bookCountInput.text, out var countInput))
            {
                if (countInput > 0)
                {
                    return true;
                }
                else
                {
                    SendWarning("Eklenene kitap sayısı 0 dan büyük olmalıdır.", false);
                    return false;
                }
            }
            else
            {
                SendWarning("Lütfen kitap sayısı kısmına geçerli bir değer giriniz.", false);
                return false;
            }
        
        }

        private void SendWarning(string message, bool isSuccess)
        {
            warningText.color = isSuccess ? Color.green : Color.red;
            warningText.text = message;
        }
        private void ClearAllInputs(bool saveWarning = false)
        {
            bookNameInput.text = "";
            authorNameInput.text = "";
            isbnNoInput.text = "";
            bookCountInput.text = "";

            if (saveWarning) return;
            warningText.text = "";
        }

        #endregion
    
    }
}
