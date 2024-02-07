// dynamic vs. statically typed variables
dynamic d = 1;  // type: dynamic {int}
var testSum = d + 3;  // type: dynamic {int}
d = 2.0; // type: dynamic {double}
var i = 2; // type: int
d = "value of i = " + i; // type: dynamic {string}
Console.WriteLine(d);

// Using the Word COM interop assembly with dynamic objects
dynamic word = new Microsoft.Office.Interop.Word.Application();
word.Visible = true;
dynamic doc = word.Documents.Add();
doc.Activate();
Write("Using Dynamic Variables", true);
NewLine();
NewLine();
Write("This document was created using Word automation from C# language code.", false);
NewLine();
doc.SaveAs("testdocument.docx");
doc.Close();
word.Quit();

void Write(string text, bool bold)
{
    var endOfDocument = word.ActiveDocument.Content.End - 1;
    var currentLocation = word.ActiveDocument.Range(endOfDocument);
    currentLocation.Bold = bold ? 1 : 0;
    currentLocation.InsertAfter(text);
}

void NewLine()
{
    var endOfDocument = word.ActiveDocument.Content.End - 1;
    var currentLocation = word.ActiveDocument.Range(endOfDocument);
    currentLocation.InsertBreak(Microsoft.Office.Interop.Word.WdBreakType.wdLineBreak);
}
