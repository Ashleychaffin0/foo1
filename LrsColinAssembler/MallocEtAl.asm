; malloc, strlen, strcat, strcpy
;            $Traceon
            LA              R1,src
            LA              R2,target
;           CALL            R14,strcpy
;           $PSTRING        target
            LA              R1,LLData
            CALL            R14,TestLinkedList
            $STOP
src         DS              Hello
target      DS              abcde            

; strcpy: R1=source address, R2=target address. Stops at first \0 byte
strcpy          ST          R1,strcpyR1
                ST          R2,strcpyR2
                ST          R3,strcpyR3
                SR          R3,R3
strcpyLoop      IC          R3,0[R1]
                STC         R3,0[R2]
                LA          R1,1[R1]
                LA          R2,1[R2]
                LTR         R3,R3
                BNZ         strcpyLoop
strcpyExit      L           R1,strcpyR1
                L           R2,strcpyR2
                L           R3,strcpyR3
                RET         R14
strcpyR1        DI          0
strcpyR2        DI          0
strcpyR3        DI          0 

; Linked list test code
TestLinkedList  ST          R1,LLR1     ; Save register(s)
                
;LLLoop         ;$PSTRING    LLMsg
                ;$PREGHEX    R1
                ;$PSTRING    LLNL
LLLoop          $PSTRING    2[R1]       ; The string is 2 bytes past the pointer
                L           R1,0[R1]    ; Go on to next element in the list
                LTR         R1,R1       ; Test for null/None (0) pointer
                BNZ         LLLoop      ; If not null, more to go
                L           R1,LLR1     ; Restore register(s)
                RET         R14

; ---------           ---------              ---------
; | pNext |     ->    | pNext |      ->      |   0   |
; ------------------  ---------------------  -----------------
; |      Data      |  |       Data        |  |     Data      |
; ------------------  ---------------------  -----------------

; Linked list data
LLData          DI          pNext1
                DS          Hello
pNext1          DI          pNext2
                DS          ,\b
pNext2          DI          pNext3
                DS          world
pNext3          DI          pNext4
                DS          . How are you
pNext4          DI          0
                DS          \btoday?

LLMsg           DS          Processing node at address\b
LLNL            DS          \n

; Save area
LLR1            DI          0

;;;;;;;;;;; Ignore the stuff below, for now

; Linked list format
LLNextPointer     DI  0         ; First 2 bytes is the address of the next entry, or zero
LLDataOffset      DI  2         ; The data itself starts 2 bytes into the entry

; Heap entry structure, in general:
HeapSizeOffset    DI  0         ; First 2 bytes is the size of this entry        
HeapData          DI  2         ; The actual data starts 2 bytes into the entry

HeapNextFree      DI  0         ; Address of current top (or bottom, depending on 
                                ; how you look at it) of the heap. New entries go
                                ; at this address
TheHeap           DI  0         ; The start of the heap

