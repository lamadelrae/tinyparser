using TinyParser.Core.Generators.LINQ;

var expression = LambdaFactory.Produce<Person>("(Age:lt:18)");

var list = new List<Person>
{
    new Person () { Name = "Matthew", Age = 18 },
    new Person () { Name = "Teste", Age = 10 }
};

var result = list.Where(expression);

foreach (var person in result) Console.WriteLine(person.Name);

public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
}
