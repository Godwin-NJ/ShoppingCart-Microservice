namespace Mango.Web.Utility
{
    public class StaticDetails
    {
        public static string CouponApiBaseUrl { get; set; }
        public static string AuthApiBaseUrl { get; set; }
        public static string ProductApiBaseUrl { get; set; }
        public static string ShoppingCartApiBaseUrl { get; set; }

        public const string RoleAdmin = "ADMIN";
        public const string RoleCustomer = "CUSTOMER";
        public const string TokenCookie = "JWTToken";
        public  enum ApiType 
        { 
          GET,
          POST,
          PUT,
          DELETE,       
        }
    }
}
