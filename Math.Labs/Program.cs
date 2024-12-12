using System.Drawing;
using System.Text.Json;
using Math.Code.QrBarcode;
using ZXing;
using ZXing.OneD;
using ZXing.QrCode;
using ZXing.QrCode.Internal;
using ZXing.Windows.Compatibility;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#pragma warning disable CA1416
var barcodeWriter = new BarcodeWriter<Bitmap>()
{
    Format = BarcodeFormat.CODE_128,
    Options = new Code128EncodingOptions()
    {
        Width = 300,
        Height = 100,
        Margin = 20
    },
    Renderer = new BitmapRenderer()
};

builder.Services.AddKeyedSingleton(DiKeyConstants.BarcodeKey, barcodeWriter);

var qrCodeWriter = new BarcodeWriter<Bitmap>()
{
    Format = BarcodeFormat.QR_CODE,
    Options = new QrCodeEncodingOptions()
    {
        DisableECI = true,
        Height = 300,
        Width = 300,
        ErrorCorrection = ErrorCorrectionLevel.H,
    },
    Renderer = new BitmapRenderer()
};

builder.Services.AddKeyedSingleton(DiKeyConstants.QrKey, qrCodeWriter);

#pragma warning restore CA1416


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();