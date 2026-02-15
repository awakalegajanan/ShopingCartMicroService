using System;

namespace Catelog.API.Exception
{
    public class ProductNotFoundException : System.Exception
    {
        public ProductNotFoundException() : base("Product not found")
        {
        }
        public ProductNotFoundException(string message) : base(message)
        {
        }
        public ProductNotFoundException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
