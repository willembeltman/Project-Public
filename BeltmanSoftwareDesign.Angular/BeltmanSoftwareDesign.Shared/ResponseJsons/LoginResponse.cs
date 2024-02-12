using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeltmanSoftwareDesign.Shared.Jsons;

namespace BeltmanSoftwareDesign.Shared.ResponseJsons
{
    public class LoginResponse : Response
    {
        public bool ErrorEmailNotValid { get; set; }
        public bool AuthenticationError { get; set; }
    }
}
