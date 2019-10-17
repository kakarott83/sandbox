--create user InstanceStore;

create table InstancesTable
(
	Id raw(16) primary key,
	SurrogateInstanceId integer not null,
	SurrogateLockOwnerId integer null,
	PrimitiveDataProperties blob,
	ComplexDataProperties blob,
	WOPrimitiveDataProperties blob,
	WOComplexDataProperties blob,
	MetadataProperties blob,
	DataEncodingOption number(1) default 0,
	MetadataEncodingOption number(1) default 0,
	Version integer not null,
	PendingTimer date null,
	CreationTime date not null,
	LastUpdated date default null,
	WorkflowHostType raw(16) null,
	ServiceDeploymentId integer null,
	SuspensionExceptionName varchar(450) default null,
	SuspensionReason varchar(4000) default null,
	BlockingBookmarks varchar(4000) default null,
	LastMachineRunOn varchar(450) default null,
	ExecutionStatus varchar(450) default null,
	IsInitialized number(1) default 0,
	IsSuspended number(1) default 0,
	IsReadyToRun number(1) default 0,
	IsCompleted number(1) default 0
);

create unique index IX_InstancesTable on InstancesTable (SurrogateInstanceId);

create sequence seq_InstancesTable;

create table RunnableInstancesTable
(
	SurrogateInstanceId integer primary key,
	WorkflowHostType raw(16) null,
	ServiceDeploymentId integer null,
	RunnableTime date not null
);

create table KeysTable
(
	Id raw(16) primary key,
	SurrogateKeyId integer not null,
	SurrogateInstanceId integer null,
	EncodingOption number(1) null,
	Properties blob,
	IsAssociated number(1)
);

create sequence seq_KeysTable;

create unique index IX_KeysTable on KeysTable (SurrogateInstanceId);

create table LockOwnersTable
(
	Id raw(16) primary key,
	SurrogateLockOwnerId integer not null,
	LockExpiration date not null,
	WorkflowHostType raw(16) null,
	MachineName varchar(128) not null,
	EnqueueCommand number(1) not null,
	DeletesInstanceOnCompletion number(1) not null,
	PrimitiveLockOwnerData blob,
	ComplexLockOwnerData blob,
	WOPrimitiveLockOwnerData blob,
	WOComplexLockOwnerData blob,
	EncodingOption number(1) default 0
);

create unique index IX_LockOwnersTable on LockOwnersTable (SurrogateLockOwnerId);

create sequence seq_LockOwnersTable;

create table InstanceMetadataChangesTable
(
	SurrogateInstanceId integer not null,
	ChangeTime integer primary key,
	EncodingOption number(1) not null,
	Change blob
);

create sequence seq_InstanceMetadataChanges;

create table ServiceDeploymentsTable
(
	Id integer primary key,
	ServiceDeploymentHash raw(16) not null,
	SiteName varchar(4000) not null,
	RelativeServicePath varchar(4000) not null,
	RelativeApplicationPath varchar(4000) not null,
	ServiceName varchar(4000) not null,
	ServiceNamespace varchar(4000) not null
);

create sequence seq_ServiceDeployments;
