namespace Math.Code.QrBarcode.ManualImplementation.CorrectionLevel;

public enum QrErrorCorrectionLevel
{
    // значение по умолчанию
    Unknown = 0,
    
    /// <summary>
    /// Допустимы повреждения до 7%
    /// </summary>
    Low = 1,
    
    /// <summary>
    /// Допустимы повреждения до 15%
    /// </summary>
    Medium = 2,
    
    /// <summary>
    /// Допустимы повреждения до 25%
    /// </summary>
    Quartile = 3,
    
    /// <summary>
    /// Допустимы повреждения до 30%
    /// </summary>
    High = 4
}