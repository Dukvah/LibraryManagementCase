using System;
using TMPro;
using UnityEngine;

namespace UI_Scripts.UI_List_Objects
{
    public class LendedBooksListObject : MonoBehaviour
    {
        #region Variables

        [SerializeField] private TextMeshProUGUI noText;
        [SerializeField] private TextMeshProUGUI bookNameText;
        [SerializeField] private TextMeshProUGUI authorNameText;
        [SerializeField] private TextMeshProUGUI isbnNoText;
        [SerializeField] private TextMeshProUGUI borrowerNameText;
        [SerializeField] private TextMeshProUGUI borrowerTelText;
        [SerializeField] private TextMeshProUGUI lendingDateText;
        [SerializeField] private TextMeshProUGUI deliveryDateText;
        [SerializeField] private TextMeshProUGUI deliveryStatusText;
    

        public string BookName { get; private set; }
        public string AuthorName { get; private set; }
        public string IsbnNo { get; private set; }
        public string BorrowerName { get; private set; }
        public string BorrowerTel { get; private set; }
        public bool IsExpired { get; private set; }

        #endregion

        #region Functions

        public void SetListObject(string no, string bookName, string authorName, string isbnNo, string borrowerName, string borrowerTelNo, DateTime lendingDate, DateTime deliveryDate, bool deliveryStatus)
        {
            noText.text = no;
            bookNameText.text = bookName;
            authorNameText.text = authorName;
            isbnNoText.text = isbnNo;
            borrowerNameText.text = borrowerName;
            borrowerTelText.text = borrowerTelNo;
            lendingDateText.text = lendingDate.ToShortDateString();
            deliveryDateText.text = deliveryDate.ToShortDateString();

            var elapsedTime = DateTime.Now - lendingDate;
            var elapsedDays = (int)elapsedTime.TotalDays;
        
            if (deliveryStatus)
            {
                deliveryStatusText.text = "Teslim edildi";
                deliveryStatusText.color = Color.green;
                IsExpired = false;
            }
            else if (elapsedDays > 30)
            {
                deliveryStatusText.text = "Teslim süresi geçti";
                deliveryStatusText.color = Color.red;
                IsExpired = true;
            }
            else
            {
                deliveryStatusText.text = "Teslim bekleniyor";
                deliveryStatusText.color = Color.yellow;
                IsExpired = false;
            }
        
        
            BookName = bookName;
            AuthorName = authorName;
            IsbnNo = isbnNo;
            BorrowerName = borrowerName;
            BorrowerTel = borrowerTelNo;
        }

        #endregion
    }
}
