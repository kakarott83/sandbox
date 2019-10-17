create table wf_record_type
(
  type_id number(2) primary key,
  type_name varchar(100)
);

insert into wf_record_type(type_id, type_name) values (1, 'ActivityScheduledRecord');
insert into wf_record_type(type_id, type_name) values (2, 'ActivityStateRecord');
insert into wf_record_type(type_id, type_name) values (3, 'BookmarkResumptionRecord');
insert into wf_record_type(type_id, type_name) values (4, 'CancelRequestedRecord');
insert into wf_record_type(type_id, type_name) values (5, 'CustomTrackingRecord');
insert into wf_record_type(type_id, type_name) values (6, 'FaultPropagationRecord');
insert into wf_record_type(type_id, type_name) values (7, 'WorkflowInstanceRecord');
insert into wf_record_type(type_id, type_name) values (8, 'WorkflowInstanceAbortedRecord');
insert into wf_record_type(type_id, type_name) values (9, 'WorkflowInstanceSuspendedRecord');
insert into wf_record_type(type_id, type_name) values (10, 'WorkflowInstanceTerminatedRecord');
insert into wf_record_type(type_id, type_name) values (11, 'WorkflowInstanceUnhandledExceptionRecord');
insert into wf_record_type(type_id, type_name) values (12, 'StateMachineStateRecord');
insert into wf_record_type(type_id, type_name) values (13, 'InteropTrackingRecord');
insert into wf_record_type(type_id, type_name) values (14, 'ReceiveMessageRecord');
insert into wf_record_type(type_id, type_name) values (15, 'SendMessageRecord');

create table wf_tracking_record
(
  record_id number(18) primary key,
  record_number number(18),
  record_type number(2) references wf_record_type(type_id),
  instance_id raw(16),
  trace_level number(2),
  event_time date
);

create sequence seq_wf_tracking_record;

create table wf_annotation
(
  record_id number(18) references wf_tracking_record(record_id),
  key varchar(100),
  annotation varchar(4000),
  primary key (record_id, key)
);

create table wf_instance_record
(
  record_id number(18) primary key references wf_tracking_record(record_id),
  activity_definition_id varchar(4000),
  state varchar(100),
  reason varchar(4000)
);

create table wf_activity_info
(
  activity_info_id number(18) primary key,
  activity_id varchar(100) not null,
  instance_id raw(16) not null,
  name varchar(1000)  not null,
  type_name varchar(1000) not null,
  unique (activity_id, instance_id, name, type_name)
);

create sequence seq_wf_activity_info;

create table wf_unhandled_exception_record
(
  record_id number(18) primary key references wf_instance_record(record_id),
  fault_source number(18) references wf_activity_info(activity_info_id),
  exception_type varchar(100),
  exception_data varchar(4000)
);

create table wf_value_collection
(
  collection_id number(18) primary key
);

create sequence seq_wf_value_collection;

create table wf_value
(
  collection_id number(18) references wf_value_collection,
  order_ number(9),
  name varchar(100) not null,
  value varchar(4000),
  primary key (collection_id, order_)
);

create table wf_activity_state_record
(
  record_id number(18) primary key references wf_tracking_record(record_id),
  state varchar(100),
  activity number(18) references wf_activity_info(activity_info_id),
  arguments_id number(18) references wf_value_collection(collection_id),
  variables_id number(18) references wf_value_collection(collection_id)
);

create table wf_activity_scheduled_record
(
  record_id number(18) primary key references wf_tracking_record(record_id),
  activity number(18) references wf_activity_info(activity_info_id),
  child number(18) references wf_activity_info(activity_info_id)
);

create table wf_fault_propagation_record
(
  record_id number(18) primary key references wf_tracking_record(record_id),
  fault_handler number(18) references wf_activity_info(activity_info_id),
  fault_source number(18) references wf_activity_info(activity_info_id),
  is_fault_source number(1),
  fault_type varchar(100),
  fault_data varchar(4000)
);

create table wf_cancel_requested_record
(
  record_id number(18) primary key references wf_tracking_record(record_id),
  child number(18) references wf_activity_info(activity_info_id)
);

create table wf_bookmark_resumption_record
(
  record_id number(18) primary key references wf_tracking_record(record_id),
  bookmark_name varchar(1000),
  bookmark_scope raw(16),
  owner number(18) references wf_activity_info(activity_info_id),
  payload varchar(4000)
);

create table wf_custom_tracking_record
(
  record_id number(18) primary key references wf_tracking_record(record_id),
  activity number(18) references wf_activity_info(activity_info_id),
  name varchar(100),
  data_id number(18) references wf_value_collection(collection_id)
);
