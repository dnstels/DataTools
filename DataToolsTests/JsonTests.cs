using System.Reflection;
using Newtonsoft.Json.Linq;
using Xunit.Abstractions;
using FluentAssertions;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace JsonToolsTests;

public class JsonTests
{
    private readonly ITestOutputHelper _outputConsole;
    public JsonTests(ITestOutputHelper output)
    {
        _outputConsole = output;
    }

    [Fact]
    public void Test1_JObject()
    {
        // string path = Directory.GetCurrentDirectory();
        var o1 = JObject.Parse(File.ReadAllText(@"Files\file.json"));
        
        var o2 = o1.Children();

        o2.Should().HaveCount(6);
        o2.Any(t=>t.Path == "Date").Should().BeTrue();
        o2.FirstOrDefault(t=>t.Path=="TemperatureCelsius")?.ToObject<int>().Should().Be(25);
    }

    [Fact]
    public void Test2_FromMicrosoft(){
        var fileName = @"Files\file.json";
        var jsonString = File.ReadAllText(fileName);

        WeatherForecast? weatherForecast = 
                JsonSerializer.Deserialize<WeatherForecast>(jsonString);

        _outputConsole.WriteLine($"Date: {weatherForecast?.Date}");
        _outputConsole.WriteLine($"TemperatureCelsius: {weatherForecast?.TemperatureCelsius}");
        _outputConsole.WriteLine($"Summary: {weatherForecast?.Summary}");
    }

    [Fact]
    public void Test3_LoopOfClassParametres(){
        var props = typeof(HighLowTemps).GetProperties();
        HighLowTemps highLowTemps= new();
        int[] v= [1,2];
        int i =0;
        foreach(var prop in props) {
            _outputConsole.WriteLine($"test context -> { prop.Name }");
            PropertySet(highLowTemps, prop.Name, v[i++]);
        }
        
        highLowTemps.High.Should().Be(v[0]);
        highLowTemps.Low.Should().Be(v[1]);
    }

    static void PropertySet(object obj, string propName, object value)
    {
        if( obj is null ||
            propName is null ||
            value is null) 
            return;
        Type t = obj.GetType();
        PropertyInfo? info = t.GetProperty(propName);
        if (info is null || !info.CanWrite)
            return;
        info.SetValue(obj, value, null);
    }
    static void PropertySetLooping(object p, string propName, object value)
    {
        Type t = p.GetType();
        foreach (PropertyInfo info in t.GetProperties())
        {
            if (info.Name == propName && info.CanWrite)
            {
                info.SetValue(p, value, null);
            }
        }
    }
}

public class WeatherForecast
{
    public DateTimeOffset Date { get; set; }
    public int TemperatureCelsius { get; set; }
    public string? Summary { get; set; }
    public string? SummaryField;
    public IList<DateTimeOffset>? DatesAvailable { get; set; }
    public Dictionary<string, HighLowTemps>? TemperatureRanges { get; set; }
    public string[]? SummaryWords { get; set; }
}

public class HighLowTemps
{
    public int High { get; set; }
    public int Low { get; set; }
}