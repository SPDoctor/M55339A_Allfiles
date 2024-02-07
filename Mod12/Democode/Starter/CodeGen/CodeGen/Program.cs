using System.CodeDom;
using System.CodeDom.Compiler;

Console.WriteLine("Creating CodeDOM objects...");
var unit = new CodeCompileUnit();
var dynamicNamespace = new CodeNamespace("HelloWorld");
unit.Namespaces.Add(dynamicNamespace);
dynamicNamespace.Imports.Add(new CodeNamespaceImport("System"));
var programType = new CodeTypeDeclaration("Program");
dynamicNamespace.Types.Add(programType);
var mainMethod = new CodeEntryPointMethod();
programType.Members.Add(mainMethod);
var expression = new CodeMethodInvokeExpression(
    new CodeTypeReferenceExpression("Console"),
    "WriteLine", new CodePrimitiveExpression("Hello, World!"));
mainMethod.Statements.Add(expression);

Console.WriteLine("Generating C# source code...");
var provider = CodeDomProvider.CreateProvider("CSharp");
var fileName = @"..\..\..\HelloWorld.cs";
var stream = new StreamWriter(fileName);
var textWriter = new IndentedTextWriter(stream);
var options = new CodeGeneratorOptions();
options.BlankLinesBetweenMembers = true;
provider.GenerateCodeFromCompileUnit(unit, textWriter, options);
textWriter.Close();
stream.Close();


Console.WriteLine("Generating VB source code...");
provider = CodeDomProvider.CreateProvider("VB");
fileName = @"..\..\..\HelloWorld.vb";
stream = new StreamWriter(fileName);
textWriter = new IndentedTextWriter(stream);
options = new CodeGeneratorOptions();
options.BlankLinesBetweenMembers = true;
provider.GenerateCodeFromCompileUnit(unit, textWriter, options);
textWriter.Close();
stream.Close();
