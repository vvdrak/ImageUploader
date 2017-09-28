using Prism.Events;
using System;

namespace ImageUploader.Events
{
    /// <summary>
    /// Событие обновления Progressbar
    /// </summary>
    public sealed class PbValueEvent : PubSubEvent<Tuple<int, double, double>> { }
}
