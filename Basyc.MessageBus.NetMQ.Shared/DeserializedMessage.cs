using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.NetMQ.Shared;

//public record DeserializedMessage(int SessionId, MessageCase MessageCase, bool ExpectsResponse, object Message, string MessageType, Type? ResponseType);
public class DeserializedMessage
{
    public RequestCase? Request { get; init; }
    public ResponseCase? Response  { get; init; }
    public CheckInMessage?  CheckIn { get; init; }
    public MessageCase MessageCase { get; }

    private DeserializedMessage(MessageCase messageCase)
    {
        this.MessageCase = messageCase;
    }
    private DeserializedMessage(MessageCase messageCase, RequestCase requestCase) : this(messageCase)
    {
        Request = requestCase;
    }

    private DeserializedMessage(MessageCase messageCase, ResponseCase responseCase) : this(messageCase)
    {
        Response = responseCase;
    }

    private DeserializedMessage(MessageCase messageCase, CheckInMessage checkIn) : this(messageCase)
    {
        CheckIn = checkIn;
    }

    public static DeserializedMessage CreateCheckIn(CheckInMessage checkIn)
    {      
        return new DeserializedMessage(MessageCase.CheckIn, checkIn);
    }

    public static DeserializedMessage CreateRequest(RequestCase requestCase)
    {       
        return new DeserializedMessage(MessageCase.Request, requestCase);
    }

    public static DeserializedMessage CreateResponse(ResponseCase responseCase)
    {
        return new DeserializedMessage(MessageCase.Response, responseCase);
    }
}
