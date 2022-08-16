// See https://aka.ms/new-console-template for more information
using EFTest.Data;
using EFTest.Domains;
using EFTest.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Text.Json;

Console.WriteLine("Hello, World!");

JsonSerializerOptions options = new JsonSerializerOptions
{
    ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
};

DataContext db = new DataContext();
ProductTypeService service = new ProductTypeService(new ProductTypeRepository(db), db);

db.ProductTypes.RemoveRange(db.ProductTypes.ToList());
db.SaveChanges();

var header = new EFTest.Entities.ProductType
{
    Name = "Default"
};

header.AddAttribute(new EFTest.Entities.ProductAttribute
{
    Name = "Size"
});

var colorAttr = new EFTest.Entities.ProductAttribute
{
    Name = "Color2"
};

header.AddAttribute(colorAttr);

await service.Add(header);

Console.WriteLine("First");
Console.WriteLine(JsonSerializer.Serialize(db.ProductTypes.Include(e => e.Attributes).ToList(), options));

header.AddAttribute(new EFTest.Entities.ProductAttribute
{
    Name = "Color"
});

header.RemoveAttribute(colorAttr);

header.Attributes.First().Name = "First";

try
{
    await service.MakeTransaction(async () =>
    {
        header.Name += "2";
        await service.Update(header);

        await service.Add(new EFTest.Entities.ProductType
        {
            Name = "Yoo"
        });
        throw new Exception();
    });
}
catch
{
    Console.WriteLine("Rollback");
}

/*await service.Add(new EFTest.Entities.ProductType
{
    Name = "Second"
});
service.CommitTransaction();*/

Console.WriteLine("Second");
Console.WriteLine(JsonSerializer.Serialize(new DataContext().ProductTypes.Include(e => e.Attributes).ToList(), options));

Console.ReadLine();