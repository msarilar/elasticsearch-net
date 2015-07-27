﻿using System;
using System.Collections.Generic;
using Elasticsearch.Net;
using Newtonsoft.Json;

namespace Nest
{
	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	public interface ICountRequest : IQueryPath<CountRequestParameters>
	{
		
		[JsonProperty("query")]
		IQueryContainer Query { get; set; }
	}
	public interface ICountRequest<T> : ICountRequest
		where T : class {}

	internal static class CountPathInfo
	{
		public static void Update(ElasticsearchPathInfo<CountRequestParameters> pathInfo, ICountRequest request)
		{
			var source = request.RequestParameters.GetQueryStringValue<string>("source");
			pathInfo.HttpMethod = source.IsNullOrEmpty() 
				&& (request.Query == null || request.Query.IsConditionless)
				? HttpMethod.GET
				: HttpMethod.POST;
		}
	}
	
	public partial class CountRequest : QueryPathBase<CountRequestParameters>, ICountRequest
	{
		public CountRequest() {}

		public CountRequest(IndexName index, TypeName type = null) : base(index, type) { }

		public CountRequest(IEnumerable<IndexName> indices, IEnumerable<TypeName> types = null) : base(indices, types) { }

		public IQueryContainer Query { get; set; }

		protected override void UpdatePathInfo(IConnectionSettingsValues settings, ElasticsearchPathInfo<CountRequestParameters> pathInfo)
		{
			CountPathInfo.Update(pathInfo, this);
		}
	}

	public partial class CountRequest<T> : QueryPathBase<CountRequestParameters, T>, ICountRequest
		where T : class
	{
		public CountRequest() {}

		public CountRequest(IndexName index, TypeName type = null) : base(index, type) { }

		public CountRequest(IEnumerable<IndexName> indices, IEnumerable<TypeName> types = null) : base(indices, types) { }

		public IQueryContainer Query { get; set; }

		protected override void UpdatePathInfo(IConnectionSettingsValues settings, ElasticsearchPathInfo<CountRequestParameters> pathInfo)
		{
			CountPathInfo.Update(pathInfo, this);
		}
	}
	
	[DescriptorFor("Count")]
	public partial class CountDescriptor<T> : QueryPathDescriptorBase<CountDescriptor<T>, CountRequestParameters, T>, ICountRequest
		where T : class
	{
		private ICountRequest Self => this;

		IQueryContainer ICountRequest.Query { get; set; }

		public CountDescriptor<T> Query(Func<QueryContainerDescriptor<T>, QueryContainer> querySelector)
		{
			Self.Query = querySelector(new QueryContainerDescriptor<T>());
			return this;
		}

		protected override void UpdatePathInfo(IConnectionSettingsValues settings, ElasticsearchPathInfo<CountRequestParameters> pathInfo)
		{
			CountPathInfo.Update(pathInfo, this);
		}
	}
}
