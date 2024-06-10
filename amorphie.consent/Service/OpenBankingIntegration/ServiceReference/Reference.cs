﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------



[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.ServiceModel.ServiceContractAttribute(ConfigurationName="IOpenBankingIntegration")]
public interface IOpenBankingIntegration
{
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOpenBankingIntegration/VerificationUser", ReplyAction="http://tempuri.org/IOpenBankingIntegration/VerificationUserResponse")]
    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
    [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SecureData))]
    System.Threading.Tasks.Task<VerificationUserResponse> VerificationUserAsync(VerificationUserRequest request);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOpenBankingIntegration/AccountList", ReplyAction="http://tempuri.org/IOpenBankingIntegration/AccountListResponse")]
    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
    [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SecureData))]
    System.Threading.Tasks.Task<AccountListResponse> AccountListAsync(AccountListRequest request);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOpenBankingIntegration/PaymentCheck", ReplyAction="http://tempuri.org/IOpenBankingIntegration/PaymentCheckResponse")]
    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
    [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SecureData))]
    System.Threading.Tasks.Task<PaymentCheckResponse> PaymentCheckAsync(PaymentCheckRequest request);
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://tempuri.org/VeriBranchMessages.xsd")]
public partial class VeriBranchMessageHeader
{
    
    private bool isSuccessField;
    
    private string descriptionField;
    
    private string displayMessageField;
    
    private string errorCodeField;
    
    private string exceptionMessageField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Order=0)]
    public bool IsSuccess
    {
        get
        {
            return this.isSuccessField;
        }
        set
        {
            this.isSuccessField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Order=1)]
    public string Description
    {
        get
        {
            return this.descriptionField;
        }
        set
        {
            this.descriptionField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Order=2)]
    public string DisplayMessage
    {
        get
        {
            return this.displayMessageField;
        }
        set
        {
            this.displayMessageField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Order=3)]
    public string ErrorCode
    {
        get
        {
            return this.errorCodeField;
        }
        set
        {
            this.errorCodeField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Order=4)]
    public string ExceptionMessage
    {
        get
        {
            return this.exceptionMessageField;
        }
        set
        {
            this.exceptionMessageField = value;
        }
    }
}

/// <remarks/>
[System.Xml.Serialization.XmlIncludeAttribute(typeof(ResponseTransactionData))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(OpenBankingGenericResponse))]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.datacontract.org/2004/07/VeriBranch.Common.MessageDefinitions")]
public partial class SecureData
{
}

/// <remarks/>
[System.Xml.Serialization.XmlIncludeAttribute(typeof(OpenBankingGenericResponse))]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.datacontract.org/2004/07/VeriBranch.Common.MessageDefinitions")]
public partial class ResponseTransactionData : SecureData
{
    
    private string fraudStatusFieldField;
    
