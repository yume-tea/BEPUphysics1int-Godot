using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BEPUutilities;
using FixMath.NET;

namespace BEPUphysicsDemos.Demos.Extras.SolverTypeTests
{
    abstract class Constraint
    {

        public Fix64 Softness = 0;
        public Fix64 BiasFactor = 0.2m;

        public abstract void Preupdate(Fix64 inverseDt, bool useConstraintCounts);

        public abstract void SolveIteration();

        public abstract void ApplyImpulse(LinearDynamic dynamic);

        public abstract void ApplyAccumulatedImpulse(LinearDynamic dynamic);

        public abstract void ApplyImpulses();

        public abstract void ApplyAccumulatedImpulses();

        internal abstract void AddToConnections();

        public abstract void EnterLock();
        public abstract void ExitLock();
    }
}
