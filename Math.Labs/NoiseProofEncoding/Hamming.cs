using System.Text;

public class Hamming
{
    private static readonly Dictionary<char, string> _asciiToBinary = new()
    {
            { ' ', "00100000" }, { '!', "00100001" }, { '\"', "00100010" }, { '#', "00100011" },
            { '$', "00100100" }, { '%', "00100101" }, { '&', "00100110" }, { '\'', "00100111" },
            { '(', "00101000" }, { ')', "00101001" }, { '*', "00101010" }, { '+', "00101011" },
            { ',', "00101100" }, { '-', "00101101" }, { '.', "00101110" }, { '/', "00101111" },
            { '0', "00110000" }, { '1', "00110001" }, { '2', "00110010" }, { '3', "00110011" },
            { '4', "00110100" }, { '5', "00110101" }, { '6', "00110110" }, { '7', "00110111" },
            { '8', "00111000" }, { '9', "00111001" }, { ':', "00111010" }, { ';', "00111011" },
            { '<', "00111100" }, { '=', "00111101" }, { '>', "00111110" }, { '?', "00111111" },
            { '@', "01000000" }, { 'A', "01000001" }, { 'B', "01000010" }, { 'C', "01000011" },
            { 'D', "01000100" }, { 'E', "01000101" }, { 'F', "01000110" }, { 'G', "01000111" },
            { 'H', "01001000" }, { 'I', "01001001" }, { 'J', "01001010" }, { 'K', "01001011" },
            { 'L', "01001100" }, { 'M', "01001101" }, { 'N', "01001110" }, { 'O', "01001111" },
            { 'P', "01010000" }, { 'Q', "01010001" }, { 'R', "01010010" }, { 'S', "01010011" },
            { 'T', "01010100" }, { 'U', "01010101" }, { 'V', "01010110" }, { 'W', "01010111" },
            { 'X', "01011000" }, { 'Y', "01011001" }, { 'Z', "01011010" }, { '[', "01011011" },
            { '\\', "01011100" }, { ']', "01011101" }, { '^', "01011110" }, { '_', "01011111" },
            { '`', "01100000" }, { 'a', "01100001" }, { 'b', "01100010" }, { 'c', "01100011" },
            { 'd', "01100100" }, { 'e', "01100101" }, { 'f', "01100110" }, { 'g', "01100111" },
            { 'h', "01101000" }, { 'i', "01101001" }, { 'j', "01101010" }, { 'k', "01101011" },
            { 'l', "01101100" }, { 'm', "01101101" }, { 'n', "01101110" }, { 'o', "01101111" },
            { 'p', "01110000" }, { 'q', "01110001" }, { 'r', "01110010" }, { 's', "01110011" },
            { 't', "01110100" }, { 'u', "01110101" }, { 'v', "01110110" }, { 'w', "01110111" },
            { 'x', "01111000" }, { 'y', "01111001" }, { 'z', "01111010" }, { '{', "01111011" },
            { '|', "01111100" }, { '}', "01111101" }, { '~', "01111110" }
        };