    private string responseRefNoFieldField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=0)]
    public string fraudStatusField
    {
        get
        {
            return this.fraudStatusFieldField;
        }
        set
        {
            this.fraudStatusFieldField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=1)]
    public string responseRefNoField
    {
        get
        {
            return this.responseRefNoFieldField;
        }
        set
        {
            this.responseRefNoFieldField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.datacontract.org/2004/07/VeriBranch.Common.MessageDefinitions")]
public partial class OpenBankingGenericResponse : ResponseTransactionData
{
    
    private string displayMessageFieldField;
    
    private string errorCodeFieldField;
    
    private bool isSuccessFieldField;
    
    private string jSONResultFieldField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=0)]
    public string displayMessageField
    {
        get
        {
            return this.displayMessageFieldField;
        }
        set
        {
            this.displayMessageFieldField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=1)]
    public string errorCodeField
    {
        get
        {
            return this.errorCodeFieldField;
        }
        set
        {
            this.errorCodeFieldField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Order=2)]
    public bool isSuccessField
    {
        get
        {
            return this.isSuccessFieldField;
        }
        set
        {
            this.isSuccessFieldField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=3)]
    public string jSONResultField
    {
        get
        {
            return this.jSONResultFieldField;
        }
        set
        {
            this.jSONResultFieldField = value;
        }
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(WrapperName="VerificationUser", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
public partial class VerificationUserRequest
{
    
    [System.ServiceModel.MessageHeaderAttribute(Namespace="http://tempuri.org/VeriBranchMessages.xsd")]
    public VeriBranchMessageHeader VeriBranchMessageHeader;
    
    [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string username;
    
    [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string password;
    
    [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=2)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string customerID;
    
    [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=3)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string corporateUserCustomerID;
    
    [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=4)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string jsonData;
    
    public VerificationUserRequest()
    {
    }
    
    public VerificationUserRequest(VeriBranchMessageHeader VeriBranchMessageHeader, string username, string password, string customerID, string corporateUserCustomerID, string jsonData)
    {
        this.VeriBranchMessageHeader = VeriBranchMessageHeader;
        this.username = username;
        this.password = password;
        this.customerID = customerID;
        this.corporateUserCustomerID = corporateUserCustomerID;
        this.jsonData = jsonData;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(WrapperName="VerificationUserResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
public partial class VerificationUserResponse
{
    
    [System.ServiceModel.MessageHeaderAttribute(Namespace="http://tempuri.org/VeriBranchMessages.xsd")]
    public VeriBranchMessageHeader VeriBranchMessageHeader;
    
    [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public OpenBankingGenericResponse VerificationUserResult;
    
    public VerificationUserResponse()
    {
    }
    
    public VerificationUserResponse(VeriBranchMessageHeader VeriBranchMessageHeader, OpenBankingGenericResponse VerificationUserResult)
    {
        this.VeriBranchMessageHeader = VeriBranchMessageHeader;
        this.VerificationUserResult = VerificationUserResult;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(WrapperName="AccountList", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
public partial class AccountListRequest
{
    
    [System.ServiceModel.MessageHeaderAttribute(Namespace="http://tempuri.org/VeriBranchMessages.xsd")]
    public VeriBranchMessageHeader VeriBranchMessageHeader;
    
    [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string username;
    
    [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string password;
    
    [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=2)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string customerID;
    
    [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=3)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string corporateUserCustomerID;
    
    public AccountListRequest()
    {
    }
    
    public AccountListRequest(VeriBranchMessageHeader VeriBranchMessageHeader, string username, string password, string customerID, string corporateUserCustomerID)
    {
        this.VeriBranchMessageHeader = VeriBranchMessageHeader;
        this.username = username;
        this.password = password;
        this.customerID = customerID;
        this.corporateUserCustomerID = corporateUserCustomerID;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(WrapperName="AccountListResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
public partial class AccountListResponse
{
    
    [System.ServiceModel.MessageHeaderAttribute(Namespace="http://tempuri.org/VeriBranchMessages.xsd")]
    public VeriBranchMessageHeader VeriBranchMessageHeader;
    
    [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public OpenBankingGenericResponse AccountListResult;
    
    public AccountListResponse()
    {
    }
    
    public AccountListResponse(VeriBranchMessageHeader VeriBranchMessageHeader, OpenBankingGenericResponse AccountListResult)
    {
        this.VeriBranchMessageHeader = VeriBranchMessageHeader;
        this.AccountListResult = AccountListResult;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(WrapperName="PaymentCheck", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
public partial class PaymentCheckRequest
{
    
    [System.ServiceModel.MessageHeaderAttribute(Namespace="http://tempuri.org/VeriBranchMessages.xsd")]
    public VeriBranchMessageHeader VeriBranchMessageHeader;
    
    [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string username;
    
    [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string password;
    
    [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=2)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string customerID;
    
    [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=3)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string corporateUserCustomerID;
    
    public PaymentCheckRequest()
    {
    }
    
    public PaymentCheckRequest(VeriBranchMessageHeader VeriBranchMessageHeader, string username, string password, string customerID, string corporateUserCustomerID)
    {
        this.VeriBranchMessageHeader = VeriBranchMessageHeader;
        this.username = username;
        this.password = password;
        this.customerID = customerID;
        this.corporateUserCustomerID = corporateUserCustomerID;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(WrapperName="PaymentCheckResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
public partial class PaymentCheckResponse
{
    
    [System.ServiceModel.MessageHeaderAttribute(Namespace="http://tempuri.org/VeriBranchMessages.xsd")]
    public VeriBranchMessageHeader VeriBranchMessageHeader;
    
    [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public OpenBankingGenericResponse PaymentCheckResult;
    
    public PaymentCheckResponse()
    {
    }
    
    public PaymentCheckResponse(VeriBranchMessageHeader VeriBranchMessageHeader, OpenBankingGenericResponse PaymentCheckResult)
    {
        this.VeriBranchMessageHeader = VeriBranchMessageHeader;
        this.PaymentCheckResult = PaymentCheckResult;
    }
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
public interface IOpenBankingIntegrationChannel : IOpenBankingIntegration, System.ServiceModel.IClientChannel
{
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
public partial class OpenBankingIntegrationClient : System.ServiceModel.ClientBase<IOpenBankingIntegration>, IOpenBankingIntegration
{
    
    /// <summary>
    /// Implement this partial method to configure the service endpoint.
    /// </summary>
    /// <param name="serviceEndpoint">The endpoint to configure</param>
    /// <param name="clientCredentials">The client credentials</param>
    static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
    
    public OpenBankingIntegrationClient() : 
            base(OpenBankingIntegrationClient.GetDefaultBinding(), OpenBankingIntegrationClient.GetDefaultEndpointAddress())
    {
        this.Endpoint.Name = EndpointConfiguration.BasicHttpBinding_IOpenBankingIntegration.ToString();
        ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
    }
    
    public OpenBankingIntegrationClient(EndpointConfiguration endpointConfiguration) : 
            base(OpenBankingIntegrationClient.GetBindingForEndpoint(endpointConfiguration), OpenBankingIntegrationClient.GetEndpointAddress(endpointConfiguration))
    {
        this.Endpoint.Name = endpointConfiguration.ToString();
        ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
    }
    
    public OpenBankingIntegrationClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
            base(OpenBankingIntegrationClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
    {
        this.Endpoint.Name = endpointConfiguration.ToString();
        ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
    }
    
    public OpenBankingIntegrationClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(OpenBankingIntegrationClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
    {
        this.Endpoint.Name = endpointConfiguration.ToString();
        ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
    }
    
    public OpenBankingIntegrationClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(binding, remoteAddress)
    {
    }
    
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    System.Threading.Tasks.Task<VerificationUserResponse> IOpenBankingIntegration.VerificationUserAsync(VerificationUserRequest request)
    {
        return base.Channel.VerificationUserAsync(request);
    }
    
    public System.Threading.Tasks.Task<VerificationUserResponse> VerificationUserAsync(VeriBranchMessageHeader VeriBranchMessageHeader, string username, string password, string customerID, string corporateUserCustomerID, string jsonData)
    {
        VerificationUserRequest inValue = new VerificationUserRequest();
        inValue.VeriBranchMessageHeader = VeriBranchMessageHeader;
        inValue.username = username;
        inValue.password = password;
        inValue.customerID = customerID;
        inValue.corporateUserCustomerID = corporateUserCustomerID;
        inValue.jsonData = jsonData;
        return ((IOpenBankingIntegration)(this)).VerificationUserAsync(inValue);
    }
    
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    System.Threading.Tasks.Task<AccountListResponse> IOpenBankingIntegration.AccountListAsync(AccountListRequest request)
    {
        return base.Channel.AccountListAsync(request);
    }
    
    public System.Threading.Tasks.Task<AccountListResponse> AccountListAsync(VeriBranchMessageHeader VeriBranchMessageHeader, string username, string password, string customerID, string corporateUserCustomerID)
    {
        AccountListRequest inValue = new AccountListRequest();
        inValue.VeriBranchMessageHeader = VeriBranchMessageHeader;
        inValue.username = username;
        inValue.password = password;
        inValue.customerID = customerID;
        inValue.corporateUserCustomerID = corporateUserCustomerID;
        return ((IOpenBankingIntegration)(this)).AccountListAsync(inValue);
    }
    
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    System.Threading.Tasks.Task<PaymentCheckResponse> IOpenBankingIntegration.PaymentCheckAsync(PaymentCheckRequest request)
    {
        return base.Channel.PaymentCheckAsync(request);
    }
    
    public System.Threading.Tasks.Task<PaymentCheckResponse> PaymentCheckAsync(VeriBranchMessageHeader VeriBranchMessageHeader, string username, string password, string customerID, string corporateUserCustomerID)
    {
        PaymentCheckRequest inValue = new PaymentCheckRequest();
        inValue.VeriBranchMessageHeader = VeriBranchMessageHeader;
        inValue.username = username;
        inValue.password = password;
        inValue.customerID = customerID;
        inValue.corporateUserCustomerID = corporateUserCustomerID;
        return ((IOpenBankingIntegration)(this)).PaymentCheckAsync(inValue);
    }
    
    public virtual System.Threading.Tasks.Task OpenAsync()
    {
        return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
    }
    
    private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
    {
        if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IOpenBankingIntegration))
        {
            System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
            result.MaxBufferSize = int.MaxValue;
            result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
            result.MaxReceivedMessageSize = int.MaxValue;
            result.AllowCookies = true;
            return result;
        }
        throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
    }
    
    private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
    {
        if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IOpenBankingIntegration))
        {
            return new System.ServiceModel.EndpointAddress("http://svtstr3webapp01.ebt.bank/SharedAspectsService/OpenBankingIntegration.svc/h" +
                    "ttp");
        }
        throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
    }
    
    private static System.ServiceModel.Channels.Binding GetDefaultBinding()
    {
        return OpenBankingIntegrationClient.GetBindingForEndpoint(EndpointConfiguration.BasicHttpBinding_IOpenBankingIntegration);
    }
    
    private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
    {
        return OpenBankingIntegrationClient.GetEndpointAddress(EndpointConfiguration.BasicHttpBinding_IOpenBankingIntegration);
    }
    
    public enum EndpointConfiguration
    {
        
        BasicHttpBinding_IOpenBankingIntegration,
    }
}