using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using ZXing;
using ZXing.OneD;
using ZXing.QrCode;
using ZXing.QrCode.Internal;
using ZXing.Windows.Compatibility;

namespace Math.Code.QrBarcode;

/// <summary>
/// Контроллер, содержащий решения для лабораторных работ по дисциплине "Основы теории кодирования и шифрования"
/// </summary>
[ApiController]
[Route("barcode")]
public class QrBarcodeController : ControllerBase
{
    #region QR/Barcode

    /// <summary>
    /// singleton настроек генерации qr кодов
    /// </summary>
    private static readonly QrCodeEncodingOptions _options = new()
    {
        DisableECI = true,
        Height = 300,
        Width = 300,
        ErrorCorrection = ErrorCorrectionLevel.H
    };

    /// <summary>
    /// singleton настроек генерации штрихкодов
    /// </summary>
    private static readonly Code128EncodingOptions _code128EncodingOptions = new()
    {
        Width = 300,
        Height = 100
    };

#pragma warning disable CA1416
    /// <summary>
    /// singleton генератора штрихкодов
    /// </summary>
    private static readonly BarcodeWriter<Bitmap> _barcodeWriter = new()
    {
        Format = BarcodeFormat.CODE_128,
        Options = _code128EncodingOptions,
        Renderer = new BitmapRenderer()
    };

    /// <summary>
    /// singleton генератора qr кодов
    /// </summary>
    private static readonly BarcodeWriter<Bitmap> _qrCodeWriter = new()
    {
        Format = BarcodeFormat.QR_CODE,
        Options = _options,
        Renderer = new BitmapRenderer()
    };
    
    /// <summary>
    /// Генерация штрих-кода
    /// </summary>
    [HttpPost]
    public IActionResult GenerateBarcode([FromBody] GenerateCodeRequest input)
    {
        var codeByteList = input.CodeType switch
        {
            CodeType.Qr => _qrCodeWriter.Write(input.Input),
            CodeType.Barcode => _barcodeWriter.Write(input.Input),
            _ => throw new ArgumentOutOfRangeException()
        };
        
        // Создание стрима для записи
        var ms = new MemoryStream();
        
        // Запись бинарных данных PNG-изображения в стрим
        codeByteList.Save(ms, ImageFormat.Png);
        
        // В процессе записи стрима указатель на позицию будет сдвинут на записанное количество байт. 
        // Если вернуть стрим в таком состоянии, то произойдет ошибка - из стрима в процессе обработки ответа будет прочитано меньше байт, чем фактически отправлено
        // Таким образом, нужно сдвинуть позицию на начальную - 0.
        ms.Seek(0, SeekOrigin.Begin);
        
        // Выше было задано, что изображение будет возвращено в формате PNG. Указываем соответствующий content-type
        return File(ms, MediaTypeNames.Image.Png);
    }

    #endregion
    
    public class GenerateCodeRequest
    {
        public string Input { get; init; }
        
        public CodeType CodeType { get; init; }
    }
    
    public enum CodeType
    {
        Unknown = 0,
        Qr = 1,
        Barcode = 2
    }
}