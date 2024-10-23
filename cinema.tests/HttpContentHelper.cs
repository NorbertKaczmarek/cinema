using Newtonsoft.Json;
using System.Text;

namespace cinema.tests;

public class HttpContentHelper
{
    public static HttpContent ToJsonHttpContent(object obj)
    {
        return new StringContent(
            JsonConvert.SerializeObject(obj), 
            Encoding.UTF8, 
            "application/json");
    }
}