    private static readonly Dictionary<string, char> _binaryToAscii = new()
    {
        { "00100000", ' ' }, { "00100001", '!' }, { "00100010", '\"' }, { "00100011", '#' },
        { "00100100", '$' }, { "00100101", '%' }, { "00100110", '&' }, { "00100111", '\'' },
        { "00101000", '(' }, { "00101001", ')' }, { "00101010", '*' }, { "00101011", '+' },
        { "00101100", ',' }, { "00101101", '-' }, { "00101110", '.' }, { "00101111", '/' },
        { "00110000", '0' }, { "00110001", '1' }, { "00110010", '2' }, { "00110011", '3' },
        { "00110100", '4' }, { "00110101", '5' }, { "00110110", '6' }, { "00110111", '7' },
        { "00111000", '8' }, { "00111001", '9' }, { "00111010", ':' }, { "00111011", ';' },
        { "00111100", '<' }, { "00111101", '=' }, { "00111110", '>' }, { "00111111", '?' },
        { "01000000", '@' }, { "01000001", 'A' }, { "01000010", 'B' }, { "01000011", 'C' },
        { "01000100", 'D' }, { "01000101", 'E' }, { "01000110", 'F' }, { "01000111", 'G' },
        { "01001000", 'H' }, { "01001001", 'I' }, { "01001010", 'J' }, { "01001011", 'K' },
        { "01001100", 'L' }, { "01001101", 'M' }, { "01001110", 'N' }, { "01001111", 'O' },
        { "01010000", 'P' }, { "01010001", 'Q' }, { "01010010", 'R' }, { "01010011", 'S' },
        { "01010100", 'T' }, { "01010101", 'U' }, { "01010110", 'V' }, { "01010111", 'W' },
        { "01011000", 'X' }, { "01011001", 'Y' }, { "01011010", 'Z' }, { "01011011", '[' },
        { "01011100", '\\' }, { "01011101", ']' }, { "01011110", '^' }, { "01011111", '_' },
        { "01100000", '`' }, { "01100001", 'a' }, { "01100010", 'b' }, { "01100011", 'c' },
        { "01100100", 'd' }, { "01100101", 'e' }, { "01100110", 'f' }, { "01100111", 'g' },
        { "01101000", 'h' }, { "01101001", 'i' }, { "01101010", 'j' }, { "01101011", 'k' },
        { "01101100", 'l' }, { "01101101", 'm' }, { "01101110", 'n' }, { "01101111", 'o' },
        { "01110000", 'p' }, { "01110001", 'q' }, { "01110010", 'r' }, { "01110011", 's' },
        { "01110100", 't' }, { "01110101", 'u' }, { "01110110", 'v' }, { "01110111", 'w' },
        { "01111000", 'x' }, { "01111001", 'y' }, { "01111010", 'z' }, { "01111011", '{' },
        { "01111100", '|' }, { "01111101", '}' }, { "01111110", '~' }
    };
    
    /// <summary>
    /// Метод для демонстрации
    /// </summary>
    public static void Execute(string message)
    {
        var encodedMessage = Encode(message, 16);
        var messageWithErrors = Noisify(encodedMessage);
        Decode(messageWithErrors);
    }

    // Функция кодирования сообщения
    public static List<string> Encode(string message, int length)
    {
        var encodedBlocks = new List<string>();

        if (length % 8 != 0)
        {
            Console.WriteLine($"Invalid argument {length}");
            return encodedBlocks;
        }

        var messageBlocks = SplitByBlocks(message, length / 8);
        messageBlocks = ConvertToBinary(messageBlocks);

        foreach (var block in messageBlocks)
        {
            encodedBlocks.Add(InsertControlBits(block));
        }

        Console.WriteLine($"Message '{message}' encoded as: ");
        foreach (var block in encodedBlocks) Console.WriteLine(block);

        return encodedBlocks;
    }

    private static List<string> SplitByBlocks(string message, int charsPerBlock)
    {
        return message
            .Chunk(charsPerBlock)
            .Select(x => new string(x))
            .ToList();
    }

    /// <summary>
    /// Преобразовать набор блоков в соответствующие бинарное представление таковых
    /// </summary>
    private static List<string> ConvertToBinary(List<string> messageBlocks)
    {
        return messageBlocks
            .Select(x => string.Join(string.Empty, x.Select(y => _asciiToBinary[y])))
            .ToList();
    }

    /// <summary>
    /// Вставить в двоичное сообщение контрольные биты
    /// </summary>
    private static string InsertControlBits(string message)
    {
        var controlBitsPos = GetControlBitsPositionList(message);

        var messageCharList = new List<char>(message);

        // Вставка контрольных битов со значением по умолчанию
        foreach (var bitPos in controlBitsPos)
        {
            messageCharList.Insert(bitPos, '0');
        }

        // Установка настоящего значения контрольных битов
        foreach (var bitPos in controlBitsPos)
        {
            messageCharList[bitPos] = CalculateBitValue(messageCharList, bitPos);
        }

        return new string(messageCharList.ToArray());
    }

