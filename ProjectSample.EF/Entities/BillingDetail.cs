﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectSample.EF.Entities
{
    public abstract class BillingDetail
    {
        public int BillingDetailId { get; set; }
        public string Owner { get; set; }
        public string Number { get; set; }
    }

    public class BankAccount : BillingDetail
    {
        public string BankName { get; set; }
        [Index(IsUnique = true)]
        public string Swift { get; set; }
    }

    public class CreditCard : BillingDetail
    {
        public int CardType { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }
    }
}
