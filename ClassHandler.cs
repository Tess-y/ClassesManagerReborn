using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ClassesManagerReborn
{
    public abstract class ClassHandler
    {
        public abstract IEnumerator Init();

        public virtual IEnumerator PostInit() { yield break; }
    }
}
