using Mango.Web.Models;
using Mango.Web.Service.IService;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Text;
using static Mango.Web.Utility.StaticDetails;
using System.Net;

namespace Mango.Web.Service
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenProvider _tokenProvider;
        public BaseService(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider)
        {
            _httpClientFactory = httpClientFactory;       
            _tokenProvider = tokenProvider;
        }
        public async Task<ResponseDto?> SendAsync(RequestDto requestDto, bool withBearer = true)
        {
            try
            {
           
            HttpClient client = _httpClientFactory.CreateClient("MangoApi");
            HttpRequestMessage msg = new();
            msg.Headers.Add("Accept", "application/json");

            if (withBearer) 
            {
                    var token = _tokenProvider.GetToken();
                    msg.Headers.Add("Authorization", $"Bearer {token}");
            }

            //token

            msg.RequestUri = new Uri(requestDto.Url);

            if (requestDto.Data != null) {
                msg.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
            }

            HttpResponseMessage apiResp = null;

            switch (requestDto.ApiType)
            {
                case ApiType.POST: 
                    msg.Method = HttpMethod.Post;
                    break;
                case ApiType.PUT:
                    msg.Method = HttpMethod.Put;
                    break;
                case ApiType.DELETE:
                    msg.Method = HttpMethod.Delete;
                    break;
                default:
                    msg.Method = HttpMethod.Get;
                    break;
            }
              
            apiResp = await client.SendAsync(msg);

            switch(apiResp.StatusCode)
            {
                case HttpStatusCode.NotFound:
                    return new() { IsSuccess = false, Message = "Not Found" };
                case HttpStatusCode.Forbidden:
                    return new() { IsSuccess = false, Message = "Access Forbidden" };
                case HttpStatusCode.Unauthorized:
                    return new() { IsSuccess = false, Message = "Unauthorized" };
                case HttpStatusCode.InternalServerError:
                    return new() { IsSuccess = false, Message = "Internal Server Error" };
                default:
                    var apiContent = await apiResp.Content.ReadAsStringAsync();
                    var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                    return apiResponseDto;
            }

            }
            catch (Exception ex)
            {
                var dto = new ResponseDto
                {
                    Message = ex.Message.ToString(),
                    IsSuccess = false
                };

               return dto;
            }

        }
    }
}
