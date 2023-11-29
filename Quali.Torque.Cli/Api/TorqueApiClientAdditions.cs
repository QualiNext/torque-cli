namespace Torque.Cli.Api;

public partial class TorqueApiClient
{
    /// <summary>
    /// Get eac entries
    /// </summary>
    /// <returns>Success</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual System.Threading.Tasks.Task<System.Collections.Generic.ICollection<EacResponse>> EacAsync(
        string space_name)
    {
        return EacAsync(space_name, System.Threading.CancellationToken.None);
    }

    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Get eac entries
    /// </summary>
    /// <returns>Success</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async System.Threading.Tasks.Task<System.Collections.Generic.ICollection<EacResponse>> EacAsync(
        string space_name, System.Threading.CancellationToken cancellationToken)
    {
        if (space_name == null)
            throw new System.ArgumentNullException("space_name");

        var client_ = _httpClient;
        var disposeClient_ = false;
        try
        {
            using (var request_ = new System.Net.Http.HttpRequestMessage())
            {
                request_.Method = new System.Net.Http.HttpMethod("GET");
                request_.Headers.Accept.Add(
                    System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));

                var urlBuilder_ = new System.Text.StringBuilder();
                if (!string.IsNullOrEmpty(BaseUrl)) urlBuilder_.Append(BaseUrl);
                urlBuilder_.Append("api");
                urlBuilder_.Append('/');
                urlBuilder_.Append("spaces");
                urlBuilder_.Append('/');
                urlBuilder_.Append(System.Uri.EscapeDataString(ConvertToString(space_name,
                    System.Globalization.CultureInfo.InvariantCulture)));
                urlBuilder_.Append('/');
                urlBuilder_.Append("eac");

                PrepareRequest(client_, request_, urlBuilder_);

                var url_ = urlBuilder_.ToString();
                request_.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);

                PrepareRequest(client_, request_, url_);

                var response_ = await client_
                    .SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                    .ConfigureAwait(false);
                var disposeResponse_ = true;
                try
                {
                    var headers_ = System.Linq.Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                    if (response_.Content != null && response_.Content.Headers != null)
                    {
                        foreach (var item_ in response_.Content.Headers)
                            headers_[item_.Key] = item_.Value;
                    }

                    ProcessResponse(client_, response_);

                    var status_ = (int)response_.StatusCode;
                    if (status_ == 200)
                    {
                        var objectResponse_ =
                            await ReadObjectResponseAsync<System.Collections.Generic.ICollection<EacResponse>>(
                                response_, headers_, cancellationToken).ConfigureAwait(false);
                        if (objectResponse_.Object == null)
                        {
                            throw new ApiException("Response was null which was not expected.", status_,
                                objectResponse_.Text, headers_, null);
                        }

                        return objectResponse_.Object;
                    }
                    else if (status_ == 404)
                    {
                        var objectResponse_ =
                            await ReadObjectResponseAsync<ErrorResponse>(response_, headers_, cancellationToken)
                                .ConfigureAwait(false);
                        if (objectResponse_.Object == null)
                        {
                            throw new ApiException("Response was null which was not expected.", status_,
                                objectResponse_.Text, headers_, null);
                        }

                        throw new ApiException<ErrorResponse>("Not Found", status_, objectResponse_.Text, headers_,
                            objectResponse_.Object, null);
                    }
                    else
                    {
                        var responseData_ = response_.Content == null
                            ? null
                            : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                        throw new ApiException(
                            "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                            responseData_, headers_, null);
                    }
                }
                finally
                {
                    if (disposeResponse_)
                        response_.Dispose();
                }
            }
        }
        finally
        {
            if (disposeClient_)
                client_.Dispose();
        }
    }

    /// <summary>
    /// Plan environment
    /// </summary>
    /// <remarks>
    /// Initiates plan on environment request.
    /// </remarks>
    /// <param name="space_name">Name of the space containing the environments.</param>
    /// <param name="environment_id">Environment ID.</param>
    /// <param name="body">Plan request.</param>
    /// <returns>Success</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual System.Threading.Tasks.Task<PlanEnvironmentResponse> PlanPOSTAsync(string space_name,
        string environment_id, PlanEnvironmentRequest body)
    {
        return PlanPOSTAsync(space_name, environment_id, body, System.Threading.CancellationToken.None);
    }

    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Plan environment
    /// </summary>
    /// <remarks>
    /// Initiates plan on environment request.
    /// </remarks>
    /// <param name="space_name">Name of the space containing the environments.</param>
    /// <param name="environment_id">Environment ID.</param>
    /// <param name="body">Plan request.</param>
    /// <returns>Success</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async System.Threading.Tasks.Task<PlanEnvironmentResponse> PlanPOSTAsync(string space_name,
        string environment_id, PlanEnvironmentRequest body, System.Threading.CancellationToken cancellationToken)
    {
        if (space_name == null)
            throw new System.ArgumentNullException("space_name");

        if (environment_id == null)
            throw new System.ArgumentNullException("environment_id");

        var client_ = _httpClient;
        var disposeClient_ = false;
        try
        {
            using (var request_ = new System.Net.Http.HttpRequestMessage())
            {
                var json_ = Newtonsoft.Json.JsonConvert.SerializeObject(body, _settings.Value);
                var content_ = new System.Net.Http.StringContent(json_);
                content_.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json");
                request_.Content = content_;
                request_.Method = new System.Net.Http.HttpMethod("POST");
                request_.Headers.Accept.Add(
                    System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));

                var urlBuilder_ = new System.Text.StringBuilder();
                if (!string.IsNullOrEmpty(BaseUrl)) urlBuilder_.Append(BaseUrl);
                urlBuilder_.Append("api");
                urlBuilder_.Append('/');
                urlBuilder_.Append("spaces");
                urlBuilder_.Append('/');
                urlBuilder_.Append(System.Uri.EscapeDataString(ConvertToString(space_name,
                    System.Globalization.CultureInfo.InvariantCulture)));
                urlBuilder_.Append('/');
                urlBuilder_.Append("environments");
                urlBuilder_.Append('/');
                urlBuilder_.Append(System.Uri.EscapeDataString(ConvertToString(environment_id,
                    System.Globalization.CultureInfo.InvariantCulture)));
                urlBuilder_.Append('/');
                urlBuilder_.Append("plan");

                PrepareRequest(client_, request_, urlBuilder_);

                var url_ = urlBuilder_.ToString();
                request_.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);

                PrepareRequest(client_, request_, url_);

                var response_ = await client_
                    .SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                    .ConfigureAwait(false);
                var disposeResponse_ = true;
                try
                {
                    var headers_ = System.Linq.Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                    if (response_.Content != null && response_.Content.Headers != null)
                    {
                        foreach (var item_ in response_.Content.Headers)
                            headers_[item_.Key] = item_.Value;
                    }

                    ProcessResponse(client_, response_);

                    var status_ = (int)response_.StatusCode;
                    if (status_ == 200)
                    {
                        var objectResponse_ =
                            await ReadObjectResponseAsync<PlanEnvironmentResponse>(response_, headers_,
                                cancellationToken).ConfigureAwait(false);
                        if (objectResponse_.Object == null)
                        {
                            throw new ApiException("Response was null which was not expected.", status_,
                                objectResponse_.Text, headers_, null);
                        }

                        return objectResponse_.Object;
                    }
                    else if (status_ == 404)
                    {
                        var objectResponse_ =
                            await ReadObjectResponseAsync<ErrorResponse>(response_, headers_, cancellationToken)
                                .ConfigureAwait(false);
                        if (objectResponse_.Object == null)
                        {
                            throw new ApiException("Response was null which was not expected.", status_,
                                objectResponse_.Text, headers_, null);
                        }

                        throw new ApiException<ErrorResponse>("Space not found", status_, objectResponse_.Text,
                            headers_, objectResponse_.Object, null);
                    }
                    else if (status_ == 422)
                    {
                        var objectResponse_ =
                            await ReadObjectResponseAsync<ErrorResponse>(response_, headers_, cancellationToken)
                                .ConfigureAwait(false);
                        if (objectResponse_.Object == null)
                        {
                            throw new ApiException("Response was null which was not expected.", status_,
                                objectResponse_.Text, headers_, null);
                        }

                        throw new ApiException<ErrorResponse>("Client Error", status_, objectResponse_.Text, headers_,
                            objectResponse_.Object, null);
                    }
                    else if (status_ == 408)
                    {
                        var objectResponse_ =
                            await ReadObjectResponseAsync<ErrorResponse>(response_, headers_, cancellationToken)
                                .ConfigureAwait(false);
                        if (objectResponse_.Object == null)
                        {
                            throw new ApiException("Response was null which was not expected.", status_,
                                objectResponse_.Text, headers_, null);
                        }

                        throw new ApiException<ErrorResponse>("Request timeout", status_, objectResponse_.Text,
                            headers_, objectResponse_.Object, null);
                    }
                    else if (status_ == 409)
                    {
                        var objectResponse_ =
                            await ReadObjectResponseAsync<ErrorResponse>(response_, headers_, cancellationToken)
                                .ConfigureAwait(false);
                        if (objectResponse_.Object == null)
                        {
                            throw new ApiException("Response was null which was not expected.", status_,
                                objectResponse_.Text, headers_, null);
                        }

                        throw new ApiException<ErrorResponse>(
                            "Request is in conflict with the operation that is currently running", status_,
                            objectResponse_.Text, headers_, objectResponse_.Object, null);
                    }
                    else
                    {
                        var responseData_ = response_.Content == null
                            ? null
                            : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                        throw new ApiException(
                            "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                            responseData_, headers_, null);
                    }
                }
                finally
                {
                    if (disposeResponse_)
                        response_.Dispose();
                }
            }
        }
        finally
        {
            if (disposeClient_)
                client_.Dispose();
        }
    }

    /// <summary>
    /// Update environment grains
    /// </summary>
    /// <remarks>
    /// Get Plan results for environment
    /// </remarks>
    /// <param name="space_name">Name of the space containing the environments.</param>
    /// <param name="environment_id">Environment ID.</param>
    /// <param name="request_id">Plan request.</param>
    /// <returns>Success</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual System.Threading.Tasks.Task<GetEnvironmentPlanResultResponse> PlanGETAsync(string space_name,
        string environment_id, System.Guid request_id)
    {
        return PlanGETAsync(space_name, environment_id, request_id, System.Threading.CancellationToken.None);
    }

    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Update environment grains
    /// </summary>
    /// <remarks>
    /// Get Plan results for environment
    /// </remarks>
    /// <param name="space_name">Name of the space containing the environments.</param>
    /// <param name="environment_id">Environment ID.</param>
    /// <param name="request_id">Plan request.</param>
    /// <returns>Success</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async System.Threading.Tasks.Task<GetEnvironmentPlanResultResponse> PlanGETAsync(string space_name,
        string environment_id, System.Guid request_id, System.Threading.CancellationToken cancellationToken)
    {
        if (space_name == null)
            throw new System.ArgumentNullException("space_name");

        if (environment_id == null)
            throw new System.ArgumentNullException("environment_id");

        var client_ = _httpClient;
        var disposeClient_ = false;
        try
        {
            using (var request_ = new System.Net.Http.HttpRequestMessage())
            {
                request_.Method = new System.Net.Http.HttpMethod("GET");
                request_.Headers.Accept.Add(
                    System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));

                var urlBuilder_ = new System.Text.StringBuilder();
                if (!string.IsNullOrEmpty(BaseUrl)) urlBuilder_.Append(BaseUrl);
                urlBuilder_.Append("api");
                urlBuilder_.Append('/');
                urlBuilder_.Append("spaces");
                urlBuilder_.Append('/');
                urlBuilder_.Append(System.Uri.EscapeDataString(ConvertToString(space_name,
                    System.Globalization.CultureInfo.InvariantCulture)));
                urlBuilder_.Append('/');
                urlBuilder_.Append("environments");
                urlBuilder_.Append('/');
                urlBuilder_.Append(System.Uri.EscapeDataString(ConvertToString(environment_id,
                    System.Globalization.CultureInfo.InvariantCulture)));
                urlBuilder_.Append('/');
                urlBuilder_.Append("plan");
                urlBuilder_.Append('/');
                urlBuilder_.Append(System.Uri.EscapeDataString(ConvertToString(request_id,
                    System.Globalization.CultureInfo.InvariantCulture)));

                PrepareRequest(client_, request_, urlBuilder_);

                var url_ = urlBuilder_.ToString();
                request_.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);

                PrepareRequest(client_, request_, url_);

                var response_ = await client_
                    .SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                    .ConfigureAwait(false);
                var disposeResponse_ = true;
                try
                {
                    var headers_ = System.Linq.Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                    if (response_.Content != null && response_.Content.Headers != null)
                    {
                        foreach (var item_ in response_.Content.Headers)
                            headers_[item_.Key] = item_.Value;
                    }

                    ProcessResponse(client_, response_);

                    var status_ = (int)response_.StatusCode;
                    if (status_ == 200)
                    {
                        var objectResponse_ =
                            await ReadObjectResponseAsync<GetEnvironmentPlanResultResponse>(response_, headers_,
                                cancellationToken).ConfigureAwait(false);
                        if (objectResponse_.Object == null)
                        {
                            throw new ApiException("Response was null which was not expected.", status_,
                                objectResponse_.Text, headers_, null);
                        }

                        return objectResponse_.Object;
                    }
                    else if (status_ == 404)
                    {
                        var objectResponse_ =
                            await ReadObjectResponseAsync<ErrorResponse>(response_, headers_, cancellationToken)
                                .ConfigureAwait(false);
                        if (objectResponse_.Object == null)
                        {
                            throw new ApiException("Response was null which was not expected.", status_,
                                objectResponse_.Text, headers_, null);
                        }

                        throw new ApiException<ErrorResponse>("Space not found", status_, objectResponse_.Text,
                            headers_, objectResponse_.Object, null);
                    }
                    else if (status_ == 422)
                    {
                        var objectResponse_ =
                            await ReadObjectResponseAsync<ErrorResponse>(response_, headers_, cancellationToken)
                                .ConfigureAwait(false);
                        if (objectResponse_.Object == null)
                        {
                            throw new ApiException("Response was null which was not expected.", status_,
                                objectResponse_.Text, headers_, null);
                        }

                        throw new ApiException<ErrorResponse>("Client Error", status_, objectResponse_.Text, headers_,
                            objectResponse_.Object, null);
                    }
                    else if (status_ == 408)
                    {
                        var objectResponse_ =
                            await ReadObjectResponseAsync<ErrorResponse>(response_, headers_, cancellationToken)
                                .ConfigureAwait(false);
                        if (objectResponse_.Object == null)
                        {
                            throw new ApiException("Response was null which was not expected.", status_,
                                objectResponse_.Text, headers_, null);
                        }

                        throw new ApiException<ErrorResponse>("Request timeout", status_, objectResponse_.Text,
                            headers_, objectResponse_.Object, null);
                    }
                    else if (status_ == 409)
                    {
                        var objectResponse_ =
                            await ReadObjectResponseAsync<ErrorResponse>(response_, headers_, cancellationToken)
                                .ConfigureAwait(false);
                        if (objectResponse_.Object == null)
                        {
                            throw new ApiException("Response was null which was not expected.", status_,
                                objectResponse_.Text, headers_, null);
                        }

                        throw new ApiException<ErrorResponse>(
                            "Request is in conflict with the operation that is currently running", status_,
                            objectResponse_.Text, headers_, objectResponse_.Object, null);
                    }
                    else
                    {
                        var responseData_ = response_.Content == null
                            ? null
                            : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                        throw new ApiException(
                            "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                            responseData_, headers_, null);
                    }
                }
                finally
                {
                    if (disposeResponse_)
                        response_.Dispose();
                }
            }
        }
        finally
        {
            if (disposeClient_)
                client_.Dispose();
        }
    }
    
            /// <summary>
        /// Export environment YAML file
        /// </summary>
        /// <returns>Success</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public virtual System.Threading.Tasks.Task<FileResponse> EacAsync(string space_name, string environment_id)
        {
            return EacAsync(space_name, environment_id, System.Threading.CancellationToken.None);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>
        /// Export environment YAML file
        /// </summary>
        /// <returns>Success</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public virtual async System.Threading.Tasks.Task<FileResponse> EacAsync(string space_name, string environment_id, System.Threading.CancellationToken cancellationToken)
        {
            if (space_name == null)
                throw new System.ArgumentNullException("space_name");

            if (environment_id == null)
                throw new System.ArgumentNullException("environment_id");

            var client_ = _httpClient;
            var disposeClient_ = false;
            try
            {
                using (var request_ = new System.Net.Http.HttpRequestMessage())
                {
                    request_.Method = new System.Net.Http.HttpMethod("GET");
                    request_.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));

                    var urlBuilder_ = new System.Text.StringBuilder();
                    if (!string.IsNullOrEmpty(BaseUrl)) urlBuilder_.Append(BaseUrl);
                    urlBuilder_.Append("api");
                    urlBuilder_.Append('/');
                    urlBuilder_.Append("spaces");
                    urlBuilder_.Append('/');
                    urlBuilder_.Append(System.Uri.EscapeDataString(ConvertToString(space_name, System.Globalization.CultureInfo.InvariantCulture)));
                    urlBuilder_.Append('/');
                    urlBuilder_.Append("environments");
                    urlBuilder_.Append('/');
                    urlBuilder_.Append(System.Uri.EscapeDataString(ConvertToString(environment_id, System.Globalization.CultureInfo.InvariantCulture)));
                    urlBuilder_.Append('/');
                    urlBuilder_.Append("eac");

                    PrepareRequest(client_, request_, urlBuilder_);

                    var url_ = urlBuilder_.ToString();
                    request_.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);

                    PrepareRequest(client_, request_, url_);

                    var response_ = await client_.SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse_ = true;
                    try
                    {
                        var headers_ = System.Linq.Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                        if (response_.Content != null && response_.Content.Headers != null)
                        {
                            foreach (var item_ in response_.Content.Headers)
                                headers_[item_.Key] = item_.Value;
                        }

                        ProcessResponse(client_, response_);

                        var status_ = (int)response_.StatusCode;
                        if (status_ == 200 || status_ == 206)
                        {
                            var responseStream_ = response_.Content == null ? System.IO.Stream.Null : await response_.Content.ReadAsStreamAsync().ConfigureAwait(false);
                            var fileResponse_ = new FileResponse(status_, headers_, responseStream_, null, response_);
                            disposeClient_ = false; disposeResponse_ = false; // response and client are disposed by FileResponse
                            return fileResponse_;
                        }
                        else
                        if (status_ == 404)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<ErrorResponse>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }
                            throw new ApiException<ErrorResponse>("Not Found", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        {
                            var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + status_ + ").", status_, responseData_, headers_, null);
                        }
                    }
                    finally
                    {
                        if (disposeResponse_)
                            response_.Dispose();
                    }
                }
            }
            finally
            {
                if (disposeClient_)
                    client_.Dispose();
            }
        }
}


