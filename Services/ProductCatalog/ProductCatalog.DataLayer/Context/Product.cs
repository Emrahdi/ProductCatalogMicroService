using System;

namespace ProductCatalog.DataLayer.Context
{
    public partial class Product
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public byte[] Photo { get; set; }
        public decimal Price { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string LastUpdatedUser { get; set; }
    }
}
