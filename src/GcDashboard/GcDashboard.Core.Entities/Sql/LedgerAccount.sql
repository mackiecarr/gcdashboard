create table ledger_account (
  ledger_account_id integer primary key autoincrement,
  nh_version integer,
  name text,
  description text,
  full_name text,
  account_type_id integer,
  gnucash_identifier uniqueidentifier,
  parent_ledger_account_id integer,
  list_index integer,
  is_placeholder integer
)
