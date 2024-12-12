using System.Collections;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Mime;
using System.Text;
using Math.Code.QrBarcode.ManualImplementation;
using Math.Code.QrBarcode.ManualImplementation.CorrectionLevel;
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
    /// Генерация QR-кода с помощью ручной реализации
    /// </summary>
    [HttpPost("manual")]
    public IActionResult GenerateBarcodeViaManual([FromQuery] string input, QrErrorCorrectionLevel level)
    {
        // Получение последовательности байт для переданных данных
        var byteList = Encoding.UTF8.GetBytes(input);
        var bitList = new BitArray(byteList);

        var version = QrPayloadEncoder.GetVersion(bitList.Length, level);

        var bitCountFieldSize = QrPayloadEncoder.GetBitCountForQrVersionAndEncodingType(version, EncodingType.Byte);

        var totalBitCount = bitCountFieldSize + 4 + bitList.Length;
        var newVersion = QrPayloadEncoder.GetVersion(totalBitCount, level);
        if (newVersion > version)
        {
            version = newVersion;
        }

        // дополняем до кратного 8
        while (totalBitCount % 8 > 0)
        {
            totalBitCount++;
        }

        // 4 - размер поля, определяющего способ кодирования - Byte
        var bitListWithHeader = new BitArray(length: totalBitCount)
        {
            // для байтового способа первые четыре бита - 0100. 1,3 и 4 по умолчанию = false
            [1] = true
        };

        var byteListLengthBits = new BitArray(new[] { byteList.Length });
        for (var i = 0; i < bitCountFieldSize; i++)
        {
            bitListWithHeader[i + 4] = byteListLengthBits[i];
        }

        var primaryCounter = 0;
        for (var i = bitCountFieldSize + 4; i < bitList.Length; i++)
        {
            bitListWithHeader[i] = bitList[primaryCounter];
            primaryCounter++;
        }

        var versionBitCount = QrPayloadEncoder.GetMaxBitCountForLevelAndVersion(level, version);

        var toAdd = versionBitCount - bitListWithHeader.Length;
        bitListWithHeader.Length += toAdd;

        var byte1 = QrPayloadEncoder.FillerByteList[0];
        var byte2 = QrPayloadEncoder.FillerByteList[1];

        
        for (var i = totalBitCount; i < bitListWithHeader.Length;)
        {
            if (i % 2 > 0)
            {
                var c = 0;
                while (c < 8)
                {
                    bitListWithHeader[i] = byte2[c];
                    i++;
                    c++;
                }
            }
            else
            {
                var c = 0;
                while (c < 8)
                {
                    bitListWithHeader[i] = byte1[c];
                    i++;
                    c++;
                }
            }
        }

        var levelBlockCount = QrPayloadEncoder.BlockCountPerLevelAndVersion[level][version];
        
        if (levelBlockCount > 1)
        {
            
        }

        return Ok();
    }

    /// <summary>
    /// Генерация штрих-кода
    /// </summary>
    [HttpPost("lib")]
    public IActionResult GenerateBarcodeViaLib([FromBody] GenerateCodeRequest input)
    {
        var codeByteList = input.CodeType switch
        {
            CodeType.Qr => _serviceProvider.GetRequiredKeyedService<BarcodeWriter<Bitmap>>(DiKeyConstants.QrKey)
                .Write(input.Input),
            CodeType.Barcode => _serviceProvider
                .GetRequiredKeyedService<BarcodeWriter<Bitmap>>(DiKeyConstants.BarcodeKey).Write(input.Input),
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