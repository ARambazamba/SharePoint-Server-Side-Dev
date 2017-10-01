using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using CustomWS.ISAPI;
using Microsoft.SharePoint.Client.Services;

namespace CustomWS
{
    [BasicHttpBindingServiceMetadataExchangeEndpoint]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ScriptService]
    public class DemoService : IDemoService
    {
        public string GetResponse()
        {
            return "Default Response";
        }

        public string GetResponseByValue(string Value)
        {
            return Value;
        }

        public string SetResponse(string Value)
        {
            return Value;
        }

        public ComplexPayload SetPayload(ComplexPayload Payload)
        {
            Payload.Name += " - Returned by WCF!";
            return Payload;
        }
    }
}
