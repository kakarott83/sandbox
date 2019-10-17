create or replace package InstanceStoreLogic is

  type raw_table is table of raw(32767) index by binary_integer;

  procedure InsertRunnableInstanceEntry(
  	p_surrogateInstanceId integer,
  	p_workflowHostType raw,
  	p_serviceDeploymentId integer, 
  	p_isSuspended integer,
  	p_isReadyToRun integer,
  	p_pendingTimer date
  );
  
  procedure RecoverInstanceLocks;
  
  function GetExpirationTime (offsetInMilliseconds integer)
  return date;
  
  function CreateLockOwner(
  	p_lockOwnerId raw,
  	p_LockTimeout integer,
  	p_WorkflowHostType raw,
  	p_EnqueueCommand integer,
  	p_DeletesInstanceOnCompletion integer,
  	p_PrimitiveLockOwnerData blob,
  	p_ComplexLockOwnerData blob,
  	p_WOPrimitiveLockOwnerData blob,
  	p_WOComplexLockOwnerData blob,
  	p_EncodingOption integer,
  	p_MachineName varchar
  ) return integer;
  
  procedure DeleteLockOwner(
  	p_surrogateLockOwnerId integer
  );
  
  function ExtendLock(
  	p_surrogateLockOwnerId integer,
  	p_lockTimeout integer
  ) return integer;
  
  function AssociateKeys(
  	p_surrogateInstanceId integer,
  	p_keysToAssociateId raw_table,
  	p_keysToAssociateData raw_table,
  	p_encodingOption integer,
  	p_singleKeyId raw
  ) return integer;
  
  function CompleteKeys(
  	p_surrogateInstanceId integer,
  	p_keysToComplete raw_table
  ) return integer;
  
  function FreeKeys(
  	p_surrogateInstanceId integer,
  	p_keysToFree raw_table
  ) return integer;
  
  procedure CreateInstance(
  	p_instanceId raw,
  	p_surrogateLockOwnerId integer,
  	p_workflowHostType raw,
  	p_serviceDeploymentId integer,
  	p_surrogateInstanceId out integer
  );
  
  function LockInstance(
  	p_instanceId raw,
  	p_surrogateLockOwnerId integer,
  	p_handleInstanceVersion integer,
  	p_handleIsBoundToLock integer,
  	p_surrogateInstanceId out integer,
  	p_lockVersion out integer
  ) return integer;
  
  procedure UnlockInstance(
  	p_instanceId raw,
  	p_surrogateLockOwnerId integer,
  	p_handleInstanceVersion integer
  );
  
  procedure DetectRunnableInstances(
  	p_workflowHostType raw,
    p_nextRunnableTime out date,
    p_now out date
  );
  
  procedure GetActivatableWorkflows(
  	p_machineName varchar2,
    p_result out sys_refcursor
  );
  
  function LoadInstance(
  	p_surrogateLockOwnerId integer,
  	p_operationType integer,
  	p_keyToLoadBy raw,
  	p_instanceId raw,
  	p_handleInstanceVersion integer,
  	p_handleIsBoundToLock integer,
  	p_keysToAssociateId raw_table,
  	p_keysToAssociateData raw_table,
  	p_singleKeyId raw,
  	p_encodingOption integer,
  	p_operationTimeout integer,
    p_instance out sys_refcursor,
    p_changes out sys_refcursor,
    p_keys out sys_refcursor
  ) return integer;
  
  function TryLoadRunnableInstance(
  	p_surrogateLockOwnerId integer,
  	p_workflowHostType raw,
  	p_operationType integer,
  	p_handleInstanceVersion integer,
  	p_handleIsBoundToLock integer,
  	p_encodingOption integer,	
  	p_operationTimeout integer,
    p_hasInstance out integer,
    p_instance out sys_refcursor,
    p_changes out sys_refcursor,
    p_keys out sys_refcursor
  ) return integer;
  
  procedure DeleteInstance(
  	p_surrogateInstanceId integer
  );
  
  procedure CreateServiceDeployment(
  	p_serviceDeploymentHash raw,
  	p_siteName varchar,
  	p_relativeServicePath varchar,
  	p_relativeApplicationPath varchar,
  	p_serviceName varchar,
    p_serviceNamespace varchar,
    p_serviceDeploymentId out integer
  );
  
  function SaveInstance(
  	p_instanceId raw,
  	p_surrogateLockOwnerId integer,
  	p_handleInstanceVersion integer,
  	p_handleIsBoundToLock integer,
  	p_primitiveDataProperties blob,
  	p_complexDataProperties blob,
  	p_wOPrimitiveDataProperties blob,
  	p_wOComplexDataProperties blob,
  	p_metadataProperties blob,
  	p_metadataIsConsistent integer,
  	p_encodingOption integer,
  	p_timerDurationMilliseconds integer,
  	p_suspensionStateChange integer,
  	p_suspensionReason varchar,
  	p_suspensionExceptionName varchar,
  	p_keysToAssociateId raw_table,
  	p_keysToAssociateData raw_table,
  	p_keysToComplete raw_table,
  	p_keysToFree raw_table,
  	p_unlockInstance integer,
  	p_isReadyToRun integer,
  	p_isCompleted integer,
  	p_singleKeyId raw,
  	p_lastMachineRunOn varchar,
  	p_executionStatus varchar,
  	p_blockingBookmarks varchar,
  	p_workflowHostType raw,
  	p_serviceDeploymentId integer,
  	p_operationTimeout integer,
    p_currentInstanceVersion out integer
  ) return integer;

