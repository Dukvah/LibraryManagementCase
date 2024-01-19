using System;
using Managers;
using Objects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI_Scripts.Modules
{
    public class LentBook : MonoBehaviour
    {
        #region UI Elements

        [Header("Inputs")] 
        [SerializeField] private TMP_InputField bookNameInput; 
        [SerializeField] private TMP_InputField authorNameInput;
        [SerializeField] private TMP_InputField isbnNoInput;
        [SerializeField] private TMP_InputField borrowerNameInput;
        [SerializeField] private TMP_InputField borrowerTelInput;

        [Header("Buttons")] 
        [SerializeField] private Button lendButton;
        [SerializeField] private Button clearButton;

        [Header("Text Areas")]
        [SerializeField] private TextMeshProUGUI warningText;
    
        #endregion

        #region Main Functions

        private void Start()
        {
            ButtonInitialize();
        }
    
        private void TryLentBook()
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
                LendingDate = DateTime.Now,
                DeliveryDate = DateTime.Now.AddDays(30),
                IsDelivered = false,
            };

            if (LibraryManager.Instance.LentBook(newBook))
            {
                SendWarning("Kitap ödünç verme işlemi başarılı. Ödünç aldığınız kitabı 30 gün içinde teslim etmeniz gerekmektedir ", true);
            }
            else
            {
                SendWarning("Aradığınız kitabın uygun kopyası bulunamamıştır. Lütfen uygun bir kitap deneyiniz.", false);
            }
            ClearAllInputs(true);
        }

        #endregion

        #region UI Functions
        
        private void ButtonInitialize()
        {
            lendButton.onClick.AddListener(TryLentBook);
            clearButton.onClick.AddListener(() => ClearAllInputs());
        }
    
        private bool CheckInputs()
        {
            if (bookNameInput.text == "")
            {
                SendWarning("Lütfen ödünç vermek istediğiniz kitabın adını yazınız.", false);
                return false;
            }

            if (authorNameInput.text == "")
            {
                SendWarning("Lütfen ödünç vermek istediğiniz kitabın yazarını giriniz.", false);
                return false;
            }
        
            if (isbnNoInput.text == "")
            {
                SendWarning("Lütfen ödünç vermek istediğiniz kitabın ISBN numarasını giriniz.", false);
                return false;
            }
            
            if (borrowerNameInput.text == "")
            {
                SendWarning("Lütfen ödünç alan kişinin adını giriniz.", false);
                return false;
            }
            
            if (borrowerTelInput.text == "")
            {
                SendWarning("Lütfen ödünç alan kişinin telefon numarasını giriniz.", false);
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
