create or replace package wf_tracking is

  type varchar_table is table of varchar(32767) index by binary_integer;

  function insert_instance_record(
    p_record_number number,
    p_record_type_name varchar,
    p_instance_id raw,
    p_trace_level integer,
    p_event_time date,
    p_annotation_key varchar_table,
    p_annotation varchar_table,
    p_activity_definition_id varchar,
    p_state varchar,
    p_reason varchar
  ) return integer;

  function insert_exception_record(
    p_record_number number,
    p_record_type_name varchar,
    p_instance_id raw,
    p_trace_level integer,
    p_event_time date,
    p_annotation_key varchar_table,
    p_annotation varchar_table,
    p_activity_definition_id varchar,
    p_state varchar,
    p_fault_source_id varchar,
    p_fault_source_instance_id raw,
    p_fault_source_name varchar,
    p_fault_source_type_name varchar,
    p_exception_type varchar,
    p_exception_data varchar
  ) return integer;

  function insert_activity_state_record(
    p_record_number number,
    p_record_type_name varchar,
    p_instance_id raw,
    p_trace_level integer,
    p_event_time date,
    p_annotation_key varchar_table,
    p_annotation varchar_table,
    p_state varchar,
    p_activity_id varchar,
    p_activity_instance_id raw,
    p_activity_name varchar,
    p_activity_type_name varchar,
    p_argument_names varchar_table,
    p_argument_values varchar_table,
    p_variable_names varchar_table,
    p_variable_values varchar_table
  ) return integer;

  function insert_scheduled_record(
    p_record_number number,
    p_record_type_name varchar,
    p_instance_id raw,
    p_trace_level integer,
    p_event_time date,
    p_annotation_key varchar_table,
    p_annotation varchar_table,
    p_activity_id varchar,
    p_activity_instance_id raw,
    p_activity_name varchar,
    p_activity_type_name varchar,
    p_child_id varchar,
    p_child_instance_id raw,
    p_child_name varchar,
    p_child_type_name varchar
  ) return integer;

  function insert_fault_record(
    p_record_number number,
    p_record_type_name varchar,
    p_instance_id raw,
    p_trace_level integer,
    p_event_time date,
    p_annotation_key varchar_table,
    p_annotation varchar_table,
    p_fault_handler_id varchar,
    p_fault_handler_instance_id raw,
    p_fault_handler_name varchar,
    p_fault_handler_type_name varchar,
    p_fault_source_id varchar,
    p_fault_source_instance_id raw,
    p_fault_source_name varchar,
    p_fault_source_type_name varchar,
    p_is_fault_source integer,
    p_fault_type varchar,
    p_fault_data varchar
  ) return integer;

  function insert_cancel_record(
    p_record_number number,
    p_record_type_name varchar,
    p_instance_id raw,
    p_trace_level integer,
    p_event_time date,
    p_annotation_key varchar_table,
    p_annotation varchar_table,
    p_child_id varchar,
    p_child_instance_id raw,
    p_child_name varchar,
    p_child_type_name varchar
  ) return integer;

  function insert_bookmark_record(
    p_record_number number,
    p_record_type_name varchar,
    p_instance_id raw,
    p_trace_level integer,
    p_event_time date,
    p_annotation_key varchar_table,
    p_annotation varchar_table,
    p_bookmark_name varchar,
    p_bookmark_scope raw,
    p_owner_id varchar,
    p_owner_instance_id raw,
    p_owner_name varchar,
    p_owner_type_name varchar,
    p_payload varchar
  ) return integer;

  function insert_custom_record(
    p_record_number number,
    p_record_type_name varchar,
    p_instance_id raw,
    p_trace_level integer,
    p_event_time date,
    p_annotation_key varchar_table,
    p_annotation varchar_table,
    p_activity_id varchar,
    p_activity_instance_id raw,
    p_activity_name varchar,
    p_activity_type_name varchar,
    p_name varchar,
    p_data_names varchar_table,
    p_data_values varchar_table
  ) return integer;
