var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// (Warstwa poœrednia) Middleware
app.Use((context, next) =>
{
    if (context.Request.Path == "/api/products")
    {
        // TODO: forward request to the http://address1 endpoint of the Product API

        context.Response.Redirect("https://localhost:7291/api/products");

        return Task.CompletedTask;

    }
    else if (context.Request.Path == "/api/cart/items")
    {
        // TODO: forward request to the http://address2 endpoint of the Cart Items API
        context.Response.Redirect("https://localhost:7285/api/cart/items");

        return Task.CompletedTask;
    }



    return next();
});

app.MapGet("/", () => "Hello ApiGateway!");

app.MapGet("/ping", () => "Pong!");

app.Run();
