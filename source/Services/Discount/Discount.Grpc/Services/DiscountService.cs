using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services;

public class DiscountService(DiscountContext dbContext, ILogger<DiscountService> logger)
    : DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await dbContext.Coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName) ??
                     new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount" };
        var couponModel = coupon.Adapt<CouponModel>();
        logger.LogInformation("Discount retrieved. ProductName: {ProductName}, Amount: {Amount}",
            couponModel.ProductName, couponModel.Amount);
        return couponModel;
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        if (coupon is null)
        {
            logger.LogError("Coupon is null in CreateDiscount");
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Coupon cannot be null"));
        }

        dbContext.Coupons.Add(coupon);
        await dbContext.SaveChangesAsync();
        var couponModel = coupon.Adapt<CouponModel>();
        logger.LogInformation("Discount is successfully created. ProductName: {ProductName}, Amount: {Amount}",
            couponModel.ProductName, couponModel.Amount);
        return couponModel;
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        if (coupon is null)
        {
            logger.LogError("Coupon is null in CreateDiscount");
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Coupon cannot be null"));
        }

        dbContext.Coupons.Update(coupon);
        await dbContext.SaveChangesAsync();
        var couponModel = coupon.Adapt<CouponModel>();
        logger.LogInformation("Discount is successfully updated. ProductName: {ProductName}, Amount: {Amount}",
            couponModel.ProductName, couponModel.Amount);
        return couponModel;
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request,
        ServerCallContext context)
    {
        var coupon = dbContext.Coupons.FirstOrDefault(x => x.ProductName == request.ProductName);
        if (coupon is null)
        {
            logger.LogError("Coupon not found for deletion. ProductName: {ProductName}", request.ProductName);
            throw new RpcException(new Status(StatusCode.NotFound, "Coupon not found"));
        }

        dbContext.Coupons.Remove(coupon);
        await dbContext.SaveChangesAsync();
        logger.LogInformation("Discount is successfully deleted. ProductName: {ProductName}",
            request.ProductName);
        return new DeleteDiscountResponse { Success = true };
    }
}