using System;
using System.Collections.Generic;
using System.Text;

namespace Cache.Core.Model {
    public interface ICacheItem<T> {

        string Key { get; set; }

        T Value { get; set; }
    }
}
