//using StructureMap.Configuration.DSL;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Liquid.IoC.StructureMap
//{
//    class StructureMapRegistry : Registry
//    {
//        public StructureMapRegistry()
//        {
//            //// First I'll specify the "default" Instance of IRepository
//            //ForRequestedType<IRepository>().TheDefaultIsConcreteType<InMemoryRepository>();
 
//            //// Now, I'll add three more Instances of IRepository
//            //ForRequestedType<IRepository>().AddInstances(x =>
//            //{
//            //    // "NorthAmerica" is the concrete type DatabaseRepository with
//            //    // the connectionString pointed to the NorthAmerica database
//            //    x.OfConcreteType<DatabaseRepository>().WithName("NorthAmerica")
//            //        .WithCtorArg("connectionString").EqualTo("database=NorthAmerica");
 
//            //    // "Asia/Pacific" is the concrete type DatabaseRepository with
//            //    // the connectionString pointed to the AsiaPacific database
//            //    x.OfConcreteType<DatabaseRepository>().WithName("Asia/Pacific")
//            //        .WithCtorArg("connectionString").EqualTo("database=AsiaPacific");
 
//            //    // Lastly, the "Weird" instance is built by calling a specified
//            //    // Lambda (an anonymous delegate will work as well).
//            //    x.ConstructedBy(() => WeirdLegacyRepository.Current).WithName("Weird");
//            //});
//        }
//    }
//}