end;
/

create or replace package body wf_tracking is

  function insert_tracking_record(
    p_record_number number,
    p_record_type_name varchar,
    p_instance_id raw,
    p_trace_level integer,
    p_event_time date,
    p_annotation_key varchar_table,
    p_annotation varchar_table
  ) return integer
  is
    p_record_id integer;
    p_record_type integer;
    i integer;
  begin
    select seq_wf_tracking_record.nextval, type_id into p_record_id, p_record_type
    from wf_record_type
    where type_name = p_record_type_name;

    insert into wf_tracking_record(record_id, record_number, record_type, instance_id, trace_level, event_time)
    values (p_record_id, p_record_number, p_record_type, p_instance_id, p_trace_level, p_event_time);

    for i in 1..p_annotation_key.count loop
      if p_annotation_key(i) is not null then
        insert into wf_annotation(record_id, key, annotation)
        values (p_record_id, p_annotation_key(i), p_annotation(i));
      end if;
    end loop;

    return p_record_id;
  end;

  function insert_instance_record(
    p_record_number number,
    p_record_type_name varchar,
    p_instance_id raw,
    p_trace_level integer,
    p_event_time date,
    p_annotation_key varchar_table,
    p_annotation varchar_table,
    p_activity_definition_id varchar,
    p_state varchar,
    p_reason varchar
  ) return integer
  is
    p_record_id integer;
  begin
    p_record_id := insert_tracking_record(p_record_number, p_record_type_name, p_instance_id,
      p_trace_level, p_event_time, p_annotation_key, p_annotation);

    insert into wf_instance_record(record_id, activity_definition_id, state, reason)
    values (p_record_id, p_activity_definition_id, p_state, p_reason);

    return p_record_id;
  end;

  function insert_activity_info(
    p_activity_id varchar,
    p_instance_id raw,
    p_name varchar,
    p_type_name varchar
  ) return integer
  is
    p_activity_info_id integer;
  begin
    if p_activity_id is null then
      return null;
    end if;

    begin
      select activity_info_id into p_activity_info_id
      from wf_activity_info
      where activity_id = p_activity_id and instance_id = p_instance_id
        and name = p_name and type_name = p_type_name;
    exception
      when no_data_found then
        select seq_wf_activity_info.nextval into p_activity_info_id from dual;

        insert into wf_activity_info(activity_info_id, activity_id, instance_id, name, type_name)
        values (p_activity_info_id, p_activity_id, p_instance_id, p_name, p_type_name);
    end;

    return p_activity_info_id;
  end;

  function insert_exception_record(
    p_record_number number,
    p_record_type_name varchar,
    p_instance_id raw,
    p_trace_level integer,
    p_event_time date,
    p_annotation_key varchar_table,
    p_annotation varchar_table,
    p_activity_definition_id varchar,
    p_state varchar,
    p_fault_source_id varchar,
    p_fault_source_instance_id raw,
    p_fault_source_name varchar,
    p_fault_source_type_name varchar,
    p_exception_type varchar,
    p_exception_data varchar
  ) return integer
  is
    p_record_id integer;
    p_fault_source integer;
  begin
    p_fault_source := insert_activity_info(p_fault_source_id, p_fault_source_instance_id, p_fault_source_name, p_fault_source_type_name);
    
    p_record_id := insert_instance_record(p_record_number, p_record_type_name, p_instance_id, p_trace_level,
      p_event_time, p_annotation_key, p_annotation, p_activity_definition_id, p_state, null);

    insert into wf_unhandled_exception_record(record_id, fault_source, exception_type, exception_data)
    values (p_record_id, p_fault_source, p_exception_type, p_exception_data);

    return p_record_id;
  end;

  function insert_activity_state_record(
    p_record_number number,
    p_record_type_name varchar,
    p_instance_id raw,
    p_trace_level integer,
    p_event_time date,
    p_annotation_key varchar_table,
    p_annotation varchar_table,
    p_state varchar,
    p_activity_id varchar,
    p_activity_instance_id raw,
    p_activity_name varchar,
    p_activity_type_name varchar,
    p_argument_names varchar_table,
    p_argument_values varchar_table,
    p_variable_names varchar_table,
    p_variable_values varchar_table
  ) return integer
  is
    p_record_id integer;
    p_activity integer;
    p_arguments_id integer;
    p_variables_id integer;
    i integer;
  begin
    p_activity := insert_activity_info(p_activity_id, p_activity_instance_id, p_activity_name, p_activity_type_name);
    
    p_record_id := insert_tracking_record(p_record_number, p_record_type_name, p_instance_id, p_trace_level,
      p_event_time, p_annotation_key, p_annotation);

    if (p_argument_names.count > 0 and p_argument_names(1) is not null) then
      select seq_wf_value_collection.nextval into p_arguments_id from dual;

      insert into wf_value_collection(collection_id) values (p_arguments_id);

      for i in 1..p_argument_names.count loop
        insert into wf_value(collection_id, order_, name, value)
        values (p_arguments_id, i - 1, p_argument_names(i), p_argument_values(i));
      end loop;
    end if;

    if (p_variable_names.count > 0 and p_variable_names(1) is not null) then
      select seq_wf_value_collection.nextval into p_variables_id from dual;

      insert into wf_value_collection(collection_id) values (p_variables_id);

      for i in 1..p_variable_names.count loop
        insert into wf_value(collection_id, order_, name, value)
        values (p_variables_id, i - 1, p_variable_names(i), p_variable_values(i));
      end loop;
    end if;

    insert into wf_activity_state_record(record_id, state, activity, arguments_id, variables_id)
    values (p_record_id, p_state, p_activity, p_arguments_id, p_variables_id);

    return p_record_id;
  end;

  function insert_scheduled_record(
    p_record_number number,
    p_record_type_name varchar,
    p_instance_id raw,
    p_trace_level integer,
    p_event_time date,
    p_annotation_key varchar_table,
    p_annotation varchar_table,
    p_activity_id varchar,
    p_activity_instance_id raw,
    p_activity_name varchar,
    p_activity_type_name varchar,
    p_child_id varchar,
    p_child_instance_id raw,
    p_child_name varchar,
    p_child_type_name varchar
  ) return integer
  is
    p_record_id integer;
    p_activity integer;
    p_child integer;
  begin
    p_activity := insert_activity_info(p_activity_id, p_activity_instance_id, p_activity_name, p_activity_type_name);
    p_child := insert_activity_info(p_child_id, p_child_instance_id, p_child_name, p_child_type_name);
    
    p_record_id := insert_tracking_record(p_record_number, p_record_type_name, p_instance_id, p_trace_level,
      p_event_time, p_annotation_key, p_annotation);

    insert into wf_activity_scheduled_record(record_id, activity, child)
    values (p_record_id, p_activity, p_child);

    return p_record_id;
  end;

  function insert_fault_record(
    p_record_number number,
    p_record_type_name varchar,
    p_instance_id raw,
    p_trace_level integer,
    p_event_time date,
    p_annotation_key varchar_table,
    p_annotation varchar_table,
    p_fault_handler_id varchar,
    p_fault_handler_instance_id raw,
    p_fault_handler_name varchar,
    p_fault_handler_type_name varchar,
    p_fault_source_id varchar,
    p_fault_source_instance_id raw,
    p_fault_source_name varchar,
    p_fault_source_type_name varchar,
    p_is_fault_source integer,
    p_fault_type varchar,
    p_fault_data varchar
  ) return integer
  is
    p_record_id integer;
    p_fault_handler integer;
    p_fault_source integer;
  begin
    p_fault_handler := insert_activity_info(p_fault_handler_id, p_fault_handler_instance_id, p_fault_handler_name, p_fault_handler_type_name);
    p_fault_source := insert_activity_info(p_fault_source_id, p_fault_source_instance_id, p_fault_source_name, p_fault_source_type_name);
    
    p_record_id := insert_tracking_record(p_record_number, p_record_type_name, p_instance_id, p_trace_level,
      p_event_time, p_annotation_key, p_annotation);

    insert into wf_fault_propagation_record(record_id, fault_handler, fault_source, is_fault_source, fault_type, fault_data)
    values (p_record_id, p_fault_handler, p_fault_source, p_is_fault_source, p_fault_type, p_fault_data);

    return p_record_id;
  end;

  function insert_cancel_record(
    p_record_number number,
    p_record_type_name varchar,
    p_instance_id raw,
    p_trace_level integer,
    p_event_time date,
    p_annotation_key varchar_table,
    p_annotation varchar_table,
    p_child_id varchar,
    p_child_instance_id raw,
    p_child_name varchar,
    p_child_type_name varchar
  ) return integer
  is
    p_record_id integer;
    p_child integer;
  begin
    p_child := insert_activity_info(p_child_id, p_child_instance_id, p_child_name, p_child_type_name);
    
    p_record_id := insert_tracking_record(p_record_number, p_record_type_name, p_instance_id, p_trace_level,
      p_event_time, p_annotation_key, p_annotation);

    insert into wf_cancel_requested_record(record_id, child)
    values (p_record_id, p_child);

    return p_record_id;
  end;

  function insert_bookmark_record(
    p_record_number number,
    p_record_type_name varchar,
    p_instance_id raw,
    p_trace_level integer,
    p_event_time date,
    p_annotation_key varchar_table,
    p_annotation varchar_table,
    p_bookmark_name varchar,
    p_bookmark_scope raw,
    p_owner_id varchar,
    p_owner_instance_id raw,
    p_owner_name varchar,
    p_owner_type_name varchar,
    p_payload varchar
  ) return integer
  is
    p_record_id integer;
    p_owner integer;
  begin
    p_owner := insert_activity_info(p_owner_id, p_owner_instance_id, p_owner_name, p_owner_type_name);
    
    p_record_id := insert_tracking_record(p_record_number, p_record_type_name, p_instance_id, p_trace_level,
      p_event_time, p_annotation_key, p_annotation);

    insert into wf_bookmark_resumption_record(record_id, bookmark_name, bookmark_scope, owner, payload)
    values (p_record_id, p_bookmark_name, p_bookmark_scope, p_owner, p_payload);

    return p_record_id;
  end;

  function insert_custom_record(
    p_record_number number,
    p_record_type_name varchar,
    p_instance_id raw,
    p_trace_level integer,
    p_event_time date,
    p_annotation_key varchar_table,
    p_annotation varchar_table,
    p_activity_id varchar,
    p_activity_instance_id raw,
    p_activity_name varchar,
    p_activity_type_name varchar,
    p_name varchar,
    p_data_names varchar_table,
    p_data_values varchar_table
  ) return integer
  is
    p_record_id integer;
    p_activity integer;
    p_data_id integer;
    i integer;
  begin
    p_activity := insert_activity_info(p_activity_id, p_activity_instance_id, p_activity_name, p_activity_type_name);
    
    p_record_id := insert_tracking_record(p_record_number, p_record_type_name, p_instance_id, p_trace_level,
      p_event_time, p_annotation_key, p_annotation);

    if (p_data_names.count > 0 and p_data_names(1) is not null) then
      select seq_wf_value_collection.nextval into p_data_id from dual;

      insert into wf_value_collection(collection_id) values (p_data_id);

      for i in 1..p_data_names.count loop
        insert into wf_value(collection_id, order_, name, value)
        values (p_data_id, i - 1, p_data_names(i), p_data_values(i));
      end loop;
    end if;

    insert into wf_custom_tracking_record(record_id, activity, name, data_id)
    values (p_record_id, p_activity, p_name, p_data_id);

    return p_record_id;
  end;

end;
/