    /// <summary>
    /// Вычислить значение контрольного бита
    /// </summary>
    private static char CalculateBitValue(List<char> message, int bitPos)
    {
        var onesCount = 0;

        // Проверка битов, которые контролируются текущим контрольным битом
        for (var i = 0; i < message.Count; i++)
        {
            // Проверка, находится ли бит в области контроля контрольного бита
            if (((i + 1) & (bitPos + 1)) != 0)
                onesCount += message[i] == '1' ? 1 : 0;
        }

        return onesCount % 2 == 0 ? '0' : '1';
    }

    /// <summary>
    /// Зашумить блоки сообщения рандомными значениями
    /// </summary>
    public static List<string> Noisify(List<string> message)
    {
        var result = new List<string>();
        var rand = new Random();

        foreach (var messageBlock in message)
        {
            var a = rand.NextDouble();
            if (a <= 0.5)
            {
                result.Add(messageBlock);
            }
            else
            {
                var randomIndex = rand.Next(0, messageBlock.Length);
                var messageChars = messageBlock.ToCharArray();
                messageChars[randomIndex] = messageChars[randomIndex] == '0' ? '1' : '0';
                result.Add(new string(messageChars));
            }
        }

        return result;
    }

    // Декодирование сообщения с учетом ошибок
    public static string Decode(List<string> message)
    {
        var sb = new StringBuilder();
        foreach (var messageBlock in message)
        {
            var decodedBlock = AnalyzeAndDecodeCore(messageBlock);
            sb.Append(decodedBlock);
        }

        return sb.ToString();
    }

    // Метод сравнения и исправления ошибок в блоках
    private static string AnalyzeAndDecodeCore(string messageBlock)
    {
        // Получение контрольных позиций и вычисление битов
        var controlBitsPos = GetControlBitsPositionList(messageBlock);
        var recalculatedMessageBlock = RecalculateBits(messageBlock);

        var mistakeIndex = -1;
        var hasMistake = false;

        for (var i = 0; i < controlBitsPos.Count; i++)
        {
            var bitPos = controlBitsPos[i];
            if (messageBlock[bitPos] != recalculatedMessageBlock[bitPos])
            {
                hasMistake = true;
                mistakeIndex += bitPos + 1;
            }
        }

        // В случае, если нашли ошибку при анализе, не забываем ее передать дальше. 
        if (hasMistake)
        {
            return DecodeBlock(recalculatedMessageBlock, controlBitsPos, mistakeIndex);
        }

        return DecodeBlock(messageBlock, controlBitsPos);
    }

    /// <summary>
    /// Получить список позиций контрольных битов
    /// </summary>
    /// <remarks>Контрольные биты стоят на позициях, соответствующих степеням двойки</remarks>
    private static List<int> GetControlBitsPositionList(string messageBlock)
    {
        var pow = (int)System.Math.Floor(System.Math.Log2(messageBlock.Length));
        var controlBitsPos = new List<int>();

        for (var i = 0; i <= pow; i++) controlBitsPos.Add((int)System.Math.Pow(2, i) - 1);

        return controlBitsPos;
    }

    // Пересчитываем биты (с учетом исправлений контрольных битов)
    private static string RecalculateBits(string messageBlock)
    {
        var controlBitsPos = GetControlBitsPositionList(messageBlock);
        var message = new List<char>(messageBlock);

        foreach (var bitPos in controlBitsPos)
        {
            message[bitPos] = '0';
        }

        foreach (var bitPos in controlBitsPos)
        {
            message[bitPos] = CalculateBitValue(message, bitPos);
        }

        return new string(message.ToArray());
    }

