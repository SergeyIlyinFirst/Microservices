using Discount.Grpc.Protos;

namespace Basket.API.GrpcServices
{
    public class DiscountGrpcService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoService;

        public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoService)
        {
            _discountProtoService = discountProtoService;
        }

        public async Task<CouponModel> GetDiscountAsync(string productName)
        {
            GetDiscountRequest discountRequest = new() { ProductName = productName };

            return await _discountProtoService.GetDiscountAsync(discountRequest, new Grpc.Core.CallOptions());
        } 
    }
}
