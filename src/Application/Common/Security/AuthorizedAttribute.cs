namespace Application.Common.Security;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class AuthorizedAttribute : Attribute
{
    public AuthorizedAttribute() { }
}
