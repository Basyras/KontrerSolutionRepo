﻿namespace Basyc.MessageBus.Manager.Application.ResultDiagnostics
{
	public interface IResultLoggingManager
	{
		ResultLoggingContext RegisterLoggingContex(RequestResult requestResult);
		ResultLoggingContext GetLoggingContext(RequestResult requestResult);
		void FinishLoggingContext(RequestResult requestResult);
	}
}