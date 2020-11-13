;       $TraceOn
;        CALL            R14,TestIxes
;        CALL            R14,OneToTen
        CALL            R14,DoFib
;        CALL            R14,DoPrimes
        $PSTRING        DoneMsg
        $Stop

; ---------------------------------------------------------
        
TestIxes $PSTRING       MsgTestingIxes
        ST              R1,IxSaveR1
        ST              R2,IxSaveR2
        ST              R3,IxSaveR3
        SR              R1,R1                   ; Index reg
        LA              R2,1                    ; Increment
        SR              R3,R3                   ; Zero R3 for IC
IxLoop  IC              R3,MsgTestingIxes[R1]   ; Get next char
        LTR             R3,R3                   ; Hit end of string?
        BZ              IxRet
        CMP             R3,LC_a
        BL              IxNext                  ; Don't modify
        CMP             R3,LC_z
        BH              IxNext
        SUB             R3,Hex20                ; Convert to lower case
IxNext  STC             R3,IxTarget[R1]
        AR              R1,R2                   ; Increment index reg
        B               IxLoop
IxRet   $PSTRING        IxTarget
        L               R1,IxSaveR1
        L               R2,IxSaveR2
        L               R3,IxSaveR3
        RET             R14
IxSaveR1        DI      0        
IxSaveR2        DI      0
IxSaveR3        DI      0
Hex20           DI      32              ; 0x20 -- Convert L/C to U/C
LC_a            DI      97              ; 0x61 -- 'a' -- Lower Case a
LC_z            DI      122             ; 0x7a -- 'z' -- Lower Case z
Ix1             DI      1               ; Increment
MsgTestingIxes  DS      TextIxes: Converting lower-case to UPPER-CASE\n        
IxTarget        DS      TextIxes: Converting lower-case to UPPER-CASE\n

; ---------------------------------------------------------

OneToTen $PSTRING       Msg
        LA              R2,1		; Starting value
        LA              R3,1		; Increment
Loop    CMP             R2,Ten		; Stopping value
        BLE             Print
        $PSTRING        NL
        RET             R14
Print   $PSTRING        SEP
        $PREG           R2
        AR              R2,R3
        B               Loop
Ten     DI              10
Msg     DS              The numbers from 1 to 10 are:\b
Space   DS              \b
NL      DS              \n

; ---------------------------------------------------------

DoFib   $PSTRING        FibMsg
        ST              R14,FibR14      ; Save our return address
        LA              R5,15           ; Get first 15 Fib#s after 1, 1
        LA              R1,1            ; Most recent Fib# 
        LA              R2,1            ; Earlier Fib#
        LA              R7,NULL
NextFib LR              R6,R1           ; Temp = Most recent
        AR              R6,R2           ; Add second most recent
        LR              R2,R1           ; New second most recent = old most recent
        LR              R1,R6           ; New most recent = Temp
        CALL            R14,PrintNum    ; Print that puppy
        SUB             R5,One
        BNZ             NextFib 
        $PSTRING        NL
        L               R14,FibR14
        RET             R14

FibMsg  DS              Fibonacci numbers...: 1\c 1  
NULL    DS              "\0"
SEP     DS              "\c\b"            ; ", "
One     DI              1
FibR14  DI              0

PrintNum $PSTRING       0[R7]
        LA              R7,SEP
        $PREGHEX        R1
        RET             R14 

; ---------------------------------------------------------

DoPrimes $PSTRING       PrimeMsg
; Save registers
        ST              R2,PrimeR2
        ST              R3,PrimeR3
        ST              R5,PrimeR5
        ST              R14,PrimeR14
        LA              R2,1            ; First potential prime
        LA              R3,13           ; # of primes to find
;        $Traceon
NextPrime SUB           R3,One
        BZ              PrimeReturn
        ADD             R2,Two
        LR              R5,R2           
        DIV             R5,Two 
        MUL             R5,Two
        CR              R5,R2           ; Compare Num/Div * Div to Num
        BE              NextPrime       ; If equal, found a divisor       
; Do prime check
        LR              R1,R2
        CALL            R14,PrintNum
        ADD             R2,Two          ; Next odd number
        SUB             R3,One
        BNZ             NextPrime
        $PSTRING        NL
        $Traceoff
; Restore registers before returning  
PrimeReturn $PSTRING    NL
        L               R5,PrimeR5
        L               R3,PrimeR3
        L               R2,PrimeR2
        L               R14,PrimeR14
        RET             R14               

PrimeMsg  DS            "Primes...: 2"
Two     DI              2
PrimeR2 DI              0
PrimeR3 DI              0
PrimeR5 DI              0
PrimeR14 DI             0

DoneMsg	DS		Finished\n
