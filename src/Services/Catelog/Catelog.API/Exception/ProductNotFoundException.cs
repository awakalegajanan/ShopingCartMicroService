using BuildingBlocks.Exceptions;
using System;

namespace Catelog.API.Exception
{
    public class ProductNotFoundException : NotFoundException
    {
        public ProductNotFoundException(Guid Id) : base("Product", Id)
        {
        }        
    }
}
