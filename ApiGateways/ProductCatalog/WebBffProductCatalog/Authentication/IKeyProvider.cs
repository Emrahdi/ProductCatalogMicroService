using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUIGateWayApi.Authentication {
    public interface IKeyProvider {
        string GetSecretKey();
    }
}
