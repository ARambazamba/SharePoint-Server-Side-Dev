using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using CustomWS.ISAPI;

namespace CustomWS
{
    [ServiceContract]
    public interface IDemoService
    {
        [OperationContract]
        [WebGet(UriTemplate = "GetResponse",
            ResponseFormat = WebMessageFormat.Json)]
        string GetResponse();

        [OperationContract(Name = "GetResponseByValue")]
        [WebGet(UriTemplate = "GetResponseByValue/{Value}",
            ResponseFormat = WebMessageFormat.Json)]
        string GetResponseByValue(string Value);

        [OperationContract(Name = "SetResponse")]
        [WebInvoke(UriTemplate = "/SetResponse?Param={Value}", Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        string SetResponse(string Value);

        [OperationContract(Name = "SetPayload")]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        ComplexPayload SetPayload(ComplexPayload Payload);

    }
}