end;
/

create or replace package body InstanceStoreLogic is

procedure InsertRunnableInstanceEntry(
	p_surrogateInstanceId integer,
	p_workflowHostType raw,
	p_serviceDeploymentId integer, 
	p_isSuspended integer,
	p_isReadyToRun integer,
	p_pendingTimer date
)
is
	p_runnableTime date;
begin
	if p_isSuspended = 0 then
		if p_isReadyToRun = 1 then
			p_runnableTime := sysdate;
		elsif p_pendingTimer is not null then
			p_runnableTime := p_pendingTimer;
		end if;
	end if;
		
	if p_runnableTime is not null and p_workflowHostType is not null then
		insert into RunnableInstancesTable
			(SurrogateInstanceId, WorkflowHostType, ServiceDeploymentId, RunnableTime)
			values( p_surrogateInstanceId, p_workflowHostType, p_serviceDeploymentId, p_runnableTime);
	end if;
end;

procedure RecoverInstanceLocks
is
begin
	insert into RunnableInstancesTable (SurrogateInstanceId, WorkflowHostType, ServiceDeploymentId, RunnableTime)
		select instances.SurrogateInstanceId, instances.WorkflowHostType, instances.ServiceDeploymentId, sysdate
		from LockOwnersTable lockOwners inner join
			 InstancesTable instances
				on instances.SurrogateLockOwnerId = lockOwners.SurrogateLockOwnerId
			where
				lockOwners.LockExpiration <= sysdate and
				instances.IsInitialized = 1 and
				instances.IsSuspended = 0
        and rownum <= 1000;

	delete from LockOwnersTable lockOwners
	where LockExpiration <= sysdate
	and not exists
	(
		select 1
		from InstancesTable inst
		where inst.SurrogateLockOwnerId = lockOwners.SurrogateLockOwnerId
	);
end;

function GetExpirationTime (offsetInMilliseconds integer)
return date
is
begin
	if offsetInMilliseconds is null then
		return null;
	end if;

	return sysdate + offsetInMilliseconds / (24*60*60*1000);
end;

function CreateLockOwner(
	p_lockOwnerId raw,
	p_LockTimeout integer,
	p_WorkflowHostType raw,
	p_EnqueueCommand integer,
	p_DeletesInstanceOnCompletion integer,
	p_PrimitiveLockOwnerData blob,
	p_ComplexLockOwnerData blob,
	p_WOPrimitiveLockOwnerData blob,
	p_WOComplexLockOwnerData blob,
	p_EncodingOption integer,
	p_MachineName varchar
) return integer
is
	p_LockExpiration date;
	p_surrogateLockOwnerId integer;
begin
	if p_lockTimeout = 0 then
		p_lockExpiration := to_date('9999-12-31 23:59:59', 'yyyy-mm-dd hh24:mi:ss');
	else
		p_lockExpiration := sysdate + p_lockTimeout / 24*60*60;
  end if;
		
	select seq_LockOwnersTable.nextval into p_surrogateLockOwnerId from dual;

	insert into LockOwnersTable (Id, SurrogateLockOwnerId, LockExpiration, MachineName, WorkflowHostType, EnqueueCommand, DeletesInstanceOnCompletion, PrimitiveLockOwnerData, ComplexLockOwnerData, WOPrimitiveLockOwnerData, WOComplexLockOwnerData, EncodingOption)
	values (p_lockOwnerId, p_surrogateLockOwnerId, p_lockExpiration, p_machineName, p_workflowHostType, p_enqueueCommand, p_deletesInstanceOnCompletion, p_primitiveLockOwnerData, p_complexLockOwnerData, p_wOPrimitiveLockOwnerData, p_wOComplexLockOwnerData, p_encodingOption);
	
	return p_surrogateLockOwnerId;
