using System.ComponentModel.DataAnnotations;

namespace TweyesBackend.Domain.QueryParameters
{
    public class LogQueryParameters : UrlQueryParameters
    {
        [DataType(DataType.Date)]
        public string? StartDate { get; set; }
        [DataType(DataType.Date)]
        public string? EndDate { get; set; }
    }
}
