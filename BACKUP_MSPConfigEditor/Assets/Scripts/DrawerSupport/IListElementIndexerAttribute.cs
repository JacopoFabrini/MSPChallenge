using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

interface IListElementIndexerAttribute
{
    bool Removable { get; }
    bool Reorderable { get; }
}

