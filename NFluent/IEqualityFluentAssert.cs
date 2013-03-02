namespace NFluent
{
    public interface IEqualityFluentAssert
    {
        void IsEqualTo(object expected);

        void IsNotEqualTo(object expected);
    }
}