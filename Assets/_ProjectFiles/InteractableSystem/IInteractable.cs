public interface IInteractable
{
    string GetInteractText(); // Для UI типа "Е - поднять"
    void OnInteract(); // Краткое нажатие
    void OnHoldInteract(); // Удержание
    void OnStopInteract(); // Отпустили
}
