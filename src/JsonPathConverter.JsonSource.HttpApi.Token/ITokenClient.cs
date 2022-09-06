﻿using IdentityModel.Client;

namespace JsonPathConverter.JsonSource.HttpApi.Token
{
    internal interface ITokenClient<TRequest> where TRequest : TokenRequest
    {
        Task<string> GetAccessTokenAsync(TRequest request, CancellationToken cancellationToken = default);
    }
}
