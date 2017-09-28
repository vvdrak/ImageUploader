using Prism.Commands;

namespace ImageUploader.Helpers
{
    /// <summary>
    /// Класс инкапсулирующий в себе глобальную команду
    /// </summary>
    public static class GlobalCommands
    {
        /// <summary>
        /// Запуск всех загрузок
        /// </summary>
        public static CompositeCommand StartAllCommand = new CompositeCommand();
    }
}
