using RestEase;
using System.Threading.Tasks;
using System.Net.Http;

namespace SerialNumberCheckup
{
    public interface ISnipeItApi
    {
        [Header("Authorization")]
        string Authorization { get; set; }

        [Get("/hardware/byserial/{serialNumber}")]
        public Task<Response<HttpResponseMessage>> GetHardwareBySerialnumber([Path("serialNumber")] string serialNumber);
    }
}
