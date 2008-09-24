create table ledger_transaction_split (
  ledger_transaction_split_id integer primary key autoincrement,
  ledger_transaction_id integer,
  ledger_account_id integer,
  list_index integer,
  nh_version integer,
  amount real,
  memo text,
  gnucash_identifier uniqueidentifier,
  reconciled_state text
)
