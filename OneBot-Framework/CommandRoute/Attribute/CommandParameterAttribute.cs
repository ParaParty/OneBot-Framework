namespace QQRobot.Attribute
{
    /// <summary>
    /// 指令参数绑定
    /// </summary>
    internal class CommandParameterAttribute: System.Attribute
    {
        /// <summary>
        /// 参数名
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 指令参数绑定
        /// </summary>
        /// <param name="name">参数名</param>
        public CommandParameterAttribute(string name)
        {
            Name = name;
        }
    }
}