﻿using Basyc.MessageBus.Manager.Application;

namespace Basyc.MessageBus.Manager.Presentation.BlazorLibrary.Pages.Requests.RequestTag;

public enum RequestTagType
{
	Query,
	Command,
	Response,
	Generic,
}

public static class RequestTagTypeHelper
{
	public static RequestTagType FromRequestType(RequestType requestType)
	{
		return requestType switch
		{
			RequestType.Query => RequestTagType.Query,
			RequestType.Command => RequestTagType.Command,
			RequestType.Generic => RequestTagType.Generic,
			_ => throw new NotImplementedException(),
		};
	}
}
