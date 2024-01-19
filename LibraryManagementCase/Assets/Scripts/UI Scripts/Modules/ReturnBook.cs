using Managers;
using Objects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI_Scripts.Modules
{
    public class ReturnBook : MonoBehaviour
    {
        #region UI Elements

        [Header("Inputs")] 
        [SerializeField] private TMP_InputField bookNameInput; 
        [SerializeField] private TMP_InputField authorNameInput;
        [SerializeField] private TMP_InputField isbnNoInput;
        [SerializeField] private TMP_InputField borrowerNameInput;
        [SerializeField] private TMP_InputField borrowerTelInput;

        [Header("Buttons")] 
        [SerializeField] private Button returnButton;
        [SerializeField] private Button clearButton;

        [Header("Text Areas")]
        [SerializeField] private TextMeshProUGUI warningText;
    
        #endregion

        #region Main Functions

        private void Start()
        {
            ButtonInitialize();
        }
    
        private void TryReturnBook()
        {
            if (!CheckInputs()) return;
        
            var bookName = bookNameInput.text;
            var authorName = authorNameInput.text;
            var isbnNo = isbnNoInput.text;
            var borrowerName = borrowerNameInput.text;
            var borrowerTel = borrowerTelInput.text;
        
            var newBook = new LoanedBook()
            {
                BookName = bookName,
                AuthorName = authorName,
                IsbnNo = isbnNo,
                BorrowerName = borrowerName,
                BorrowerTel = borrowerTel,
            };

            if (LibraryManager.Instance.ReturnBook(newBook))
            {
                SendWarning("Kitap başarıyla teslim alındı", true);
            }
            else
            {
                SendWarning("Sistemimizde aradığınız kriterlerde bir kitap bulunamamıştır. Lütfen girdiğiniz bilgileri kontrol ediniz.", false);
            }
            ClearAllInputs(true);
        }

        #endregion
        
        #region UI Functions
        
        private void ButtonInitialize()
        {
            returnButton.onClick.AddListener(TryReturnBook);
            clearButton.onClick.AddListener(() => ClearAllInputs());
        }
    
        private bool CheckInputs()
        {
            if (bookNameInput.text == "")
            {
                SendWarning("Lütfen teslim etmek istediğiniz kitabın adını yazınız.", false);
                return false;
            }

            if (authorNameInput.text == "")
            {
                SendWarning("Lütfen teslim etmek istediğiniz kitabın yazarını giriniz.", false);
                return false;
            }
        
            if (isbnNoInput.text == "")
            {
                SendWarning("Lütfen teslim etmek istediğiniz kitabın ISBN numarasını giriniz.", false);
                return false;
            }
            
            if (borrowerNameInput.text == "")
            {
                SendWarning("Lütfen teslim eden kişinin adını giriniz.", false);
                return false;
            }
            
            if (borrowerTelInput.text == "")
            {
                SendWarning("Lütfen teslim eden kişinin telefon numarasını giriniz.", false);
                return false;
            }
            
            return true;
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
            borrowerNameInput.text = "";
            borrowerTelInput.text = "";

            if (saveWarning) return;
            warningText.text = "";
        }

        #endregion
    }
}
