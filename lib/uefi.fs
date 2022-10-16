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

struct s-uefi-srv-boot {
  header 24
  raise-tpl *
  restore-tpl *
  allocate-pages *
  free-pages *
  get-memory-map *
  allocate-pool *
  free-pool *
  create-event *
  set-timer *
  wait-for-event *
  signal-event *
  close-event *
  check-event *
  install-protocol-interface *
  reinstall-protocol-interface *
  uninstall-protocol-interface *
  handle-protocol *
  reserved *
  register-protocol-notify *
  locate-handle *
  locate-device-path *
  install-config-table *
  load-image *
  start-image *
  exit *
  unload-image *
  exit-boot-srv *
  get-next-monotonic-count *
  stall *
  set-watchdog-timer *
  connect-controller *
  disconnect-controller *
  open-protocol *
  close-protocol *
  open-protocol-info *
  protocols-per-handle *
  locate-handle-buffer *
  locate-protocol *
  install-multiple-protocol-interfaces *
  uninstall-multiple-protocol-interfaces *
  calculate-crc32 *
  copy-mem *
  set-mem *
  create-event-ex *
}

struct s-uefi-srv-runtime {
  get-time *
  set-time *
  get-wakeup-time *
  set-wakeup-time *
  set-virt-address-map *
  convert-pointer *
  get-var *
  get-next-var-name *
  set-var *
  get-next-hight-monotonic-count *
  reset-system *
  update-capsule *
  query-capsule-caps *
  query-var-info *
}

struct s-uefi-memmap-desc {
  type 4
  pad 4
  phys *
  virt *
  pages *
  attr 8
}

val c-uefi-guid-gop
  blob dea94290dc23384a96fb7aded080516a
    to c-uefi-guid-gop

