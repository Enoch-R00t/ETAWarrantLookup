using ETAWarrantLookup.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETAWarrantLookup.ViewModels
{
    public class PaymentOptionsViewModel
    {
        public IEnumerable<PaymentOptions> paymentOptions { get; set; }

        public Guid referenceToken { get; set; }
        public string redirectUrl { get; set; }

       public string paymentUrl { get; set; }

    }
}
