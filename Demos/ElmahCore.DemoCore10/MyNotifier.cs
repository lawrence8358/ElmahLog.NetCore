using System;
using System.Diagnostics;
using ElmahCore;

namespace ElmahCore.DemoCore10;

public class MyNotifier : IErrorNotifier
{
    public void Notify(Error error) => Debug.WriteLine(error.Message);
    public string Name => "my";
}

public class MyNotifierWithId : IErrorNotifierWithId
{
    public void Notify(Error error) => throw new NotImplementedException();
    public void Notify(string id, Error error) => Debug.WriteLine(error.Message);
    public string Name => "myWithId";
}