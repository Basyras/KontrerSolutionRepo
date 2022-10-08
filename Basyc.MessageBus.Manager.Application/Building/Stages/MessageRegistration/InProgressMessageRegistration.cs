using Basyc.MessageBus.Manager.Application.Initialization;
using System;
using System.Collections.Generic;

namespace Basyc.MessageBus.Manager.Application.Building.Stages.MessageRegistration
{
	public class InProgressMessageRegistration
	{
		public string? MessagDisplayName { get; set; }
		public RequestType MessageType { get; set; }
		public List<ParameterInfo> Parameters { get; } = new List<ParameterInfo>();
		public Action<RequestResultContext>? RequestHandler { get; set; }
		public Type? ResponseRunTimeType { get; set; }
		public string? ResponseRunTimeTypeDisplayName { get; set; }
		public bool HasResponse => ResponseRunTimeType is not null;

	}
}
