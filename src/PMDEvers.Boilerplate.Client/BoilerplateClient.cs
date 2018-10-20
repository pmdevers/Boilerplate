using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using PMDEvers.Boilerplate.Abstractions;

namespace PMDEvers.Boilerplate.Client
{
    public class BoilerplateClient : IBoilerplateClient
    {
	    private readonly HttpClient _client;
	    private readonly ILogger<BoilerplateClient> _logger;
	    private readonly string _entpointName;

	    public BoilerplateClient(HttpClient client, ILogger<BoilerplateClient> logger)
	    {
		    _client = client;
		    _logger = logger;
		    _entpointName = GetType().Name.Replace("Client", String.Empty).ToLower();

	    }

	    public async Task<IBoilerplate> GetAsync(CancellationToken cancellationToken = default(CancellationToken))
	    {
			cancellationToken.ThrowIfCancellationRequested();
		    HttpResponseMessage response = await _client.GetAsync($"{_entpointName}", HttpCompletionOption.ResponseHeadersRead, cancellationToken);

		    Stream stream = await response.Content.ReadAsStreamAsync();

		    if (response.IsSuccessStatusCode)
			    return DeserializeJsonFromStream<IBoilerplate>(stream);

		    string content = await StreamToStringAsync(stream);
		    throw new BoilerplateException(
			    content,
			    (int)response.StatusCode
		    );
	    }


		private static T DeserializeJsonFromStream<T>(Stream stream)
	    {
		    if (stream == null || stream.CanRead == false)
			    return default(T);

		    using (var sr = new StreamReader(stream))
		    using (var jtr = new JsonTextReader(sr))
		    {
			    var js = new JsonSerializer();
			    var searchResult = js.Deserialize<T>(jtr);
			    return searchResult;
		    }
	    }

	    private static async Task<string> StreamToStringAsync(Stream stream)
	    {
		    if (stream == null)
			    return null;

		    using (var sr = new StreamReader(stream))
			    return await sr.ReadToEndAsync();

	    }
	}
}
