// OWNER BK, 23-04-2009
namespace Cic.OpenLease.Model
{
	[System.CLSCompliant(true)]
	public static class ObjectContextHelper
	{
		#region Private constants
		private const int CnstMaxSaveChangesLoop = 3;
		#endregion

		#region Methods
		// TEST BK 0 BK, Not tested
		public static void SaveChanges(System.Data.Objects.ObjectContext objectContext, System.Data.Objects.DataClasses.EntityObject[] entityObjects)
		{
			try
			{
				// Save changes
				MySaveChanges(objectContext, entityObjects);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static void SaveChanges(System.Data.Objects.ObjectContext objectContext, System.Data.Objects.DataClasses.EntityObject entityObject)
		{
			try
			{
				// Save changes
				MySaveChanges(objectContext, new System.Data.Objects.DataClasses.EntityObject[] { entityObject});
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}
		#endregion

		#region My methods
		private static void MySaveChanges(System.Data.Objects.ObjectContext objectContext, System.Data.Objects.DataClasses.EntityObject[] entityObjects)
		{
			System.Data.Objects.DataClasses.EntityObject EntityObject;
			System.Collections.Generic.List<System.Data.Objects.DataClasses.EntityObject> FailedEntityList;
			bool AllDone;
			int Loop = 0;

			// Check context
			if (objectContext != null)
			{
				// Check object list
				if ((entityObjects == null) || (entityObjects.GetLength(0) <= 0))
				{
					try
					{
						// Save changes
						objectContext.SaveChanges();
					}
					catch
					{
						// Throw caught exception
						throw;
					}
				}
				else
				{
					// Set state
					AllDone = false;
					Loop = 0;

					while ((!AllDone) && (Loop <= CnstMaxSaveChangesLoop))
					{
						try
						{
							// Set loop
							Loop = (Loop + 1);
							// Save changes
							objectContext.SaveChanges();
							// Set state
							AllDone = true;
						}
						catch (System.Data.OptimisticConcurrencyException ex)
						{
							// Check loop
							if (Loop == CnstMaxSaveChangesLoop)
							{
								// Set state
								AllDone = true;
								// Throw caught exception
								throw ex;
							}
							else
							{
								// New list
								FailedEntityList = new System.Collections.Generic.List<System.Data.Objects.DataClasses.EntityObject>();
								// This will only iterate once.
								foreach (System.Data.Objects.ObjectStateEntry LoopObjectStateEntry in ex.StateEntries)
								{
									// Get entity object
									EntityObject = (System.Data.Objects.DataClasses.EntityObject)LoopObjectStateEntry.Entity;
									// Check state
									if (EntityObject.EntityState == System.Data.EntityState.Detached)
									{
										// Attach
										objectContext.Attach(EntityObject);
									}
									// Refresh
									objectContext.Refresh(System.Data.Objects.RefreshMode.ClientWins, LoopObjectStateEntry.Entity);
									// Add to list
									FailedEntityList.Add(EntityObject);
								}
								// Set state
								AllDone = (FailedEntityList.Count == 0);
							}
						}
						catch
						{
							// Set state
							AllDone = true;
							// Throw caught exception
							throw;
						}
					}
				}
			}
		}
		#endregion
	}
}
