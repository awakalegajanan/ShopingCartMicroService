

namespace Catelog.API.Products.CreateProduct
{
    public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
        :ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Product name is required.");
            RuleFor(x => x.Category).NotEmpty().WithMessage("At least one category is required.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Product description is required.");
            RuleFor(x => x.ImageFile).NotEmpty().WithMessage("Image file is required.");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");
        }
    }
    //internal class CreateProductCommandHandler(IDocumentSession session, IValidator<CreateProductCommand> validator)
    internal class CreateProductCommandHandler(IDocumentSession session, ILogger<CreateProductCommandHandler> logger)
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            //create product entity from command object

            //var resut = await validator.ValidateAsync(command, cancellationToken);
            //var errors = resut.Errors.Select(x => x.ErrorMessage).ToList();
            //if(errors.Any())
            //{
            //    throw new ValidationException(errors.FirstOrDefault());
            //}

            logger.LogInformation("Creating product with name: {Commond}", command);

            var product = new Product
            {                
                Name = command.Name,
                Category = command.Category,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price
            };
            //save product entity to database
            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);


            // return the result with the new product id
            return new CreateProductResult(product.Id);
        }
    }
}
