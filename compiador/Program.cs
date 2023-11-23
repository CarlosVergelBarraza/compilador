using compiador.clases;
using compiador.processCompiler;
using compiador.Typos;

string input = "int chicos = 2; int chicas = 6;";
Lexer lexer = new Lexer(input);
List<Token> tokens = lexer.GetTokens();

Parser parser = new Parser(tokens);
parser.ParseProgram();

foreach (Token t in tokens)
{
    var stringAImprimir = $"Tipo: {t.Type}, Valor: {t.Value}";
    if (t.Type == TokenType.TipoDato) stringAImprimir = stringAImprimir + $" Accept:{t.typeIdentificator.ToString()}"; 
    Console.WriteLine(stringAImprimir);
}

foreach (var variable in parser.Variables)
{
    Console.WriteLine($"Variable: {variable.Key}, Tipo: {variable.Value.Key}, Valor: {variable.Value.Value}");
}