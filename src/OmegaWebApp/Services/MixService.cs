using Omega.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmegaWebApp.Services
{
    public class MixService
    {
        MixTableGateway _mixTableGateway;
        public MixService(MixTableGateway mixTableGateway)
        {
            _mixTableGateway = mixTableGateway;
        }

        public async Task<Mix> RetrieveMix(string name, string guid)
        {
            return await _mixTableGateway.RetrieveMix(name, guid);
        }

        public async Task<List<Mix>> RetrieveAllMixUser(string guid)
        {
            return await _mixTableGateway.RetrieveAllMixUser(guid);
        }

        public async Task InsertMix(Mix mix, string guid)
        {
            await _mixTableGateway.InsertMix(mix, guid);
        }

        public async Task DeleteMix(string name, string guid)
        {
            await _mixTableGateway.DeleteMix(name, guid);
        }
    }
}
