﻿namespace InsuranceApp.Core.Helpers.ResponseModels
{
    public interface IResult
    {
        bool Success { get; }
        string Message { get; }
    }

}
