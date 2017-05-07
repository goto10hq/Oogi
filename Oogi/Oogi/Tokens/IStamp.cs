using System;

namespace Oogi.Tokens
{
    public interface IStamp
    {
        DateTime DateTime { get; set; }
        int Epoch { get; }        
    }
}
