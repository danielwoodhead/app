using System.Collections.Generic;

namespace MyHealth.App.Api.Identity.Models
{
    public class PersistedGrantsResponse
    {
        public int TotalCount { get; set; }

        public int PageSize { get; set; }

        public IEnumerable<PersistedGrant> PersistedGrants { get; set; }
    }
}
