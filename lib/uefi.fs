struct s-uefi-systable {
  header 24
  firmware-vendor *
  firmware-revision 8
  console-in-handle *
  con-in *
  console-out-handle *
  con-out *
  stderr-handle *
  stderr *
  runtime-services *
  boot-services *
  num-table-entries 8
  config-table *
}
