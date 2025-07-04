﻿namespace Basket.API.Basket.GetBasket;

public record GetBasketResponse(ShoppingCart Cart);

public class GetBasketEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/basket/{userName}", async (string userName, ISender sender) =>
            {
                var result = await sender.Send(new GetBasketQuery(userName));

                var response = result.Adapt<GetBasketResponse>();

                return Results.Ok(response);
            }).WithName("GetBasket")
            .Produces<GetBasketResponse>()
            .Produces(StatusCodes.Status400BadRequest)
            .WithSummary("Get Basket By Username")
            .WithDescription("Get Basket By Username");
    }
}