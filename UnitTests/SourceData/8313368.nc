;--------------HALLO-------------------------
; PROGRAM NAME : 8313368
; ORDER        : 83133
; OPER. NAME   : Drilling:191
; OPER. DESCR. : Drilling c_PINHOLE_11_e/WYTACZANIE 24_1
; GEO. OPTION  : 1
; OBRABIARKA   : HURON_EX20_SIM840D
;              : HURON Ex20 - Absolute Mode -
; STEROWANIE   : STOL  A/A1  (0-360)
;                GLOWICA B (+/-100)
; OSIE OBROTOWE: SIM840D
; created      : 2022-10-21 11:45:27 Friday
;-------DODATKOWE DANE-------------------
; PROGRAMISTA : Mariusz Orzechowski
; PARTID      : RSE0302021P0001_-
;----------------------------------------
N100 ;------------------- 
; CUTTER ZIGNOROWANY
N102 G0 X75.000 Y300.000 Z670.000 A=DC(0) B0
N103 MSG(" TLID=970266 TLD=24.05 TLR=0.0 TLA=0.0 TLL=184.0 ")
;NARZ: T970266 TNR=TA970266 ***
N104 D0 M5 M75
N105 M53 ;USUN ZACIAG OS B
N106 SOFT
N107 FNORM
N108 T22 ; 970266
N109 L9006
N110 STOPRE
;POMIAR DLUGOSI BLUMEM
 G90 S1000 M3
 R252=184.0 R253=0.2 R254=12.025 R255=0.0
