using System.Numerics;
using Zero_KnowledgeProof;

Print("Упрощённый протокол идентификации с нулевой передачей данных", ConsoleColor.Yellow);

var center = new Center(); // B
var client = new Client(0); // A

Console.Write($"\nВведите количество циклов: "); var t_size = int.Parse(Console.ReadLine());

var result = true;

for (int t = 0; t < t_size; t++)
{
    Print($"\nЦикл идентификации ({t})");
    
    var x = client.GetX(center.n); Console.WriteLine($"[Отправка значения стороне B] x: {x}"); // Сторона А выбирает некоторое случайное число r, r < n. Затем она вычисляет x = r2 mod n и отправляет х стороне В.
    center.SetX(x);
    var bit = center.Ping(); Console.Write($"[Отправка значения стороне A] "); Print($"Случайный бит: {bit}", ConsoleColor.Cyan);// Сторона В посылает А случайный бит b.
    var ry = client.Pong(bit, center.n);
    var verfiy = center.Check(ry); 
    
    Console.Write($"[Проверка] ");
    if (verfiy)
        Print("Подтверждено", ConsoleColor.Green);
    else
        Print("Не подтверждено", ConsoleColor.Red);

    result &= verfiy;
}

Print("\nРезультат идентификации стороны А: ", newLine: false);

if (result)
    Print("Подтверждено", ConsoleColor.Green);
else
    Print("Не подтверждено", ConsoleColor.Red);

Console.ReadKey();

void Print(string mess, ConsoleColor color = ConsoleColor.Yellow, bool newLine = true)
{
    Console.ForegroundColor = color;

    if (newLine) Console.WriteLine(mess);
    else Console.Write(mess);

    Console.ResetColor();
}