end;

procedure DeleteLockOwner(
	p_surrogateLockOwnerId integer
)
is
begin
	update LockOwnersTable set LockExpiration = to_date('2000-01-01 00:00:00', 'yyyy-mm-dd hh24:mi:ss')
	where SurrogateLockOwnerId = p_surrogateLockOwnerId;
end;

function ExtendLock(
	p_surrogateLockOwnerId integer,
	p_lockTimeout integer
) return integer
is
	newLockExpiration date;
  p_count integer;
begin
	if p_lockTimeout = 0 then
		newLockExpiration := to_date('9999-12-31 23:59:59', 'yyyy-mm-dd hh24:mi:ss');
	else
		newLockExpiration := sysdate + p_lockTimeout / 24*60*60;
  end if;
	
	update LockOwnersTable
	set LockExpiration = newLockExpiration
	where (SurrogateLockOwnerId = p_surrogateLockOwnerId) and
		  (LockExpiration > sysdate);
	
	if SQL%rowcount = 0 then
		select count(*) into p_count from LockOwnersTable where (SurrogateLockOwnerId = p_surrogateLockOwnerId);
    if p_count > 0 then
			DeleteLockOwner(p_surrogateLockOwnerId);
			return 11;
		else
			return 12;
    end if;
	end if;

  return 0;
end;

function AssociateKeys(
	p_surrogateInstanceId integer,
	p_keysToAssociateId raw_table,
	p_keysToAssociateData raw_table,
	p_encodingOption integer,
	p_singleKeyId raw
) return integer
is
  i integer;
  p_surrogate_keyid integer;
  p_count integer;
begin
  for i in 1 .. p_keysToAssociateId.Count - 1 loop
    select seq_KeysTable.nextval into p_surrogate_keyid from dual;

    insert into KeysTable (Id, SurrogateKeyId, SurrogateInstanceId, IsAssociated, Properties, EncodingOption)
		values (p_keysToAssociateId(i), p_surrogate_keyid, p_surrogateInstanceId, 1, p_keysToAssociateData(i), p_encodingOption);
	end loop;
		
	if (p_singleKeyId is not null) then
		update KeysTable
		set Properties = p_keysToAssociateData(1),
			EncodingOption = p_encodingOption
		where (Id = p_singleKeyId) and (SurrogateInstanceId = p_surrogateInstanceId);
			  
		if (SQL%rowcount != 1) then
      select count(*) into p_count from KeysTable where Id = p_singleKeyId;
			if p_count > 0 then
				return 3;
			end if;
			
      select seq_KeysTable.nextval into p_surrogate_keyid from dual;

			insert into KeysTable (Id, SurrogateKeyId, SurrogateInstanceId, IsAssociated, Properties, EncodingOption)
			values (p_singleKeyId, p_surrogate_keyid, p_surrogateInstanceId, 1, p_keysToAssociateData(1), p_encodingOption);
		end if;
	end if;

  return 0;
end;

function CompleteKeys(
	p_surrogateInstanceId integer,
	p_keysToComplete raw_table
) return integer
is
begin	
	if (p_keysToComplete is not null) then
    for i in 1 .. p_keysToComplete.Count - 1 loop
		  update KeysTable
		  set IsAssociated = 0
  		where SurrogateInstanceId = p_surrogateInstanceId and Id = p_keysToComplete(i);
		
		  if SQL%NOTFOUND then
			  return 4;
		  end if;
    end loop;
	end if;
  return 0;
end;

function FreeKeys(
	p_surrogateInstanceId integer,
	p_keysToFree raw_table
) return integer
is
begin	
	if (p_keysToFree is not null) then
    for i in 1 .. p_keysToFree.Count - 1 loop
		  delete from KeysTable
  		where SurrogateInstanceId = p_surrogateInstanceId and Id = p_keysToFree(i);
		
		  if SQL%NOTFOUND then
			  return 4;
		  end if;
    end loop;
	end if;
  return 0;
end;

procedure CreateInstance(
	p_instanceId raw,
	p_surrogateLockOwnerId integer,
	p_workflowHostType raw,
	p_serviceDeploymentId integer,
	p_surrogateInstanceId out integer
)
is
begin
  select seq_InstancesTable.nextval into p_surrogateInstanceId from dual;
	
	insert into InstancesTable (Id, SurrogateInstanceId, SurrogateLockOwnerId, CreationTime, WorkflowHostType, ServiceDeploymentId, Version)
	values (p_instanceId, p_surrogateInstanceId, p_surrogateLockOwnerId, sysdate, p_workflowHostType, p_serviceDeploymentId, 1);