public partial class EacResponse
{
    [Newtonsoft.Json.JsonProperty("url", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
    public string Url { get; set; }

    [Newtonsoft.Json.JsonProperty("environment_name", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
    public string Environment_name { get; set; }

    [Newtonsoft.Json.JsonProperty("environment_id", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
    public string Environment_id { get; set; }

    [Newtonsoft.Json.JsonProperty("owner_email", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
    public string Owner_email { get; set; }

    [Newtonsoft.Json.JsonProperty("blueprint_name", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
    public string Blueprint_name { get; set; }

    [Newtonsoft.Json.JsonProperty("inputs", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
    public System.Collections.Generic.IDictionary<string, string> Inputs { get; set; }

    [Newtonsoft.Json.JsonProperty("status", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
    public string Status { get; set; }

    [Newtonsoft.Json.JsonProperty("errors", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
    public System.Collections.Generic.ICollection<Error> Errors { get; set; }
}

public partial class PlanEnvironmentRequest
{
    /// <summary>
    /// Environment yaml content
    /// </summary>
    [Newtonsoft.Json.JsonProperty("env_yaml_content", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
    public string Env_yaml_content { get; set; }

}

public partial class PlanEnvironmentResponse
{
    [Newtonsoft.Json.JsonProperty("request_handle", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
    public System.Guid Request_handle { get; set; }

}

public partial class GetEnvironmentPlanResultResponse
{
    [Newtonsoft.Json.JsonProperty("status", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
    public string Status { get; set; }

    [Newtonsoft.Json.JsonProperty("errors", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
    public System.Collections.Generic.ICollection<string> Errors { get; set; }

    [Newtonsoft.Json.JsonProperty("plan", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
    public EnvironmentPlanResultInfo Plan { get; set; }
}

public partial class EnvironmentPlanResultInfo
{
    [Newtonsoft.Json.JsonProperty("environment", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
    public EnvironmentPlanResultEnvironmentInfo Environment { get; set; }

}

public partial class EnvironmentPlanResultEnvironmentInfo
{
    [Newtonsoft.Json.JsonProperty("name", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
    public string Name { get; set; }

    [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
    public string Id { get; set; }

    [Newtonsoft.Json.JsonProperty("grains", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
    public System.Collections.Generic.ICollection<EnvironmentPlanResultGrainInfo> Grains { get; set; }
}

public partial class EnvironmentPlanResultGrainInfo
{
    [Newtonsoft.Json.JsonProperty("path", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
    public string Path { get; set; }

    [Newtonsoft.Json.JsonProperty("source_commit", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
    public string Source_commit { get; set; }

    [Newtonsoft.Json.JsonProperty("target_commit", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
    public string Target_commit { get; set; }

    [Newtonsoft.Json.JsonProperty("content", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
    public string Content { get; set; }
}
