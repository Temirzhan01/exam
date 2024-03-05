runtime error: invalid memory address or nil pointer dereference
C:/Program Files/Go/src/runtime/panic.go:221 (0x309e1c)
        panicmem: panic(memoryError)
C:/Program Files/Go/src/runtime/signal_windows.go:254 (0x309dec)
        sigpanic: panicmem()
C:/Users/00055864/go/src/halykbpm-git.homebank.kz/business-processes/compra.integration/service/compraService.go:28 (0x91d411)
        (*CompraService).CheckIP: err = s.Repository.SaveRow(row)
