<CsoundSynthesizer>
<CsOptions>
;RealTime audio
-odac
; Menos verbose => Mejor rendimiento
--m-amps=0
--m-range=0
--m-warnings=0
; MIDI
-Ma -m0
</CsOptions>
<CsInstruments>

sr = 44100
ksmps = 32
nchnls = 2
0dbfs = 1.0

gi_Cos ftgen 0, 0, 2^10, 11, 1  ; Cosine Table

;; TODO: Las tablas NO necesitan ser globales?

;; SUBSTRACTIVE 1 (TABLES)
gi_DurTable1   ftgen 10, 0, 8, -2, 	1, 1, 1, 1, 1, 1, 1, 1   ; Duracion de los pasos (1, 2 ó 4 == Corchea, Negra ó Blanca)
gi_NotesTable1 ftgen 11, 0, 8, -2,	6.05, 6.08, 6.10, 6.08, 7.03, 7.01, 7.03, 7.05  ;; On the run
gi_EnabledTable1 ftgen 12, 0, 8, -2,1, 1, 1, 1, 1, 1, 1, 1  ;  Pasos activos == 1, inactivos == 0
;; ADDITIVE 1 (TABLES)
gi_DurTableAdd1   ftgen 20, 0, 8, -2, 	8, 2, 2, 4, 8, 8, 2, 4   ; Duracion de los pasos (1, 2 ó 4 == Corchea, Negra ó Blanca)
gi_NotesTableAdd1 ftgen 21, 0, 8, -2,	7.01, 7.01, 7.03, 7.01, 7.01, 7.01, 7.05, 7.01
;; SAMPLER 1 (TABLES)
gi_DurTableSmp1    ftgen 30, 0, 8, -2, 	8, 2, 2, 2, 2, 2, 4, 2   ; Duracion de los pasos (1, 2 ó 4 == Corchea, Negra ó Blanca)
gi_SampleTableSmp1 ftgen 31, 0, 8, -2,	1, 2, 3, 3, 2, 1, 2, 1
	; TODO: Crear una tabla binaria para cada sample de la batería. Cada posicion indicará si el sample es disparado o no
	;       Con eso se consigue poder lanzar samples distintos a la vez (ejem: caja + kick)
	; IDEA: ¿La betería debería funcionar siempre con pasos de duración 1?

;; SEQUENCER VARIABLES ;; TODO: ======> Estas variables no necesitan ser globales? <======
	; Substractive 1
gk_Index1 init -1  ; NOTE: -1 en el init para que el primer trigger lo ponga en la primera posición
gk_DurCount1 init 0 
	; Additive 1
gk_IndexAdd1 init -1
gk_DurCountAdd1 init 0
	; Sampler 1
gk_IndexSmp1 init -1
gk_DurCountSmp1 init 0


;; ////////////////////
;; ///     MIDI     ///
;; ////////////////////
massign 0,1
instr 1
	iinstrument = 30
	kstatus, kchan, kdata1, kdata2 midiin
	if (kstatus == 144) then
		printk2 pchmidinn (kdata1) ;; DEBUG
		event "i", iinstrument, 0, 1, pchmidinn (kdata1)
	endif
endin


;; ////////////////////
;; ///  GLOBAL LFO  ///
;; ////////////////////
instr 5
	ifreq = .05
	imode = 1  ; 0 - Sine, 1 - Triangle, 2 - Square (bipolar), 3 - Square (unipolar), 4 - Sawtooth, 5 - Sawtooth (down)
	gk_LFO lfo 1, ifreq, imode
endin

;; Encendemos los intrumentos de apoyo
	turnon 5 ;; LFO Global

