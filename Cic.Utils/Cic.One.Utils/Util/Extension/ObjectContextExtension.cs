using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using Cic.OpenOne.Common.Util.Linq;

namespace Cic.OpenOne.Common.Util.ExtensionOld
{
    /// <summary>
    /// ObjectContextExtension-Klasse
    /// </summary>
    [System.CLSCompliant(true)]
    public static class ObjectContextExtension
    {
        #region Methods

        /// <summary>
        /// getKey
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long getKey(this System.Data.Objects.ObjectContext context,System.Data.EntityKey key)
        {
            if (key == null) return 0;
            EntityKeyMember[] member = key.EntityKeyValues;
            if (member == null || member.Length == 0) return 0;
            return (long)member[0].Value;
        }

        /// <summary>
        /// getKeyName
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static String getKeyName(this System.Data.Objects.ObjectContext context, System.Data.EntityKey key)
        {
            if (key == null) return null;
            EntityKeyMember[] member = key.EntityKeyValues;
            if (member == null || member.Length == 0) return null;
            return member[0].Key;
        }

        /// <summary>
        /// getEntityKey
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static System.Data.EntityKey getEntityKey(this System.Data.Objects.ObjectContext context,System.Type type, long value)
        {
            System.Data.Metadata.Edm.EntityType etype = MyGetEntityType(context, type);
            return new System.Data.EntityKey(context.DefaultContainerName + "." + etype.Name, etype.KeyMembers[0].Name, value);
        }

       

