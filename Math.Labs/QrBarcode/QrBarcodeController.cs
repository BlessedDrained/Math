using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using ZXing;

namespace Math.Code.QrBarcode;

#pragma warning disable CA1416
/// <summary>
/// Контроллер, содержащий решения для лабораторных работ по дисциплине "Основы теории кодирования и шифрования"
/// </summary>
[ApiController]
[Route("barcode")]
public class QrBarcodeController : ControllerBase
{
    private readonly IServiceProvider _serviceProvider;

    public QrBarcodeController(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    /// <summary>
    /// Генерация штрих-кода
    /// </summary>
    [HttpPost]
    public IActionResult GenerateBarcode([FromBody] GenerateCodeRequest input)
    {
        var codeByteList = input.CodeType switch
        {
          
            CodeType.Qr => _serviceProvider.GetRequiredKeyedService<BarcodeWriter<Bitmap>>(DiKeyConstants.QrKey).Write(input.Input),
            CodeType.Barcode => _serviceProvider.GetRequiredKeyedService<BarcodeWriter<Bitmap>>(DiKeyConstants.BarcodeKey).Write(input.Input),
            _ => throw new ArgumentOutOfRangeException(nameof(input.CodeType))
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