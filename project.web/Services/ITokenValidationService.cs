using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.web.Services
{
    public interface ITokenValidationService
    {
        Task Validate(string sourceClass, string sourceMethod);
    }
}