    // Декодирование блока (после возможных исправлений)
    private static string DecodeBlock(string messageBlock, List<int> controlBitPositionList, int mistakeIndex = -1)
    {
        var result = "";
        var blockCharList = messageBlock.ToCharArray();
        
        // Восстанавливаем сообщение
        if (mistakeIndex > -1)
        {
            if (blockCharList[mistakeIndex] == '0')
                blockCharList[mistakeIndex] = '1';
            else
                blockCharList[mistakeIndex] = '0';
        }

        // После восстановления контрольные биты нам больше не нужны, поэтому удаляем
        messageBlock = ExcludeControlBits(blockCharList, controlBitPositionList);
        
        // 
        // messageBlock = string.Join(string.Empty, messageBlock);
        var count = messageBlock.Length / 8;

        for (var i = 0; i < count; i++)
        {
            var charBinary = messageBlock[(i * 8)..((i + 1) * 8)];
            var actualChar = _binaryToAscii[charBinary];
            result += actualChar;
        }

        return result;
    }

    /// <summary>
    /// Исключить из информационного слова контрольные биты
    /// </summary>
    private static string ExcludeControlBits(char[] block, List<int> controlBitPositionList)
    {
        var result = new List<char>();
        var index = 0;

        foreach (var bit in block)
        {
            if (controlBitPositionList.Contains(index))
            {
                index++;
                continue;
            }

            result.Add(bit);
            index++;
        }

        return string.Join(string.Empty, result);
    }

    // Точка входа программы
    public static void Main()
    {
        var message = "volodya";
        Execute(message);
    }
}


