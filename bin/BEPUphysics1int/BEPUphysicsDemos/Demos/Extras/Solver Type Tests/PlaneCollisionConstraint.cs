using System;
using System.Threading;
using BEPUutilities;
using FixMath.NET;

namespace BEPUphysicsDemos.Demos.Extras.SolverTypeTests
{
    class PlaneCollisionConstraint : Constraint
    {
        public LinearDynamic Dynamic { get; private set; }

        private Plane plane;

        private Fix64 effectiveMass;


        private Fix64 biasVelocity;

        private Fix64 accumulatedImpulse;
        private Fix64 impulse;


        public Plane Plane
        {
            get { return plane; }
        }

        /// <summary>
        /// Gets the distance from the plane to the dynamic.
        /// </summary>
        public Fix64 Distance
        {
            get
            {
                Fix64 d;
                Vector3.Dot(ref plane.Normal, ref Dynamic.Position, out d);
                return d + plane.D;
            }
        }

        public PlaneCollisionConstraint(LinearDynamic dynamic, Plane plane)
        {
            Dynamic = dynamic;
            ++dynamic.ConstraintCount;
            this.plane = plane;
        }


        public override void Preupdate(Fix64 inverseDt, bool useConstraintCounts)
        {
            Fix64 d;
            Vector3.Dot(ref plane.Normal, ref Dynamic.Position, out d);

            if (useConstraintCounts)
                effectiveMass = 1 / (Dynamic.ConstraintCount * Dynamic.InverseMass + Softness);
            else
                effectiveMass = 1 / (Dynamic.InverseMass + Softness);

            Fix64 error = d + plane.D;
            if (error > 0)
            {
                //Allow the dynamic to approach the plane, but no closer.
                biasVelocity = error * inverseDt;
            }
            else
            {
                biasVelocity = error * BiasFactor * inverseDt;
            }
        }

        public override void SolveIteration()
        {
            Fix64 velocityAlongJacobian;
            Vector3.Dot(ref Dynamic.Velocity, ref plane.Normal, out velocityAlongJacobian);


            Fix64 changeInVelocity = -velocityAlongJacobian - biasVelocity - Softness * accumulatedImpulse;

            Fix64 newImpulse = changeInVelocity * effectiveMass;

            Fix64 newAccumulatedImpulse = accumulatedImpulse + newImpulse;
            newAccumulatedImpulse = MathHelper.Max(newAccumulatedImpulse, 0);

            impulse = newAccumulatedImpulse - accumulatedImpulse;
            accumulatedImpulse = newAccumulatedImpulse;

        }

        public override void ApplyImpulse(LinearDynamic dynamic)
        {
            ApplyImpulses();
        }

        public override void ApplyAccumulatedImpulse(LinearDynamic dynamic)
        {
            ApplyAccumulatedImpulses();
        }

        public override void ApplyImpulses()
        {
            Vector3 worldSpaceImpulse;
            Vector3.Multiply(ref plane.Normal, impulse, out worldSpaceImpulse);
            Dynamic.ApplyImpulse(ref worldSpaceImpulse);
        }

        public override void ApplyAccumulatedImpulses()
        {
            Vector3 worldSpaceImpulse;
            Vector3.Multiply(ref plane.Normal, accumulatedImpulse, out worldSpaceImpulse);
            Dynamic.ApplyImpulse(ref worldSpaceImpulse);
        }

        internal override void AddToConnections()
        {
            Dynamic.Constraints.Add(this);
        }

        public override void EnterLock()
        {
            Dynamic.SolverSpinLock.Enter();
        }

        public override void ExitLock()
        {
            Dynamic.SolverSpinLock.Exit();
        }

    }
}