;; ////////////////////
;; ///  SEQUENCER   ///
;; ////////////////////
instr 10
	
	
	ibpm = 165 * 2 ; <P>
	idur = (ibpm * 2) / 60 ;; bpm * 2 para cuantificar como corcheas los valores 1 en las tablas de duración
	
	kDuration1 init 0
	kDurAdd1 init 0
	kDurSmp1 init 0
	
	ktimer phasor idur
	ktrg trigger ktimer, .99, 0
	if (ktrg == 1) then
		
		;; SUBSTRACTIVE 1
		if (gk_DurCount1 == kDuration1) then
			gk_Index1 = gk_Index1 + 1
			if gk_Index1 > 7 then
				gk_Index1 = 0
			endif
      outvalue "durIndex_1", gk_Index1
			kDuration1 table gk_Index1, gi_DurTable1, 0, 0, 1
			knote 	  table gk_Index1, gi_NotesTable1, 0, 0, 1
      kactive   table gk_Index1, gi_EnabledTable1, 0, 0, 1
			
      
			kdur = (1/idur)
			kdur = kdur * kDuration1
			;printk2 kdur ;; DEBUG
      if (kactive > 0) then
			  event "i", 20, 0, kdur, knote   ; FIRE: Substractive 1
      endif
      gk_DurCount1 = 0
		endif
		;; ADDITIVE 1
		if (gk_DurCountAdd1 == kDurAdd1) then
			gk_IndexAdd1 = gk_IndexAdd1 + 1
			if gk_IndexAdd1 > 7 then
				gk_IndexAdd1 = 0
			endif
			kDurAdd1 table gk_IndexAdd1, gi_DurTableAdd1, 0, 0, 1
			knote 	  table gk_IndexAdd1, gi_NotesTableAdd1, 0, 0, 1
			
			kdur = (1/idur)
			kdur = kdur * kDurAdd1
			
			kpartials = 33 * (1 - abs(gk_LFO)) ;; TODO: Como se controlará esto? LFO GLobal? ;; NOTE: El 33 crea un sonido tipo "botella soplada"
			event "i", 30, 0, kdur, knote, kpartials   ; FIRE: Additive 1 
			gk_DurCountAdd1 = 0
		endif
		
		;; SAMPLER 1
		if (gk_DurCountSmp1 == kDurSmp1) then
			gk_IndexSmp1 = gk_IndexSmp1 + 1
			if gk_IndexSmp1 > 7 then
				gk_IndexSmp1 = 0
			endif
			kDurSmp1 table gk_IndexSmp1, gi_DurTableSmp1, 0, 0, 1
			ksample 	  table gk_IndexSmp1, gi_SampleTableSmp1, 0, 0, 1
			
			kdur = (1/idur)
			kdur = kdur * kDurSmp1
			
			kdb = 0  ;<P>
			
			event "i", 40, 0, kdur, ksample, kdb   ; FIRE: Sampler 1 
			gk_DurCountSmp1 = 0
		endif
		
		gk_DurCount1 = gk_DurCount1 + 1
		gk_DurCountAdd1 = gk_DurCountAdd1 + 1
		gk_DurCountSmp1 = gk_DurCountSmp1 + 1
		
	endif
	;printk2 ktrg
endin


;; ////////////////////
;; /// SUBSTRACTIVE ///
;; ////////////////////
instr 20
; Envelope VCA
	; TODO: Calcular los valores ADR con respecto a la duración de la nota (p4) para que sumen en total ese valor (p4).
	iatt = .01   ; <P>
	idec = .01    ; <P>
	islev = .8   ; <P>
	irel = .7   ; <P>
	kaenv adsr iatt, idec, islev, irel  ; VCA's ADRS envelope

;LFO1 - pitch
	iltype = 4   ; LFO1 waveform  <P>
	ildepth = 0  ; LFO1 amplitude <P>
	ilfreq = 10  ; LFO1 frequency <P>
	
	klfo lfo ildepth, ilfreq, iltype

; LFO2 - Filter
	iltype2 = 0   ; LFO2 waveform    <P>
	ildepth2 = 400  ; LFO2 amplitude
	ilfreq2 = 5  ; LFO2 frequency   <P>
	
	klfo2 lfo ildepth2, ilfreq2, iltype2

