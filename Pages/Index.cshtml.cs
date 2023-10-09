using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TagLib;
using MediaInfo;
using System.IO;

namespace RazorPagesMovie.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public string Message { get; set;}

    private readonly IHostEnvironment Environment;

    public IndexModel(ILogger<IndexModel> logger, IHostEnvironment  _environment)
    {
        _logger = logger;
        Environment = _environment;
    }

    public void OnGet()
    {
    }

    public JsonResult OnPost(IFormFile file) 
    {
        string wwwroot = $"{Environment.ContentRootPath}/wwwroot/videos";
        using (FileStream stream = new(Path.Combine(wwwroot, file.FileName), FileMode.Create))
        file.CopyTo(stream);
        var midiaInfo = new MediaInfoWrapper($"{wwwroot}/{file.FileName}", _logger);
        Dictionary <string, string> format = new()
        {
            { "aspectRatio", midiaInfo.AspectRatio.ToString() },
            { "format", midiaInfo.Format },
            { "duration", midiaInfo.Duration.ToString() },
            { "height", midiaInfo.Height.ToString() },
            { "isDvd", midiaInfo.IsDvd.ToString() },
            { "isBlueRay", midiaInfo.IsBluRay.ToString() },
            { "is3d", midiaInfo.Is3D.ToString() },
            { "width", midiaInfo.Width.ToString() },
            { "hasSubtitles", midiaInfo.HasSubtitles.ToString() },
            { "attachments", midiaInfo.Attachments.ToString() },
            { "audioCodec", midiaInfo.AudioCodec.ToString() },
            { "profile", midiaInfo.Profile.ToString() },
            { "codec", midiaInfo.Codec.ToString() },
            { "formatVersion", midiaInfo.FormatVersion.ToString() },
            { "framerate", midiaInfo.Framerate.ToString() },
            { "videoResolution", midiaInfo.VideoResolution.ToString() },  
        };
        Message = $"Arquivo {file.FileName} enviado com sucesso";
        var metadata = new JsonResult(format);

        return metadata;
    }
}