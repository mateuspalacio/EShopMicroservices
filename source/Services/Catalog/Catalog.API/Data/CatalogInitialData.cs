using Marten.Schema;

namespace Catalog.API.Data;

public class CatalogInitialData : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        using var session = store.LightweightSession();

        // Add initial data if needed
        if (!await session.Query<Product>().AnyAsync(cancellation))
        {
            var products = new List<Product>
            {
                new()
                {
                    Id = Guid.NewGuid(), Name = "Product 1", ImageFile = "Imaghe", Category = new List<string> { "c1" },
                    Price = 10.99m
                },
                new()
                {
                    Id = Guid.NewGuid(), Name = "Product 2", ImageFile = "Imaghe 2",
                    Category = new List<string> { "c2" }, Price = 20.99m
                },
                new()
                {
                    Id = Guid.NewGuid(), Name = "Product 3", ImageFile = "Imaghe 3",
                    Category = new List<string> { "c1", "c2" }, Price = 15.49m
                }
            };

            session.Store<Product>(products);
            await session.SaveChangesAsync(cancellation);
        }
    }
}