
namespace Catelog.API.Products.DeleteProduct
{
    //public record DeleteProductRequest(Guid Id);
    public record DeleteProductResponse(bool IsSuccess);
    public class DeleteProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/products/{id}",
                async (Guid id, ISender sender) =>
                {
                    var command = new DeleteProductCommand(id);
                    var result = await sender.Send(command);
                    var response = result.Adapt<DeleteProductResponse>();
                    //if (response.Success)
                    //{
                    return Results.Ok(response);
                    //}
                    //else
                    //{
                    //    return Results.BadRequest("Failed to delete the product. Please check the provided ID and try again.");
                    //}
                })
                .WithName("DeleteProduct")
                .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithSummary("Deletes a product")
                .WithDescription("Deletes a product with the specified ID and returns whether the deletion was successful.");
        }
    }
}