end;

function LockInstance(
	p_instanceId raw,
	p_surrogateLockOwnerId integer,
	p_handleInstanceVersion integer,
	p_handleIsBoundToLock integer,
	p_surrogateInstanceId out integer,
	p_lockVersion out integer
) return integer
is
  p_isCompleted integer;
	p_currentLockOwnerId integer;
	p_currentVersion integer;
  p_count integer;
  p_result integer;
begin

<<TryLockInstance>>
	p_currentLockOwnerId := 0;
	p_surrogateInstanceId := 0;
	p_result := 0;
	
  begin
	select SurrogateInstanceId,
    (case when (InstancesTable.SurrogateLockOwnerId is null or InstancesTable.SurrogateLockOwnerId != p_surrogateLockOwnerId)
			then Version + 1
  		else Version
		end)
  into p_surrogateInstanceId, p_lockVersion
	from InstancesTable
	left outer join LockOwnersTable on InstancesTable.SurrogateLockOwnerId = LockOwnersTable.SurrogateLockOwnerId
	where (InstancesTable.Id = p_instanceId) and
		  (InstancesTable.IsCompleted = 0) and
		  (
		   (p_handleIsBoundToLock = 0 and
		    (
		     (InstancesTable.SurrogateLockOwnerId is null) or
		     (LockOwnersTable.SurrogateLockOwnerId is null) or
			  (
		       (LockOwnersTable.LockExpiration < sysdate) and
               (LockOwnersTable.SurrogateLockOwnerId != p_surrogateLockOwnerId)
			  )
		    )
		   ) or 
		   (
			(p_handleIsBoundToLock = 1) and
		    (LockOwnersTable.SurrogateLockOwnerId = p_surrogateLockOwnerId) and
		    (LockOwnersTable.LockExpiration > sysdate) and
		    (InstancesTable.Version = p_handleInstanceVersion)
		   )
		  ) for update;
  exception
    when no_data_found then
	
		select count(*) into p_count from LockOwnersTable where (SurrogateLockOwnerId = p_surrogateLockOwnerId) and (LockExpiration > sysdate);
		if p_count = 0 then
    	select count(*) into p_count from LockOwnersTable where SurrogateLockOwnerId = p_surrogateLockOwnerId;
      if p_count > 0 then
				return 11;
			else
				return 12;
			end if;
		end if;
		
    begin
		select SurrogateLockOwnerId, IsCompleted, Version
    into p_currentLockOwnerId, p_isCompleted, p_currentVersion
		from InstancesTable
		where Id = p_instanceId;
	
			if (p_isCompleted = 1) then
				p_result := 7;
			elsif (p_currentLockOwnerId = p_surrogateLockOwnerId) then
				if (p_handleIsBoundToLock = 1) then
					p_result := 10;
				else
					p_result := 14;
        end if;
			elsif (p_handleIsBoundToLock = 0) then
				p_result := 2;
			else
				p_result := 6;
      end if;
    exception
      when no_data_found then
		    if (p_handleIsBoundToLock = 1) then
			    p_result := 6;
        end if;
    end;

	  if (p_result = 2) then
      select count(1) into p_count from LockOwnersTable
		    inner join InstancesTable on InstancesTable.SurrogateLockOwnerId = LockOwnersTable.SurrogateLockOwnerId
		    where InstancesTable.SurrogateLockOwnerId = p_currentLockOwnerId and
			    InstancesTable.Id = p_instanceId;
      if p_count = 0 then
        goto TryLockInstance;
      end if;
	  end if;
    return p_result;
	end;

  update InstancesTable
 	set SurrogateLockOwnerId = p_surrogateLockOwnerId, Version = p_lockVersion
  where Id = p_instanceId;

  return 0;
end;

procedure UnlockInstance(
	p_instanceId raw,
	p_surrogateLockOwnerId integer,
	p_handleInstanceVersion integer
)
is
	p_surrogateInstanceId integer;
	p_workflowHostType raw(16);
	p_serviceDeploymentId integer;
  p_pendingTimer date;
	p_isReadyToRun integer;
	p_isSuspended integer;
begin
		
	select SurrogateInstanceId, WorkflowHostType, ServiceDeploymentId, PendingTimer, IsReadyToRun, IsSuspended
  into p_surrogateInstanceId, p_workflowHostType, p_serviceDeploymentId, p_pendingTimer, p_isReadyToRun, p_isSuspended
  from InstancesTable
	where Id = p_instanceId and
		  SurrogateLockOwnerId = p_surrogateLockOwnerId and
		  Version = p_handleInstanceVersion for update;
	
	update InstancesTable
	set SurrogateLockOwnerId = null
	where Id = p_instanceId and
		  SurrogateLockOwnerId = p_surrogateLockOwnerId and
		  Version = p_handleInstanceVersion;
	
	InsertRunnableInstanceEntry(p_surrogateInstanceId, p_workflowHostType, p_serviceDeploymentId, p_isSuspended, p_isReadyToRun, p_pendingTimer);
