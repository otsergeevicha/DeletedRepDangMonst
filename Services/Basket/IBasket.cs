namespace Services.Basket
{
    public interface IBasket
    {
        bool IsEmpty { get; }
        void SpendBox();
    }
}