;L9010
N111 STOPRE
N112 TRAILON(A1,A)
N113 FGROUP(X,Y,Z,A,B)
N114 M21 M23 ;LUZOWANIE OSI B I A
N115 G0 G64 G54 G90 X75.000 A=DC(0) B0
N116 Y300.000
N117 G94 Z670.000 D1 F15000 S926 M03
N118 ;M74
N119 STOPRE
N120 G0 X-102.000 
N121 D1 
N122 ;PI1_D - wlot
N123 TRANS  X=R41 Z=R42
N124 M20 ; blokada osi B wlacz
N125 M22 ; blokada osi A wlacz
N126 M07 ; cental flood
N127 G0 X-102.000 Y-0.000 Z619.500 A=DC(270.000) S926 M03
N128 G0 Y-0.000 Z539.500 
N129 G0 Y-0.000 Z534.500 
N130 G1 Y-0.000 Z457.500 F37 
N131 Y-0.000 Z534.500 
N132 G0 Y-0.000 Z539.500 
N133 G0 Y-0.000 Z619.500 
N135 M09
; points cnt = 2
N136 MSG (" ***** KONIEC PODPROGRAMU ***** ")
N137 MSG (" ***** BRAK ODJAZDU ***** ")
N138 TRANS
;--------------HALLO-------------------------
; PROGRAM NAME : 8313368
; ORDER        : 83133
; OPER. NAME   : Drilling:192
; OPER. DESCR. : Drilling c_PINHOLE_12
; GEO. OPTION  : 1
; OBRABIARKA   : HURON_EX20_SIM840D
;              : HURON Ex20 - Absolute Mode -
; STEROWANIE   : STOL  A/A1  (0-360)
;                GLOWICA B (+/-100)
; OSIE OBROTOWE: SIM840D
; created      : 2022-10-21 11:45:29 Friday
;-------DODATKOWE DANE-------------------
; PROGRAMISTA : Mariusz Orzechowski
; PARTID      : RSE0302021P0001_-
;----------------------------------------
N100 ;------------------- 
N102 ; usunieto zbedne ruchy 
N103 D1 
N104 ;PI1_D - wlot
N105 TRANS  X=R43 Z=R44
N106 M20 ; blokada osi B wlacz
N107 M22 ; blokada osi A wlacz
N108 M07 ; cental flood
N109 G0 X-47.500 Y-0.000 Z595.750 A=DC(270.000) S926 M03
N110 G0 Y-0.000 Z535.750 
N111 G0 Y-0.000 Z530.750 
N112 G1 Y-0.000 Z453.750 F37 
N113 Y-0.000 Z530.750 
N114 G0 Y-0.000 Z535.750 
N115 G0 Y-0.000 Z595.750 
N117 M09
; points cnt = 2
N118 MSG (" ***** KONIEC PODPROGRAMU ***** ")
N119 MSG (" ***** BRAK ODJAZDU ***** ")
N120 TRANS
;--------------HALLO-------------------------
; PROGRAM NAME : 8313368
; ORDER        : 83133
; OPER. NAME   : Drilling:193
; OPER. DESCR. : Drilling c_PINHOLE_11_a
; GEO. OPTION  : 1
; OBRABIARKA   : HURON_EX20_SIM840D
;              : HURON Ex20 - Absolute Mode -
; STEROWANIE   : STOL  A/A1  (0-360)
;                GLOWICA B (+/-100)
; OSIE OBROTOWE: SIM840D
; created      : 2022-10-21 11:45:31 Friday
;-------DODATKOWE DANE-------------------
; PROGRAMISTA : Mariusz Orzechowski
; PARTID      : RSE0302021P0001_-
;----------------------------------------
N100 ;------------------- 
N102 G0 Z680 ; wstawiono odjazd bezpieczenstwa
N103 G0 Y300 ; wstawiono odjazd bezpieczenstwa
N104 MSG(" TLID=970266 TLD=24.05 TLR=0.0 TLA=0.0 TLL=184.0 ")
N105 ;(Grouped operation with T970266)
N106 ;(Continue without toolchange)
N107 G0 G64 G54 G90 A=DC(90.000)
N108 G0 B0
N109 D1 
N110 ;PI1_D - wlot
N111 TRANS  X=R47 Z=R48
N112 M20 ; blokada osi B wlacz
N113 M22 ; blokada osi A wlacz
N114 M07 ; cental flood
N115 G0 X-102.000 Y0.000 Z619.500 A=DC(90.000) S926 M03
N116 G0 Y0.000 Z539.500 
N117 G0 Y0.000 Z534.500 
N118 G1 Y0.000 Z457.500 F37 
N119 Y0.000 Z534.500 
N120 G0 Y0.000 Z539.500 
N121 G0 Y0.000 Z619.500 
N123 M09
; points cnt = 2
N124 MSG (" ***** KONIEC PODPROGRAMU ***** ")
N125 MSG (" ***** BRAK ODJAZDU ***** ")
N126 TRANS
;--------------HALLO-------------------------
; PROGRAM NAME : 8313368
; ORDER        : 83133
; OPER. NAME   : Drilling:194
; OPER. DESCR. : Drilling c_PINHOLE_12A
; GEO. OPTION  : 1
; OBRABIARKA   : HURON_EX20_SIM840D
;              : HURON Ex20 - Absolute Mode -
; STEROWANIE   : STOL  A/A1  (0-360)
;                GLOWICA B (+/-100)
; OSIE OBROTOWE: SIM840D
; created      : 2022-10-21 11:45:32 Friday
;-------DODATKOWE DANE-------------------
; PROGRAMISTA : Mariusz Orzechowski
; PARTID      : RSE0302021P0001_-
;----------------------------------------
N100 ;------------------- 
N102 ; usunieto zbedne ruchy 
N103 D1 
N104 ;PI1_D - wlot
N105 TRANS  X=R49 Z=R50
N106 M20 ; blokada osi B wlacz
N107 M22 ; blokada osi A wlacz
N108 M07 ; cental flood
N109 G0 X-47.500 Y0.000 Z595.750 A=DC(90.000) S926 M03
N110 G0 Y0.000 Z535.750 
N111 G0 Y0.000 Z530.750 
N112 G1 Y0.000 Z453.750 F37 
N113 Y0.000 Z530.750 
N114 G0 Y0.000 Z535.750 
N115 G0 Y0.000 Z595.750 
N117 M09
; points cnt = 2
MSG (" ***** KONIEC PROGRAMU ***** ")
N118 T0 D0 M5 M75
N119 M53 ;USUN ZACIAG OS B
N120 SOFT
N121 FNORM
N122 L9006
N123 STOPRE
N124 G0 G53 G90 D0 Y0 Z0
N125 G53 X0
N126 M30
