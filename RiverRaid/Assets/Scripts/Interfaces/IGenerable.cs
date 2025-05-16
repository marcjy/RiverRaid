using System;

public interface IGenerable
{
    public event EventHandler OnShouldBeReleased;

    void Init();
}
