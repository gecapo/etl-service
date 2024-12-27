﻿namespace ETL.Factories;

public sealed class Factory<T>(List<T> variants) : IFactory<T> where T : IStrategy
{
    public T GetStrategy(string type) => variants.Find(s => s.IsHandler(type))!;
}