end;

procedure DetectRunnableInstances(
	p_workflowHostType raw,
  p_nextRunnableTime out date,
  p_now out date
)
is
begin
  p_now := sysdate;
	select RunnableInstancesTable.RunnableTime into p_nextRunnableTime
			  from RunnableInstancesTable
			  where WorkflowHostType = p_workflowHostType and rownum <= 1
			  order by WorkflowHostType, RunnableTime;
exception
  when no_data_found then
    p_nextRunnableTime := null;
end;

procedure GetActivatableWorkflows(
	p_machineName varchar2,
  p_result out SYS_REFCURSOR
)
is
  p_now date;
begin
	p_now := sysdate;
	
  open p_result for
	select serviceDeployments.SiteName, serviceDeployments.RelativeApplicationPath, serviceDeployments.RelativeServicePath
	from (
		select distinct ServiceDeploymentId, WorkflowHostType
		from RunnableInstancesTable
		where RunnableTime <= p_now
		) runnableWorkflows inner join ServiceDeploymentsTable serviceDeployments
		on runnableWorkflows.ServiceDeploymentId = serviceDeployments.Id
	where rownum <= 1000 and not exists (
						select 1
						from LockOwnersTable lockOwners
						where lockOwners.LockExpiration > p_now
						and lockOwners.MachineName = p_machineName
						and lockOwners.WorkflowHostType = runnableWorkflows.WorkflowHostType
					  );
end;

function LoadInstance(
	p_surrogateLockOwnerId integer,
	p_operationType integer,
	p_keyToLoadBy raw,
	p_instanceId raw,
	p_handleInstanceVersion integer,
	p_handleIsBoundToLock integer,
	p_keysToAssociateId raw_table,
	p_keysToAssociateData raw_table,
	p_singleKeyId raw,
	p_encodingOption integer,
	p_operationTimeout integer,
  p_instance out sys_refcursor,
  p_changes out sys_refcursor,
  p_keys out sys_refcursor
) return integer
is
  p_result integer;
	p_lockAcquired integer;
	p_isInitialized integer;
	p_createKey integer;
	p_createdInstance integer;
	p_keyIsAssociated integer;
	p_loadedByKey integer;
	p_now date;
	p_surrogateInstanceId integer;
  p_id raw(16);
  p_lockVersion integer;
  p_count integer;
begin
	p_createdInstance := 0;
	p_isInitialized := 0;
	p_keyIsAssociated := 0;
	p_result := 0;
	p_surrogateInstanceId := null;
	p_now := sysdate;
  p_id := p_instanceid;

<<MapKeyToInstanceId>>
	if (p_operationType = 0) or (p_operationType = 2) then
		p_loadedByKey := 0;
		p_createKey := 0;
			
    begin
		  select SurrogateInstanceId, IsAssociated
      into p_surrogateInstanceId, p_keyIsAssociated
			  from KeysTable
			  where Id = p_keyToLoadBy;
		exception
      when no_data_found then
			  if (p_operationType = 2) then
				  p_result := 4;
			  end if;
			  p_createKey := 1;
		end;

		if (p_keyIsAssociated = 0) and (p_result = 0) then
			p_result := 8;
		else
      begin
			  select Id into p_Id
				  from InstancesTable
				  where SurrogateInstanceId = p_surrogateInstanceId;
      exception
        when no_data_found then
				  goto MapKeyToInstanceId;
      end;
			p_loadedByKey := 1;
		end if;
	end if;

	if (p_result = 0) then
