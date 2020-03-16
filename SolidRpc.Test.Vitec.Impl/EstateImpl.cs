using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SolidRpc.Test.Vitec.Services;
using SolidRpc.Test.Vitec.Types.Common.Estate;
using SolidRpc.Test.Vitec.Types.Criteria.Estate;
using SolidRpc.Test.Vitec.Types.Estate.Models;
using SolidRpc.Test.Vitec.Types.List.Estate;

namespace Microsoft.Extensions.DependencyInjection
{
    public class EstateImpl : IEstate
    {
        public Task<CommercialProperty> EstateGetCommercialProperty(string estateId, string customerId = null, bool? onlyFutureViewings = false, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<Condominium> EstateGetCondominium(string estateId, string customerId = null, bool? onlyFutureViewings = false, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<Cottage> EstateGetCottage(string estateId, string customerId = null, bool? onlyFutureViewings = false, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<EstateList>> EstateGetEstateList(EstateCriteria criteria, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<Farm> EstateGetFarm(string estateId, string customerId = null, bool? onlyFutureViewings = false, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<ForeignProperty> EstateGetForeignProperty(string estateId, string customerId = null, bool? onlyFutureViewings = false, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<House> EstateGetHouse(string estateId, string customerId = null, bool? onlyFutureViewings = false, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<HousingCooperative> EstateGetHousingCooperative(string estateId, string customerId = null, bool? onlyFutureViewings = false, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<Plot> EstateGetPlot(string estateId, string customerId = null, bool? onlyFutureViewings = false, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<Premises> EstateGetPremises(string estateId, string customerId = null, bool? onlyFutureViewings = false, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<Project> EstateGetProject(string projectId, string customerId = null, bool? onlyFutureViewings = false, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Status>> EstateGetStatuses(string customerId = null, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }
    }
}