using Marten.Schema;

namespace Catelog.API.Data
{
    public class CatelogInitialData : IInitialData
    {
        public async Task Populate(IDocumentStore store, CancellationToken cancellation)
        {
            using var session = store.LightweightSession();
            if(await session.Query<Product>().AnyAsync(cancellation))
            {
                return;
            }

            // Marten UPSERT will cater for existing records
            session.Store<Product>(GetPreConfiguredProducts());
            await session.SaveChangesAsync(cancellation);
        }

        private static IEnumerable<Product> GetPreConfiguredProducts()
        {
            var lst = new List<Product>
            {
                new Product
                {
                    Id = new Guid("019c60c6-7990-4d50-a14b-58e3a941be01"),
                    Name = "IPhone 15",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec suscipit auctor dui, sed efficitur ipsum.",
                    ImageFile = "product-1.png",
                    Price = 950.00M,
                    Category = new List<string> { "Smart Phone" }
                },
                new Product
                {
                    Id = new Guid("019c60c6-7990-4d50-a14b-58e3a941be02"),
                    Name = "IPhone 16",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec suscipit auctor dui, sed efficitur ipsum.",
                    ImageFile = "product-2.png",
                    Price = 960.00M,
                    Category = new List<string> { "Smart Phone" }
                },
                new Product
                {
                    Id = new Guid("019c60c6-7990-4d50-a14b-58e3a941be03"),
                    Name = "Samsung 10",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec suscipit auctor dui, sed efficitur ipsum.",
                    ImageFile = "product-2.png",
                    Price = 840.00M,
                    Category = new List<string> { "Smart Phone" }
                },
                new Product
                {
                    Id = new Guid("019c60c6-7990-4d50-a14b-58e3a941be04"),
                    Name = "Huawei Plus",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec suscipit auctor dui, sed efficitur ipsum.",
                    ImageFile = "product-3.png",
                    Price = 650.00M,
                    Category = new List<string> { "White Appliances" }
                },
            };

            return lst;
        }
    }
}
