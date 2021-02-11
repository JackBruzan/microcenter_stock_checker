using System;
using System.Collections.Generic;
using System.Text;

namespace MicrocenterStockChecker
{
    public record URLInfo
    {
        public string Products { get; init; }
        public string Condition { get; init; }
        public string URL { get; init; }

        public URLInfo(string Products, string Condition, string URL)
        {
            this.Products = Products;
            this.Condition = Condition;
            this.URL = URL;
        }
    }
}
