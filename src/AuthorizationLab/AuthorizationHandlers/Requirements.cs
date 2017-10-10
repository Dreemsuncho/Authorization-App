using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationLab.AuthorizationHandlers
{
    public class OfficeEntryRequirement : IAuthorizationRequirement { }

    public class EditDocumentRequirement: IAuthorizationRequirement { }
     
    public class MinimumAgeRequirement : IAuthorizationRequirement
    {
        internal int _minAge;

        public MinimumAgeRequirement(int minAge)
        {
            _minAge = minAge;
        }
    }
}