        /// <summary>
        /// BuildContainsExpression
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="context"></param>
        /// <param name="valueSelector"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static Expression<Func<TElement, bool>> BuildContainsExpression<TElement, TValue>(this System.Data.Objects.ObjectContext context,
                                                                                Expression<Func<TElement, TValue>> valueSelector, IEnumerable<TValue> values)
        {
            if (null == valueSelector)
            {
                throw new ArgumentNullException("valueSelector");
            }
            if (null == values)
            {
                throw new ArgumentNullException("values");
            }
            ParameterExpression p = valueSelector.Parameters.Single();

            if (!values.Any())
            {
                return e => false;
            }

            var equals = values.Select(value => (Expression)Expression.Equal(valueSelector.Body, Expression.Constant(value, typeof(TValue))));
            var body = equals.Aggregate<Expression>((accumulate, equal) => Expression.Or(accumulate, equal));

            return Expression.Lambda<Func<TElement, bool>>(body, p);
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static T Insert<T>(this System.Data.Objects.ObjectContext context, T entity) where T : System.Data.Objects.DataClasses.EntityObject
        {
            string QualifiedEntitySetName;

            // Check context
            if (context == null)
            {
                // Throw exception
                throw new ArgumentNullException("context");
            }

            // Check entity
            if (entity == null)
            {
                // Throw exception
                throw new ArgumentNullException("entity");
            }

            // Check state
            if (entity.EntityState != System.Data.EntityState.Detached)
            {
                // Throw exception
                throw new ArgumentException("entity");
            }

            // Get qualified name
            QualifiedEntitySetName = GetQualifiedEntitySetName(context, typeof(T));

            try
            {
                // Add object
                context.AddObject(QualifiedEntitySetName, entity);
                // Save changes
                context.SaveChanges();
            }
            catch
            {
                // Throw exception
                throw;
            }
            return entity;
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="modifiedEntity"></param>
        /// <param name="originalEntity"></param>
        /// <returns></returns>
        public static T Update<T>(this System.Data.Objects.ObjectContext context, T modifiedEntity, T originalEntity)
            where T : System.Data.Objects.DataClasses.EntityObject
        {
            T UpdatedEntity;

            // Check context
            if (context == null)
            {
                // Throw exception
                throw new ArgumentException("context");
            }

            // Check modified entity
            if (modifiedEntity == null)
            {
                // Throw exception
                throw new ArgumentException("modifiedEntity");
            }

            // Check state
            if ((modifiedEntity.EntityState == System.Data.EntityState.Added) || (modifiedEntity.EntityState == System.Data.EntityState.Deleted))
            {
                // Throw exception
                throw new ArgumentException("modifiedEntity");
            }

            // Check state: entity is attached and unchanged
            if (modifiedEntity.EntityState == System.Data.EntityState.Unchanged)
            {

                // Set entity
                UpdatedEntity = modifiedEntity;
            }
            // Check state: entity is attached
            else if (modifiedEntity.EntityState == System.Data.EntityState.Modified)
            {
                try
                {
                    // Save changes
                    context.SaveChanges();
                }
                catch (OptimisticConcurrencyException)
                {
                    // Resolve the concurrently conflict by refreshing the 
                    // object context before saving changes. 
                    context.Refresh(RefreshMode.ClientWins, modifiedEntity);

                    // Resave changes in the object context.
                    context.SaveChanges();
                }
                catch
                {
                    // Throw caught exception
                    throw;
                }

                // Set entity
                UpdatedEntity = modifiedEntity;
            }
            // Check original entity
            else if (originalEntity == null)
            {
                try
                {
                    // Internal
                    UpdatedEntity = MyUpdateWithoutOriginalEntity<T>(context, modifiedEntity);
                }
                catch
                {
                    // Throw exception
                    throw;
                }
            }
            else
            {
                try
                {
                    // Internal
                    UpdatedEntity = MyUpdateWithOriginalEntity<T>(context, modifiedEntity, originalEntity);
                }
                catch
                {
                    // Throw exception
                    throw;
                }
            }
            return UpdatedEntity;
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="entity"></param>
        public static void Delete<T>(this System.Data.Objects.ObjectContext context, T entity) where T : System.Data.Objects.DataClasses.EntityObject
        {
            // Check context
            if (context == null)
            {
                // Throw exception
                throw new ArgumentException("context");
            }

            // Check entity
            if (entity == null)
            {
                // Throw exception
                throw new ArgumentException("entity");
            }

            // Check state
            if ((entity.EntityState == System.Data.EntityState.Added) || (entity.EntityState == System.Data.EntityState.Deleted))
            {
                // Throw exception
                throw new ArgumentException("entity");
            }

            try
            {
                // Check state
                if (entity.EntityState == System.Data.EntityState.Detached)
                {
                    // Attach
                    context.Attach(entity);
                }
                // Delete
                context.DeleteObject(entity);
                // Save changes
                context.SaveChanges();
            }
            catch
            {
                // Throw exception
                throw;
            }
        }

        /// <summary>
        /// SelectById
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T SelectById<T>(this System.Data.Objects.ObjectContext context, long id) where T : System.Data.Objects.DataClasses.EntityObject
        {
            // Check context
            if (context == null)
            {
                // Throw exception
                throw new ArgumentException("context");
            }

            try
            {
                // Try to get the object
                return MySelectById<T>(context, id);
            }
            catch
            {
                // Throw exception
                throw;
            }
        }

        /// <summary>
        /// Select
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="where"></param>
        /// <param name="whereParams"></param>
        /// <returns></returns>
        public static System.Collections.Generic.List<T> Select<T>(this System.Data.Objects.ObjectContext context, string where, object[] whereParams)
            where T : System.Data.Objects.DataClasses.EntityObject
        {
            try
            {
                // Try to get the objects
                return MySelect<T>(context, where, whereParams, null, 0, 0);
            }
            catch
            {
                // Throw exception
                throw;
            }
        }

        /// <summary>
        /// Select
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="where"></param>
        /// <param name="whereParams"></param>
        /// <param name="order"></param>
        /// <param name="skip"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public static System.Collections.Generic.List<T> Select<T>(this System.Data.Objects.ObjectContext context, string where, object[] whereParams,
                                                                   string order, int skip, int top)
            where T : System.Data.Objects.DataClasses.EntityObject
        {
            try
            {
                // Try to get the objects
                return MySelect<T>(context, where, whereParams, order, skip, top);
            }
            catch
            {
                // Throw exception
                throw;
            }
        }

        /// <summary>
        /// Select
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="order"></param>
        /// <param name="skip"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public static System.Collections.Generic.List<T> Select<T>(this System.Data.Objects.ObjectContext context, string order, int skip, int top)
            where T : System.Data.Objects.DataClasses.EntityObject
        {
            try
            {
                // Try to get the objects
                return MySelect<T>(context, null, null, order, skip, top);
            }
            catch
            {
                // Throw exception
                throw;
            }
        }

        /// <summary>
        /// Select
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="skip"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public static System.Collections.Generic.List<T> Select<T>(this System.Data.Objects.ObjectContext context, int skip, int top)
            where T : System.Data.Objects.DataClasses.EntityObject
        {
            try
            {
                // Try to get the objects
                return MySelect<T>(context, null, null, null, skip, top);
            }
            catch
            {
                // Throw exception
                throw;
            }
        }

        /// <summary>
        /// Select
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public static System.Collections.Generic.List<T> Select<T>(this System.Data.Objects.ObjectContext context)
            where T : System.Data.Objects.DataClasses.EntityObject
        {
            try
            {
                // Try to get the objects
                return MySelect<T>(context, null, null, null, 0, 0);
            }
            catch
            {
                // Throw exception
                throw;
            }
        }

        /// <summary>
        /// Count
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="where"></param>
        /// <param name="whereParams"></param>
        /// <returns></returns>
        public static int Count<T>(this System.Data.Objects.ObjectContext context, string where, object[] whereParams)
            where T : System.Data.Objects.DataClasses.EntityObject
        {
            try
            {
                // Internal
                return MyCount<T>(context, where, whereParams);
            }
            catch
            {
                // Throw caught exception
                throw;
            }
        }

        /// <summary>
        /// Count
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public static int Count<T>(this System.Data.Objects.ObjectContext context)
            where T : System.Data.Objects.DataClasses.EntityObject
        {
            try
            {
                // Internal
                return MyCount<T>(context, null, null);
            }
            catch
            {
                // Throw caught exception
                throw;
            }
        }

        /// <summary>
        /// DeliverRelationshipEntitiesRemote
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="entity"></param>
        /// <param name="relationshipName"></param>
        /// <param name="targetRoleName"></param>
        /// <returns></returns>
        public static System.Collections.Generic.List<object> DeliverRelationshipEntitiesRemote<T>(this System.Data.Objects.ObjectContext context, T entity,
            string relationshipName, string targetRoleName)
            where T : System.Data.Objects.DataClasses.EntityObject
        {
            System.Collections.Generic.List<object> List;
            System.Data.Objects.DataClasses.IEntityWithRelationships EntityWithRelationships;
            System.Data.Objects.DataClasses.IRelatedEnd RelatedEnd;

            // Check context
            if (context == null)
            {
                // Throw exception
                throw new ArgumentException("context");
            }

            // Check entity
            if (entity == null)
            {
                // Throw exception
                throw new ArgumentException("entity");
            }

            // Check relationship name
            if (string.IsNullOrEmpty(relationshipName))
            {
                // Throw exception
                throw new ArgumentException("relationshipName");
            }

            // Check target role name
            if (string.IsNullOrEmpty(targetRoleName))
            {
                // Throw exception
                throw new ArgumentException("targetRoleName");
            }

            // Create new list
            List = new System.Collections.Generic.List<object>();

            // Cast
            EntityWithRelationships = entity as System.Data.Objects.DataClasses.IEntityWithRelationships;

            // Check object
            if (EntityWithRelationships != null)
            {
                try
                {
                    // Attach
                    context.Attach((System.Data.Objects.DataClasses.EntityObject)entity);
                    // Get related end
                    RelatedEnd = EntityWithRelationships.RelationshipManager.GetRelatedEnd(relationshipName, targetRoleName);
                    // Load
                    RelatedEnd.Load();
                }
                catch
                {
                    // Throw caught exception
                    throw;
                }

                // Check object
                if (RelatedEnd != null)
                {
                    // Loop
                    foreach (object LoopObject in RelatedEnd)
                    {
                        // Add
                        List.Add(LoopObject);
                    }
                }
            }

            // Return list
            return List;
        }
        #endregion

        #region My methods
        private static T MyUpdateWithoutOriginalEntity<T>(System.Data.Objects.ObjectContext context, T modifiedEntity)
            where T : System.Data.Objects.DataClasses.EntityObject
        {

            T OriginalEntity;
            long Id;

            // Check state
            if (modifiedEntity.EntityState != System.Data.EntityState.Detached)
            {
                // Throw exception
                throw new ArgumentException("modifiedEntity");
            }

            // Check key
            if ((modifiedEntity.EntityKey != null) && (modifiedEntity.EntityKey.EntityKeyValues != null) && (modifiedEntity.EntityKey.EntityKeyValues.Length != 1))
            {
                // Throw exception
                throw new ArgumentException("ApplicationDataMissingPrimaryKeyData " + modifiedEntity);
            }

            try
            {
                // Get key value
                Id = System.Convert.ToInt64(modifiedEntity.EntityKey.EntityKeyValues[0].Value);
                // Get original entity
                OriginalEntity = MySelectById<T>(context, Id);
            }
            catch
            {
                // Throw exception
                throw new ArgumentException("ApplicationDataMissingPrimaryKeyData " + modifiedEntity);
            }

            try
            {
                // Update entity
                return MyUpdateWithOriginalEntity<T>(context, modifiedEntity, OriginalEntity);
            }
            catch
            {
                // Throw
                throw;
            }
        }

        private static T MyUpdateWithOriginalEntity<T>(System.Data.Objects.ObjectContext context, T modifiedEntity, T originalEntity)
            where T : System.Data.Objects.DataClasses.EntityObject
        {
            string QualifiedEntitySetName;

            // Check state
            if (modifiedEntity.EntityState != System.Data.EntityState.Detached)
            {
                // Throw exception
                throw new ArgumentException("modifiedEntity");
            }

            // Get qualified name
            QualifiedEntitySetName = GetQualifiedEntitySetName(context, typeof(T));

            try
            {
                // Check state
                if (originalEntity.EntityState == System.Data.EntityState.Detached)
                {
                    // Attach
                    context.Attach(originalEntity);
                }

                // Apply property changes
                context.ApplyCurrentValues(QualifiedEntitySetName, modifiedEntity);

                // Save changes
                context.SaveChanges();
            }
            catch
            {
                // Throw exception
                throw;
            }

            // Return
            return originalEntity;
        }

        private static T MySelectById<T>(System.Data.Objects.ObjectContext context, long id)
            where T : System.Data.Objects.DataClasses.EntityObject
        {
            System.Data.EntityKey EntityKey;
            string QualifiedEntitySetName;
            System.Type Type;
            string KeyName;
            object ObjectOfT;

            // Get type
            Type = typeof(T);

            try
            {
                // Get key name
                KeyName = GetEntityTypeKeyName(context, Type);
            }
            catch
            {
                // Throw exception
                throw;
            }

            // Get qualified name
            QualifiedEntitySetName = GetQualifiedEntitySetName(context, Type);
            // New entity key
            EntityKey = new System.Data.EntityKey(QualifiedEntitySetName, KeyName, id);

            // Create query to initialize context
            context.CreateQuery<T>(GetQualifiedEntitySetName(context, Type));

            try
            {
                // Try to get the object
                context.TryGetObjectByKey(EntityKey, out ObjectOfT);
            }
            catch
            {
                // Throw exception
                throw;
            }

            // Return
            return (T)ObjectOfT;
        }

        private static int MyCount<T>(this System.Data.Objects.ObjectContext context, string where, object[] whereParams)
            where T : System.Data.Objects.DataClasses.EntityObject
        {
            System.Linq.IQueryable<T> Query;

            // Check context
            if (context == null)
            {
                // Throw exception
                throw new ArgumentNullException("context");
            }

            string test = GetQualifiedEntitySetName(context, typeof(T));

            // Create query to initialize context

            Query = context.CreateQuery<T>(GetQualifiedEntitySetName(context, typeof(T)));

            // Check where
            if (!string.IsNullOrEmpty(where))
            {

                Query = DynamicQueryable.Where<T>(Query, where, whereParams);
            }

            try
            {
                // Return count
                return Query.Count<T>();
            }
            catch
            {
                // Throw caught exception
                throw;
            }
        }

        private static System.Collections.Generic.List<T> MySelect<T>(this System.Data.Objects.ObjectContext context, string where, object[] whereParams,
            string order, int skip, int top)
            where T : System.Data.Objects.DataClasses.EntityObject
        {
            System.Linq.IQueryable<T> Query;

            // Check context
            if (context == null)
            {
                // Throw exception
                throw new ArgumentNullException("context");
            }

            // Check skip
            if (skip < 0)
            {
                // Throw exception
                throw new ArgumentNullException("skip");
            }

            // Check skip
            if (top < 0)
            {
                // Throw exception
                throw new ArgumentNullException("top");
            }

            // Check context
            if (context == null)
            {
                // Throw exception
                throw new ArgumentNullException("context");
            }

            try
            {
                // Create query to initialize context
                Query = context.CreateQuery<T>(GetQualifiedEntitySetName(context, typeof(T)));
            }
            catch
            {
                // Throw caught exception
                throw;
            }

            // Check query
            if (Query != null)
            {
                // Check where
                if (!string.IsNullOrEmpty(where))
                {
                    Query = DynamicQueryable.Where<T>(Query, where, whereParams);

                }

                // Check order
                if (!string.IsNullOrEmpty(order))
                {

                    Query = Query.OrderBy<T>(order);
                }

                // Check top
                if (top > 0)
                {

                    if (!string.IsNullOrEmpty(order))
                    {
                        Query = Query.Skip<T>(skip);
                    }

                    Query = Query.Take<T>(top);
                }

                try
                {
                    // Return list

                    return Query.ToList<T>();
                }
                catch
                {
                    throw;
                }
            }
            else
            {

                return new System.Collections.Generic.List<T>();
            }
        }
        #endregion

        /// <summary>
        /// GetQualifiedEntitySetName
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetQualifiedEntitySetName(System.Data.Objects.ObjectContext context, System.Type type)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return (context.DefaultContainerName + "." + MyGetEntityTypeName(context, type));
        }

        /// <summary>
        /// GetEntityTypeKeyName
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetEntityTypeKeyName(System.Data.Objects.ObjectContext context, System.Type type)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            System.Data.Metadata.Edm.EntityType EntityType;

            // Get entity tyoe
            EntityType = MyGetEntityType(context, type);

            if ((EntityType.KeyMembers == null) || (EntityType.KeyMembers.Count != 1))
            {

                throw new ArgumentNullException(type.Namespace + type.Name + ".KeyMembers");

            }
            // Return key name
            return EntityType.KeyMembers[0].Name;
        }