<<LockOrCreateInstance>>
		p_result := LockInstance(p_Id, p_surrogateLockOwnerId, p_handleInstanceVersion, p_handleIsBoundToLock, p_surrogateInstanceId, p_lockVersion);
														  
		if (p_result = 0 and p_surrogateInstanceId = 0) then
			if (p_loadedByKey = 1) then
				goto MapKeyToInstanceId;
      end if;
			
			if (p_operationType > 1) then
				p_result := 1;
			else
				CreateInstance(p_Id, p_surrogateLockOwnerId, null, null, p_surrogateInstanceId);
				p_createdInstance := 1;
			end if;
		elsif (p_result = 0) then
			delete from RunnableInstancesTable
			where SurrogateInstanceId = p_surrogateInstanceId;
		end if;
	end if;
		
	if (p_result = 0) then
		if (p_createKey = 1) then
			select IsInitialized into p_isInitialized
			from InstancesTable
			where SurrogateInstanceId = p_surrogateInstanceId;
			
			if (p_isInitialized = 1) then
				p_result := 5;
			else
				insert into KeysTable (Id, SurrogateInstanceId, IsAssociated)
				values (p_keyToLoadBy, p_surrogateInstanceId, 1);
			end if;
		elsif (p_loadedByKey = 1) then
      select count(*) into p_count from KeysTable where (Id = p_keyToLoadBy) and (IsAssociated = 1);
      if (p_count = 0) then
			  p_result := 8;
      end if;
		end if;
		
		if (p_operationType > 1) then
      select count(*) into p_count from InstancesTable where (Id = p_Id) and (IsInitialized = 1);
      if (p_count = 0) then
			  p_result := 1;
      end if;
		end if;
		
		if (p_result = 0) then
			p_result := AssociateKeys(p_surrogateInstanceId, p_keysToAssociateId, p_keysToAssociateData, p_encodingOption, p_singleKeyId);
    end if;
		
		-- Ensure that this key's data will never be overwritten.
		if (p_result = 0 and p_createKey = 1) then
			update KeysTable
			set EncodingOption = p_encodingOption
			where Id = p_keyToLoadBy;
		end if;
	end if;
	
	if (p_result = 0) then
    open p_instance for
		select Id,
			   SurrogateInstanceId,
			   PrimitiveDataProperties,
			   ComplexDataProperties,
			   MetadataProperties,
			   DataEncodingOption,
			   MetadataEncodingOption,
			   Version,
			   IsInitialized,
			   p_createdInstance
		from InstancesTable
		where SurrogateInstanceId = p_surrogateInstanceId;
		
		if (p_createdInstance = 0) then
      open p_changes for
			select EncodingOption, Change
			from InstanceMetadataChangesTable
			where SurrogateInstanceId = p_surrogateInstanceId
			order by(ChangeTime);
			
      open p_keys for
			select Id,
				   IsAssociated,
				   EncodingOption,
				   Properties
			from KeysTable
			where (KeysTable.SurrogateInstanceId = p_surrogateInstanceId);
		end if;
	end if;

	if not (p_result = 0 or p_result = 2 or p_result = 14) then
		rollback;
  end if;

  return p_result;
end;

function TryLoadRunnableInstance(
	p_surrogateLockOwnerId integer,
	p_workflowHostType raw,
	p_operationType integer,
	p_handleInstanceVersion integer,
	p_handleIsBoundToLock integer,
	p_encodingOption integer,	
	p_operationTimeout integer,
  p_hasInstance out integer,
  p_instance out sys_refcursor,
  p_changes out sys_refcursor,
  p_keys out sys_refcursor
) return integer
is
  p_instanceId raw(16);
  p_raw_table raw_table;
  p_count integer;
begin
  begin
	  select instances.Id
    into p_instanceid
	  from RunnableInstancesTable runnableInstances
		  inner join InstancesTable instances
		  on runnableInstances.SurrogateInstanceId = instances.SurrogateInstanceId
	  where runnableInstances.WorkflowHostType = p_workflowHostType
		    and runnableInstances.RunnableTime <= sysdate
        and rownum <= 1;
  exception
    when no_data_found then
      p_hasInstance := 0;
      return 0;
  end;
    
  p_hasInstance := 1;
  return LoadInstance(p_surrogateLockOwnerId, p_operationType, null, p_instanceId, p_handleInstanceVersion, p_handleIsBoundToLock, p_raw_table, p_raw_table, null, p_encodingOption, p_operationTimeout,
    p_instance, p_changes, p_keys);
end;

procedure DeleteInstance(
	p_surrogateInstanceId integer
)
is
begin	
	/*delete InstancePromotedPropertiesTable
	where SurrogateInstanceId = p_surrogateInstanceId;*/
		
	delete KeysTable
	where SurrogateInstanceId = p_surrogateInstanceId;
		
	delete InstanceMetadataChangesTable
	where SurrogateInstanceId = p_surrogateInstanceId;

	delete RunnableInstancesTable 
	where SurrogateInstanceId = p_surrogateInstanceId;

	delete InstancesTable 
	where SurrogateInstanceId = p_surrogateInstanceId;
end;

