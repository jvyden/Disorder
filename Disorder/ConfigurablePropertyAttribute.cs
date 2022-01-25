namespace Disorder; 

[AttributeUsage(AttributeTargets.Property)]
public class ConfigurablePropertyAttribute : Attribute {
    public string Name { get; set; }
    public bool IsPassword { get; set; }
    public ConfigurablePropertyAttribute(string name, bool isPassword = false) {
        this.Name = name;
        this.IsPassword = isPassword;
    }
}