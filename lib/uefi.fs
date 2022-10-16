struct s-uefi-systable {
  header 24
  firmware-vendor 8
  firmware-revision 8
  console-in-handle 8
  con-in 8
  console-out-handle 8
  con-out 8
  stderr-handle 8
  stderr 8
  runtime-services 8
  boot-services 8
  num-table-entries 8
  config-table 8
}

struct s-uefi-srv-boot {
  header 24
  raise-tpl 8
  restore-tpl 8
  allocate-pages 8
  free-pages 8
  get-memory-map 8
  allocate-pool 8
  free-pool 8
  create-event 8
  set-timer 8
  wait-for-event 8
  signal-event 8
  close-event 8
  check-event 8
  install-protocol-interface 8
  reinstall-protocol-interface 8
  uninstall-protocol-interface 8
  handle-protocol 8
  reserved 8
  register-protocol-notify 8
  locate-handle 8
  locate-device-path 8
  install-config-table 8
  load-image 8
  start-image 8
  exit 8
  unload-image 8
  exit-boot-srv 8
  get-next-monotonic-count 8
  stall 8
  set-watchdog-timer 8
  connect-controller 8
  disconnect-controller 8
  open-protocol 8
  close-protocol 8
  open-protocol-info 8
  protocols-per-handle 8
  locate-handle-buffer 8
  locate-protocol 8
  install-multiple-protocol-interfaces 8
  uninstall-multiple-protocol-interfaces 8
  calculate-crc32 8
  copy-mem 8
  set-mem 8
  create-event-ex 8
}

struct s-uefi-srv-runtime {
  get-time 8
  set-time 8
  get-wakeup-time 8
  set-wakeup-time 8
  set-virt-address-map 8
  convert-pointer 8
  get-var 8
  get-next-var-name 8
  set-var 8
  get-next-hight-monotonic-count 8
  reset-system 8
  update-capsule 8
  query-capsule-caps 8
  query-var-info 8
}

struct s-uefi-memmap-desc {
  type 4
  pad 4
  phys 8
  virt 8
  pages 8
  attr 8
}

struct s-uefi-prot-gop {
  query-mode 8
  set-mode 8
  blt 8
  mode 8
}

struct s-uefi-prot-gop-mode {
  max-mode 4
  mode 4
  info 8
  info-size 8
  fb-base 8
  fb-size 8
}

struct s-uefi-prot-gop-mode-info {
  version 4
  hres 4
  vres 4
  format 8
  rmask 4
  gmask 4
  bmask 4
  _mask 4
  ppl 4
}

val c-uefi-guid-gop
  blob dea94290dc23384a96fb7aded080516a
    to c-uefi-guid-gop