procedure CreateServiceDeployment(
	p_serviceDeploymentHash raw,
	p_siteName varchar,
	p_relativeServicePath varchar,
	p_relativeApplicationPath varchar,
	p_serviceName varchar,
  p_serviceNamespace varchar,
  p_serviceDeploymentId out integer
)
is
begin
		--Create or select the service deployment id
  begin
		select Id into p_serviceDeploymentId
			from ServiceDeploymentsTable
			where ServiceDeploymentHash = p_serviceDeploymentHash;
  exception
    when no_data_found then
      
      select seq_ServiceDeployments.nextval into p_serviceDeploymentId from dual;
		  
      insert into ServiceDeploymentsTable
			(Id, ServiceDeploymentHash, SiteName, RelativeServicePath, RelativeApplicationPath, ServiceName, ServiceNamespace)
			values (p_serviceDeploymentId, p_serviceDeploymentHash, p_siteName, p_relativeServicePath, p_relativeApplicationPath, p_serviceName, p_serviceNamespace);
  end;
end;

function SaveInstance(
	p_instanceId raw,
	p_surrogateLockOwnerId integer,
	p_handleInstanceVersion integer,
	p_handleIsBoundToLock integer,
	p_primitiveDataProperties blob,
	p_complexDataProperties blob,
	p_wOPrimitiveDataProperties blob,
	p_wOComplexDataProperties blob,
	p_metadataProperties blob,
	p_metadataIsConsistent integer,
	p_encodingOption integer,
	p_timerDurationMilliseconds integer,
	p_suspensionStateChange integer,
	p_suspensionReason varchar,
	p_suspensionExceptionName varchar,
	p_keysToAssociateId raw_table,
	p_keysToAssociateData raw_table,
	p_keysToComplete raw_table,
	p_keysToFree raw_table,
	p_unlockInstance integer,
	p_isReadyToRun integer,
	p_isCompleted integer,
	p_singleKeyId raw,
	p_lastMachineRunOn varchar,
	p_executionStatus varchar,
	p_blockingBookmarks varchar,
	p_workflowHostType raw,
	p_serviceDeploymentId integer,
	p_operationTimeout integer,
  p_currentInstanceVersion out integer
) return integer
is
	p_deleteInstanceOnCompletion integer;
	p_enqueueCommand integer;
	p_isSuspended integer;
	p_lockAcquired integer;
	p_metadataUpdateOnly integer;
	p_now date;
	p_result integer;
	p_surrogateInstanceId integer;
	p_pendingTimer date;
	p_workflowHostType2 raw(16);
	p_serviceDeploymentId2 integer;
	p_isReadyToRun2 integer;
begin
	p_result := 0;
	p_metadataUpdateOnly := 0;
	p_now := sysdate;
	
	if (p_primitiveDataProperties is null and p_complexDataProperties is null and p_wOPrimitiveDataProperties is null and p_wOComplexDataProperties is null) then
		p_metadataUpdateOnly := 1;
  end if;

