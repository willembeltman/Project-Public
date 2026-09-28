Console.WriteLine("Hello, World!");


var list = Test();
await foreach (var item in list)
{

}
IAsyncEnumerable<string> Test()
{
    return Test2();
}
async IAsyncEnumerable<string> Test2()
{
    yield return "1";
    yield return "2";
    yield return "3";
}