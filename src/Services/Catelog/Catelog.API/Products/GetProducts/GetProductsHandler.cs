


namespace Catelog.API.Products.GetProducts
{
    public record GetProductsQuery(int? PageNumber, int? PageSize=10):IQuery<GetProductsResult>;
    public record GetProductsResult(IEnumerable<Product> Products);

    //internal class GetProductsQueryHandler(IDocumentSession session, ILogger<GetProductsQueryHandler> logger)
    internal class GetProductsQueryHandler(IDocumentSession session)
        : IQueryHandler<GetProductsQuery, GetProductsResult>
    {
        public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            //logger.LogInformation("GetProductsQueryHandler.handle called with {@Query}", query);
            var products = await session.Query<Product>().ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10, cancellationToken);
            return new GetProductsResult(products);
        }
    }
}
