using System;

namespace Objects
{
    public class LoanedBook
    {
        public string BookName;
        public string AuthorName;
        public string IsbnNo;
        public string BorrowerName;
        public string BorrowerTel;
        public DateTime LendingDate;
        public DateTime DeliveryDate;
        public bool IsDelivered = false;
    }
}
