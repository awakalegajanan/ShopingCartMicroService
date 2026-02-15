
namespace Catelog.API.Products.UpdateProduct
{
    public record UpdateProductRequest(Guid Id, string Name, string Description, decimal Price, List<string> Category, string ImageFile);
    
    public record UpdateProductResponse(bool IsSuccess);
    public class UpdateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/products", 
                async (UpdateProductRequest request, ISender sender) =>
                {
                    
                    var command = request.Adapt<UpdateProductCommand>();
                    var result = await sender.Send(command);
                    var response = result.Adapt<UpdateProductResponse>();
                    //if (response.IsSuccess)
                    //{
                        return Results.Ok(response);
                    //}
                    //else
                    //{
                    //    return Results.BadRequest("Failed to update the product. Please check the provided details and try again.");
                    //}
                })
                .WithName("UpdateProduct")
                .Produces<UpdateProductResponse>(StatusCodes.Status200OK)                
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithSummary("Updates an existing product")
                .WithDescription("Updates the details of an existing product with the specified information and returns whether the update was successful.");
        }
    }
}
