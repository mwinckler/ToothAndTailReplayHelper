namespace ToothAndTailReplayHelper.View
{
    internal interface ITrayNotifier
    {
        void Notify(string message);

        void Notify(string title, string message);
    }
}