        private static string MyGetEntityTypeName(System.Data.Objects.ObjectContext context, System.Type type)
        {
            return MyGetEntityType(context, type).Name;
        }

        private static System.Data.Metadata.Edm.EntityType MyGetEntityType(System.Data.Objects.ObjectContext context, System.Type type)
        {
            return context.MetadataWorkspace.GetType(type.Name, type.Namespace, System.Data.Metadata.Edm.DataSpace.CSpace) as System.Data.Metadata.Edm.EntityType;
        }

        /// <summary>
        /// Executes a stored Function and returns the result value
        /// The Parameters have to contain a ReturnValue Direction Parameter. 
        /// All Parameters (except the return value Parameter) must be named as defined in the Database.
        /// </summary>
        /// <param name="context">Object Context</param>
        /// <param name="function">Name of the Function including Package-Name, eg. CIC_UTILS.TEST</param>
        /// <param name="param">All Parameters including Return Value Parameter, including Type and Name</param>
        /// <returns></returns>
        public static object ExecuteFunction(this System.Data.Objects.ObjectContext context, string function, params DbParameter[] param)
        {
            object rval = null;
            //Execute Query
            DbConnection con = (context.Connection as EntityConnection).StoreConnection;
            try
            {
                con.Open();
                DbCommand cmd = con.CreateCommand();

                // query values with a stored procedure with two out parameters
                cmd.CommandText = function;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                DbParameter rParam = null;
                foreach (DbParameter p in param)
                {
                    cmd.Parameters.Add(p);
                    if (p.Direction == System.Data.ParameterDirection.ReturnValue)
                        rParam = p;
                }
                //Execute Stored Function
                //rval = cmd.ExecuteScalar();
                if (rParam == null)
                    throw new Exception("no Parameter with ParameterDirection.ReturnValue was specified!");
                cmd.ExecuteNonQuery();
                rval = rParam.Value;

                cmd.Dispose();
            }
            catch (Exception ex)
            {
                //Log l = new Cic.Basic.Logging.Log4Net.Log(Cic.OpenLease.Service.UserConfigurationHelper.DeliverLoggerNameValue(), Cic.OpenLease.Service.UserConfigurationHelper.DeliverFileInfo());
                //l.Error("calling stored Procedure " + procedure + " failed: " + ex.Message);
                throw new InvalidOperationException("calling stored Function " + function + " failed: " + ex);
            }
            finally
            {
                con.Close();
            }
            return rval;
        }

