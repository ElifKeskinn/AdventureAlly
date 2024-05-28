using AutoFixture.Kernel;
using MongoDB.Bson;
using System;

namespace CleanArchitecture.UnitTests.Utilities
{
    public class ObjectIdSpecimenBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request is Type type && type == typeof(ObjectId))
            {
                return ObjectId.GenerateNewId();
            }

            return new NoSpecimen();
        }
    }
}