; VCO 1
	ifreq = cpspch(p4) ;         <S>
  	imode = 2    ;               <S>  		;0 - Sawtooth, 2 - pwm sqr, 4 - pwm sawtooth/triangle/ramp, 6 - pulse, 10 - sqr (fast), 12 - triangle (fast)
	iamp = ampdbfs(0)  ; VCO1 amplitude <P>
	kpwm = 0.99 - abs(gk_LFO) ; VCO1 pwm       <P>
	
  	a1 vco2 iamp * kaenv, ifreq + klfo, imode, kpwm, 0, .5

; VCO 2 (sub osc)
	ifreq2 = ifreq / 2 ; VCO2 note value  <Divisor es P>
  	imode2 = 0 ;                <P>        ;0 - Sawtooth, 2 - pwm sqr, 4 - pwm sawtooth/triangle/ramp, 6 - pulse, 10 - sqr (fast), 12 - triangle (fast)
	iamp2 = ampdbfs(-6)  ; VCO2 amplitude <S>
	kpwm2 = 0.01 + abs(gk_LFO); VCO2 pwm       <P>  
	
  	a2 vco2 iamp2 * kaenv, ifreq2 + klfo, imode2, kpwm2, 0, .5 	 	
  	avco sum a1, a2

; NOISE
	inoiseAmp = 0.03  ;;  Range [0, 1]   <P>
	inoiseBeta = -0.99   ;; Range [-1, 1]

	a3 noise inoiseAmp * kaenv, inoiseBeta
	anoise sum avco, a3
	
; VCF 1

	icutoff = 360	 ; 				 <P>
	ires = .8    ; 				 <P>
	kfilterDepth = abs(gk_LFO); range [0, 1] <P>
	iLFODepth = 0  ; range [0, 1] <P>
	
	af1 moogvcf2 anoise, icutoff + (klfo2 * iLFODepth), ires, 32768  ;; TODO: Mirar el Magicnumber bien
	af1 sum ((1-kfilterDepth) * anoise), (af1 * kfilterDepth) ; Establecemos la cantidad de señal filtrada

	out af1, af1
endin

;; ////////////////////
;; ///   ADDITIVE   ///
;; ////////////////////
; TODO: Work In PRogress


instr 30
; Envelope VCA
	; TODO: Calcular los valores ADR con respecto a la duración de la nota (p4) para que sumen en total ese valor (p4).
	iatt invalue "add1_attack" ;iatt = .2 / p4  ; <P>
	idec invalue "add1_decay"  ;idec = 1 / p4  ; <P>
	isus invalue "add1_sustain";islev = .8  ; <P>
	irel invalue "add1_release";irel = 1 / p4 ; <P>
	
	kaenv adsr iatt, idec, isus, irel  ; VCA's ADRS envelope

; Synthesis
	ifreq = cpspch(p4) ;         <S>
	iamp =  ampdbfs(-8)
	iharmnum invalue "add1_harmnum";iharmnum  =  p5; number of harmonics
	klh invalue "add1_lharm"		;klh  =     1          ; lowest harmonic
	kmul invalue "add1_mul"		;kmul =     .333          ; amplitude coefficient multiplier
	
	asig gbuzz kaenv * iamp, ifreq, iharmnum, klh, kmul, gi_Cos
     outs  asig, asig
endin

;; ////////////////////
;; ///   SAMPLER    ///
;; ////////////////////
; TODO: Work In Progress
gi_SampleKick ftgen  1, 0, 0, 1, "samples\kit1\kick.wav",  0, 0, 0
gi_SampleHihat ftgen 2, 0, 0, 1, "samples\kit1\hihat.wav", 0, 0, 0
gi_SampleSnare ftgen 3, 0, 0, 1, "samples\kit1\snare.wav", 0, 0, 0

instr 40

   	iamp = ampdbfs(p5)
	istable = p4
	icps = 1;p6
	ifn = 1;p7
   	ibas = 1;p8

   	a1 loscil iamp, icps, istable, ibas
	out a1, a1
endin

</CsInstruments>
</CsoundSynthesizer>
