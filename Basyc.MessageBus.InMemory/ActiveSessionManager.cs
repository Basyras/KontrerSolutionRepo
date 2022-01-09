using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Client.NetMQ
{
    public class ActiveSessionManager : IActiveSessionManager
    {
        public ActiveSessionManager(ILogger<ActiveSessionManager> logger)
        {
            this.logger = logger;
        }
        private int lastUsedSessionId = 0;
        private Dictionary<int, ActiveSession> sessions = new Dictionary<int, ActiveSession>();
        private readonly ILogger<ActiveSessionManager> logger;

        /// <summary>
        /// Return new session's id
        /// </summary>
        /// <returns></returns>
        public ActiveSession CreateSession(string messageType)
        {
            var newSessionId = ++lastUsedSessionId;
            TaskCompletionSource<object> responseSource = new TaskCompletionSource<object>();
            var newSession = new ActiveSession(newSessionId, messageType, responseSource);
            sessions.Add(newSessionId, newSession);
            logger.LogDebug($"Session '{newSession.SessionId}' created for '{messageType}'");
            return newSession;
        }

        /// <summary>
        /// Returns true if session with specific id exists
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public bool TryCompleteSession(int sessionId, object sessionResult)
        {
            if (sessions.TryGetValue(sessionId, out var sessionToComplete) is false)
            {
                return false;
            }
            sessionToComplete.ResponseSource.SetResult(sessionResult);
            sessions.Remove(sessionId);
            logger.LogDebug($"Session '{sessionToComplete.SessionId}' completed for '{sessionToComplete.MessageType}'");
            return true;
        }

    }
}
