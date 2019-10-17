// OWNER BK, 03-03-2009
namespace Cic.OpenLease.Model.DdOl
{
	#region Using directives
	using System.Linq;
    using System.Collections.Generic;
	#endregion

	[System.CLSCompliant(true)]
	public static class PersonHelper
	{
		#region Methods
        public static bool Contains(OlExtendedEntities olExtendedEntities, long sysPERSON)
        {
            // Check context
            if (olExtendedEntities == null)
            {
                throw new System.ArgumentException("olExtendedEntities");
            }

            return olExtendedEntities.PERSON.Where(par => par.SYSPERSON == sysPERSON).Any<PERSON>();
        }

        public static bool Contains(OlExtendedEntities olExtendedEntities, string Code)
        {
            // Check context
            if (olExtendedEntities == null)
            {
                throw new System.ArgumentException("olExtendedEntities");
            }

            return olExtendedEntities.PERSON.Where(par => par.CODE == Code).Any<PERSON>();
        }

        public static Cic.OpenLease.Model.DdOl.PERSON SelectByCode(OlExtendedEntities olExtendedEntities, string Code) 
        {
            System.Linq.IQueryable<Cic.OpenLease.Model.DdOl.PERSON> Query;
            Cic.OpenLease.Model.DdOl.PERSON Person;
            Person = null;
            // Check context
            if (olExtendedEntities == null)
            {
                throw new System.ArgumentException("olExtendedEntities");
            }
            // Set Query
            Query = olExtendedEntities.PERSON;
            // Search for code
            Query = Query.Where<Cic.OpenLease.Model.DdOl.PERSON>(par => par.CODE == Code);

            if (Query != null)
            {
                // Select
                Person = Query.FirstOrDefault<Cic.OpenLease.Model.DdOl.PERSON>();
            }
            return Person;
        }

		// TEST BK 0 BK, Not tested
		public static Cic.OpenLease.Model.DdOl.PERSON SelectBySysPERSON(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, long sysPERSON)
		{
			
			Cic.OpenLease.Model.DdOl.PERSON Person;

			// Check context
			if (olExtendedEntities == null)
			{
				throw new System.ArgumentException("olExtendedEntities");
			}

			
			try
			{
                Person = olExtendedEntities.ExecuteStoreQuery<Cic.OpenLease.Model.DdOl.PERSON>("select * from person where sysperson="+sysPERSON).FirstOrDefault();

			}
			catch
			{
				throw;
			}

			// Check object
			if (Person == null)
			{
				throw new System.Exception(typeof(Cic.OpenLease.Model.DdOl.PERSON).ToString() + "." + Cic.OpenLease.Model.DdOl.PERSON.FieldNames.SYSPERSON.ToString() + " = " + sysPERSON.ToString());
			}

			return Person;
		}

        public static Cic.OpenLease.Model.DdOl.PERSON SelectBySysPERSONWithoutException(OlExtendedEntities olExtendedEntities, long sysPerson)
        {
            System.Linq.IQueryable<Cic.OpenLease.Model.DdOl.PERSON> Query;
            Cic.OpenLease.Model.DdOl.PERSON Person;
            Person = null;
            // Check context
            if (olExtendedEntities == null)
            {
                throw new System.ArgumentException("olExtendedEntities");
            }
            // Set Query
            Query = olExtendedEntities.PERSON;
            // Search for code
            Query = Query.Where<Cic.OpenLease.Model.DdOl.PERSON>(par => par.SYSPERSON == sysPerson);

            if (Query != null)
            {
                // Select
                Person = Query.FirstOrDefault<Cic.OpenLease.Model.DdOl.PERSON>();
            }
            return Person;
        }

        public static List<Cic.OpenLease.Model.DdOl.PERSON> SelectBySysPUSER(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, long sysPuser)
        {
            System.Linq.IQueryable<Cic.OpenLease.Model.DdOl.PERSON> Query;
            List<Cic.OpenLease.Model.DdOl.PERSON> PersonList;

            // Check context
            if (olExtendedEntities == null)
            {
                throw new System.ArgumentException("olExtendedEntities");
            }

            // Set Query
            Query = olExtendedEntities.PERSON;
            // Search for code
            Query = Query.Where<Cic.OpenLease.Model.DdOl.PERSON>(par => par.SYSPUSER == sysPuser);
            
            try
            {
                // Select
                PersonList = Query.ToList();
            }
            catch
            {
                throw;
            }

            return PersonList;
        }
        
        public static bool KontoExists(OlExtendedEntities olExtendedEntities, long sysPERSON) 
        {
            // Check context
            if (olExtendedEntities == null)
            {
                throw new System.ArgumentException("olExtendedEntities");
            }

            return olExtendedEntities.PERSON.Where(par => par.SYSPERSON == sysPERSON && (!string.IsNullOrEmpty(par.KONTONR) || !string.IsNullOrEmpty(par.IBAN))).Any<PERSON>();
        }

        public static bool KontoExists(OlExtendedEntities olExtendedEntities, PERSON Person)
        {
            // Check context
            if (olExtendedEntities == null)
            {
                throw new System.ArgumentException("olExtendedEntities");
            }
            if (!string.IsNullOrEmpty(Person.KONTONR) || !string.IsNullOrEmpty(Person.IBAN))
                return true;
            else 
                return false;            
        }
        
        public static void Insert(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, PERSON Person)
        {
            // Check context
            if (olExtendedEntities == null)
            {
                // Throw exception
                throw new System.ArgumentException("olExtendedEntities");
            }

            // Save
            olExtendedEntities.AddToPERSON(Person);
            olExtendedEntities.SaveChanges();
        }

		#endregion
	}
}