        /// <summary>
        /// Executes a named stored procedure directly at the database with the given parameters
        /// the conceptual model is not used. The Procedure doesnt have to be mapped to the Entity Framework.
        /// Returned Values are stored in the param.value when param is defined as out-param.
        /// </summary>
        /// <param name="context">Object Context</param>
        /// <param name="procedure">Name of the Procedure</param>
        /// <param name="param">Parameters needed by the Procedure.</param>
        public static void ExecuteProcedure(this System.Data.Objects.ObjectContext context, string procedure, params DbParameter[] param)
        {
            //Execute Query
            DbConnection con = (context.Connection as EntityConnection).StoreConnection;
            try
            {
                con.Open();
                DbCommand cmd = con.CreateCommand();

                // query values with a stored procedure with two out parameters
                cmd.CommandText = procedure;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                foreach (DbParameter p in param)
                    cmd.Parameters.Add(p);

                //Execute Stored Procedure
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                //Log l = new Cic.Basic.Logging.Log4Net.Log(Cic.OpenLease.Service.UserConfigurationHelper.DeliverLoggerNameValue(), Cic.OpenLease.Service.UserConfigurationHelper.DeliverFileInfo());
                //l.Error("calling stored Procedure " + procedure + " failed: " + ex.Message);
                throw new InvalidOperationException("calling stored Procedure " + procedure + " failed: " + ex);
            }
            finally
            {
                con.Close();
            }
        }
    }
}