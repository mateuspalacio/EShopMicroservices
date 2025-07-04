﻿namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductRequest(
    Guid Id,
    string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price);

public record UpdateProductResponse(
    bool IsSuccess);

public class UpdateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/products", async (UpdateProductRequest request, ISender sender) =>
            {
                var command = request.Adapt<UpdateProductCommand>();
                var result = await sender.Send(command);
                var response = new UpdateProductResponse(result.IsSuccess);
                return Results.Ok(response);
            }).WithName("UpdateProduct")
            .Produces<UpdateProductResponse>()
            .Produces(StatusCodes.Status400BadRequest)
            .WithSummary("Update Product")
            .WithDescription("Updates an existing product in the catalog.");
    }
}