<<LockOrCreateInstance>>
	if (p_result = 0) then
		p_result := LockInstance(p_instanceId, p_surrogateLockOwnerId, p_handleInstanceVersion, p_handleIsBoundToLock, p_surrogateInstanceId, p_currentInstanceVersion);
															  
		if (p_result = 0 and p_surrogateInstanceId = 0) then
			CreateInstance(p_instanceId, p_surrogateLockOwnerId, p_workflowHostType, p_serviceDeploymentId, p_surrogateInstanceId);
			
			if (p_result = 0 and p_surrogateInstanceId = 0) then
				goto LockOrCreateInstance;
      end if;
			
			p_currentInstanceVersion := 1;
		end if;
	end if;
	
	if (p_result = 0) then
		select EnqueueCommand, DeletesInstanceOnCompletion
    into p_enqueueCommand, p_deleteInstanceOnCompletion
		from LockOwnersTable
		where (SurrogateLockOwnerId = p_surrogateLockOwnerId);
		
		if (p_isCompleted = 1 and p_deleteInstanceOnCompletion = 1) then
			DeleteInstance(p_surrogateInstanceId);
			return p_result;
		end if;
		
    select
			case when (p_workflowHostType is null)
				then WorkflowHostType
				else p_workflowHostType 
			end,
			case when (p_serviceDeploymentId is null)
				then ServiceDeploymentId
				else p_serviceDeploymentId 
			end,
			case when (p_metadataUpdateOnly = 1)
				then PendingTimer
				else GetExpirationTime(p_timerDurationMilliseconds)
			end,
			case when (p_metadataUpdateOnly = 1)
				then IsReadyToRun
				else p_isReadyToRun
			end,
			case when (p_suspensionStateChange = 0) then IsSuspended
				when (p_suspensionStateChange = 1) then 1
				else 0
			end
    into p_workflowHostType2, p_serviceDeploymentId2, p_pendingTimer, p_isReadyToRun2, p_isSuspended
    from InstancesTable
		where (InstancesTable.SurrogateInstanceId = p_surrogateInstanceId);

		update InstancesTable
		set
			WorkflowHostType = p_workflowHostType2,
			ServiceDeploymentId = p_serviceDeploymentId2,
			PendingTimer = p_pendingTimer,
			IsReadyToRun = p_isReadyToRun2,
			IsSuspended = p_isSuspended,
			SurrogateLockOwnerId = case when (p_unlockInstance = 1 or p_isCompleted = 1)
										then null
										else p_surrogateLockOwnerId
									 end,
			PrimitiveDataProperties = case when (p_metadataUpdateOnly = 1)
										then PrimitiveDataProperties
										else p_primitiveDataProperties
									   end,
			ComplexDataProperties = case when (p_metadataUpdateOnly = 1)
										then ComplexDataProperties
										else p_complexDataProperties
									   end,
			WOPrimitiveDataProperties = case when (p_metadataUpdateOnly = 1)
										then WOPrimitiveDataProperties
										else p_wOPrimitiveDataProperties
									   end,
			WOComplexDataProperties = case when (p_metadataUpdateOnly = 1)
										then WOComplexDataProperties
										else p_wOComplexDataProperties
									   end,
			MetadataProperties = case
									 when (p_metadataIsConsistent = 1) then p_metadataProperties
									 else MetadataProperties
								   end,
			SuspensionReason = case
									when (p_suspensionStateChange = 0) then SuspensionReason
									when (p_suspensionStateChange = 1) then p_suspensionReason
									else null
								 end,
			SuspensionExceptionName = case
									when (p_suspensionStateChange = 0) then SuspensionExceptionName
									when (p_suspensionStateChange = 1) then p_suspensionExceptionName
									else null
								 end,
			IsCompleted = p_isCompleted,
			IsInitialized = case
								when (p_metadataUpdateOnly = 0) then 1
								else IsInitialized
							  end,
			DataEncodingOption = case
									when (p_metadataUpdateOnly = 0) then p_encodingOption
									else DataEncodingOption
								   end,
			MetadataEncodingOption = case
									when (p_metadataIsConsistent = 1) then p_encodingOption
									else MetadataEncodingOption
								   end,
			BlockingBookmarks = case
									when (p_metadataUpdateOnly = 0) then p_blockingBookmarks
									else BlockingBookmarks
								  end,
			LastUpdated = p_now,
			LastMachineRunOn = case
									when (p_metadataUpdateOnly = 0) then p_lastMachineRunOn
									else LastMachineRunOn
								 end,
			ExecutionStatus = case
									when (p_metadataUpdateOnly = 0) then p_executionStatus
									else ExecutionStatus
								end
		where (InstancesTable.SurrogateInstanceId = p_surrogateInstanceId);
	
		if (p_keysToAssociateId is not null or p_singleKeyId is not null) then
			p_result := AssociateKeys(p_surrogateInstanceId, p_keysToAssociateId, p_keysToAssociateData, p_encodingOption, p_singleKeyId);
    end if;
    			
		if (p_result = 0 and p_keysToComplete is not null) then
			p_result := CompleteKeys(p_surrogateInstanceId, p_keysToComplete);
    end if;
			
		if (p_result = 0 and p_keysToFree is not null) then
			p_result := FreeKeys(p_surrogateInstanceId, p_keysToFree);
    end if;
			
		/*if (p_result = 0) and (p_metadataUpdateOnly = 0) then
			delete from InstancePromotedPropertiesTable
			where SurrogateInstanceId = p_surrogateInstanceId;
    end if;*/
			
		if (p_result = 0) then
			if (p_metadataIsConsistent = 1) then

				delete from InstanceMetadataChangesTable
				where SurrogateInstanceId = p_surrogateInstanceId;

			elsif (p_metadataProperties is not null) then

				insert into InstanceMetadataChangesTable (SurrogateInstanceId, EncodingOption, Change)
				values (p_surrogateInstanceId, p_encodingOption, p_metadataProperties);

			end if;
		end if;
			
		if (p_result = 0 and p_unlockInstance = 1 and p_isCompleted = 0) then
			InsertRunnableInstanceEntry(p_surrogateInstanceId, p_workflowHostType2, p_serviceDeploymentId2, p_isSuspended, p_isReadyToRun2, p_pendingTimer);
    end if;

  end if;

  if p_result <> 0 then
    rollback;
  end if;

	return p_result;
end;

end;
