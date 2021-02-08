namespace QQRobot.Attribute
{
    internal class CommandParameterAttribute: System.Attribute
    {
        public string Name { get; private set; }

        public CommandParameterAttribute(string name)
        {
            Name = name;
        }
    }
}