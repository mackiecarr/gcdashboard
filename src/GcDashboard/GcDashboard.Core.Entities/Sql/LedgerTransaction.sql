create table ledger_transaction (
  ledger_transaction_id integer primary key autoincrement,
  nh_version integer,
  date_entered date,
  date_posted date,
  description text,
  gnucash_identifier uniqueidentifier,
  notes text
)