// using System.Text;
//
// public class Hamming
// {
//     private static readonly Dictionary<char, string> _asciiToBinary = new()
//     {
//             { ' ', "00100000" }, { '!', "00100001" }, { '\"', "00100010" }, { '#', "00100011" },
//             { '$', "00100100" }, { '%', "00100101" }, { '&', "00100110" }, { '\'', "00100111" },
//             { '(', "00101000" }, { ')', "00101001" }, { '*', "00101010" }, { '+', "00101011" },
//             { ',', "00101100" }, { '-', "00101101" }, { '.', "00101110" }, { '/', "00101111" },
//             { '0', "00110000" }, { '1', "00110001" }, { '2', "00110010" }, { '3', "00110011" },
//             { '4', "00110100" }, { '5', "00110101" }, { '6', "00110110" }, { '7', "00110111" },
//             { '8', "00111000" }, { '9', "00111001" }, { ':', "00111010" }, { ';', "00111011" },
//             { '<', "00111100" }, { '=', "00111101" }, { '>', "00111110" }, { '?', "00111111" },
//             { '@', "01000000" }, { 'A', "01000001" }, { 'B', "01000010" }, { 'C', "01000011" },
//             { 'D', "01000100" }, { 'E', "01000101" }, { 'F', "01000110" }, { 'G', "01000111" },
//             { 'H', "01001000" }, { 'I', "01001001" }, { 'J', "01001010" }, { 'K', "01001011" },
//             { 'L', "01001100" }, { 'M', "01001101" }, { 'N', "01001110" }, { 'O', "01001111" },
//             { 'P', "01010000" }, { 'Q', "01010001" }, { 'R', "01010010" }, { 'S', "01010011" },
//             { 'T', "01010100" }, { 'U', "01010101" }, { 'V', "01010110" }, { 'W', "01010111" },
//             { 'X', "01011000" }, { 'Y', "01011001" }, { 'Z', "01011010" }, { '[', "01011011" },
//             { '\\', "01011100" }, { ']', "01011101" }, { '^', "01011110" }, { '_', "01011111" },
//             { '`', "01100000" }, { 'a', "01100001" }, { 'b', "01100010" }, { 'c', "01100011" },
//             { 'd', "01100100" }, { 'e', "01100101" }, { 'f', "01100110" }, { 'g', "01100111" },
//             { 'h', "01101000" }, { 'i', "01101001" }, { 'j', "01101010" }, { 'k', "01101011" },
//             { 'l', "01101100" }, { 'm', "01101101" }, { 'n', "01101110" }, { 'o', "01101111" },
//             { 'p', "01110000" }, { 'q', "01110001" }, { 'r', "01110010" }, { 's', "01110011" },
//             { 't', "01110100" }, { 'u', "01110101" }, { 'v', "01110110" }, { 'w', "01110111" },
//             { 'x', "01111000" }, { 'y', "01111001" }, { 'z', "01111010" }, { '{', "01111011" },
//             { '|', "01111100" }, { '}', "01111101" }, { '~', "01111110" }
//         };
//
//     private static readonly Dictionary<string, char> _binaryToAscii = new()
//     {
//         { "00100000", ' ' }, { "00100001", '!' }, { "00100010", '\"' }, { "00100011", '#' },
//         { "00100100", '$' }, { "00100101", '%' }, { "00100110", '&' }, { "00100111", '\'' },
//         { "00101000", '(' }, { "00101001", ')' }, { "00101010", '*' }, { "00101011", '+' },
//         { "00101100", ',' }, { "00101101", '-' }, { "00101110", '.' }, { "00101111", '/' },
//         { "00110000", '0' }, { "00110001", '1' }, { "00110010", '2' }, { "00110011", '3' },
//         { "00110100", '4' }, { "00110101", '5' }, { "00110110", '6' }, { "00110111", '7' },
//         { "00111000", '8' }, { "00111001", '9' }, { "00111010", ':' }, { "00111011", ';' },
//         { "00111100", '<' }, { "00111101", '=' }, { "00111110", '>' }, { "00111111", '?' },
//         { "01000000", '@' }, { "01000001", 'A' }, { "01000010", 'B' }, { "01000011", 'C' },
//         { "01000100", 'D' }, { "01000101", 'E' }, { "01000110", 'F' }, { "01000111", 'G' },
//         { "01001000", 'H' }, { "01001001", 'I' }, { "01001010", 'J' }, { "01001011", 'K' },
//         { "01001100", 'L' }, { "01001101", 'M' }, { "01001110", 'N' }, { "01001111", 'O' },
//         { "01010000", 'P' }, { "01010001", 'Q' }, { "01010010", 'R' }, { "01010011", 'S' },
//         { "01010100", 'T' }, { "01010101", 'U' }, { "01010110", 'V' }, { "01010111", 'W' },
//         { "01011000", 'X' }, { "01011001", 'Y' }, { "01011010", 'Z' }, { "01011011", '[' },
//         { "01011100", '\\' }, { "01011101", ']' }, { "01011110", '^' }, { "01011111", '_' },
//         { "01100000", '`' }, { "01100001", 'a' }, { "01100010", 'b' }, { "01100011", 'c' },
//         { "01100100", 'd' }, { "01100101", 'e' }, { "01100110", 'f' }, { "01100111", 'g' },
//         { "01101000", 'h' }, { "01101001", 'i' }, { "01101010", 'j' }, { "01101011", 'k' },
//         { "01101100", 'l' }, { "01101101", 'm' }, { "01101110", 'n' }, { "01101111", 'o' },
//         { "01110000", 'p' }, { "01110001", 'q' }, { "01110010", 'r' }, { "01110011", 's' },
//         { "01110100", 't' }, { "01110101", 'u' }, { "01110110", 'v' }, { "01110111", 'w' },
//         { "01111000", 'x' }, { "01111001", 'y' }, { "01111010", 'z' }, { "01111011", '{' },
//         { "01111100", '|' }, { "01111101", '}' }, { "01111110", '~' }
//     };
//     
//     /// <summary>
//     /// Метод для демонстрации
//     /// </summary>
//     // public static void Execute(string message)
//     // {
//     //     var encodedMessage = Encode(message, 16);
//     //     var messageWithErrors = Noisify(encodedMessage);
//     //     Decode(messageWithErrors);
//     // }
//
//     // Функция кодирования сообщения
//     public static string Encode(string message, int length)
//     {
//         var encodedBlocks = new List<string>();
//
//         if (length % 8 != 0)
//         {
//             Console.WriteLine($"Invalid argument {length}");
//             throw new Exception();
//         }
//
//         var messageBlocks = SplitByBlocks(message, length / 8);
//         messageBlocks = ConvertToBinary(messageBlocks);
//
//         foreach (var block in messageBlocks)
//         {
//             encodedBlocks.Add(InsertControlBits(block));
//         }
//
//         Console.WriteLine($"Message '{message}' encoded as: ");
//         foreach (var block in encodedBlocks) Console.WriteLine(block);
//
//         return string.Join(string.Empty, encodedBlocks);
//     }
//
//     private static List<string> SplitByBlocks(string message, int charsPerBlock)
//     {
//         return message
//             .Chunk(charsPerBlock)
//             .Select(x => new string(x))
//             .ToList();
//     }
//
//     /// <summary>
//     /// Преобразовать набор блоков в соответствующие бинарное представление таковых
//     /// </summary>
//     private static List<string> ConvertToBinary(List<string> messageBlocks)
//     {
//         return messageBlocks
//             .Select(x => string.Join(string.Empty, x.Select(y => _asciiToBinary[y])))
//             .ToList();
//     }
//
//     /// <summary>
//     /// Вставить в двоичное сообщение контрольные биты
//     /// </summary>
//     private static string InsertControlBits(string message)
//     {
//         var controlBitsPos = GetControlBitsPositionList(message);
//
//         var messageCharList = new List<char>(message);
//
//         // Вставка контрольных битов со значением по умолчанию
//         foreach (var bitPos in controlBitsPos)
//         {
//             messageCharList.Insert(bitPos, '0');
//         }
//
//         // Установка настоящего значения контрольных битов
//         foreach (var bitPos in controlBitsPos)
//         {
//             messageCharList[bitPos] = CalculateBitValue(messageCharList, bitPos);
//         }
//
//         return new string(messageCharList.ToArray());
//     }
//
//     /// <summary>
//     /// Вычислить значение контрольного бита
//     /// </summary>
//     private static char CalculateBitValue(List<char> message, int bitPos)
//     {
//         var onesCount = 0;
//
//         // Проверка битов, которые контролируются текущим контрольным битом
//         for (var i = 0; i < message.Count; i++)
//         {
//             // Проверка, находится ли бит в области контроля контрольного бита
//             if (((i + 1) & (bitPos + 1)) != 0)
//                 onesCount += message[i] == '1' ? 1 : 0;
//         }
//
//         return onesCount % 2 == 0 ? '0' : '1';
//     }
//
//     /// <summary>
//     /// Зашумить блоки сообщения рандомными значениями
//     /// </summary>
//     public static string Noisify(string message)
//     {
//         var result = new StringBuilder();
//         var rand = new Random();
//         
//         foreach (var messageBlock in message.Chunk(16))
//         {
//             var str = new string(messageBlock);
//             var a = rand.NextDouble();
//             if (a <= 0.5)
//             {
//                 result.Append(str);
//             }
//             else
//             {
//                 var randomIndex = rand.Next(0, messageBlock.Length);
//                 messageBlock[randomIndex] = messageBlock[randomIndex] == '0' ? '1' : '0';
//                 result.Append(messageBlock);
//             }
//         }
//
//         return result.ToString();
//     }
//
//     /// <summary>
//     /// Декодировать сообщение
//     /// </summary>
//     public static string Decode(List<string> message)
//     {
//         var sb = new StringBuilder();
//         foreach (var messageBlock in message)
//         {
//             var decodedBlock = AnalyzeAndDecodeCore(messageBlock);
//             sb.Append(decodedBlock);
//         }
//
//         return sb.ToString();
//     }
//
//     /// <summary>
//     /// Непосредственно логика анализа, восстановления и декодирования
//     /// </summary>
//     private static string AnalyzeAndDecodeCore(string messageBlock)
//     {
//         // Получение контрольных позиций и вычисление битов
//         var controlBitsPos = GetControlBitsPositionList(messageBlock);
//         var recalculatedMessageBlock = RecalculateBits(messageBlock);
//
//         var mistakeIndex = -1;
//         var hasMistake = false;
//
//         for (var i = 0; i < controlBitsPos.Count; i++)
//         {
//             var bitPos = controlBitsPos[i];
//             if (messageBlock[bitPos] != recalculatedMessageBlock[bitPos])
//             {
//                 hasMistake = true;
//                 mistakeIndex += bitPos + 1;
//             }
//         }
//
//         // В случае, если нашли ошибку при анализе, не забываем ее передать дальше. 
//         if (hasMistake)
//         {
//             return DecodeBlock(recalculatedMessageBlock, controlBitsPos, mistakeIndex);
//         }
//
//         return DecodeBlock(messageBlock, controlBitsPos);
//     }
//
//     /// <summary>
//     /// Получить список позиций контрольных битов
//     /// </summary>
//     /// <remarks>Контрольные биты стоят на позициях, соответствующих степеням двойки</remarks>
//     private static List<int> GetControlBitsPositionList(string messageBlock)
//     {
//         var pow = (int)System.Math.Floor(System.Math.Log2(messageBlock.Length));
//         var controlBitsPos = new List<int>();
//
//         for (var i = 0; i <= pow; i++) controlBitsPos.Add((int)System.Math.Pow(2, i) - 1);
//
//         return controlBitsPos;
//     }
//
//     // Пересчитываем биты (с учетом исправлений контрольных битов)
//     private static string RecalculateBits(string messageBlock)
//     {
//         var controlBitsPos = GetControlBitsPositionList(messageBlock);
//         var message = new List<char>(messageBlock);
//
//         foreach (var bitPos in controlBitsPos)
//         {
//             message[bitPos] = '0';
//         }
//
//         foreach (var bitPos in controlBitsPos)
//         {
//             message[bitPos] = CalculateBitValue(message, bitPos);
//         }
//
//         return new string(message.ToArray());
//     }
//
//     // Декодирование блока (после возможных исправлений)
//     private static string DecodeBlock(string messageBlock, List<int> controlBitPositionList, int mistakeIndex = -1)
//     {
//         var result = "";
//         var blockCharList = messageBlock.ToCharArray();
//         
//         // Восстанавливаем сообщение
//         if (mistakeIndex > -1)
//         {
//             if (blockCharList[mistakeIndex] == '0')
//                 blockCharList[mistakeIndex] = '1';
//             else
//                 blockCharList[mistakeIndex] = '0';
//         }
//
//         // После восстановления контрольные биты нам больше не нужны, поэтому удаляем
//         messageBlock = ExcludeControlBits(blockCharList, controlBitPositionList);
//         
//         var count = messageBlock.Length / 8;
//
//         for (var i = 0; i < count; i++)
//         {
//             var charBinary = messageBlock[(i * 8)..((i + 1) * 8)];
//             var actualChar = _binaryToAscii[charBinary];
//             result += actualChar;
//         }
//
//         return result;
//     }
//
//     /// <summary>
//     /// Исключить из информационного слова контрольные биты
//     /// </summary>
//     private static string ExcludeControlBits(char[] block, List<int> controlBitPositionList)
//     {
//         var result = new List<char>();
//         var index = 0;
//
//         foreach (var bit in block)
//         {
//             if (controlBitPositionList.Contains(index))
//             {
//                 index++;
//                 continue;
//             }
//
//             result.Add(bit);
//             index++;
//         }
//
//         return string.Join(string.Empty, result);
//     }
//
//     // // Точка входа программы
//     // public static void Main()
//     // {
//     //     var message = "volodya";
//     //     Execute(message);
//     // }
// }