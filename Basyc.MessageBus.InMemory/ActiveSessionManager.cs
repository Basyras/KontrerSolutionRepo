using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Client.NetMQ
{
    public class ActiveSessionManager : IActiveSessionManager
    {
        private int lastUsedSessionId = 0;
        private Dictionary<int, ActiveSession> sessions = new Dictionary<int, ActiveSession>();
        /// <summary>
        /// Return new session's id
        /// </summary>
        /// <returns></returns>
        public ActiveSession CreateSession()
        {
            var newSessionId = ++lastUsedSessionId;
            TaskCompletionSource<object> responseSource = new TaskCompletionSource<object>();
            var newSession = new ActiveSession(newSessionId, responseSource);
            sessions.Add(newSessionId, newSession);
            return newSession;
        }

        /// <summary>
        /// Return if session with specific id exists
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public bool TryCompleteSession(int sessionId, object sessionResult)
        {
            if (sessions.TryGetValue(sessionId, out var session) is false)
            {
                return false;
            }
            session.ResponseSource.SetResult(sessionResult);
            sessions.Remove(sessionId);

            return true;
        }

    }
}
