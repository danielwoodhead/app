using System.Collections.Generic;
using System.Threading.Tasks;
using MyHealth.Web.App.Areas.DataSharing.Models;

namespace MyHealth.Web.App.Areas.DataSharing.Client
{
    public interface IDataSharingClient
    {
        Task DeleteDataSharingAgreementAsync(string name);
        Task<IEnumerable<DataSharingAgreement>> GetDataSharingAgreements();
    }
}
