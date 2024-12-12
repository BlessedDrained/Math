using Microsoft.AspNetCore.Mvc;

namespace Math.Code.NoiseProofEncoding;

[ApiController]
[Route("hamming")]
public class HammingController : ControllerBase
{
    [HttpPost("encode")]
    public IActionResult Encode([FromBody] EncodeRequest request)
    {
        var result = Hamming.Encode(request.Message, 16);

        var response = new EncodeResponse()
        {
            EncodedMessageBlockList = result
        };
        
        return Ok(response);
    }

    [HttpPost("noisify")]
    public IActionResult Noisify([FromBody] NoisifyRequest request)
    {
        var result = Hamming.Noisify(request.MessageBlockList);

        var response = new NoisifyResponse()
        {
            NoisifiedMessageBlockList = result
        };

        return Ok(response);
    }

    [HttpPost("decode")]
    public IActionResult Decode([FromBody] DecodeRequest request)
    {
        var result = Hamming.Decode(request.EncodedMessage);

        var response = new DecodeResponse()
        {
            Message = result
        };
        
        return Ok(response);
    }
}

public record EncodeRequest
{
    public string Message { get; init; }
}

public record EncodeResponse
{
    public List<string> EncodedMessageBlockList { get; init; }
}

public record NoisifyRequest
{
    public List<string> MessageBlockList { get; init; }
}

public record NoisifyResponse
{
    public List<string> NoisifiedMessageBlockList { get; init; }
}

public record DecodeRequest
{
    public List<string> EncodedMessage { get; init; }
}

public record DecodeResponse
{
    public string Message { get; init; }
}